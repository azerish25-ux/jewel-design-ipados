using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetVisibleComponentsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public ComponentsEnum[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ComponentsEnum[])results[0];
		}
	}

	public GetVisibleComponentsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
