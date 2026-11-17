using System.Buffers;
using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class AnsiNlsProfileAssertions
{
 private sealed class Resource:IDisposable {public bool Disposed;public void Dispose()=>Disposed=true;}
 public static string Run()
 {
  using var resource=typeof(AnsiNlsProfileAssertions).Assembly.GetManifestResourceStream("AnsiNlsReference")??throw new Exception("Native ANSI fixture missing");using var compressed=new BrotliStream(resource,CompressionMode.Decompress);using var fixture=JsonDocument.Parse(compressed);var r=fixture.RootElement;
  var engine=Rhino5AnsiNlsProfile.Reference60502;var crt=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.C);int checks=0,comparisons=0,lowerPairs=0,decodePairs=0,scalarCount=0,recovered=0;
  void Check(bool value,string message){checks++;if(!value)throw new Exception(message);}
  byte[] nativeCompare=Convert.FromBase64String(r.GetProperty("PairCompareABase64").GetString()!),nativeLower=Convert.FromBase64String(r.GetProperty("PairLowerOutputBase64").GetString()!),decoded= new byte[262144];
  for(int a=0;a<256;a++)for(int b=0;b<256;b++)
  {
   byte[] pair=[(byte)a,(byte)b];int index=a*256+b;
   if(engine.CompareBytes([(byte)a,0],[(byte)b,0])!=nativeCompare[index]-2)throw new Exception("Native ANSI comparison pair "+index);comparisons++;
   if(!engine.TryLowerBytes(pair,out byte[] mapped,out int error)||error!=0||mapped.Length!=2||mapped[0]!=nativeLower[index*2]||mapped[1]!=nativeLower[index*2+1])throw new Exception("Native ANSI lower pair "+index);lowerPairs++;
   string value=engine.DecodeBytes(pair);if(value.Length!=2)throw new Exception("CP1256 pair width");BinaryPrimitives.WriteUInt16LittleEndian(decoded.AsSpan(index*4),value[0]);BinaryPrimitives.WriteUInt16LittleEndian(decoded.AsSpan(index*4+2),value[1]);decodePairs++;
  }
  Check(Hash(decoded)==r.GetProperty("PairDecodedSha256").GetString(),"Complete native CP1256 decode stream differs");
  var utf8=new UTF8Encoding(false,true);byte[] header=new byte[8];int plane=0;
  foreach(var nativePlane in r.GetProperty("Planes").EnumerateArray())
  {
   using var stream=new MemoryStream();int count=0,invalid=0,changed=0;
   for(int key=plane<<16;key<(plane+1)*65536;key++)
   {
    if(key is >=0xd800 and <=0xdfff)continue;string source=char.ConvertFromUtf32(key);byte[] input=utf8.GetBytes(source);
    if(!engine.TryLowerBytes(input,out byte[] output,out int error)||error!=0)throw new Exception("Valid scalar byte lower failed");BinaryPrimitives.WriteInt32LittleEndian(header,key);BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4),output.Length);stream.Write(header);stream.Write(output);if(!input.AsSpan().SequenceEqual(output))changed++;
    OperationStatus status=Rune.DecodeFromUtf8(output,out Rune rune,out int consumed);bool bad=status!=OperationStatus.Done||consumed!=output.Length;if(bad)invalid++;
    // Whole-pipeline expected recovery is source-derived, separate from the native byte-API oracle.
    var whole=engine.LowerDetailed(source,crt);string expected=bad?"\ufffd":rune.ToString();if(whole.Value!=expected||whole.DecodeErrors!=0||whole.LegacyReplacementGroups!=(bad?1:0))throw new Exception("Legacy whole-scalar decoder mismatch at "+key);
    if(key==0){if(!whole.UsedCrtFallback||whole.NlsError!=87)throw new Exception("Zero C-prefix fallback lost");}else if(whole.UsedCrtFallback||whole.NlsError!=0)throw new Exception("Valid scalar routed to CRT");
    count++;scalarCount++;if(bad)recovered++;
   }
   Check(count==nativePlane.GetProperty("Cases").GetInt32()&&changed==nativePlane.GetProperty("Changed").GetInt32()&&invalid==nativePlane.GetProperty("InvalidUtf8Outputs").GetInt32(),"Native scalar plane counts differ");Check(Hash(stream.ToArray())==nativePlane.GetProperty("NativeSha256").GetString(),"Complete native scalar byte output digest differs");plane++;
  }
  Check(!engine.TryLowerBytes([],out byte[] none,out int zeroError)&&none.Length==0&&zeroError==87,"Native zero-count failure control lost");
  Check(engine.EncodePrefix("\ufeffA").Bytes.AsSpan().SequenceEqual(new byte[]{0xef,0xbb,0xbf,65}),"BOM was stripped");
  var prefix=engine.EncodePrefix("A\ud800B");Check(prefix.Bytes.AsSpan().SequenceEqual(new byte[]{65})&&prefix.QueryErrors==16&&prefix.WriteErrors==18&&prefix.ConsumedUtf16Units==1&&prefix.CPrefixUtf16Units==3,"Strict measured prefix/writer-capacity policy changed");
  var firstBad=engine.EncodePrefix("\ud800B");Check(firstBad.Bytes.Length==0&&firstBad.QueryErrors==16&&firstBad.WriteErrors==0,"Zero-capacity writer was called");
  Check(engine.EncodePrefix("A\0B").Bytes.AsSpan().SequenceEqual(new byte[]{65}),"ANSI encoding crossed C terminator");
  Check(engine.LowerDetailed("A\ud800B",crt) is {Value:"a",QueryErrors:16,WriteErrors:18,UsedCrtFallback:false},"Malformed suffix no longer truncates at strict prefix");
  Check(engine.LowerDetailed("\ud800B",crt) is {Value:"\ud800b",QueryErrors:16,WriteErrors:0,UsedCrtFallback:true,NlsError:87},"Invalid first unit no longer falls back on original string");
  Check(engine.Lower("ABC\0DEF",crt)=="abc"&&engine.Lower("\0ABC",crt)=="\0ABC","ANSI success vs zero-prefix stored suffix behavior changed");
  Check(engine.LowerDetailed("",crt) is {Value:"",NlsAttempted:false,UsedCrtFallback:false},"Empty stored string attempted NLS");
  Check(engine.Lower("Ì",crt)=="Ü","Original byte-level non-Unicode casing was repaired");
  string high=char.ConvertFromUtf32(0x10c000),last=char.ConvertFromUtf32(0x10cfff);Check(engine.LowerDetailed(high+"A"+last,crt) is {Value:"\ufffda\ufffd",DecodeErrors:0,LegacyReplacementGroups:2},"Invalid four-byte groups did not recover once per group");
  Check(engine.LowerDetailed(char.ConvertFromUtf32(0x10bfff),crt).DecodeErrors==0&&engine.LowerDetailed(char.ConvertFromUtf32(0x10d000),crt).DecodeErrors==0,"Invalid-output boundary widened");
  Check(engine.CompareDetailed("A-B.png","AB.png",crt) is {Value:-53,Route:Rhino5AnsiCompareRoute.CrtAscii},"All-ASCII compare stopped using raw CRT magnitude");
  Check(engine.CompareDetailed("zz","aa猫",crt) is {Value:25,Route:Rhino5AnsiCompareRoute.CrtAscii},"ASCII scan incorrectly compared characters before its terminator");
  Check(engine.CompareDetailed("z猫","ab",crt) is {Value:1,Route:Rhino5AnsiCompareRoute.NlsBytes},"Non-ASCII stop did not use normalized NLS result");
  Check(engine.CompareRaw(null,null,crt)==0&&engine.CompareRaw(null,"",crt)==-1&&engine.CompareRaw("",null,crt)==1,"Raw pointer guards changed");
  Check(engine.CompareDetailed("\ud800X","\udc00Y",crt) is {Value:0,Route:Rhino5AnsiCompareRoute.NlsBytes,FirstQueryErrors:16,SecondQueryErrors:16},"Strict-empty malformed comparison was repaired");
  var names=engine.CreateFilenameSemantics(crt);string?[] boundaries=[null,"","\0","\0tail","A","a","A\0B","a\0C"];foreach(var left in boundaries)foreach(var right in boundaries){int expected=string.IsNullOrEmpty(right)||right[0]==0?(string.IsNullOrEmpty(left)?0:1):string.IsNullOrEmpty(left)?-1:engine.CompareRaw(left,right,crt);Check(names.Compare(left,right)==expected,"Stored-length filename wrapper changed");}
  int normalCreated=0;using(var normal=new Rhino5ContextImageCache<Resource>(names)){using var original=normal.Lookup(2,"Ì.PNG",false,()=>{normalCreated++;return new();})!;using var alias=normal.Lookup(2,"Ü.png",false,()=>throw new Exception("ANSI alias missed real cache"))!;Check(normalCreated==1&&ReferenceEquals(original.Resource,alias.Resource),"CP1256 case alias did not retain identity");Check(normal.Snapshot().Single().Key.Filename=="Ü.png","Actual ANSI stored casing changed");Check(normal.Invalidate("Ì.PNG")==1&&!original.Resource.Disposed,"ANSI alias invalidation killed leased resource");var held=original.Resource;original.Dispose();alias.Dispose();Check(held.Disposed,"Normal ANSI cached resource did not retire");}
  int made=0;using var cache=new Rhino5ContextImageCache<Resource>(names);using var one=cache.Lookup(1,high+".PNG",false,()=>{made++;return new();})!;using var two=cache.Lookup(1,high+".PNG",false,()=>{made++;return new();})!;Check(made==2&&cache.Count==2,"Invalid-UTF8 name cache miss was silently repaired");Check(cache.Invalidate(high+".PNG")==0&&cache.Invalidate("\ufffd.png")==2,"Original vs decoded replacement invalidation changed");var v1=one.Resource;var v2=two.Resource;Check(!v1.Disposed&&!v2.Disposed,"Invalidation killed active leases");one.Dispose();two.Dispose();Check(v1.Disposed&&v2.Disposed,"ANSI cache resources did not retire");
  foreach(Action bad in new Action[]{()=>engine.EncodePrefix(null!),()=>engine.LowerDetailed(null!,crt),()=>engine.CreateFilenameSemantics(null!)}){try{bad();throw new Exception("Null argument accepted");}catch(ArgumentNullException){checks++;}}
  foreach(Action bad in new Action[]{()=>engine.EncodePrefix("a",-1),()=>engine.EncodePrefix("a",1,-1),()=>engine.CompareBytes([],[],-1)}){try{bad();throw new Exception("Negative budget accepted");}catch(ArgumentOutOfRangeException){checks++;}}
  foreach(Action bad in new Action[]{()=>engine.EncodePrefix("AB",1),()=>engine.EncodePrefix("猫",2,2),()=>engine.CompareBytes([1,2],[1],1),()=>engine.DecodeBytes([1,2],1)}){try{bad();throw new Exception("Budget exceeded");}catch(InvalidDataException){checks++;}}
  Check(engine.EncodePrefix("猫",1,3).Bytes.Length==3&&engine.CompareBytes([65],[97],1)==0,"Exact budgets rejected");
  Check(scalarCount==1112064&&comparisons==65536&&lowerPairs==65536&&decodePairs==65536&&recovered==4096,"ANSI native coverage changed");
  return $"PASS ANSI portable profile:{scalarCount} native scalar byte outputs/{comparisons} native comparison pairs/{lowerPairs} native lower pairs/{decodePairs} native decode pairs/{recovered} legacy recovery cases/{checks} boundary/cache checks; explicit recorded profile, runtime/document/PC/physical remain open.";
 }
 private static string Hash(byte[] bytes)=>Convert.ToHexStringLower(SHA256.HashData(bytes));
}
