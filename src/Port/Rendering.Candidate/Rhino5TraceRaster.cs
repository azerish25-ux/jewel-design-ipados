namespace JewelDesign.Rendering.Candidate;
/// <summary>Mapped original trace DIB RGB24/BGRA32 processing, on explicit straight RGBA storage.
/// RGB24 has opaque alpha; source32 grayscale writes gray to alpha as well. No resampling/profile transform.</summary>
public static class Rhino5TraceRaster
{
 public static byte[] Prepare(ReadOnlySpan<byte> rgba,int width,int height,int sourceDibBitDepth,bool gray)
 {
  if(sourceDibBitDepth is not (24 or 32))throw new NotSupportedException("Trace DIB must already be prepared as24 or32 bits.");
  if(width<1||height<1||(long)width*height*4!=rgba.Length)throw new ArgumentException("Trace RGBA dimensions/storage mismatch.");
  byte[] result=rgba.ToArray();
  for(int i=0;i<result.Length;i+=4){
   if(gray){byte value=(byte)(int)(((result[i]*.299+result[i+1]*.589)+result[i+2]*.111)+.5);result[i]=value;result[i+1]=value;result[i+2]=value;if(sourceDibBitDepth==32)result[i+3]=value;}
   if(sourceDibBitDepth==24)result[i+3]=255;
  }
  return result;
 }
}
