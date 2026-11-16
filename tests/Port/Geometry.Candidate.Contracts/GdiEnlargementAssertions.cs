using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class GdiEnlargementAssertions
{
 public static string Run(){int cases=0;using var stream=typeof(GdiEnlargementAssertions).Assembly.GetManifestResourceStream("GdiEnlargementReference")??throw new Exception("Enlargement fixture missing");using var brotli=new BrotliStream(stream,CompressionMode.Decompress);using var doc=JsonDocument.Parse(brotli);var root=doc.RootElement;var sources=root.GetProperty("Sources").EnumerateArray().ToDictionary(s=>s.GetProperty("Id").GetString()!);string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
  foreach(var c in root.GetProperty("Cases").EnumerateArray()){var s=sources[c.GetProperty("SourceId").GetString()!];int sw=s.GetProperty("Width").GetInt32(),sh=s.GetProperty("Height").GetInt32(),depth=s.GetProperty("Depth").GetInt32(),stride=s.GetProperty("Stride").GetInt32(),w=c.GetProperty("Width").GetInt32(),h=c.GetProperty("Height").GetInt32(),destinationDepth=c.GetProperty("Depth").GetInt32();var rgba=Convert.FromBase64String(s.GetProperty("RgbaBase64").GetString()!);byte[] input=new byte[stride*sh];Array.Fill(input,(byte)165);for(int y=0;y<sh;y++)for(int x=0;x<sw;x++){int i=(y*sw+x)*4,j=(sh-1-y)*stride+x*depth/8;input[j]=rgba[i+2];input[j+1]=rgba[i+1];input[j+2]=rgba[i];if(depth==32)input[j+3]=rgba[i+3];}if(Hash(input)!=s.GetProperty("StoredSha256").GetString())throw new Exception("Native enlargement source storage hash");var copy=(byte[])input.Clone();var output=Rhino5GdiHalftoneStretch.ResizeStored(input,sw,sh,depth,w,h,(byte)c.GetProperty("Seed").GetInt32(),null,64*1024*1024,destinationDepth);
   if(Hash(output)!=c.GetProperty("StoredSha256").GetString())throw new Exception("Native enlargement mismatch "+s.GetProperty("Id").GetString()+" -> "+w+"x"+h+" b"+destinationDepth);if(!input.AsSpan().SequenceEqual(copy))throw new Exception("Source mutated");cases++;
  }
  if(cases!=1408)throw new Exception("Incomplete enlargement/native depth controls");return "PASS GDI enlargement:"+cases+" exact complete native buffers across24/32 source/output, fast/general/unity/single axes and1100/16384 boundaries; default positive rectangles only, original driver/profile/PC/physical separate.";
 }
}
