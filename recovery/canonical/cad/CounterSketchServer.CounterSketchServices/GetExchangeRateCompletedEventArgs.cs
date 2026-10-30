using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
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
