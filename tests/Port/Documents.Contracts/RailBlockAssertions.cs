using System.Text.Json;
using System.Buffers.Binary;
using JewelDesign.Documents;
namespace DocumentContractChecks;

internal static class RailBlockAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Throws<T>(Action action)where T:Exception
    {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static uint Crc(ReadOnlySpan<byte> bytes)
    {uint crc=uint.MaxValue;foreach(byte b in bytes){crc^=b;for(int i=0;i<8;i++)crc=(crc&1)!=0?0xedb88320U^(crc>>1):crc>>1;}return ~crc;}
    public static string Run(LegacyCsg4Document document,byte[] original)
    {
        var ring=document.Records[4].SimpleBlock??throw new Exception("Missing ring consumer header");
        var outside=document.Records[13].SimpleBlock??throw new Exception("Missing outside consumer header");
        Check(ring.ObjectIndices.SequenceEqual(new[]{2})&&ring.ChannelIndices.SequenceEqual(new[]{0}),"Ring input bindings");
        Check(outside.ObjectIndices.SequenceEqual(new[]{2,6,8,9,10,11})&&outside.ChannelIndices.SequenceEqual(new[]{0,3,0,0,0,0}),"Outside input bindings");
        Check(document.Records.Count(x=>x.Header is not null)==125&&document.Records.Count(x=>x.OpaqueReason is not null)==116&&document.ScalarRecordCount==100,"Header vs scalar boundary");
        foreach(var resource in new[]{"RailHeaderRingOracle","RailHeaderOutsideOracle","RailHeaderSelectOracle","TransformHeaderOracle"}){
            using var stream=typeof(RailBlockAssertions).Assembly.GetManifestResourceStream(resource)!;using var oracle=JsonDocument.Parse(stream);
            foreach(var expected in oracle.RootElement.GetProperty(resource=="TransformHeaderOracle"?"blocks":"rails").EnumerateArray()){
                int ordinal=expected.GetProperty("index").GetInt32();var header=document.Records[ordinal].SimpleBlock!;
                Check(header.Identifier==expected.GetProperty("identifier").GetString()&&header.Flags==expected.GetProperty("flags").GetUInt32()&&header.LegacyLayer==expected.GetProperty("legacyLayer").GetInt32(),"Independent header fields");
                Check(header.ObjectIndices.SequenceEqual(expected.GetProperty("objects").EnumerateArray().Select(x=>x.GetInt32()))&&header.ChannelIndices.SequenceEqual(expected.GetProperty("channels").EnumerateArray().Select(x=>x.GetInt32())),"Independent header links");
            }
        }
        Throws<NotSupportedException>(()=>((IList<int>)outside.ObjectIndices)[0]=1);
        Check(LegacyCsg4Codec.Write(document).AsSpan().SequenceEqual(original),"Rail block structural no-op");
        var changed=document.WithInputLink(13,2,9,0);var reopened=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(changed));
        Check(reopened.Records[13].SimpleBlock!.ObjectIndices[2]==9,"Consumer rewire lost");
        Check(LegacyCsg4Codec.Write(reopened.WithInputLink(13,2,8,0)).AsSpan().SequenceEqual(original),"Consumer rewire reversal changed archive");
        Throws<NotSupportedException>(()=>document.WithScalarValue(13,1));
        Throws<ArgumentOutOfRangeException>(()=>document.WithInputLink(13,6,9,0));
        Throws<NotSupportedException>(()=>document.WithInputLink(0,0,1,0));
        foreach(int index in new[]{4,13,16,49,50,111}){
            var record=document.Records[index];var future=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(future.AsSpan(record.OriginalFileOffset+8),99);
            int start=record.OriginalFileOffset,length=record.CopyOriginalRecordBytes().Length;
            BinaryPrimitives.WriteUInt32LittleEndian(future.AsSpan(start+length-4),Crc(future.AsSpan(start+8,length-12)));
            var parsed=LegacyCsg4Codec.Read(future);Check(parsed.Records[index].SimpleBlock is null,"Unknown rail version guessed");
            Check(LegacyCsg4Codec.Write(parsed).AsSpan().SequenceEqual(future),"Unknown consumer changed");
        }
        var mismatch=(byte[])original.Clone();var target=document.Records[13];int frameLength=target.CopyOriginalRecordBytes().Length,crcOffset=target.OriginalFileOffset+frameLength-4;
        BinaryPrimitives.WriteInt32LittleEndian(mismatch.AsSpan(crcOffset-28),5);
        BinaryPrimitives.WriteUInt32LittleEndian(mismatch.AsSpan(crcOffset),Crc(mismatch.AsSpan(target.OriginalFileOffset+8,frameLength-12)));
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(mismatch));
        return "PASS legacy rail block bindings:exact known headers/links, structural no-op/rewire/reopen/reversal and future/slot/non-scalar guards; no complete graph/PC claim.";
    }
}
