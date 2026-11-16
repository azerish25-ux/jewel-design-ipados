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
public static class TextureAnisotropyGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  using var renderer=new MetalImageQuadRenderer();byte[] green=[0,255,0,255];
  for(int i=1;i<=16;i++){
   renderer.SetImage(1,1,green,Rhino5TextureAnisotropy.ForTextureMode(3,true,i));
   using var f=renderer.PrepareFrame(3,2,filtered:i%2==0);var b=await f.CommitAsync();
   Check(f.MaximumAnisotropy==i&&Enumerable.Range(0,6).All(p=>b[p*4]==0&&b[p*4+1]==255&&b[p*4+2]==0&&b[p*4+3]==255),"Actual descriptor/command/readback at anisotropy"+i);
  }
  Check(renderer.Stats.LiveSamplers==32,"Sampler cache bounded to sixteen nearest/linear pairs");
  int retained=renderer.Stats.LiveSamplers;
  try{renderer.SetImage(1,1,[255,0,0,255],Rhino5TextureAnisotropy.ForTextureMode(3,true,1.5f));throw new Exception("Expected fractional rejection");}catch(NotSupportedException){}
  using(var f=renderer.PrepareFrame(1,1)){var b=await f.CommitAsync();Check(f.MaximumAnisotropy==16&&b[1]==255&&renderer.Stats.LiveSamplers==retained,"Unsupported replacement preserves previous real image/sampler");}
  // Measured anisotropic-footprint output, not a guessed legacy-driver reference.
  byte[] pattern=new byte[1024*8*4];for(int y=0;y<8;y++)for(int x=0;x<1024;x++){int o=(y*1024+x)*4;pattern[o]=(byte)((x%7)*37);pattern[o+1]=(byte)((x*73+y*41)&255);pattern[o+2]=(byte)((x*29+y*101)&255);pattern[o+3]=255;}
  byte[]? baseline=null;var probes=new List<AnisotropyProbe>();
  foreach(int maximum in new[]{1,4,8,16}){
   renderer.SetImage(1024,8,pattern,Rhino5TextureAnisotropy.ForTextureMode(3,true,maximum));using var f=renderer.PrepareFrame(64,64,filtered:true);var b=await f.CommitAsync();baseline??=b;int changed=0;for(int i=0;i<b.Length;i+=4)if(!b.AsSpan(i,4).SequenceEqual(baseline.AsSpan(i,4)))changed++;File.WriteAllBytes(Path.Combine(directory,"anisotropy-"+maximum+".bgra"),b);probes.Add(new(maximum,changed,Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant()));
  }
  renderer.SetImage(1,1,[255,0,0,255],Rhino5TextureAnisotropy.ForTextureMode(1,true,16));using var a=renderer.PrepareFrame(1,1);
  renderer.SetImage(1,1,green,Rhino5TextureAnisotropy.ForTextureMode(2,true,16));using var c=renderer.PrepareFrame(1,1);
  renderer.SetImage(1,1,[0,0,255,255],Rhino5TextureAnisotropy.ForTextureMode(3,true,16));using var d=renderer.PrepareFrame(1,1);
  Check(a.MaximumAnisotropy==4&&c.MaximumAnisotropy==8&&d.MaximumAnisotropy==16,"Frame leases capture source-selected sampler");
  renderer.Dispose();Check(renderer.Stats.LiveSamplers==32&&!renderer.Stats.PipelineReleased,"Native samplers retained through pending commands/disposal");
  var outputs=await Task.WhenAll(a.CommitAsync(),c.CommitAsync(),d.CommitAsync());Check(outputs[0][2]==255&&outputs[1][1]==255&&outputs[2][0]==255,"Actual differently sampled revisions survive replacement/disposal");
  Check(renderer.Stats.LiveSamplers==0&&renderer.Stats.LiveTextures==0&&renderer.Stats.LiveTargets==0&&renderer.Stats.LiveVertexBuffers==0&&renderer.Stats.InFlight==0&&renderer.Stats.PipelineReleased&&renderer.Stats.LastError is null,"All sampler/image/frame resources retire");
  File.WriteAllText(Path.Combine(directory,"anisotropy-gpu.json"),JsonSerializer.Serialize(new AnisotropyGpuReport(1,checks,probes,renderer.Stats,"Actual Metal sampler descriptors/encoded frames/readback and lifetime. Footprint probe is measured only, not an original GL-driver oracle. Source mipmaps/compression/cache/camera/PC/physical parity remain separate."),AnisotropyGpuReportContext.Default.AnisotropyGpuReport));
  return "PASS actual texture anisotropy GPU:"+checks+" descriptor/readback/failure-preservation/retained-sampler cases; original driver/filtering/PC parity separate.";
 }
}
public sealed record AnisotropyProbe(int Maximum,int ChangedPixels,string BgraSha256);
public sealed record AnisotropyGpuReport(int SchemaVersion,int Checks,List<AnisotropyProbe> Probes,MetalImageQuadStats Stats,string Scope);
[JsonSourceGenerationOptions(WriteIndented=true)]
[JsonSerializable(typeof(AnisotropyGpuReport))]
internal partial class AnisotropyGpuReportContext:JsonSerializerContext{}
