using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5CameraLocationAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static bool Near(double x,double y)=>Math.Abs(x-y)<1e-12;
    private static Rhino5ViewportState Seed(CandidatePoint3 location,CandidateVector3 direction,CandidateVector3 up)
    {
        var f=new Rhino5CameraFrame(new(location,direction,up,default,default,default,true,false,false,false));
        Check(f.TrySetFrame(),"Seed frame");return new(f.Current,new(-1,1,-1,1,1,1000),true,Rhino5Viewport.Perspective,3,new(0,0,0));
    }
    private sealed class RecordingCommit:IRhino5CameraLocationCommit
    {
        public readonly List<string> Calls=new();public bool Accepted;public Rhino5ViewportState State;public CandidatePoint3 Target;
        public bool TryCommit(Rhino5ViewportState state,int mode){Check(mode==0,"Commit mode0");State=state;Calls.Add("commit");return Accepted;}
        public void SetTargetMetadata(CandidatePoint3 target){Target=target;Calls.Add("target");}
    }
    public static string Run()
    {
        checks=0;var top=Seed(new(0,0,80),new(0,0,-80),new(0,1,0));
        foreach(var target in new[]{new CandidatePoint3(double.NaN,0,0),new(0,0,80),new(0,0,80-Rhino5CameraFrame.ZeroTolerance)})
            Check(!Rhino5CameraLocations.TryPrepare(top,target,new(0,0,80),out _),"Invalid/tiny proposal rejected");
        Check(Rhino5CameraLocations.TryPrepare(top,new(0,0,0),new(0,0,80),out var same)&&same!.State.Frame.Y==top.Frame.Y,"Polar up retry");
        Check(Rhino5CameraLocations.TryPrepare(top,new(0,0,0),new(0,-80,0),out var front)&&Near(front!.State.Frame.Y.Z,1),"World Z accepted for horizontal camera");
        var frontSeed=Seed(new(0,-80,0),new(0,80,0),new(0,0,1));
        Check(Rhino5CameraLocations.TryPrepare(frontSeed,new(0,0,0),new(0,0,80),out var fallback)&&Near(fallback!.State.Frame.Y.Y,1),"Third up attempt uses cross-product rotation");
        var rolled=Seed(new(0,-80,0),new(0,80,0),new(1,0,0));
        Check(Rhino5CameraLocations.TryPrepare(rolled,new(0,0,0),new(0,-80,0),out var roll)&&Near(roll!.State.Frame.Y.X,1),"Signed prior roll restored");
        Check(!Rhino5CameraLocations.TryPrepare(top,new(0,0,0),new(double.PositiveInfinity,0,0),out _),"Invalid camera rejected");
        Check(!Rhino5CameraLocations.TryPrepare(top,new(1e308,0,0),new(-1e308,0,0),out _),"Overflowed difference rejected");
        Check(!Rhino5CameraLocations.TryPrepare(top,new(0,0,0),new(0,0,Rhino5CameraFrame.ZeroTolerance),out _),"Exact tiny threshold rejected");
        Check(Rhino5CameraLocations.TryPrepare(top,new(0,0,0),new(0,0,Math.BitIncrement(Rhino5CameraFrame.ZeroTolerance)),out _),"First non-tiny direction accepted");
        Check(!Rhino5CameraLocations.TryPrepare(rolled,new(0,0,0),new(0,0,80),out _),"Roll about old Z can fail final up; do not rotate about new Z instead");
        foreach(double angle in new[]{1e-7,2e-6})
        {
            var slight=Seed(new(0,-80,0),new(0,80,0),new(Math.Sin(angle),0,Math.Cos(angle)));
            Check(Rhino5CameraLocations.TryPrepare(slight,new(0,0,0),new(0,-80,0),out var threshold),"Roll threshold preparation");
            Check(angle<1e-6?threshold!.State.Frame.Up==new CandidateVector3(0,0,1):Math.Abs(threshold!.State.Frame.Up.X-angle)<1e-9,"Roll deadband and application");
        }
        var locked=top with {Frame=top.Frame with {UpLocked=true}};
        Check(Rhino5CameraLocations.TryPrepare(locked,new(0,0,0),new(0,-80,0),out var upLock)&&upLock!.State.Frame.Direction==top.Frame.Direction&&upLock.State.Frame.UpLocked,"Up lock projected direction fallback");
        var locationLock=top with {Frame=top.Frame with {LocationLocked=true}};
        Check(Rhino5CameraLocations.TryPrepare(locationLock,new(0,0,0),new(0,-80,0),out var loc)&&loc!.State.Frame.Location==top.Frame.Location,"Ignored location setter failure");
        var directionLock=top with {Frame=top.Frame with {DirectionLocked=true}};
        Check(Rhino5CameraLocations.TryPrepare(directionLock,new(0,0,0),new(0,-80,0),out var dir)&&dir!.State.Frame.Direction==top.Frame.Direction,"Ignored direction setter failure");
        var broken=top with {Frame=top.Frame with {DirectionLocked=true,Y=default,Up=new(0,0,1)}};
        Check(!Rhino5CameraLocations.TryPrepare(broken,new(0,0,0),new(0,-80,0),out _),"All up attempts fail without partial proposal");
        foreach(bool accepted in new[]{false,true})
        {
            var sink=new RecordingCommit{Accepted=accepted};var outcome=Rhino5CameraLocations.Apply(top,new(1,2,3),new(0,-80,0),sink);
            Check(outcome==(accepted?Rhino5CameraLocationOutcome.CommitAccepted:Rhino5CameraLocationOutcome.CommitRejected),"Commit outcome");
            Check(sink.Calls.SequenceEqual(new[]{"commit","target"})&&sink.Target==new CandidatePoint3(1,2,3)&&sink.State.Target==sink.Target,"Metadata follows accepted or rejected commit");
        }
        var untouched=new RecordingCommit();Check(Rhino5CameraLocations.Apply(top,top.Frame.Location,top.Frame.Location,untouched)==Rhino5CameraLocationOutcome.RejectedBeforeCommit&&untouched.Calls.Count==0,"Rejected preparation has no writes");
        return "PASS original-derived Rhino5 camera locations:"+checks+" static-policy/analytical checks; temporary copy, up/roll/locks and commit-target protocol; concrete native commit/history/cache and original runtime/PC parity separate.";
    }
}
