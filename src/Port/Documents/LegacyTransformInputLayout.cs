using JewelDesign.Graph;
namespace JewelDesign.Documents;

/// <summary>Source load-time sockets for exact TransformObject; stored links are never normalized in the document.</summary>
public sealed class LegacyTransformInputLayout
{
    public IReadOnlyList<SerializedChannelLink> AllChannels {get;}
    public IReadOnlyList<SerializedChannelLink> OperationChannels {get;}
    private LegacyTransformInputLayout(List<SerializedChannelLink> channels)
    {AllChannels=Array.AsReadOnly(channels.ToArray());OperationChannels=Array.AsReadOnly(channels.Take(channels.Count-1).ToArray());}
    public static LegacyTransformInputLayout Reconstruct(IReadOnlyList<SerializedChannelLink> stored,int recordCount,int importOffset=0,int fixedInputCount=1)
    {
        ArgumentNullException.ThrowIfNull(stored);if(recordCount<0)throw new ArgumentOutOfRangeException(nameof(recordCount));
        if(fixedInputCount is not (0 or 1))throw new ArgumentOutOfRangeException(nameof(fixedInputCount));
        var disconnected=new SerializedChannelLink(-1,-1);
        var channels=Enumerable.Repeat(disconnected,fixedInputCount+1).ToList();
        int Expansion(){var last=channels[^1];if(last.ObjectIndex>=0&&last.ChannelIndex>=0)channels.Add(disconnected);return channels.Count-1;}
        for(int i=0;i<stored.Count;i++){
            var link=stored[i];int node=link.ObjectIndex;
            if(node>=0)node=unchecked(node+importOffset);
            var runtime=node>=0&&node<recordCount?new SerializedChannelLink(node,link.ChannelIndex):disconnected;
            int slot=i<channels.Count?i:Expansion();channels[slot]=runtime;
        }
        Expansion(); // CSBlockExpandableInput.Read calls this after base.Read.
        return new(channels);
    }
}
