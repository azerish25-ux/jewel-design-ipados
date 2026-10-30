using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
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
