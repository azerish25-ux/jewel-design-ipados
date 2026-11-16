namespace JewelDesign.Rendering.Candidate;

/// <summary>Measured current-host Win32 SRCCOPY integer stretch behavior for positive-height BI_RGB24/32 DIBs.
/// Preserves stored BGR(A), alpha and untouched padding; does not apply source32bit alpha repair.
/// HALFTONE/palette/profile/negative-height/vendor-driver equivalence remain separate acceptance.</summary>
public static class Rhino5GdiIntegerStretch
{
 public static int SelectTextureMode(int sourceWidth,int sourceHeight,int width,int height,int currentMode=1)
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sourceWidth);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sourceHeight);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
  if(currentMode<1||currentMode>4)throw new ArgumentOutOfRangeException(nameof(currentMode));
  return sourceWidth/width>=2||sourceHeight/height>=2?4:currentMode;
 }
 public static byte[] ResizeStored(ReadOnlySpan<byte> source,int sourceWidth,int sourceHeight,int depth,int width,int height,int mode,byte destinationSeed=0,LocalImageLimits? limits=null)
 {
  if(mode is 2 or 4)throw new NotSupportedException("WHITEONBLACK/HALFTONE RGB behavior is not implemented by the measured integer stretch component.");
  if(mode is not (1 or 3))throw new ArgumentOutOfRangeException(nameof(mode));
  if(depth is not (24 or 32))throw new NotSupportedException("Integer stretch requires explicit BI_RGB24/32 stored source bytes.");
  limits??=new();limits.ValidateDimensions(sourceWidth,sourceHeight);limits.ValidateDimensions(width,height);
  int pixel=depth/8,sourceStride=checked((sourceWidth*pixel+3)&~3),stride=checked((width*pixel+3)&~3);
  if(source.Length!=checked(sourceStride*sourceHeight))throw new ArgumentException("Stored source byte length differs from positive-height DWORD rows.",nameof(source));
  byte[] result=new byte[checked(stride*height)];if(destinationSeed!=0)Array.Fill(result,destinationSeed);
  // Joint dimension-delta shortcut is measured independently; it is not ordinary nearest interpolation.
  bool direct=Math.Abs(sourceWidth-width)<=1&&Math.Abs(sourceHeight-height)<=1;
  var xs=Spans(sourceWidth,width,mode,direct);var ys=Spans(sourceHeight,height,mode,direct);
  for(int y=0;y<height;y++)for(int x=0;x<width;x++){
   int destination=(height-1-y)*stride+x*pixel;
   for(int channel=0;channel<pixel;channel++){
    byte value=255;
    for(int sy=ys[y].Start;sy<ys[y].End;sy++)for(int sx=xs[x].Start;sx<xs[x].End;sx++)value&=source[(sourceHeight-1-sy)*sourceStride+sx*pixel+channel];
    result[destination+channel]=value;
   }
  }
  return result;
 }
 private static (int Start,int End)[] Spans(int source,int destination,int mode,bool direct)
 {
  var spans=new (int,int)[destination];int previous=-1;
  for(int i=0;i<destination;i++){
   int selected=direct?Math.Min(i,source-1):checked((int)(((2L*i+1)*source)/(2L*destination)));
   spans[i]=!direct&&mode==1&&source>destination?(previous+1,selected+1):(selected,selected+1);
   previous=selected;
  }
  return spans;
 }
}
