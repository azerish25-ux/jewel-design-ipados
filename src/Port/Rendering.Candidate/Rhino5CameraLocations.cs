using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Native SetVP implementation remains a required separate adapter, including its guards and side effects.</summary>
public interface IRhino5CameraLocationCommit
{
    bool TryCommit(Rhino5ViewportState proposed,int mode);
    void SetTargetMetadata(CandidatePoint3 target);
}
public enum Rhino5CameraLocationOutcome {RejectedBeforeCommit,CommitRejected,CommitAccepted}
public sealed record Rhino5CameraLocationProposal(Rhino5ViewportState State,double PriorRoll,int RotationDiagnosticCount);

/// <summary>Original5 static-derived temporary-viewport up/roll recipe. Does not implement native SetVP.</summary>
public static class Rhino5CameraLocations
{
    private static bool Valid(CandidateVector3 v)=>Rhino5ScalarValidity.IsValid(v.X)&&Rhino5ScalarValidity.IsValid(v.Y)&&Rhino5ScalarValidity.IsValid(v.Z);
    private static bool Valid(CandidatePoint3 p)=>Valid(new CandidateVector3(p.X,p.Y,p.Z));
    private static bool Tiny(CandidateVector3 v)=>Math.Abs(v.X)<=Rhino5CameraFrame.ZeroTolerance&&Math.Abs(v.Y)<=Rhino5CameraFrame.ZeroTolerance&&Math.Abs(v.Z)<=Rhino5CameraFrame.ZeroTolerance;
    private static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
    private static CandidateVector3 Negate(CandidateVector3 a)=>new(-a.X,-a.Y,-a.Z);
    private static CandidateVector3 Subtract(CandidateVector3 a,CandidateVector3 b)=>new(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
    private static CandidateVector3 Scale(double n,CandidateVector3 v)=>new(n*v.X,n*v.Y,n*v.Z);
    private static CandidateVector3 Cross(CandidateVector3 a,CandidateVector3 b)=>new(a.Y*b.Z-a.Z*b.Y,a.Z*b.X-a.X*b.Z,a.X*b.Y-a.Y*b.X);
    public static bool TryPrepare(Rhino5ViewportState original,CandidatePoint3 target,CandidatePoint3 camera,out Rhino5CameraLocationProposal? proposal)
    {
        proposal=null;if(!Valid(target)||!Valid(camera))return false;
        var direction=new CandidateVector3(target.X-camera.X,target.Y-camera.Y,target.Z-camera.Z);
        if(!Valid(direction)||Tiny(direction))return false;
        var temporary=new Rhino5Viewport(original);var old=original.Frame;var oldDirection=Negate(old.Z);var oldY=old.Y;
        var up=old.UpLocked?oldY:new CandidateVector3(0,0,1);
        if(old.UpLocked)
        {
            direction=Subtract(direction,Scale(Dot(direction,oldY),oldY));
            if(!Valid(direction)||Tiny(direction))direction=old.Direction;
        }
        double roll=0;int diagnostics=0;
        if(!old.UpLocked)
        {
            var projectedUp=Subtract(up,Scale(Dot(up,old.Z),old.Z));
            if(CandidateCameraFrame.Unitize(ref projectedUp))
            {
                double cosine=Dot(projectedUp,oldY);if(cosine< -1)cosine=-1;else if(cosine>1)cosine=1;
                roll=Math.Acos(cosine);if(Dot(projectedUp,old.X)<0)roll=-roll;
            }
        }
        // The original ignores these returns; locks and false-return mutations still matter.
        temporary.TrySetLocation(camera);temporary.TrySetDirection(direction);
        bool accepted=old.UpLocked;
        if(!accepted)
        {
            accepted=temporary.TrySetUp(up);
            if(!accepted)
            {
                up=oldY;accepted=temporary.TrySetUp(up);
                if(!accepted)
                {
                    var axis=Cross(oldDirection,direction);
                    var rotation=Rhino5VectorRotation.FromSinCos(CandidateCameraFrame.SourceVectorLength(axis),Dot(oldDirection,direction),axis);
                    diagnostics+=rotation.DiagnosticCount;up=rotation.Apply(oldY);accepted=temporary.TrySetUp(up);
                    if(!accepted)return false;
                }
            }
        }
        if(Math.Abs(roll)>1e-6)
        {
            // Rotation center is the requested camera, but vector multiplication has no translation.
            var rotation=Rhino5VectorRotation.FromAngle(roll,Negate(oldDirection));
            diagnostics+=rotation.DiagnosticCount;up=rotation.Apply(up);accepted=temporary.TrySetUp(up);
        }
        if(!accepted)return false;
        temporary.TrySetTarget(target);proposal=new(temporary.Current,roll,diagnostics);return true;
    }
    public static Rhino5CameraLocationOutcome Apply(Rhino5ViewportState original,CandidatePoint3 target,CandidatePoint3 camera,IRhino5CameraLocationCommit commit)
    {
        ArgumentNullException.ThrowIfNull(commit);
        if(!TryPrepare(original,target,camera,out var proposal))return Rhino5CameraLocationOutcome.RejectedBeforeCommit;
        bool accepted=commit.TryCommit(proposal!.State,0);
        // Original void caller ignores the commit BOOL and writes target metadata afterward.
        commit.SetTargetMetadata(target);
        return accepted?Rhino5CameraLocationOutcome.CommitAccepted:Rhino5CameraLocationOutcome.CommitRejected;
    }
}
