using JewelDesign.Domain;
using JewelDesign.Graph;

namespace GraphContractChecks;

public static class SessionIdentityAssertions
{
    public static string Run()
    {
        var guid = Guid.Parse("8c969c9a-a810-4fe2-8fc6-1d802d26bd55");
        foreach (var format in new[] { "N", "D", "B", "P", "X" })
        {
            var text = "  " + guid.ToString(format).ToUpperInvariant() + "  ";
            var identity = InstanceIdentity.ResolveStored(text);
            Check(identity.StoredText == text && identity.EffectiveGuid == guid && !identity.WasGenerated, "Valid GUID formatting/raw preservation");
        }
        foreach (var text in new string?[] { null, "", " ", "malformed-preserve-me", "{not-a-guid}" })
        {
            var identity = InstanceIdentity.ResolveStored(text);
            Check(identity.StoredText == text && identity.WasGenerated && identity.EffectiveGuid != Guid.Empty, "Legacy fallback/raw preservation");
        }
        var zero = InstanceIdentity.ResolveStored(Guid.Empty.ToString());
        Check(!zero.WasGenerated && zero.EffectiveGuid == Guid.Empty, "A valid zero GUID must not be repaired");

        GraphNodeSnapshot Node(string name, string? stored) => new(9, Guid.Empty, new(99, 7), name, stored, uint.MaxValue, new byte[] { 0, 255, 42 }, Array.Empty<SerializedChannelLink>());
        var originalA = Node("Duplicate", guid.ToString());
        var originalB = Node("Duplicate", guid.ToString());
        var original = new GraphSnapshot(new[] { originalA, originalB });
        var scope = new GraphIdentityScope(original);
        var initial = scope.InitialView;
        var a = initial.NodeIdentities[0]; var b = initial.NodeIdentities[1];
        Check(!ReferenceEquals(a, b) && a.InitialIdentity!.EffectiveGuid == b.InitialIdentity!.EffectiveGuid, "Duplicate GUID records collapsed/repaired");
        var address = initial.AddressParameter(0, "Value");
        Check(address.Equals(a.AddressParameter("Value")), "Stable node address creation");
        Check(address.GetHashCode() == a.AddressParameter("Value").GetHashCode(), "Stable address hash");
        Check(!address.Equals(initial.AddressParameter(1, "Value")), "Duplicate label/GUID parameter collision");
        Check(!address.Equals(a.AddressParameter("value")) && !address.Equals(a.AddressParameter(" Value ")), "Ordinal field keys lost");


        var renamedA = Node("Renamed", guid.ToString());
        var order = new List<GraphNodeIdentity> { b, a };
        var changed = scope.CreateView(new GraphSnapshot(new[] { originalB, renamedA }), order);
        order.Clear();
        Check(address.Equals(changed.AddressParameter(1, "Value")), "Address lost on explicit rename/reorder mapping");
        Check(ReferenceEquals(changed.ResolveNode(address), renamedA), "Mapped address resolved wrong node");
        Check(ReferenceEquals(initial.ResolveNode(address), originalA), "Old immutable view changed");
        Check(changed.ResolveNode(address).CopyOriginalRecordBytes().SequenceEqual(new byte[] { 0, 255, 42 }), "Opaque provenance lost");
        Check(changed.ResolveNode(address).StoredTableIndex == 9 && changed.ResolveNode(address).Version.Major == 99, "Stored index/version normalized");

        var guidChanged = Node("Renamed", Guid.NewGuid().ToString());
        var guidView = scope.CreateView(new GraphSnapshot(new[] { guidChanged, originalB }), new[] { a, b });
        Check(ReferenceEquals(guidView.ResolveNode(address), guidChanged) && a.InitialIdentity!.EffectiveGuid == guid, "Explicit metadata mapping changed registration identity provenance");

        var removed = scope.CreateView(new GraphSnapshot(new[] { originalB }), new[] { b });
        Throws<KeyNotFoundException>(() => removed.ResolveNode(address));
        var restored = scope.CreateView(original, new[] { a, b });
        Check(ReferenceEquals(restored.ResolveNode(address), originalA), "Removed identity cannot be restored explicitly");
        var replacement = scope.RegisterCapturedNode(originalA);
        var replaced = scope.CreateView(original, new[] { replacement, b });
        Throws<KeyNotFoundException>(() => replaced.ResolveNode(address)); // never retarget to a new object with the same GUID
        Check(!address.Equals(replacement.AddressParameter("Value")), "Replacement reused removed node identity");

        var otherScope = new GraphIdentityScope(original);
        Check(!address.Equals(otherScope.InitialView.AddressParameter(0, "Value")), "Cross-document handle collision");
        Throws<ArgumentException>(() => otherScope.InitialView.ResolveNode(address));
        Throws<ArgumentException>(() => scope.CreateView(original, new[] { a, otherScope.InitialView.NodeIdentities[1] }));
        Throws<ArgumentException>(() => scope.CreateView(original, new[] { a, a }));
        Throws<ArgumentException>(() => scope.CreateView(original, new[] { a }));
        Throws<ArgumentException>(() => scope.CreateView(original, new GraphNodeIdentity[] { null!, b }));
        Throws<ArgumentNullException>(() => new GraphIdentityScope(null!));
        Throws<ArgumentNullException>(() => scope.RegisterCapturedNode(null!));
        Throws<ArgumentNullException>(() => scope.CreateView(null!, new[] { a, b }));
        Throws<ArgumentNullException>(() => scope.CreateView(original, null!));
        Throws<ArgumentNullException>(() => initial.ResolveNode(null!));
        Throws<ArgumentOutOfRangeException>(() => initial.AddressParameter(-1, "Value"));
        Throws<ArgumentOutOfRangeException>(() => initial.AddressParameter(2, "Value"));
        Throws<ArgumentNullException>(() => a.AddressParameter(null!));
        Throws<ArgumentException>(() => a.AddressParameter(""));
        Throws<ArgumentException>(() => a.AddressParameter(" 	"));
        Throws<NotSupportedException>(() => ((IList<GraphNodeIdentity>)changed.NodeIdentities)[0] = a);

        var empty = new GraphIdentityScope(new GraphSnapshot(Array.Empty<GraphNodeSnapshot>()));
        Check(empty.InitialView.NodeIdentities.Count == 0, "Empty graph identity scope");
        Throws<ArgumentOutOfRangeException>(() => empty.InitialView.AddressParameter(0, "Value"));
        var sameSlotObject = new GraphIdentityScope(new GraphSnapshot(new[] { originalA, originalA }));
        Check(!sameSlotObject.InitialView.AddressParameter(0, "Value").Equals(sameSlotObject.InitialView.AddressParameter(1, "Value")), "Repeated captured object collapsed two slots");
        var legacyNode = Node("Legacy", null);
        var legacyScope = new GraphIdentityScope(new GraphSnapshot(new[] { legacyNode }));
        var legacyIdentity = legacyScope.InitialView.NodeIdentities[0];
        var legacyView = legacyScope.CreateView(new GraphSnapshot(new[] { legacyNode }), new[] { legacyIdentity });
        Check(ReferenceEquals(legacyIdentity.InitialIdentity, legacyView.NodeIdentities[0].InitialIdentity) && legacyIdentity.InitialIdentity!.WasGenerated, "Runtime GUID regenerated between views");

        Check(!address.Equals((object?)null) && !address.Equals("Value"), "Handle null/type equality");

        return "PASS session identity: raw/effective GUID rules, duplicate/zero/legacy identity, explicit rename/reorder/delete/restore mappings and ownership guards; no codec migration or applied graph-edit claim.";
    }

    private static void Check(bool value, string message) { if (!value) throw new Exception(message); }
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; }
        throw new Exception("Expected " + typeof(T).Name);
    }
}
