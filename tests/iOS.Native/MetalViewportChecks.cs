using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreGraphics;
using Foundation;
using UIKit;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace NativeGeometryCandidate;
internal sealed record MetalFrameReport(int SchemaVersion,string Device,int Width,int Height,int SourceTriangles,int EditedTriangles,int ForegroundPixels,int EditedPixels,int OrbitPixels,int OrderPixels,string OriginalBgraSha256,string Scope);
[JsonSourceGenerationOptions(WriteIndented=true,PropertyNamingPolicy=JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(MetalFrameReport))]
internal partial class MetalFrameReportContext:JsonSerializerContext{}
internal static class MetalViewportChecks
{
 private static void Check(bool value,string message){if(!value)throw new Exception(message);}
 private static int Different(byte[] a,byte[] b){int count=0;for(int i=0;i<a.Length;i+=4)if(a[i]!=b[i]||a[i+1]!=b[i+1]||a[i+2]!=b[i+2])count++;return count;}
 private static void Save(byte[] bytes,int width,int height,string path){using var color=CGColorSpace.CreateDeviceRGB();using var provider=new CGDataProvider(bytes);using var image=new CGImage(width,height,8,32,width*4,color,CGBitmapFlags.ByteOrder32Little|CGBitmapFlags.PremultipliedFirst,provider,null,false,CGColorRenderingIntent.Default);using var ui=UIImage.FromImage(image);using var png=ui.AsPNG()??throw new Exception("GPU PNG encoding failed");if(!png.Save(path,NSDataWritingOptions.Atomic,out var error))throw new Exception("GPU PNG save failed: "+error);}
 public static string Run(string folder){const int width=320,height=320;using var stream=typeof(CandidateGeometryContractChecks.Assertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;using var memory=new MemoryStream();stream.CopyTo(memory);using var context=CandidateContext.Create();using var graph=new CandidateRailGraphSession(context,LegacyCsg4Codec.Read(memory.ToArray()),new(1e-7,1e-7,1e-7));using var renderer=new CadMetalRenderer();graph.Generate(140);using var shape=graph.RetainBrep(140);var source=context.TessellateBrep(shape,.04,.35);var upload=CadViewportMesh.FromSnapshot(source);
  var original=renderer.RenderBgra(upload,width,height);var orbit=renderer.RenderBgra(upload,width,height,-.7f,.2f);var reordered=renderer.RenderBgra(upload,width,height,.6f,.35f,true);
  graph.Values.SetValue(2,graph.Values.Document.Records[2].Scalar!.Value+.4);graph.Values.SetValue(136,.8);graph.Generate(140);using var changed=graph.RetainBrep(140);var changedSource=context.TessellateBrep(changed,.04,.35);var edited=renderer.RenderBgra(CadViewportMesh.FromSnapshot(changedSource),width,height);
  using var box=context.CreateBox(1,1,1);using var far=context.Translate(box,10,0,0);using var empty=context.Boolean(CandidateBooleanOperation.Common,box,far,0);var clear=renderer.RenderBgra(CadViewportMesh.FromSnapshot(context.TessellateBrep(empty,.04,.35)),width,height);
  for(int i=0;i<clear.Length;i+=4)Check(clear[i]==clear[0]&&clear[i+1]==clear[1]&&clear[i+2]==clear[2]&&clear[i+3]==255,"Empty GPU frame has scene content");int foreground=Different(original,clear),editedPixels=Different(original,edited),orbitPixels=Different(original,orbit),orderPixels=Different(original,reordered);Check(foreground>100,"Actual CAD GPU frame is empty");Check(editedPixels>100&&orbitPixels>100,"CAD edit/orbit did not change GPU image");Check(orderPixels<=32,"Depth-tested image depends on triangle submission order");
  Directory.CreateDirectory(folder);Save(original,width,height,Path.Combine(folder,"native-metal-original.png"));Save(edited,width,height,Path.Combine(folder,"native-metal-edited.png"));Save(orbit,width,height,Path.Combine(folder,"native-metal-orbit.png"));Save(clear,width,height,Path.Combine(folder,"native-metal-empty.png"));var report=new MetalFrameReport(1,renderer.DeviceName,width,height,source.Triangles.Count,changedSource.Triangles.Count,foreground,editedPixels,orbitPixels,orderPixels,Convert.ToHexString(SHA256.HashData(original)).ToLowerInvariant(),"Metal offscreen actual CAD proof; no full UI/material/source-camera/original-render/physical parity");File.WriteAllText(Path.Combine(folder,"native-metal-report.json"),JsonSerializer.Serialize(report,MetalFrameReportContext.Default.MetalFrameReport));return "PASS Metal offscreen viewport:actual-CAD pixels="+foreground+" edited="+editedPixels+" orbit="+orbitPixels+" depth-order="+orderPixels+"; no CPU/preview fallback, full UI/render parity separate.";
 }
}
