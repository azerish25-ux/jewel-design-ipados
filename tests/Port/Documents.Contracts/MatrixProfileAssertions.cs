using System.Buffers.Binary;
using System.Text.Json;
using JewelDesign.Documents;
namespace DocumentContractChecks;

internal static class MatrixProfileAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static string Bits(double value)=>unchecked((ulong)BitConverter.DoubleToInt64Bits(value)).ToString("x16");
    private static void Same(byte[] a,byte[] b,string message)=>Check(a.AsSpan().SequenceEqual(b),message);
    private static void Throws<T>(Action action)where T:Exception
    {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static uint Crc(ReadOnlySpan<byte> data)
    {uint crc=uint.MaxValue;foreach(byte b in data){crc^=b;for(int i=0;i<8;i++)crc=(crc&1)!=0?0xedb88320U^(crc>>1):crc>>1;}return ~crc;}
    private static void LeafCrc(byte[] bytes,LegacyCsg4Record record)
    {int p=record.OriginalFileOffset,n=record.CopyOriginalRecordBytes().Length;BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(p+n-4),Crc(bytes.AsSpan(p+8,n-12)));}
    private static byte[] ReplacePayload(byte[] original,LegacyCsg4Document document,LegacyCsg4Record record,byte[] payload)
    {
        int start=record.OriginalFileOffset,oldLength=record.CopyOriginalRecordBytes().Length,newLength=payload.Length+12,delta=newLength-oldLength;
        var result=new byte[original.Length+delta];original.AsSpan(0,start).CopyTo(result);
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(start),0x40008000);BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(start+4),payload.Length+4);
        payload.CopyTo(result,start+8);BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(start+8+payload.Length),Crc(payload));
        original.AsSpan(start+oldLength).CopyTo(result.AsSpan(start+newLength));
        int graph=document.Records[0].OriginalFileOffset-8-12-document.Records.Count*16;
        int graphLength=BinaryPrimitives.ReadInt32LittleEndian(original.AsSpan(graph+4));
        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(graph+4),graphLength+delta);
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(result.Length-4),(uint)result.Length);return result;
    }
    public static string Run(LegacyCsg4Document document,byte[] original)
    {
        Check(document.MatrixProfileRecordCount==6&&document.Records.Where(x=>x.MatrixProfiles is not null).Sum(x=>x.MatrixProfiles!.Curves.Count)==155,"Matrix group/curve counts");
        Check(document.Records.Where(x=>x.MatrixProfiles is not null).Sum(x=>x.MatrixProfiles!.Curves.Sum(c=>c.ControlVertexCount))==4012,"Stored CV count");
        Check(document.ScalarRecordCount==100&&document.Records.Count(x=>x.Header is not null)==134&&document.Records.Count(x=>x.OpaqueReason is not null)==107,"Typed header/scalar/opaque boundaries");
        using var stream=typeof(MatrixProfileAssertions).Assembly.GetManifestResourceStream("MatrixProfileOracle")!;
        using var oracle=JsonDocument.Parse(stream);var groups=oracle.RootElement.GetProperty("groups").EnumerateArray().ToArray();
        Check(groups.Length==6,"Independent group count");
        foreach(var expected in groups){
            int index=expected.GetProperty("index").GetInt32();var record=document.Records[index];
            var group=record.MatrixProfiles??throw new Exception("Missing embedded matrix profiles");
            Check(expected.GetProperty("fullyConsumed").GetBoolean(),"Independent profile boundary");
            Check(group.Identifier==expected.GetProperty("identifier").GetString()&&group.Flags==expected.GetProperty("flags").GetUInt32()&&group.LegacyLayer==expected.GetProperty("legacyLayer").GetInt32(),"Profile header");
            Check(group.ObjectIndices.SequenceEqual(expected.GetProperty("objects").EnumerateArray().Select(x=>x.GetInt32()))&&group.ChannelIndices.SequenceEqual(expected.GetProperty("channels").EnumerateArray().Select(x=>x.GetInt32())),"Profile input links");
            Check(group.Curves.Count==expected.GetProperty("curves").GetArrayLength(),"Profile curve count");
            for(int i=0;i<group.Curves.Count;i++){
                var curve=group.Curves[i];var data=expected.GetProperty("curves")[i];
                Check(curve.Dimension==data.GetProperty("dimension").GetInt32()&&curve.Order==data.GetProperty("order").GetInt32()&&curve.ControlVertexCount==data.GetProperty("controlVertexCount").GetInt32(),"NURBS dimensions/order/count");
                Check(curve.IsRational==data.GetProperty("rational").GetBoolean()&&curve.FormatVersion==0x10,"NURBS rational/version");
                Check(curve.OriginalDataOffset==data.GetProperty("start").GetInt32()&&curve.OriginalControlOffset==data.GetProperty("controlOffset").GetInt32(),"NURBS source offsets");
                Check(curve.Reserved.SequenceEqual(data.GetProperty("reserved").EnumerateArray().Select(x=>x.GetInt32())),"Reserved integers changed");
                void Compare(IReadOnlyList<double> values,string key){
                    var bits=data.GetProperty(key).EnumerateArray().Select(x=>x.GetString()).ToArray();Check(values.Count==bits.Length,"NURBS array size");
                    for(int j=0;j<bits.Length;j++)Check(Bits(values[j])==bits[j],"NURBS raw bits changed: "+key);
                }
                Compare(curve.Knots,"knotBits");Compare(curve.ControlValues,"controlBits");Compare(curve.BoundingBoxValues,"boundingBoxBits");
            }
            Check(group.Names.SequenceEqual(expected.GetProperty("names").EnumerateArray().Select(x=>x.GetString())),"Legacy names changed");
            Check(group.SweepParameters.Count==expected.GetProperty("sweepBits").GetArrayLength(),"Sweep group count");
            for(int i=0;i<group.SweepParameters.Count;i++)Check(group.SweepParameters[i].Select(Bits).SequenceEqual(expected.GetProperty("sweepBits")[i].EnumerateArray().Select(x=>x.GetString())),"Sweep bits");
            Throws<NotSupportedException>(()=>document.WithScalarValue(index,0));
        }
        var selected=document.Records[7];var profile=selected.MatrixProfiles!;
        Check(profile.Curves.Count==5&&profile.Curves.Count(x=>x.IsRational)==2&&profile.Curves[0].DomainStart==0&&profile.Curves[0].DomainEnd==1,"Outside profile subset");
        Throws<NotSupportedException>(()=>((IList<double>)profile.Curves[0].Knots)[0]=42);
        Throws<NotSupportedException>(()=>((IList<double>)profile.Curves[0].ControlValues)[0]=42);
        Throws<NotSupportedException>(()=>((IList<string>)profile.Names)[0]="changed");
        Same(original,LegacyCsg4Codec.Write(document),"Typed profile structural no-op");
        var reopened=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(document.WithScalarValue(8,4.48)));
        foreach(var expected in groups){int index=expected.GetProperty("index").GetInt32();Same(document.Records[index].CopyOriginalRecordBytes(),reopened.Records[index].CopyOriginalRecordBytes(),"Scalar edit changed profiles");}
        Check(reopened.MatrixProfileRecordCount==6,"Profiles lost reopening");
        var unknown=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(unknown.AsSpan(selected.OriginalFileOffset+8),99);LeafCrc(unknown,selected);
        var unknownDoc=LegacyCsg4Codec.Read(unknown);Check(unknownDoc.Records[7].MatrixProfiles is null&&unknownDoc.Records[7].Header is null,"Unknown group guessed");Same(unknown,LegacyCsg4Codec.Write(unknownDoc),"Unknown group changed");
        var nested=(byte[])original.Clone();nested[profile.Curves[0].OriginalDataOffset]=0x11;LeafCrc(nested,selected);
        var nestedDoc=LegacyCsg4Codec.Read(nested);Check(nestedDoc.Records[7].MatrixProfiles is null,"Unknown nested curve guessed");Same(nested,LegacyCsg4Codec.Write(nestedDoc),"Unknown nested curve changed");
        var malformed=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(malformed.AsSpan(profile.Curves[0].OriginalKnotOffset-4),int.MaxValue);LeafCrc(malformed,selected);
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(malformed));
        var inconsistent=(byte[])original.Clone();BinaryPrimitives.WriteInt32LittleEndian(inconsistent.AsSpan(profile.Curves[0].OriginalControlOffset-4),25);LeafCrc(inconsistent,selected);
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(inconsistent));
        var raw=(byte[])original.Clone();int control=profile.Curves[0].OriginalControlOffset;
        BinaryPrimitives.WriteUInt64LittleEndian(raw.AsSpan(control),0x8000000000000000);BinaryPrimitives.WriteUInt64LittleEndian(raw.AsSpan(control+8),0x7ff8123456789abc);LeafCrc(raw,selected);
        var rawDoc=LegacyCsg4Codec.Read(raw);Check(Bits(rawDoc.Records[7].MatrixProfiles!.Curves[0].ControlValues[0])=="8000000000000000"&&Bits(rawDoc.Records[7].MatrixProfiles!.Curves[0].ControlValues[1])=="7ff8123456789abc","Raw geometry bits normalized");Same(raw,LegacyCsg4Codec.Write(rawDoc),"Raw geometry bits not preserved");
        int namesOffset=groups.Single(x=>x.GetProperty("index").GetInt32()==7).GetProperty("namesOffset").GetInt32();
        var oldPayload=original.AsSpan(selected.OriginalFileOffset+8,namesOffset-selected.OriginalFileOffset-8).ToArray();BinaryPrimitives.WriteInt32LittleEndian(oldPayload.AsSpan(4),0);
        var old=ReplacePayload(original,document,selected,oldPayload);var oldDoc=LegacyCsg4Codec.Read(old);
        Check(oldDoc.Records[7].MatrixProfiles!.Names.Count==0&&oldDoc.Records[7].MatrixProfiles!.Curves.Count==5,"Version1.0 names fabricated");Same(old,LegacyCsg4Codec.Write(oldDoc),"Version1.0 round trip changed");
        var truncated=ReplacePayload(original,document,selected,oldPayload.AsSpan(0,200).ToArray());
        Throws<InvalidDataException>(()=>LegacyCsg4Codec.Read(truncated));
        return "PASS embedded matrix profiles:6 groups/155 curves/4012 controls, bit-exact knots/homogeneous coordinates/bbox/sweeps/names, structural no-op/edit/reopen, legacy1.0/unknown/malformed/immutability and raw NaN/-0 guards; no native import/chooser/PC claim.";
    }
}
