using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class CreateCustomerThemeCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public int Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[0];
		}
	}

	public CreateCustomerThemeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
