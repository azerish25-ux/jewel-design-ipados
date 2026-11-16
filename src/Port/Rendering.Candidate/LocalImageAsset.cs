using System.Security.Cryptography;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Caller-selected local import; OriginalName is inert provenance, never a path to execute/resolve.
/// Encoded cache/import success is deliberately separate from actual image decoding.</summary>
public sealed class LocalImageAsset
{
 private readonly byte[] encoded;
 public string OriginalName{get;}
 public int EncodedLength=>encoded.Length;
 public string ContentSha256{get;}
 private LocalImageAsset(string name,byte[] bytes){OriginalName=name;encoded=bytes;ContentSha256=Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();}
 public byte[] CopyEncodedBytes()=>(byte[])encoded.Clone();
 public static LocalImageAsset FromBytes(string originalName,ReadOnlySpan<byte> bytes,LocalImageLimits? limits=null)
 {
  ArgumentException.ThrowIfNullOrEmpty(originalName);limits??=new();limits.Validate();
  if(bytes.Length<1||bytes.Length>limits.MaxEncodedBytes)throw new InvalidDataException("Encoded image is empty or exceeds the explicit import budget.");
  return new(originalName,bytes.ToArray());
 }
 public static LocalImageAsset ImportFile(string originalName,string selectedLocalPath,LocalImageLimits? limits=null)
 {
  ArgumentException.ThrowIfNullOrEmpty(originalName);ArgumentException.ThrowIfNullOrEmpty(selectedLocalPath);
  if(!Path.IsPathFullyQualified(selectedLocalPath))throw new ArgumentException("Import requires an explicitly selected absolute local path.",nameof(selectedLocalPath));
  limits??=new();limits.Validate();
  using var stream=new FileStream(selectedLocalPath,FileMode.Open,FileAccess.Read,FileShare.Read);
  if(stream.Length<1||stream.Length>limits.MaxEncodedBytes)throw new InvalidDataException("Encoded local file is empty or exceeds the import budget.");
  byte[] bytes=new byte[(int)stream.Length];stream.ReadExactly(bytes);
  if(stream.ReadByte()!=-1)throw new IOException("Local image changed during import.");
  return new(originalName,bytes);
 }
}

public sealed record LocalImageLimits(int MaxEncodedBytes=32*1024*1024,int MaxPixels=16*1024*1024,int MaxDimension=16384,int MaxDecodedBytes=64*1024*1024)
{
 public void Validate()
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxEncodedBytes);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxPixels);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxDimension);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxDecodedBytes);
 }
 public int ValidateDimensions(long width,long height)
 {
  Validate();
  if(width<1||height<1||width>MaxDimension||height>MaxDimension)throw new InvalidDataException("Image dimensions exceed the explicit decoder budget.");
  long pixels=width*height;
  if(pixels>MaxPixels||pixels*4>MaxDecodedBytes)throw new InvalidDataException("Decoded RGBA storage exceeds the explicit decoder budget.");
  return checked((int)(pixels*4));
 }
}

public enum RgbaAlphaRepresentation {Premultiplied,Straight}
public enum DecodedPixelSource {CallerSupplied,DrawingContext,NativeProvider,EncodedPng,PreparedTexture,PreparedWallpaper}

/// <summary>Owned top-row-first RGBA8/sRGB with explicit alpha representation; source EXIF orientation retained,
/// not applied. Source bytes preserve other metadata. Not original LEADTOOLS/GDI conversion parity.</summary>
public sealed class DecodedLocalImage:IDisposable
{
 private byte[]? rgba;private readonly object sync=new();
 public RgbaAlphaRepresentation AlphaRepresentation{get;}
 public ImageProviderLayout? ProviderLayout{get;}
 public DecodedPixelSource PixelSource{get;}
 public LocalImageAsset Source{get;}
 public int Width{get;} public int Height{get;}
 public long SourceOrientation{get;} public long SourceFrameCount{get;} public int FrameIndex=>0;
 public string TypeIdentifier{get;}
 public DecodedLocalImage(LocalImageAsset source,int width,int height,long orientation,long frames,string type,ReadOnlySpan<byte> pixels,RgbaAlphaRepresentation alphaRepresentation=RgbaAlphaRepresentation.Premultiplied,ImageProviderLayout? providerLayout=null,DecodedPixelSource pixelSource=DecodedPixelSource.CallerSupplied)
 {
  ArgumentNullException.ThrowIfNull(source);if(width<1||height<1||(long)width*height>int.MaxValue/4||(long)width*height*4!=pixels.Length)throw new ArgumentException("RGBA dimensions/storage mismatch.");
  if(alphaRepresentation is not (RgbaAlphaRepresentation.Premultiplied or RgbaAlphaRepresentation.Straight))throw new ArgumentOutOfRangeException(nameof(alphaRepresentation));
  if(pixelSource is not (DecodedPixelSource.CallerSupplied or DecodedPixelSource.DrawingContext or DecodedPixelSource.NativeProvider or DecodedPixelSource.EncodedPng or DecodedPixelSource.PreparedTexture or DecodedPixelSource.PreparedWallpaper))throw new ArgumentOutOfRangeException(nameof(pixelSource));
  AlphaRepresentation=alphaRepresentation;ProviderLayout=providerLayout;PixelSource=pixelSource;
  Source=source;Width=width;Height=height;SourceOrientation=orientation;SourceFrameCount=frames;TypeIdentifier=type;rgba=pixels.ToArray();
 }
 public byte[] CopyRgba(){lock(sync){ObjectDisposedException.ThrowIf(rgba is null,this);return (byte[])rgba.Clone();}}
 public void Dispose(){lock(sync){if(rgba is {} bytes){Array.Clear(bytes);rgba=null;}}}
}
