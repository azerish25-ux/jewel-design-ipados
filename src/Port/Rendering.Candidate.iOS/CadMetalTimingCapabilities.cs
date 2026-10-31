using System.Runtime.InteropServices;
using ObjCRuntime;
namespace JewelDesign.Rendering.Candidate.iOS;

internal static class CadMetalTimingCapabilities
{
    [DllImport("/usr/lib/libobjc.dylib",EntryPoint="object_getClass")]
    private static extern IntPtr ObjectClass(IntPtr value);
    [DllImport("/usr/lib/libobjc.dylib",EntryPoint="class_respondsToSelector")]
    [return:MarshalAs(UnmanagedType.I1)]
    private static extern bool Responds(IntPtr type,IntPtr selector);
    internal static bool SupportsNativeTiming(INativeObject drawable)
    {
        var type=ObjectClass(drawable.Handle);
        bool supported=type!=IntPtr.Zero&&Responds(type,Selector.GetHandle("addPresentedHandler:"))&&Responds(type,Selector.GetHandle("presentedTime"));
        GC.KeepAlive(drawable);return supported;
    }
}
