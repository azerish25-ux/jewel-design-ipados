namespace JewelDesign.Rendering.Candidate;
public sealed record ImageProviderLayout(int BitsPerComponent,int BitsPerPixel,long BytesPerRow,uint BitmapInfo,int ColorSpaceModel,string ColorSpaceName,bool HasDecodeArray);

/// <summary>Lossless channel/row repacking of supported unassociated RGB8 provider layouts.
/// No color transformation, alpha division, bit-depth reduction or guessed layout fallback.</summary>
public static class StraightRgbaProvider
{
 public static int RequiredStorageBytes(int width,int height,ImageProviderLayout layout,LocalImageLimits? limits=null)
 {
  ArgumentNullException.ThrowIfNull(layout);limits??=new();limits.ValidateDimensions(width,height);
  if(layout.BitsPerComponent!=8||layout.ColorSpaceModel!=1||layout.ColorSpaceName!="kCGColorSpaceSRGB"||layout.HasDecodeArray)
   throw new NotSupportedException("Straight extraction requires RGB8/sRGB with default decode mapping: "+layout);
  uint alpha=layout.BitmapInfo&31,order=layout.BitmapInfo&0x7000;
  if((layout.BitmapInfo&~0x701fu)!=0||order is not (0 or 0x2000 or 0x4000))
   throw new NotSupportedException("Unsupported provider component/byte-order flags: "+layout);
  if(layout.BitsPerPixel==24){if(alpha!=0||order!=0)throw new NotSupportedException("RGB24 requires no-alpha/default component order: "+layout);}
  else if(layout.BitsPerPixel!=32||alpha is not (3 or 4 or 5 or 6))
   throw new NotSupportedException("Provider is not a supported straight/opaque RGB layout; premultiplied data is not unpremultiplied: "+layout);
  long row=(long)width*(layout.BitsPerPixel/8);
  if(layout.BytesPerRow<row||layout.BytesPerRow>limits.MaxDecodedBytes)throw new InvalidDataException("Invalid or over-budget provider stride.");
  if(height>1&&layout.BytesPerRow>(limits.MaxDecodedBytes-row)/(height-1))throw new InvalidDataException("Provider rows exceed the storage budget.");
  long needed=(height-1)*layout.BytesPerRow+row;
  if(needed>limits.MaxDecodedBytes)throw new InvalidDataException("Provider storage exceeds the budget.");
  return checked((int)needed);
 }
 public static byte[] Extract(int width,int height,ReadOnlySpan<byte> provider,ImageProviderLayout layout,LocalImageLimits? limits=null)
 {
  limits??=new();int required=RequiredStorageBytes(width,height,layout,limits);
  if(provider.Length<required||provider.Length>limits.MaxDecodedBytes)throw new InvalidDataException("Truncated or over-budget provider bytes.");
  byte[] output=new byte[limits.ValidateDimensions(width,height)];
  int components=layout.BitsPerPixel/8;uint alpha=layout.BitmapInfo&31;bool little=(layout.BitmapInfo&0x7000)==0x2000;
  for(int y=0;y<height;y++)for(int x=0;x<width;x++){
   int start=checked((int)(y*layout.BytesPerRow+x*components)),dest=(y*width+x)*4;
   if(components==3){output[dest]=provider[start];output[dest+1]=provider[start+1];output[dest+2]=provider[start+2];output[dest+3]=255;continue;}
   int r=alpha is 3 or 5?0:1,g=r+1,b=r+2,a=alpha is 3 or 5?3:0;
   if(little){r=3-r;g=3-g;b=3-b;a=3-a;}
   output[dest]=provider[start+r];output[dest+1]=provider[start+g];output[dest+2]=provider[start+b];output[dest+3]=alpha>=5?(byte)255:provider[start+a];
  }
  return output;
 }
}
