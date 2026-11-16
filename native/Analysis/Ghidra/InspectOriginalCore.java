// Static hash-locked original core comparison; never original DLL execution.
//@category CounterSketch.CameraAnalysis
import ghidra.app.decompiler.*;
import ghidra.program.model.address.*;
import ghidra.program.model.data.*;
import ghidra.program.model.listing.*;
import ghidra.program.model.symbol.*;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import com.google.gson.*;
public class InspectOriginalCore extends RefineNativeCamera {
 private DataType observedType(String name)throws Exception{
  if(name.endsWith("*"))return new PointerDataType(observedType(name.substring(0,name.length()-1)),currentProgram.getDataTypeManager());
  switch(name){case "void":return VoidDataType.dataType;case "int":return IntegerDataType.dataType;case "uint":return UnsignedIntegerDataType.dataType;case "byte":return UnsignedCharDataType.dataType;case "float":return FloatDataType.dataType;case "double":return DoubleDataType.dataType;default:throw new Exception("Unknown explicit external ABI type "+name);}
 }
 @Override public void run()throws Exception{
  String[] args=getScriptArgs();if(args.length!=3)throw new Exception("Expected manifest, output folder and root");
  Path root=Paths.get(args[2]).toRealPath(),out=Paths.get(args[1]).toAbsolutePath().normalize();
  if(!root.equals(Paths.get(System.getProperty("user.dir")).toRealPath())||!out.startsWith(root))throw new Exception("Core analysis leaves launcher workspace");
  if(System.getProperty("os.name").toLowerCase(Locale.ROOT).contains("windows")&&!root.toString().toLowerCase(Locale.ROOT).startsWith("d:"))throw new Exception("Core analysis must stay on D:");
  JsonObject manifest=JsonParser.parseString(Files.readString(Paths.get(args[0]),StandardCharsets.UTF_8)).getAsJsonObject();
  if(!manifest.get("targetSha256").getAsString().equalsIgnoreCase(currentProgram.getExecutableSHA256()))throw new Exception("Original core SHA differs");
  Files.createDirectories(out);out=out.toRealPath();if(!out.startsWith(root))throw new Exception("Resolved output leaves workspace");
  Map<String,Object> report=new LinkedHashMap<>();report.put("targetSha256",currentProgram.getExecutableSHA256());report.put("targetExecuted",false);
  List<Map<String,Object>> signatures=new ArrayList<>(),views=new ArrayList<>();report.put("signatures",signatures);report.put("views",views);
  int transaction=currentProgram.startTransaction("Inspect original core mathematical contracts");DecompInterface engine=null;
  try{
   List<Function> functions=new ArrayList<>();Map<Address,Function> called=new LinkedHashMap<>();
   for(JsonElement element:manifest.getAsJsonArray("functions"))functions.add(region(element.getAsJsonObject()));
   shape("ON_3dPoint",3);shape("ON_3dVector",3);shape("ON_Xform",16);
   for(int i=0;i<functions.size();i++){
    JsonObject spec=manifest.getAsJsonArray("functions").get(i).getAsJsonObject();Function function=functions.get(i);
    if(spec.has("name")&&!spec.get("name").isJsonNull())signatures.add(demangle(function,spec.get("name").getAsString()));
    else {
     String abi=spec.get("abi").getAsString();List<Variable> parameters=new ArrayList<>();
     if(abi.equals("textureMaximum")){
     }else if(abi.equals("textureDimensions")){
      parameters.add(new ParameterImpl("dib",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("memoryFraction",observedType("float"),currentProgram));parameters.add(new ParameterImpl("powerOfTwo",BooleanDataType.dataType,currentProgram));parameters.add(new ParameterImpl("width",observedType("int*"),currentProgram));parameters.add(new ParameterImpl("height",observedType("int*"),currentProgram));
     }else if(abi.equals("alphaRepair")){for(String name:new String[]{"source","destination"})parameters.add(new ParameterImpl(name,observedType("void*"),currentProgram));
     }else if(abi.equals("textureUpload")){
      parameters.add(new ParameterImpl("texture",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("dib",observedType("void*"),currentProgram));
     }else if(abi.equals("textureEntryFind")){
      for(String name:new String[]{"manager","filename","context"})parameters.add(new ParameterImpl(name,observedType("void*"),currentProgram));parameters.add(new ParameterImpl("kind",observedType("uint"),currentProgram));
     }else if(abi.equals("textureCreate")){
      parameters.add(new ParameterImpl("filename",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("gray",BooleanDataType.dataType,currentProgram));parameters.add(new ParameterImpl("option",observedType("uint"),currentProgram));parameters.add(new ParameterImpl("context",observedType("void*"),currentProgram));
     }else if(abi.equals("bitmapCacheFind")){
      parameters.add(new ParameterImpl("manager",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("filename",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("gray",BooleanDataType.dataType,currentProgram));
     }else if(abi.equals("textureLookup")){
      parameters.add(new ParameterImpl("manager",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("filename",observedType("void*"),currentProgram));parameters.add(new ParameterImpl("gray",BooleanDataType.dataType,currentProgram));parameters.add(new ParameterImpl("context",observedType("void*"),currentProgram));
     }else if(abi.equals("gdiTraceLookup")){
      for(String name:new String[]{"manager","trace","engine"})parameters.add(new ParameterImpl(name,observedType("void*"),currentProgram));
     }else if(abi.equals("textureBind")){parameters.add(new ParameterImpl("texture",observedType("void*"),currentProgram));
     }else if(abi.equals("viewValidation")||abi.equals("viewportIdRestore")){
      parameters.add(new ParameterImpl("view",new PointerDataType(VoidDataType.dataType,currentProgram.getDataTypeManager()),currentProgram));
      if(abi.equals("viewportIdRestore"))parameters.add(new ParameterImpl("oldViewportId",new PointerDataType(VoidDataType.dataType,currentProgram.getDataTypeManager()),currentProgram));
     }else if(abi.equals("viewMerge")){
      for(String name:new String[]{"savedView","currentView","resultView"})parameters.add(new ParameterImpl(name,new PointerDataType(VoidDataType.dataType,currentProgram.getDataTypeManager()),currentProgram));
     }else if(!abi.equals("voidNoArgs")){
     parameters.add(new ParameterImpl("viewport",new PointerDataType(VoidDataType.dataType,currentProgram.getDataTypeManager()),currentProgram));
     parameters.add(new ParameterImpl("targetDistance",DoubleDataType.dataType,currentProgram));
     if(abi.equals("perspectiveHelper"))parameters.add(new ParameterImpl("lens",DoubleDataType.dataType,currentProgram));
     else if(!abi.equals("targetUpdate"))throw new Exception("Unknown observed helper ABI");
     }
     DataType result=abi.equals("textureMaximum")?observedType("uint"):(abi.equals("textureLookup")||abi.equals("gdiTraceLookup")||abi.equals("textureEntryFind")||abi.equals("textureCreate")||abi.equals("bitmapCacheFind"))?observedType("void*"):(abi.equals("textureDimensions")||abi.equals("alphaRepair")||abi.equals("textureBind")||abi.equals("targetUpdate")||abi.equals("voidNoArgs")||abi.equals("viewMerge")||abi.equals("viewportIdRestore"))?VoidDataType.dataType:BooleanDataType.dataType;
     function.updateFunction("__fastcall",new ReturnParameterImpl(result,currentProgram),parameters,Function.FunctionUpdateType.DYNAMIC_STORAGE_FORMAL_PARAMS,true,SourceType.USER_DEFINED);
    }
   }
   for(Function function:functions){InstructionIterator iterator=currentProgram.getListing().getInstructions(function.getBody(),true);while(iterator.hasNext()){
    Instruction instruction=iterator.next();if(!instruction.getFlowType().isCall())continue;
    for(Reference reference:instruction.getReferencesFrom()){
     Function target=external(reference.getToAddress());
     if(target==null&&reference.getReferenceType().isCall()&&currentProgram.getMemory().contains(reference.getToAddress())){
      target=currentProgram.getFunctionManager().getFunctionAt(reference.getToAddress());
      if(target==null){Symbol symbol=currentProgram.getSymbolTable().getPrimarySymbol(reference.getToAddress());if(symbol!=null&&symbol.getName().startsWith("?"))target=currentProgram.getFunctionManager().createFunction(symbol.getName(),reference.getToAddress(),new AddressSet(reference.getToAddress()),SourceType.IMPORTED);}
     }
     if(target!=null)called.put(target.getEntryPoint(),target);
    }
   }}
   for(Function function:called.values())if(function.getName().startsWith("?"))signatures.add(demangle(function,function.getName()));
   shape("ON_3dPoint",3);shape("ON_3dVector",3);shape("ON_Xform",16);
   Set<Function> all=new LinkedHashSet<>(called.values());all.addAll(functions);for(Function function:all)refresh(function);
   if(manifest.has("externalSignatures"))for(JsonElement element:manifest.getAsJsonArray("externalSignatures")){
    JsonObject abi=element.getAsJsonObject();String name=abi.get("name").getAsString();boolean found=false;
    for(Function function:called.values())if(function.getName().equals(name)){
     List<Variable> parameters=new ArrayList<>();int index=0;for(JsonElement type:abi.getAsJsonArray("parameters"))parameters.add(new ParameterImpl("arg"+(++index),observedType(type.getAsString()),currentProgram));
     function.updateFunction("__fastcall",new ReturnParameterImpl(observedType(abi.get("returnType").getAsString()),currentProgram),parameters,Function.FunctionUpdateType.DYNAMIC_STORAGE_FORMAL_PARAMS,true,SourceType.USER_DEFINED);
     Map<String,Object> item=new LinkedHashMap<>();item.put("external",name);item.put("address",function.getEntryPoint().toString());item.put("decoded",function.getPrototypeString(true,false));item.put("applied",true);signatures.add(item);found=true;
    }
    if(!found)throw new Exception("Requested explicit external ABI not called: "+name);
   }
   engine=new DecompInterface();engine.toggleCCode(true);engine.toggleSyntaxTree(true);if(!engine.openProgram(currentProgram))throw new Exception("Core decompiler open failed");
   for(int i=0;i<functions.size();i++){
    Function function=functions.get(i);JsonObject spec=manifest.getAsJsonArray("functions").get(i).getAsJsonObject();DecompileResults result=engine.decompileFunction(function,90,monitor);
    if(!result.decompileCompleted())throw new Exception("Core decompilation incomplete "+result.getErrorMessage());
    String code=result.getDecompiledFunction().getC(),role=spec.get("role").getAsString();Files.writeString(out.resolve(role+".c"),code,StandardCharsets.UTF_8);
    Map<String,Object> view=new LinkedHashMap<>();view.put("role",role);view.put("entry",function.getEntryPoint().toString());view.put("signature",function.getPrototypeString(true,false));view.put("decompilerSha256",hash(code.getBytes(StandardCharsets.UTF_8)));
    List<Map<String,Object>> blocks=new ArrayList<>();for(ghidra.program.model.pcode.PcodeBlockBasic block:result.getHighFunction().getBasicBlocks()){
     Map<String,Object> item=new LinkedHashMap<>();item.put("start",block.getStart().toString());item.put("stop",block.getStop().toString());blocks.add(item);
    }view.put("blocks",blocks);views.add(view);
   }
   report.put("status","original_core_static_views_pending_comparison");report.put("scope","Actual original bytes verified with explicit known value-type/helper ABI annotations. Static views, not original runtime outputs.");
  }catch(Exception error){report.put("status","failed");report.put("error",error.toString());throw error;}
  finally{if(engine!=null)engine.dispose();currentProgram.endTransaction(transaction,true);Files.writeString(out.resolve("analysis.json"),new GsonBuilder().setPrettyPrinting().disableHtmlEscaping().create().toJson(report)+"\n",StandardCharsets.UTF_8);}
  println("STATIC_ORIGINAL_CORE_COMPLETE "+out);
 }
}
