using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5CameraAssertions
{
    private static int checks;
    private static void Check(bool value,string message){checks++;if(!value)throw new Exception(message);}
    private static Rhino5CameraState Seed(bool valid=true,bool locationLock=false,bool directionLock=false,bool upLock=false)=>new(new(0,0,80),new(0,0,-80),new(0,1,0),new(2,3,4),new(5,6,7),new(8,9,10),valid,locationLock,directionLock,upLock);
    private static bool Near(double x,double y)=>Math.Abs(x-y)<1e-12;
    public static string Run()
    {
        checks=0;var seed=Seed();var camera=new Rhino5CameraFrame(seed);
        Check(!camera.TrySetLocation(new(double.NaN,0,0))&&camera.Current==seed,"Legacy invalid location must return false and retain valid old state");
        Check(camera.TrySetLocation(new(1e200,0,0))&&camera.Current.Location.X==1e200,"Legacy location must not invent published6 world bounds");
        var invalid=new Rhino5CameraFrame(Seed(false));Check(!invalid.TrySetLocation(new(1,2,3))&&!invalid.Current.CameraValid&&invalid.Current.Location==new CandidatePoint3(1,2,3),"Valid location must not repair legacy camera flag");
        Check(invalid.TrySetFrame()&&invalid.Current.CameraValid,"Explicit frame rebuild should repair legacy camera flag");
        var lockedState=Seed(false,true) with {Location=new(1e200,0,0)};var locked=new Rhino5CameraFrame(lockedState);
        Check(locked.TrySetLocation(lockedState.Location)&&locked.Current==lockedState,"Locked equal point returns true even with invalid camera flag");
        Check(!locked.TrySetLocation(new(0,0,80))&&locked.Current==lockedState,"Valid large locked location must retain lock behavior");
        camera=new Rhino5CameraFrame(seed);var unset=new CandidatePoint3(-1.23432101234321e308,-1.23432101234321e308,-1.23432101234321e308);
        Check(!camera.TrySetLocation(unset)&&!camera.Current.CameraValid&&camera.Current.Location==unset,"Full unset point storage/validity differs");
        Check(!camera.TrySetLocation(new(0,0,80))&&!camera.Current.CameraValid,"Location after unset does not repair flag");
        Check(camera.TrySetFrame(),"Frame after location repair should succeed");
        camera=new Rhino5CameraFrame(seed);Check(camera.TrySetLocation(new(1.23432101234321e308,0,0)),"Legacy positive unset coordinate is scalar-valid");
        foreach(var request in new[]{new CandidateVector3(double.NaN,0,0),new(0,0,0),new(0,0,-Rhino5CameraFrame.ZeroTolerance),new(-1.23432101234321e308,-1.23432101234321e308,-1.23432101234321e308)})
        {
            camera=new Rhino5CameraFrame(seed);Check(!camera.TrySetDirection(request)&&camera.Current==seed,"Invalid/tiny direction setter must retain all state");
            Check(!camera.TrySetUp(request)&&camera.Current==seed,"Invalid/tiny up setter must retain all state");
        }
        camera=new Rhino5CameraFrame(seed with {Direction=new(0,0,-5e-324)});Check(camera.TrySetFrame()&&camera.Current.Z==new CandidateVector3(-0d,-0d,1),"Direct legacy frame accepts nonzero microscopic raw direction");
        camera=new Rhino5CameraFrame(seed with {Direction=new(0,0,-1e200)});Check(camera.TrySetFrame(),"Direct legacy frame does not invent upper direction bounds");
        camera=new Rhino5CameraFrame(seed with {Direction=new(0,1,0),Up=new(0,2,0)});Check(!camera.TrySetFrame()&&!camera.Current.CameraValid&&camera.Current.X==seed.X&&camera.Current.Y==seed.Y&&camera.Current.Z==seed.Z,"Failed frame must retain prior basis");
        camera=new Rhino5CameraFrame(seed with {Location=new(double.NaN,0,0)});Check(!camera.TrySetFrame()&&!camera.Current.CameraValid&&camera.Current.X!=seed.X,"Legacy frame can update basis yet return false for invalid location");
        var priority=Seed(true,false,false,true) with {Direction=new(1,1,-1),Up=new(0,0,1)};camera=new Rhino5CameraFrame(priority);
        Check(camera.TrySetFrame()&&camera.Current.Y==new CandidateVector3(0,0,1)&&Near(camera.Current.Z.Z,0),"Up-lock precedence differs");
        camera=new Rhino5CameraFrame(priority with {DirectionLocked=true});Check(camera.TrySetFrame()&&Near(camera.Current.Z.Z,1/Math.Sqrt(3)),"Direction lock must take precedence when both locked");
        int poses=0;foreach(var pose in new[]{("top",new CandidateVector3(0,0,-80)),("front",new CandidateVector3(0,80,0)),("side",new CandidateVector3(-80,0,0)),("perspective",new CandidateVector3(-45,75,-50))})
        {
            var preset=LegacyCameraPreset.Resolve(pose.Item1);camera=new Rhino5CameraFrame(seed with {Location=preset.CameraLocation,Direction=pose.Item2,Up=preset.Up});
            Check(camera.TrySetFrame(),"Original pose frame failed");var f=camera.Current;double length=CandidateCameraFrameLength(preset.CameraLocation);
            Check(Near(f.Z.X,preset.CameraLocation.X/length)&&Near(f.Z.Y,preset.CameraLocation.Y/length)&&Near(f.Z.Z,preset.CameraLocation.Z/length),"Legacy source pose camera axis differs");poses++;
        }
        return "PASS original Rhino5 camera frame:"+checks+" native-derived state/boundary checks and"+poses+" analytical poses; one validity flag, no location repair/world cutoff, reject-with-retention setters and raw tiny frame; static control flow, original runtime/PC parity separate.";
    }
    private static double CandidateCameraFrameLength(CandidatePoint3 p)=>Math.Sqrt(p.X*p.X+p.Y*p.Y+p.Z*p.Z);
}
