// Read-only Ghidra import-model diagnostics. No target execution.
//@category CounterSketch.CameraAnalysis
import ghidra.app.script.GhidraScript;
import ghidra.program.model.address.*;
import ghidra.program.model.listing.*;
import ghidra.program.model.symbol.*;
import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import com.google.gson.GsonBuilder;
public class InspectNativeImportModel extends GhidraScript {
 @Override public void run()throws Exception{
  String[] args=getScriptArgs();if(args.length!=2)throw new Exception("Expected output folder and root");
  if(!"951ff639f2c9b61bbf7c32a67169d0e23bdbd026f74ada80ace9b1f02b1b48eb".equals(currentProgram.getExecutableSHA256()))throw new Exception("Wrong target");
  Path root=Paths.get(args[1]).toRealPath(),out=Paths.get(args[0]).toAbsolutePath().normalize();if(!root.equals(Paths.get(System.getProperty("user.dir")).toRealPath()))throw new Exception("Repository root differs from launcher workspace");if(System.getProperty("os.name").toLowerCase(Locale.ROOT).contains("windows")&&!root.toString().toLowerCase(Locale.ROOT).startsWith("d:"))throw new Exception("Windows analysis root must stay on D:");if(!out.startsWith(root))throw new Exception("Output leaves workspace");Files.createDirectories(out);
  List<Map<String,Object>> rows=new ArrayList<>();
  for(long rva:new long[]{0xee2bd0,0xed5f58,0xed9ff0,0xeddf08}){
   Address at=currentProgram.getImageBase().add(rva);Map<String,Object> row=new LinkedHashMap<>();row.put("iat",at.toString());
   Data data=currentProgram.getListing().getDataAt(at);row.put("dataType",data==null?null:data.getDataType().toString());row.put("dataValue",data==null?null:String.valueOf(data.getValue()));
   List<Map<String,Object>> refs=new ArrayList<>();for(Reference reference:currentProgram.getReferenceManager().getReferencesFrom(at)){
    Map<String,Object> item=new LinkedHashMap<>();item.put("type",reference.getReferenceType().toString());item.put("class",reference.getClass().getName());item.put("to",reference.getToAddress().toString());
    Symbol symbol=currentProgram.getSymbolTable().getPrimarySymbol(reference.getToAddress());item.put("symbol",symbol==null?null:symbol.getName(true));item.put("symbolType",symbol==null?null:symbol.getSymbolType().toString());
    if(reference instanceof ExternalReference){ExternalLocation location=((ExternalReference)reference).getExternalLocation();item.put("externalLabel",location.getLabel());item.put("isFunction",location.isFunction());item.put("function",location.getFunction()==null?null:location.getFunction().getPrototypeString(true,false));}
    refs.add(item);
   }row.put("references",refs);rows.add(row);
  }
  Map<String,Object> code=new LinkedHashMap<>();for(long rva:new long[]{0x44c194,0x44c1fe,0x44c21d}){Address at=currentProgram.getImageBase().add(rva);Instruction instruction=currentProgram.getListing().getInstructionAt(at);code.put(at.toString(),instruction==null?"undefined":instruction.toString());}Files.writeString(out.resolve("listing-model.json"),new GsonBuilder().setPrettyPrinting().create().toJson(code),StandardCharsets.UTF_8);
  Files.writeString(out.resolve("imports.json"),new GsonBuilder().setPrettyPrinting().create().toJson(rows)+"\n",StandardCharsets.UTF_8);println("STATIC_IMPORT_MODEL_COMPLETE");
 }
}
