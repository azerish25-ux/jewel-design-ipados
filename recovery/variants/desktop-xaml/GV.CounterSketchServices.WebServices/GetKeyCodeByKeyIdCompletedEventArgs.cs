using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetKeyCodeByKeyIdCompletedEventArgs : AsyncCompletedEventArgs
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

	internal GetKeyCodeByKeyIdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
