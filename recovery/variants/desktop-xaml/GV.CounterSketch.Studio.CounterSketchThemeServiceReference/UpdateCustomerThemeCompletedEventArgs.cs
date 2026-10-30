using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class UpdateCustomerThemeCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public bool Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (bool)results[0];
		}
	}

	public UpdateCustomerThemeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
