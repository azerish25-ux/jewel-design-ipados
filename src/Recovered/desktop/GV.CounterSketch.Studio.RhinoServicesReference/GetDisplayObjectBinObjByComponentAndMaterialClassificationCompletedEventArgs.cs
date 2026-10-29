using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public byte[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (byte[])results[0];
		}
	}

	public GetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
