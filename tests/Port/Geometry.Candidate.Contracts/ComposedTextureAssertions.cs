using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class ComposedTextureAssertions
{
 public static string Run(){int exact=0,grayOrder=0,alphaOrder=0,guards=0;using var stream=typeof(ComposedTextureAssertions).Assembly.GetManifestResourceStream("GdiComposedTextureReference")??throw new Exception("Composed texture fixture missing");using var doc=JsonDocument.Parse(stream);var root=doc.RootElement;
  foreach(var c in root.GetProperty("Cases").EnumerateArray()){
   int sw=c.GetProperty("SourceWidth").GetInt32(),sh=c.GetProperty("SourceHeight").GetInt32(),depth=c.GetProperty("Depth").GetInt32();bool gray=c.GetProperty("Gray").GetBoolean();var s=c.GetProperty("Settings");var settings=new Rhino5TextureRasterSettings(s.GetProperty("DeviceMaximum").GetInt32(),s.GetProperty("MemoryBudget").GetUInt32(),s.GetProperty("MemoryFraction").GetSingle(),s.GetProperty("PowerOfTwo").GetBoolean());byte[] input=Convert.FromBase64String(c.GetProperty("SourceStoredBase64").GetString()!),copy=(byte[])input.Clone(),expected=Convert.FromBase64String(c.GetProperty("OutputRgbaBase64").GetString()!);
   using var prepared=Rhino5TextureRasterPreparation.PrepareStored(input,sw,sh,depth,settings,gray);
   if(prepared.Width!=c.GetProperty("Width").GetInt32()||prepared.Height!=c.GetProperty("Height").GetInt32()||prepared.StretchMode!=c.GetProperty("Mode").GetInt32()||prepared.GrayScale!=gray||prepared.DibBitDepth!=depth)throw new Exception("Preparation metadata/native mode");
   if(!prepared.CopyStored().AsSpan().SequenceEqual(Convert.FromBase64String(c.GetProperty("OutputStoredBase64").GetString()!))||!prepared.CopyRgba().AsSpan().SequenceEqual(expected))throw new Exception("Composed native pixels/alpha/padding mismatch "+sw+"x"+sh+" b"+depth+" gray"+gray);
   if(!input.AsSpan().SequenceEqual(copy))throw new Exception("Native source mutated");var independent=prepared.CopyRgba();independent[0]^=255;if(!prepared.CopyRgba().AsSpan().SequenceEqual(expected))throw new Exception("Result alias exposed");
   using var decoded=new DecodedLocalImage(LocalImageAsset.FromBytes("native-source",input),sw,sh,3,2,"native-reference",Convert.FromBase64String(c.GetProperty("SourceRgbaBase64").GetString()!),RgbaAlphaRepresentation.Straight);
   using var fromDecoded=Rhino5TextureRasterPreparation.Prepare(decoded,depth,settings,gray);using var image=fromDecoded.CreateDecodedImage(decoded);
   if(!image.CopyRgba().AsSpan().SequenceEqual(expected)||image.Source!=decoded.Source||image.SourceOrientation!=3||image.SourceFrameCount!=2||image.TypeIdentifier!="native-reference"||image.AlphaRepresentation!=RgbaAlphaRepresentation.Straight||image.PixelSource!=DecodedPixelSource.PreparedTexture)throw new Exception("Decoded source/provenance/typed output mismatch");
   grayOrder+=c.GetProperty("GrayAfterRgbDifferences").GetInt32()>0?1:0;alphaOrder+=c.GetProperty("OriginalAlphaDifferences").GetInt32()>0?1:0;exact++;
  }
  void Reject<T>(Action action)where T:Exception{guards++;try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  var standard=new Rhino5TextureRasterSettings(16,1000000,1f,false);
  Reject<NotSupportedException>(()=>Rhino5TextureRasterPreparation.PrepareStored([],1,1,16,standard));
  Reject<ArgumentException>(()=>Rhino5TextureRasterPreparation.PrepareStored([],1,1,24,standard));
  Reject<InvalidDataException>(()=>Rhino5TextureRasterPreparation.PrepareStored([],int.MaxValue,1,24,standard));
  Reject<InvalidDataException>(()=>Rhino5TextureRasterPreparation.PrepareStored(new byte[4],1,1,32,new(1,0,1f,false)));
  Reject<InvalidDataException>(()=>Rhino5TextureRasterPreparation.PrepareStored(new byte[4],1,1,32,standard,false,null,1));
  using var premultiplied=new DecodedLocalImage(LocalImageAsset.FromBytes("premul",[1]),1,1,1,1,"raw",[10,20,30,40]);Reject<NotSupportedException>(()=>Rhino5TextureRasterPreparation.Prepare(premultiplied,32,standard));
  using var alpha=new DecodedLocalImage(LocalImageAsset.FromBytes("straight",[1]),1,1,1,1,"raw",[10,20,30,40],RgbaAlphaRepresentation.Straight);Reject<NotSupportedException>(()=>Rhino5TextureRasterPreparation.Prepare(alpha,24,standard));
  var disposed=Rhino5TextureRasterPreparation.PrepareStored(new byte[4],1,1,32,standard);disposed.Dispose();Reject<ObjectDisposedException>(()=>disposed.CopyRgba());Reject<ObjectDisposedException>(()=>disposed.CopyStored());disposed.Dispose();
  using var provenance=Rhino5TextureRasterPreparation.PrepareStored(new byte[4],1,1,32,standard);Reject<ArgumentException>(()=>provenance.CreateDecodedImage(alpha));
  if(exact!=120||grayOrder<1||alphaOrder<1||root.GetProperty("LiveOwnedHandles").GetInt32()!=0)throw new Exception("Incomplete composed source/order controls");return "PASS composed texture preparation:"+exact+" exact native stored/RGBA and decoded-adapter cases/"+grayOrder+" gray-order distinctions/"+alphaOrder+" processed-alpha distinctions/"+guards+" ownership/storage/depth/budget guards; source cache/codecs/vendor/PC/physical acceptance open.";
 }
}
