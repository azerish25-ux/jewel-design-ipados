using System.Runtime.CompilerServices;
using JewelDesign.Domain;

namespace JewelDesign.Graph;

/// <summary>One editing-session identity space. It does not infer correspondence across file loads.</summary>
public sealed class GraphIdentityScope
{
    public GraphIdentityView InitialView { get; }

    public GraphIdentityScope(GraphSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        var identities = snapshot.Nodes.Select(RegisterCapturedNode).ToArray();
        InitialView = new GraphIdentityView(this, snapshot, identities);
    }

    /// <summary>Create a distinct record identity, even for duplicate GUIDs or repeated snapshot objects.</summary>
    public GraphNodeIdentity RegisterCapturedNode(GraphNodeSnapshot node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return new GraphNodeIdentity(this, InstanceIdentity.ResolveStored(node.StoredInstanceGuid));
    }

    /// <summary>Bind host-supplied node correspondence; do not match labels, GUIDs or stored indices.</summary>
    public GraphIdentityView CreateView(GraphSnapshot snapshot, IEnumerable<GraphNodeIdentity> identityOrder)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(identityOrder);
        return new GraphIdentityView(this, snapshot, identityOrder);
    }
}

/// <summary>Reference identity of a captured record in one scope; never serialize this as a project ID.</summary>
public sealed class GraphNodeIdentity
{
    private readonly GraphIdentityScope owner;
    // Registration-time identity provenance; this token is not a mutable CSObject.
    public InstanceIdentity InitialIdentity { get; }

    internal GraphNodeIdentity(GraphIdentityScope owner, InstanceIdentity identity)
    {
        this.owner = owner;
        InitialIdentity = identity;
    }

    internal bool BelongsTo(GraphIdentityScope scope) => ReferenceEquals(owner, scope);

    public GraphParameterHandle AddressParameter(string fieldKey)
    {
        ArgumentNullException.ThrowIfNull(fieldKey);
        if (string.IsNullOrWhiteSpace(fieldKey))
            throw new ArgumentException("A nonblank field key is required.", nameof(fieldKey));
        return new GraphParameterHandle(this, fieldKey);
    }
}

/// <summary>Immutable mapping of live record identities to one captured graph's node order.</summary>
public sealed class GraphIdentityView
{
    private readonly GraphIdentityScope owner;
    private readonly Dictionary<GraphNodeIdentity, int> nodeOrdinals = new(ReferenceEqualityComparer.Instance);
    public GraphSnapshot Snapshot { get; }
    public IReadOnlyList<GraphNodeIdentity> NodeIdentities { get; }

    internal GraphIdentityView(GraphIdentityScope owner, GraphSnapshot snapshot, IEnumerable<GraphNodeIdentity> identityOrder)
    {
        this.owner = owner;
        Snapshot = snapshot;
        var identities = identityOrder.ToArray();
        if (identities.Length != snapshot.Nodes.Count)
            throw new ArgumentException("Every captured node requires one explicit identity.", nameof(identityOrder));
        for (int i = 0; i < identities.Length; i++)
        {
            var identity = identities[i];
            if (identity is null || !identity.BelongsTo(owner) || !nodeOrdinals.TryAdd(identity, i))
                throw new ArgumentException("Identity mapping contains a null, foreign or repeated record identity.", nameof(identityOrder));
        }
        NodeIdentities = Array.AsReadOnly(identities);
    }

    public GraphParameterHandle AddressParameter(int nodeOrdinal, string fieldKey)
    {
        if ((uint)nodeOrdinal >= (uint)NodeIdentities.Count)
            throw new ArgumentOutOfRangeException(nameof(nodeOrdinal));
        return NodeIdentities[nodeOrdinal].AddressParameter(fieldKey);
    }

    public GraphNodeSnapshot ResolveNode(GraphParameterHandle handle)
    {
        ArgumentNullException.ThrowIfNull(handle);
        if (!handle.NodeIdentity.BelongsTo(owner))
            throw new ArgumentException("The handle belongs to another identity scope.", nameof(handle));
        if (!nodeOrdinals.TryGetValue(handle.NodeIdentity, out var ordinal))
            throw new KeyNotFoundException("The record is not present in this captured view.");
        return Snapshot.Nodes[ordinal];
    }
}

/// <summary>Stable session parameter address. Field existence and editability require a codec/schema.</summary>
public sealed class GraphParameterHandle : IEquatable<GraphParameterHandle>
{
    public GraphNodeIdentity NodeIdentity { get; }
    public string FieldKey { get; }

    internal GraphParameterHandle(GraphNodeIdentity nodeIdentity, string fieldKey)
    {
        NodeIdentity = nodeIdentity;
        FieldKey = fieldKey;
    }

    public bool Equals(GraphParameterHandle? other) => other is not null
        && ReferenceEquals(NodeIdentity, other.NodeIdentity)
        && StringComparer.Ordinal.Equals(FieldKey, other.FieldKey);

    public override bool Equals(object? obj) => obj is GraphParameterHandle other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(
        RuntimeHelpers.GetHashCode(NodeIdentity), StringComparer.Ordinal.GetHashCode(FieldKey));
}
