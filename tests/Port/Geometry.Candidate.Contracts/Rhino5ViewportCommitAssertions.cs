using System.Globalization;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ViewportCommitAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static Rhino5ViewportCommitState Seed()
    {
        var vp=new Rhino5ViewportState(new(new(0,0,80),new(0,0,-80),new(0,1,0),new(1,0,0),new(0,1,0),new(0,0,1),true,false,false,false),new(-1,1,-1,1,1,1000),true,Rhino5Viewport.Perspective,3,new(0,0,0));
        var m=Rhino5Matrix4.Identity.CopyCoefficients();m[0]=11;
        return new(vp,new(7,8,9),Rhino5Matrix4.Identity,new(0,100,200,0,0,65535,true),7,4,new(m),Rhino5Matrix4.Identity,17,19);
    }
    public static string Run()
    {
        checks=0;var raw=Seed();var incoming=raw.Viewport with {Frame=raw.Viewport.Frame with {Location=new(1,2,80)},Target=new(99,98,97)};
        foreach(var guard in new[]{new Rhino5ViewportCommitGuards(true,false),new(false,true)})
        {var c=new Rhino5ViewportCommitCore(raw);Check(!c.TryApplyNumericViewport(incoming,0,guard)&&c.Current==raw,"Guard rejection retains all numeric state");}
        foreach(var invalid in new[]{incoming with {FrustumValid=false},incoming with {Frame=incoming.Frame with {CameraValid=false}}})
        {var c=new Rhino5ViewportCommitCore(raw);Check(!c.TryApplyNumericViewport(invalid,0,default)&&c.Current==raw,"Stored flag rejection retains state");}
        var core=new Rhino5ViewportCommitCore(raw);
        Check(core.TryApplyNumericViewport(incoming,0,default),"Accepted numeric commit");
        Check(core.Current.Viewport.Frame.Location==incoming.Frame.Location&&core.Current.Viewport.Target==raw.Viewport.Target&&core.Current.TargetMirror==raw.TargetMirror,"Only selected incoming state copied; target/mirror untouched in mode0");
        Check(core.Current.ChangeSerial==8&&core.Current.CacheSerial==8&&!ReferenceEquals(core.Current.CachedWorldToClip,raw.CachedWorldToClip),"Serial increments and real matrix rebuilds");
        Check(core.Current.CachedWidth==100&&core.Current.CachedHeight==200,"Screen dimension orientation");
        var expected=Rhino5Matrix4.TryWorldToClip(core.Current.Viewport,raw.ClipModification,out var matrix);Check(expected&&core.Current.CachedWorldToClip.CopyCoefficients().SequenceEqual(matrix.CopyCoefficients()),"Cache uses committed viewport and retained clip modification");
        var locked=raw with {Viewport=raw.Viewport with {Frame=raw.Viewport.Frame with {LocationLocked=true,DirectionLocked=true,UpLocked=true},SymmetryFlags=0x80}};
        var requested=incoming with {Frame=incoming.Frame with {Direction=new(1,1,-1),Up=new(0,0,1),LocationLocked=true,DirectionLocked=false,UpLocked=true},SymmetryFlags=0x43};core=new(locked);
        Check(core.TryApplyNumericViewport(requested,0,default),"Locked old state can commit");
        Check(core.Current.Viewport.Frame.Location==requested.Frame.Location&&core.Current.Viewport.Frame.LocationLocked&&!core.Current.Viewport.Frame.DirectionLocked&&core.Current.Viewport.Frame.UpLocked,"Clear old locks before setters, restore incoming known locks");
        Check(core.Current.Viewport.SymmetryFlags==0x83,"Preserve old unknown symmetry bits, copy known incoming bits");
        Check(Math.Abs(core.Current.Viewport.Frame.Y.Z-1)>1e-3,"Restored up lock must not rebuild basis with different priority");
        var restricted=incoming with {Projection=Rhino5Viewport.Parallel};core=new(raw);
        Check(core.TryApplyNumericViewport(restricted,0,new(false,true)),"Restricted world-aligned parallel allowed");
        foreach(int axis in new[]{0,1,2})
        {var f=restricted.Frame;f=axis==0?f with {X=new(double.NaN,0,0)}:axis==1?f with {Y=new(0,0,1)}:f with {Z=new(0,1,0)};core=new(raw);Check(!core.TryApplyNumericViewport(restricted with {Frame=f},0,new(false,true))&&core.Current==raw,"Restriction checks each incoming basis axis");}
        core=new(raw);Check(core.TryApplyNumericViewport(incoming with {Bounds=new(1,1,-1,1,1,1000),SymmetryFlags=0},0,default)&&core.Current.Viewport.Bounds==raw.Viewport.Bounds&&core.Current.ChangeSerial==8,"Ignored frustum rejection retains bounds but accepts commit");
        core=new(raw);Check(core.TryApplyNumericViewport(incoming with {Bounds=new(-double.MaxValue,double.MaxValue/2,-1,1,1,1000),SymmetryFlags=2},0,default)&&core.Current.Viewport.Bounds==raw.Viewport.Bounds,"Overflow during source symmetrization does not abort commit");
        var badOld=raw with {Viewport=raw.Viewport with {Frame=raw.Viewport.Frame with {Location=new(double.NaN,0,0),CameraValid=false}}};core=new(badOld);
        Check(core.TryApplyNumericViewport(incoming with {Frame=incoming.Frame with {Location=new(double.NaN,0,0)}},0,default),"True incoming camera flag admits raw invalid requests");
        Check(!core.Current.Viewport.Frame.CameraValid&&core.Current.CachedWorldToClip.IsIdentity()&&core.Current.CacheSerial==raw.CacheSerial&&core.Current.ChangeSerial==8,"Failed lookup resets matrix to identity and leaves stale cache serial");
        Check(core.Current.CachedWidth==100&&core.Current.CachedHeight==200,"Screen reads still run after failed matrix lookup");
        core=new(raw with {ChangeSerial=uint.MaxValue,CacheSerial=1});Check(core.TryApplyNumericViewport(incoming,0,default)&&core.Current.ChangeSerial==1&&ReferenceEquals(core.Current.CachedWorldToClip,raw.CachedWorldToClip)&&core.Current.CachedWidth==17&&core.Current.CachedHeight==19,"Serial wrap skips zero; matching cache serial skips matrix and screen refresh");
        core=new(raw with {ScreenPort=raw.ScreenPort with {Valid=false}});Check(core.TryApplyNumericViewport(incoming,0,default)&&core.Current.CachedWidth==0&&core.Current.CachedHeight==0,"Invalid port zeroes dimensions");
        core=new(raw with {ScreenPort=new(int.MinValue,int.MaxValue,int.MaxValue,int.MinValue,0,1,true)});Check(core.TryApplyNumericViewport(incoming,0,default)&&core.Current.CachedWidth==-1&&core.Current.CachedHeight==-1,"Native unchecked screen differences");
        core=new(raw);Check(core.TryApplyNumericViewport(incoming,1,default)&&core.Current.Viewport.Target==new CandidatePoint3(1,2,0)&&core.Current.TargetMirror==core.Current.Viewport.Target,"Nonzero mode preserves old target distance with new camera and synchronizes mirror");
        core=new(raw);Check(core.ReadViewTarget()==raw.Viewport.Target&&core.Current.TargetMirror==raw.Viewport.Target&&core.DiagnosticCount==1,"Getter diagnoses and repairs stale target mirror");
        core.ReadViewTarget();Check(core.DiagnosticCount==1,"Repaired getter does not emit duplicate diagnostic");
        core=new(raw);Check(!core.TrySetViewTarget(new(double.NaN,0,0))&&core.Current.TargetMirror==raw.Viewport.Target&&core.Current.Viewport.Target==raw.Viewport.Target,"Rejected target request still synchronizes mirror from retained core target");
        Check(core.TrySetViewTarget(new(1.23432101234321e308,0,0)),"Legacy positive unset target accepts");
        var unset=new CandidatePoint3(-1.23432101234321e308,-1.23432101234321e308,-1.23432101234321e308);Check(core.TrySetViewTarget(unset)&&core.ReadViewTarget()==unset,"Full negative unset target accepts/synchronizes");
        core=new(raw with {Viewport=raw.Viewport with {Target=new(double.NaN,0,0)},TargetMirror=new(double.NaN,0,0)});core.ReadViewTarget();core.ReadViewTarget();Check(core.DiagnosticCount==2,"Raw NaN compares unequal on every getter despite previous repair");
        core=new(raw with {TargetMirror=new(-0d,0,0)});core.ReadViewTarget();Check(core.DiagnosticCount==0&&BitConverter.DoubleToInt64Bits(core.Current.TargetMirror.X)<0,"Signed zeros compare equal without overwriting mirror bits");
        foreach(var oldTarget in new[]{new CandidatePoint3(0,0,80),new(0,0,150)})
        {
            var before=raw with {Viewport=raw.Viewport with {Target=oldTarget,FrustumValid=false}};core=new(before);
            Check(core.TryApplyNumericViewport(incoming,1,default)&&core.Current.Viewport.Target==oldTarget&&core.Current.TargetMirror==raw.TargetMirror,"Nonpositive old distance skips target branch");
        }
        core=new(raw with {Viewport=raw.Viewport with {Frame=raw.Viewport.Frame with {CameraValid=false}}});
        Check(core.TryApplyNumericViewport(incoming,1,default)&&core.Current.Viewport.Target==raw.Viewport.Target&&core.Current.TargetMirror==raw.TargetMirror,"Invalid old camera target distance skips mode branch even after frame repair");
        core=new(raw);Check(core.TryApplyNumericViewport(incoming with {Frame=incoming.Frame with {Direction=new(double.NaN,0,0)}},0,default)&&core.Current.Viewport.Frame.Direction==raw.Viewport.Frame.Direction,"Ignored invalid direction request retains old requested direction");
        core=new(raw with {ChangeSerial=uint.MaxValue});Check(core.TryApplyNumericViewport(incoming,0,default)&&core.Current.ChangeSerial==1&&core.Current.CacheSerial==1,"Serial wrap with stale cache rebuilds at1");
        foreach(uint oldProjection in new[]{1u,9u})
        {core=new(raw with {Viewport=raw.Viewport with {Projection=oldProjection}});Check(core.TryApplyNumericViewport(incoming with {Projection=9},0,default)&&core.Current.Viewport.Projection==(oldProjection==9?9u:1u),"Unknown projection self-assignment versus normalization");}
        using var stream=typeof(Rhino5ViewportCommitAssertions).Assembly.GetManifestResourceStream("ViewportCacheSourceCases")!;
        using var source=JsonDocument.Parse(stream);
        var row=source.RootElement.GetProperty("cases").EnumerateArray().First(r=>{var a=r.GetProperty("args");return a[0].GetString()=="3"&&a[1].GetString()=="2"&&a[2].GetString()=="0"&&a[4].GetString()=="80"&&a[14].GetString()=="-1"&&a[15].GetString()=="1"&&a[21].GetString()=="0.2"&&a[36].GetString()=="2";});
        var args=row.GetProperty("args").EnumerateArray().Select(x=>double.Parse(x.GetString()!,CultureInfo.InvariantCulture)).ToArray();
        var modifier=new Rhino5Matrix4(args.AsSpan(20,16));var extra=new Rhino5Matrix4(args.AsSpan(36,16));
        core=new(raw with {ClipModification=modifier,ExtraTransform=extra});Check(core.TryApplyNumericViewport(raw.Viewport,0,default),"Modified-cache integration accepts");
        var values=row.GetProperty("values").EnumerateArray().Select(x=>double.Parse(x.GetString()!,CultureInfo.InvariantCulture)).ToArray();var actual=core.Current.CachedWorldToClip.CopyCoefficients();
        for(int i=0;i<16;i++)Check(Math.Abs(actual[i]-values[i])<=2e-13*Math.Max(1,Math.Abs(values[i])),"Integrated retained clip/extra order versus independent compiled source:"+i);
        Check(ReferenceEquals(core.Current.ClipModification,modifier)&&ReferenceEquals(core.Current.ExtraTransform,extra),"Commit retains owned immutable modifier objects");
        return "PASS original-derived Rhino5 numeric viewport commit:"+checks+" state/cache/target checks; required document/history continuation, full native setter/PC/physical parity separate.";
    }
}
