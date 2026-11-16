using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class PngStraightSampleAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action a)where T:Exception{checks++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  using var input=typeof(PngStraightSampleAssertions).Assembly.GetManifestResourceStream("PngStraightSamples")??throw new Exception("PNG corpus missing");
  using var doc=JsonDocument.Parse(input);byte[]? valid=null;
  foreach(var f in doc.RootElement.GetProperty("cases").EnumerateArray()){
   var encoded=Convert.FromBase64String(f.GetProperty("png").GetString()!);var expected=Convert.FromBase64String(f.GetProperty("rgba").GetString()!);
   var result=PngStraightSamples.Decode(encoded);
   Check(result.Width==f.GetProperty("width").GetInt32()&&result.Height==f.GetProperty("height").GetInt32(),"PNG dimensions");
   Check(result.Rgba.AsSpan().SequenceEqual(expected),f.GetProperty("name").GetString()+" exact samples/filters/Adam7/transparency");
   Check(result.BitDepth==f.GetProperty("depth").GetInt32()&&result.ColorType==f.GetProperty("type").GetInt32()&&result.Interlace==f.GetProperty("interlace").GetInt32(),"PNG sample provenance");
   if(f.GetProperty("type").GetInt32()==6&&f.GetProperty("interlace").GetInt32()==0)valid=encoded;
  }
  foreach(var f in doc.RootElement.GetProperty("invalid").EnumerateArray()){var bytes=Convert.FromBase64String(f.GetProperty("png").GetString()!);if(f.GetProperty("error").GetString()=="NotSupported")Expect<NotSupportedException>(()=>PngStraightSamples.Decode(bytes));else Expect<InvalidDataException>(()=>PngStraightSamples.Decode(bytes));}
  byte[] png=valid!;var bad=(byte[])png.Clone();bad[20]^=1;Expect<InvalidDataException>(()=>PngStraightSamples.Decode(bad));
  Expect<InvalidDataException>(()=>PngStraightSamples.Decode(png.AsSpan(0,png.Length-1)));
  Expect<InvalidDataException>(()=>PngStraightSamples.Decode([1,2,3,4]));
  Expect<InvalidDataException>(()=>PngStraightSamples.Decode(png,new(MaxDecodedBytes:1)));
  var trailing=png.Concat(new byte[]{0}).ToArray();Expect<InvalidDataException>(()=>PngStraightSamples.Decode(trailing));
  return "PASS lossless PNG samples:"+checks+" color/depth/filter/Adam7/transparency/CRC/budget cases; profile/high-depth/APNG/source codec parity separate.";
 }
}
