using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class ComposedTextureGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  using var stream=typeof(CandidateGeometryContractChecks.ComposedTextureAssertions).Assembly.GetManifestResourceStream("ComposedTextureGpuFixture")??throw new Exception("Composed GPU fixture missing");using var fixture=JsonDocument.Parse(stream);using var renderer=new MetalImageQuadRenderer();var results=new List<ComposedTextureGpuCase>();int pixels=0,rejections=0,pending=0;
  foreach(var c in fixture.RootElement.GetProperty("cases").EnumerateArray()){
   int index=c.GetProperty("nativeIndex").GetInt32(),depth=c.GetProperty("Depth").GetInt32(),w=c.GetProperty("Width").GetInt32(),h=c.GetProperty("Height").GetInt32();var s=c.GetProperty("Settings");var settings=new Rhino5TextureRasterSettings(s.GetProperty("DeviceMaximum").GetInt32(),s.GetProperty("MemoryBudget").GetUInt32(),s.GetProperty("MemoryFraction").GetSingle(),s.GetProperty("PowerOfTwo").GetBoolean());bool gray=c.GetProperty("Gray").GetBoolean();using var decoded=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("composed-source.png",Convert.FromBase64String(c.GetProperty("pngBase64").GetString()!)));
   if(!decoded.CopyRgba().AsSpan().SequenceEqual(Convert.FromBase64String(c.GetProperty("SourceRgbaBase64").GetString()!)))throw new Exception("PNG source did not preserve captured native input");
   var receipt=renderer.SetTextureSource(decoded,depth,settings,gray);
   if(receipt.Width!=w||receipt.Height!=h||receipt.StretchMode!=c.GetProperty("Mode").GetInt32()||receipt.GrayScale!=gray)throw new Exception("Actual source upload receipt differs from native case");
   if(results.Count==0){try{renderer.SetTextureSource(decoded,depth,new(1,0,1f,false));throw new Exception("Expected source preparation failure");}catch(InvalidDataException){rejections++;}}
   decoded.Dispose();byte[] actual;using(var frame=renderer.PrepareFrame(w,h)){if(results.Count==19){pending=renderer.Stats.InFlight;renderer.Dispose();}actual=await frame.CommitAsync();}
   byte[] expected=Convert.FromBase64String(c.GetProperty("OutputRgbaBase64").GetString()!);for(int i=0;i<expected.Length;i+=4){byte red=expected[i];expected[i]=expected[i+2];expected[i+2]=red;}
   if(!actual.AsSpan().SequenceEqual(expected))throw new Exception("Composed decode/filter/alpha/Metal readback mismatch native case"+index);
   string name="composed-texture-"+results.Count+".bgra";File.WriteAllBytes(Path.Combine(directory,name),actual);results.Add(new(index,receipt,Convert.ToHexString(SHA256.HashData(actual)).ToLowerInvariant(),name));pixels+=w*h;
  }
  renderer.Dispose();var stats=renderer.Stats;if(results.Count!=20||pixels!=584||rejections!=1||pending!=1||stats.InFlight!=0||stats.LiveTextures!=0||stats.LiveTargets!=0||stats.LiveVertexBuffers!=0||stats.LiveSamplers!=0||stats.LastError is not null)throw new Exception("Composed GPU coverage/retirement mismatch");
  File.WriteAllText(Path.Combine(directory,"composed-texture-gpu.json"),JsonSerializer.Serialize(new ComposedTextureGpuReport(1,results,pixels,rejections,pending,stats),ComposedTextureGpuContext.Default.ComposedTextureGpuReport));
  return "PASS actual composed texture GPU:20 decoded PNG/source preparation/upload/readback cases/584 exact pixels/1 retained-image failure guard; all owned resources retired, original codecs/cache/vendor/PC/physical separate.";
 }
}
public sealed record ComposedTextureGpuCase(int NativeIndex,Rhino5TextureRasterReceipt Receipt,string BgraSha256,string File);
public sealed record ComposedTextureGpuReport(int SchemaVersion,List<ComposedTextureGpuCase> Cases,int ExactPixels,int RejectedSourceReplacements,int DisposalInFlight,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ComposedTextureGpuReport))]
internal partial class ComposedTextureGpuContext:JsonSerializerContext{}
