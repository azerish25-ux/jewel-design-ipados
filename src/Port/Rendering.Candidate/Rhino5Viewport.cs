using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

public readonly record struct Rhino5ViewportState(Rhino5CameraState Frame,CandidateFrustumBounds Bounds,bool FrustumValid,uint Projection,byte SymmetryFlags,CandidatePoint3 Target)
{
    public bool LeftRightSymmetric=>(SymmetryFlags&2)!=0;
    public bool TopBottomSymmetric=>(SymmetryFlags&1)!=0;
}

/// <summary>Original5-derived numerical viewport closure; native UI commit/history are separate adapters.</summary>
public sealed class Rhino5Viewport
{
    public const uint Parallel=1,Perspective=2;
    private const double Unset=-1.23432101234321e308;
    public Rhino5ViewportState Current {get;private set;}
    public int DiagnosticCount {get;private set;}
    public Rhino5Viewport(Rhino5ViewportState rawState)=>Current=rawState;
    private static bool Valid(double value)=>Rhino5ScalarValidity.IsValid(value);
    private static bool Valid(CandidatePoint3 p)=>Valid(p.X)&&Valid(p.Y)&&Valid(p.Z);
    private bool UseFrame(Func<Rhino5CameraFrame,bool> operation){var f=new Rhino5CameraFrame(Current.Frame);bool result=operation(f);Current=Current with {Frame=f.Current};return result;}
    public bool TrySetLocation(CandidatePoint3 p)=>UseFrame(f=>f.TrySetLocation(p));
    public bool TrySetDirection(CandidateVector3 v)=>UseFrame(f=>f.TrySetDirection(v));
    public bool TrySetUp(CandidateVector3 v)=>UseFrame(f=>f.TrySetUp(v));
    public bool TrySetFrame()=>UseFrame(f=>f.TrySetFrame());
    public void SetCameraLocks(bool location,bool direction,bool up)=>Current=Current with {Frame=Current.Frame with {LocationLocked=location,DirectionLocked=direction,UpLocked=up}};
    public void SetFrustumSymmetry(bool lr,bool tb)
    {
        byte flags=Current.SymmetryFlags;flags=lr?(byte)(flags|2):(byte)(flags&0xFD);flags=tb?(byte)(flags|1):(byte)(flags&0xFE);Current=Current with {SymmetryFlags=flags};
    }
    public bool TrySetProjection(uint projection)
    {
        if(projection==Current.Projection)return true;
        Current=Current with {Projection=projection==Perspective?Perspective:Parallel};return projection==Perspective||projection==Parallel;
    }
    public bool TrySetTarget(CandidatePoint3 target)
    {
        if(!Valid(target)&&!(target.X==Unset&&target.Y==Unset&&target.Z==Unset))return false;
        Current=Current with {Target=target};return true;
    }
    public bool TrySetBounds(CandidateFrustumBounds request)
    {
        var (l,r,b,t,n,f)=request;
        if(!(Valid(l)&&Valid(r)&&Valid(t)&&Valid(b)&&Valid(n)&&Valid(f)&&l<r&&b<t&&0<n&&n<f)){DiagnosticCount++;return false;}
        if(Current.Projection==Perspective&&(n<=1e-8||f>1.0001e8*n))DiagnosticCount++;
        if(Current.LeftRightSymmetric&&l!=-r){r=.5*(r-l);l=-r;}
        if(Current.TopBottomSymmetric&&b!=-t){t=.5*(t-b);b=-t;}
        Current=Current with {Bounds=new(l,r,b,t,n,f),FrustumValid=true};return true;
    }
    private bool TwoPoint=>Current.Projection==Perspective&&Current.Frame.UpLocked&&Current.LeftRightSymmetric&&!Current.TopBottomSymmetric;
    public bool TryGetLens(out double lens)
    {
        lens=0;var b=Current.Bounds;if(!Current.FrustumValid||b.Near<=0)return false;
        double w=b.Right>-b.Left?b.Right:-b.Left,h=b.Top>-b.Bottom?b.Top:-b.Bottom;
        double radius=w<=h||TwoPoint?w:h;if(radius<=0)return false;
        lens=b.Near*12/radius;return true;
    }
    public bool TrySetLens(double lens)
    {
        var b=Current.Bounds;if(!Valid(lens)||lens<=0||!Current.FrustumValid||b.Near<=0)return false;
        double w=b.Right>-b.Left?b.Right:-b.Left,h=b.Top>-b.Bottom?b.Top:-b.Bottom;
        double radius=w<=h||TwoPoint?w:h;if(radius<=0)return false;
        double scale=(b.Near/lens)*(12/radius);if(Math.Abs(scale-1)<1e-6)return true;
        return TrySetBounds(new(b.Left*scale,b.Right*scale,b.Bottom*scale,b.Top*scale,b.Near,b.Far));
    }
    public double TargetDistance(bool fallback)
    {
        var s=Current;if(!s.Frame.CameraValid)return Unset;
        if(fallback&&!s.FrustumValid)fallback=false;
        double distance=Unset;
        if(Valid(s.Target))
        {
            distance=(s.Frame.Location.X-s.Target.X)*s.Frame.Z.X+(s.Frame.Location.Y-s.Target.Y)*s.Frame.Z.Y+(s.Frame.Location.Z-s.Target.Z)*s.Frame.Z.Z;
            if(!fallback)return distance;
            if(!Valid(distance)||distance<=0)distance=Unset;
        }
        if(fallback&&distance==Unset&&s.Bounds.Near<=s.Bounds.Far)
        {
            double midpoint=.5*(s.Bounds.Near+s.Bounds.Far),chosen=s.Bounds.Near;
            if(s.Bounds.Near<=midpoint){chosen=midpoint;if(s.Bounds.Far<midpoint)chosen=s.Bounds.Far;}
            return chosen>0?chosen:Unset;
        }
        return distance;
    }
    public bool TryChangeToSymmetric(bool lr,bool tb,double targetDistance)
    {
        var b=Current.Bounds;if(lr&&b.Left==-b.Right)lr=false;if(tb&&b.Bottom==-b.Top)tb=false;
        if(!lr&&!tb)return true;if(!Current.FrustumValid)return false;
        double halfWidth=.5*(b.Right-b.Left),halfHeight=.5*(b.Top-b.Bottom),dx=lr?b.Right-halfWidth:0,dy=tb?b.Top-halfHeight:0;
        if(lr)b=b with {Left=-halfWidth,Right=halfWidth};if(tb)b=b with {Bottom=-halfHeight,Top=halfHeight};Current=Current with {Bounds=b};
        if(Current.Frame.CameraValid&&(dx!=0||dy!=0))
        {
            if(Current.Projection==Perspective)
            {
                if(b.Near>0){if(targetDistance==Unset)targetDistance=TargetDistance(true);if(Valid(targetDistance)&&targetDistance>0){double scale=targetDistance/b.Near;dx*=scale;dy*=scale;}}
                else {dx=0;dy=0;}
            }
            if(dx!=0||dy!=0){var f=Current.Frame;TrySetLocation(new(f.Location.X+dx*f.X.X+dy*f.Y.X,f.Location.Y+dx*f.X.Y+dy*f.Y.Y,f.Location.Z+dx*f.X.Z+dy*f.Y.Z));}
        }
        return true;
    }
    private void UpdateTarget(double distance)
    {
        var s=Current;if(!s.Frame.CameraValid||!s.FrustumValid||!Valid(distance)||distance<=0)return;
        var target=new CandidatePoint3(s.Frame.Location.X-distance*s.Frame.Z.X,s.Frame.Location.Y-distance*s.Frame.Z.Y,s.Frame.Location.Z-distance*s.Frame.Z.Z);
        if(!Valid(target))return;
        double tolerance=1e-5*((s.Bounds.Top-s.Bounds.Bottom)+(s.Bounds.Right-s.Bounds.Left))+Rhino5CameraFrame.ZeroTolerance;
        if(!Valid(s.Target)||CandidateCameraFrame.SourceVectorLength(new(s.Target.X-target.X,s.Target.Y-target.Y,s.Target.Z-target.Z))>tolerance)TrySetTarget(target);
    }
    public bool TryChangeToParallel(bool symmetric)
    {
        bool result=Current.Frame.CameraValid&&Current.FrustumValid;SetCameraLocks(Current.Frame.LocationLocked,false,false);
        if(Current.Projection==Parallel&&symmetric==Current.LeftRightSymmetric&&symmetric==Current.TopBottomSymmetric)return result;
        TryChangeToSymmetric(symmetric,symmetric,Unset);SetFrustumSymmetry(symmetric,symmetric);
        uint oldProjection=Current.Projection;double distance=TargetDistance(true);var b=Current.Bounds;
        if(!Valid(distance)||!Current.FrustumValid||!Valid(b.Near)||b.Near<=0||distance<=b.Near)distance=0;
        if(oldProjection!=Parallel&&!TrySetProjection(Parallel))result=false;
        if(result)
        {
            if(oldProjection==Perspective&&distance>0&&0<b.Near&&b.Near<b.Far){double scale=distance/b.Near;if(!TrySetBounds(new(b.Left*scale,b.Right*scale,b.Bottom*scale,b.Top*scale,b.Near,b.Far)))result=false;}
            if(Valid(Current.Target))UpdateTarget(distance);
        }
        return result;
    }
    private bool ChangeFromParallel(double distance,double lens)
    {
        if(Current.Projection==Perspective)return true;
        TrySetProjection(Perspective);if(!Current.FrustumValid)return false;
        var b=Current.Bounds;double width=Math.Abs(b.Right-b.Left);var f=Current.Frame;
        CandidatePoint3 widthPoint=Valid(distance)&&distance>0?new(f.Location.X-distance*f.Z.X,f.Location.Y-distance*f.Z.Y,f.Location.Z-distance*f.Z.Z):new(Unset,Unset,Unset);
        if(b.Near<1e-8&&b.Far>=1e-7)TrySetBounds(new(b.Left,b.Right,b.Bottom,b.Top,1e-8,b.Far));
        bool result=false;
        if(Valid(lens)&&lens>0)
        {
            result=TrySetLens(lens);
            if(result&&Valid(widthPoint)&&!Current.Frame.LocationLocked&&Current.FrustumValid&&Current.Bounds.Near>0)
            {
                f=Current.Frame;b=Current.Bounds;
                double d=(f.Location.X-widthPoint.X)*f.Z.X+(f.Location.Y-widthPoint.Y)*f.Z.Y+(f.Location.Z-widthPoint.Z)*f.Z.Z;
                if(d>b.Near)
                {
                    double w=Math.Abs(b.Right-b.Left)*d/b.Near;
                    if(width>w&&w>0){double dz=d*(width/w-1);TrySetLocation(new(f.Location.X+dz*f.Z.X,f.Location.Y+dz*f.Z.Y,f.Location.Z+dz*f.Z.Z));}
                }
            }
        }
        return result;
    }
    public bool TryChangeToPerspective(double targetDistance,bool symmetric,double lens)
    {
        bool result=Current.Frame.CameraValid&&Current.FrustumValid;SetCameraLocks(Current.Frame.LocationLocked,false,false);
        if(Current.Projection==Perspective&&symmetric==Current.LeftRightSymmetric&&symmetric==Current.TopBottomSymmetric)
        {
            if(Valid(lens)&&lens>0&&TryGetLens(out double current)&&Math.Abs(current-lens)>.125)TrySetLens(lens);
            return result;
        }
        if(!Valid(targetDistance)||targetDistance<=0)targetDistance=TargetDistance(true);
        TryChangeToSymmetric(symmetric,symmetric,targetDistance);SetFrustumSymmetry(symmetric,symmetric);
        if(!ChangeFromParallel(targetDistance,lens))result=false;
        if(result&&Valid(Current.Target))UpdateTarget(targetDistance);
        return result;
    }
}
