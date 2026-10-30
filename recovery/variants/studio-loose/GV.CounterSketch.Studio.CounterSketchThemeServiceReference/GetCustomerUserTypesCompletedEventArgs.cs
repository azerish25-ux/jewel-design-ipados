using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetCustomerUserTypesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public string[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string[])results[0];
		}
	}

	public GetCustomerUserTypesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
