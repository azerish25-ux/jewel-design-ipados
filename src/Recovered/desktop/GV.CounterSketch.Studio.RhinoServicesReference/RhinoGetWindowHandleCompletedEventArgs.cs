using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RhinoGetWindowHandleCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public IntPtr Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (IntPtr)results[0];
		}
	}

	public RhinoGetWindowHandleCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
