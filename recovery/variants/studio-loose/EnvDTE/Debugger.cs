using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[CompilerGenerated]
[Guid("338FB9A0-BAE5-11D2-8AD1-00C04F79E479")]
[TypeIdentifier]
public interface Debugger
{
	void _VtblGap1_30();

	[DispId(113)]
	Processes LocalProcesses
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
