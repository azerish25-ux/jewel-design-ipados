using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetAllRingSizesForCurrentRingSizeRegionCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public KeyValuePair<int, string>[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (KeyValuePair<int, string>[])results[0];
		}
	}

	public GetAllRingSizesForCurrentRingSizeRegionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
