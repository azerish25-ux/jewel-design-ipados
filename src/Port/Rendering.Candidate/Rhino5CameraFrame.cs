using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Original Rhino5 camera closure: one validity flag, requested vectors, basis and locks.</summary>
public readonly record struct Rhino5CameraState(CandidatePoint3 Location,CandidateVector3 Direction,CandidateVector3 Up,
    CandidateVector3 X,CandidateVector3 Y,CandidateVector3 Z,bool CameraValid,bool LocationLocked,bool DirectionLocked,bool UpLocked);

/// <summary>Static-original5-derived frame/setter policy. Original runtime/PC comparisons remain separate.</summary>
public sealed class Rhino5CameraFrame
{
    public const double ZeroTolerance=2.3283064365386962890625e-10;
    private const double Unset=-1.23432101234321e308;
    public Rhino5CameraState Current {get;private set;}
    public Rhino5CameraFrame(Rhino5CameraState rawState)=>Current=rawState;
    private static bool Valid(CandidateVector3 v)=>Rhino5ScalarValidity.IsValid(v.X)&&Rhino5ScalarValidity.IsValid(v.Y)&&Rhino5ScalarValidity.IsValid(v.Z);
    private static bool Valid(CandidatePoint3 p)=>Valid(new CandidateVector3(p.X,p.Y,p.Z));
    private static bool Tiny(CandidateVector3 v)=>Math.Abs(v.X)<=ZeroTolerance&&Math.Abs(v.Y)<=ZeroTolerance&&Math.Abs(v.Z)<=ZeroTolerance;
    private static bool Equal(CandidateVector3 a,CandidateVector3 b)=>a.X==b.X&&a.Y==b.Y&&a.Z==b.Z;
    private static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
    private static CandidateVector3 Negate(CandidateVector3 v)=>new(-v.X,-v.Y,-v.Z);
    private static CandidateVector3 Scale(double d,CandidateVector3 v)=>new(d*v.X,d*v.Y,d*v.Z);
    private static CandidateVector3 Add(CandidateVector3 a,CandidateVector3 b)=>new(a.X+b.X,a.Y+b.Y,a.Z+b.Z);
    private static CandidateVector3 Subtract(CandidateVector3 a,CandidateVector3 b)=>new(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
    private static CandidateVector3 Cross(CandidateVector3 a,CandidateVector3 b)=>new(a.Y*b.Z-a.Z*b.Y,a.Z*b.X-a.X*b.Z,a.X*b.Y-a.Y*b.X);
    private static bool Unit(CandidateVector3 v)=>v.X!=Unset&&v.Y!=Unset&&v.Z!=Unset&&Math.Abs(CandidateCameraFrame.SourceVectorLength(v)-1)<=1e-6;
    public void SetLocks(bool location,bool direction,bool up)=>Current=Current with {LocationLocked=location,DirectionLocked=direction,UpLocked=up};
    public bool TrySetLocation(CandidatePoint3 request)
    {
        var s=Current;
        if(s.LocationLocked&&Valid(s.Location))return Equal(new(request.X,request.Y,request.Z),new(s.Location.X,s.Location.Y,s.Location.Z));
        bool fullUnset=request.X==Unset&&request.Y==Unset&&request.Z==Unset;
        if(!fullUnset&&!Valid(request))return false;
        Current=s with {Location=request};
        if(!Valid(request))Current=Current with {CameraValid=false};
        return Current.CameraValid;
    }
    public bool TrySetDirection(CandidateVector3 request)
    {
        var s=Current;
        if(s.DirectionLocked&&Valid(s.Direction)&&!Tiny(s.Direction))return Equal(request,s.Direction);
        if(!Valid(request)||Tiny(request))return false;
        Current=s with {Direction=request};return TrySetFrame();
    }
    public bool TrySetUp(CandidateVector3 request)
    {
        var s=Current;
        if(s.UpLocked&&Valid(s.Up)&&!Tiny(s.Up))return Equal(request,s.Up);
        if(!Valid(request)||Tiny(request))return false;
        Current=s with {Up=request};return TrySetFrame();
    }
    public bool TrySetFrame()
    {
        Current=Current with {CameraValid=false};var s=Current;
        if(!Valid(s.Direction)||!Valid(s.Up))return false;
        CandidateVector3 y,z;
        if(s.UpLocked&&!s.DirectionLocked)
        {
            y=s.Up;if(!Valid(y)||!CandidateCameraFrame.Unitize(ref y))return false;
            z=Add(Negate(s.Direction),Scale(Dot(s.Direction,y),y));
            if(!Valid(z)||!CandidateCameraFrame.Unitize(ref z))return false;
        }
        else
        {
            z=Negate(s.Direction);if(!Valid(z)||!CandidateCameraFrame.Unitize(ref z))return false;
            y=Subtract(s.Up,Scale(Dot(s.Up,z),z));
            if(!Valid(y)||!CandidateCameraFrame.Unitize(ref y))return false;
        }
        var x=Cross(y,z);if(!Valid(x)||!CandidateCameraFrame.Unitize(ref x))return false;
        if(!Unit(x)||!Unit(y)||!Unit(z)||Math.Abs(Dot(x,y))>1e-6||Math.Abs(Dot(y,z))>1e-6||Math.Abs(Dot(z,x))>1e-6)return false;
        Current=s with {X=x,Y=y,Z=z,CameraValid=Valid(s.Location)};return Current.CameraValid;
    }
}
