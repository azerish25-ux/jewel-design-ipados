namespace JewelDesign.Rendering.Candidate;

/// <summary>Original Rhino5 wallpaper size and BGR24 arithmetic, before/after platform resampling.
/// This does not decode an image, perform GDI HALFTONE resampling, or report a successful load.</summary>
public static class Rhino5WallpaperRaster
{
 public const int PixelThreshold=9_000_000;

 public static Rhino5WallpaperRasterPlan Plan(int width,int height,int sourceWidth,int sourceHeight)
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sourceWidth);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sourceHeight);
  long area=(long)width*height;
  // Original uses signed32 IMUL. Reject overflowing inputs rather than emulate unsafe native sizing.
  if(area>int.MaxValue)throw new ArgumentOutOfRangeException(nameof(width),"Native signed32 pixel-product overflow is outside the supported sizing domain.");
  int divisor=1;
  if(area>PixelThreshold)
  {
   while(divisor<1000&&area/(divisor*divisor)>=PixelThreshold)divisor++;
   if(divisor==1000)throw new ArgumentOutOfRangeException(nameof(width),"Native sizing divisor exhausted.");
  }
  int inputWidth=width/divisor,inputHeight=height/divisor;
  // Recursive original call rejects zero reduced dimensions before attempting image allocation.
  if(inputWidth==0||inputHeight==0)return new(inputWidth,inputHeight,0,0,0,divisor,false);
  width=inputWidth;height=inputHeight;
  if(sourceHeight<sourceWidth)
  {
   double fitted=width*((double)sourceHeight/sourceWidth);
   if(fitted<=height)height=(int)fitted;
   else width=(int)(height*((double)sourceWidth/sourceHeight));
  }
  else
  {
   double fitted=height*((double)sourceWidth/sourceHeight);
   if(fitted<=width)width=(int)fitted;
   else height=(int)(width*((double)sourceHeight/sourceWidth));
  }
  int aligned=checked((width+3)&~3);
  return new(inputWidth,inputHeight,width,height,aligned,divisor,true);
 }

 /// <summary>Mutates every complete BGR triple in the supplied storage span, in original SSE order.
 /// Does not linearize/gamma-correct colors. Incomplete final bytes remain untouched.</summary>
 public static void GrayscaleBgr24Storage(Span<byte> storage)
 {
  int end=storage.Length-storage.Length%3;
  for(int i=0;i<end;i+=3)
  {
   double red=storage[i+2]*0.299;
   double green=storage[i+1]*0.589;
   double blue=storage[i]*0.111;
   byte gray=(byte)(int)((red+green)+blue);
   storage[i]=gray;storage[i+1]=gray;storage[i+2]=gray;
  }
 }
}

/// <summary>AllocationRequested distinguishes rejected recursive zero input from an attempted
/// zero-size aspect fit. StorageWidth is rounded after fitting and is also the StretchBlt width.</summary>
public readonly record struct Rhino5WallpaperRasterPlan(
 int ReducedWidth,int ReducedHeight,int FittedWidth,int Height,int StorageWidth,
 int DownscaleDivisor,bool AllocationRequested);
