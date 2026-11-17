using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class CrtSortAssertions
{
 private sealed record Item(int Key,int Id);
 public static string Run()
 {
  using var stream=typeof(CrtSortAssertions).Assembly.GetManifestResourceStream("Crt100SortReference")??throw new Exception("Native sort reference missing");using var brotli=new BrotliStream(stream,CompressionMode.Decompress);using var fixture=JsonDocument.Parse(brotli);int cases=0,comparisons=0;
  foreach(var c in fixture.RootElement.GetProperty("Cases").EnumerateArray())
  {
   int[] keys=c.GetProperty("InputKeys").EnumerateArray().Select(x=>x.GetInt32()).ToArray(),expected=c.GetProperty("NativeIds").EnumerateArray().Select(x=>x.GetInt32()).ToArray();int magnitude=c.GetProperty("Magnitude").GetInt32();var items=keys.Select((key,id)=>new Item(key,id)).ToList();var trace=new List<(int,int,int)>();
   Rhino5Crt100QSort.Sort(items,(a,b)=>{int result=a.Key.CompareTo(b.Key)*magnitude;trace.Add((a.Id,b.Id,result));return result;});
   if(!items.Select(x=>x.Id).SequenceEqual(expected))throw new Exception("Native qsort identity order mismatch case "+cases+" "+c.GetProperty("Group").GetString()+" size"+keys.Length);
   if(trace.Count!=c.GetProperty("NativeCompareCount").GetInt32())throw new Exception("Native qsort comparison count mismatch case "+cases);
   byte[] bytes=new byte[trace.Count*12];for(int i=0;i<trace.Count;i++){BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(i*12),trace[i].Item1);BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(i*12+4),trace[i].Item2);BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(i*12+8),trace[i].Item3);}
   if(Convert.ToHexStringLower(SHA256.HashData(bytes))!=c.GetProperty("NativeTraceSha256").GetString())throw new Exception("Native qsort callback trace mismatch case "+cases);
   comparisons+=trace.Count;cases++;
  }
  int guards=0;foreach(Action action in new Action[]{()=>Rhino5Crt100QSort.Sort<Item>(null!,(a,b)=>0),()=>Rhino5Crt100QSort.Sort(new List<Item>(),null!)}){try{action();throw new Exception("Invalid sort dependency accepted");}catch(ArgumentNullException){guards++;}}
  if(cases!=10431||comparisons!=1972827||guards!=2)throw new Exception("Native sort coverage changed");
  return $"PASS CRT100 portable qsort:{cases} exact native orders/{comparisons} exact callback events/{guards} dependency guards; width8-equivalent value sorting, original cache/document/PC/physical acceptance separate.";
 }
}
