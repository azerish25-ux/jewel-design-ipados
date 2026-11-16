using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class StraightImageProviderAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Expect<T>(Action a)where T:Exception{checks++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  byte[] wanted=[17,31,77,1,211,43,19,0,128,129,130,127];
  foreach(uint alpha in new uint[]{3,4,5,6})foreach(uint order in new uint[]{0,0x4000,0x2000})
  {
   byte[] raw=new byte[16];Array.Fill(raw,(byte)222);byte[] expected=(byte[])wanted.Clone();
   for(int i=0;i<3;i++){
    int p=i*4;byte r=wanted[p],g=wanted[p+1],b=wanted[p+2],a=alpha>=5?(byte)255:wanted[p+3];
    byte[] pixel=alpha is 3 or 5?[r,g,b,a]:[a,r,g,b];
    if(alpha>=5)pixel[alpha==5?3:0]=23;
    if(order==0x2000)Array.Reverse(pixel);pixel.CopyTo(raw,p);expected[p+3]=a;
   }
   var layout=new ImageProviderLayout(8,32,16,alpha|order,1,"kCGColorSpaceSRGB",false);
   var output=StraightRgbaProvider.Extract(3,1,raw,layout);
   Check(output.AsSpan().SequenceEqual(expected),"Straight/skip, first/last, default/big/little byte order");
   Check(raw[15]==222,"Source padding untouched");
  }
  byte[] padded=[17,31,77,1,211,43,19,0,222,222,222,222,128,129,130,127,255,254,253,0];
  var rows=new ImageProviderLayout(8,32,12,3,1,"kCGColorSpaceSRGB",false);
  Check(StraightRgbaProvider.RequiredStorageBytes(2,2,rows)==20,"Final row need not contain unused trailing padding");
  Check(StraightRgbaProvider.Extract(2,2,padded,rows).AsSpan().SequenceEqual(new byte[]{17,31,77,1,211,43,19,0,128,129,130,127,255,254,253,0}),"Top row order and per-row padding");
  var rgb=new ImageProviderLayout(8,24,8,0,1,"kCGColorSpaceSRGB",false);
  Check(StraightRgbaProvider.Extract(2,1,[17,31,77,211,43,19],rgb).AsSpan().SequenceEqual(new byte[]{17,31,77,255,211,43,19,255}),"RGB24 gains opaque alpha without changing colors");
  Expect<InvalidDataException>(()=>StraightRgbaProvider.Extract(2,2,padded.AsSpan(0,19),rows));
  Expect<InvalidDataException>(()=>StraightRgbaProvider.Extract(2,2,padded,rows with{BytesPerRow=7}));
  Expect<InvalidDataException>(()=>StraightRgbaProvider.Extract(2,2,padded,rows with{BytesPerRow=long.MaxValue}));
  Expect<InvalidDataException>(()=>StraightRgbaProvider.Extract(2,2,padded,rows,new(MaxDecodedBytes:15)));
  foreach(var bad in new[]{
   rows with{BitsPerComponent=16},rows with{BitsPerPixel=64},rows with{BitmapInfo=1},rows with{BitmapInfo=2},
   rows with{BitmapInfo=0},rows with{BitmapInfo=7},rows with{BitmapInfo=0x1003},rows with{BitmapInfo=0x103},
   rows with{ColorSpaceModel=0},rows with{ColorSpaceName="Display P3"},rows with{HasDecodeArray=true}})
   Expect<NotSupportedException>(()=>StraightRgbaProvider.Extract(2,2,padded,bad));
  var source=LocalImageAsset.FromBytes("source",[1]);
  using var straight=new DecodedLocalImage(source,3,1,1,1,"test",wanted,RgbaAlphaRepresentation.Straight,rows);
  Check(straight.AlphaRepresentation==RgbaAlphaRepresentation.Straight&&straight.ProviderLayout==rows,"Typed output representation and original provider metadata");
  using var legacy=new DecodedLocalImage(source,3,1,1,1,"test",wanted);
  Check(legacy.AlphaRepresentation==RgbaAlphaRepresentation.Premultiplied,"Existing decoder contract unchanged");
  Expect<ArgumentOutOfRangeException>(()=>new DecodedLocalImage(source,3,1,1,1,"test",wanted,(RgbaAlphaRepresentation)99));
  using var concurrent=new DecodedLocalImage(source,3,1,1,1,"test",wanted);int errors=0;
  Parallel.Invoke(()=>Parallel.For(0,64,_=>{try{if(!concurrent.CopyRgba().AsSpan().SequenceEqual(wanted))Interlocked.Increment(ref errors);}catch(ObjectDisposedException){}catch{Interlocked.Increment(ref errors);}}),()=>concurrent.Dispose());
  Check(errors==0,"Concurrent copy/disposal yields complete pixels or explicit disposal, never partial cleared bytes");
  return "PASS straight image provider:"+checks+" layout/alpha/stride/byte-order/budget/rejection/typed-output cases; native iOS formats/source codec/PC parity separate.";
 }
}
