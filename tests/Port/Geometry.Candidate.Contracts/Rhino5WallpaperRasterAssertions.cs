using System.Security.Cryptography;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5WallpaperRasterAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action act) where T:Exception {checks++;try{act();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  void Size(int w,int h,int sw,int sh,int fw,int fh,int aw,int divisor)
  {
   var p=Rhino5WallpaperRaster.Plan(w,h,sw,sh);
   Check(p.FittedWidth==fw&&p.Height==fh&&p.StorageWidth==aw&&p.DownscaleDivisor==divisor,"Original aspect/divisor/alignment dimensions");
  }
  Size(1920,1080,4000,3000,1440,1080,1440,1);
  Size(1001,1000,16,9,1001,563,1004,1);
  Size(1000,500,9,16,281,500,284,1);
  Size(300,1000,9,16,300,533,300,1);
  Size(100,50,1,1,50,50,52,1);
  Size(50,100,1,1,50,50,52,1);
  Size(1,1,1,100000,0,1,0,1);
  Size(1,1,100000,1,1,0,4,1);
  Size(3000,3000,1,1,3000,3000,3000,1);
  Size(3001,3000,1,1,1500,1500,1500,2);
  Size(6000,6000,1,1,2000,2000,2000,3);
  Size(9000001,1,1,1,0,0,0,2);
  Size(9000000,1,1,1,1,1,4,1);
  Size(9999,9999,1,1,2499,2499,2500,4);
  foreach(int bad in new[]{0,-1,int.MinValue})
  {
   Expect<ArgumentOutOfRangeException>(()=>Rhino5WallpaperRaster.Plan(bad,100,20,10));
   Expect<ArgumentOutOfRangeException>(()=>Rhino5WallpaperRaster.Plan(100,bad,20,10));
   Expect<ArgumentOutOfRangeException>(()=>Rhino5WallpaperRaster.Plan(100,100,bad,10));
   Expect<ArgumentOutOfRangeException>(()=>Rhino5WallpaperRaster.Plan(100,100,20,bad));
  }
  Expect<ArgumentOutOfRangeException>(()=>Rhino5WallpaperRaster.Plan(65536,65536,20,10));
  Size(int.MaxValue,1,int.MaxValue,1,0,0,0,16);
  Check(!Rhino5WallpaperRaster.Plan(9000001,1,1,1).AllocationRequested,"Zero recursive input rejects before allocation");
  Check(Rhino5WallpaperRaster.Plan(1,1,1,100000).AllocationRequested,"Zero aspect fit still attempts original allocation");
  byte[] colors=[0,0,255,0,255,0,255,0,0,255,255,255,0,0,0,99,88];
  Rhino5WallpaperRaster.GrayscaleBgr24Storage(colors);
  Check(colors.AsSpan().SequenceEqual(new byte[]{76,76,76,150,150,150,28,28,28,254,254,254,0,0,0,99,88}),"Exact weights/truncation, BGR order, white254 and untouched trailing bytes");
  Rhino5WallpaperRaster.GrayscaleBgr24Storage(Span<byte>.Empty);
  Check(true,"Empty storage legal");
  byte[] section=[77,0,0,255,88];Rhino5WallpaperRaster.GrayscaleBgr24Storage(section.AsSpan(1,3));
  Check(section.AsSpan().SequenceEqual(new byte[]{77,76,76,76,88}),"Only supplied span mutates");
  using var hash=IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
  byte[] plane=new byte[256*256*3];
  for(int r=0;r<256;r++)
  {
   int k=0;for(int b=0;b<256;b++)for(int g=0;g<256;g++){plane[k++]=(byte)b;plane[k++]=(byte)g;plane[k++]=(byte)r;}
   Rhino5WallpaperRaster.GrayscaleBgr24Storage(plane);hash.AppendData(plane);
  }
  Check(Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant()=="5966f944c04aa9b94a8d4af30c222fefd55c04e0ffe47d8e3609fe9f2225b509","Every RGB combination matches static source-ordered JavaScript golden; no FMA or alternate weights");
  return "PASS original-derived wallpaper raster:"+checks+" sizing/buffer checks and16777216 color triples; decode/GDI/Metal/SetView/PC/physical acceptance separate.";
 }
}
