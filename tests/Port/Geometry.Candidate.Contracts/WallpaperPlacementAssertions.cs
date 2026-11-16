using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class WallpaperPlacementAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expected(int cw,int ch,int iw,int ih,int scale,int x,int y,int w,int h)
  {var p=Rhino5WallpaperPlacement.Gdi(cw,ch,iw,ih,scale);Check(p.Left==x&&p.Top==y&&p.Width==w&&p.Height==h,"Original GDI centered draw rectangle");}
  Expected(101,99,7,5,1,47,47,7,5);Expected(101,99,7,5,3,40,42,21,15);
  Expected(10,10,15,15,1,-2,-2,15,15);Expected(101,99,7,5,0,47,47,7,5);Expected(101,99,7,5,-4,47,47,7,5);
  var p=Rhino5WallpaperPlacement.Gdi(100,100,100,100,1);Check(p.CopyClipQuad().AsSpan().SequenceEqual(new float[]{1,-1,0,1,1,1,0,1,-1,1,0,1,-1,-1,0,1}),"Top-row-first source quad maps exact pixel rectangle to Metal clip");
  foreach(var v in new[]{0,-1}){checks++;try{Rhino5WallpaperPlacement.Gdi(v,10,2,2,1);throw new Exception("Expected dimensions failure");}catch(ArgumentOutOfRangeException){}}
  checks++;try{Rhino5WallpaperPlacement.Gdi(100,100,int.MaxValue,1,2);throw new Exception("Expected native multiplication overflow rejection");}catch(ArgumentOutOfRangeException){}
  return "PASS original GDI wallpaper placement:"+checks+" centering/scale/clip/boundary cases; resampling/GL/source document/PC parity separate.";
 }
}
