using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

public readonly record struct Rhino5ScreenPort(int Left,int Right,int Bottom,int Top,int Near,int Far,bool Valid);
/// <summary>Effective original guards; native field names and parent ownership are resolved by the caller.</summary>
public readonly record struct Rhino5ViewportCommitGuards(bool Inhibited,bool WorldAlignedParallelOnly);
/// <summary>Numeric portion of the original owned view/cache state. Not a whole ON_3dmView history snapshot.</summary>
public readonly record struct Rhino5ViewportCommitState(Rhino5ViewportState Viewport,CandidatePoint3 TargetMirror,
    Rhino5Matrix4 ClipModification,Rhino5ScreenPort ScreenPort,uint ChangeSerial,uint CacheSerial,
    Rhino5Matrix4 CachedWorldToClip,Rhino5Matrix4 ExtraTransform,int CachedWidth,int CachedHeight);

/// <summary>Original5 SetVP numerical mutation. Accepted commits still require document notification then whole-view history push.</summary>
public sealed class Rhino5ViewportCommitCore
{
    public Rhino5ViewportCommitState Current {get;private set;}
    public int DiagnosticCount {get;private set;}
    public Rhino5ViewportCommitCore(Rhino5ViewportCommitState state)
    {
        ArgumentNullException.ThrowIfNull(state.ClipModification);ArgumentNullException.ThrowIfNull(state.CachedWorldToClip);ArgumentNullException.ThrowIfNull(state.ExtraTransform);Current=state;
    }
    private static bool Equal(CandidateVector3 a,CandidateVector3 b)=>a.X==b.X&&a.Y==b.Y&&a.Z==b.Z;
    public bool TrySetViewTarget(CandidatePoint3 target)
    {
        var viewport=new Rhino5Viewport(Current.Viewport);bool accepted=viewport.TrySetTarget(target);
        // Original ON_3dmView setter copies the resulting core target even when the request is rejected.
        Current=Current with {Viewport=viewport.Current,TargetMirror=viewport.Current.Target};return accepted;
    }
    public CandidatePoint3 ReadViewTarget()
    {
        var target=Current.Viewport.Target;var mirror=Current.TargetMirror;
        if(!(target.X==mirror.X&&target.Y==mirror.Y&&target.Z==mirror.Z))
        {DiagnosticCount++;Current=Current with {TargetMirror=target};}
        return target;
    }
    /// <summary>Performs numeric state changes only. True requires the caller to continue document/history effects before final target write.</summary>
    public bool TryApplyNumericViewport(Rhino5ViewportState incoming,int mode,Rhino5ViewportCommitGuards guards)
    {
        var original=Current;
        if(guards.Inhibited)return false;
        if(guards.WorldAlignedParallelOnly&&(incoming.Projection!=Rhino5Viewport.Parallel||!Equal(incoming.Frame.X,new(1,0,0))||!Equal(incoming.Frame.Y,new(0,1,0))||!Equal(incoming.Frame.Z,new(0,0,1))))return false;
        if(!incoming.Frame.CameraValid||!incoming.FrustumValid)return false;
        var viewport=new Rhino5Viewport(original.Viewport);double distance=viewport.TargetDistance(true);
        viewport.SetCameraLocks(false,false,false);viewport.SetFrustumSymmetry(false,false);
        // Return values are deliberately ignored, as in the original SetVP sequence.
        viewport.TrySetProjection(incoming.Projection);viewport.TrySetLocation(incoming.Frame.Location);
        viewport.TrySetDirection(incoming.Frame.Direction);viewport.TrySetUp(incoming.Frame.Up);
        var b=incoming.Bounds;
        if(incoming.LeftRightSymmetric&&b.Right!=-b.Left){double half=.5*(b.Right-b.Left);b=b with {Left=-half,Right=half};}
        if(incoming.TopBottomSymmetric&&b.Top!=-b.Bottom){double half=.5*(b.Top-b.Bottom);b=b with {Bottom=-half,Top=half};}
        viewport.TrySetBounds(b);var mirror=original.TargetMirror;
        if(mode!=0&&distance>0)
        {
            var f=viewport.Current.Frame;
            viewport.TrySetTarget(new(f.Location.X-distance*f.Z.X,f.Location.Y-distance*f.Z.Y,f.Location.Z-distance*f.Z.Z));
            mirror=viewport.Current.Target;
        }
        viewport.SetCameraLocks(incoming.Frame.LocationLocked,incoming.Frame.DirectionLocked,incoming.Frame.UpLocked);
        viewport.SetFrustumSymmetry(incoming.LeftRightSymmetric,incoming.TopBottomSymmetric);
        uint serial=unchecked(original.ChangeSerial+1);if(serial==0)serial=1;
        uint cacheSerial=original.CacheSerial;var cache=original.CachedWorldToClip;int width=original.CachedWidth,height=original.CachedHeight;
        if(cacheSerial!=serial)
        {
            bool transformed=Rhino5Matrix4.TryWorldToClip(viewport.Current,original.ClipModification,out cache);
            // The original output points into its cache: false still overwrites the matrix with identity.
            if(transformed){if(!original.ExtraTransform.IsIdentity())cache=cache.Multiply(original.ExtraTransform);cacheSerial=serial;}
            if(original.ScreenPort.Valid){width=unchecked(original.ScreenPort.Right-original.ScreenPort.Left);height=unchecked(original.ScreenPort.Bottom-original.ScreenPort.Top);}
            else {width=0;height=0;}
        }
        DiagnosticCount+=viewport.DiagnosticCount;
        Current=original with {Viewport=viewport.Current,TargetMirror=mirror,ChangeSerial=serial,CacheSerial=cacheSerial,CachedWorldToClip=cache,CachedWidth=width,CachedHeight=height};
        return true;
    }
}
