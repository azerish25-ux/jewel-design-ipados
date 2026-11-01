using System.Text.Json;
using System.Buffers.Binary;
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks;
public static class LegacyUserStringListAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static uint Crc(ReadOnlySpan<byte> bytes){uint c=uint.MaxValue;foreach(byte b in bytes){c^=b;for(int i=0;i<8;i++)c=(c&1)!=0?(c>>1)^0xedb88320:c>>1;}return ~c;}
    private static string Utf16(string hex){if(hex=="-")return string.Empty;var b=Convert.FromHexString(hex);return string.Create(b.Length/2,b,static(chars,bytes)=>{for(int i=0;i<chars.Length;i++)chars[i]=(char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(2*i,2));});}
    public static string Run()
    {
        checks=0;var pairs=new[]{new KeyValuePair<string,string>("key","value"),new("key","値 💎"),new(string.Empty,string.Empty),new("a\0b","x\0y")};
        foreach(int version in new[]{4,5,50})
        {
            var bytes=LegacyUserStringListCodec.Write(pairs,version);var read=LegacyUserStringListCodec.Read(bytes,version);
            Check(read.NativeReadSucceeded&&read.CrcErrorCount==0&&read.CopyPairs().SequenceEqual(pairs),"Actual native string-list schema round trip");Check(read.ConsumedBytes==bytes.Length,"Exactly one native body consumed");
            var corrupted=(byte[])bytes.Clone();corrupted[^1]^=1;read=LegacyUserStringListCodec.Read(corrupted,version);Check(read.NativeReadSucceeded&&read.CrcErrorCount==1&&read.CopyPairs().SequenceEqual(pairs),"CRC mismatch is diagnostic independently of native bool");
            int header=version<50?8:12;int first=header+12;long firstSize=version<50?BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(first+4)):BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(first+4));int second=checked(first+header+(int)firstSize);
            var partial=(byte[])bytes.Clone();BinaryPrimitives.WriteInt32LittleEndian(partial.AsSpan(second+header),2);read=LegacyUserStringListCodec.Read(partial,version);Check(!read.NativeReadSucceeded&&read.CopyPairs().Length==1&&read.CopyPairs()[0].Equals(pairs[0]),"Later unsupported entry retains earlier decoded pairs");
            var term=(byte[])bytes.Clone();int keyTerm=first+header+12+"key".Length*2;BinaryPrimitives.WriteUInt16LittleEndian(term.AsSpan(keyTerm),65);int crc=first+header+(int)firstSize-4;BinaryPrimitives.WriteUInt32LittleEndian(term.AsSpan(crc),Crc(term.AsSpan(first+header,(int)firstSize-4)));read=LegacyUserStringListCodec.Read(term,version);Check(read.NativeReadSucceeded&&read.CrcErrorCount==0&&read.CopyPairs()[0].Key=="key","Windows native reader discards final UTF16 element without requiring null");
            var empty=LegacyUserStringListCodec.Read(LegacyUserStringListCodec.Write(Array.Empty<KeyValuePair<string,string>>(),version),version);Check(empty.NativeReadSucceeded&&empty.CopyPairs().Length==0,"Empty list native body");
        }
        foreach(int unsupported in new[]{-1,0,1,2,3,6,49,51})
        {
            bool rejectedRead=false,rejectedWrite=false;
            try{LegacyUserStringListCodec.Read([],unsupported);}catch(ArgumentOutOfRangeException){rejectedRead=true;}
            try{LegacyUserStringListCodec.Write([],unsupported);}catch(ArgumentOutOfRangeException){rejectedWrite=true;}
            Check(rejectedRead&&rejectedWrite,"Unsupported archive context must be explicit");
        }
        var bounded=LegacyUserStringListCodec.Read(LegacyUserStringListCodec.Write(pairs,50),50,new(MaxPairs:1));
        Check(bounded.ManagedLimitHit&&!bounded.NativeReadSucceeded&&bounded.CopyPairs().Length==1,"Managed entry limit retains prefix distinctly from native failure");
        var textBound=LegacyUserStringListCodec.Read(LegacyUserStringListCodec.Write(pairs,4),4,new(MaxStringCodeUnits:2));
        Check(textBound.ManagedLimitHit&&!textBound.NativeReadSucceeded,"Managed UTF16 bound");
        var future=LegacyUserStringListCodec.Write(pairs,50);BinaryPrimitives.WriteInt32LittleEndian(future.AsSpan(16),1);
        var futureRead=LegacyUserStringListCodec.Read(future,50);
        Check(futureRead.NativeReadSucceeded&&futureRead.HasUnknownSchema,"Future minor schema explicitly retained");
        var original=futureRead.CopyOriginal();original[0]^=1;var copy=futureRead.CopyPairs();copy[0]=new("changed","changed");
        Check(futureRead.CopyOriginal().SequenceEqual(future)&&futureRead.CopyPairs()[0].Equals(pairs[0]),"Read results retain independent provenance and values");
        using var stream=typeof(LegacyUserStringListAssertions).Assembly.GetManifestResourceStream("UserStringBodySourceCases")!;using var corpus=JsonDocument.Parse(stream);
        foreach(var row in corpus.RootElement.GetProperty("writes").EnumerateArray())
        {
            var values=row.GetProperty("pairs").EnumerateArray().Select(x=>new KeyValuePair<string,string>(x[0].GetString()!,x[1].GetString()!)).ToArray();
            Check(Convert.ToHexString(LegacyUserStringListCodec.Write(values,row.GetProperty("version").GetInt32())).Equals(row.GetProperty("hex").GetString(),StringComparison.OrdinalIgnoreCase),"Full native writer byte comparison");
        }
        int nativeCases=0;
        foreach(var row in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string label=row.GetProperty("name").GetString()!;var read=LegacyUserStringListCodec.Read(Convert.FromHexString(row.GetProperty("hex").GetString()!),row.GetProperty("version").GetInt32());var expected=row.GetProperty("expected");
            Check(read.NativeReadSucceeded==expected.GetProperty("ok").GetBoolean(),label+" native bool");Check(read.CrcErrorCount==expected.GetProperty("crcErrors").GetInt32(),label+" native CRC count");Check(read.ConsumedBytes==expected.GetProperty("consumed").GetInt32(),label+" native cursor");
            var initial=row.GetProperty("initial").EnumerateArray().Select(x=>new KeyValuePair<string,string>(x[0].GetString()!,x[1].GetString()!));var actual=initial.Concat(read.CopyPairs()).ToArray();var wanted=expected.GetProperty("pairs").EnumerateArray().Select(x=>new KeyValuePair<string,string>(Utf16(x[0].GetString()!),Utf16(x[1].GetString()!))).ToArray();Check(actual.SequenceEqual(wanted),label+" native retained pairs");nativeCases++;
        }
        Check(nativeCases==1076,"Full native read case count");
        return "PASS legacy native user-string bodies:"+checks+" checks including "+nativeCases+" full-library native reads and12 byte comparisons; full3dm archive/original-PC parity separate.";
    }
}
