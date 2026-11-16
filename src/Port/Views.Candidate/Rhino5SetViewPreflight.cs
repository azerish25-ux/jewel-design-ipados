using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

/// <summary>Original633-byte SetView input helper. This is validation only, not SetView,
/// display-conduit rebinding or any document/UI restoration effect.</summary>
public static class Rhino5SetViewPreflight
{
    public const double MinimumSpan=1e-18;
    public const double MaximumSpan=1e18;
    public const double MinimumPerspectiveNear=2.3283064365386962890625e-10;
    private static bool Valid(double value)=>Rhino5ScalarValidity.IsValid(value);
    private static bool Valid(CandidatePoint3 p)=>Valid(p.X)&&Valid(p.Y)&&Valid(p.Z);
    private static bool Valid(CandidateVector3 v)=>Valid(v.X)&&Valid(v.Y)&&Valid(v.Z);
    public static bool Accepts(Rhino5RuntimeView view)
    {
        ArgumentNullException.ThrowIfNull(view);ObjectDisposedException.ThrowIf(view.IsDisposed,view);
        var state=view.Viewport;var frame=state.Frame;
        // Original virtual IsValid(view,null) checks these three flags only.
        if(!frame.CameraValid||!state.FrustumValid||!view.Details.ScreenPort.Valid)return false;
        if(!Valid(frame.Location)||!Valid(frame.Direction)||!Valid(frame.Up)||!Valid(frame.X)||!Valid(frame.Y)||!Valid(frame.Z))return false;
        var b=state.Bounds;
        if(!Valid(b.Left)||!Valid(b.Right)||!Valid(b.Top)||!Valid(b.Bottom)||!Valid(b.Near)||!Valid(b.Far))return false;
        double width=Math.Abs(b.Right-b.Left),height=Math.Abs(b.Top-b.Bottom);
        if(!Valid(width)||width<MinimumSpan||width>MaximumSpan||!Valid(height)||height<MinimumSpan||height>MaximumSpan)return false;
        return state.Projection!=2||(b.Near>=MinimumPerspectiveNear&&b.Near>=MinimumSpan);
    }
}
