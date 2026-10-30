using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[CompilerGenerated]
[Guid("04A72314-32E9-48E2-9B87-A63603454F3E")]
[DefaultMember("Name")]
[TypeIdentifier]
public interface _DTE
{
	[DispId(0)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void _VtblGap1_45();

	[DispId(244)]
	Debugger Debugger
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(244)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
