using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace CounterSketchServer.CounterSketchServices;

[GeneratedCode("System.Web.Services", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetCAMSettingFilesCompletedEventArgs : AsyncCompletedEventArgs
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

	public string[] settingXMLs
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (string[])results[1];
		}
	}

	internal GetCAMSettingFilesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
