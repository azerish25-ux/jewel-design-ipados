using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GeoCodeAddressCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public double[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double[])results[0];
		}
	}

	internal GeoCodeAddressCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
