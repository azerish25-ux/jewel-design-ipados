using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetIterationAttributesByTypeIdCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public IterationAttribute[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (IterationAttribute[])results[0];
		}
	}

	internal GetIterationAttributesByTypeIdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
