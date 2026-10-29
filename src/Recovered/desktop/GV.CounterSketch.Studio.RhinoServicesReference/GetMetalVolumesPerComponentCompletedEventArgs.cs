using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetMetalVolumesPerComponentCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>)results[0];
		}
	}

	public GetMetalVolumesPerComponentCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
