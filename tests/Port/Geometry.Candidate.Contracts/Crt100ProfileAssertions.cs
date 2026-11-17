using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Crt100ProfileAssertions
{
 private sealed class Resource:IDisposable {public bool Disposed;public void Dispose()=>Disposed=true;}
 public static string Run()
 {
  using var resource=typeof(Crt100ProfileAssertions).Assembly.GetManifestResourceStream("Crt100WideReference")??throw new Exception("Native CRT wide fixture missing");
  using var brotli=new BrotliStream(resource,CompressionMode.Decompress);using var fixture=JsonDocument.Parse(brotli);
  int lowerCount=0,compareCount=0,contextCount=0,checks=0;
  void Check(bool condition,string message){checks++;if(!condition)throw new Exception(message);}
  int profileIndex=0;
  foreach(var p in fixture.RootElement.GetProperty("Profiles").EnumerateArray())
  {
   var engine=Rhino5Crt100FilenameProfile.Get((Rhino5Crt100Locale)profileIndex);var names=engine.FilenameSemantics;
   Check(engine.LocaleName==p.GetProperty("Locale").GetString(),"CRT profile identity changed");
   byte[] expectedUnits=Convert.FromBase64String(p.GetProperty("LowerUtf16Base64").GetString()!),unitOutputs=new byte[131072],unitComparisons=new byte[65536*4];
   for(int unit=0;unit<65536;unit++)
   {
    string input=new((char)unit,1),output=engine.Lower(input+"\0A\ud800\udfff");
    if(output.Length!=5||output[1]!=0||output[2]!='A'||output[3]!=0xd800||output[4]!=0xdfff)throw new Exception("CRT lower crossed C terminator");
    unitOutputs[unit*2]=(byte)output[0];unitOutputs[unit*2+1]=(byte)(output[0]>>8);
    BinaryPrimitives.WriteInt32LittleEndian(unitComparisons.AsSpan(unit*4),engine.CompareRaw(input,output[..1]));lowerCount++;compareCount++;
   }
   Check(unitOutputs.AsSpan().SequenceEqual(expectedUnits),"Complete native whole lower units differ");
   Check(Hash(unitOutputs)==p.GetProperty("LowerSha256").GetString(),"Whole lower unit digest differs");
   Check(Hash(unitComparisons)==p.GetProperty("UnitWholeCompareSha256").GetString(),"Native unit comparison stream differs");
   byte[] scalarOutputs=new byte[1048576*4],scalarComparisons=new byte[1048576*4];
   for(int scalar=0x10000;scalar<=0x10ffff;scalar++)
   {
    string input=char.ConvertFromUtf32(scalar),output=engine.Lower(input);if(output.Length!=2)throw new Exception("CRT supplementary width changed");int at=(scalar-0x10000)*4;
    BinaryPrimitives.WriteUInt16LittleEndian(scalarOutputs.AsSpan(at),output[0]);BinaryPrimitives.WriteUInt16LittleEndian(scalarOutputs.AsSpan(at+2),output[1]);BinaryPrimitives.WriteInt32LittleEndian(scalarComparisons.AsSpan(at),engine.CompareRaw(input,output));lowerCount++;compareCount++;
   }
   Check(Hash(scalarOutputs)==p.GetProperty("SupplementarySha256").GetString(),"Complete native supplementary lower stream differs");
   Check(Hash(scalarComparisons)==p.GetProperty("SupplementaryCompareSha256").GetString(),"Complete native supplementary comparison stream differs");
   foreach(var pair in p.GetProperty("Comparisons").EnumerateArray()){if(engine.CompareRaw(pair.GetProperty("Left").GetString()!,pair.GetProperty("Right").GetString()!)!=pair.GetProperty("Result").GetInt32())throw new Exception("Native whole-string comparison differs");compareCount++;}
   foreach(var context in p.GetProperty("LowerContexts").EnumerateArray()){string input=Decode(Convert.FromBase64String(context.GetProperty("InputUtf16Base64").GetString()!)),expected=Decode(Convert.FromBase64String(context.GetProperty("OutputUtf16Base64").GetString()!));if(engine.Lower(input)!=expected)throw new Exception("Native whole-buffer lower differs");contextCount++;}
   string?[] boundaries=[null,"","\0","\0tail","A","a","A\0B","a\0C"];
   foreach(string? left in boundaries)foreach(string? right in boundaries){int expected=string.IsNullOrEmpty(right)||right[0]==0?(string.IsNullOrEmpty(left)?0:1):string.IsNullOrEmpty(left)?-1:engine.CompareRaw(left,right);Check(names.Compare(left,right)==expected,"Original stored-length wrapper differs");}
   Check(engine.Lower("A\0B")=="a\0B","CRT lower crossed terminator in ordinary string");
   Check(engine.CompareRaw("A\0B","a\0C")==0,"CRT comparison crossed terminator");
   Check(engine.Lower("ABC",3)=="abc"&&engine.CompareRaw("ABC","abc",3)==0,"Exact budget rejected");
   foreach(Action invalid in new Action[]{()=>engine.Lower(null!),()=>engine.CompareRaw(null!,"a"),()=>engine.CompareRaw("a",null!)}){try{invalid();throw new Exception("Null accepted");}catch(ArgumentNullException){checks++;}}
   foreach(Action invalid in new Action[]{()=>engine.Lower("a",-1),()=>engine.CompareRaw("a","b",-1)}){try{invalid();throw new Exception("Negative budget accepted");}catch(ArgumentOutOfRangeException){checks++;}}
   foreach(Action invalid in new Action[]{()=>engine.Lower("AB",1),()=>engine.CompareRaw("A","BB",1),()=>engine.CompareRaw("AA","B",1)}){try{invalid();throw new Exception("Budget overflow accepted");}catch(InvalidDataException){checks++;}}
   if(profileIndex>0)
   {
    string original="\ud801\udc00.PNG",lowered="\ud801\udc28.png";
    Check(names.Lower(original)==lowered&&names.Compare(original,lowered)==-40,"Original Deseret lower/compare disagreement lost");
    int created=0;using var cache=new Rhino5ContextImageCache<Resource>(names);
    using var first=cache.Lookup(1,original,false,()=>{created++;return new();})!;using var second=cache.Lookup(1,original,false,()=>{created++;return new();})!;
    Check(created==2&&cache.Count==2&&!ReferenceEquals(first.Resource,second.Resource),"Original uppercase cache miss silently repaired");
    Check(cache.Invalidate(original)==0&&cache.Count==2,"Original uppercase invalidation mismatch repaired");
    Check(cache.Invalidate(lowered)==2&&cache.Count==0&&!first.Resource.Disposed&&!second.Resource.Disposed,"Lowercase invalidation/lease retention failed");
    var firstResource=first.Resource;var secondResource=second.Resource;first.Dispose();second.Dispose();Check(firstResource.Disposed&&secondResource.Disposed,"Invalidated resources did not retire with their final leases");
   }
   if(profileIndex==2)Check(engine.Lower("Ý")=="ý"&&engine.CompareRaw("Ý","ý")==-32,"Turkish whole-lower vs unit-compare distinction lost");
   profileIndex++;
  }
  foreach(var invalid in new[]{(Rhino5Crt100Locale)(-1),(Rhino5Crt100Locale)3}){try{Rhino5Crt100FilenameProfile.Get(invalid);throw new Exception("Unknown recorded profile accepted");}catch(ArgumentOutOfRangeException){checks++;}}
  Check(lowerCount==3342336&&compareCount==3345804&&contextCount==147,"Native CRT profile coverage changed");
  return $"PASS CRT100 portable profiles:{lowerCount} native lower inputs/{compareCount} native comparisons/{contextCount} whole buffers/{checks} boundary/cache checks; recorded snapshots only, current-state/ANSI/failure/document/PC/physical remain open.";
 }
 private static string Hash(byte[] bytes)=>Convert.ToHexStringLower(SHA256.HashData(bytes));
 private static string Decode(byte[] bytes){char[] chars=new char[bytes.Length/2];for(int i=0;i<chars.Length;i++)chars[i]=(char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i*2));return new(chars);}
}
