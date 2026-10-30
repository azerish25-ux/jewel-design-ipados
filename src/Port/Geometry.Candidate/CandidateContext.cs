using System.Text;
namespace JewelDesign.Geometry.Candidate;

/// <summary>Experimental native candidate adapter. It does not implement the full application kernel.</summary>
public sealed partial class CandidateContext:IDisposable
{
    private const uint DiagnosticCapacity=256;
    private readonly object _gate=new();
    private readonly ContextHandle _handle;
    private bool _disposed;
    private CandidateContext(ContextHandle handle) {_handle=handle;}
    internal ulong DangerousContextId=>_handle.RawId;

    public static CandidateContext Create()
    {
        if(IntPtr.Size!=8)throw new PlatformNotSupportedException("Candidate handles require a 64-bit process.");
        if(NativeMethods.AbiVersion()!=1)throw new NotSupportedException("Unsupported candidate native ABI version.");
        var diagnostic=new byte[DiagnosticCapacity];
        Check(NativeMethods.ContextCreate(1,out var id,diagnostic,DiagnosticCapacity),diagnostic);
        if(id==0)throw new InvalidOperationException("Native context success returned a zero handle.");
        ContextHandle? handle=null;
        try {handle=new ContextHandle(id);return new CandidateContext(handle);}
        catch {
            if(handle is null)NativeMethods.ContextReleaseRaw(id,null,0);else handle.Dispose();
            throw;
        }
    }
    public CandidateShape CreateBox(double x,double y,double z)=>CreateShape(diagnostic=>{
        var status=NativeMethods.Box(_handle,x,y,z,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    });
    public CandidateShape Translate(CandidateShape shape,double x,double y,double z)=>CreateShape(diagnostic=>{
        var status=NativeMethods.Translate(_handle,shape.Handle,x,y,z,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);
    public CandidateShape Clone(CandidateShape shape)=>CreateShape(diagnostic=>{
        var status=NativeMethods.ShapeClone(_handle,shape.Handle,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);
    public CandidateShape Retain(CandidateShape shape)=>CreateShape(diagnostic=>{
        var id=shape.Handle.RawId;
        var status=NativeMethods.ShapeRetain(_handle,shape.Handle,diagnostic,DiagnosticCapacity);return(status,id);
    },shape);
    public CandidateShape Boolean(CandidateBooleanOperation operation,CandidateShape lhs,CandidateShape rhs,double fuzzy)=>CreateShape(diagnostic=>{
        var status=NativeMethods.Boolean(_handle,(int)operation,lhs.Handle,rhs.Handle,fuzzy,out var id,diagnostic,DiagnosticCapacity);return(status,id);
    },lhs,rhs);
    public CandidateShapeInfo Inspect(CandidateShape shape)
    {
        lock(_gate) {
            ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
            Check(NativeMethods.ShapeInspect(_handle,shape.Handle,out var volume,out var solids,out var valid,diagnostic,DiagnosticCapacity),diagnostic);
            if(valid>1||!double.IsFinite(volume))throw new InvalidOperationException("Invalid native inspection response.");
            return new CandidateShapeInfo(volume,solids,valid==1);
        }
    }
    public ulong DiagnosticLiveShapeCount {
        get {
            lock(_gate) {
                ThrowIfDisposed();var diagnostic=new byte[DiagnosticCapacity];
                Check(NativeMethods.ContextLiveShapes(_handle,out var count,diagnostic,DiagnosticCapacity),diagnostic);
                return count;
            }
        }
    }
    public void Dispose()
    {
        lock(_gate) {if(_disposed)return;_disposed=true;_handle.Dispose();}
    }

    private CandidateShape CreateShape(Func<byte[],(int Status,ulong Id)> operation,params CandidateShape[] inputs)
    {
        // Keep logical disposal outside native creation plus parent-lease acquisition.
        lock(_gate) {
            ThrowIfDisposed();foreach(var shape in inputs)Validate(shape);
            var diagnostic=new byte[DiagnosticCapacity];var result=operation(diagnostic);
            Check(result.Status,diagnostic);
            if(result.Id==0)throw new InvalidOperationException("Native shape success returned a zero handle.");
            ShapeHandle? handle=null;
            try {handle=new ShapeHandle(_handle,result.Id);return new CandidateShape(this,handle);}
            catch {
                if(handle is null)NativeMethods.ShapeReleaseRaw(_handle.RawId,result.Id,null,0);else handle.Dispose();
                throw;
            }
        }
    }
    private void ThrowIfDisposed()
    {if(_disposed||_handle.IsClosed)throw new ObjectDisposedException(nameof(CandidateContext));}
    private void Validate(CandidateShape shape)
    {
        ArgumentNullException.ThrowIfNull(shape);
        if(!ReferenceEquals(shape.Owner,this))throw new ArgumentException("Shape belongs to another candidate context.",nameof(shape));
        if(shape.Handle.IsClosed)throw new ObjectDisposedException(nameof(CandidateShape));
    }
    private static void Check(int status,byte[] diagnostic)
    {
        var end=Array.IndexOf(diagnostic,(byte)0);
        if(end<0)throw new InvalidOperationException("Native diagnostic is not NUL-terminated.");
        if(status!=0)throw new CandidateGeometryException((CandidateNativeStatus)status,Encoding.UTF8.GetString(diagnostic,0,end));
    }
}
