using System.Buffers.Binary;
using System.Globalization;
using System.Text.Json;
using JewelDesign.Documents;
namespace DocumentContractChecks;

public static class AdditionalScalarAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Same(byte[] a,byte[] b,string message)=>Check(a.AsSpan().SequenceEqual(b),message);
    private static void Throws<T>(Action action) where T:Exception
    {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static string Bits(double value)=>unchecked((ulong)BitConverter.DoubleToInt64Bits(value)).ToString("x16",CultureInfo.InvariantCulture);
    private static uint Crc(ReadOnlySpan<byte> bytes){uint crc=uint.MaxValue;foreach(byte value in bytes){crc^=value;for(int i=0;i<8;i++)crc=(crc&1)==0?crc>>1:0xedb88320^(crc>>1);}return ~crc;}
    private static void LeafCrc(byte[] bytes,LegacyCsg4Record record)
    {int start=record.OriginalFileOffset,length=record.CopyOriginalRecordBytes().Length;BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(start+length-4),Crc(bytes.AsSpan(start+8,length-12)));}
    public static string Run(LegacyCsg4Document document,byte[] original,string? outputFolder)
    {
        Check(document.ScalarRecordCount==100&&document.NumericRecordCount==74,"Additional scalar counts");
        Same(original,LegacyCsg4Codec.Write(document),"Additional scalars changed no-op archive");
        using var oracleStream=typeof(AdditionalScalarAssertions).Assembly.GetManifestResourceStream("PlainScalarOracle")!;
        using var oracle=JsonDocument.Parse(oracleStream);
        var extras=oracle.RootElement.GetProperty("extras").EnumerateArray().ToArray();
        Check(extras.Length==26,"Missing independent scalar oracle");
        var exportCases=new List<(string File,int Index,string Before,string After)>();
        var exportedTypes=new HashSet<Guid>();
        foreach(var expected in extras){
            int index=expected.GetProperty("index").GetInt32();var record=document.Records[index];
            var scalar=record.Scalar??throw new Exception("Missing supported plain scalar");
            Check(record.Numeric is null&&record.OpaqueReason is null,"Plain scalar gained fabricated numeric bounds");
            Check(record.TypeGuid==Guid.Parse(expected.GetProperty("guid").GetString()!),"Scalar type GUID");
            Check(record.Version==new LegacyCsg4Version(1,1),"Unsupported scalar profile guessed");
            Check(scalar.Identifier==expected.GetProperty("identifier").GetString()&&scalar.Flags==expected.GetProperty("flags").GetUInt32()&&scalar.LegacyLayer==expected.GetProperty("legacyLayer").GetInt32(),"Scalar header oracle mismatch");
            Check(scalar.LegacyUiInteger==expected.GetProperty("legacyUiInteger").GetInt32()&&scalar.OriginalValueOffset==expected.GetProperty("valueOffset").GetInt32(),"Scalar legacy UI/offset provenance");
            Check(scalar.ObjectIndices.SequenceEqual(expected.GetProperty("objects").EnumerateArray().Select(x=>x.GetInt32()))&&scalar.ChannelIndices.SequenceEqual(expected.GetProperty("channels").EnumerateArray().Select(x=>x.GetInt32()))&&scalar.CategoryIndices.SequenceEqual(expected.GetProperty("categories").EnumerateArray().Select(x=>x.GetInt32())),"Scalar link/category order");
            var userData=expected.GetProperty("userData").EnumerateArray().ToArray();Check(userData.Length==scalar.UserData.Count,"Scalar user data count");
            for(int u=0;u<userData.Length;u++)Check(scalar.UserData[u]==new LegacyUserDataEntry(userData[u].GetProperty("type").GetString()!,userData[u].GetProperty("key").GetString()!,userData[u].GetProperty("value").GetString()!),"Scalar user data fidelity");
            Check(Bits(scalar.Value)==expected.GetProperty("valueBits").GetString(),"Scalar stored value bits");
            Throws<NotSupportedException>(()=>document.WithNumericValue(index,.125));
            double next=scalar.Value==0?.125:0;
            var edited=LegacyCsg4Codec.Write(document.WithScalarValue(index,next));var reopened=LegacyCsg4Codec.Read(edited);
            Check(Bits(reopened.Records[index].Scalar!.Value)==Bits(next),"Scalar edit/reopen");
            for(int other=0;other<document.Records.Count;other++)if(other!=index)Same(document.Records[other].CopyOriginalRecordBytes(),reopened.Records[other].CopyOriginalRecordBytes(),"Scalar edit changed another record");
            Same(original,LegacyCsg4Codec.Write(reopened.WithScalarValue(index,scalar.Value)),"Scalar reversal not byte-identical");
            if(outputFolder is not null&&exportedTypes.Add(record.TypeGuid)){
                string folder=Path.Combine(outputFolder,"additional-scalars");Directory.CreateDirectory(folder);
                string name="scalar-"+exportCases.Count+".csg";File.WriteAllBytes(Path.Combine(folder,name),edited);
                exportCases.Add((name,index,Bits(scalar.Value),Bits(next)));
            }
        }
        int selectedIndex=extras[0].GetProperty("index").GetInt32();var selected=document.Records[selectedIndex];
        foreach(long bits in new[]{long.MinValue,0x7ff0000000000000L,0x7ff8000000000042L}){
            var data=LegacyCsg4Codec.Write(document.WithScalarValue(selectedIndex,BitConverter.Int64BitsToDouble(bits)));
            Check(BitConverter.DoubleToInt64Bits(LegacyCsg4Codec.Read(data).Records[selectedIndex].Scalar!.Value)==bits,"Plain scalar IEEE754 bits normalized");
        }
        var future=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(future.AsSpan(selected.OriginalFileOffset+8),99);LeafCrc(future,selected);
        var futureDoc=LegacyCsg4Codec.Read(future);Check(futureDoc.Records[selectedIndex].Scalar is null,"Future scalar decoded speculatively");
        Same(future,LegacyCsg4Codec.Write(futureDoc),"Future scalar not retained opaque");
        Throws<NotSupportedException>(()=>futureDoc.WithScalarValue(selectedIndex,0));
        var malformed=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(malformed.AsSpan(selected.OriginalFileOffset+16),int.MaxValue);LeafCrc(malformed,selected);
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(malformed));
        var badLinks=(byte[])original.Clone();int channelOffset=extras[0].GetProperty("channelCountOffset").GetInt32();
        BinaryPrimitives.WriteInt32LittleEndian(badLinks.AsSpan(channelOffset),extras[0].GetProperty("channels").GetArrayLength()+1);LeafCrc(badLinks,selected);
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(badLinks));
        if(outputFolder is not null){
            string folder=Path.Combine(outputFolder,"additional-scalars");File.WriteAllBytes(Path.Combine(folder,"source.csg"),original);
            using var stream=File.Create(Path.Combine(folder,"cases.json"));using var writer=new Utf8JsonWriter(stream,new(){Indented=true});
            writer.WriteStartArray();foreach(var item in exportCases){writer.WriteStartObject();writer.WriteString("file",item.File);writer.WriteNumber("index",item.Index);writer.WriteString("beforeBits",item.Before);writer.WriteString("afterBits",item.After);writer.WriteEndObject();}writer.WriteEndArray();
        }
        return "PASS additional scalar codec: 26 source/oracle-matched Value/OnOff/Chooser records, 100 total scalars/74 bounded numeric; structural edit/reopen/reversal and other-record/bit/version/bounds fidelity; no UI/graph regeneration or PC claim.";
    }
}
