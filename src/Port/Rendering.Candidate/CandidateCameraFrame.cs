// Numerical adaptation of pinned openNURBS; original notices in THIRD_PARTY_NOTICE.md.
// Copyright (c) 1993-2018 Robert McNeel & Associates. This C# adaptation is distinct.
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Immutable raw source camera state, including retained basis and validity flags.</summary>
public readonly record struct CandidateCameraFrameState(
    CandidatePoint3 Location, CandidateVector3 Direction, CandidateVector3 Up,
    CandidateVector3 X, CandidateVector3 Y, CandidateVector3 Z,
    bool IsValidCamera, bool IsValidFrame, bool LocationLocked, bool DirectionLocked, bool UpLocked);

/// <summary>Published source frame/setter semantics; private Rhino5 viewport equivalence is unverified.</summary>
public sealed class CandidateCameraFrame
{
    public const double ZeroTolerance=2.3283064365386962890625e-10;
    private const double MinimumNormal=2.2250738585072014e-308;
    private const double Unset=-1.23432101234321e308;
    public CandidateCameraFrameState Current { get; private set; }
    public long ProjectionInvalidations { get; private set; }
    public int DiagnosticCount { get; private set; }
    public CandidateCameraFrame(CandidateCameraFrameState rawState)=>Current=rawState;

    private static bool Valid(double x)=>double.IsFinite(x)&&x!=Unset&&x!=-Unset;
    private static bool Valid(CandidateVector3 v)=>Valid(v.X)&&Valid(v.Y)&&Valid(v.Z);
    private static bool ValidLocation(CandidatePoint3 p)=>Valid(new CandidateVector3(p.X,p.Y,p.Z))&&Maximum(new(p.X,p.Y,p.Z))<1e100;
    private static bool ValidDirection(CandidateVector3 v)=>Valid(v)&&Maximum(v)<1e100&&Maximum(v)>ZeroTolerance;
    private static double Maximum(CandidateVector3 v)
    {
        double c=Math.Abs(v.X);if(Math.Abs(v.Y)>c)c=Math.Abs(v.Y);if(Math.Abs(v.Z)>c)c=Math.Abs(v.Z);return c;
    }
    private static bool Equal(CandidateVector3 a,CandidateVector3 b)=>a.X==b.X&&a.Y==b.Y&&a.Z==b.Z;
    private static bool IsUnset(CandidateVector3 v)=>v.X==Unset&&v.Y==Unset&&v.Z==Unset;
    private static CandidateVector3 Negate(CandidateVector3 v)=>new(-v.X,-v.Y,-v.Z);
    private static CandidateVector3 Scale(double d,CandidateVector3 v)=>new(d*v.X,d*v.Y,d*v.Z);
    private static CandidateVector3 Add(CandidateVector3 a,CandidateVector3 b)=>new(a.X+b.X,a.Y+b.Y,a.Z+b.Z);
    private static CandidateVector3 Subtract(CandidateVector3 a,CandidateVector3 b)=>new(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
    private static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
    private static CandidateVector3 Cross(CandidateVector3 a,CandidateVector3 b)=>new(a.Y*b.Z-b.Y*a.Z,a.Z*b.X-b.Z*a.X,a.X*b.Y-b.X*a.Y);
    private static double Length(CandidateVector3 v)
    {
        double x=Math.Abs(v.X),y=Math.Abs(v.Y),z=Math.Abs(v.Z),length;
        if(y>=x&&y>=z){length=x;x=y;y=length;}
        else if(z>=x&&z>=y){length=x;x=z;z=length;}
        if(x>MinimumNormal){y/=x;z/=x;length=x*Math.Sqrt(1+y*y+z*z);}
        else if(x>0&&double.IsFinite(x))length=x;
        else length=0;
        return length;
    }
    private static bool Unitize(ref CandidateVector3 v)
    {
        double d=Length(v);
        if(double.IsFinite(d))
        {
            if(d>MinimumNormal){v=new(v.X/d,v.Y/d,v.Z/d);return true;}
            if(d>0)
            {
                var tmp=new CandidateVector3(v.X*8.9884656743115795386465259539451e307,v.Y*8.9884656743115795386465259539451e307,v.Z*8.9884656743115795386465259539451e307);
                d=Length(tmp);
                if(double.IsFinite(d)&&d>MinimumNormal){v=new(tmp.X/d,tmp.Y/d,tmp.Z/d);return true;}
            }
        }
        v=new(0,0,0);return false;
    }
    private static bool Unit(CandidateVector3 v)=>v.X!=Unset&&v.Y!=Unset&&v.Z!=Unset&&Math.Abs(Length(v)-1)<=1e-6;
    private static bool Perpendicular(CandidateVector3 a,CandidateVector3 b)=>Math.Abs(Dot(a,b))<=1e-6;
    private bool Failed(){DiagnosticCount++;return false;}

    public bool TrySetLocation(CandidatePoint3 request)
    {
        var s=Current;
        if(s.LocationLocked&&ValidLocation(s.Location))return Equal(new(request.X,request.Y,request.Z),new(s.Location.X,s.Location.Y,s.Location.Z));
        if(IsUnset(new(request.X,request.Y,request.Z)))
        {Current=s with {Location=request,IsValidCamera=false};ProjectionInvalidations++;}
        else if(ValidLocation(request))
        {Current=s with {Location=request,IsValidCamera=s.IsValidFrame};ProjectionInvalidations++;}
        return Current.IsValidCamera;
    }
    public bool TrySetDirection(CandidateVector3 request)
    {
        var s=Current;
        if(s.DirectionLocked&&ValidDirection(s.Direction))return Equal(request,s.Direction);
        if(IsUnset(request))
        {Current=s with {Direction=request,IsValidCamera=false,IsValidFrame=false};ProjectionInvalidations++;}
        else {Current=s with {Direction=request};TrySetFrame();}
        return Current.IsValidCamera;
    }
    public bool TrySetUp(CandidateVector3 request)
    {
        var s=Current;
        if(s.UpLocked&&ValidDirection(s.Up))return Equal(request,s.Up);
        if(IsUnset(request))
        {Current=s with {Up=request,IsValidCamera=false,IsValidFrame=false};ProjectionInvalidations++;}
        else {Current=s with {Up=request};TrySetFrame();}
        return Current.IsValidCamera;
    }
    public bool TrySetFrame()
    {
        Current=Current with {IsValidCamera=false,IsValidFrame=false};ProjectionInvalidations++;
        var s=Current;
        if(!ValidDirection(s.Direction)||!ValidDirection(s.Up))return Failed();
        CandidateVector3 y,z;
        if(s.UpLocked&&!s.DirectionLocked)
        {
            y=s.Up;if(!Valid(y)||!Unitize(ref y))return Failed();
            double d=Dot(s.Direction,y);z=Add(Negate(s.Direction),Scale(d,y));
            if(!Valid(z))return Failed();if(!Unitize(ref z))return false;
        }
        else
        {
            z=Negate(s.Direction);if(!Valid(z)||!Unitize(ref z))return Failed();
            double d=Dot(s.Up,z);y=Subtract(s.Up,Scale(d,z));
            if(!Valid(y))return Failed();if(!Unitize(ref y))return false;
        }
        var x=Cross(y,z);if(!Valid(x))return Failed();if(!Unitize(ref x))return false;
        if(!Unit(x)||!Unit(y)||!Unit(z)||!Perpendicular(x,y)||!Perpendicular(y,z)||!Perpendicular(z,x))return Failed();
        Current=s with {X=x,Y=y,Z=z,IsValidFrame=true,IsValidCamera=ValidLocation(s.Location)};
        return Current.IsValidCamera;
    }
}
