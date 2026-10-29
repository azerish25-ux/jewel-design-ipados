using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RhinoScript4;

[ComImport]
[CompilerGenerated]
[InterfaceType(2)]
[Guid("DC7E0867-5922-4DCB-935F-585F81484030")]
[TypeIdentifier]
public interface IRhinoScript
{
	void _VtblGap1_243();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(244)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object SetObjectData([MarshalAs(UnmanagedType.Struct)] object vaObject, [MarshalAs(UnmanagedType.Struct)] object vaApp, [MarshalAs(UnmanagedType.Struct)] object vaKey, [MarshalAs(UnmanagedType.Struct)] object vaValue);
}
