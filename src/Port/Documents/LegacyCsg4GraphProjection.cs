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
            var header = record.Header;
            var links = header is null ? Array.Empty<SerializedChannelLink>()
                : header.ObjectIndices.Select((index, channel) => new SerializedChannelLink(index, header.ChannelIndices[channel])).ToArray();
            return new GraphNodeSnapshot(ordinal, record.TypeGuid, new(record.Version.Major, record.Version.Minor),
                header?.Identifier, null, header?.Flags, record.CopyOriginalRecordBytes(), links,
                isInstanceIdentityDecoded: header is not null, areInputLinksDecoded: header is not null);
        });
        return new GraphSnapshot(nodes);
    }
}
