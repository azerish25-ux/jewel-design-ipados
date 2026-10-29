using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetMetalMaterialsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public MetalMaterial[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MetalMaterial[])results[0];
		}
	}

	public GetMetalMaterialsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
