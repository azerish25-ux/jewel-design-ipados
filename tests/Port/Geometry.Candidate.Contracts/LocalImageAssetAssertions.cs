using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class LocalImageAssetAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action a) where T:Exception {checks++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  byte[] source=[1,2,3];var asset=LocalImageAsset.FromBytes(@"C:\original\trace.png",source);
  source[0]=9;Check(asset.CopyEncodedBytes()[0]==1,"Import owns bytes");
  var copy=asset.CopyEncodedBytes();copy[1]=9;Check(asset.CopyEncodedBytes()[1]==2,"Export does not leak mutable source");
  Check(asset.OriginalName==@"C:\original\trace.png"&&asset.EncodedLength==3,"Original Windows filename preserved as inert metadata");
  Check(asset.ContentSha256=="039058c6f2c0cb492c533b0a4d14ef77cc0f78abccced5287d84a1a2011cfb81","Content identity");
  Expect<ArgumentException>(()=>LocalImageAsset.FromBytes("",source));
  Expect<InvalidDataException>(()=>LocalImageAsset.FromBytes("empty",[]));
  Expect<InvalidDataException>(()=>LocalImageAsset.FromBytes("large",source,new(MaxEncodedBytes:2)));
  Expect<ArgumentOutOfRangeException>(()=>LocalImageAsset.FromBytes("bad",source,new(MaxEncodedBytes:0)));
  Expect<ArgumentException>(()=>LocalImageAsset.ImportFile("name","relative.png"));
  var temp=Path.Combine(Path.GetTempPath(),"jewel-image-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
  try
  {
   string path=Path.Combine(temp,"image.bin");File.WriteAllBytes(path,source);
   var file=LocalImageAsset.ImportFile("requested/path.jpg",path);
   Check(file.CopyEncodedBytes().AsSpan().SequenceEqual(source)&&file.OriginalName=="requested/path.jpg","Concrete local file import, preserved caller name");
   Expect<InvalidDataException>(()=>LocalImageAsset.ImportFile("file",path,new(MaxEncodedBytes:2)));
   Expect<FileNotFoundException>(()=>LocalImageAsset.ImportFile("missing",Path.Combine(temp,"missing.png")));
  }
  finally {File.Delete(Path.Combine(temp,"image.bin"));Directory.Delete(temp);}
  Check(new LocalImageLimits().ValidateDimensions(2,3)==24,"RGBA byte-size planning");
  Expect<InvalidDataException>(()=>new LocalImageLimits(MaxPixels:5).ValidateDimensions(2,3));
  Expect<InvalidDataException>(()=>new LocalImageLimits(MaxDimension:1).ValidateDimensions(2,3));
  Expect<InvalidDataException>(()=>new LocalImageLimits(MaxDecodedBytes:23).ValidateDimensions(2,3));
  Expect<InvalidDataException>(()=>new LocalImageLimits().ValidateDimensions(0,3));
  Expect<InvalidDataException>(()=>new LocalImageLimits().ValidateDimensions(long.MaxValue,long.MaxValue));
  byte[] pixels=[255,0,0,255];using var decoded=new DecodedLocalImage(asset,1,1,1,1,"test",pixels);pixels[0]=0;Check(decoded.CopyRgba()[0]==255,"Decoded output owns caller bytes");
  var exposed=decoded.CopyRgba();exposed[0]=0;Check(decoded.CopyRgba()[0]==255,"Decoded output copies cannot mutate owner");
  Check(decoded.Width==1&&decoded.Height==1&&decoded.FrameIndex==0&&decoded.SourceFrameCount==1,"Decoded dimension/frame provenance");
  decoded.Dispose();decoded.Dispose();Expect<ObjectDisposedException>(()=>decoded.CopyRgba());Check(decoded.Source.ContentSha256==asset.ContentSha256,"Disposal retains immutable encoded source");
  Expect<ArgumentException>(()=>new DecodedLocalImage(asset,0,1,1,1,"test",pixels));
  Expect<ArgumentException>(()=>new DecodedLocalImage(asset,int.MaxValue,int.MaxValue,1,1,"test",pixels));
  Expect<ArgumentException>(()=>new DecodedLocalImage(asset,1,2,1,1,"test",pixels));
  return "PASS offline encoded image assets:"+checks+" immutable-byte/file/identity/budget cases; actual decode/Metal/SetView/PC parity separate.";
 }
}
