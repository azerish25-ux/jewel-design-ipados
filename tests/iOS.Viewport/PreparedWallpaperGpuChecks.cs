using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class PreparedWallpaperGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  using var stream=typeof(CandidateGeometryContractChecks.PreparedWallpaperAssertions).Assembly.GetManifestResourceStream("PreparedWallpaperGpuFixture")??throw new Exception("Wallpaper GPU fixture missing");using var brotli=new BrotliStream(stream,CompressionMode.Decompress);using var fixture=JsonDocument.Parse(brotli);using var renderer=new MetalImageQuadRenderer();var cases=new List<PreparedWallpaperGpuCase>();int pixels=0,rejected=0,pending=0;
  foreach(var c in fixture.RootElement.GetProperty("cases").EnumerateArray()){int depth=c.GetProperty("Depth").GetInt32(),rw=c.GetProperty("RequestedWidth").GetInt32(),rh=c.GetProperty("RequestedHeight").GetInt32(),w=c.GetProperty("Width").GetInt32(),h=c.GetProperty("Height").GetInt32();bool gray=c.GetProperty("Gray").GetBoolean();using var image=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("wallpaper-source.png",Convert.FromBase64String(c.GetProperty("pngBase64").GetString()!)));if(!image.CopyRgba().AsSpan().SequenceEqual(Convert.FromBase64String(c.GetProperty("SourceRgbaBase64").GetString()!)))throw new Exception("Wallpaper PNG input differs from native source");
   var receipt=renderer.SetWallpaperSource(image,depth,rw,rh,gray);if(receipt.Plan.StorageWidth!=w||receipt.Plan.Height!=h||receipt.Plan.DownscaleDivisor!=c.GetProperty("Divisor").GetInt32()||receipt.GrayScale!=gray)throw new Exception("Wallpaper source receipt/native fit mismatch");
   if(cases.Count==0){try{renderer.SetWallpaperSource(image,depth,rw,rh,gray,null,1);throw new Exception("Expected wallpaper budget failure");}catch(InvalidDataException){rejected++;}}
   image.Dispose();byte[] bytes;using(var frame=renderer.PrepareFrame(w,h)){if(cases.Count==19){pending=renderer.Stats.InFlight;renderer.Dispose();}bytes=await frame.CommitAsync();}byte[] expected=Convert.FromBase64String(c.GetProperty("OutputRgbaBase64").GetString()!);for(int i=0;i<expected.Length;i+=4){byte r=expected[i];expected[i]=expected[i+2];expected[i+2]=r;}if(!bytes.AsSpan().SequenceEqual(expected))throw new Exception("Actual wallpaper opaque/post-gray/GPU mismatch case"+c.GetProperty("nativeIndex").GetInt32());string file="prepared-wallpaper-"+cases.Count+".bgra";File.WriteAllBytes(Path.Combine(directory,file),bytes);cases.Add(new(c.GetProperty("nativeIndex").GetInt32(),receipt,Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant(),file));pixels+=w*h;
  }
  renderer.Dispose();var stats=renderer.Stats;if(cases.Count!=20||pixels!=194272||rejected!=1||pending!=1||stats.InFlight!=0||stats.LiveTextures!=0||stats.LiveTargets!=0||stats.LiveVertexBuffers!=0||stats.LiveSamplers!=0||stats.LastError is not null)throw new Exception("Wallpaper GPU coverage/resource retirement");File.WriteAllText(Path.Combine(directory,"prepared-wallpaper-gpu.json"),JsonSerializer.Serialize(new PreparedWallpaperGpuReport(1,cases,pixels,rejected,pending,stats),PreparedWallpaperGpuContext.Default.PreparedWallpaperGpuReport));return "PASS actual prepared wallpaper GPU:20 decoded source/HALFTONE24/post-gray/upload cases/194272 exact opaque pixels/1 retained-image failure guard; pending-frame resources retired, source codecs/cache/vendor/PC/physical separate.";
 }
}
public sealed record PreparedWallpaperGpuCase(int NativeIndex,Rhino5WallpaperRasterReceipt Receipt,string BgraSha256,string File);
public sealed record PreparedWallpaperGpuReport(int SchemaVersion,List<PreparedWallpaperGpuCase> Cases,int ExactPixels,int RejectedSourceReplacements,int DisposalInFlight,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(PreparedWallpaperGpuReport))]
internal partial class PreparedWallpaperGpuContext:JsonSerializerContext{}
