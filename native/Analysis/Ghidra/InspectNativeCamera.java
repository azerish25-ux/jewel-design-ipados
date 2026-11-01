// Static original-dependency analysis only. No target code is executed.
//@category CounterSketch.CameraAnalysis
import ghidra.app.script.GhidraScript;
import ghidra.app.cmd.disassemble.DisassembleCommand;
import ghidra.app.decompiler.*;
import ghidra.program.model.address.*;
import ghidra.program.model.data.*;
import ghidra.program.model.listing.*;
import ghidra.program.model.symbol.*;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.*;
import com.google.gson.GsonBuilder;

public class InspectNativeCamera extends GhidraScript {
    private String hash(byte[] bytes) throws Exception {
        return HexFormat.of().formatHex(MessageDigest.getInstance("SHA-256").digest(bytes));
    }
    private void text(Path path,String content) throws Exception {
        Files.writeString(path,content,StandardCharsets.UTF_8);
    }
    private String decompile(DecompInterface engine,Function function) throws Exception {
        DecompileResults result=engine.decompileFunction(function,90,monitor);
        if(!result.decompileCompleted())throw new Exception("Native decompilation incomplete: "+result.getErrorMessage());
        String code=result.getDecompiledFunction().getC();
        if(code==null||code.isEmpty())throw new Exception("Native decompiler returned no C");
        return code;
    }
    @Override public void run() throws Exception {
        String[] args=getScriptArgs();
        if(args.length!=6)throw new Exception("Expected target SHA, RVA, length, output folder, repository root and function SHA");
        Path root=Paths.get(args[4]).toRealPath(),out=Paths.get(args[3]).toAbsolutePath().normalize();
        if(!root.equals(Paths.get(System.getProperty("user.dir")).toRealPath()))throw new Exception("Repository root differs from launcher workspace");
        if(System.getProperty("os.name").toLowerCase(Locale.ROOT).contains("windows")&&!root.toString().toLowerCase(Locale.ROOT).startsWith("d:"))throw new Exception("Windows analysis root must stay on D:");
        if(!out.startsWith(root))throw new Exception("Analysis output leaves workspace");
        Files.createDirectories(out);out=out.toRealPath();
        if(!out.startsWith(root))throw new Exception("Resolved output leaves workspace");
        Map<String,Object> report=new LinkedHashMap<>();
        report.put("schemaVersion",1);report.put("targetSha256",currentProgram.getExecutableSHA256());
        report.put("executablePath",currentProgram.getExecutablePath());
        report.put("language",currentProgram.getLanguageID().toString());
        report.put("compilerSpec",currentProgram.getCompilerSpec().getCompilerSpecID().toString());
        report.put("imageBase",currentProgram.getImageBase().toString());
        report.put("targetExecuted",false);
        DecompInterface engine=null;
        int transaction=currentProgram.startTransaction("Inspect original camera setter only");
        try {
            if(!args[0].equalsIgnoreCase(currentProgram.getExecutableSHA256()))throw new Exception("Imported target SHA differs");
            long rva=Long.decode(args[1]);int length=Integer.decode(args[2]);
            if(length<1||length>65536)throw new Exception("Unbounded function length");
            Address start=currentProgram.getImageBase().add(rva),end=start.add(length-1);
            byte[] bytes=new byte[length];
            if(currentProgram.getMemory().getBytes(start,bytes)!=length)throw new Exception("Function memory incomplete");
            String functionHash=hash(bytes);
            if(!args[5].equalsIgnoreCase(functionHash))throw new Exception("Loaded function bytes differ from original PE region");
            report.put("entry",start.toString());report.put("mainUnwindRegionEndInclusive",end.toString());
            report.put("mainUnwindRegionBytes",length);report.put("functionBytesSha256",functionHash);
            List<String> names=new ArrayList<>();
            for(Symbol symbol:currentProgram.getSymbolTable().getSymbols(start))names.add(symbol.getName(true));
            report.put("originalEntrySymbols",names);
            AddressSet region=new AddressSet(start,end);
            DisassembleCommand command=new DisassembleCommand(start,region,true);
            if(!command.applyTo(currentProgram,monitor))throw new Exception("Disassembly failed: "+command.getStatusMsg());
            Function function=currentProgram.getFunctionManager().getFunctionAt(start);
            if(function==null)function=currentProgram.getFunctionManager().createFunction("OriginalRhino5_SetTargetAndCameraLocation",start,region,SourceType.ANALYSIS);
            else function.setBody(region);
            StringBuilder listing=new StringBuilder();List<Map<String,Object>> calls=new ArrayList<>();int count=0;
            InstructionIterator instructions=currentProgram.getListing().getInstructions(region,true);
            while(instructions.hasNext()){
                monitor.checkCancelled();Instruction instruction=instructions.next();count++;
                listing.append(instruction.getAddress()).append(" ").append(HexFormat.of().formatHex(instruction.getBytes())).append(" ").append(instruction).append("\n");
                if(instruction.getFlowType().isCall()){
                    Map<String,Object> call=new LinkedHashMap<>();call.put("address",instruction.getAddress().toString());call.put("instruction",instruction.toString());
                    List<Map<String,Object>> references=new ArrayList<>();
                    for(Reference reference:instruction.getReferencesFrom()){
                        Map<String,Object> item=new LinkedHashMap<>();Address to=reference.getToAddress();
                        item.put("to",to.toString());item.put("type",reference.getReferenceType().toString());
                        Symbol symbol=currentProgram.getSymbolTable().getPrimarySymbol(to);
                        item.put("symbol",symbol==null?null:symbol.getName(true));references.add(item);
                    }
                    call.put("references",references);calls.add(call);
                }
            }
            text(out.resolve("instructions.txt"),listing.toString());
            report.put("instructionCount",count);report.put("calls",calls);
            engine=new DecompInterface();engine.toggleCCode(true);engine.toggleSyntaxTree(true);
            if(!engine.openProgram(currentProgram))throw new Exception("Decompiler could not open imported program");
            String raw=decompile(engine,function);text(out.resolve("raw-decompiler.c"),raw);
            report.put("rawDecompilerSha256",hash(raw.getBytes(StandardCharsets.UTF_8)));
            // ABI view derived from the managed/native wrapper and Win64 registers.
            // Original C++ Point3d values use hidden pointers; no target bytes are patched.
            DataType pointer=new PointerDataType(VoidDataType.dataType,currentProgram.getDataTypeManager());
            DataType pointPointer=new PointerDataType(DoubleDataType.dataType,currentProgram.getDataTypeManager());
            function.setReturnType(VoidDataType.dataType,SourceType.USER_DEFINED);
            function.updateFunction("__fastcall",null,Function.FunctionUpdateType.DYNAMIC_STORAGE_ALL_PARAMS,true,SourceType.USER_DEFINED,
                new ParameterImpl("viewport",pointer,currentProgram),new ParameterImpl("targetValue",pointPointer,currentProgram),
                new ParameterImpl("cameraValue",pointPointer,currentProgram),new ParameterImpl("mode",IntegerDataType.dataType,currentProgram));
            engine.flushCache();String annotated=decompile(engine,function);text(out.resolve("abi-annotated-decompiler.c"),annotated);
            report.put("annotatedDecompilerSha256",hash(annotated.getBytes(StandardCharsets.UTF_8)));
            report.put("abiAnnotation","Known Win64 wrapper call: RCX viewport, RDX target value pointer, R8 camera value pointer, R9D int mode. Native wrapper supplies mode1. This is an annotated decompiler view, not recovered original C++ types.");
            List<String> storage=new ArrayList<>();for(Parameter parameter:function.getParameters())storage.add(parameter.getName()+":"+parameter.getVariableStorage());
            report.put("annotatedParameterStorage",storage);report.put("status","decompiled_static_views_pending_semantic_review");
            report.put("scope","Original target SHA/function bytes verified. Main unwind region only; called routines and possible separate funclets require follow-up. No target execution or original-PC/runtime parity claim.");
        }
        catch(Exception error){report.put("status","failed");report.put("error",error.toString());throw error;}
        finally {
            if(engine!=null)engine.dispose();
            currentProgram.endTransaction(transaction,true);
            text(out.resolve("analysis.json"),new GsonBuilder().setPrettyPrinting().disableHtmlEscaping().create().toJson(report)+"\n");
        }
        println("STATIC_NATIVE_CAMERA_COMPLETE "+out);
    }
}
