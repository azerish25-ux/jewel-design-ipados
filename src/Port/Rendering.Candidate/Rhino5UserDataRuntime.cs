namespace JewelDesign.Rendering.Candidate;

/// <summary>Explicit class factory/reader bindings. No automatic plugin loading or generic byte-to-object reader.</summary>
public sealed class Rhino5UserDataRuntime
{
    internal sealed record Binding(Func<Rhino5UserDataRuntime,Rhino5UserDataOwner?> Create,Func<Rhino5RuntimeUserData,Rhino5UserDataArchive,bool> Read);
    private readonly Dictionary<Guid,Binding> bindings=new();
    public bool LeakUserDataOnPurge {get;set;}
    public int DiagnosticCount {get;private set;}
    internal void Error()=>DiagnosticCount++;
    public void RegisterClass(Guid id,Func<Rhino5UserDataRuntime,Rhino5UserDataOwner?> create,Func<Rhino5RuntimeUserData,Rhino5UserDataArchive,bool> read)
    {ArgumentNullException.ThrowIfNull(create);ArgumentNullException.ThrowIfNull(read);bindings.Add(id,new(create,read));}
    internal Binding? Find(Guid id)=>bindings.GetValueOrDefault(id);
}

public sealed class Rhino5UserDataArchive
{
    private readonly byte[] bytes;
    public int ArchiveVersion {get;}
    public uint OpenNurbsVersion {get;}
    internal Rhino5UserDataArchive(ReadOnlySpan<byte> data,int version,uint build){bytes=data.ToArray();ArchiveVersion=version;OpenNurbsVersion=build;}
    public byte[] CopyBytes()=>(byte[])bytes.Clone();
}

/// <summary>Original-derived mutable object list protocol; converted current-view data remains a separate immutable model.</summary>
public class Rhino5UserDataOwner:IDisposable
{
    public Rhino5UserDataRuntime Runtime {get;}
    public Rhino5RuntimeUserData? Head {get;private set;}
    public bool IsDisposed {get;private set;}
    public Rhino5UserDataOwner(Rhino5UserDataRuntime runtime){ArgumentNullException.ThrowIfNull(runtime);Runtime=runtime;}
    protected void Alive()=>ObjectDisposedException.ThrowIf(IsDisposed,this);
    public bool AttachUserData(Rhino5RuntimeUserData? data)
    {
        Alive();if(data is null)return false;ObjectDisposedException.ThrowIf(data.IsDisposed,data);
        if(data.Owner is not null||data.UserDataId==Guid.Empty||GetUserData(data.UserDataId) is not null)return false;
        if(data.RuntimeClassId==Rhino5RuntimeUserData.UnknownClassId&&!(data is Rhino5UnknownUserData unknown&&unknown.IsValid())){Runtime.Error();return false;}
        data.Owner=this;data.Next=Head;Head=data;return true;
    }
    public bool DetachUserData(Rhino5RuntimeUserData? data)
    {
        Alive();if(data is null||!ReferenceEquals(data.Owner,this))return false;
        Rhino5RuntimeUserData? previous=null;
        for(var current=Head;current is not null;current=current.Next)
        {
            if(ReferenceEquals(current,data)){if(previous is null)Head=current.Next;else previous.Next=current.Next;current.Owner=null;current.Next=null;return true;}
            previous=current;
        }
        return false;
    }
    public Rhino5RuntimeUserData? GetUserData(Guid id)
    {
        Alive();Rhino5RuntimeUserData? previous=null;
        for(var current=Head;current is not null;previous=current,current=current.Next)
        {
            if(current.UserDataId!=id)continue;
            if(current.RuntimeClassId!=Rhino5RuntimeUserData.UnknownClassId||current is not Rhino5UnknownUserData unknown)return current;
            var converted=unknown.TryConvert();if(converted is null)return current;
            if(previous is not null)previous.Next=converted;
            else if(ReferenceEquals(current,Head)){Head=converted;converted.Owner=this;}
            // Original non-head replacement does not set owner. Preserve that observable legacy quirk.
            converted.Next=current.Next;current.Next=null;current.Owner=null;current.Dispose();return converted;
        }
        return null;
    }
    public void CopyUserDataFrom(Rhino5UserDataOwner source)
    {
        Alive();ArgumentNullException.ThrowIfNull(source);source.Alive();
        for(var current=source.Head;current is not null;current=current.Next)
        {
            if(current.CopyCount==0)continue;
            var clone=current.Duplicate();if(clone is null)continue;
            if(clone is not Rhino5RuntimeUserData data||!AttachUserData(data))clone.Dispose();
        }
    }
    public void PurgeUserData()
    {
        Alive();var next=Head;
        // Original5 retains the head until all nodes are processed and samples the flag per node.
        while(next is not null){bool delete=!Runtime.LeakUserDataOnPurge;var current=next;next=current.Next;current.Owner=null;current.Next=null;if(delete)current.Dispose();}
        Head=null;
    }
    public virtual void Dispose(){if(IsDisposed)return;PurgeUserData();IsDisposed=true;}
}

public abstract class Rhino5RuntimeUserData:Rhino5UserDataOwner
{
    public static readonly Guid BaseClassId=Guid.Parse("850324a7-050e-11d4-bffa-0010830122f0");
    public static readonly Guid UnknownClassId=Guid.Parse("850324a8-050e-11d4-bffa-0010830122f0");
    public Guid UserDataId {get;set;}
    public Guid ApplicationId {get;set;}
    public uint CopyCount {get;set;}
    private Rhino5Matrix4 transform=Rhino5Matrix4.Identity;
    public Rhino5Matrix4 Transform {get=>transform;set{ArgumentNullException.ThrowIfNull(value);transform=value;}}
    public Rhino5UserDataOwner? Owner {get;internal set;}
    public Rhino5RuntimeUserData? Next {get;internal set;}
    public abstract Guid RuntimeClassId {get;}
    protected virtual bool Archives=>false;
    protected Rhino5RuntimeUserData(Rhino5UserDataRuntime runtime):base(runtime){}
    protected Rhino5RuntimeUserData(Rhino5RuntimeUserData source):base(source.Runtime)
    {source.Alive();CopyUserDataFrom(source);UserDataId=source.UserDataId;ApplicationId=source.ApplicationId;CopyCount=Increment(source.CopyCount);Transform=source.Transform;}
    private static uint Increment(uint count){if(count==0)return 0;count=unchecked(count+1);return count==0?1:count;}
    public virtual bool IsValid()=>UserDataId!=Guid.Empty&&UserDataId!=BaseClassId&&(!Archives||RuntimeClassId!=BaseClassId);
    public abstract Rhino5UserDataOwner? Duplicate();
    public void AssignBaseFrom(Rhino5RuntimeUserData source)
    {
        Alive();ArgumentNullException.ThrowIfNull(source);source.Alive();if(ReferenceEquals(this,source))return;
        PurgeUserData();CopyUserDataFrom(source);CopyCount=Increment(source.CopyCount);Transform=source.Transform;
        // Identity and owner/next links are intentionally retained by base assignment.
    }
    public override void Dispose(){if(IsDisposed)return;if(Owner is not null)Owner.DetachUserData(this);base.Dispose();}
}
