using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;

public enum CandidateCurveLineKind:uint { Point=1,Overlap=2 }

/// <summary>Independent ascending curve and finite-line ranges, not paired overlap orientation.</summary>
public readonly record struct CandidateCurveLineEvent(
    CandidateCurveLineKind Kind,double CurveStart,double CurveEnd,double LineStart,double LineEnd,
    CandidatePoint3 CurvePointStart,CandidatePoint3 CurvePointEnd,CandidatePoint3 LinePointStart,CandidatePoint3 LinePointEnd);

public sealed class CandidateCurveLineResult
{
    public double RequestedTolerance {get;}
    public double EffectiveFuzzyTolerance {get;}
    public IReadOnlyList<CandidateCurveLineEvent> Events {get;}
    internal CandidateCurveLineResult(double requested,double effective,CandidateCurveLineEvent[] events)
    {RequestedTolerance=requested;EffectiveFuzzyTolerance=effective;Events=Array.AsReadOnly(events);}
}

[StructLayout(LayoutKind.Sequential)]
internal struct NativeCandidatePoint3
{
    internal double X,Y,Z;
    internal readonly CandidatePoint3 ToPoint()
    {
        if(!double.IsFinite(X)||!double.IsFinite(Y)||!double.IsFinite(Z))
            throw new InvalidOperationException("Nonfinite native intersection point.");
        return new(X,Y,Z);
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct NativeCurveLineEvent
{
    internal uint Kind,Reserved;
    internal double CurveStart,CurveEnd,LineStart,LineEnd;
    internal NativeCandidatePoint3 CurvePointStart,CurvePointEnd,LinePointStart,LinePointEnd;
    internal readonly CandidateCurveLineEvent ToEvent()
    {
        if(Kind is not (1 or 2)||Reserved!=0||
            !double.IsFinite(CurveStart)||!double.IsFinite(CurveEnd)||!double.IsFinite(LineStart)||!double.IsFinite(LineEnd)||
            CurveEnd<CurveStart||LineEnd<LineStart||LineStart<0||LineEnd>1||
            (Kind==1&&(CurveStart!=CurveEnd||LineStart!=LineEnd)))
            throw new InvalidOperationException("Invalid native intersection event.");
        return new((CandidateCurveLineKind)Kind,CurveStart,CurveEnd,LineStart,LineEnd,
            CurvePointStart.ToPoint(),CurvePointEnd.ToPoint(),LinePointStart.ToPoint(),LinePointEnd.ToPoint());
    }
}
