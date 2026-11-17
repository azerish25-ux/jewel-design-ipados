using System.Buffers.Binary;
using System.IO.Compression;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class NlsFilenamePolicyAssertions
{
    private sealed class Resource : IDisposable { public bool Disposed; public void Dispose()=>Disposed=true; }
    public static string Run()
    {
        var policy = Rhino5FilenameSemantics.UnicodeNls1Reference60502;
        int checks=0, pairs=0;
        void Check(bool value,string message){checks++;if(!value)throw new Exception(message);}
        string?[] boundaries=[null,"","\0","\0tail","A","a","A\0B","a\0C"];
        foreach(var left in boundaries) foreach(var right in boundaries)
        {
            int expected=string.IsNullOrEmpty(right)||right[0]==0?(string.IsNullOrEmpty(left)?0:1):string.IsNullOrEmpty(left)?-1:Rhino5NlsDirectComparer.Reference60502.Compare(left,right);
            Check(policy.Compare(left,right)==expected,"Stored-length/terminator wrapper changed");
        }
        Check(policy.Compare("\0tail","")==1,"Stored length replaced by C-string length");
        Check(policy.Compare("A\0B","a\0C")==0,"Comparison crossed C terminator");
        Check(policy.Lower("A\0B")=="a\0b","Explicit-length NLS lowercase stopped at NUL");
        Check(policy.Lower("ÉＡ")=="éａ","NLS lowercase silently used CRT-C");
        try {policy.Lower(null!);throw new Exception("Null lower accepted");}catch(ArgumentNullException){checks++;}
        using var stream=typeof(NlsFilenamePolicyAssertions).Assembly.GetManifestResourceStream("NlsDirectStringReference")??throw new Exception("Native fixture missing");
        using var compressed=new BrotliStream(stream,CompressionMode.Decompress);
        using var fixture=JsonDocument.Parse(compressed);
        string[] names=fixture.RootElement.GetProperty("Strings").EnumerateArray().Select(s=>Decode(Convert.FromBase64String(s.GetProperty("InputUtf16Base64").GetString()!))).ToArray();
        foreach(var pair in fixture.RootElement.GetProperty("Pairs").EnumerateArray())
        {
            string left=names[pair.GetProperty("Left").GetInt32()],right=names[pair.GetProperty("Right").GetInt32()];
            int expected=right.Length==0||right[0]==0?(left.Length>0?1:0):left.Length==0?-1:pair.GetProperty("NativeResult").GetInt32()-2;
            if(policy.Compare(left,right)!=expected)throw new Exception("NLS filename wrapper native pair "+pairs);
            pairs++;
        }
        Check(pairs==40663,"Native pair coverage");
        int created=0;
        using var cache=new Rhino5ContextImageCache<Resource>(policy);
        using var sharp=cache.Lookup(1,"ß.PNG",false,()=>{created++;return new();})!;
        using var ss=cache.Lookup(1,"ss.png",false,()=>throw new Exception("NLS expansion alias missed"))!;
        Check(ReferenceEquals(sharp.Resource,ss.Resource)&&created==1,"Expansion identity changed");
        using var kana=cache.Lookup(1,"カナ.PNG",false,()=>{created++;return new();})!;
        using var half=cache.Lookup(1,"ｶﾅ.png",false,()=>throw new Exception("NLS width alias missed"))!;
        Check(ReferenceEquals(kana.Resource,half.Resource)&&created==2,"Width identity changed");
        Check(cache.Snapshot().Any(e=>e.Key.Filename=="ß.png"),"Stored filename was not NLS-lowered");
        Check(cache.Invalidate("SS.PNG")==1&&cache.Count==1&&!sharp.Resource.Disposed,"Alias invalidation/lease ownership changed");
        Check(Rhino5FilenameSemantics.CrtCLocale.Compare("ß.PNG","ss.png")!=0,"Explicit CRT profile changed");
        return $"PASS NLS1 filename policy:{pairs} native wrapper pairs/{checks} boundary/cache checks; fixed successful profile, CRT/ANSI failure/state/document/PC/physical remain open.";
    }
    private static string Decode(byte[] bytes){char[] chars=new char[bytes.Length/2];for(int i=0;i<chars.Length;i++)chars[i]=(char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i*2));return new(chars);}
}
