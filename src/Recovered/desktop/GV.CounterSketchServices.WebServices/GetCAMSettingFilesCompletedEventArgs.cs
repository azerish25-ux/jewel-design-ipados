using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
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
