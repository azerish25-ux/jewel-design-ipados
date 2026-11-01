// Numerical adaptation of openNURBS, pin f2dc7fba67017475b8d530835a40fcfb5372134a.
// Copyright (c) 1993-2018 Robert McNeel & Associates. Original source notices retained
// in native/Rendering/FrustumReference/LICENSE.openNURBS.reference. This C# adaptation is distinct.
namespace JewelDesign.Rendering.Candidate;

public readonly record struct CandidateFrustumBounds(double Left,double Right,double Bottom,double Top,double Near,double Far);
public readonly record struct CandidateFrustumOptions(bool Perspective,bool CameraUpLocked,bool LeftRightSymmetric,bool TopBottomSymmetric);

/// <summary>
/// Pinned published openNURBS numeric behavior. Raw captured validity/state is preserved;
/// this is not an implementation of private Rhino5 viewport, camera movement or projection matrices.
/// </summary>
public sealed class CandidateViewportFrustum
{
    public const double UnsetPositive=1.23432101234321e308;
    public const double NonsenseCoordinate=1e100;
    public const double SqrtEpsilon=1.490116119385e-8;
    public CandidateFrustumBounds Bounds { get; private set; }
    public CandidateFrustumOptions Options { get; }
    public bool IsValid { get; private set; }
    public int DiagnosticCount { get; private set; }
    public long ProjectionInvalidations { get; private set; }
    public bool IsTwoPointPerspective=>Options.Perspective&&Options.CameraUpLocked&&Options.LeftRightSymmetric&&!Options.TopBottomSymmetric;

    public CandidateViewportFrustum(CandidateFrustumBounds rawBounds,bool rawValidity,CandidateFrustumOptions options)
    {Bounds=rawBounds;IsValid=rawValidity;Options=options;}

    private static bool ValidNumber(double value)=>double.IsFinite(value)&&value!=-UnsetPositive&&value!=UnsetPositive;
    public bool TrySetBounds(CandidateFrustumBounds request)
    {
        var (left,right,bottom,top,near,far)=request;
        if(!(ValidNumber(left)&&ValidNumber(right)&&ValidNumber(top)&&ValidNumber(bottom)&&ValidNumber(near)&&ValidNumber(far)
            &&left<right&&bottom<top&&0<near&&near<far&&-NonsenseCoordinate<left&&right<NonsenseCoordinate
            &&-NonsenseCoordinate<bottom&&top<NonsenseCoordinate&&far<NonsenseCoordinate))
        {DiagnosticCount++;return false;}
        // Published ON_ERROR warns here but still accepts and stores the frustum.
        if(Options.Perspective&&(near<=1e-8||far>1.0001e8*near))DiagnosticCount++;
        if(Options.LeftRightSymmetric&&left!=-right){double distance=.5*(right-left);right=distance;left=-distance;}
        if(Options.TopBottomSymmetric&&bottom!=-top){double distance=.5*(top-bottom);top=distance;bottom=-distance;}
        Bounds=new(left,right,bottom,top,near,far);IsValid=true;ProjectionInvalidations++;return true;
    }

    public bool TryGetAspect(out double aspect)
    {
        bool result=IsValid;aspect=0;
        if(IsValid){double width=Bounds.Right-Bounds.Left,height=Bounds.Top-Bounds.Bottom;if(height==0)result=false;else aspect=width/height;}
        return result;
    }
    public bool TryGetLens(out double lens)
    {
        lens=0;if(!IsValid||Bounds.Near<=0)return false;
        double halfWidth=Bounds.Right>-Bounds.Left?Bounds.Right:-Bounds.Left;
        double halfHeight=Bounds.Top>-Bounds.Bottom?Bounds.Top:-Bounds.Bottom;
        double radius=halfWidth<=halfHeight||IsTwoPointPerspective?halfWidth:halfHeight;
        if(radius<=0)return false;
        lens=Bounds.Near*12/radius;return true;
    }
    public bool TrySetLens(double lens)
    {
        if(!ValidNumber(lens)||lens<=0||!IsValid||Bounds.Near<=0)return false;
        double halfWidth=Bounds.Right>-Bounds.Left?Bounds.Right:-Bounds.Left;
        double halfHeight=Bounds.Top>-Bounds.Bottom?Bounds.Top:-Bounds.Bottom;
        double radius=halfWidth<=halfHeight||IsTwoPointPerspective?halfWidth:halfHeight;
        if(radius<=0)return false;
        double scale=(12/radius)*(Bounds.Near/lens);
        if(Math.Abs(scale-1)<1e-6)return true;
        return TrySetBounds(new(Bounds.Left*scale,Bounds.Right*scale,Bounds.Bottom*scale,Bounds.Top*scale,Bounds.Near,Bounds.Far));
    }
    public bool TrySetHalfAngle(double angle)
    {
        if(!(angle>0&&angle<.5*Math.PI*(1-SqrtEpsilon)))return false;
        if(!IsValid||!TryGetAspect(out double aspect))return false;
        double distance=Bounds.Near*Math.Tan(angle),halfWidth,halfHeight;
        if(aspect>=1){halfWidth=distance*aspect;halfHeight=distance;}
        else{halfWidth=distance;halfHeight=distance/aspect;}
        return TrySetBounds(new(-halfWidth,halfWidth,-halfHeight,halfHeight,Bounds.Near,Bounds.Far));
    }
}
