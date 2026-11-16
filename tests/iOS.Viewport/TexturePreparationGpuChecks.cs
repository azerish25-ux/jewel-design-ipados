using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class TexturePreparationGpuChecks
{
 public static async Task<string> Run(string directory)
 {
  byte[] source=new byte[48],target=new byte[24];
  byte[] alpha=[1,2,4,8,16,32,64,128,3,5,9,17];
  for(int i=0;i<12;i++)source[i*4+3]=alpha[i];
  for(int i=0;i<6;i++){target[i*4]=21;target[i*4+1]=43;target[i*4+2]=65;target[i*4+3]=128;}
  Rhino5TexturePreparation.RepairAlphaTopDownRgba(source,4,3,target,3,2);
  byte[] wantedAlpha=[144,160,192,131,133,137];
  if(!Enumerable.Range(0,6).All(i=>target[i*4+3]==wantedAlpha[i]))throw new Exception("Original alpha repair adapter differs");
  using var renderer=new MetalImageQuadRenderer();renderer.SetImage(3,2,target);
  byte[] pixels;using(var frame=renderer.PrepareFrame(3,2)){pixels=await frame.CommitAsync();}
  for(int i=0;i<6;i++)if(pixels[i*4]!=65||pixels[i*4+1]!=43||pixels[i*4+2]!=21||pixels[i*4+3]!=wantedAlpha[i])throw new Exception("Repaired straight alpha did not survive actual GPU upload/sample/readback");
  renderer.Dispose();if(renderer.Stats.InFlight!=0||renderer.Stats.LiveTextures!=0||renderer.Stats.LiveTargets!=0||renderer.Stats.LiveVertexBuffers!=0||renderer.Stats.LastError is not null)throw new Exception("Image preparation GPU retained native resources");
  File.WriteAllBytes(Path.Combine(directory,"texture-preparation-gpu.bgra"),pixels);
  File.WriteAllText(Path.Combine(directory,"texture-preparation-gpu.json"),JsonSerializer.Serialize(new PreparationGpuReport(1,6,wantedAlpha,renderer.Stats),PreparationGpuReportContext.Default.PreparationGpuReport));
  return "PASS actual texture preparation GPU:6 original alpha-OR/row-mapping pixels with RGB preserved and owned resources retired; source RGB filtering/PC parity separate.";
 }
}
public sealed record PreparationGpuReport(int SchemaVersion,int Pixels,byte[] Alpha,MetalImageQuadStats Stats);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(PreparationGpuReport))]
internal partial class PreparationGpuReportContext:JsonSerializerContext{}
