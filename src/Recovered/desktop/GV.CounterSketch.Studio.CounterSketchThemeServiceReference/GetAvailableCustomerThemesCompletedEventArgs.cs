using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetAvailableCustomerThemesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public CustomerTheme[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (CustomerTheme[])results[0];
		}
	}

	public GetAvailableCustomerThemesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
