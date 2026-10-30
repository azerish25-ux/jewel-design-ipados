namespace JewelDesign.Documents;

/// <summary>Exact known payload-free legacy CSBlock header; does not execute its node.</summary>
public sealed class LegacyCsg4SimpleBlock:ILegacyCsg4Header
{
    private readonly LegacyCsg4Version version;
    private readonly StoredText identifier;
    private readonly (StoredText Type,StoredText Key,StoredText Value)[] data;
    public string Identifier=>identifier.Value;
    public uint Flags {get;}
    public int LegacyLayer {get;}
    public IReadOnlyList<int> ObjectIndices {get;}
    public IReadOnlyList<int> ChannelIndices {get;}
    public IReadOnlyList<LegacyUserDataEntry> UserData {get;}
    private LegacyCsg4SimpleBlock(LegacyCsg4Version version,StoredText name,uint flags,int layer,
        (StoredText Type,StoredText Key,StoredText Value)[] data,int[] objects,int[] channels)
    {
        this.version=version;identifier=name;Flags=flags;LegacyLayer=layer;this.data=data;
        ObjectIndices=Array.AsReadOnly(objects);ChannelIndices=Array.AsReadOnly(channels);
        UserData=Array.AsReadOnly(data.Select(x=>new LegacyUserDataEntry(x.Type.Value,x.Key.Value,x.Value.Value)).ToArray());
    }
    internal static LegacyCsg4SimpleBlock Read(CsgCursor r,LegacyCsg4Version version,int end)
    {
        var name=r.Text();uint flags=r.UInt();int layer=r.Int();var data=r.UserData();var objects=r.Ints();var channels=r.Ints();
        if(objects.Length!=channels.Length)throw new InvalidDataException("Rail block link count mismatch");
        if(r.Position!=end)throw new InvalidDataException("Unexpected trailing known rail block data");
        return new(version,name,flags,layer,data,objects,channels);
    }
    internal LegacyCsg4SimpleBlock WithInputLink(int input,int objectIndex,int channelIndex)
    {
        if((uint)input>=(uint)ObjectIndices.Count)throw new ArgumentOutOfRangeException(nameof(input));
        var objects=ObjectIndices.ToArray();var channels=ChannelIndices.ToArray();objects[input]=objectIndex;channels[input]=channelIndex;
        return new(version,identifier,Flags,LegacyLayer,data,objects,channels);
    }
    internal byte[] Encode()
    {
        using var stream=new MemoryStream();using var writer=new BinaryWriter(stream);
        writer.Write(version.Major);writer.Write(version.Minor);Text(writer,identifier);writer.Write(Flags);writer.Write(LegacyLayer);
        writer.Write(data.Length);foreach(var item in data){Text(writer,item.Type);Text(writer,item.Key);Text(writer,item.Value);}
        writer.Write(ObjectIndices.Count);foreach(int value in ObjectIndices)writer.Write(value);
        writer.Write(ChannelIndices.Count);foreach(int value in ChannelIndices)writer.Write(value);
        writer.Flush();return stream.ToArray();
    }
    private static void Text(BinaryWriter writer,StoredText value)
    {writer.Write(value.Terminated?checked(value.Value.Length+1):0);foreach(char c in value.Value)writer.Write((ushort)c);if(value.Terminated)writer.Write((ushort)0);}
}
