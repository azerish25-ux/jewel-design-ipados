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

/// <summary>Decoded observed numeric schema. Original offsets are provenance, never parameter identities.</summary>
public sealed class LegacyCsg4Numeric
{
    private readonly StoredText name;
    private readonly (StoredText Type, StoredText Key, StoredText Value)[] userData;
    private readonly int[] objects, channels, categories;
    internal LegacyCsg4Version Version { get; }
    public string Identifier => name.Value;
    public uint Flags { get; }
    public int LegacyLayer { get; }
    public int LegacyUiInteger { get; }
    public double Value { get; }
    public double Minimum { get; }
    public double Maximum { get; }
    public double? Step { get; }
    public int OriginalValueOffset { get; }
    public IReadOnlyList<int> ObjectIndices { get; }
    public IReadOnlyList<int> ChannelIndices { get; }
    public IReadOnlyList<int> CategoryIndices { get; }
    public IReadOnlyList<LegacyUserDataEntry> UserData { get; }

    internal LegacyCsg4Numeric(LegacyCsg4Version version, StoredText name, uint flags, int layer,
        (StoredText Type, StoredText Key, StoredText Value)[] userData, int[] objects, int[] channels, int[] categories,
        int ui, double value, double min, double max, double? step, int originalValueOffset)
    {
        Version = version; this.name = name; Flags = flags; LegacyLayer = layer;
        this.userData = userData; this.objects = objects; this.channels = channels; this.categories = categories;
        LegacyUiInteger = ui; Value = value; Minimum = min; Maximum = max; Step = step; OriginalValueOffset = originalValueOffset;
        ObjectIndices = Array.AsReadOnly(objects); ChannelIndices = Array.AsReadOnly(channels); CategoryIndices = Array.AsReadOnly(categories);
        UserData = Array.AsReadOnly(userData.Select(x => new LegacyUserDataEntry(x.Type.Value, x.Key.Value, x.Value.Value)).ToArray());
    }

    internal LegacyCsg4Numeric WithValue(double value) => new(Version, name, Flags, LegacyLayer, userData, objects, channels, categories,
        LegacyUiInteger, value, Minimum, Maximum, Step, OriginalValueOffset);

    internal byte[] Encode()
    {
        using var stream = new MemoryStream(); using var w = new BinaryWriter(stream);
        w.Write(Version.Major); w.Write(Version.Minor); WriteText(w, name); w.Write(Flags); w.Write(LegacyLayer);
        w.Write(userData.Length); foreach (var item in userData) { WriteText(w, item.Type); WriteText(w, item.Key); WriteText(w, item.Value); }
        WriteInts(w, objects); WriteInts(w, channels); WriteInts(w, categories); w.Write(LegacyUiInteger);
        w.Write(BitConverter.DoubleToInt64Bits(Value)); w.Write(BitConverter.DoubleToInt64Bits(Minimum)); w.Write(BitConverter.DoubleToInt64Bits(Maximum));
        if (Step.HasValue) w.Write(BitConverter.DoubleToInt64Bits(Step.Value));
        w.Flush(); return stream.ToArray();
    }

    private static void WriteInts(BinaryWriter w, int[] values) { w.Write(values.Length); foreach (int n in values) w.Write(n); }
    private static void WriteText(BinaryWriter w, StoredText text)
    {
        w.Write(text.Terminated ? checked(text.Value.Length + 1) : 0);
        foreach (char c in text.Value) w.Write((ushort)c);
        if (text.Terminated) w.Write((ushort)0);
    }
}

public sealed class LegacyCsg4Record
{
    private readonly byte[] originalRecord;
    public Guid TypeGuid { get; }
    public LegacyCsg4Version Version { get; }
    public int OriginalFileOffset { get; }
    public LegacyCsg4Numeric? Numeric { get; }
    public string? OpaqueReason => Numeric is not null ? null :
        TypeGuid == LegacyCsg4Codec.RangeTypeGuid || TypeGuid == LegacyCsg4Codec.RingTypeGuid ? "Numeric record version is not decoded" : "Record type is not decoded";

    internal LegacyCsg4Record(Guid type, LegacyCsg4Version version, int offset, byte[] originalRecord, LegacyCsg4Numeric? numeric)
    {
        TypeGuid = type; Version = version; OriginalFileOffset = offset; this.originalRecord = originalRecord; Numeric = numeric;
    }
    public byte[] CopyOriginalRecordBytes() => (byte[])originalRecord.Clone();
    internal byte[] EncodePayload() => Numeric?.Encode() ?? originalRecord.AsSpan(8, originalRecord.Length - 12).ToArray();
    internal LegacyCsg4Record WithValue(double value) => new(TypeGuid, Version, OriginalFileOffset, originalRecord,
        (Numeric ?? throw new NotSupportedException(OpaqueReason)).WithValue(value));
}

/// <summary>Immutable library archive model; it does not contain regenerated CAD geometry.</summary>
public sealed class LegacyCsg4Document
{
    private readonly byte[] prefix;
    internal ReadOnlySpan<byte> Prefix => prefix;
    public IReadOnlyList<LegacyCsg4Record> Records { get; }
    public int NumericRecordCount => Records.Count(r => r.Numeric is not null);
    public string IntegrityCoverage => "File-info/category/graph-directory/leaf CRCs checked; preview, gem and envelope nested checksums remain opaque";
    internal LegacyCsg4Document(byte[] prefix, LegacyCsg4Record[] records) { this.prefix = prefix; Records = Array.AsReadOnly(records); }

    /// <summary>Encode a stored number exactly; UI validation and recovered setter policy belong to graph commands.</summary>
    public LegacyCsg4Document WithNumericValue(int ordinal, double value)
    {
        if ((uint)ordinal >= (uint)Records.Count) throw new ArgumentOutOfRangeException(nameof(ordinal));
        var records = Records.ToArray(); records[ordinal] = records[ordinal].WithValue(value);
        return new LegacyCsg4Document(prefix, records);
    }
}
