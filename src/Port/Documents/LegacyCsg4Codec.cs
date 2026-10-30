using System.Text;

namespace JewelDesign.Documents;

/// <summary>Observed archive-4 CSG library envelope and numeric record schemas; not a portfolio or CAD evaluator.</summary>
public static class LegacyCsg4Codec
{
    public static readonly Guid ArrayTypeGuid=new("8d8b262f-f2bb-43c5-9486-fbd563c4759b");
    public static readonly Guid UnarrayV1TypeGuid=new("e147e20f-a225-4005-ab5e-2f37d149e81c");
    public static readonly Guid TransformObjectTypeGuid=new("27f4537a-6fc3-404f-9abd-eb53b7b3d8a1");
    public static readonly Guid RotateObjectTypeGuid=new("8c04b2e7-408f-40cd-920f-803c3a395275");
    public static readonly Guid ProfilePlacement2RailTypeGuid=new("fc789225-3f3b-41f5-9eb0-6c25d4fc54b8");
    public static readonly Guid AddTypeGuid=new("8793b48f-30c7-4841-9314-157a5c7bb629");
    public static readonly Guid TranslateObjectTypeGuid=new("f9af1449-1903-4d26-92df-6febc8a61427");
    public static readonly Guid ScaleObjectToSizeTypeGuid=new("e80ed2a1-d415-4ded-b1e8-b8557ac37e9c");
    public static readonly Guid SelectRingRailTypeGuid=new("e10a6966-719f-4ba9-9efe-b7ad0378b5f3");
    public static readonly Guid RingRailV1TypeGuid=new("65aa45c6-d1cd-4c45-92bb-b5a9a6a2ec47");
    public static readonly Guid OutsideRailV1TypeGuid=new("f9f6400f-67c2-4883-8c44-a91f411f2ee3");
    public static readonly Guid MatrixProfilesTypeGuid=new("98415b72-dadb-4b9b-94a9-1e6c5865b5f2");
    public static readonly Guid RangeTypeGuid = new("56b84606-7f1d-41fb-b81f-f521d9a9577d");
    public static readonly Guid RingTypeGuid = new("dd5c2342-d89a-440e-a76e-624a1faf2b68");
    public static readonly Guid ValueTypeGuid=new("38455005-5ef2-43db-8fb9-60e447d75d59");
    public static readonly Guid OnOffTypeGuid=new("a5d6b280-42fe-4188-8ff0-9ac6caed7831");
    public static readonly Guid ChooserTypeGuid=new("6c85fe71-927a-4d28-9335-8f1eb8713979");
    internal static bool IsPlainScalarType(Guid type)=>type==ValueTypeGuid||type==OnOffTypeGuid||type==ChooserTypeGuid;
    private static readonly Guid marker = new("b237ee6c-ac7e-4725-86ee-54bccda34fea");
    private const uint RecordCode = 0x40008000;
    private const string Header = "3D Geometry File Format        4";

    public static LegacyCsg4Document Read(ReadOnlySpan<byte> input, LegacyCsg4Limits? limits = null)
    {
        limits ??= new LegacyCsg4Limits(); limits.Validate();
        if (input.Length < 32 || input.Length > limits.MaxFileBytes) throw new InvalidDataException("Archive outside configured byte bound");
        if (Encoding.ASCII.GetString(input[..32]) != Header) throw new NotSupportedException("Expected archive-4 CSG library envelope");
        var bytes = input.ToArray(); var r = new CsgCursor(bytes, 32, bytes.Length, limits);
        foreach (uint type in new uint[] { 1, 0x10000014, 0x10000015 }) { var frame = r.Chunk(type); r.Position = frame.End; }
        if (r.Guid() != marker) throw new NotSupportedException("Unsupported CSG container marker");
        var info = r.Chunk(RecordCode); CsgCrc32.Check(bytes, info);
        var i = new CsgCursor(bytes, info.Payload, info.CrcOffset, limits);
        var infoVersion = Version(i);
        if (infoVersion.Major != 1 || infoVersion.Minor < 0 || infoVersion.Minor > 1) throw new NotSupportedException("Unsupported file-info version");
        i.Text(); i.Text(); for (int n = 0; n < 4; n++) i.Int();
        int provenance = i.Count(limits.MaxArrayItems); i.NeedItems(provenance, 16);
        for (int n = 0; n < provenance; n++) i.Guid();
        RequireEnd(i, info.CrcOffset); r.Position = info.End;
        r.UserData();
        var gems = r.Chunk(RecordCode);
        // Nested gem geometry/checksum scopes are opaque. No execution or regeneration occurs here.
        r.Position = gems.End;
        var categories = r.Chunk(RecordCode); CsgCrc32.Check(bytes, categories);
        var c = new CsgCursor(bytes, categories.Payload, categories.CrcOffset, limits);
        if (Version(c) != new LegacyCsg4Version(1, 0)) throw new NotSupportedException("Unsupported category table version");
        int categoryCount = c.Count(limits.MaxArrayItems);
        for (int n = 0; n < categoryCount; n++) c.Text();
        RequireEnd(c, categories.CrcOffset); r.Position = categories.End;

        var graph = r.Chunk(RecordCode);
        var g = new CsgCursor(bytes, graph.Payload, graph.CrcOffset, limits);
        if (Version(g) != new LegacyCsg4Version(1, 0)) throw new NotSupportedException("Unsupported graph directory version");
        int count = g.Count(limits.MaxRecords); g.NeedItems(count, 16);
        var types = new Guid[count]; for (int n = 0; n < count; n++) types[n] = g.Guid();
        CsgCrc32.Check(bytes, graph, g.Position); // native nested-chunk scope excludes leaf records
        var records = new LegacyCsg4Record[count];
        for (int n = 0; n < count; n++)
        {
            var frame = g.Chunk(RecordCode); CsgCrc32.Check(bytes, frame);
            var body = new CsgCursor(bytes, frame.Payload, frame.CrcOffset, limits);
            var version = Version(body);
            LegacyCsg4Scalar? scalar=null;
            if ((types[n] == RangeTypeGuid && version == new LegacyCsg4Version(1, 1))
                || (types[n] == RingTypeGuid && version == new LegacyCsg4Version(1, 2)))
                scalar=ReadScalar(body,version,true,types[n]==RangeTypeGuid,frame.CrcOffset);
            else if(IsPlainScalarType(types[n])&&version==new LegacyCsg4Version(1,1))
                scalar=ReadScalar(body,version,false,false,frame.CrcOffset);
            LegacyCsg4MatrixProfiles? profiles=null;
            if(types[n]==MatrixProfilesTypeGuid&&version.Major==1&&version.Minor is 0 or 1){
                try{profiles=LegacyMatrixProfilesCodec.Read(body,version,frame.CrcOffset,limits);}
                catch(NotSupportedException){/* Preserve the complete record opaque for an unsupported nested curve format. */}
            }
            LegacyCsg4SimpleBlock? block=null;
            if((types[n]==RingRailV1TypeGuid||types[n]==OutsideRailV1TypeGuid||types[n]==SelectRingRailTypeGuid||types[n]==TranslateObjectTypeGuid||types[n]==ScaleObjectToSizeTypeGuid||types[n]==AddTypeGuid||types[n]==ProfilePlacement2RailTypeGuid||types[n]==RotateObjectTypeGuid||types[n]==TransformObjectTypeGuid||types[n]==ArrayTypeGuid||types[n]==UnarrayV1TypeGuid)&&version==new LegacyCsg4Version(1,0))
                block=LegacyCsg4SimpleBlock.Read(body,version,frame.CrcOffset);
            records[n] = new LegacyCsg4Record(types[n], version, frame.Offset, bytes.AsSpan(frame.Offset, frame.End - frame.Offset).ToArray(), scalar,profiles,block);
            g.Position = frame.End;
        }
        RequireEnd(g, graph.CrcOffset); r.Position = graph.End;
        if (r.UInt() != 0x7fff || r.Int() != 4 || r.UInt() != (uint)bytes.Length || r.Position != bytes.Length)
            throw new InvalidDataException("Invalid archive EOF record");
        return new LegacyCsg4Document(bytes.AsSpan(0, graph.Offset).ToArray(), records);
    }

    private static LegacyCsg4Version Version(CsgCursor r) => new(r.Int(), r.Int());
    private static void RequireEnd(CsgCursor r, int expected) { if (r.Position != expected) throw new InvalidDataException("Unexpected trailing frame data"); }
    private static LegacyCsg4Scalar ReadScalar(CsgCursor r, LegacyCsg4Version version, bool bounded, bool stepPresent, int end)
    {
        var name = r.Text(); uint flags = r.UInt(); int layer = r.Int(); var userData = r.UserData();
        var objects = r.Ints(); var channels = r.Ints(); var categories = r.Ints();
        if (objects.Length != channels.Length) throw new InvalidDataException("Link array count mismatch");
        int ui = r.Int(), offset = r.Position;
        double value=r.Double();
        if(!bounded){RequireEnd(r,end);return new LegacyCsg4Scalar(version,name,flags,layer,userData,objects,channels,categories,ui,value,offset);}
        double min=r.Double(),max=r.Double();double? step=stepPresent?r.Double():null;
        RequireEnd(r,end);
        return new LegacyCsg4Numeric(version,name,flags,layer,userData,objects,channels,categories,ui,value,min,max,step,offset);
    }

    public static byte[] Write(LegacyCsg4Document document)
    {
        ArgumentNullException.ThrowIfNull(document);
        using var directory = new MemoryStream();
        using (var d = new BinaryWriter(directory, Encoding.UTF8, true))
        {
            d.Write(1); d.Write(0); d.Write(document.Records.Count);
            foreach (var record in document.Records) d.Write(record.TypeGuid.ToByteArray());
        }
        byte[] directoryBytes = directory.ToArray();
        using var graph = new MemoryStream(); graph.Write(directoryBytes);
        using (var g = new BinaryWriter(graph, Encoding.UTF8, true))
            foreach (var record in document.Records) WriteChunk(g, record.EncodePayload());
        byte[] graphBody = graph.ToArray();
        using var output = new MemoryStream(); using var w = new BinaryWriter(output);
        w.Write(document.Prefix); WriteChunk(w, graphBody, CsgCrc32.Compute(directoryBytes));
        long archiveLength = output.Length + 12;
        if (archiveLength > int.MaxValue) throw new InvalidDataException("Archive too large for observed 32-bit envelope");
        w.Write(0x7fffU); w.Write(4); w.Write((uint)archiveLength); w.Flush();
        return output.ToArray();
    }

    private static void WriteChunk(BinaryWriter w, byte[] body, uint? scopedCrc = null)
    {
        w.Write(RecordCode); w.Write(checked(body.Length + 4)); w.Write(body);
        w.Write(scopedCrc ?? CsgCrc32.Compute(body));
    }
}
