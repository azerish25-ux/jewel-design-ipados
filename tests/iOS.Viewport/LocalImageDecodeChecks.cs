using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace CandidateAppleImageChecks;
public static class LocalImageDecodeChecks
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Fail(Action a){checks++;try{a();}catch(InvalidDataException){return;}throw new Exception("Expected explicit invalid-image/budget failure");}
  var assembly=typeof(CandidateGeometryContractChecks.LocalImageAssetAssertions).Assembly;
  using var input=assembly.GetManifestResourceStream("LocalImageDecodeFixtures")??throw new Exception("Image fixtures missing");
  using var doc=JsonDocument.Parse(input);var root=doc.RootElement;
  foreach(string name in new[]{"png","bmp"})
  {
   var f=root.GetProperty(name);var bytes=Convert.FromBase64String(f.GetProperty("base64").GetString()!);
   var asset=LocalImageAsset.FromBytes(@"C:\original\"+name,bytes);
   using var image=AppleLocalImageDecoder.Decode(asset);
   Check(image.Width==f.GetProperty("width").GetInt32()&&image.Height==f.GetProperty("height").GetInt32(),name+" actual dimensions");
   byte[] expected=f.GetProperty("rawRgba").EnumerateArray().Select(x=>(byte)x.GetInt32()).ToArray();
   var actual=image.CopyRgba();Check(actual.AsSpan().SequenceEqual(expected),name+" raw top-row-first premultiplied RGBA pixels, no EXIF auto rotation: "+Convert.ToHexString(actual));
   Check(image.SourceFrameCount==1&&image.FrameIndex==0&&!string.IsNullOrEmpty(image.TypeIdentifier),name+" concrete source frame/type");
   Check(image.Source.CopyEncodedBytes().AsSpan().SequenceEqual(bytes),name+" original metadata/bytes retained");
   if(name=="png")Check(image.SourceOrientation==6,"PNG EXIF orientation retained without applying it");
   var copy=image.CopyRgba();copy[0]=33;Check(image.CopyRgba()[0]==expected[0],"No mutable output alias");
   image.Dispose();image.Dispose();checks++;
   try{image.CopyRgba();throw new Exception("Disposed decoded image still accessible");}catch(ObjectDisposedException){}
   Fail(()=>{using var rejected=AppleLocalImageDecoder.Decode(asset,new(MaxPixels:1));});
   Fail(()=>{using var rejected=AppleLocalImageDecoder.Decode(asset,new(MaxDecodedBytes:1));});
   Fail(()=>{using var rejected=AppleLocalImageDecoder.Decode(asset,new(MaxEncodedBytes:1));});
   Fail(()=>{using var rejected=AppleLocalImageDecoder.Decode(LocalImageAsset.FromBytes("truncated",bytes.AsSpan(0,8)));});
  }
  using var jpegStream=assembly.GetManifestResourceStream("RecoveredImageDecodeJpeg")??throw new Exception("Recovered JPEG missing");
  using var memory=new MemoryStream();jpegStream.CopyTo(memory);
  var jpegAsset=LocalImageAsset.FromBytes("recovered-image.jpg",memory.ToArray());var j=root.GetProperty("jpeg");
  using(var decoded=AppleLocalImageDecoder.Decode(jpegAsset))
  {
   Check(decoded.Width==j.GetProperty("width").GetInt32()&&decoded.Height==j.GetProperty("height").GetInt32(),"Actual unchanged recovered JPEG decoded");
   Check(jpegAsset.ContentSha256==j.GetProperty("sha256").GetString(),"Recovered JPEG hash/provenance");
   var bytes=decoded.CopyRgba();bool opaque=true,nonuniform=false;for(int i=0;i<bytes.Length;i+=4){opaque&=bytes[i+3]==255;if(i>0)nonuniform|=bytes[i]!=bytes[0]||bytes[i+1]!=bytes[1]||bytes[i+2]!=bytes[2];}
   Check(opaque&&nonuniform,"Real JPEG produces actual nonuniform opaque pixels");
  }
  Fail(()=>{using var rejected=AppleLocalImageDecoder.Decode(LocalImageAsset.FromBytes("invalid",[1,2,3,4,5]));});
  for(int i=0;i<8;i++){using var image=AppleLocalImageDecoder.Decode(LocalImageAsset.FromBytes("repeat",Convert.FromBase64String(root.GetProperty("png").GetProperty("base64").GetString()!)));Check(image.CopyRgba().Length==16,"Repeated native decode/dispose");}
  return "PASS actual offline Apple image decode:"+checks+" PNG/BMP/JPEG pixel/alpha/orientation/budget/corrupt/disposal cases; LEADTOOLS/GDI/Metal/SetView/PC parity separate.";
 }
}
