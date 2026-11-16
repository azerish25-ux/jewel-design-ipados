using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Metal;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class StraightImageDecodeChecks
{
 public static async Task<string> Run(string directory)
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action a)where T:Exception{checks++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  var assembly=typeof(CandidateGeometryContractChecks.StraightImageProviderAssertions).Assembly;
  using var stream=assembly.GetManifestResourceStream("StraightImageAlphaFixture")??throw new Exception("Straight fixture missing");
  using var doc=JsonDocument.Parse(stream);var f=doc.RootElement;
  byte[] encoded=Convert.FromBase64String(f.GetProperty("encodedBase64").GetString()!),wanted=Convert.FromBase64String(f.GetProperty("expectedRgba").GetString()!);
  var source=LocalImageAsset.FromBytes(@"C:\original\alpha.png",encoded);
  using var image=AppleLocalImageDecoder.DecodeStraight(source);
  Check(image.AlphaRepresentation==RgbaAlphaRepresentation.Straight&&image.PixelSource==DecodedPixelSource.EncodedPng,"Explicit straight-alpha PNG sample extraction, no premultiplied-provider reconstruction");
  Check(image.Width==16&&image.Height==16&&image.SourceFrameCount==1,"Real provider dimensions/frame");
  Check(image.CopyRgba().AsSpan().SequenceEqual(wanted),"All256 alpha values retain original non-primary and transparent RGB exactly");
  Check(source.ContentSha256==f.GetProperty("encodedSha256").GetString()&&image.Source.CopyEncodedBytes().AsSpan().SequenceEqual(encoded),"Original encoded source retained exactly");
  Check(image.ProviderLayout is {BitsPerComponent:8,ColorSpaceModel:1,HasDecodeArray:false},"Actual native provider layout retained");
  using var renderer=new MetalImageQuadRenderer();renderer.SetImage(image);
  using(var frame=renderer.PrepareFrame(16,16)){
   var gpu=await frame.CommitAsync();var expected=(byte[])wanted.Clone();for(int i=0;i<expected.Length;i+=4)(expected[i],expected[i+2])=(expected[i+2],expected[i]);
   Check(gpu.AsSpan().SequenceEqual(expected),"Real typed straight decode/upload/sample/readback preserves all1024 channels");
   File.WriteAllBytes(Path.Combine(directory,"straight-image-gpu.bgra"),gpu);
  }
  using(var frame=renderer.PrepareFrame(16,16,blend:true,clear:new MTLClearColor(0,0,1,1))){
   var gpu=await frame.CommitAsync();bool ok=true;
   for(int i=0;i<wanted.Length;i+=4){double a=wanted[i+3]/255d;double[] expected=[wanted[i+2]*a+255*(1-a),wanted[i+1]*a,wanted[i]*a,wanted[i+3]*a+255*(1-a)];for(int c=0;c<4;c++)ok&=Math.Abs(gpu[i+c]-expected[c])<=1;}
   Check(ok,"All256 alpha levels follow original source-alpha blend factors through real GPU");
  }
  using var premultiplied=AppleLocalImageDecoder.Decode(source);
  Check(premultiplied.AlphaRepresentation==RgbaAlphaRepresentation.Premultiplied,"Existing drawing decoder remains explicitly premultiplied");
  Expect<NotSupportedException>(()=>renderer.SetImage(premultiplied));
  using(var frame=renderer.PrepareFrame(16,16)){var gpu=await frame.CommitAsync();Check(gpu.Length==wanted.Length&&gpu[2]==wanted[0],"Rejected representation did not replace the valid image");}
  using var fixture=JsonDocument.Parse(assembly.GetManifestResourceStream("LocalImageDecodeFixtures")!);
  foreach(string name in new[]{"png","bmp"}){
   var item=fixture.RootElement.GetProperty(name);var bytes=Convert.FromBase64String(item.GetProperty("base64").GetString()!);
   using var decoded=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes(name,bytes));
   var expected=item.GetProperty("rawRgba").EnumerateArray().Select(x=>(byte)x.GetInt32()).ToArray();if(name=="png")expected[12]=255;
   Check(decoded.CopyRgba().AsSpan().SequenceEqual(expected),name+" exact raw provider row order/straight pixels");
   if(name=="png")Check(decoded.SourceOrientation==6,"Source orientation retained without auto-transform");
  }
  using var jpg=assembly.GetManifestResourceStream("RecoveredImageDecodeJpeg")!;using var memory=new MemoryStream();jpg.CopyTo(memory);
  using(var decoded=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("jpg",memory.ToArray()))){var p=decoded.CopyRgba();Check(decoded.Width==100&&decoded.Height==100&&Enumerable.Range(0,p.Length/4).All(i=>p[i*4+3]==255),"Actual JPEG provider and skip-alpha normalization");}
  Expect<InvalidDataException>(()=>AppleLocalImageDecoder.DecodeStraight(source,new(MaxEncodedBytes:1)));
  Expect<InvalidDataException>(()=>AppleLocalImageDecoder.DecodeStraight(source,new(MaxDecodedBytes:1)));
  Expect<InvalidDataException>(()=>AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("corrupt",[1,2,3])));
  image.Dispose();Expect<ObjectDisposedException>(()=>renderer.SetImage(image));
  renderer.Dispose();Check(renderer.Stats.InFlight==0&&renderer.Stats.LiveTextures==0&&renderer.Stats.LiveTargets==0&&renderer.Stats.LiveVertexBuffers==0&&renderer.Stats.PipelineReleased&&renderer.Stats.LastError is null,"Typed upload native resources retired");
  File.WriteAllText(Path.Combine(directory,"straight-image-result.json"),JsonSerializer.Serialize(new StraightImageReport(1,checks,256,image.ProviderLayout!,renderer.Stats),StraightImageReportContext.Default.StraightImageReport));
  return "PASS actual straight image decode:"+checks+" provider/typed-upload/failure cases and256 exact alpha levels with actual GPU bytes; wider layouts/profile/visible/source codec/PC parity separate.";
 }
}
public sealed record StraightImageReport(int SchemaVersion,int Checks,int AlphaLevels,ImageProviderLayout Layout,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(StraightImageReport))]
internal partial class StraightImageReportContext:JsonSerializerContext{}
