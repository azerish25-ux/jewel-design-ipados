using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed record OutsideRailV1Parameters(double TopThickness=3,double SideThickness=2,double BottomThickness=1,bool LockOn=false);
/// <summary>Caller-resolved cached helper, current application and explicit candidate join policies.</summary>
public sealed record OutsideRailCandidateSettings(double HelperTolerance,double ApplicationTolerance,double JoinTolerance,double BoundsTolerance=1e-10);
public enum OutsideRailJoinDisposition { NotUsed,Merged,UnmergedInputs,KernelFailureFallback }
public readonly record struct OutsideRailKernelDiagnostic(string Stage,CandidateNativeStatus Status,string Message);
public sealed class OutsideRailCandidateException:InvalidOperationException
{
    internal OutsideRailCandidateException(string message):base(message){}
}

/// <summary>Owns real geometry from the recovered pipeline. Diagnostics identify source fallback/no-op paths.</summary>
public sealed class OutsideRailCandidate:IDisposable
{
    public CandidateShape Curve {get;}
    public double ResolvedRadius {get;}
    public double MinimumSampledDistance {get;}
    public double OutsideScale=>0;
    public OutsideRailJoinDisposition JoinDisposition {get;}
    public IReadOnlyList<OutsideRailKernelDiagnostic> Diagnostics {get;}
    internal OutsideRailCandidate(CandidateShape curve,double radius,double minimum,OutsideRailJoinDisposition join,List<OutsideRailKernelDiagnostic> diagnostics)
    {Curve=curve;ResolvedRadius=radius;MinimumSampledDistance=minimum;JoinDisposition=join;Diagnostics=Array.AsReadOnly(diagnostics.ToArray());}
    public void Dispose()=>Curve.Dispose();
}
