using System.Security.Cryptography;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Original wallpaper CPU path: fit/divisor/aligned24bit destination, unconditional HALFTONE,
/// then optional post-resize gray without trace rounding. Source32 RGB is intentionally displayed opaque.
/// Original encoded asset remains unchanged. Source depth is explicit; original codec/cache/profile acquisition remains separate.</summary>
public static class Rhino5WallpaperPreparation
{
 public static Rhino5PreparedWallpaper Prepare(DecodedLocalImage source,int sourceDibDepth,int requestedWidth,int requestedHeight,bool gray=false,LocalImageLimits? limits=null,long maxWorkingBytes=512L*1024*1024)
 {
  ArgumentNullException.ThrowIfNull(source);if(source.AlphaRepresentation!=RgbaAlphaRepresentation.Straight)throw new NotSupportedException("Wallpaper preparation requires explicit straight RGB values.");CheckDepth(sourceDibDepth);limits??=new();int stride=checked((source.Width*(sourceDibDepth/8)+3)&~3);_=Plan(source.Width,source.Height,sourceDibDepth,checked(stride*source.Height),requestedWidth,requestedHeight,limits,maxWorkingBytes);
  byte[] rgba=source.CopyRgba();if(sourceDibDepth==24)for(int i=3;i<rgba.Length;i+=4)if(rgba[i]!=255)throw new NotSupportedException("Declared source RGB24 conflicts with decoded partial alpha. Supply the original32bit source depth.");byte[] stored=new byte[checked(stride*source.Height)];Rhino5TextureRasterPreparation.Pack(rgba,source.Width,source.Height,sourceDibDepth,stored);return PrepareStored(stored,source.Width,source.Height,sourceDibDepth,requestedWidth,requestedHeight,gray,limits,maxWorkingBytes);
 }
 public static Rhino5PreparedWallpaper PrepareStored(ReadOnlySpan<byte> source,int sw,int sh,int sourceDibDepth,int requestedWidth,int requestedHeight,bool gray=false,LocalImageLimits? limits=null,long maxWorkingBytes=512L*1024*1024)
 {
  CheckDepth(sourceDibDepth);limits??=new();var plan=Plan(sw,sh,sourceDibDepth,source.Length,requestedWidth,requestedHeight,limits,maxWorkingBytes);string sourceHash=Convert.ToHexString(SHA256.HashData(Rhino5TextureRasterPreparation.Normalize(source,sw,sh,sourceDibDepth)));
  byte[] output=Rhino5GdiHalftoneStretch.ResizeStored(source,sw,sh,sourceDibDepth,plan.StorageWidth,plan.Height,0,limits,maxWorkingBytes,24);
  if(gray)Rhino5WallpaperRaster.GrayscaleBgr24Storage(output);
  var receipt=new Rhino5WallpaperRasterReceipt(sw,sh,sourceDibDepth,requestedWidth,requestedHeight,plan,gray);
  return new(receipt,new Rhino5PreparedTexture(sw,sh,plan.StorageWidth,plan.Height,24,gray,4,sourceHash,output,Rhino5TextureRasterPreparation.Normalize(output,plan.StorageWidth,plan.Height,24)));
 }
 private static void CheckDepth(int depth){if(depth is not(24 or 32))throw new NotSupportedException("Wallpaper source must already be declared BI_RGB24/32; indexed/other decode conversion is not inferred.");}
 private static Rhino5WallpaperRasterPlan Plan(int sw,int sh,int depth,int sourceLength,int requestedWidth,int requestedHeight,LocalImageLimits limits,long maxWorkingBytes)
 {
  limits.ValidateDimensions(sw,sh);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxWorkingBytes);int stride=checked((sw*(depth/8)+3)&~3);if(sourceLength!=checked(stride*sh))throw new ArgumentException("Wallpaper source differs from positive-height DWORD rows.");var plan=Rhino5WallpaperRaster.Plan(requestedWidth,requestedHeight,sw,sh);limits.ValidateDimensions(plan.StorageWidth,plan.Height);
  long native=Rhino5GdiHalftoneStretch.EstimateWorkingStorage(sw,sh,plan.StorageWidth,plan.Height,sourceLength,(long)plan.StorageWidth*plan.Height*3),working=native+8L*sw*sh+12L*plan.StorageWidth*plan.Height;
  if(working>maxWorkingBytes)throw new InvalidDataException("Wallpaper CPU preparation exceeds the explicit working storage budget before copying source.");return plan;
 }
}
public readonly record struct Rhino5WallpaperRasterReceipt(int SourceWidth,int SourceHeight,int SourceDibDepth,int RequestedWidth,int RequestedHeight,Rhino5WallpaperRasterPlan Plan,bool GrayScale);
public sealed class Rhino5PreparedWallpaper:IDisposable
{
 private readonly Rhino5PreparedTexture storage;public Rhino5WallpaperRasterReceipt Receipt{get;}public Rhino5WallpaperRasterPlan Plan=>Receipt.Plan;public int Width=>Plan.StorageWidth;public int Height=>Plan.Height;
 internal Rhino5PreparedWallpaper(Rhino5WallpaperRasterReceipt receipt,Rhino5PreparedTexture storage){Receipt=receipt;this.storage=storage;}
 public byte[] CopyStored()=>storage.CopyStored();public byte[] CopyRgba()=>storage.CopyRgba();
 public DecodedLocalImage CreateDecodedImage(DecodedLocalImage source)=>storage.CreateDecodedImage(source,DecodedPixelSource.PreparedWallpaper);
 public void Dispose()=>storage.Dispose();
}
