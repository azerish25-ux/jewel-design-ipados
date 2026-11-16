namespace JewelDesign.Rendering.Candidate;
public readonly record struct Rhino5TextureDimensions(int Width,int Height);

/// <summary>Original Rhino5 texture size/alpha preparation, separate from unresolved GDI RGB filtering.
/// Device maximum and memory budget are explicit runtime inputs, not guessed global constants.</summary>
public static class Rhino5TexturePreparation
{
 public static Rhino5TextureDimensions Dimensions(int width,int height,int bitDepth,int deviceMaximum,uint memoryBudget,float memoryFraction,bool powerOfTwo)
 {
  if(width<1||height<1||width>1<<30||height>1<<30||deviceMaximum<1||deviceMaximum>1<<30||bitDepth<0||bitDepth>ushort.MaxValue||memoryFraction<0)
   throw new ArgumentOutOfRangeException(nameof(width),"Unsafe native dimension/negative-budget-loop domain is not emulated.");
  static int CeilingPower(int n){int p=1;while(p<n)p=checked(p*2);return p;}
  int cw=CeilingPower(width),ch=CeilingPower(height);
  int w=cw>deviceMaximum?deviceMaximum:powerOfTwo?cw:width;
  int h=ch>deviceMaximum?deviceMaximum:powerOfTwo?ch:height;
  // Original rounds the otherwise-unchanged axis after any change, even when powerOfTwo is false.
  if(w!=width&&h==height)h=ch;
  else if(w==width&&h!=height)w=cw;
  int bytesPerPixel=bitDepth<24?3:bitDepth>>3;
  uint Bytes(){
   if(w==0||h==0)return 0;
   if((ulong)w>(ulong)uint.MaxValue/(uint)bytesPerPixel/(uint)h)throw new ArgumentOutOfRangeException(nameof(width),"Native uint32 byte-count overflow is outside the managed preparation domain.");
   return (uint)((long)w*h*bytesPerPixel);
  }
  uint bytes=Bytes();
  // Single-precision ratio/comparison and inclusive equality match original SSE instructions.
  // NaN fraction skips reduction; zero memoryBudget uses IEEE infinity/NaN, retaining zero-size outcomes.
  while(memoryFraction<(float)bytes/(float)memoryBudget){w>>=1;h>>=1;bytes=Bytes();}
  return new(w,h);
 }
 private static void Validate(int width,int height,int stride,int length)
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
  if(stride<(long)width*4||(long)(height-1)*stride+(long)width*4>length)throw new ArgumentException("Truncated32-bit pixel rows or invalid stride.");
 }
 /// <summary>Repairs alpha in native DIB stored-row order (normally bottom first).
 /// Samples floor(x*sourceWidth/destWidth), floor(y*sourceHeight/destHeight), then ORs high alpha bytes.
 /// RGB and padding are unchanged; source/destination must not overlap.</summary>
 public static void RepairAlphaStoredRows(ReadOnlySpan<byte> source,int sourceWidth,int sourceHeight,int sourceStride,Span<byte> destination,int destWidth,int destHeight,int destStride)
 {
  Validate(sourceWidth,sourceHeight,sourceStride,source.Length);Validate(destWidth,destHeight,destStride,destination.Length);
  if(source.Overlaps(destination))throw new ArgumentException("Distinct native source/destination buffers are required.");
  double xScale=(double)sourceWidth/destWidth,yScale=(double)sourceHeight/destHeight;
  for(int y=0;y<destHeight;y++){int sy=(int)(y*yScale);for(int x=0;x<destWidth;x++){int sx=(int)(x*xScale);destination[y*destStride+x*4+3]|=source[sy*sourceStride+sx*4+3];}}
 }
 /// <summary>Adapts the original bottom-up DIB sampling to explicit top-row-first RGBA storage.
 /// Does not use conventional top-down nearest scaling, which differs for fractional resize ratios.</summary>
 public static void RepairAlphaTopDownRgba(ReadOnlySpan<byte> source,int sourceWidth,int sourceHeight,Span<byte> destination,int destWidth,int destHeight)
 {
  int sourceStride=checked(sourceWidth*4),destStride=checked(destWidth*4);
  Validate(sourceWidth,sourceHeight,sourceStride,source.Length);Validate(destWidth,destHeight,destStride,destination.Length);
  if(source.Overlaps(destination))throw new ArgumentException("Distinct native source/destination buffers are required.");
  double xScale=(double)sourceWidth/destWidth,yScale=(double)sourceHeight/destHeight;
  for(int y=0;y<destHeight;y++){int sy=sourceHeight-1-(int)((destHeight-1-y)*yScale);for(int x=0;x<destWidth;x++){int sx=(int)(x*xScale);destination[y*destStride+x*4+3]|=source[sy*sourceStride+sx*4+3];}}
 }
}
