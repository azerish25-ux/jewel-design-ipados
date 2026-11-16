using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using ImageIO;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;

/// <summary>Real offline ImageIO decode into owned sRGB premultiplied RGBA storage.
/// Bounds our encoded/output allocations, not undocumented internal codec allocations.</summary>
public static class AppleLocalImageDecoder
{
 public static DecodedLocalImage Decode(LocalImageAsset asset,LocalImageLimits? limits=null)
 {
  ArgumentNullException.ThrowIfNull(asset);limits??=new();limits.Validate();
  if(asset.EncodedLength>limits.MaxEncodedBytes)throw new InvalidDataException("Encoded source exceeds the decoder budget.");
  using var data=NSData.FromArray(asset.CopyEncodedBytes());
  var options=new CGImageOptions{ShouldCache=false};
  using var source=CGImageSource.FromData(data,options)??throw new InvalidDataException("Unsupported or invalid encoded image.");
  if(source.ImageCount<1||source.GetStatus()!=CGImageSourceStatus.Complete)throw new InvalidDataException("Image source is incomplete or corrupt.");
  using var properties=source.CopyProperties(options,0)??throw new InvalidDataException("Image metadata is unavailable.");
  long width=(properties[ImageIO.CGImageProperties.PixelWidth] as NSNumber)?.Int64Value??0;
  long height=(properties[ImageIO.CGImageProperties.PixelHeight] as NSNumber)?.Int64Value??0;
  long orientation=(properties[ImageIO.CGImageProperties.Orientation] as NSNumber)?.Int64Value??1;
  int size=limits.ValidateDimensions(width,height);
  using var image=source.CreateImage(0,options)??throw new InvalidDataException("ImageIO could not decode the image pixels.");
  if((long)image.Width!=width||(long)image.Height!=height||source.GetStatus(0)!=CGImageSourceStatus.Complete)throw new InvalidDataException("Decoded dimensions/status differ from the validated image metadata.");
  using var color=CGColorSpace.CreateSrgb();
  using var bitmap=new CGBitmapContext(IntPtr.Zero,(nint)width,(nint)height,8,checked((nint)width*4),color,CGBitmapFlags.ByteOrder32Big|CGBitmapFlags.PremultipliedLast);
  if(bitmap.Data==IntPtr.Zero)throw new InvalidOperationException("Apple bitmap allocation failed.");
  bitmap.SetBlendMode(CGBlendMode.Copy);
  bitmap.DrawImage(new CGRect(0,0,width,height),image);
  if(source.GetStatus(0)!=CGImageSourceStatus.Complete)throw new InvalidDataException("ImageIO failed while materializing pixels.");
  byte[] rgba=new byte[size];Marshal.Copy(bitmap.Data,rgba,0,size);
  return new DecodedLocalImage(asset,(int)width,(int)height,orientation,(long)source.ImageCount,source.TypeIdentifier??"",rgba);
 }
 /// <summary>Copies supported unassociated provider components directly. Unsupported layouts/profiles
 /// fail explicitly; encoded source remains owned by the caller. No unpremultiplication or redraw.</summary>
 public static DecodedLocalImage DecodeStraight(LocalImageAsset asset,LocalImageLimits? limits=null)
 {
  ArgumentNullException.ThrowIfNull(asset);limits??=new();limits.Validate();
  if(asset.EncodedLength>limits.MaxEncodedBytes)throw new InvalidDataException("Encoded source exceeds the decoder budget.");
  using var data=NSData.FromArray(asset.CopyEncodedBytes());var options=new CGImageOptions{ShouldCache=false};
  using var source=CGImageSource.FromData(data,options)??throw new InvalidDataException("Unsupported or invalid encoded image.");
  if(source.ImageCount<1||source.GetStatus()!=CGImageSourceStatus.Complete)throw new InvalidDataException("Image source is incomplete or corrupt.");
  using var properties=source.CopyProperties(options,0)??throw new InvalidDataException("Image metadata is unavailable.");
  long width=(properties[ImageIO.CGImageProperties.PixelWidth] as NSNumber)?.Int64Value??0,height=(properties[ImageIO.CGImageProperties.PixelHeight] as NSNumber)?.Int64Value??0;
  long orientation=(properties[ImageIO.CGImageProperties.Orientation] as NSNumber)?.Int64Value??1;
  limits.ValidateDimensions(width,height);
  using var image=source.CreateImage(0,options)??throw new InvalidDataException("ImageIO could not decode the image.");
  if((long)image.Width!=width||(long)image.Height!=height||source.GetStatus(0)!=CGImageSourceStatus.Complete)throw new InvalidDataException("Decoded image metadata/status mismatch.");
  if(image.IsMask)throw new NotSupportedException("Image mask conversion requires an explicit source-compatible adapter.");
  using var color=image.ColorSpace;
  var layout=new ImageProviderLayout(checked((int)image.BitsPerComponent),checked((int)image.BitsPerPixel),(long)image.BytesPerRow,(uint)image.BitmapInfo,color is null?-1:(int)color.Model,color?.Name??"",HasDecodeArray(image));
  int needed=StraightRgbaProvider.RequiredStorageBytes((int)width,(int)height,layout,limits);
  using var provider=image.DataProvider;
  using var bytes=provider.CopyData()??throw new InvalidDataException("Image provider data is unavailable.");
  if((ulong)bytes.Length<(ulong)needed||(ulong)bytes.Length>(ulong)limits.MaxDecodedBytes)throw new InvalidDataException("Image provider storage is truncated or over budget.");
  var rgba=StraightRgbaProvider.Extract((int)width,(int)height,bytes.ToArray(),layout,limits);
  if(source.GetStatus(0)!=CGImageSourceStatus.Complete)throw new InvalidDataException("ImageIO failed while materializing provider pixels.");
  return new DecodedLocalImage(asset,(int)width,(int)height,orientation,(long)source.ImageCount,source.TypeIdentifier??"",rgba,RgbaAlphaRepresentation.Straight,layout);
 }
 private static unsafe bool HasDecodeArray(CGImage image)=>image.Decode!=null;
}
