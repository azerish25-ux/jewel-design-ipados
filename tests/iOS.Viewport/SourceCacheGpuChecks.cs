using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class SourceCacheGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  using var stream=typeof(CandidateGeometryContractChecks.ComposedTextureAssertions).Assembly.GetManifestResourceStream("ComposedTextureGpuFixture")??throw new Exception("Source cache PNG fixture missing");using var fixture=JsonDocument.Parse(stream);var items=fixture.RootElement.GetProperty("cases").EnumerateArray().ToArray();var color=items.Single(c=>c.GetProperty("nativeIndex").GetInt32()==1);var gray=items.Single(c=>c.GetProperty("nativeIndex").GetInt32()==4);byte[] png=Convert.FromBase64String(color.GetProperty("pngBase64").GetString()!);int resolves=0,failures=0;DecodedLocalImage Resolve(){resolves++;return AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("Ring.PNG",png));}
  byte[] Expected(JsonElement c){var bytes=Convert.FromBase64String(c.GetProperty("OutputRgbaBase64").GetString()!);for(int i=0;i<bytes.Length;i+=4){byte r=bytes[i];bytes[i]=bytes[i+2];bytes[i+2]=r;}return bytes;}
  byte[] wantedColor=Expected(color),wantedGray=Expected(gray);using var renderer=new MetalImageQuadRenderer();renderer.ConfigureSourceCache(Rhino5FilenameSemantics.CrtCLocale,4);renderer.SetSourceCacheUsage(9);var settings=new Rhino5TextureRasterSettings(16,1000000,1f,false);var marks=new List<SourceCacheGpuFrame>();
  async Task Record(string stage,byte[] expected,MetalImageQuadRenderer.ImageFrame? pending=null){byte[] pixels;if(pending is null){using var frame=renderer.PrepareFrame(3,2);pixels=await frame.CommitAsync();}else pixels=await pending.CommitAsync();if(!pixels.AsSpan().SequenceEqual(expected))throw new Exception("Actual cached GPU content mismatch "+stage);string file="source-cache-"+marks.Count+".bgra";File.WriteAllBytes(Path.Combine(directory,file),pixels);marks.Add(new(stage,Convert.ToHexString(SHA256.HashData(pixels)).ToLowerInvariant(),file));}
  renderer.SetCachedTextureSource("Ring.PNG",Resolve,24,settings);await Record("created-color",wantedColor);long uploads=renderer.Stats.TextureUploads;renderer.SetCachedTextureSource("ring.png",()=>throw new Exception("Cache hit resolved source"),24,settings);await Record("cached-color",wantedColor);if(resolves!=1||uploads!=1||renderer.Stats.TextureUploads!=1||renderer.SourceCacheEntries!=1)throw new Exception("Cached source re-decoded/reuploaded");
  try{renderer.SetCachedTextureSource("ring.png",()=>throw new InvalidDataException("Missing gray source"),24,settings,true);throw new Exception("Expected gray miss failure");}catch(InvalidDataException){failures++;}await Record("failed-gray-retained-color",wantedColor);
  renderer.SetCachedTextureSource("RING.PNG",Resolve,24,settings,true);await Record("created-gray",wantedGray);if(resolves!=2||renderer.SourceCacheEntries!=2||renderer.Stats.TextureUploads!=2||renderer.Stats.LiveTextures!=2)throw new Exception("Color/gray cache alias/ownership");
  using(var other=new MetalImageQuadRenderer()){other.ConfigureSourceCache(Rhino5FilenameSemantics.CrtCLocale);other.SetCachedTextureSource("ring.png",Resolve,24,settings);if(other.Stats.TextureUploads!=1||other.SourceCacheEntries!=1)throw new Exception("Other rendering context reused cache");other.Dispose();if(other.Stats.LiveTextures!=0)throw new Exception("Other context cache leaked");}
  try{renderer.SetCachedTextureSource("broken.png",()=>throw new InvalidDataException("Decode failed"),24,settings);throw new Exception("Expected source miss failure");}catch(InvalidDataException){failures++;}await Record("failed-source-retained-gray",wantedGray);
  using(var pending=renderer.PrepareFrame(3,2)){if(renderer.InvalidateSource("rInG.pNg")!=2||renderer.SourceCacheEntries!=0)throw new Exception("Explicit invalidation did not remove both kinds");renderer.SetCachedTextureSource("ring.png",Resolve,24,settings);await Record("invalidated-old-gray-pending",wantedGray,pending);}await Record("reloaded-color",wantedColor);
  int inFlight;using(var pending=renderer.PrepareFrame(3,2)){inFlight=renderer.Stats.InFlight;renderer.Dispose();await Record("disposed-pending-color",wantedColor,pending);}var stats=renderer.Stats;
  if(marks.Count!=8||resolves!=4||failures!=2||inFlight!=1||stats.TextureUploads!=3||renderer.SourceCacheEntries!=0||stats.InFlight!=0||stats.LiveTextures!=0||stats.LiveTargets!=0||stats.LiveVertexBuffers!=0||stats.LiveSamplers!=0||stats.LastError is not null)throw new Exception("Actual cache hit/reload/queued lifetime counters");File.WriteAllText(Path.Combine(directory,"source-cache-gpu.json"),JsonSerializer.Serialize(new SourceCacheGpuReport(1,marks,resolves,failures,1,inFlight,stats),SourceCacheGpuContext.Default.SourceCacheGpuReport));return "PASS actual source cache GPU:8 exact image readbacks/1 decode-and-upload-free hit/2 retained-image failures/2 invalidated color kinds/1 independent context/1 queued-disposal frame; explicit CRT-C policy, NLS/document/PC/physical remain open.";
 }
}
public sealed record SourceCacheGpuFrame(string Stage,string BgraSha256,string File);
public sealed record SourceCacheGpuReport(int SchemaVersion,List<SourceCacheGpuFrame> Frames,int SourceResolves,int SourceFailures,int IndependentContexts,int DisposalInFlight,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(SourceCacheGpuReport))]
internal partial class SourceCacheGpuContext:JsonSerializerContext{}
