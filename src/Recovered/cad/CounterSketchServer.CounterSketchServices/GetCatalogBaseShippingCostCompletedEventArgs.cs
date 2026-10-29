using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetCatalogBaseShippingCostCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public double Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double)results[0];
		}
	}

	internal GetCatalogBaseShippingCostCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
