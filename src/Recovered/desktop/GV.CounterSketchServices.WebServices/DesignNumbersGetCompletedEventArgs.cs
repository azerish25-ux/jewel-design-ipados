using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class DesignNumbersGetCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public bool Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (bool)results[0];
		}
	}

	public int DesignId
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[1];
		}
	}

	public int IterationIndex
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[2];
		}
	}

	public int ComponentId
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[3];
		}
	}

	internal DesignNumbersGetCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
