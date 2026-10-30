namespace JewelDesign.Geometry.Candidate;
public enum CandidatePairedCorrespondence:uint {ExplicitLinear=0,SectionPlanes=1}
public readonly record struct CandidateSurfaceDomain(double FirstU,double LastU,double FirstV,double LastV);
public sealed partial class CandidateContext
{
 /// <summary>Experimental open-path paired map; closed/source Rhino sweep policies are not implemented.</summary>
 public CandidateShape CreatePairedSweep(CandidateShape first,CandidateShape second,IReadOnlyList<CandidateShape> profiles,IReadOnlyList<double> parameters1,IReadOnlyList<double> parameters2,bool maintainHeight,double tolerance,CandidatePairedCorrespondence correspondence=CandidatePairedCorrespondence.SectionPlanes)
 {
  ArgumentNullException.ThrowIfNull(profiles);ArgumentNullException.ThrowIfNull(parameters1);ArgumentNullException.ThrowIfNull(parameters2);
  int count=profiles.Count;
  if(count<2||count>4096||count!=parameters1.Count||count!=parameters2.Count)throw new ArgumentException("Paired sweep requires matching bounded curve/parameter lists.");
  var inputs=new CandidateShape[count+2];inputs[0]=first;inputs[1]=second;for(int i=0;i<count;i++)inputs[i+2]=profiles[i];
  var a=parameters1.ToArray();var b=parameters2.ToArray();
  return CreateShape(diagnostic=>{
   var leased=new List<ShapeHandle>();
   try{
    // Raw-ID arrays require explicit SafeHandle leases, including concurrent disposal.
    foreach(var input in inputs){bool added=false;try{input.Handle.DangerousAddRef(ref added);leased.Add(input.Handle);}catch{if(added)input.Handle.DangerousRelease();throw;}}
    var ids=new ulong[count];for(int i=0;i<ids.Length;i++)ids[i]=inputs[i+2].Handle.RawId;
    var status=NativeMethods.PairedSweep(_handle,first.Handle,second.Handle,ids,(uint)ids.Length,a,(uint)a.Length,b,(uint)b.Length,maintainHeight?1u:0u,(uint)correspondence,tolerance,out var id,diagnostic,DiagnosticCapacity);return(status,id);
   }finally{foreach(var handle in leased)handle.DangerousRelease();}
  },inputs);
 }
 public CandidateSurfaceDomain InspectSurface(CandidateShape shape)
 {
  lock(_gate){ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];Check(NativeMethods.SurfaceBounds(_handle,shape.Handle,out var u0,out var u1,out var v0,out var v1,diagnostic,DiagnosticCapacity),diagnostic);
   if(!double.IsFinite(u0)||!double.IsFinite(u1)||!double.IsFinite(v0)||!double.IsFinite(v1)||u0>=u1||v0>=v1)throw new InvalidOperationException("Invalid native surface domain response.");return new(u0,u1,v0,v1);}
 }
 /// <summary>Underlying native surface within face UV bounds; trim classification is separate.</summary>
 public CandidatePoint3 PointAtSurface(CandidateShape shape,double u,double v)
 {
  lock(_gate){ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];Check(NativeMethods.SurfacePoint(_handle,shape.Handle,u,v,out var x,out var y,out var z,diagnostic,DiagnosticCapacity),diagnostic);
   if(!double.IsFinite(x)||!double.IsFinite(y)||!double.IsFinite(z))throw new InvalidOperationException("Invalid native surface point response.");return new(x,y,z);}
 }
}
