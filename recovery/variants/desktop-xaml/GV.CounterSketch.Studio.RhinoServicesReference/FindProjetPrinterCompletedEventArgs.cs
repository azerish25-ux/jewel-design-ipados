using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class FindProjetPrinterCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public string FindAddress
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[0];
		}
	}

	public bool Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (bool)results[1];
		}
	}

	public FindProjetPrinterCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
