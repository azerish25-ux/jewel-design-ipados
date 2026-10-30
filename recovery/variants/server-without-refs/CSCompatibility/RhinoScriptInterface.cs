using System;
using Rhino;
using RhinoScript4;

namespace CSCompatibility;

public static class RhinoScriptInterface
{
	private static IRhinoScript _rhScript;

	private static bool Connect()
	{
		if (_rhScript != null)
		{
			return true;
		}
		try
		{
			_rhScript = (IRhinoScript)RhinoApp.GetPlugInObject("RhinoScript");
		}
		catch (Exception)
		{
			return false;
		}
		return _rhScript != null;
	}

	public static void SetObjectData(Guid objId, string app, string key, object value)
	{
		if (Connect())
		{
			_rhScript.SetObjectData(objId.ToString(), app, key, value);
		}
	}
}
