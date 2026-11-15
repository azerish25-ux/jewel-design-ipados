using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5RuntimeViewAssertions
{
    private static Guid Id(int n)=>new(n,0,0,0,0,0,0,0,0,0,0);
    public static string Run()
    {
        int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
        void Reject<T>(Action action,string why) where T:Exception {bool failed=false;try{action();}catch(T){failed=true;}Check(failed,why);}
        var runtime=new Rhino5UserDataRuntime();var binding=Rhino5UserStringArchiveBinding.Register(runtime);
        var pairs=new[]{new KeyValuePair<string,string>("key","value"),new("key","値 💎")};var body=LegacyUserStringListCodec.Write(pairs,50);
        var transform=new Rhino5Matrix4(new double[]{1,0,0,.5,0,1,0,-1,0,0,1,2,0,0,0,1});
        var envelopes=new[]{new Rhino5ViewUserData(Rhino5UserStringList.ClassId,Rhino5UserStringList.ClassId,Rhino5UserStringList.OpenNurbs4Id,1,transform,body),new(Id(12),Id(13),Id(14),0,Rhino5Matrix4.Identity,new byte[]{1,2}),new(Id(15),Id(16),Id(17),uint.MaxValue,transform,new byte[]{3,4,5})};
        var capture=Fixture(envelopes);var context=new Rhino5ViewArchiveContext(50,201508103);
        using var source=Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,context);
        Check(ReferenceEquals(source.OriginalArchiveCapture,capture)&&source.Head!.UserDataId==Rhino5UserStringList.ClassId&&source.Head.Next!.UserDataId==Id(13)&&source.Head.Next.Next!.UserDataId==Id(16),"Unknown import retains source list order and independent archive provenance");
        var imported=(Rhino5UnknownUserData)source.Head!;
        Check(imported.ArchiveVersion==50&&imported.OpenNurbsVersion==201508103&&imported.CopyBuffer()!.SequenceEqual(body),"Actual unknown buffer and context owned");
        var strings=(Rhino5UserStringList)source.GetUserData(Rhino5UserStringList.ClassId)!;
        Check(imported.IsDisposed&&strings.CopyPairs().SequenceEqual(pairs)&&binding.CopyReadOrigins(strings)[0].OpenNurbsVersion==201508103,"Real registered body reader materializes imported view userdata");
        var nested=new Rhino5UserStringList(runtime);binding.ReadInto(nested,body,50);strings.AttachUserData(nested);
        var skipped=source.Head!.Next!;var max=skipped.Next!;
        using var copy=source.Duplicate();
        Check(copy.Head!.UserDataId==Id(16)&&copy.Head.CopyCount==1&&copy.Head.Next!.UserDataId==Rhino5UserStringList.ClassId&&copy.Head.Next.CopyCount==2&&copy.Head.Next.Next is null,"Constructor selective virtual copying skips count0, wraps max and prepends eligible clones");
        Check(copy.Head is Rhino5UnknownUserData retainedUnknown&&retainedUnknown.OriginalClassId==Id(15)&&retainedUnknown.ApplicationId==Id(17)&&retainedUnknown.Transform.CopyCoefficients().SequenceEqual(transform.CopyCoefficients())&&retainedUnknown.CopyBuffer()!.SequenceEqual(new byte[]{3,4,5}),"Whole-view copy retains actual unknown identity/transform/buffer rather than fabricating a converted class");
        var copied=(Rhino5UserStringList)copy.Head.Next!;
        Check(ReferenceEquals(copied.Owner,copy)&&!ReferenceEquals(copied,strings)&&ReferenceEquals(copied.Head!.Owner,copied)&&copied.Head.CopyCount==2,"Nested mutable lists and owner links are independent");
        Check(binding.WriteForArchive(copied,50).SequenceEqual(body)&&binding.WriteForArchive((Rhino5UserStringList)copied.Head!,50).SequenceEqual(body),"Actual native codec provenance survives whole-view nested copying");
        Check(copied.Transform.CopyCoefficients().SequenceEqual(transform.CopyCoefficients()),"Converted userdata transform survives real whole-view import and cloning");
        Check(copy.Viewport==capture.Viewport&&copy.TargetMirror==capture.TargetMirror&&copy.TargetMirror!=copy.Viewport.Target,"Whole numeric viewport and stale target mirror copied independently");
        Check(copy.Details==capture.Details&&copy.Details.OpaqueCopyByte==0xa5,"Constructor copies UUID/screen/clip/inverse/near and opaque byte");
        Check(copy.Metadata==capture.Metadata,"All original5 metadata records copied, including raw enums/position bytes and inert paths");
        Check(copy.ClippingPlaneCount==2&&copy.GetClippingPlane(0)==capture.GetClippingPlane(0)&&copy.GetClippingPlane(1)==capture.GetClippingPlane(1),"Ordered clipping records/UUIDs copied");
        Check(BitConverter.DoubleToInt64Bits(copy.Viewport.Frame.Location.X)==unchecked((long)0x7ff8000000001234)&&BitConverter.DoubleToInt64Bits(copy.Viewport.Frame.Direction.X)<0&&BitConverter.DoubleToInt64Bits(copy.Details.PerspectiveMinNearOverFar)<0,"Raw NaN payload and signed zero survive copy");
        for(int i=0;i<16;i++)Check(BitConverter.DoubleToInt64Bits(copy.Details.ClipModification[i/4,i%4])==BitConverter.DoubleToInt64Bits(capture.Details.ClipModification[i/4,i%4]),"Every raw clip coefficient preserved");
        Check(copy.OriginalArchiveCapture!.UserDataCount==3&&copy.OriginalArchiveCapture.GetUserData(1).CopyCount==0&&copy.OriginalArchiveCapture.UnknownChunkCount==2&&copy.OriginalArchiveCapture.GetUnknownChunk(1).CopyEncoded().SequenceEqual(new byte[]{9,8,7}),"File provenance retains count0 and duplicate opaque chunks despite runtime clone filtering");
        var array=copy.CopyClippingPlanes();array[0]=default;
        Check(copy.GetClippingPlane(0).Id==Id(3)&&source.GetClippingPlane(0).Id==Id(3),"Returned arrays cannot mutate source or clone");
        strings.ReplacePairs(new[]{new KeyValuePair<string,string>("edited","source")});
        source.SetConvertedValues(capture.Viewport with {Target=new(7,8,9)},new(100,200,300),capture.Details with {OpaqueCopyByte=0xc7},capture.Metadata with {Name="changed"},[]);
        Check(copy.Metadata.Name==capture.Metadata.Name&&copy.ClippingPlaneCount==2&&copied.CopyPairs().SequenceEqual(pairs),"Source edits cannot mutate cloned values or actual userdata");
        Check(source.OriginalArchiveCapture!.GetUserData(0).CopyPayload().SequenceEqual(body)&&source.OriginalArchiveCapture.Metadata.Name==capture.Metadata.Name,"Original capture remains source provenance rather than an updated save image");
        using var destination=new Rhino5RuntimeView(runtime,capture.Viewport,capture.TargetMirror,capture.Details with {OpaqueCopyByte=0x7e},capture.Metadata with {Name="old"},[]);
        var old=new Rhino5UserStringList(runtime);destination.AttachUserData(old);int calls=0;
        var observer=new Probe(runtime,Id(80)){Observe=()=>{calls++;Check(old.IsDisposed&&destination.Metadata.Name=="old"&&destination.TargetMirror==capture.TargetMirror,"Assignment purges old userdata before clone callbacks and before value mutation");}};
        source.AttachUserData(observer);destination.AssignFrom(source);
        Check(calls==1&&old.IsDisposed&&destination.Metadata.Name=="changed"&&destination.Details.OpaqueCopyByte==0x7e,"Assignment copies once while retaining destination opaque byte");
        Check(destination.Viewport==source.Viewport&&destination.TargetMirror==source.TargetMirror&&destination.Metadata==source.Metadata&&destination.Details==(source.Details with {OpaqueCopyByte=0x7e}),"Assignment copies complete source values except constructor-only byte");
        var head=destination.Head;destination.AssignFrom(destination);
        Check(ReferenceEquals(destination.Head,head)&&calls==1&&!head!.IsDisposed,"Self-assignment neither purges nor copies");
        observer.Observe=()=>calls++;
        using(var observed=source.Duplicate())Check(calls==2,"Constructor invokes virtual copying exactly once");
        source.Dispose();
        Check(strings.IsDisposed&&skipped.IsDisposed&&max.IsDisposed&&source.OriginalArchiveCapture is null,"Disposal purges live source userdata and releases original capture");
        Check(binding.WriteForArchive(copied,50).SequenceEqual(body)&&copy.OriginalArchiveCapture!.GetUserData(0).CopyPayload().SequenceEqual(body),"Copied actual userdata and source bytes survive original disposal");
        Check(destination.GetUserData(Rhino5UserStringList.ClassId) is Rhino5UserStringList assigned&&assigned.CopyPairs()[0].Key=="edited","Assigned live userdata reflects actual edits rather than stale file envelopes");
        Reject<ArgumentNullException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,null!),"Archive context cannot be guessed");
        Reject<ArgumentOutOfRangeException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,new(-1,0)),"Negative context rejected");
        Reject<InvalidDataException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,context,new(MaxBodies:0)),"Body-count budget enforced");
        Reject<InvalidDataException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,context,new(MaxTotalPayloadBytes:1)),"Byte budget enforced");
        Reject<ArgumentOutOfRangeException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,context,new(MaxBodies:-1)),"Invalid limits rejected");
        foreach(var invalid in new[]{new Rhino5ViewUserData(Guid.Empty,Id(90),Id(91),1,Rhino5Matrix4.Identity,new byte[]{1}),new(Rhino5RuntimeUserData.UnknownClassId,Id(90),Id(91),1,Rhino5Matrix4.Identity,new byte[]{1}),new(Id(92),Guid.Empty,Id(91),1,Rhino5Matrix4.Identity,new byte[]{1}),new(Id(92),Id(90),Id(91),1,Rhino5Matrix4.Identity,[])})
            Reject<InvalidDataException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,Fixture(envelopes[2],invalid),context),"Invalid unknown envelope cannot be silently filtered");
        Reject<InvalidDataException>(()=>Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,Fixture(envelopes[2],envelopes[2]),context),"Duplicate IDs reject import with cleanup");
        Check(capture.UserDataCount==3&&capture.GetUserData(2).CopyPayload().SequenceEqual(new byte[]{3,4,5}),"Rejected imports cannot mutate immutable source bytes");
        using(var unsupported=Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,Fixture(envelopes[0]),new(3,123)))
        {
            var retained=unsupported.Head;Reject<ArgumentOutOfRangeException>(()=>unsupported.GetUserData(Rhino5UserStringList.ClassId),"Unsupported body context remains explicit codec failure");
            Check(ReferenceEquals(unsupported.Head,retained)&&!retained!.IsDisposed,"Failed reader retains original unknown object");
        }
        Rhino5RuntimeView Fresh(string name="view",byte raw=0xa5)=>new(runtime,capture.Viewport,capture.TargetMirror,capture.Details with {OpaqueCopyByte=raw},capture.Metadata with {Name=name},[]);
        using(var fresh=Fresh())Check(fresh.Head is null&&fresh.OriginalArchiveCapture is null,"Direct explicit-value construction invents no archive source or userdata");
        using(var empty=Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,Fixture(),context,new(MaxBodies:0,MaxTotalPayloadBytes:0)))Check(empty.Head is null,"Empty import obeys zero budget");
        using(var callbackSource=Fresh())using(var callbackTarget=Fresh("callback",0x10))
        {
            var p=new Probe(runtime,Id(130)){Observe=()=>callbackTarget.SetConvertedValues(callbackTarget.Viewport,callbackTarget.TargetMirror,callbackTarget.Details with {OpaqueCopyByte=0x20},callbackTarget.Metadata,[])};
            callbackSource.AttachUserData(p);callbackTarget.AssignFrom(callbackSource);
            Check(callbackTarget.Details.OpaqueCopyByte==0x20,"Assignment must leave opaque-byte mutation from legitimate copy callbacks intact");
        }
        foreach(int mode in new[]{1,2})
        {
            using var lineage=Fresh();var p=new Probe(runtime,Id(100+mode)){Mode=mode};lineage.AttachUserData(p);using var cloned=lineage.Duplicate();
            Check(cloned.Head is null&&(mode==1||p.LastClone!.IsDisposed),"Null/wrong-lineage virtual clones preserve native rejection and cleanup");
        }
        using(var repeated=Fresh())
        {
            var second=new Probe(runtime,Id(110)){CloneId=Id(112)};var first=new Probe(runtime,Id(111)){CloneId=Id(112)};repeated.AttachUserData(second);repeated.AttachUserData(first);
            using var cloned=repeated.Duplicate();Check(cloned.Head is not null&&cloned.Head.Next is null&&second.LastClone!.IsDisposed,"Duplicate virtual clone rejection disposes returned clone");
        }
        using(var failure=Fresh())
        {
            var throwing=new Probe(runtime,Id(120)){Mode=3};var first=new Probe(runtime,Id(121));failure.AttachUserData(throwing);failure.AttachUserData(first);
            Reject<InvalidOperationException>(()=>failure.Duplicate(),"Managed clone exception propagates");
            Check(first.LastClone!.IsDisposed&&!first.IsDisposed&&ReferenceEquals(failure.Head,first),"Failed managed construction cleans completed clones without damaging source");
            using var target=Fresh("before failure",0x33);var previous=new Rhino5UserStringList(runtime);target.AttachUserData(previous);
            Reject<InvalidOperationException>(()=>target.AssignFrom(failure),"Assignment exception propagates without fabricated rollback");
            Check(previous.IsDisposed&&ReferenceEquals(target.Head,first.LastClone)&&!first.LastClone!.IsDisposed&&target.Metadata.Name=="before failure"&&target.Details.OpaqueCopyByte==0x33,"Assignment failure retains partial userdata mutation and original value order");
        }
        var before=copy.Metadata;Reject<ArgumentNullException>(()=>copy.SetConvertedValues(capture.Viewport,capture.TargetMirror,capture.Details,capture.Metadata with {Name=null!},[]),"Invalid converted values rejected before replacement");
        Check(copy.Metadata==before&&binding.WriteForArchive(copied,50).SequenceEqual(body),"Rejected value update leaves data intact");
        copy.Dispose();copy.Dispose();Check(copy.Head is null&&copied.IsDisposed&&copy.OriginalArchiveCapture is null,"Idempotent disposal releases owned data and capture");
        Reject<ObjectDisposedException>(()=>copy.Duplicate(),"Disposed view cannot be cloned");
        return "PASS original-derived runtime view:"+checks+" field/opaque-byte/real-codec ownership-copy checks; history/document/full3dm/original-PC/physical parity separate.";
    }
    private static Rhino5ViewData Fixture(params Rhino5ViewUserData[] bodies)
    {
        double nan=BitConverter.Int64BitsToDouble(unchecked((long)0x7ff8000000001234));
        var frame=new Rhino5CameraState(new(nan,-0d,80),new(-0d,3,-4),new(5,6,7),new(8,9,10),new(11,12,13),new(14,15,16),false,true,false,true);
        var viewport=new Rhino5ViewportState(frame,new(-3,2,-4,5,.1,100),false,99,0x83,new(1,2,3));
        var matrix=new Rhino5Matrix4(new double[]{nan,-0d,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        var details=new Rhino5ViewportDetails(Id(1),new(-1,900,700,2,0,65535,false),matrix,Rhino5Matrix4.Identity,nan,-0d){OpaqueCopyByte=0xa5};
        var plane=new Rhino5ViewPlane(new(1,2,3),new(4,5,6),new(7,8,9),new(10,11,12),new(13,14,15,16));
        var metadata=new Rhino5ViewMetadata(true,"名前 α",Id(2),-77,new(-.2,1.3,.1,.9,1,2,3,4,5),42,new(9,210,297,1,2,3,4,@"C:\unavailable\printer"),new(plane,.5,.25,70,5,true,"grid"),true,false,true,new(plane,123,456,@"C:\unavailable\trace.png",true,false,true),new(@"C:\unavailable\wall.png",false,true));
        var clips=new[]{new Rhino5ClippingPlaneInfo(new(1,2,3,4),Id(3),true),new(new(5,6,7,8),Id(4),false)};var opaque=new Rhino5PreservedViewChunk(123,-91,new byte[]{9,8,7});
        return new(viewport,new(90,80,70),details,metadata,clips,bodies,new[]{opaque,opaque});
    }
    // Test-only dispatch observer. Functional bodies above use actual supported unknown/string-list classes.
    private sealed class Probe:Rhino5RuntimeUserData
    {
        public int Mode;public Guid? CloneId;public Action? Observe;public Rhino5UserDataOwner? LastClone;
        public override Guid RuntimeClassId=>Id(999);
        public Probe(Rhino5UserDataRuntime runtime,Guid id):base(runtime){UserDataId=id;CopyCount=1;}
        private Probe(Probe source):base(source){if(source.CloneId is Guid id)UserDataId=id;}
        public override Rhino5UserDataOwner? Duplicate(){Observe?.Invoke();if(Mode==3)throw new InvalidOperationException("test clone failure");return LastClone=Mode switch {1=>null,2=>new Rhino5UserDataOwner(Runtime),_=>new Probe(this)};}
    }
}
