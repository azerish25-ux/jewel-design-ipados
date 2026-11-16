namespace JewelDesign.Rendering.Candidate;
/// <summary>Original GDI wallpaper draw placement for an already-resized DIB; no resampling.</summary>
public readonly record struct Rhino5WallpaperPlacement(int ClientWidth,int ClientHeight,int Left,int Top,int Width,int Height)
{
 public static Rhino5WallpaperPlacement Gdi(int clientWidth,int clientHeight,int imageWidth,int imageHeight,int storedScale)
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(clientWidth);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(clientHeight);
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(imageWidth);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(imageHeight);
  int scale=storedScale<2?1:storedScale;long w=(long)imageWidth*scale,h=(long)imageHeight*scale;
  if(w>int.MaxValue||h>int.MaxValue)throw new ArgumentOutOfRangeException(nameof(storedScale),"Native signed32 draw-size overflow is outside the managed placement domain.");
  return new(clientWidth,clientHeight,(clientWidth-(int)w)/2,(clientHeight-(int)h)/2,(int)w,(int)h);
 }
 public float[] CopyClipQuad()
 {
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ClientWidth);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ClientHeight);
  int cw=ClientWidth,ch=ClientHeight;float X(long x)=>(float)(2d*x/cw-1);float Y(long y)=>(float)(1-2d*y/ch);
  float l=X(Left),r=X((long)Left+Width),t=Y(Top),b=Y((long)Top+Height);
  return [r,b,0,1,r,t,0,1,l,t,0,1,l,b,0,1];
 }
}
