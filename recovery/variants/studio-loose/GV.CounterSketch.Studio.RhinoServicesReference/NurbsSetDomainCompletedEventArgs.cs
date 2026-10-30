using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.CAD;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class NurbsSetDomainCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public NurbsCurve Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (NurbsCurve)results[0];
		}
	}

	public NurbsSetDomainCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
