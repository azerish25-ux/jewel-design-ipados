namespace JewelDesign.Documents;

/// <summary>Decoded legacy header, independent of active channel evaluation.</summary>
public interface ILegacyCsg4Header
{
    string Identifier {get;}
    uint Flags {get;}
    int LegacyLayer {get;}
    IReadOnlyList<int> ObjectIndices {get;}
    IReadOnlyList<int> ChannelIndices {get;}
    IReadOnlyList<LegacyUserDataEntry> UserData {get;}
}

/// <summary>Stored NURBS data; rational coordinates remain homogeneous. No native geometry is created.</summary>
public sealed class LegacyNurbsCurveData
{
    public byte FormatVersion {get;}
    public int Dimension {get;}
    public bool IsRational {get;}
    public int Order {get;}
    public int ControlVertexCount {get;}
    public int OriginalDataOffset {get;}
    public int OriginalKnotOffset {get;}
    public int OriginalControlOffset {get;}
    public IReadOnlyList<int> Reserved {get;}
    public IReadOnlyList<double> BoundingBoxValues {get;}
    public IReadOnlyList<double> Knots {get;}
    public IReadOnlyList<double> ControlValues {get;}
    public double DomainStart=>Knots[Order-2];
    public double DomainEnd=>Knots[ControlVertexCount-1];
    internal LegacyNurbsCurveData(byte version,int dimension,bool rational,int order,int count,int[] reserved,
        double[] box,double[] knots,double[] controls,int start,int knotOffset,int controlOffset)
    {
        FormatVersion=version;Dimension=dimension;IsRational=rational;Order=order;ControlVertexCount=count;
        Reserved=Array.AsReadOnly(reserved);BoundingBoxValues=Array.AsReadOnly(box);Knots=Array.AsReadOnly(knots);ControlValues=Array.AsReadOnly(controls);
        OriginalDataOffset=start;OriginalKnotOffset=knotOffset;OriginalControlOffset=controlOffset;
    }
    internal void Write(BinaryWriter writer)
    {
        writer.Write(FormatVersion);writer.Write(Dimension);writer.Write(IsRational?1:0);writer.Write(Order);writer.Write(ControlVertexCount);
        foreach(int value in Reserved)writer.Write(value);
        foreach(double value in BoundingBoxValues)writer.Write(BitConverter.DoubleToInt64Bits(value));
        writer.Write(Knots.Count);foreach(double value in Knots)writer.Write(BitConverter.DoubleToInt64Bits(value));
        writer.Write(ControlVertexCount);foreach(double value in ControlValues)writer.Write(BitConverter.DoubleToInt64Bits(value));
    }
}

/// <summary>Immutable profile group. Serialized names are labels, never file paths to resolve.</summary>
public sealed class LegacyCsg4MatrixProfiles:ILegacyCsg4Header
{
    private readonly LegacyCsg4Version version;
    private readonly StoredText identifier;
    private readonly (StoredText Type,StoredText Key,StoredText Value)[] storedUserData;
    private readonly StoredText[] storedNames;
    public string Identifier=>identifier.Value;
    public uint Flags {get;}
    public int LegacyLayer {get;}
    public IReadOnlyList<int> ObjectIndices {get;}
    public IReadOnlyList<int> ChannelIndices {get;}
    public IReadOnlyList<LegacyUserDataEntry> UserData {get;}
    public IReadOnlyList<LegacyNurbsCurveData> Curves {get;}
    public IReadOnlyList<IReadOnlyList<double>> SweepParameters {get;}
    public IReadOnlyList<string> Names {get;}
    internal LegacyCsg4MatrixProfiles(LegacyCsg4Version version,StoredText identifier,uint flags,int layer,
        (StoredText Type,StoredText Key,StoredText Value)[] userData,int[] objects,int[] channels,
        LegacyNurbsCurveData[] curves,double[][] sweep,StoredText[] names)
    {
        this.version=version;this.identifier=identifier;Flags=flags;LegacyLayer=layer;storedUserData=userData;storedNames=names;
        ObjectIndices=Array.AsReadOnly(objects);ChannelIndices=Array.AsReadOnly(channels);
        UserData=Array.AsReadOnly(userData.Select(x=>new LegacyUserDataEntry(x.Type.Value,x.Key.Value,x.Value.Value)).ToArray());
        Curves=Array.AsReadOnly(curves);
        SweepParameters=Array.AsReadOnly(sweep.Select(x=>(IReadOnlyList<double>)Array.AsReadOnly(x)).ToArray());
        Names=Array.AsReadOnly(names.Select(x=>x.Value).ToArray());
    }
    internal byte[] Encode()
    {
        using var stream=new MemoryStream();using var writer=new BinaryWriter(stream);
        writer.Write(version.Major);writer.Write(version.Minor);Text(writer,identifier);writer.Write(Flags);writer.Write(LegacyLayer);
        writer.Write(storedUserData.Length);foreach(var item in storedUserData){Text(writer,item.Type);Text(writer,item.Key);Text(writer,item.Value);}
        Ints(writer,ObjectIndices);Ints(writer,ChannelIndices);
        writer.Write(Curves.Count);foreach(var curve in Curves)curve.Write(writer);
        writer.Write(SweepParameters.Count);foreach(var values in SweepParameters){writer.Write(values.Count);foreach(double value in values)writer.Write(BitConverter.DoubleToInt64Bits(value));}
        if(version.Minor>=1){writer.Write(storedNames.Length);foreach(var name in storedNames)Text(writer,name);}
        writer.Flush();return stream.ToArray();
    }
    private static void Ints(BinaryWriter writer,IReadOnlyList<int> values){writer.Write(values.Count);foreach(int value in values)writer.Write(value);}
    private static void Text(BinaryWriter writer,StoredText text)
    {
        writer.Write(text.Terminated?checked(text.Value.Length+1):0);
        foreach(char value in text.Value)writer.Write((ushort)value);
        if(text.Terminated)writer.Write((ushort)0);
    }
}

internal static class LegacyMatrixProfilesCodec
{
    internal static LegacyCsg4MatrixProfiles Read(CsgCursor r,LegacyCsg4Version version,int end,LegacyCsg4Limits limits)
    {
        var name=r.Text();uint flags=r.UInt();int layer=r.Int();var data=r.UserData();var objects=r.Ints();var channels=r.Ints();
        if(objects.Length!=channels.Length)throw new InvalidDataException("Matrix profile link count mismatch");
        int count=r.Count(limits.MaxArrayItems);var curves=new List<LegacyNurbsCurveData>();
        for(int i=0;i<count;i++)curves.Add(ReadCurve(r,limits));
        int sweepCount=r.Count(limits.MaxArrayItems);r.NeedItems(sweepCount,4);var sweep=new double[sweepCount][];
        for(int i=0;i<sweepCount;i++)sweep[i]=Doubles(r,r.Count(limits.MaxArrayItems));
        var names=Array.Empty<StoredText>();
        if(version.Minor>=1){int n=r.Count(limits.MaxArrayItems);r.NeedItems(n,4);names=new StoredText[n];for(int i=0;i<n;i++)names[i]=r.Text();}
        if(r.Position!=end)throw new InvalidDataException("Unexpected trailing matrix profile data");
        return new(version,name,flags,layer,data,objects,channels,curves.ToArray(),sweep,names);
    }
    private static LegacyNurbsCurveData ReadCurve(CsgCursor r,LegacyCsg4Limits limits)
    {
        int start=r.Position;byte version=r.Byte();
        if(version!=0x10)throw new NotSupportedException("Nested NURBS format is not decoded");
        int dimension=r.Count(limits.MaxArrayItems),rational=r.Int(),order=r.Count(limits.MaxArrayItems),count=r.Count(limits.MaxArrayItems);
        if(dimension<1||(rational!=0&&rational!=1)||order<2||count<order)throw new InvalidDataException("Invalid stored NURBS header");
        int[] reserved={r.Int(),r.Int()};var box=Doubles(r,6);
        int knotCount=r.Count(limits.MaxArrayItems);
        if((long)knotCount!=(long)order+count-2)throw new InvalidDataException("Stored NURBS knot count mismatch");
        int knotOffset=r.Position;var knots=Doubles(r,knotCount);
        int storedCount=r.Count(limits.MaxArrayItems);
        if(storedCount!=count)throw new InvalidDataException("Stored NURBS control count mismatch");
        long valueCount=(long)count*((long)dimension+rational);
        if(valueCount>int.MaxValue)throw new InvalidDataException("Stored NURBS array exceeds supported byte bound");
        int controlOffset=r.Position;var controls=Doubles(r,(int)valueCount);
        return new(version,dimension,rational==1,order,count,reserved,box,knots,controls,start,knotOffset,controlOffset);
    }
    private static double[] Doubles(CsgCursor r,int count)
    {r.NeedItems(count,8);var values=new double[count];for(int i=0;i<count;i++)values[i]=r.Double();return values;}
}
