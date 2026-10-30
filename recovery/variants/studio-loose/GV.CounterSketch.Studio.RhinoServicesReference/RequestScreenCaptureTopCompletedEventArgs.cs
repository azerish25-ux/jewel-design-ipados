using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RequestScreenCaptureTopCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Bitmap Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Bitmap)results[0];
		}
	}

	public RequestScreenCaptureTopCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
