using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class DocumentGetRingSizeIndexFromSizeTextCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public double Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double)results[0];
		}
	}

	public DocumentGetRingSizeIndexFromSizeTextCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
