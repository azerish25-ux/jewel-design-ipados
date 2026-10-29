using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class DocumentUserDataGetAllDataElementsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Tuple<string, string, string>[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Tuple<string, string, string>[])results[0];
		}
	}

	public DocumentUserDataGetAllDataElementsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
