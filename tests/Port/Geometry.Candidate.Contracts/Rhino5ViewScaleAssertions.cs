using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;

public static class Rhino5ViewScaleAssertions
{
    private delegate bool Measure(Rhino5RuntimeView view,int left,int top,int right,int bottom,ref double scale);
    public static string Run()
    {
        var method=typeof(Rhino5RuntimeView).GetMethod("TryGetSynchronizationScale",new[]{typeof(int),typeof(int),typeof(int),typeof(int),typeof(double).MakeByRefType()});
        if(method is null)throw new Exception("Missing actual runtime-view synchronization scale and target-mirror ownership.");
        var measure=method.CreateDelegate<Measure>();
        int checks=0;void Check(bool value,string why){checks++;if(!value)throw new Exception(why);}
        var runtime=new Rhino5UserDataRuntime();
        var frame=new Rhino5CameraState(new(0,0,10),new(0,0,-1),new(0,1,0),new(1,0,0),new(0,1,0),new(0,0,1),true,false,false,false);
        var viewport=new Rhino5ViewportState(frame,new(-3,7,-5,11,2,100),true,Rhino5Viewport.Parallel,0,new(0,0,0));
        var details=new Rhino5ViewportDetails(Guid.NewGuid(),new(0,17,9,0,0,100,true),Rhino5Matrix4.Identity,Rhino5Matrix4.Identity,.01,.001);
        var plane=new Rhino5ViewPlane(new(0,0,0),new(1,0,0),new(0,1,0),new(0,0,1),new(0,0,1,0));
        var metadata=new Rhino5ViewMetadata(false,"scale",Guid.Empty,0,new(0,1,0,1,0,0,0,0,0),0,new(0,0,0,0,0,0,0,""),new(plane,1,1,10,5,false,"grid"),true,true,true,new(plane,1,1,"trace",false,false,false),new("wall",false,false));
        var clip=new Rhino5ClippingPlaneInfo(new(1,0,0,2),Guid.NewGuid(),true);
        using var view=new Rhino5RuntimeView(runtime,viewport,new(99,98,97),details,metadata,new[]{clip});
        var strings=new Rhino5UserStringList(runtime);strings.ReplacePairs(new[]{new KeyValuePair<string,string>("name","kept")});view.AttachUserData(strings);
        void Set(Rhino5ViewportState state,CandidatePoint3 mirror)=>view.SetConvertedValues(state,mirror,details,metadata,new[]{clip});
        void Value(double expected,string why){double value=-91;Check(measure(view,10,20,810,620,ref value),why+" accepted");Check(value==expected,why+" value");}
        Value(.005,"Parallel uses nearest lateral bound, client size and no factor two");
        Check(view.TargetMirror==new CandidatePoint3(99,98,97)&&runtime.DiagnosticCount==0,"Parallel does not read or repair mirror");
        foreach(var bounds in new[]{new CandidateFrustumBounds(-1,7,-5,11,2,100),new(-3,1,-5,11,2,100),new(-3,7,-1,11,2,100),new(-3,7,-5,1,2,100)}){Set(viewport with {Bounds=bounds},new(99,98,97));Value(1d/600,"Each lateral bound can independently be minimum");}
        Set(viewport with {Bounds=viewport.Bounds with {Near=0}},new(99,98,97));Value(.005,"Parallel ignores near");
        Set(viewport with {Projection=Rhino5Viewport.Perspective},new(0,0,100));Value(.025,"Perspective uses repaired target rather than stale mirror");
        Check(view.TargetMirror==viewport.Target&&runtime.DiagnosticCount==1,"Perspective repairs actual mirror exactly once");
        Value(.025,"Repeated perspective measurement");Check(runtime.DiagnosticCount==1,"Matching target avoids new diagnostic");
        Set(viewport with {Projection=Rhino5Viewport.Perspective,Frame=frame with {Location=new(3,4,12)}},viewport.Target);Value(.0325,"Off-axis distance is Euclidean, not projected camera depth");
        Set(viewport with {Projection=Rhino5Viewport.Perspective,Frame=frame with {Location=viewport.Target}},viewport.Target);Value(0,"Zero distance succeeds with zero scale");
        Set(viewport with {Bounds=viewport.Bounds with {Left=0}},new(99,98,97));Value(0,"Zero bound is returned, not rejected");
        Set(viewport with {Projection=99},new(99,98,97));Value(.005,"Only perspective enum uses distance");
        Set(viewport with {Bounds=viewport.Bounds with {Top=double.NaN}},new(99,98,97));Value(.005,"Later NaN bound is ignored by ordered comparisons");
        Set(viewport with {Bounds=viewport.Bounds with {Bottom=double.NaN}},new(99,98,97));double nanScale=7;Check(measure(view,0,0,600,800,ref nanScale)&&double.IsNaN(nanScale),"First NaN bound remains NaN and native success is preserved");
        Set(viewport with {Projection=Rhino5Viewport.Perspective,Frame=frame with {Location=new(1e200,0,0)},Bounds=new(-1e-200,2e-200,-3e-200,4e-200,1,100)},viewport.Target);double large=0;Check(measure(view,0,0,1,1,ref large)&&Math.Abs(large-1)<1e-15,"Robust distance and source multiplication order avoid overflow");
        Set(viewport with {Projection=Rhino5Viewport.Perspective,Frame=frame with {Location=new(double.Epsilon,0,0)},Bounds=new(-1,1,-1,1,1,100)},viewport.Target);double tiny=0;Check(measure(view,0,0,1,1,ref tiny)&&tiny==double.Epsilon,"Subnormal distance follows source max-component branch");
        Set(viewport with {Projection=Rhino5Viewport.Perspective,Frame=frame with {CameraValid=false}},new(0,0,100));Value(.025,"Source reads stored camera independently of validity flag");
        Set(viewport with {FrustumValid=false,Projection=Rhino5Viewport.Perspective},new(0,0,100));int errors=runtime.DiagnosticCount;
        foreach(var rect in new[]{(0,0,0,600),(0,0,800,0),(0,0,-1,600),(0,0,800,-1),(int.MaxValue,int.MinValue,int.MinValue,int.MaxValue)}){double output=-0d;Check(!measure(view,rect.Item1,rect.Item2,rect.Item3,rect.Item4,ref output)&&BitConverter.DoubleToInt64Bits(output)==long.MinValue,"Nonpositive signed client dimensions preserve output before frustum/mirror reads");}
        Check(runtime.DiagnosticCount==errors&&view.TargetMirror==new CandidatePoint3(0,0,100),"Rejected clients leave mirror untouched");
        double unsupported=123;bool rejected=false;try{measure(view,0,0,1,1,ref unsupported);}catch(NotSupportedException){rejected=true;}Check(rejected&&unsupported==123&&runtime.DiagnosticCount==errors,"Uninitialized native GetFrustum failure is explicit unsupported state");
        Set(viewport,new(99,98,97));double wrapped=0;Check(measure(view,int.MaxValue,int.MaxValue,int.MinValue,int.MinValue,ref wrapped)&&wrapped==3,"Win32 signed rectangle subtraction wraps to positive one");
        Check(ReferenceEquals(view.Metadata,metadata)&&view.Details==details&&view.GetClippingPlane(0)==clip&&ReferenceEquals(view.Head,strings)&&strings.CopyPairs()[0].Value=="kept","Measurement preserves actual metadata, screen port, clipping and owned userdata");
        view.Dispose();double disposedOutput=77;bool disposed=false;try{measure(view,0,0,1,1,ref disposedOutput);}catch(ObjectDisposedException){disposed=true;}Check(disposed&&disposedOutput==77,"Disposed managed owner rejects use before mutation");
        return $"PASS runtime-view synchronization scale: {checks} source arithmetic/rectangle/mirror/ownership/boundary checks; full registered-view/document/platform/PC parity separate.";
    }
}
