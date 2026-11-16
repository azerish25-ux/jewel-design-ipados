using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class TexturePreparationAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Size(int w,int h,int bits,int max,uint budget,float fraction,bool power,int ew,int eh){
   var p=Rhino5TexturePreparation.Dimensions(w,h,bits,max,budget,fraction,power);Check(p.Width==ew&&p.Height==eh,"Original texture dimension/power/cross-axis/single-precision budget order");
  }
  Size(1000,600,32,2048,uint.MaxValue,1,false,1000,600);
  Size(1000,600,32,2048,uint.MaxValue,1,true,1024,1024);
  Size(3000,1000,32,2048,16*1024*1024,.5f,false,2048,1024);
  Size(1000,3000,32,2048,uint.MaxValue,1,false,1024,2048);
  Size(3000,1000,32,2048,16*1024*1024,MathF.BitDecrement(.5f),false,1024,512);
  Size(3000,1000,8,2048,16*1024*1024,.375f,false,2048,1024);
  Size(3000,1000,8,2048,16*1024*1024,MathF.BitDecrement(.375f),false,1024,512);
  Size(9,5,24,10,uint.MaxValue,1,false,10,8);
  Size(1000,600,32,2048,uint.MaxValue,float.NaN,false,1000,600);
  Size(1000,600,32,2048,0,float.PositiveInfinity,false,1000,600);
  Size(3,1,32,2048,0,.5f,false,1,0);
  Size(3,1,32,2048,100,0,false,1,0);
  byte[] source=new byte[4*3*4],dest=new byte[3*2*4];
  byte[] storedAlpha=[3,5,9,17,16,32,64,128,1,2,4,8];
  for(int i=0;i<12;i++){source[i*4]=7;source[i*4+1]=9;source[i*4+2]=11;source[i*4+3]=storedAlpha[i];}
  for(int i=0;i<6;i++){dest[i*4]=21;dest[i*4+1]=43;dest[i*4+2]=65;dest[i*4+3]=128;}
  Rhino5TexturePreparation.RepairAlphaStoredRows(source,4,3,16,dest,3,2,12);
  Check(Enumerable.Range(0,6).Select(i=>dest[i*4+3]).SequenceEqual(new byte[]{131,133,137,144,160,192}),"Native nearest alpha sampled in DIB stored-row order and bitwise OR");
  Check(Enumerable.Range(0,6).All(i=>dest[i*4]==21&&dest[i*4+1]==43&&dest[i*4+2]==65),"Alpha repair never changes destination RGB");
  byte[] rgbaSource=new byte[48],rgbaDest=new byte[24];
  for(int y=0;y<3;y++)source.AsSpan((2-y)*16,16).CopyTo(rgbaSource.AsSpan(y*16));
  for(int i=0;i<6;i++)rgbaDest[i*4+3]=128;
  Rhino5TexturePreparation.RepairAlphaTopDownRgba(rgbaSource,4,3,rgbaDest,3,2);
  Check(Enumerable.Range(0,6).Select(i=>rgbaDest[i*4+3]).SequenceEqual(new byte[]{144,160,192,131,133,137}),"Top-row-first adapter preserves original bottom-up sampling asymmetry");
  void Fail<T>(Action action)where T:Exception{checks++;try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  Fail<ArgumentOutOfRangeException>(()=>Rhino5TexturePreparation.Dimensions(0,1,32,2048,100,.5f,false));
  Fail<ArgumentOutOfRangeException>(()=>Rhino5TexturePreparation.Dimensions(1,1,32,0,100,.5f,false));
  Fail<ArgumentOutOfRangeException>(()=>Rhino5TexturePreparation.Dimensions(1,1,32,2048,100,-.5f,false));
  Fail<ArgumentOutOfRangeException>(()=>Rhino5TexturePreparation.Dimensions(1<<30,1<<30,32,1<<30,uint.MaxValue,1,false));
  Fail<ArgumentException>(()=>Rhino5TexturePreparation.RepairAlphaStoredRows(source,4,3,16,dest,3,2,11));
  Fail<ArgumentException>(()=>Rhino5TexturePreparation.RepairAlphaStoredRows(source,4,3,16,source,3,2,12));
  return "PASS original texture preparation:"+checks+" dimension/budget/alpha-OR/row-orientation/boundary cases; GDI RGB filtering/source cache/PC parity separate.";
 }
}
