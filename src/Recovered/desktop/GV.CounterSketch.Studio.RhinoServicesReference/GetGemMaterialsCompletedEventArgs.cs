using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetGemMaterialsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public GemMaterial[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (GemMaterial[])results[0];
		}
	}

	public GetGemMaterialsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
