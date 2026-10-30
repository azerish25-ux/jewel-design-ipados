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
            var numeric = record.Numeric;
            var links = numeric is null ? Array.Empty<SerializedChannelLink>()
                : numeric.ObjectIndices.Select((index, channel) => new SerializedChannelLink(index, numeric.ChannelIndices[channel])).ToArray();
            return new GraphNodeSnapshot(ordinal, record.TypeGuid, new(record.Version.Major, record.Version.Minor),
                numeric?.Identifier, null, numeric?.Flags, record.CopyOriginalRecordBytes(), links,
                isInstanceIdentityDecoded: numeric is not null, areInputLinksDecoded: numeric is not null);
        });
        return new GraphSnapshot(nodes);
    }
}
