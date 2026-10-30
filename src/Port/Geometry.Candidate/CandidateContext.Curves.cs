namespace JewelDesign.Geometry.Candidate;

public sealed partial class CandidateContext
{
    /// <summary>Create an actual native four-span rational circle. Plane conventions are explicit.</summary>
    public CandidateShape CreateCircle(double radius,CandidatePlane plane)=>CreateShape(diagnostic=>{
        var status=NativeMethods.Circle(_handle,radius,plane.Origin.X,plane.Origin.Y,plane.Origin.Z,
            plane.Normal.X,plane.Normal.Y,plane.Normal.Z,plane.XDirection.X,plane.XDirection.Y,plane.XDirection.Z,
            out var id,diagnostic,DiagnosticCapacity);return(status,id);
    });
    public CandidateCurveInfo InspectCurve(CandidateShape shape)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveInspect(_handle,shape.Handle,out var first,out var last,out var length,
                out var degree,out var poles,out var rational,diagnostic,DiagnosticCapacity),diagnostic);
            if(!double.IsFinite(first)||!double.IsFinite(last)||!double.IsFinite(length)||last<=first||length<=0||degree==0||poles<=degree||rational>1)
                throw new InvalidOperationException("Invalid native curve inspection response.");
            return new(first,last,length,degree,poles,rational==1);
        }
    }
    /// <summary>Evaluate an actual native parameter, not a normalized arc-length fraction or angle.</summary>
    public CandidatePoint3 PointAt(CandidateShape shape,double parameter)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurvePoint(_handle,shape.Handle,parameter,out var x,out var y,out var z,diagnostic,DiagnosticCapacity),diagnostic);
            if(!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))throw new InvalidOperationException("Invalid native curve point response.");
            return new(x,y,z);
        }
    }
    /// <summary>Map native integrated arc length; tolerance equivalence with Rhino is not established.</summary>
    public double ParameterAtNormalizedLength(CandidateShape shape,double fraction,double absoluteTolerance)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveLengthParameter(_handle,shape.Handle,fraction,absoluteTolerance,out var parameter,diagnostic,DiagnosticCapacity),diagnostic);
            if(!double.IsFinite(parameter))throw new InvalidOperationException("Invalid native length parameter response.");
            return parameter;
        }
    }
    public CandidateCurveProjection ClosestPoint(CandidateShape shape,CandidatePoint3 point)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveClosest(_handle,shape.Handle,point.X,point.Y,point.Z,out var parameter,out var x,out var y,out var z,out var distance,diagnostic,DiagnosticCapacity),diagnostic);
            if(!double.IsFinite(parameter)||!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z)||!double.IsFinite(distance)||distance<0)throw new InvalidOperationException("Invalid native closest point response.");
            return new(parameter,new(x,y,z),distance);
        }
    }
}
