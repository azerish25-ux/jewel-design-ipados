using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetThumbnailFromFileCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public BitmapSourceByteArrayInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (BitmapSourceByteArrayInfo)results[0];
		}
	}

	public GetThumbnailFromFileCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
