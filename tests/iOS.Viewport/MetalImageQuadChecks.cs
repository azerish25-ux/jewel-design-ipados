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
public static class MetalImageQuadChecks
{
 public static async Task<string> Run(string directory)
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action a)where T:Exception{checks++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  byte[] red=[255,0,0,255],green=[0,255,0,255],blue=[0,0,255,255];
  using var renderer=new MetalImageQuadRenderer();
  Expect<InvalidOperationException>(()=>renderer.PrepareFrame(2,2));
  Expect<ArgumentException>(()=>renderer.SetImage(2,2,red));
  renderer.SetImage(1,1,red);using(var f=renderer.PrepareFrame(3,2)){var bytes=await f.CommitAsync();Check(bytes.Length==24&&Enumerable.Range(0,6).All(i=>bytes[i*4]==0&&bytes[i*4+1]==0&&bytes[i*4+2]==255&&bytes[i*4+3]==255),"Actual GPU nearest image quad");}
  Expect<ArgumentOutOfRangeException>(()=>renderer.PrepareFrame(0,2));
  // Actual opaque BMP pixels come from the real ImageIO decoder, not a preview image.
  var assembly=typeof(CandidateGeometryContractChecks.LocalImageAssetAssertions).Assembly;
  using var resource=assembly.GetManifestResourceStream("LocalImageDecodeFixtures")??throw new Exception("Image fixtures missing");
  using var fixture=JsonDocument.Parse(resource);var bmp=fixture.RootElement.GetProperty("bmp");
  using(var decoded=AppleLocalImageDecoder.Decode(LocalImageAsset.FromBytes("bmp",Convert.FromBase64String(bmp.GetProperty("base64").GetString()!))))
  {
   renderer.SetImage(decoded.Width,decoded.Height,decoded.CopyRgba());
   using var f=renderer.PrepareFrame(decoded.Width,decoded.Height);var actual=await f.CommitAsync();var rgba=decoded.CopyRgba();
   for(int i=0;i<actual.Length;i+=4)(rgba[i],rgba[i+2])=(rgba[i+2],rgba[i]);
   Check(actual.AsSpan().SequenceEqual(rgba),"Actual decoded BMP upload/sample/readback and top-row-first UV adaptation");
   File.WriteAllBytes(Path.Combine(directory,"image-gpu-bmp.bgra"),actual);
  }
  renderer.SetImage(2,1,[255,0,0,255,0,0,255,255]);
  using(var f=renderer.PrepareFrame(1,1,filtered:true)){var b=await f.CommitAsync();Check(Math.Abs(b[0]-128)<=1&&b[1]==0&&Math.Abs(b[2]-128)<=1&&b[3]==255,"Actual GPU linear filtering");}
  renderer.SetImage(2,1,[255,0,0,0,0,0,255,255]);using(var f=renderer.PrepareFrame(1,1,filtered:true)){var b=await f.CommitAsync();Check(Math.Abs(b[0]-128)<=1&&b[1]==0&&Math.Abs(b[2]-128)<=1&&Math.Abs(b[3]-128)<=1,"Straight alpha interpolation retains transparent texel RGB");}
  renderer.SetImage(1,1,green);using(var f=renderer.PrepareFrame(4,4,quad:[3,-1,0,1,3,1,0,1,-3,1,0,1,-3,-1,0,1])){var b=await f.CommitAsync();Check(Enumerable.Range(0,16).All(i=>b[i*4+1]==255),"Clipped image quad and clamp-to-edge sampling");}
  renderer.SetImage(1,1,[255,0,0,128]);using(var f=renderer.PrepareFrame(1,1,blend:true,clear:new MTLClearColor(0,0,1,1))){var b=await f.CommitAsync();Check(Math.Abs(b[2]-128)<=1&&b[1]==0&&Math.Abs(b[0]-127)<=1&&Math.Abs(b[3]-191)<=1,"Original SRC_ALPHA/ONE_MINUS_SRC_ALPHA applies to RGB and alpha");}
  renderer.SetImage(1,1,red);using(var cancelled=renderer.PrepareFrame(1,1)){}Check(renderer.Stats.InFlight==0,"Abandoned uncommitted image frame releases real resources");
  renderer.SetImage(1,1,red);using var a=renderer.PrepareFrame(1,1);
  renderer.SetImage(1,1,green);using var bframe=renderer.PrepareFrame(1,1);
  renderer.SetImage(1,1,blue);using var c=renderer.PrepareFrame(1,1);
  Check(renderer.Stats.InFlight==3&&renderer.Stats.LiveTextures==3,"Three pending actual command buffers retain different image revisions");
  Expect<InvalidOperationException>(()=>renderer.PrepareFrame(1,1));
  renderer.Dispose();Check(renderer.Stats.InFlight==3&&renderer.Stats.LiveTextures==3&&!renderer.Stats.PipelineReleased,"Disposal defers native texture/pipeline release while frames are prepared");
  Expect<ObjectDisposedException>(()=>renderer.SetImage(1,1,red));
  var outputs=await Task.WhenAll(a.CommitAsync(),bframe.CommitAsync(),c.CommitAsync());
  Check(outputs[0][2]==255&&outputs[1][1]==255&&outputs[2][0]==255,"Each actual GPU frame retains its original image through replacement/disposal");
  Check(renderer.Stats.InFlight==0&&renderer.Stats.LiveTextures==0&&renderer.Stats.LiveTargets==0&&renderer.Stats.LiveVertexBuffers==0&&renderer.Stats.PipelineReleased&&renderer.Stats.LastError is null,"Completed actual GPU frames retire all owned textures and pipeline");
  Expect<InvalidOperationException>(()=>a.CommitAsync());
  File.WriteAllText(Path.Combine(directory,"image-gpu-result.json"),JsonSerializer.Serialize(new MetalImageQuadReport(1,checks,renderer.Stats,"Actual offscreen image GPU sampling and owned lifetimes; full CAD image presentation/legacy decode adapter/PC/physical acceptance separate."),MetalImageQuadReportContext.Default.MetalImageQuadReport));
  return "PASS actual Metal image quad:"+checks+" decoded upload/filter/UV/blend/clipping/real-command replacement/disposal checks; visible integration/legacy codec/PC parity separate.";
 }
}

public sealed record MetalImageQuadReport(int SchemaVersion,int Checks,MetalImageQuadStats Stats,string Scope);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(MetalImageQuadReport))]
internal partial class MetalImageQuadReportContext:JsonSerializerContext{}
