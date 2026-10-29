using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetQuickEstimateCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public string Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[0];
		}
	}

	public double[] pTotalCost
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double[])results[1];
		}
	}

	public double[] pCostMounting
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double[])results[2];
		}
	}

	internal GetQuickEstimateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
