using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetDesignSignaturesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public int Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[0];
		}
	}

	public int[] Version
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int[])results[1];
		}
	}

	public int[] BlockCount
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int[])results[2];
		}
	}

	public string[] Signature
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string[])results[3];
		}
	}

	internal GetDesignSignaturesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
