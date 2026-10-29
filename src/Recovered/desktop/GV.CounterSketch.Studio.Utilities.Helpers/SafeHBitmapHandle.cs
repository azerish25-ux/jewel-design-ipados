using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	[SecurityCritical]
	public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle)
	{
		SetHandle(preexistingHandle);
	}

	protected override bool ReleaseHandle()
	{
		return NativeMethods.DeleteObject(handle);
	}
}
