using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.CAD;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class NurbsGetPolylineCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Point2D[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Point2D[])results[0];
		}
	}

	public NurbsGetPolylineCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
