using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetIterationGemInfoCompletedEventArgs : AsyncCompletedEventArgs
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

	public string STONE_ID
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[1];
		}
	}

	public string STONE_NAME
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[2];
		}
	}

	public string STONE_SHAPE
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[3];
		}
	}

	public string STONE_GROUP
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[4];
		}
	}

	public int STONE_LOCATION_INDEX
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[5];
		}
	}

	public string SET_TYPE
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[6];
		}
	}

	public string sXYZ
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[7];
		}
	}

	public string cXYZ
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[8];
		}
	}

	public string xXYZ
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[9];
		}
	}

	public string yXYZ
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string)results[10];
		}
	}

	internal GetIterationGemInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
