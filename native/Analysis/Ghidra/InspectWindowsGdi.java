// Bounded static dependency audit. Never loads/executes a driver or vendor application.
//@category CounterSketch.ImageDependencyAnalysis
import ghidra.app.decompiler.*;
import ghidra.program.model.data.*;
import ghidra.program.model.listing.*;
import ghidra.program.model.symbol.*;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import com.google.gson.*;
public class InspectWindowsGdi extends RefineNativeCamera {
 private DataType type(String value)throws Exception{if(value.endsWith("*"))return new PointerDataType(type(value.substring(0,value.length()-1)),currentProgram.getDataTypeManager());switch(value){case "void":return VoidDataType.dataType;case "uint":return UnsignedIntegerDataType.dataType;case "int":return IntegerDataType.dataType;case "u64":return UnsignedLongLongDataType.dataType;default:throw new Exception("Unverified GDI ABI type "+value);}}
 @Override public void run()throws Exception{
  String[] args=getScriptArgs();if(args.length!=3)throw new Exception("Expected manifest, output, root");
  Path root=Paths.get(args[2]).toRealPath(),manifest=Paths.get(args[0]).toAbsolutePath().normalize(),out=Paths.get(args[1]).toAbsolutePath().normalize();
  if(!root.equals(Paths.get(System.getProperty("user.dir")).toRealPath())||!manifest.startsWith(root)||!out.startsWith(root))throw new Exception("GDI analysis leaves workspace");
  if(System.getProperty("os.name").toLowerCase(Locale.ROOT).contains("windows")&&!root.toString().toLowerCase(Locale.ROOT).startsWith("d:"))throw new Exception("Windows GDI analysis must stay on D:");
  JsonObject spec=JsonParser.parseString(Files.readString(manifest,StandardCharsets.UTF_8)).getAsJsonObject();if(!spec.get("targetSha256").getAsString().equalsIgnoreCase(currentProgram.getExecutableSHA256()))throw new Exception("GDI dependency SHA mismatch");
  Files.createDirectories(out);out=out.toRealPath();if(!out.startsWith(root))throw new Exception("Resolved output escapes root");
  Map<String,Object> report=new LinkedHashMap<>();report.put("targetSha256",currentProgram.getExecutableSHA256());report.put("analysisMode","static_only_no_driver_load_debugging_or_direct_function_invocation");List<Map<String,Object>> views=new ArrayList<>();report.put("views",views);
  int transaction=currentProgram.startTransaction("Bounded Windows GDI dependency audit");DecompInterface engine=null;
  try{
   List<Function> functions=new ArrayList<>();
   for(JsonElement element:spec.getAsJsonArray("functions")){
    JsonObject item=element.getAsJsonObject();Function function=region(item);function.setName(item.has("displayName")?item.get("displayName").getAsString():"Gdi_"+item.get("role").getAsString(),SourceType.USER_DEFINED);if(item.has("noReturn"))function.setNoReturn(item.get("noReturn").getAsBoolean());
    if(item.has("parameters")){List<Variable> parameters=new ArrayList<>();int i=0;for(JsonElement p:item.getAsJsonArray("parameters"))parameters.add(new ParameterImpl("arg"+(++i),type(p.getAsString()),currentProgram));function.updateFunction("__fastcall",new ReturnParameterImpl(type(item.get("returnType").getAsString()),currentProgram),parameters,Function.FunctionUpdateType.DYNAMIC_STORAGE_FORMAL_PARAMS,true,SourceType.USER_DEFINED);}
    functions.add(function);
   }
   engine=new DecompInterface();engine.toggleCCode(true);engine.toggleSyntaxTree(true);if(!engine.openProgram(currentProgram))throw new Exception("GDI decompiler open failed");
   for(int i=0;i<functions.size();i++){Function function=functions.get(i);JsonObject item=spec.getAsJsonArray("functions").get(i).getAsJsonObject();DecompileResults result=engine.decompileFunction(function,120,monitor);if(!result.decompileCompleted())throw new Exception("GDI decompilation incomplete "+result.getErrorMessage());String code=result.getDecompiledFunction().getC(),role=item.get("role").getAsString();Files.writeString(out.resolve(role+".c"),code,StandardCharsets.UTF_8);Map<String,Object> view=new LinkedHashMap<>();view.put("role",role);view.put("entry",function.getEntryPoint().toString());view.put("signature",function.getPrototypeString(true,false));view.put("abi",item.has("parameters")?"explicit_instruction_reviewed":"decompiler_inferred_unverified");view.put("decompilerSha256",hash(code.getBytes(StandardCharsets.UTF_8)));views.add(view);}
   report.put("status","bounded_gdi_static_views_pending_instruction_review");
  }catch(Exception error){report.put("status","failed");report.put("error",error.toString());throw error;}
  finally{if(engine!=null)engine.dispose();currentProgram.endTransaction(transaction,true);Files.writeString(out.resolve("analysis.json"),new GsonBuilder().setPrettyPrinting().disableHtmlEscaping().create().toJson(report)+"\n",StandardCharsets.UTF_8);}
  println("STATIC_WINDOWS_GDI_COMPLETE "+out);
 }
}
