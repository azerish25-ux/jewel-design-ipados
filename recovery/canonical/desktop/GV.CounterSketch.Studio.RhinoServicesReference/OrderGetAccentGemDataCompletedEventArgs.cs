using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.Model.Objects;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class OrderGetAccentGemDataCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public OrderGemData[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (OrderGemData[])results[0];
		}
	}

	public OrderGetAccentGemDataCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
