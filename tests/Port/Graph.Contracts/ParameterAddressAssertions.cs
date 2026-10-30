using JewelDesign.Graph;

namespace GraphContractChecks;

internal static class ParameterAddressAssertions
{
    public static void Run()
    {
        var node = new GraphNodeSnapshot(9, Guid.Empty, new(1, 0), "Duplicate", null,
            0, new byte[] { 1 }, Array.Empty<SerializedChannelLink>());
        // Identical metadata, even the very same captured node, may occupy separate slots.
        var graph = new GraphSnapshot(new[] { node, node });
        var first = graph.AddressParameter(0, "Value");
        var again = graph.AddressParameter(0, "Value");
        var second = graph.AddressParameter(1, "Value");
        Check(first.Equals(again), "Same field in same snapshot must compare equal");
        Check(first.GetHashCode() == again.GetHashCode(), "Equal address hash mismatch");
        Check(!first.Equals(second), "Duplicate labels/GUIDs/indices/objects must not collapse slots");
        Check(!first.Equals(graph.AddressParameter(0, "value")), "Field keys must be ordinal");
        Check(!first.Equals(graph.AddressParameter(0, " Value ")), "Field key must not be trimmed");
        Check(first.NodeOrdinal == 0 && first.FieldKey == "Value", "Address fields changed");
        Check(ReferenceEquals(graph.ResolveNode(first), node), "Wrong node resolution");
        Check(new HashSet<GraphParameterAddress> { first, again, second }.Count == 2, "Set semantics");

        var other = new GraphSnapshot(new[] { node, node });
        Check(!first.Equals(other.AddressParameter(0, "Value")), "Cross-snapshot addresses collided");
        Throws<ArgumentException>(() => other.ResolveNode(first));
        Throws<ArgumentNullException>(() => graph.ResolveNode(null!));
        Throws<ArgumentOutOfRangeException>(() => graph.AddressParameter(-1, "Value"));
        Throws<ArgumentOutOfRangeException>(() => graph.AddressParameter(2, "Value"));
        Throws<ArgumentOutOfRangeException>(() => new GraphSnapshot(Array.Empty<GraphNodeSnapshot>()).AddressParameter(0, "Value"));
        Throws<ArgumentNullException>(() => graph.AddressParameter(0, null!));
        Throws<ArgumentException>(() => graph.AddressParameter(0, ""));
        Check(!first.Equals((object?)null) && !first.Equals("Value"), "Address type/null equality");
        Throws<ArgumentException>(() => graph.AddressParameter(0, " \t"));

    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new Exception("Expected " + typeof(T).Name);
    }
}
