using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetDesignIterationValuesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public DataTable Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DataTable)results[0];
		}
	}

	internal GetDesignIterationValuesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
