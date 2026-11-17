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
public static class LiveRuntimeSourceCacheGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  using var stream=typeof(CandidateGeometryContractChecks.ComposedTextureAssertions).Assembly.GetManifestResourceStream("ComposedTextureGpuFixture")??throw new Exception("Native image fixture missing");using var document=JsonDocument.Parse(stream);var cases=document.RootElement.GetProperty("cases").EnumerateArray().ToArray();var a=cases.Single(c=>c.GetProperty("nativeIndex").GetInt32()==1);var b=cases.Single(c=>c.GetProperty("nativeIndex").GetInt32()==61);
  var runtime=new Rhino5StringRuntimeProfile(Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.C));using var renderer=new MetalImageQuadRenderer();renderer.ConfigureSourceCache(runtime.FilenameSemantics,16);var transitions=new List<LiveRuntimeTransition>();var frames=new List<LiveRuntimeGpuFrame>();int resolves=0,hits=0,failures=0,removed=0,pendingFrames=0;bool prefixOriginal=false;
  void Set(uint requested,uint ansi){uint previous=runtime.SetWindowsLocaleId(requested,ansi);var state=runtime.State;transitions.Add(new(requested,ansi,previous,state.LocaleFlag,state.UnicodeFlag));}
  JsonElement Fixture(int index)=>index==1?a:b;
  Rhino5TextureRasterSettings Settings(int index){var c=Fixture(index).GetProperty("Settings");return new(c.GetProperty("DeviceMaximum").GetInt32(),c.GetProperty("MemoryBudget").GetUInt32(),c.GetProperty("MemoryFraction").GetSingle(),c.GetProperty("PowerOfTwo").GetBoolean());}
  DecodedLocalImage Resolve(int index,string original){resolves++;byte[] png=Convert.FromBase64String(Fixture(index).GetProperty("pngBase64").GetString()!);return AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes(original,png));}
  void Load(string name,int index){renderer.SetCachedTextureSource(name,()=>Resolve(index,name),24,Settings(index));}
  void Hit(string name,int index){long before=renderer.Stats.TextureUploads;int decoded=resolves;renderer.SetCachedTextureSource(name,()=>throw new Exception("Live profile cache hit resolved image "+name),24,Settings(index));if(renderer.Stats.TextureUploads!=before||resolves!=decoded)throw new Exception("Live profile hit decoded/uploaded");hits++;}
  byte[] Expected(int index){byte[] rgba=Convert.FromBase64String(Fixture(index).GetProperty("OutputRgbaBase64").GetString()!);for(int p=0;p<rgba.Length;p+=4){byte red=rgba[p];rgba[p]=rgba[p+2];rgba[p+2]=red;}return rgba;}
  async Task Record(string stage,int index,MetalImageQuadRenderer.ImageFrame? pending=null){var c=Fixture(index);int width=c.GetProperty("Width").GetInt32(),height=c.GetProperty("Height").GetInt32();byte[] raw;if(pending is null){using var frame=renderer.PrepareFrame(width,height);raw=await frame.CommitAsync();}else raw=await pending.CommitAsync();if(!raw.AsSpan().SequenceEqual(Expected(index)))throw new Exception("Live profile selected wrong GPU image "+stage);string file="live-string-cache-"+frames.Count+".bgra";File.WriteAllBytes(Path.Combine(directory,file),raw);var state=runtime.State;frames.Add(new(stage,index,width,height,file,Convert.ToHexStringLower(SHA256.HashData(raw)),renderer.Stats.TextureUploads,resolves,renderer.SourceCacheEntries,state.LocaleFlag,state.UnicodeFlag));}
  Load("ß.PNG",1);await Record("crt-sharp-a",1);
  Set(1033,0);Hit("ss.png",1);await Record("unicode-alias-a",1);
  Set(0,0);Load("ss.png",61);await Record("crt-new-ss-b",61);
  Set(1033,1);Set(1033,0);Hit("SS.PNG",61);await Record("same-request-reset-b",61);
  Load("Z.PNG",1);await Record("unicode-insert-z-a",1);Hit("ss.png",61);await Record("unicode-native-tie-b",61);
  using(var pending=renderer.PrepareFrame(8,8)){pendingFrames++;int count=renderer.InvalidateSource("SS.PNG");if(count!=2)throw new Exception("Unicode alias invalidation did not remove both source names");removed+=count;Load("W.PNG",1);await Record("pending-b-after-invalidate",61,pending);}await Record("replaced-with-w-a",1);
  removed+=renderer.InvalidateSource("W.PNG");removed+=renderer.InvalidateSource("Z.PNG");if(removed!=4||renderer.SourceCacheEntries!=0)throw new Exception("Explicit runtime phase clear failed");
  Set(1033,1);string high0=char.ConvertFromUtf32(0x10c000)+".PNG",high1=char.ConvertFromUtf32(0x10c001)+".PNG";Load(high0,1);await Record("ansi-first-a",1);Load(high1,61);await Record("ansi-second-b",61);
  try{renderer.SetCachedTextureSource("broken.png",()=>throw new InvalidDataException("Intentional resolver failure"),24,Settings(1));throw new Exception("Expected source resolver failure");}catch(InvalidDataException){failures++;}await Record("failed-resolve-retained-b",61);
  Hit("\ufffd.png",61);await Record("ansi-decoded-native-tie-b",61);
  string original="Lead.PNG\0TAIL";renderer.SetCachedTextureSource(original,()=>{var decoded=Resolve(1,original);prefixOriginal=decoded.Source.OriginalName=="Lead.PNG\0TAIL";return decoded;},24,Settings(1));await Record("c-prefix-resolver-a",1);Hit("LEAD.PNG\0OTHER",1);await Record("c-prefix-alias-a",1);
  Load("\0TAIL",61);await Record("leading-nul-b",61);Hit("",61);await Record("empty-prefix-alias-b",61);
  using(var pending=renderer.PrepareFrame(8,8)){pendingFrames++;renderer.Dispose();await Record("disposed-pending-b",61,pending);}
  var stats=renderer.Stats;if(frames.Count!=17||resolves!=8||hits!=6||failures!=1||pendingFrames!=2||stats.TextureUploads!=8||stats.InFlight!=0||stats.LiveTextures!=0||stats.LiveTargets!=0||stats.LiveVertexBuffers!=0||stats.LiveSamplers!=0||stats.LastError is not null||renderer.SourceCacheEntries!=0)throw new Exception("Live runtime cache totals/resource retirement mismatch");
  var report=new LiveRuntimeGpuReport(1,frames,transitions,1,resolves,hits,failures,removed,pendingFrames,prefixOriginal,stats);File.WriteAllText(Path.Combine(directory,"live-string-cache-gpu.json"),JsonSerializer.Serialize(report,LiveRuntimeGpuContext.Default.LiveRuntimeGpuReport));
  return "PASS live string runtime GPU:17 native image readbacks/682 exact pixels/5 profile transitions/6 decode-upload-free hits/2 retained pending frames; full document/PC/physical acceptance separate.";
 }
}
public sealed record LiveRuntimeTransition(uint Requested,uint UseAnsi,uint Previous,uint LocaleFlag,uint UnicodeFlag);
public sealed record LiveRuntimeGpuFrame(string Stage,int NativeIndex,int Width,int Height,string File,string BgraSha256,long Uploads,int Resolves,int CacheEntries,uint LocaleFlag,uint UnicodeFlag);
public sealed record LiveRuntimeGpuReport(int SchemaVersion,List<LiveRuntimeGpuFrame> Frames,List<LiveRuntimeTransition> Transitions,int ConfigurationCount,int SourceResolves,int AliasHits,int SourceFailures,int RemovedByInvalidation,int PendingFrames,bool PrefixResolverPreservedProvenance,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(LiveRuntimeGpuReport))]
internal partial class LiveRuntimeGpuContext:JsonSerializerContext{}
