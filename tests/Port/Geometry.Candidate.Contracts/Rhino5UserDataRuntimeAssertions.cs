using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5UserDataRuntimeAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static Guid Id(int n)=>new(n,0,0,0,0,0,0,0,0,0,0);
    private sealed class Probe:Rhino5RuntimeUserData
    {
        public int CloneMode;public Rhino5UserDataOwner? LastClone;public Action? BeforeDispose;
        public Probe(Rhino5UserDataRuntime r,Guid id,uint count=1):base(r){UserDataId=id;CopyCount=count;}
        private Probe(Probe source):base(source){CloneMode=source.CloneMode;}
        public override Guid RuntimeClassId=>Id(900);
        public override void Dispose(){if(!IsDisposed)BeforeDispose?.Invoke();base.Dispose();}
        public override Rhino5UserDataOwner? Duplicate(){LastClone=CloneMode==1?null:CloneMode==2?new Rhino5UserDataOwner(Runtime):new Probe(this);return LastClone;}
    }
    public static string Run()
    {
        checks=0;var runtime=new Rhino5UserDataRuntime();using var owner=new Rhino5UserDataOwner(runtime);
        Check(!owner.AttachUserData(null),"Null attach rejected");var nil=new Probe(runtime,Guid.Empty);Check(!owner.AttachUserData(nil),"Nil userdata ID rejected");
        var first=new Probe(runtime,Id(1));var second=new Probe(runtime,Id(2));Check(owner.AttachUserData(first)&&owner.AttachUserData(second)&&ReferenceEquals(owner.Head,second)&&ReferenceEquals(second.Next,first),"Attach prepends");
        Check(ReferenceEquals(first.Owner,owner)&&!new Rhino5UserDataOwner(runtime).AttachUserData(first),"Already owned node rejected");var duplicate=new Probe(runtime,Id(1));Check(!owner.AttachUserData(duplicate)&&!duplicate.IsDisposed,"Rejected direct attach retains caller ownership");
        Check(owner.DetachUserData(first)&&first.Owner is null&&first.Next is null&&!owner.DetachUserData(first),"Detach clears links exactly once");
        var invalidUnknown=new Rhino5UnknownUserData(runtime,Id(3),Id(4),1,Rhino5Matrix4.Identity,Id(5),null,7,50,20150810);Check(!owner.AttachUserData(invalidUnknown)&&runtime.DiagnosticCount==1,"Invalid unknown diagnosed and rejected");
        var a=new Probe(runtime,Id(10),1);var b=new Probe(runtime,Id(11),uint.MaxValue);var zero=new Probe(runtime,Id(12),0);using var source=new Rhino5UserDataOwner(runtime);source.AttachUserData(b);source.AttachUserData(zero);source.AttachUserData(a);
        var nested=new Probe(runtime,Id(13),2);a.AttachUserData(nested);using var target=new Rhino5UserDataOwner(runtime);target.CopyUserDataFrom(source);
        Check(target.Head!.UserDataId==Id(11)&&target.Head.CopyCount==1&&target.Head.Next!.UserDataId==Id(10)&&target.Head.Next.CopyCount==2&&target.Head.Next.Next is null,"Eligible copy reverses through prepend and wraps count past zero");
        Check(target.Head.Next!.Head!.CopyCount==3&&ReferenceEquals(target.Head.Next.Head.Owner,target.Head.Next)&&ReferenceEquals(nested.Owner,a),"Nested copies have independent ownership");
        foreach(int mode in new[]{1,2}){using var s=new Rhino5UserDataOwner(runtime);var p=new Probe(runtime,Id(20+mode)){CloneMode=mode};s.AttachUserData(p);using var d=new Rhino5UserDataOwner(runtime);d.CopyUserDataFrom(s);Check(d.Head is null&&(mode==1||p.LastClone!.IsDisposed),"Null/wrong-type clones skipped with cleanup");}
        using(var d=new Rhino5UserDataOwner(runtime)){d.AttachUserData(new Probe(runtime,Id(10)));d.CopyUserDataFrom(source);Check(a.LastClone!.IsDisposed,"Duplicate clone is destroyed after failed attachment");}
        var strings=new Rhino5UserStringList(runtime);strings.ReplacePairs(new[]{new KeyValuePair<string,string>("key","値")});var stringsCopy=(Rhino5UserStringList)strings.Duplicate()!;
        strings.ReplacePairs(new[]{new KeyValuePair<string,string>("changed","x")});Check(stringsCopy.CopyPairs()[0].Value=="値"&&stringsCopy.CopyCount==2&&stringsCopy.Owner is null,"String-list constructor copies values and increments count");
        stringsCopy.UserDataId=Id(30);stringsCopy.ApplicationId=Id(31);stringsCopy.AssignFrom(strings);Check(stringsCopy.UserDataId==Id(30)&&stringsCopy.ApplicationId==Id(31)&&stringsCopy.CopyPairs()[0].Key=="changed","String-list assignment retains destination IDs");uint count=stringsCopy.CopyCount;stringsCopy.AssignFrom(stringsCopy);Check(stringsCopy.CopyCount==count,"Self assignment is no-op");
        byte[] bytes={1,2,3};var unknown=new Rhino5UnknownUserData(runtime,Id(40),Id(41),uint.MaxValue,Rhino5Matrix4.Identity,Id(42),bytes,3,50,20150810);bytes[0]=99;var unknownCopy=(Rhino5UnknownUserData)unknown.Duplicate()!;
        Check(unknownCopy.CopyCount==1&&unknownCopy.UserDataId==Id(40)&&unknownCopy.ApplicationId==Id(41)&&unknownCopy.CopyBuffer()!.SequenceEqual(new byte[]{1,2,3})&&unknownCopy.Owner is null,"Unknown constructor owns bytes, copies identity and increments wrapped count");
        unknown.CopyCount=0;var noCopy=(Rhino5UnknownUserData)unknown.Duplicate()!;Check(noCopy.UserDataId==unknown.UserDataId&&noCopy.OriginalClassId==Guid.Empty&&noCopy.CopyBuffer() is null&&noCopy.CopyCount==0,"Zero-count unknown constructor retains base IDs but omits buffer");
        owner.AttachUserData(unknownCopy);unknownCopy.AssignFrom(unknown);Check(unknownCopy.UserDataId==Guid.Empty&&unknownCopy.ApplicationId==Id(41)&&unknownCopy.OriginalClassId==Guid.Empty&&ReferenceEquals(unknownCopy.Owner,owner),"Unknown ineligible assignment clears data ID/buffer, copies app ID and preserves owner");
        var opaque=new Rhino5UnknownUserData(runtime,Id(50),Id(51),7,Rhino5Matrix4.Identity,Id(52),new byte[]{8,9},2,50,20150810);using var converting=new Rhino5UserDataOwner(runtime);converting.AttachUserData(opaque);Check(ReferenceEquals(converting.GetUserData(Id(50)),opaque),"Unavailable class keeps unknown instance");
        bool read=false;runtime.RegisterClass(Id(52),r=>new Probe(r,Id(53)),(data,archive)=>{read=true;Check(data.CopyCount==7&&archive.ArchiveVersion==50&&archive.OpenNurbsVersion==20150810&&archive.CopyBytes().SequenceEqual(new byte[]{8,9}),"Count/transform and actual payload supplied before read");return false;});
        var converted=converting.GetUserData(Id(50));Check(read&&converted!.UserDataId==Id(53)&&ReferenceEquals(converted.Owner,converting)&&opaque.IsDisposed,"Read false is ignored; head replaced with constructor identity and owner");Check(converting.GetUserData(Id(50)) is null,"Conversion does not restore old userdata ID");
        var middle=new Rhino5UnknownUserData(runtime,Id(60),Id(61),4,Rhino5Matrix4.Identity,Id(62),new byte[]{1},1,4,20090101);var head=new Probe(runtime,Id(63));using var middleOwner=new Rhino5UserDataOwner(runtime);middleOwner.AttachUserData(middle);middleOwner.AttachUserData(head);runtime.RegisterClass(Id(62),r=>new Probe(r,Id(60)),(_,_)=>true);var replacement=middleOwner.GetUserData(Id(60));Check(ReferenceEquals(head.Next,replacement)&&replacement!.Owner is null&&middle.IsDisposed,"Original non-head conversion leaves replacement owner null");
        Rhino5UserDataOwner? wrong=null;runtime.RegisterClass(Id(70),r=>wrong=new Rhino5UserDataOwner(r),(_,_)=>throw new Exception("Wrong-type reader must not run"));var badFactory=new Rhino5UnknownUserData(runtime,Id(71),Id(72),1,Rhino5Matrix4.Identity,Id(70),new byte[]{1},1,4,1);Check(badFactory.TryConvert() is null&&wrong!.IsDisposed,"Wrong-type factory object disposed");
        runtime.RegisterClass(Id(73),_=>null,(_,_)=>throw new Exception("Null factory reader must not run"));var nullFactory=new Rhino5UnknownUserData(runtime,Id(74),Id(75),1,Rhino5Matrix4.Identity,Id(73),new byte[]{1},1,4,1);Check(nullFactory.TryConvert() is null,"Null factory stays unconverted");
        using(var leaking=new Rhino5UserDataOwner(runtime)){var p=new Probe(runtime,Id(80));leaking.AttachUserData(p);runtime.LeakUserDataOnPurge=true;leaking.PurgeUserData();Check(!p.IsDisposed&&p.Owner is null&&p.Next is null&&leaking.Head is null,"Diagnostic leak option clears links without deleting");runtime.LeakUserDataOnPurge=false;p.Dispose();}
        middleOwner.PurgeUserData();Check(replacement!.IsDisposed&&head.IsDisposed,"Purge handles owner-null converted node");
        owner.PurgeUserData();Check(second.IsDisposed&&unknownCopy.IsDisposed&&owner.Head is null,"Purge disposes detached data and empties list");
        foreach(uint rawCount in new[]{0u,1u,0x7fffffffu,0x80000000u,uint.MaxValue})
        {
            uint expected=rawCount==0?0:rawCount==uint.MaxValue?1:unchecked(rawCount+1);
            var list=new Rhino5UserStringList(runtime){CopyCount=rawCount};var cloned=(Rhino5UserStringList)list.Duplicate();Check(cloned.CopyCount==expected,"Full32-bit count constructor boundary");var assigned=new Rhino5UserStringList(runtime);assigned.AssignFrom(list);Check(assigned.CopyCount==expected,"Full32-bit count assignment boundary");cloned.Dispose();assigned.Dispose();list.Dispose();
        }
        var zeroUnknown=new Rhino5UnknownUserData(runtime,Id(90),Guid.Empty,0,Rhino5Matrix4.Identity,Id(91),new byte[]{3},1,4,1);Check(zeroUnknown.IsValid(),"Copy eligibility/app UUID are not unknown validity requirements");using(var z=new Rhino5UserDataOwner(runtime)){Check(z.AttachUserData(zeroUnknown),"Valid zero-count unknown can attach");using var zcopy=new Rhino5UserDataOwner(runtime);zcopy.CopyUserDataFrom(z);Check(zcopy.Head is null,"Valid zero-count attached data is still omitted by copy");}
        foreach(var candidate in new[]{new Rhino5UnknownUserData(runtime,Guid.Empty,Id(1),1,Rhino5Matrix4.Identity,Id(2),new byte[]{1},1,4,1),new(runtime,Rhino5RuntimeUserData.BaseClassId,Id(1),1,Rhino5Matrix4.Identity,Id(2),new byte[]{1},1,4,1),new(runtime,Id(1),Id(2),1,Rhino5Matrix4.Identity,Rhino5RuntimeUserData.UnknownClassId,new byte[]{1},1,4,1),new(runtime,Id(1),Id(2),1,Rhino5Matrix4.Identity,Guid.Empty,new byte[]{1},1,4,1),new(runtime,Id(1),Id(2),1,Rhino5Matrix4.Identity,Id(3),new byte[]{1},0,4,1)}){Check(!candidate.IsValid(),"Unknown validity branch");candidate.Dispose();}
        using(var ordinary=new Rhino5UserDataOwner(runtime)){var invalidBase=new Probe(runtime,Rhino5RuntimeUserData.BaseClassId);Check(!invalidBase.IsValid()&&ordinary.AttachUserData(invalidBase),"Ordinary attach does not invent general IsValid validation");invalidBase.Dispose();Check(ordinary.Head is null,"Explicit userdata disposal detaches from owner");}
        Rhino5RuntimeUserData? exceptional=null;runtime.RegisterClass(Id(100),r=>exceptional=new Probe(r,Id(101)),(_,_)=>throw new NotSupportedException("Unported codec"));var unreadable=new Rhino5UnknownUserData(runtime,Id(102),Id(103),1,Rhino5Matrix4.Identity,Id(100),new byte[]{1},1,4,1);using(var preserving=new Rhino5UserDataOwner(runtime)){preserving.AttachUserData(unreadable);bool threw=false;try{preserving.GetUserData(Id(102));}catch(NotSupportedException){threw=true;}Check(threw&&exceptional!.IsDisposed&&ReferenceEquals(preserving.Head,unreadable)&&!unreadable.IsDisposed,"Unsupported reader propagates with cleanup and retains original unknown");}
        using(var purging=new Rhino5UserDataOwner(runtime))
        {
            var tail=new Probe(runtime,Id(110));var leading=new Probe(runtime,Id(111));purging.AttachUserData(tail);purging.AttachUserData(leading);bool headObserved=false;
            leading.BeforeDispose=()=>{headObserved=ReferenceEquals(purging.Head,leading)&&leading.Owner is null&&leading.Next is null;runtime.LeakUserDataOnPurge=true;};
            purging.PurgeUserData();Check(headObserved&&leading.IsDisposed&&!tail.IsDisposed&&tail.Owner is null&&tail.Next is null&&purging.Head is null,"Original purge retains head until end and samples leak flag for each node");runtime.LeakUserDataOnPurge=false;tail.Dispose();
        }
        return "PASS original-derived userdata runtime:"+checks+" copy/attach/lookup/lifetime checks; registered reader protocol only, native archive codecs/catalog/plugins/full history/PC parity separate.";
    }
}
