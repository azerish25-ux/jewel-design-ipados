using System.Security.Cryptography;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Explicit original runtime inputs. PowerOfTwo derives from legacy mipmap/NPOT capability state;
/// no device maximum, video-memory budget or original file DIB depth is inferred from an Apple provider.</summary>
public sealed record Rhino5TextureRasterSettings(int DeviceMaximum,uint MemoryBudget,float MemoryFraction,bool PowerOfTwo);

/// <summary>Owned normal24/32bit texture preparation. Grayscale precedes resize; alpha OR samples that
/// processed source. Original codec/palette/depth promotion, settings acquisition and cache/document lifecycle remain separate.</summary>
public static class Rhino5TextureRasterPreparation
{
 public static Rhino5PreparedTexture Prepare(DecodedLocalImage source,int sourceDibDepth,Rhino5TextureRasterSettings settings,bool gray=false,LocalImageLimits? limits=null,long maxWorkingBytes=64*1024*1024)
 {
  ArgumentNullException.ThrowIfNull(source);
  if(source.AlphaRepresentation!=RgbaAlphaRepresentation.Straight)throw new NotSupportedException("Source preparation requires explicit straight RGBA; premultiplied pixels are not divided.");
  CheckDepth(sourceDibDepth);limits??=new();int stride=Stride(source.Width,sourceDibDepth);
  _=Plan(source.Width,source.Height,sourceDibDepth,checked(stride*source.Height),settings,limits,maxWorkingBytes);
  byte[] rgba=source.CopyRgba();if(sourceDibDepth==24)for(int i=3;i<rgba.Length;i+=4)if(rgba[i]!=255)throw new NotSupportedException("Declared RGB24 would discard partial alpha from the decoded source.");
  byte[] stored=new byte[checked(stride*source.Height)];Pack(rgba,source.Width,source.Height,sourceDibDepth,stored);
  return PrepareStored(stored,source.Width,source.Height,sourceDibDepth,settings,gray,limits,maxWorkingBytes);
 }
 public static Rhino5PreparedTexture PrepareStored(ReadOnlySpan<byte> source,int sourceWidth,int sourceHeight,int sourceDibDepth,Rhino5TextureRasterSettings settings,bool gray=false,LocalImageLimits? limits=null,long maxWorkingBytes=64*1024*1024)
 {
  CheckDepth(sourceDibDepth);limits??=new();var dimensions=Plan(sourceWidth,sourceHeight,sourceDibDepth,source.Length,settings,limits,maxWorkingBytes);
  int w=dimensions.Width,h=dimensions.Height;byte[] originalRgba=Normalize(source,sourceWidth,sourceHeight,sourceDibDepth);string sourcePixelHash=Convert.ToHexString(SHA256.HashData(originalRgba));byte[] processed=source.ToArray();
  if(gray){byte[] grayed=Rhino5TraceRaster.Prepare(originalRgba,sourceWidth,sourceHeight,sourceDibDepth,true);Pack(grayed,sourceWidth,sourceHeight,sourceDibDepth,processed);}
  bool resized=w!=sourceWidth||h!=sourceHeight;int mode=resized?Rhino5GdiIntegerStretch.SelectTextureMode(sourceWidth,sourceHeight,w,h):0;
  byte[] output=mode==0?processed:mode==4?Rhino5GdiHalftoneStretch.ResizeStored(processed,sourceWidth,sourceHeight,sourceDibDepth,w,h,0,limits,maxWorkingBytes):Rhino5GdiIntegerStretch.ResizeStored(processed,sourceWidth,sourceHeight,sourceDibDepth,w,h,mode,0,limits);
  if(resized&&sourceDibDepth==32)Rhino5TexturePreparation.RepairAlphaStoredRows(processed,sourceWidth,sourceHeight,Stride(sourceWidth,32),output,w,h,Stride(w,32));
  return new(sourceWidth,sourceHeight,w,h,sourceDibDepth,gray,mode,sourcePixelHash,output,Normalize(output,w,h,sourceDibDepth));
 }
 private static void CheckDepth(int depth){if(depth is not(24 or 32))throw new NotSupportedException("Normal texture source must already be explicit BI_RGB24 or BI_RGB32. Indexed/other depth conversion is not guessed.");}
 private static int Stride(int width,int depth)=>checked((checked(width*(depth/8))+3)&~3);
 private static Rhino5TextureDimensions Plan(int sw,int sh,int depth,int sourceLength,Rhino5TextureRasterSettings settings,LocalImageLimits limits,long maxWorkingBytes)
 {
  ArgumentNullException.ThrowIfNull(settings);limits.ValidateDimensions(sw,sh);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxWorkingBytes);
  if(sourceLength!=checked(Stride(sw,depth)*sh))throw new ArgumentException("Source storage differs from positive-height DWORD DIB rows.");
  var d=Rhino5TexturePreparation.Dimensions(sw,sh,depth,settings.DeviceMaximum,settings.MemoryBudget,settings.MemoryFraction,settings.PowerOfTwo);limits.ValidateDimensions(d.Width,d.Height);
  bool changed=d.Width!=sw||d.Height!=sh;long sourcePixels=(long)sw*sh,targetPixels=(long)d.Width*d.Height;
  // Covers decoded adapter copies, gray temporaries, stored/normalized output and conservative filter peak.
  long working=changed?48L*(sourcePixels+(long)sw*d.Height+(long)d.Width*sh+targetPixels)+24*sourcePixels+12*targetPixels+sourceLength+(long)Stride(d.Width,depth)*d.Height:32*sourcePixels;
  if(working>maxWorkingBytes)throw new InvalidDataException("Composed texture working storage budget exceeded before allocation.");
  return d;
 }
 private static void Pack(ReadOnlySpan<byte> rgba,int w,int h,int depth,Span<byte> stored)
 {int stride=Stride(w,depth),pixel=depth/8;for(int y=0;y<h;y++)for(int x=0;x<w;x++){int s=(y*w+x)*4,d=(h-1-y)*stride+x*pixel;stored[d]=rgba[s+2];stored[d+1]=rgba[s+1];stored[d+2]=rgba[s];if(depth==32)stored[d+3]=rgba[s+3];}}
 private static byte[] Normalize(ReadOnlySpan<byte> stored,int w,int h,int depth)
 {int stride=Stride(w,depth),pixel=depth/8;byte[] rgba=new byte[checked(w*h*4)];for(int y=0;y<h;y++)for(int x=0;x<w;x++){int s=(h-1-y)*stride+x*pixel,d=(y*w+x)*4;rgba[d]=stored[s+2];rgba[d+1]=stored[s+1];rgba[d+2]=stored[s];rgba[d+3]=depth==32?stored[s+3]:(byte)255;}return rgba;}
}

/// <summary>Caller-owned processed pixels and receipt. Copies never alias storage. Dispose after upload;
/// uploaded Metal resources own their own pixels and retain their separate in-flight lifetime.</summary>
public sealed class Rhino5PreparedTexture:IDisposable
{
 private readonly object sync=new();private readonly string sourcePixelHash;private byte[]? stored,rgba;
 public int SourceWidth{get;}public int SourceHeight{get;}public int Width{get;}public int Height{get;}public int DibBitDepth{get;}public bool GrayScale{get;}public int StretchMode{get;}
 public Rhino5TextureRasterReceipt Receipt=>new(SourceWidth,SourceHeight,Width,Height,DibBitDepth,GrayScale,StretchMode);
 public bool WasResized=>StretchMode!=0;
 internal Rhino5PreparedTexture(int sw,int sh,int w,int h,int depth,bool gray,int mode,string sourcePixelHash,byte[] ownedStored,byte[] ownedRgba){SourceWidth=sw;SourceHeight=sh;Width=w;Height=h;DibBitDepth=depth;GrayScale=gray;StretchMode=mode;this.sourcePixelHash=sourcePixelHash;stored=ownedStored;rgba=ownedRgba;}
 public byte[] CopyStored(){lock(sync){ObjectDisposedException.ThrowIf(stored is null,this);return(byte[])stored!.Clone();}}
 public byte[] CopyRgba(){lock(sync){ObjectDisposedException.ThrowIf(rgba is null,this);return(byte[])rgba!.Clone();}}
 public DecodedLocalImage CreateDecodedImage(DecodedLocalImage source)
 {ArgumentNullException.ThrowIfNull(source);if(source.Width!=SourceWidth||source.Height!=SourceHeight||Convert.ToHexString(SHA256.HashData(source.CopyRgba()))!=sourcePixelHash)throw new ArgumentException("Decoded provenance does not match the original prepared source pixels.",nameof(source));return new(source.Source,Width,Height,source.SourceOrientation,source.SourceFrameCount,source.TypeIdentifier,CopyRgba(),RgbaAlphaRepresentation.Straight,null,DecodedPixelSource.PreparedTexture);}
 public void Dispose(){lock(sync){if(stored is {} s)Array.Clear(s);if(rgba is {} r)Array.Clear(r);stored=null;rgba=null;}}
}

public readonly record struct Rhino5TextureRasterReceipt(int SourceWidth,int SourceHeight,int Width,int Height,int DibBitDepth,bool GrayScale,int StretchMode);
