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
    public CandidateShape TrimCurve(CandidateShape shape,double first,double last)=>CreateShape(diagnostic=>{
        var status=NativeMethods.CurveTrim(_handle,shape.Handle,first,last,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);
    public CandidateShape ReverseCurve(CandidateShape shape)=>CreateShape(diagnostic=>{
        var status=NativeMethods.CurveReverse(_handle,shape.Handle,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);
    public CandidateShape JoinCurves(CandidateShape first,CandidateShape second,double tolerance)=>CreateShape(diagnostic=>{
        var status=NativeMethods.CurveJoin(_handle,first.Handle,second.Handle,tolerance,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },first,second);

    /// <summary>Unit direction from the actual native derivative within the edge domain. No extrapolation.</summary>
    public CandidateVector3 TangentAt(CandidateShape shape,double parameter)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveTangent(_handle,shape.Handle,parameter,out var x,out var y,out var z,diagnostic,DiagnosticCapacity),diagnostic);
            var normSquared=x*x+y*y+z*z;
            if(!double.IsFinite(normSquared)||Math.Abs(normSquared-1)>1e-10)
                throw new InvalidOperationException("Invalid native unit tangent response.");
            return new(x,y,z);
        }
    }
    /// <summary>Copy a closed native NURBS with domain starting at the requested in-domain seam.</summary>
    public CandidateShape RelocateClosedCurveSeam(CandidateShape shape,double seam)=>CreateShape(diagnostic=>{
        var status=NativeMethods.CurveSeam(_handle,shape.Handle,seam,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);

    /// <summary>Native refined extrema estimates; kernel numerical padding is reported separately.</summary>
    public CandidateCurveBounds GetCurveBounds(CandidateShape shape,double tolerance)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveBounds(_handle,shape.Handle,tolerance,out var x0,out var y0,out var z0,
                out var x1,out var y1,out var z1,out var padding,diagnostic,DiagnosticCapacity),diagnostic);
            static bool Finite(double x,double y,double z)=>double.IsFinite(x)&&double.IsFinite(y)&&double.IsFinite(z);
            if(!Finite(x0,y0,z0)||!Finite(x1,y1,z1)||!double.IsFinite(padding)||padding<0||
                !Finite(x0-padding,y0-padding,z0-padding)||!Finite(x1+padding,y1+padding,z1+padding)||x0>x1||y0>y1||z0>z1)
                throw new InvalidOperationException("Invalid native curve bounds response.");
            return new(new(x0,y0,z0),new(x1,y1,z1),tolerance,padding);
        }
    }
    public bool IsCurveClosed(CandidateShape shape)
    {
        lock(_gate){
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.CurveClosed(_handle,shape.Handle,out var closed,diagnostic,DiagnosticCapacity),diagnostic);
            if(closed>1)throw new InvalidOperationException("Invalid native closure response.");return closed==1;
        }
    }
    public CandidateShape CreateNurbs(CandidateNurbsDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        return CreateShape(diagnostic=>{
            var status=NativeMethods.Nurbs(_handle,definition.Degree,definition.PoleCount,definition.Weights is null?0u:1u,definition.Coordinates,checked((uint)definition.Coordinates.Length),definition.Weights,checked((uint)(definition.Weights?.Length??0)),definition.FullKnots,checked((uint)definition.FullKnots.Length),out var id,diagnostic,DiagnosticCapacity);return(status,id);
        });
    }
}
