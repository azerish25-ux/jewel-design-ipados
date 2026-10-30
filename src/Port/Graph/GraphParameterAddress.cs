using System.Runtime.CompilerServices;

namespace JewelDesign.Graph;

/// <summary>
/// An exact field key at a node slot in one immutable capture. This is not a
/// serialized identifier and cannot be carried across loading or graph edits.
/// Creating an address does not assert that the field exists or is editable.
/// </summary>
public sealed class GraphParameterAddress : IEquatable<GraphParameterAddress>
{
    private readonly GraphSnapshot owner;
    public int NodeOrdinal { get; }
    public string FieldKey { get; }

    internal GraphParameterAddress(GraphSnapshot owner, int nodeOrdinal, string fieldKey)
    {
        this.owner = owner;
        NodeOrdinal = nodeOrdinal;
        FieldKey = fieldKey;
    }

    internal bool BelongsTo(GraphSnapshot snapshot) => ReferenceEquals(owner, snapshot);

    public bool Equals(GraphParameterAddress? other) => other is not null
        && ReferenceEquals(owner, other.owner)
        && NodeOrdinal == other.NodeOrdinal
        && StringComparer.Ordinal.Equals(FieldKey, other.FieldKey);

    public override bool Equals(object? obj) => obj is GraphParameterAddress other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(
        RuntimeHelpers.GetHashCode(owner), NodeOrdinal, StringComparer.Ordinal.GetHashCode(FieldKey));
}
