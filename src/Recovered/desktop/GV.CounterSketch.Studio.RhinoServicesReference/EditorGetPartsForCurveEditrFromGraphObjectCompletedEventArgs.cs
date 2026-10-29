using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.CAD;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class EditorGetPartsForCurveEditrFromGraphObjectCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public double chooserValue
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double)results[0];
		}
	}

	public NurbsCurve curve
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (NurbsCurve)results[1];
		}
	}

	public double[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double[])results[2];
		}
	}

	public EditorGetPartsForCurveEditrFromGraphObjectCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
