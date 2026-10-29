using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetQuotesByKeyIdCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public QuoteStatusResult[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (QuoteStatusResult[])results[0];
		}
	}

	internal GetQuotesByKeyIdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
