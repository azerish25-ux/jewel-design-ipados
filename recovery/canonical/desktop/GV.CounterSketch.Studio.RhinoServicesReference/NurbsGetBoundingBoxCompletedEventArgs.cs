using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.CAD;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class NurbsGetBoundingBoxCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public BoundingBox Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (BoundingBox)results[0];
		}
	}

	public NurbsGetBoundingBoxCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
