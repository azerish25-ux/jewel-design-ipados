namespace JewelDesign.Rendering.Candidate;

/// <summary>Original unknown-buffer ownership/copy policy, not an arbitrary native plugin implementation.</summary>
public sealed class Rhino5UnknownUserData:Rhino5RuntimeUserData
{
    private byte[]? buffer;
    public Guid OriginalClassId {get;private set;}
    public int DeclaredLength {get;private set;}
    public int ArchiveVersion {get;private set;}
    public uint OpenNurbsVersion {get;private set;}
    public override Guid RuntimeClassId=>UnknownClassId;
    protected override bool Archives=>true;
    public Rhino5UnknownUserData(Rhino5UserDataRuntime runtime,Guid id,Guid app,uint count,Rhino5Matrix4 transform,
        Guid originalClassId,byte[]? data,int declaredLength,int archiveVersion,uint openNurbsVersion):base(runtime)
    {UserDataId=id;ApplicationId=app;CopyCount=count;Transform=transform;OriginalClassId=originalClassId;buffer=data is null?null:(byte[])data.Clone();DeclaredLength=declaredLength;ArchiveVersion=archiveVersion;OpenNurbsVersion=openNurbsVersion;}
    private Rhino5UnknownUserData(Rhino5UnknownUserData source):base(source)
    {if(CopyCount!=0&&source.DeclaredLength>0&&source.buffer is not null)CopyUnknownFields(source);}
    private byte[] ReadableBytes()
    {
        if(buffer is null||DeclaredLength<0||DeclaredLength>buffer.Length)throw new InvalidDataException("Unknown userdata buffer does not cover its declared length.");
        return buffer.AsSpan(0,DeclaredLength).ToArray();
    }
    private void CopyUnknownFields(Rhino5UnknownUserData source)
    {OriginalClassId=source.OriginalClassId;DeclaredLength=source.DeclaredLength;buffer=source.ReadableBytes();ArchiveVersion=source.ArchiveVersion;OpenNurbsVersion=source.OpenNurbsVersion;}
    public byte[]? CopyBuffer()=>buffer is null?null:(byte[])buffer.Clone();
    public override bool IsValid()=>base.IsValid()&&DeclaredLength>0&&buffer is not null&&OriginalClassId!=Guid.Empty&&OriginalClassId!=UnknownClassId;
    public override Rhino5UserDataOwner Duplicate(){Alive();return new Rhino5UnknownUserData(this);}
    public void AssignFrom(Rhino5UnknownUserData source)
    {
        Alive();ArgumentNullException.ThrowIfNull(source);source.Alive();if(ReferenceEquals(this,source))return;
        buffer=null;DeclaredLength=0;AssignBaseFrom(source);UserDataId=source.UserDataId;ApplicationId=source.ApplicationId;
        if(CopyCount!=0&&source.DeclaredLength>0&&source.buffer is not null)CopyUnknownFields(source);
        else {UserDataId=Guid.Empty;OriginalClassId=Guid.Empty;DeclaredLength=0;buffer=null;ArchiveVersion=0;OpenNurbsVersion=0;}
    }
    public Rhino5RuntimeUserData? TryConvert()
    {
        Alive();if(!IsValid())return null;var binding=Runtime.Find(OriginalClassId);if(binding is null)return null;
        var created=binding.Create(Runtime);if(created is null)return null;
        if(created is not Rhino5RuntimeUserData data){created.Dispose();return null;}
        try
        {
            data.CopyCount=CopyCount;data.Transform=Transform;
            binding.Read(data,new Rhino5UserDataArchive(ReadableBytes(),ArchiveVersion,OpenNurbsVersion));
            // Native conversion ignores reader bool and retains constructor/reader-provided identity.
            return data;
        }
        catch {created.Dispose();throw;}
    }
    public override void Dispose(){if(IsDisposed)return;buffer=null;base.Dispose();}
}

/// <summary>Converted string-list value/copy behavior. Register a real archive reader to convert an opaque native body.</summary>
public sealed class Rhino5UserStringList:Rhino5RuntimeUserData
{
    public static readonly Guid ClassId=Guid.Parse("ce28de29-f4c5-4faa-a50a-c3a6849b6329");
    public static readonly Guid OpenNurbs4Id=Guid.Parse("17b3ecda-17ba-4e45-9e67-a2b8d9be520d");
    private KeyValuePair<string,string>[] pairs=Array.Empty<KeyValuePair<string,string>>();
    public override Guid RuntimeClassId=>ClassId;
    protected override bool Archives=>true;
    public Rhino5UserStringList(Rhino5UserDataRuntime runtime):base(runtime){UserDataId=ClassId;ApplicationId=OpenNurbs4Id;CopyCount=1;}
    private Rhino5UserStringList(Rhino5UserStringList source):base(source){pairs=(KeyValuePair<string,string>[])source.pairs.Clone();}
    public KeyValuePair<string,string>[] CopyPairs()=>(KeyValuePair<string,string>[])pairs.Clone();
    public void ReplacePairs(ReadOnlySpan<KeyValuePair<string,string>> values)
    {Alive();var copy=values.ToArray();foreach(var p in copy){ArgumentNullException.ThrowIfNull(p.Key);ArgumentNullException.ThrowIfNull(p.Value);}pairs=copy;}
    public override Rhino5UserDataOwner Duplicate(){Alive();return new Rhino5UserStringList(this);}
    public void AssignFrom(Rhino5UserStringList source)
    {Alive();ArgumentNullException.ThrowIfNull(source);source.Alive();if(ReferenceEquals(this,source))return;AssignBaseFrom(source);pairs=(KeyValuePair<string,string>[])source.pairs.Clone();}
    public override void Dispose(){if(IsDisposed)return;pairs=Array.Empty<KeyValuePair<string,string>>();base.Dispose();}
}
