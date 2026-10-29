using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.Pricing;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetManualSizedStonesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public StudioGem[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (StudioGem[])results[0];
		}
	}

	public GetManualSizedStonesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
