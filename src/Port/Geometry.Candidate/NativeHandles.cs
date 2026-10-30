using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;

internal sealed class ContextHandle:SafeHandle
{
    internal ContextHandle(ulong id):base(IntPtr.Zero,true)
    {SetHandle(new IntPtr(unchecked((long)id)));}
    public override bool IsInvalid=>handle==IntPtr.Zero;
    internal ulong RawId=>unchecked((ulong)DangerousGetHandle().ToInt64());
    protected override bool ReleaseHandle()=>NativeMethods.ContextReleaseRaw(RawId,null,0)==0;
}
internal sealed class ShapeHandle:SafeHandle
{
    private readonly ContextHandle _parent;
    internal ShapeHandle(ContextHandle parent,ulong id):base(IntPtr.Zero,true)
    {
        _parent=parent;bool added=false;
        try {parent.DangerousAddRef(ref added);SetHandle(new IntPtr(unchecked((long)id)));}
        catch {if(added)parent.DangerousRelease();throw;}
    }
    public override bool IsInvalid=>handle==IntPtr.Zero;
    internal ulong RawId=>unchecked((ulong)DangerousGetHandle().ToInt64());
    protected override bool ReleaseHandle()
    {
        try {return NativeMethods.ShapeReleaseRaw(_parent.RawId,RawId,null,0)==0;}
        finally {_parent.DangerousRelease();}
    }
}
