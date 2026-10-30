using JewelDesign.Documents;
using JewelDesign.Graph;

namespace DocumentContractChecks;

internal static class ProjectionAssertions
{
    public static void Run(LegacyCsg4Document document)
    {
        var snapshot = LegacyCsg4GraphProjection.Capture(document);
        if (snapshot.Nodes.Count != 241) throw new Exception("Projection shifted opaque slots");
        var scope = new GraphIdentityScope(snapshot);
        for (int i = 0; i < snapshot.Nodes.Count; i++)
        {
            var node = snapshot.Nodes[i]; var record = document.Records[i];
            if (node.TypeGuid != record.TypeGuid || node.Version.Major != record.Version.Major || node.Version.Minor != record.Version.Minor
                || !node.CopyOriginalRecordBytes().AsSpan().SequenceEqual(record.CopyOriginalRecordBytes())) throw new Exception("Projection lost provenance");
            if (record.Header is { } header)
            {
                if (node.BitFlags != header.Flags || node.Identifier != header.Identifier || !node.IsInstanceIdentityDecoded || !node.AreInputLinksDecoded
                    || scope.InitialView.NodeIdentities[i].InitialIdentity is not { WasGenerated: true }) throw new Exception("Known legacy header projection");
                if (node.InputLinks.Count != header.ObjectIndices.Count) throw new Exception("Projected channel count");
                for (int j = 0; j < node.InputLinks.Count; j++)
                    if (node.InputLinks[j] != new SerializedChannelLink(header.ObjectIndices[j], header.ChannelIndices[j])) throw new Exception("Link normalized");
            }
            else if (node.BitFlags is not null || node.Identifier is not null || node.IsInstanceIdentityDecoded || node.AreInputLinksDecoded
                || scope.InitialView.NodeIdentities[i].InitialIdentity is not null) throw new Exception("Opaque metadata or runtime GUID invented");
        }
    }
}
