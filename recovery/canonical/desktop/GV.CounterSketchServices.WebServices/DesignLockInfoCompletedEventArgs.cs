using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class DesignLockInfoCompletedEventArgs : AsyncCompletedEventArgs
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

	public int status
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[1];
		}
	}

	public DateTime dbTimeStamp
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DateTime)results[2];
		}
	}

	public DateTime fileTimeStamp
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DateTime)results[3];
		}
	}

	public int ComponentId
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[4];
		}
	}

	internal DesignLockInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
