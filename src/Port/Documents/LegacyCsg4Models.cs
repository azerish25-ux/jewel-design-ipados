namespace JewelDesign.Documents;

public sealed record LegacyCsg4Limits(int MaxFileBytes = 64 * 1024 * 1024, int MaxRecords = 100000, int MaxArrayItems = 10000, int MaxStringCodeUnits = 1000000)
{
    internal void Validate()
    {
        if (MaxFileBytes < 32 || MaxRecords < 0 || MaxArrayItems < 0 || MaxStringCodeUnits < 0)
            throw new ArgumentOutOfRangeException(nameof(LegacyCsg4Limits));
    }
}

public readonly record struct LegacyCsg4Version(int Major, int Minor);
public sealed record LegacyUserDataEntry(string Type, string Key, string Value);
internal readonly record struct StoredText(string Value, bool Terminated);

/// <summary>Observed bounded numeric schema. Plain scalars have no invented bounds.</summary>
public sealed class LegacyCsg4Numeric:LegacyCsg4Scalar
{
    public double Minimum {get;}
    public double Maximum {get;}
    public double? Step {get;}
    internal LegacyCsg4Numeric(LegacyCsg4Version version,StoredText name,uint flags,int layer,
        (StoredText Type,StoredText Key,StoredText Value)[] userData,int[] objects,int[] channels,int[] categories,
        int ui,double value,double min,double max,double? step,int originalValueOffset)
        :base(version,name,flags,layer,userData,objects,channels,categories,ui,value,originalValueOffset)
    {Minimum=min;Maximum=max;Step=step;}
    private LegacyCsg4Numeric(LegacyCsg4Numeric source,double value,int? input=null,int objectIndex=0,int channelIndex=0):base(source,value,input,objectIndex,channelIndex)
    {Minimum=source.Minimum;Maximum=source.Maximum;Step=source.Step;}
    internal override LegacyCsg4Numeric WithValue(double value)=>new(this,value);
    internal override LegacyCsg4Numeric WithInputLink(int input,int objectIndex,int channelIndex)=>new(this,Value,input,objectIndex,channelIndex);
    protected override void WriteTail(BinaryWriter writer)
    {
        writer.Write(BitConverter.DoubleToInt64Bits(Minimum));writer.Write(BitConverter.DoubleToInt64Bits(Maximum));
        if(Step.HasValue)writer.Write(BitConverter.DoubleToInt64Bits(Step.Value));
    }
}

public sealed class LegacyCsg4Record
{
    private readonly byte[] originalRecord;
    public Guid TypeGuid { get; }
    public LegacyCsg4Version Version { get; }
    public int OriginalFileOffset { get; }
    public LegacyCsg4Scalar? Scalar {get;}
    public LegacyCsg4Numeric? Numeric=>Scalar as LegacyCsg4Numeric;
    public LegacyCsg4MatrixProfiles? MatrixProfiles {get;}
    public ILegacyCsg4Header? Header=>(ILegacyCsg4Header?)Scalar??MatrixProfiles;
    public string? OpaqueReason => Header is not null ? null :
        TypeGuid==LegacyCsg4Codec.MatrixProfilesTypeGuid?"Matrix profile version or nested NURBS format is not decoded":
        TypeGuid == LegacyCsg4Codec.RangeTypeGuid || TypeGuid == LegacyCsg4Codec.RingTypeGuid ? "Numeric record version is not decoded" : LegacyCsg4Codec.IsPlainScalarType(TypeGuid) ? "Scalar record version is not decoded" : "Record type is not decoded";

    internal LegacyCsg4Record(Guid type, LegacyCsg4Version version, int offset, byte[] originalRecord, LegacyCsg4Scalar? scalar,LegacyCsg4MatrixProfiles? matrixProfiles=null)
    {
        TypeGuid = type; Version = version; OriginalFileOffset = offset; this.originalRecord = originalRecord; Scalar = scalar;MatrixProfiles=matrixProfiles;
    }
    internal LegacyCsg4Record WithInputLink(int input,int objectIndex,int channelIndex)=>new(TypeGuid,Version,OriginalFileOffset,originalRecord,(Scalar??throw new NotSupportedException("Scalar header required")).WithInputLink(input,objectIndex,channelIndex),MatrixProfiles);
    public byte[] CopyOriginalRecordBytes() => (byte[])originalRecord.Clone();
    internal byte[] EncodePayload() => Scalar?.Encode() ?? MatrixProfiles?.Encode() ?? originalRecord.AsSpan(8, originalRecord.Length - 12).ToArray();
    internal LegacyCsg4Record WithValue(double value) => new(TypeGuid, Version, OriginalFileOffset, originalRecord,
        (Scalar ?? throw new NotSupportedException(OpaqueReason??"Record has no stored scalar value")).WithValue(value),MatrixProfiles);
}

/// <summary>Immutable library archive model; it does not contain regenerated CAD geometry.</summary>
public sealed class LegacyCsg4Document
{
    private readonly byte[] prefix;
    internal ReadOnlySpan<byte> Prefix => prefix;
    public IReadOnlyList<LegacyCsg4Record> Records { get; }
    public int NumericRecordCount => Records.Count(r => r.Numeric is not null);
    public int ScalarRecordCount => Records.Count(r=>r.Scalar is not null);
    public int MatrixProfileRecordCount=>Records.Count(r=>r.MatrixProfiles is not null);
    public string IntegrityCoverage => "File-info/category/graph-directory/leaf CRCs checked; preview, gem and envelope nested checksums remain opaque";
    internal LegacyCsg4Document(byte[] prefix, LegacyCsg4Record[] records) { this.prefix = prefix; Records = Array.AsReadOnly(records); }

    /// <summary>Encode a stored number exactly; UI validation and recovered setter policy belong to graph commands.</summary>
    public LegacyCsg4Document WithNumericValue(int ordinal, double value)
    {
        if ((uint)ordinal >= (uint)Records.Count) throw new ArgumentOutOfRangeException(nameof(ordinal));
        if(Records[ordinal].Numeric is null)throw new NotSupportedException(Records[ordinal].OpaqueReason??(Records[ordinal].Scalar is not null?"Use WithScalarValue for decoded plain scalar records.":"Record has no stored numeric value."));
        return WithScalarValue(ordinal,value);
    }

    /// <summary>Store an exact decoded scalar; this does not apply OnOff/chooser UI policy or regenerate CAD.</summary>
    public LegacyCsg4Document WithScalarInputLink(int ordinal,int input,int objectIndex,int channelIndex)
    {
        if((uint)ordinal>=(uint)Records.Count)throw new ArgumentOutOfRangeException(nameof(ordinal));
        if(objectIndex>=Records.Count)throw new ArgumentOutOfRangeException(nameof(objectIndex));
        var records=Records.ToArray();records[ordinal]=records[ordinal].WithInputLink(input,objectIndex,channelIndex);return new LegacyCsg4Document(prefix,records);
    }

    public LegacyCsg4Document WithScalarValue(int ordinal,double value)
    {
        if((uint)ordinal>=(uint)Records.Count)throw new ArgumentOutOfRangeException(nameof(ordinal));
        var records=Records.ToArray();records[ordinal]=records[ordinal].WithValue(value);
        return new LegacyCsg4Document(prefix,records);
    }
}
