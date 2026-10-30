using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[CompilerGenerated]
[Guid("9F379969-5EAC-4A54-B2BC-6946CFFB56EF")]
[DefaultMember("Item")]
[TypeIdentifier]
public interface Processes : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Process Item([In][MarshalAs(UnmanagedType.Struct)] object index);
}
