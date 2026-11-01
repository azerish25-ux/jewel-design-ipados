// Refine static views using imported MSVC signatures and explicit ABI data shapes.
//@category CounterSketch.CameraAnalysis
import ghidra.app.script.GhidraScript;
import ghidra.app.cmd.disassemble.DisassembleCommand;
import ghidra.app.decompiler.*;
import ghidra.app.util.demangler.*;
import ghidra.program.model.address.*;
import ghidra.program.model.data.*;
import ghidra.program.model.listing.*;
import ghidra.program.model.symbol.*;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.*;
import com.google.gson.*;

public class RefineNativeCamera extends GhidraScript {
    protected String hash(byte[] bytes)throws Exception{return HexFormat.of().formatHex(MessageDigest.getInstance("SHA-256").digest(bytes));}
    protected Address address(long rva){return currentProgram.getImageBase().add(rva);}
    protected Function region(JsonObject spec)throws Exception{
        long rva=spec.get("rva").getAsLong();int length=spec.get("length").getAsInt();
        if(length<1||length>65536)throw new Exception("Unbounded native region");
        Address start=address(rva),end=start.add(length-1);byte[] bytes=new byte[length];
        if(currentProgram.getMemory().getBytes(start,bytes)!=length||!hash(bytes).equals(spec.get("sha256").getAsString()))throw new Exception("Original region bytes differ");
        AddressSet body=new AddressSet(start,end);DisassembleCommand command=new DisassembleCommand(start,body,true);
        if(!command.applyTo(currentProgram,monitor))throw new Exception("Region disassembly failed");
        // A defined primary entry does not cause Ghidra to redisassemble new chained ranges.
        if(spec.has("unwindRegions"))for(JsonElement element:spec.getAsJsonArray("unwindRegions")){
            long chainedRva=element.getAsJsonObject().get("begin").getAsLong();Address chained=address(chainedRva);
            if(!body.contains(chained))throw new Exception("Chained start leaves verified function bytes");
            if(!new DisassembleCommand(chained,body,true).applyTo(currentProgram,monitor))throw new Exception("Chained region disassembly failed");
        }
        Function function=currentProgram.getFunctionManager().getFunctionAt(start);
        if(function==null)function=currentProgram.getFunctionManager().createFunction("Original_"+spec.get("role").getAsString().replace('-','_'),start,body,SourceType.ANALYSIS);
        else function.setBody(body);
        return function;
    }
    protected Function external(Address at){
        Function direct=currentProgram.getFunctionManager().getFunctionAt(at);if(direct!=null&&direct.isExternal())return direct;
        for(Reference reference:currentProgram.getReferenceManager().getReferencesFrom(at)){
            // Called PE import slots can be labels until analysis creates function symbols.
            if(reference instanceof ExternalReference){
                ExternalLocation location=((ExternalReference)reference).getExternalLocation();
                return location.isFunction()?location.getFunction():location.createFunction();
            }
            Function function=currentProgram.getFunctionManager().getFunctionAt(reference.getToAddress());
            if(function!=null&&function.isExternal())return function;
        }
        return null;
    }
    protected void shape(String name,int doubles)throws Exception{
        DataTypeManager manager=currentProgram.getDataTypeManager();List<DataType> matches=new ArrayList<>();manager.findDataTypes(name,matches);
        if(matches.isEmpty()){StructureDataType created=new StructureDataType(CategoryPath.ROOT,name,0,manager);manager.addDataType(created,DataTypeConflictHandler.REPLACE_HANDLER);manager.findDataTypes(name,matches);}
        for(DataType type:matches)if(type instanceof Structure){Structure structure=(Structure)type;structure.deleteAll();for(int i=0;i<doubles;i++)structure.add(DoubleDataType.dataType,doubles==3?new String[]{"x","y","z"}[i]:"m"+i,null);}
    }
    protected Map<String,Object> demangle(Function function,String mangled)throws Exception{
        Map<String,Object> item=new LinkedHashMap<>();item.put("mangled",mangled);item.put("address",function.getEntryPoint().toString());
        try{
            DemangledObject object=DemanglerUtil.demangle(currentProgram,mangled);
            if(object==null)throw new Exception("No MSVC demangle result");
            DemanglerOptions options=new DemanglerOptions();options.setApplySignature(true);options.setApplyCallingConvention(true);options.setDoDisassembly(false);
            item.put("applied",object.applyTo(currentProgram,function.getEntryPoint(),options,monitor));
            item.put("decoded",object.toString());
        }catch(Exception error){item.put("applied",false);item.put("error",error.toString());}
        return item;
    }
    protected void refresh(Function function)throws Exception{
        List<Variable> parameters=new ArrayList<>();
        for(Parameter parameter:function.getParameters())if(!parameter.isAutoParameter())parameters.add(new ParameterImpl(parameter.getName(),parameter.getFormalDataType(),currentProgram));
        function.updateFunction(function.getCallingConventionName(),new ReturnParameterImpl(function.getReturn().getFormalDataType(),currentProgram),parameters,
            Function.FunctionUpdateType.DYNAMIC_STORAGE_FORMAL_PARAMS,true,SourceType.ANALYSIS);
    }
    @Override public void run()throws Exception{
        String[] args=getScriptArgs();if(args.length!=3)throw new Exception("Expected manifest, output directory and repository root");
        Path root=Paths.get(args[2]).toRealPath(),manifest=Paths.get(args[0]).toAbsolutePath().normalize(),out=Paths.get(args[1]).toAbsolutePath().normalize();
        if(!root.equals(Paths.get(System.getProperty("user.dir")).toRealPath()))throw new Exception("Repository root differs from launcher workspace");
        if(System.getProperty("os.name").toLowerCase(Locale.ROOT).contains("windows")&&!root.toString().toLowerCase(Locale.ROOT).startsWith("d:"))throw new Exception("Windows analysis root must stay on D:");
        if(!manifest.startsWith(root)||!out.startsWith(root))throw new Exception("Native refinement path leaves workspace");
        Files.createDirectories(out);out=out.toRealPath();if(!out.startsWith(root))throw new Exception("Resolved output leaves workspace");
        JsonObject spec=JsonParser.parseString(Files.readString(manifest,StandardCharsets.UTF_8)).getAsJsonObject();
        if(!spec.get("targetSha256").getAsString().equalsIgnoreCase(currentProgram.getExecutableSHA256()))throw new Exception("Refinement target SHA differs");
        Map<String,Object> report=new LinkedHashMap<>();report.put("schemaVersion",1);report.put("targetSha256",currentProgram.getExecutableSHA256());report.put("targetExecuted",false);
        List<Map<String,Object>> signatures=new ArrayList<>(),views=new ArrayList<>();report.put("importedSignatures",signatures);report.put("views",views);
        DecompInterface engine=null;int transaction=currentProgram.startTransaction("Refine known camera call signatures only");
        try{
            List<Function> functions=new ArrayList<>();Map<Address,Function> boundaries=new LinkedHashMap<>();
            for(JsonElement element:spec.getAsJsonArray("functions"))functions.add(region(element.getAsJsonObject()));
            for(Function function:functions){
                InstructionIterator instructions=currentProgram.getListing().getInstructions(function.getBody(),true);
                while(instructions.hasNext()){Instruction instruction=instructions.next();if(instruction.getFlowType().isCall())for(Reference reference:instruction.getReferencesFrom()){
                    Function target=external(reference.getToAddress());if(target!=null)boundaries.put(target.getEntryPoint(),target);
                }}
            }
            shape("ON_3dPoint",3);shape("ON_3dVector",3);shape("ON_Xform",16);
            for(Function function:boundaries.values())if(function.getName().startsWith("?"))signatures.add(demangle(function,function.getName()));
            // The root's wrapper-derived ABI annotation is retained; demangle the commit helper's real export.
            signatures.add(demangle(functions.get(1),spec.getAsJsonArray("functions").get(1).getAsJsonObject().get("name").getAsString()));
            shape("ON_3dPoint",3);shape("ON_3dVector",3);shape("ON_Xform",16);
            for(Function function:boundaries.values())if(!function.getName().startsWith("?"))refresh(function);
            refresh(functions.get(1));
            List<Map<String,Object>> finalSignatures=new ArrayList<>();
            for(Function function:boundaries.values()){
                Map<String,Object> item=new LinkedHashMap<>();item.put("name",function.getName(true));item.put("signature",function.getPrototypeString(true,false));
                item.put("formalReturnType",function.getReturn().getFormalDataType().getName());item.put("formalReturnBytes",function.getReturn().getFormalDataType().getLength());
                List<String> params=new ArrayList<>();int formalCount=0;
                for(Parameter parameter:function.getParameters()){
                    if(!parameter.isAutoParameter())formalCount++;
                    params.add(parameter.getName()+":"+parameter.getFormalDataType().getName()+":"+parameter.getVariableStorage()+":auto="+parameter.isAutoParameter());
                }
                item.put("formalParameterCount",formalCount);item.put("parameters",params);finalSignatures.add(item);
                String name=function.getName();
                if(name.equals("GetFrustum")&&formalCount!=6)throw new Exception("Frustum getter lost a formal argument");
                if(name.equals("IsTiny")&&formalCount!=1)throw new Exception("Tiny predicate lost tolerance argument");
                if((name.equals("CameraX")||name.equals("CameraY")||name.equals("CameraZ"))&&function.getReturn().getFormalDataType().getLength()!=24)throw new Exception("Vector getter lost24-byte formal return");
            }
            report.put("finalImportedSignatures",finalSignatures);
            int commitFormals=0;for(Parameter parameter:functions.get(1).getParameters())if(!parameter.isAutoParameter())commitFormals++;
            if(commitFormals!=2)throw new Exception("Viewport commit lost source viewport argument");
            JsonObject scalar=spec.getAsJsonObject("scalarThunk");Address thunkAddress=address(scalar.get("rva").getAsLong());
            byte[] thunkBytes=new byte[6];currentProgram.getMemory().getBytes(thunkAddress,thunkBytes);
            Address iat=thunkAddress.add(6+currentProgram.getMemory().getInt(thunkAddress.add(2)));
            if(thunkBytes[0]!=(byte)0xff||thunkBytes[1]!=(byte)0x25||!iat.equals(address(scalar.get("iatRva").getAsLong())))throw new Exception("Scalar import thunk bytes differ");
            Function scalarImport=external(iat);
            if(scalarImport==null||!scalarImport.getName().equals(scalar.get("expectedImport").getAsString()))throw new Exception("Scalar thunk does not resolve to expected acos import");
            scalarImport.updateFunction(currentProgram.getCompilerSpec().getDefaultCallingConvention().getName(),new ReturnParameterImpl(DoubleDataType.dataType,currentProgram),
                Function.FunctionUpdateType.DYNAMIC_STORAGE_FORMAL_PARAMS,true,SourceType.USER_DEFINED,new ParameterImpl("value",DoubleDataType.dataType,currentProgram));
            new DisassembleCommand(thunkAddress,new AddressSet(thunkAddress,thunkAddress.add(5)),true).applyTo(currentProgram,monitor);
            Function thunk=currentProgram.getFunctionManager().getFunctionAt(thunkAddress);
            if(thunk==null)thunk=currentProgram.getFunctionManager().createFunction("Original_acos_thunk",thunkAddress,new AddressSet(thunkAddress,thunkAddress.add(5)),SourceType.ANALYSIS);
            thunk.setThunkedFunction(scalarImport);report.put("scalarThunkImport",scalarImport.getName());report.put("scalarThunkBytesSha256",hash(thunkBytes));
            engine=new DecompInterface();engine.toggleCCode(true);engine.toggleSyntaxTree(true);if(!engine.openProgram(currentProgram))throw new Exception("Refinement decompiler open failed");
            for(int index=0;index<functions.size();index++){
                Function function=functions.get(index);DecompileResults result=engine.decompileFunction(function,90,monitor);
                if(!result.decompileCompleted())throw new Exception("Typed decompile failed "+result.getErrorMessage());
                String code=result.getDecompiledFunction().getC(),role=spec.getAsJsonArray("functions").get(index).getAsJsonObject().get("role").getAsString();
                Files.writeString(out.resolve(role+".c"),code,StandardCharsets.UTF_8);
                Map<String,Object> view=new LinkedHashMap<>();view.put("role",role);view.put("entry",function.getEntryPoint().toString());view.put("signature",function.getPrototypeString(true,false));view.put("decompilerSha256",hash(code.getBytes(StandardCharsets.UTF_8)));
                List<Map<String,Object>> blocks=new ArrayList<>();for(ghidra.program.model.pcode.PcodeBlockBasic block:result.getHighFunction().getBasicBlocks()){
                    Map<String,Object> item=new LinkedHashMap<>();item.put("start",block.getStart().toString());item.put("stop",block.getStop().toString());blocks.add(item);
                }view.put("decompilerBasicBlocks",blocks);views.add(view);
            }
            List<Map<String,Object>> constants=new ArrayList<>();Set<Address> seen=new HashSet<>();
            for(Function function:functions){InstructionIterator instructions=currentProgram.getListing().getInstructions(function.getBody(),true);while(instructions.hasNext()){
                Instruction instruction=instructions.next();if(instruction.getFlowType().isCall())continue;
                for(Reference reference:instruction.getReferencesFrom())if(reference.getReferenceType().isRead()){
                    Address at=reference.getToAddress();if(!seen.add(at)||!currentProgram.getMemory().contains(at))continue;
                    byte[] bytes=new byte[8];if(currentProgram.getMemory().getBytes(at,bytes)!=8)continue;
                    Map<String,Object> item=new LinkedHashMap<>();item.put("address",at.toString());item.put("instruction",instruction.toString());item.put("bytes",HexFormat.of().formatHex(bytes));
                    item.put("rawDoubleInterpretation",Double.toString(Double.longBitsToDouble(currentProgram.getMemory().getLong(at))));constants.add(item);
                }
            }}
            report.put("rawReadConstants",constants);report.put("status","typed_static_views_pending_semantic_review");
            report.put("annotationScope","MSVC names plus explicit known point/vector24-byte and transform128-byte data shapes. Decompiler views remain analysis hypotheses checked against original instructions; no target bytes patched.");
        }catch(Exception error){report.put("status","failed");report.put("error",error.toString());throw error;}
        finally{if(engine!=null)engine.dispose();currentProgram.endTransaction(transaction,true);Files.writeString(out.resolve("analysis.json"),new GsonBuilder().setPrettyPrinting().disableHtmlEscaping().create().toJson(report)+"\n",StandardCharsets.UTF_8);}
        println("STATIC_NATIVE_CAMERA_TYPES_COMPLETE "+out);
    }
}
