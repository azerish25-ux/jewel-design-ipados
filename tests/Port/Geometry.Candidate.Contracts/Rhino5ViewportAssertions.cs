using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ViewportAssertions
{
    private static int checks;
    private static void Check(bool value,string message){checks++;if(!value)throw new Exception(message);}
    private static bool Near(double a,double b)=>Math.Abs(a-b)<=1e-11*Math.Max(1,Math.Max(Math.Abs(a),Math.Abs(b)));
    private static Rhino5ViewportState Seed(uint projection=2,byte symmetry=0)=>new(new(new(0,0,80),new(0,0,-80),new(0,1,0),new(1,0,0),new(0,1,0),new(0,0,1),true,false,false,false),new(-1,1,-1,1,1,100),true,projection,symmetry,new(0,0,0));
    public static string Run()
    {
        checks=0;var v=new Rhino5Viewport(Seed());
        Check(v.TrySetBounds(new(-1e101,1e101,-1,1,1,1e102)),"Original frustum has no published6 world cutoffs");
        Check(v.Current.Bounds.Far==1e102&&v.Current.FrustumValid,"Large native bounds lost");
        v=new(Seed());Check(v.TrySetBounds(new(-1,1,-1,1,1,1.23432101234321e308)),"Positive unset far is valid in original5");
        Check(v.DiagnosticCount==1,"Original float-depth warning must accept bounds");
        v=new(Seed(symmetry:3));Check(v.TrySetBounds(new(-1.2e308,1e308,-1,1,1,100))&&double.IsPositiveInfinity(v.Current.Bounds.Right),"Original symmetry can overflow after validation; no invented post-validation repair");
        v=new(Seed());Check(v.TrySetLens(1.23432101234321e308)&&v.Current.Bounds.Right>0&&v.Current.Bounds.Right<1e-306,"Original lens must accept positive unset finite value");
        v=new(Seed() with {Bounds=new(-1,1,-1,1,double.NaN,100)});Check(v.TryGetLens(out var rawLens)&&double.IsNaN(rawLens),"Native unordered near comparison must preserve raw NaN getter result");
        var exact=Seed(symmetry:3) with {Bounds=new(-.24,.24,-.24,.24,1,100),Frame=Seed().Frame with {UpLocked=true,DirectionLocked=true}};
        v=new(exact);Check(v.TryChangeToPerspective(-1.23432101234321e308,true,50.125),"Already-perspective conversion failed");
        Check(v.Current.Bounds==exact.Bounds&&!v.Current.Frame.UpLocked&&!v.Current.Frame.DirectionLocked,"Exact0.125 lens threshold or lock clearing changed");
        Check(v.TryChangeToPerspective(0,true,50.126)&&v.Current.Bounds.Right!=exact.Bounds.Right,"Strictly over0.125 lens change not applied");
        v=new(exact);Check(v.TryChangeToPerspective(0,true,double.NaN)&&v.Current.Bounds==exact.Bounds,"Invalid lens on early path must retain initial success/state");
        var wide=Seed(1) with {Bounds=new(-500,500,-500,500,1,100)};v=new(wide);
        Check(v.TryChangeToPerspective(0,true,50)&&v.Current.Projection==2,"Parallel-to-perspective failed");
        Check(Near(v.Current.Frame.Location.Z,1000*50d/24),"Camera did not move backward to preserve width plane");
        Check(Near(v.Current.Target.Z,v.Current.Frame.Location.Z-80),"Original target-distance recenter order differs");
        Check(v.Current.SymmetryFlags==3,"Requested symmetry flags lost");
        v=new(wide with {Frame=wide.Frame with {LocationLocked=true}});Check(v.TryChangeToPerspective(0,true,50)&&v.Current.Frame.Location.Z==80&&v.Current.Frame.LocationLocked,"Location lock must suppress width-plane dolly");
        v=new(Seed(1) with {Bounds=new(-1,1,-1,1,1e-10,1e-5)});Check(v.TryChangeToPerspective(80,true,50)&&v.Current.Bounds.Near==1e-8,"Original near correction threshold differs");
        var invalid=Seed(1) with {FrustumValid=false,Frame=Seed().Frame with {UpLocked=true,DirectionLocked=true}};v=new(invalid);
        Check(!v.TryChangeToPerspective(80,true,50)&&v.Current.Projection==2&&v.Current.SymmetryFlags==3&&!v.Current.Frame.UpLocked&&!v.Current.Frame.DirectionLocked,"Failed conversion must retain original projection/flag mutations");
        v=new(Seed(1));Check(!v.TryChangeToPerspective(80,true,-1)&&v.Current.Projection==2,"Invalid lens must retain helper projection mutation");
        v=new(Seed());Check(v.TrySetTarget(new(1.23432101234321e308,0,0)),"Original target positive unset validity differs");
        var target=v.Current.Target;Check(!v.TrySetTarget(new(double.NaN,0,0))&&v.Current.Target==target,"Invalid target retention differs");
        Check(v.TrySetTarget(new(-1.23432101234321e308,-1.23432101234321e308,-1.23432101234321e308)),"Original full unset target must remain accepted");
        v=new(Seed(1,symmetry:0x83));Check(v.TryChangeToParallel(true)&&v.Current.SymmetryFlags==0x83,"Already-parallel unknown symmetry bits lost");
        v=new(Seed(symmetry:0x80));Check(v.TryChangeToParallel(true)&&v.Current.SymmetryFlags==0x83&&Near(v.Current.Bounds.Right,80),"Original parallel target-plane scale differs");
        Check(v.TrySetProjection(7)==false&&v.Current.Projection==1,"Invalid projection fallback differs");
        v=new(Seed(7));Check(v.TrySetProjection(7)&&v.Current.Projection==7,"Original unknown self-assignment must stay unchanged");
        return "PASS original Rhino5 viewport:"+checks+" native-derived perspective/frustum/lens/target checks; legacy limits/sentinels, strict lens threshold, width dolly/locks/near correction and failure mutation; static control flow, original runtime/preset/PC parity separate.";
    }
}
