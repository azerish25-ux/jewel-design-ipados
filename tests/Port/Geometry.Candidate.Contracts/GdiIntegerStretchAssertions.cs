using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class GdiIntegerStretchAssertions
{
 public static string Run(){int exact=0,halftone=0,guards=0;
  foreach(string fixture in new[]{"GdiRgbReference","GdiIntegerReference"}){
   using var stream=typeof(GdiIntegerStretchAssertions).Assembly.GetManifestResourceStream(fixture)??throw new Exception("GDI corpus missing");using var doc=JsonDocument.Parse(stream);var root=doc.RootElement;
   var sources=root.GetProperty("Sources").EnumerateArray().ToDictionary(x=>x.GetProperty("Id").GetString()!);
   foreach(var c in root.GetProperty("Cases").EnumerateArray()){
    var s=sources[c.GetProperty("SourceId").GetString()!];int sw=s.GetProperty("Width").GetInt32(),sh=s.GetProperty("Height").GetInt32(),dw=c.GetProperty("Width").GetInt32(),dh=c.GetProperty("Height").GetInt32(),depth=c.GetProperty("Depth").GetInt32(),mode=c.GetProperty("EffectiveStretchMode").GetInt32();byte seed=(byte)c.GetProperty("Seed").GetInt32();
    byte[] input=Convert.FromBase64String(s.GetProperty("StoredBase64").GetString()!),copy=(byte[])input.Clone(),expected=Convert.FromBase64String(c.GetProperty("StoredBase64").GetString()!);
    if(c.GetProperty("RequestedMode").GetInt32()==0&&Rhino5GdiIntegerStretch.SelectTextureMode(sw,sh,dw,dh,c.GetProperty("PreviousStretchMode").GetInt32())!=mode)throw new Exception("Source ratio mode boundary");
    if(mode==4){try{Rhino5GdiIntegerStretch.ResizeStored(input,sw,sh,depth,dw,dh,mode,seed);throw new Exception("HALFTONE silently approximated");}catch(NotSupportedException){halftone++;}continue;}
    var actual=Rhino5GdiIntegerStretch.ResizeStored(input,sw,sh,depth,dw,dh,mode,seed);
    if(!actual.AsSpan().SequenceEqual(expected))throw new Exception("Native stored RGB/alpha/padding mismatch "+s.GetProperty("Id").GetString()+" -> "+dw+"x"+dh+" mode"+mode);
    if(!input.AsSpan().SequenceEqual(copy))throw new Exception("Input mutated");exact++;
   }
  }
  void Reject<T>(Action a)where T:Exception{guards++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  Reject<NotSupportedException>(()=>Rhino5GdiIntegerStretch.ResizeStored([],1,1,16,1,1,1));
  Reject<NotSupportedException>(()=>Rhino5GdiIntegerStretch.ResizeStored([0,0,0,0],1,1,32,1,1,2));
  Reject<ArgumentOutOfRangeException>(()=>Rhino5GdiIntegerStretch.ResizeStored([],1,1,24,1,1,0));
  Reject<InvalidDataException>(()=>Rhino5GdiIntegerStretch.ResizeStored([],0,1,24,1,1,1));
  Reject<InvalidDataException>(()=>Rhino5GdiIntegerStretch.ResizeStored([],int.MaxValue,1,24,1,1,1));
  Reject<ArgumentException>(()=>Rhino5GdiIntegerStretch.ResizeStored([0,0,0],1,1,24,1,1,1));
  Reject<ArgumentException>(()=>Rhino5GdiIntegerStretch.ResizeStored(new byte[5],1,1,32,1,1,3));
  Reject<InvalidDataException>(()=>Rhino5GdiIntegerStretch.ResizeStored(new byte[4],1,1,32,2,2,3,0,new(MaxPixels:1)));
  Reject<ArgumentOutOfRangeException>(()=>Rhino5GdiIntegerStretch.SelectTextureMode(1,1,0,1));
  if(exact!=1596||halftone!=132)throw new Exception("Incomplete native integer/HALFTONE coverage");
  return "PASS measured GDI integer stretch:"+exact+" exact native-byte cases/"+halftone+" explicit HALFTONE rejections/"+guards+" storage/depth/budget guards; original vendor/driver/HALFTONE/full RGB/PC parity separate.";
 }
}
