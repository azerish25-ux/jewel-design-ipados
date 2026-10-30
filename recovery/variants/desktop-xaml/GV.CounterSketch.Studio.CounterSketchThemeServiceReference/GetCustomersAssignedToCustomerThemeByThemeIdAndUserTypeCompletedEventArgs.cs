using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Customer[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Customer[])results[0];
		}
	}

	public GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
