namespace JewelDesign.Graph;

/// <summary>Raw record version; interpretation and supported-version checks belong to the codec.</summary>
public readonly record struct RecordVersion(int Major, int Minor);

/// <summary>Serialized table/channel indices. Preserve negative and unresolved values without normalization.</summary>
public readonly record struct SerializedChannelLink(int ObjectIndex, int ChannelIndex)
{
    // CSBlock.Write and WriteJson emit this pair for an unconnected input.
    public static SerializedChannelLink Disconnected => new(-1, -5);
}

/// <summary>
/// Immutable captured node metadata. Original record bytes are provenance, not regenerated save output.
/// A codec must reconcile edited fields, unknown regions and CRCs before writing.
/// </summary>
public sealed class GraphNodeSnapshot
{
    private readonly byte[] originalRecord;
    public int StoredTableIndex { get; }
    public Guid TypeGuid { get; }
    public RecordVersion Version { get; }
    public string? Identifier { get; }
    public string? StoredInstanceGuid { get; }
    public uint BitFlags { get; }
    public IReadOnlyList<SerializedChannelLink> InputLinks { get; }

    public GraphNodeSnapshot(int storedTableIndex, Guid typeGuid, RecordVersion version,
        string? identifier, string? storedInstanceGuid, uint bitFlags,
        ReadOnlySpan<byte> originalRecordBytes, IEnumerable<SerializedChannelLink> inputLinks)
    {
        ArgumentNullException.ThrowIfNull(inputLinks);
        StoredTableIndex = storedTableIndex;
        TypeGuid = typeGuid;
        Version = version;
        Identifier = identifier;
        StoredInstanceGuid = storedInstanceGuid;
        BitFlags = bitFlags;
        originalRecord = originalRecordBytes.ToArray();
        InputLinks = Array.AsReadOnly(inputLinks.ToArray());
    }

    public byte[] CopyOriginalRecordBytes() => (byte[])originalRecord.Clone();
}

/// <summary>
/// Captured graph order. Stored table indices are independent: selected/active serialization may remap them.
/// This container does not register types, normalize identities, resolve links, evaluate CAD or serialize bytes.
/// </summary>
public sealed class GraphSnapshot
{
    public IReadOnlyList<GraphNodeSnapshot> Nodes { get; }

    /// <summary>Capture-local address using an exact serialized field key, never a display label.</summary>
    public GraphParameterAddress AddressParameter(int nodeOrdinal, string fieldKey)
    {
        ArgumentNullException.ThrowIfNull(fieldKey);
        if (string.IsNullOrWhiteSpace(fieldKey))
            throw new ArgumentException("A nonblank field key is required.", nameof(fieldKey));
        if ((uint)nodeOrdinal >= (uint)Nodes.Count)
            throw new ArgumentOutOfRangeException(nameof(nodeOrdinal));
        return new GraphParameterAddress(this, nodeOrdinal, fieldKey);
    }

    /// <summary>Reject stale/foreign addresses instead of guessing from labels, GUIDs or table indices.</summary>
    public GraphNodeSnapshot ResolveNode(GraphParameterAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);
        if (!address.BelongsTo(this))
            throw new ArgumentException("The address belongs to a different snapshot.", nameof(address));
        return Nodes[address.NodeOrdinal];
    }

    public GraphSnapshot(IEnumerable<GraphNodeSnapshot> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        var captured = nodes.ToArray();
        if (captured.Any(node => node is null))
            throw new ArgumentException("Represent unresolved records explicitly; never discard a null slot and shift indices.", nameof(nodes));
        Nodes = Array.AsReadOnly(captured);
    }
}
