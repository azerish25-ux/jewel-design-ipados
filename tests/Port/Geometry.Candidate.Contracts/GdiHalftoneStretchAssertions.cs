using System.Text.Json;
using System.IO.Compression;
using System.Security.Cryptography;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class GdiHalftoneStretchAssertions
{
 public static string Run(){int cases=0,guards=0;string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
  byte[] Stored(byte[] rgba,int w,int h,int depth){int stride=(w*depth/8+3)&~3;byte[] b=new byte[stride*h];Array.Fill(b,(byte)165);for(int y=0;y<h;y++)for(int x=0;x<w;x++){int i=(y*w+x)*4,j=(h-1-y)*stride+x*depth/8;b[j]=rgba[i+2];b[j+1]=rgba[i+1];b[j+2]=rgba[i];if(depth==32)b[j+3]=rgba[i+3];}return b;}
  foreach(string name in new[]{"GdiRgbReference","GdiMixedReference","GdiFixupReference","GdiLargeFixupReference"}){
   using var stream=typeof(GdiHalftoneStretchAssertions).Assembly.GetManifestResourceStream(name)??throw new Exception("Missing native fixture "+name);using var expanded=name=="GdiLargeFixupReference"?new BrotliStream(stream,CompressionMode.Decompress):null;using var doc=JsonDocument.Parse(expanded is null?stream:expanded);var root=doc.RootElement;var sources=root.GetProperty("Sources").EnumerateArray().ToDictionary(x=>x.GetProperty("Id").GetString()!);
   foreach(var c in root.GetProperty("Cases").EnumerateArray()){if(c.GetProperty("EffectiveStretchMode").GetInt32()!=4)continue;var s=sources[c.GetProperty("SourceId").GetString()!];int sw=s.GetProperty("Width").GetInt32(),sh=s.GetProperty("Height").GetInt32(),depth=s.GetProperty("Depth").GetInt32(),w=c.GetProperty("Width").GetInt32(),h=c.GetProperty("Height").GetInt32();byte[] input=name=="GdiLargeFixupReference"?Stored(Convert.FromBase64String(root.GetProperty("SourcePixels").GetProperty(s.GetProperty("RgbaSha256").GetString()!).GetString()!),sw,sh,depth):Convert.FromBase64String(s.GetProperty("StoredBase64").GetString()!);if(Hash(input)!=s.GetProperty("StoredSha256").GetString())throw new Exception("Native source storage hash");var copy=(byte[])input.Clone();var actual=Rhino5GdiHalftoneStretch.ResizeStored(input,sw,sh,depth,w,h,(byte)c.GetProperty("Seed").GetInt32());if(Hash(actual)!=c.GetProperty("StoredSha256").GetString())throw new Exception("HALFTONE mismatch "+s.GetProperty("Id").GetString()+" -> "+w+"x"+h);if(!input.AsSpan().SequenceEqual(copy))throw new Exception("Input mutated");cases++;}
  }
  using(var stream=typeof(GdiHalftoneStretchAssertions).Assembly.GetManifestResourceStream("GdiHalftoneResponse")??throw new Exception("Missing response corpus"))using(var doc=JsonDocument.Parse(stream)){foreach(var c in doc.RootElement.GetProperty("Cases").EnumerateArray()){bool vertical=c.GetProperty("Vertical").GetBoolean();int n=c.GetProperty("Source").GetInt32(),d=c.GetProperty("Target").GetInt32(),depth=c.GetProperty("Depth").GetInt32();var input=Stored(Convert.FromBase64String(c.GetProperty("SourceRgbaBase64").GetString()!),vertical?1:n,vertical?n:1,depth);var actual=Rhino5GdiHalftoneStretch.ResizeStored(input,vertical?1:n,vertical?n:1,depth,vertical?1:d,vertical?d:1,90);if(Hash(actual)!=c.GetProperty("StoredSha256").GetString())throw new Exception("HALFTONE response mismatch "+n+" -> "+d);cases++;}}
  void Reject<T>(Action a)where T:Exception{guards++;try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
  Reject<NotSupportedException>(()=>Rhino5GdiHalftoneStretch.ResizeStored([],1,1,16,1,1));
  Reject<InvalidDataException>(()=>Rhino5GdiHalftoneStretch.ResizeStored([],0,1,24,1,1));
  Reject<InvalidDataException>(()=>Rhino5GdiHalftoneStretch.ResizeStored([],int.MaxValue,1,24,1,1));
  Reject<ArgumentException>(()=>Rhino5GdiHalftoneStretch.ResizeStored([],1,1,24,1,1));
  Reject<InvalidDataException>(()=>Rhino5GdiHalftoneStretch.ResizeStored(new byte[4],1,1,32,2,2,0,new(MaxPixels:1)));
  Reject<InvalidDataException>(()=>Rhino5GdiHalftoneStretch.ResizeStored(new byte[4],1,1,32,2,2,0,null,1));
  Reject<ArgumentOutOfRangeException>(()=>Rhino5GdiHalftoneStretch.ResizeStored(new byte[4],1,1,32,1,1,0,null,0));
  if(cases!=3860)throw new Exception("Incomplete managed native coverage "+cases);
  return "PASS managed GDI HALFTONE:"+cases+" exact complete stored-buffer cases/"+guards+" explicit storage/depth/domain/budget guards; default current-host component only, source integration/vendor/PC/physical parity open.";
 }
}
