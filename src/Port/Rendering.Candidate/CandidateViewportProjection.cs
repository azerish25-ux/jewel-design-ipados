// Numerical adaptation of pinned openNURBS; original notices in THIRD_PARTY_NOTICE.md.
// Copyright (c) 1993-2018 Robert McNeel & Associates. This C# adaptation is distinct.
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Joint raw source state. Unknown projection values and symmetry bits remain explicit.</summary>
public readonly record struct CandidateViewportProjectionState(
    CandidateCameraFrameState Frame, CandidateFrustumBounds Bounds, bool IsValidFrustum,
    uint Projection, byte SymmetryFlags, CandidatePoint3 Target)
{
    public bool LeftRightSymmetric=>(SymmetryFlags&2)!=0;
    public bool TopBottomSymmetric=>(SymmetryFlags&1)!=0;
}

/// <summary>Published parallel/symmetry/target transitions, without private Rhino5 or rendered-preset claims.</summary>
public sealed class CandidateViewportProjection
{
    public const uint Parallel=1,Perspective=2;
    private const double Unset=-1.23432101234321e308;
    public CandidateViewportProjectionState Current { get; private set; }
    public long ProjectionInvalidations { get; private set; }
    public int DiagnosticCount { get; private set; }
    public CandidateViewportProjection(CandidateViewportProjectionState rawState)=>Current=rawState;
    private static bool Valid(double x)=>double.IsFinite(x)&&x!=Unset&&x!=-Unset;
    private static bool Valid(CandidatePoint3 p)=>Valid(p.X)&&Valid(p.Y)&&Valid(p.Z);
    private static bool IsUnset(CandidatePoint3 p)=>p.X==Unset&&p.Y==Unset&&p.Z==Unset;

    private bool UseFrame(Func<CandidateCameraFrame,bool> operation)
    {
        var frame=new CandidateCameraFrame(Current.Frame);bool result=operation(frame);
        Current=Current with {Frame=frame.Current};ProjectionInvalidations+=frame.ProjectionInvalidations;DiagnosticCount+=frame.DiagnosticCount;
        return result;
    }
    private bool UseFrustum(Func<CandidateViewportFrustum,bool> operation)
    {
        var options=new CandidateFrustumOptions(Current.Projection==Perspective,Current.Frame.UpLocked,Current.LeftRightSymmetric,Current.TopBottomSymmetric);
        var frustum=new CandidateViewportFrustum(Current.Bounds,Current.IsValidFrustum,options);bool result=operation(frustum);
        Current=Current with {Bounds=frustum.Bounds,IsValidFrustum=frustum.IsValid};ProjectionInvalidations+=frustum.ProjectionInvalidations;DiagnosticCount+=frustum.DiagnosticCount;
        return result;
    }
    public bool TrySetLocation(CandidatePoint3 p)=>UseFrame(f=>f.TrySetLocation(p));
    public bool TrySetDirection(CandidateVector3 v)=>UseFrame(f=>f.TrySetDirection(v));
    public bool TrySetUp(CandidateVector3 v)=>UseFrame(f=>f.TrySetUp(v));
    public bool TrySetFrame()=>UseFrame(f=>f.TrySetFrame());
    public bool TrySetLens(double lens)=>UseFrustum(f=>f.TrySetLens(lens));
    public bool TrySetBounds(CandidateFrustumBounds bounds)=>UseFrustum(f=>f.TrySetBounds(bounds));
    public void SetCameraLocks(bool location,bool direction,bool up)=>Current=Current with {Frame=Current.Frame with {LocationLocked=location,DirectionLocked=direction,UpLocked=up}};
    public void SetFrustumSymmetry(bool leftRight,bool topBottom)
    {
        byte flags=Current.SymmetryFlags;
        flags=leftRight?(byte)(flags|2):(byte)(flags&0xFD);
        flags=topBottom?(byte)(flags|1):(byte)(flags&0xFE);
        Current=Current with {SymmetryFlags=flags};
    }
    public bool TrySetProjection(uint projection)
    {
        if(projection==Current.Projection)return true;
        Current=Current with {Projection=projection==Perspective?Perspective:Parallel};
        return projection==Perspective||projection==Parallel;
    }
    public bool TrySetTarget(CandidatePoint3 target)
    {
        bool result=Valid(target)||IsUnset(target);
        if(result)Current=Current with {Target=target};
        return result;
    }
    public double TargetDistance(bool useFrustumCenterFallback)
    {
        double distance=Unset;var s=Current;
        if(s.Frame.IsValidCamera)
        {
            if(useFrustumCenterFallback&&!s.IsValidFrustum)useFrustumCenterFallback=false;
            if(Valid(s.Target))
            {
                distance=(s.Frame.Location.X-s.Target.X)*s.Frame.Z.X+(s.Frame.Location.Y-s.Target.Y)*s.Frame.Z.Y+(s.Frame.Location.Z-s.Target.Z)*s.Frame.Z.Z;
                if(useFrustumCenterFallback&&(!Valid(distance)||distance<=0))distance=Unset;
            }
            if(useFrustumCenterFallback&&distance==Unset&&s.Bounds.Far>=s.Bounds.Near)
            {
                distance=.5*(s.Bounds.Near+s.Bounds.Far);
                if(distance<s.Bounds.Near)distance=s.Bounds.Near;else if(distance>s.Bounds.Far)distance=s.Bounds.Far;
                if(distance<=0)distance=Unset;
            }
        }
        return distance;
    }
    public bool TryChangeToSymmetric(bool leftRight,bool topBottom,double targetDistance)
    {
        var b=Current.Bounds;
        if(leftRight&&b.Left==-b.Right)leftRight=false;
        if(topBottom&&b.Bottom==-b.Top)topBottom=false;
        if(!leftRight&&!topBottom)return true;
        if(!Current.IsValidFrustum)return false;
        double halfWidth=.5*(b.Right-b.Left),halfHeight=.5*(b.Top-b.Bottom);
        double dx=leftRight?b.Right-halfWidth:0,dy=topBottom?b.Top-halfHeight:0;
        if(leftRight){b=b with {Right=halfWidth,Left=-halfWidth};ProjectionInvalidations++;}
        if(topBottom){b=b with {Top=halfHeight,Bottom=-halfHeight};ProjectionInvalidations++;}
        Current=Current with {Bounds=b};
        if(Current.Frame.IsValidCamera&&(dx!=0||dy!=0))
        {
            if(Current.Projection==Perspective)
            {
                if(b.Near>0)
                {
                    if(targetDistance==Unset)targetDistance=TargetDistance(true);
                    if(Valid(targetDistance)&&targetDistance>0){double scale=targetDistance/b.Near;dx*=scale;dy*=scale;}
                }
                else {dx=0;dy=0;}
            }
            if(dx!=0||dy!=0)
            {
                var f=Current.Frame;
                var location=new CandidatePoint3(f.Location.X+dx*f.X.X+dy*f.Y.X,f.Location.Y+dx*f.X.Y+dy*f.Y.Y,f.Location.Z+dx*f.X.Z+dy*f.Y.Z);
                TrySetLocation(location);
            }
        }
        return true;
    }
    private void UpdateTarget(double targetDistance)
    {
        var s=Current;
        if(!s.Frame.IsValidCamera||!s.IsValidFrustum||!Valid(targetDistance)||targetDistance<=0)return;
        double tolerance=1e-5*((s.Bounds.Right-s.Bounds.Left)+(s.Bounds.Top-s.Bounds.Bottom))+CandidateCameraFrame.ZeroTolerance;
        var target=new CandidatePoint3(s.Frame.Location.X-targetDistance*s.Frame.Z.X,s.Frame.Location.Y-targetDistance*s.Frame.Z.Y,s.Frame.Location.Z-targetDistance*s.Frame.Z.Z);
        if(!Valid(target))return;
        if(!Valid(s.Target)||CandidateCameraFrame.SourceVectorLength(new(s.Target.X-target.X,s.Target.Y-target.Y,s.Target.Z-target.Z))>tolerance)TrySetTarget(target);
    }
    public bool TryChangeToParallel(bool symmetricFrustum)
    {
        bool result=Current.Frame.IsValidCamera&&Current.IsValidFrustum;
        SetCameraLocks(Current.Frame.LocationLocked,false,false);
        if(Current.Projection==Parallel&&symmetricFrustum==Current.LeftRightSymmetric&&symmetricFrustum==Current.TopBottomSymmetric)return result;
        // Original ordering ignores symmetry conversion failure and still applies flags/projection.
        TryChangeToSymmetric(symmetricFrustum,symmetricFrustum,Unset);
        SetFrustumSymmetry(symmetricFrustum,symmetricFrustum);
        uint oldProjection=Current.Projection;double targetDistance=TargetDistance(true);var b=Current.Bounds;
        if(!Valid(targetDistance)||!Current.IsValidFrustum||!Valid(b.Near)||b.Near<=0||targetDistance<=b.Near)targetDistance=0;
        if(oldProjection!=Parallel&&!TrySetProjection(Parallel))result=false;
        if(result)
        {
            if(oldProjection==Perspective&&targetDistance>0&&b.Near>0&&b.Near<b.Far)
            {
                double scale=targetDistance/b.Near;
                if(!TrySetBounds(new(b.Left*scale,b.Right*scale,b.Bottom*scale,b.Top*scale,b.Near,b.Far)))result=false;
            }
            if(Valid(Current.Target))UpdateTarget(targetDistance);
        }
        return result;
    }
}
