using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[CompilerGenerated]
[DefaultMember("Name")]
[Guid("5C5A0070-F396-4E37-A82A-1B767E272DF9")]
[TypeIdentifier]
public interface Process
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void Attach();

	void _VtblGap1_3();

	[DispId(0)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(100)]
	int ProcessID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		get;
	}
}
