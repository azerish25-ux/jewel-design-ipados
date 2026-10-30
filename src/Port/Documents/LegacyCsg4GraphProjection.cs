using JewelDesign.Graph;

namespace JewelDesign.Documents;

public static class LegacyCsg4GraphProjection
{
    /// <summary>Capture all record slots; unknown headers/links remain explicitly undecoded.</summary>
    public static GraphSnapshot Capture(LegacyCsg4Document document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var nodes = document.Records.Select((record, ordinal) =>
        {
            var scalar = record.Scalar;
            var links = scalar is null ? Array.Empty<SerializedChannelLink>()
                : scalar.ObjectIndices.Select((index, channel) => new SerializedChannelLink(index, scalar.ChannelIndices[channel])).ToArray();
            return new GraphNodeSnapshot(ordinal, record.TypeGuid, new(record.Version.Major, record.Version.Minor),
                scalar?.Identifier, null, scalar?.Flags, record.CopyOriginalRecordBytes(), links,
                isInstanceIdentityDecoded: scalar is not null, areInputLinksDecoded: scalar is not null);
        });
        return new GraphSnapshot(nodes);
    }
}
