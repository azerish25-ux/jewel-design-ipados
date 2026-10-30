namespace JewelDesign.Documents;

/// <summary>Immutable stored-double/header schema. It does not imply runtime Boolean/chooser behavior.</summary>
public class LegacyCsg4Scalar:ILegacyCsg4Header
{
    private readonly StoredText name;
    private readonly (StoredText Type,StoredText Key,StoredText Value)[] userData;
    private readonly int[] objects,channels,categories;
    internal LegacyCsg4Version Version {get;}
    public string Identifier=>name.Value;
    public uint Flags {get;}
    public int LegacyLayer {get;}
    public int LegacyUiInteger {get;}
    public double Value {get;}
    public int OriginalValueOffset {get;}
    public IReadOnlyList<int> ObjectIndices {get;}
    public IReadOnlyList<int> ChannelIndices {get;}
    public IReadOnlyList<int> CategoryIndices {get;}
    public IReadOnlyList<LegacyUserDataEntry> UserData {get;}
    internal LegacyCsg4Scalar(LegacyCsg4Version version,StoredText name,uint flags,int layer,
        (StoredText Type,StoredText Key,StoredText Value)[] userData,int[] objects,int[] channels,int[] categories,
        int ui,double value,int originalValueOffset)
    {
        Version=version;this.name=name;Flags=flags;LegacyLayer=layer;this.userData=userData;
        this.objects=objects;this.channels=channels;this.categories=categories;LegacyUiInteger=ui;
        Value=value;OriginalValueOffset=originalValueOffset;
        ObjectIndices=Array.AsReadOnly(objects);ChannelIndices=Array.AsReadOnly(channels);CategoryIndices=Array.AsReadOnly(categories);
        UserData=Array.AsReadOnly(userData.Select(x=>new LegacyUserDataEntry(x.Type.Value,x.Key.Value,x.Value.Value)).ToArray());
    }
    protected LegacyCsg4Scalar(LegacyCsg4Scalar source,double value)
    {
        Version=source.Version;name=source.name;Flags=source.Flags;LegacyLayer=source.LegacyLayer;userData=source.userData;
        objects=source.objects;channels=source.channels;categories=source.categories;LegacyUiInteger=source.LegacyUiInteger;
        Value=value;OriginalValueOffset=source.OriginalValueOffset;
        ObjectIndices=source.ObjectIndices;ChannelIndices=source.ChannelIndices;CategoryIndices=source.CategoryIndices;UserData=source.UserData;
    }
    internal virtual LegacyCsg4Scalar WithValue(double value)=>new(this,value);
    internal byte[] Encode()
    {
        using var stream=new MemoryStream();using var writer=new BinaryWriter(stream);
        writer.Write(Version.Major);writer.Write(Version.Minor);WriteText(writer,name);writer.Write(Flags);writer.Write(LegacyLayer);
        writer.Write(userData.Length);foreach(var item in userData){WriteText(writer,item.Type);WriteText(writer,item.Key);WriteText(writer,item.Value);}
        WriteInts(writer,objects);WriteInts(writer,channels);WriteInts(writer,categories);writer.Write(LegacyUiInteger);
        writer.Write(BitConverter.DoubleToInt64Bits(Value));WriteTail(writer);writer.Flush();return stream.ToArray();
    }
    protected virtual void WriteTail(BinaryWriter writer) { }
    private static void WriteInts(BinaryWriter writer,int[] values)
    {writer.Write(values.Length);foreach(int value in values)writer.Write(value);}
    private static void WriteText(BinaryWriter writer,StoredText text)
    {
        writer.Write(text.Terminated?checked(text.Value.Length+1):0);
        foreach(char value in text.Value)writer.Write((ushort)value);
        if(text.Terminated)writer.Write((ushort)0);
    }
}
