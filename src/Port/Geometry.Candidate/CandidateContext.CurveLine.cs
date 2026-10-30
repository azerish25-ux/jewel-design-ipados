using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;

public sealed partial class CandidateContext
{
    public CandidateShape CreateLine(CandidatePoint3 start,CandidatePoint3 end)=>CreateShape(diagnostic=>{
        var status=NativeMethods.Line(_handle,start.X,start.Y,start.Z,end.X,end.Y,end.Z,out var id,diagnostic,DiagnosticCapacity);
        return(status,id);
    });

    /// <summary>Native finite3D point/overlap events. Candidate tolerance/order are not established Rhino equivalence.</summary>
    public CandidateCurveLineResult IntersectCurveLine(CandidateShape shape,CandidatePoint3 start,CandidatePoint3 end,double tolerance)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);
            if(Marshal.SizeOf<NativeCurveLineEvent>()!=136||Marshal.OffsetOf<NativeCurveLineEvent>(nameof(NativeCurveLineEvent.CurvePointStart)).ToInt64()!=40)
                throw new PlatformNotSupportedException("Unsupported native intersection layout.");
            var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveLineIntersections(_handle,shape.Handle,start.X,start.Y,start.Z,end.X,end.Y,end.Z,
                tolerance,null,0,out var count,out var effective,diagnostic,DiagnosticCapacity),diagnostic);
            if(count>65536||!double.IsFinite(effective)||effective<=0||effective<tolerance)
                throw new InvalidOperationException("Invalid native intersection count/tolerance.");
            if(count==0)return new(tolerance,effective,Array.Empty<CandidateCurveLineEvent>());
            var native=new NativeCurveLineEvent[checked((int)count)];
            Check(NativeMethods.CurveLineIntersections(_handle,shape.Handle,start.X,start.Y,start.Z,end.X,end.Y,end.Z,
                tolerance,native,count,out var filled,out var filledEffective,diagnostic,DiagnosticCapacity),diagnostic);
            if(filled!=count||filledEffective!=effective)throw new InvalidOperationException("Native intersection query changed between passes.");
            var events=new CandidateCurveLineEvent[native.Length];
            for(int i=0;i<native.Length;i++)events[i]=native[i].ToEvent();
            return new(tolerance,effective,events);
        }
    }
}
