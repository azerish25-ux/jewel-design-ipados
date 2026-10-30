using System;
using System.Reflection;
using System.ServiceModel;

namespace CounterSketchServer.Utilities.Helpers;

public static class DebugHelper
{
	public const bool IncludeExceptionDetailInFaults = true;

	public static Exception ExtractException(this FaultException<ExceptionDetail> fault)
	{
		return ExtractException(fault.Detail);
	}

	public static string GetTimeStamp()
	{
		return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} : ";
	}

	public static void PrintTimedOutput(string message)
	{
	}

	private static Exception ExtractException(ExceptionDetail detail)
	{
		Exception ex = null;
		if (detail.InnerException != null)
		{
			ex = ExtractException(detail.InnerException);
		}
		Exception obj = Activator.CreateInstance(Type.GetType(detail.Type), detail.Message, ex) as Exception;
		SetExceptionField(obj, "_stackTraceString", detail.StackTrace);
		SetExceptionField(obj, "_helpURL", detail.HelpLink);
		return obj;
	}

	private static void SetExceptionField(Exception exception, string field, string value)
	{
		typeof(Exception).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(exception, value);
	}
}
