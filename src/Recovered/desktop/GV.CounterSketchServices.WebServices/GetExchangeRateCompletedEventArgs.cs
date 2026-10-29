using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetExchangeRateCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public float Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (float)results[0];
		}
	}

	public string cultureCode
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[1];
		}
	}

	internal GetExchangeRateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
