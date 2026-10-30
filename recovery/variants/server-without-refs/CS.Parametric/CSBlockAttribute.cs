using System;
using RMA.Rhino;

namespace CS.Parametric;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CSBlockAttribute : Attribute
{
	private Guid _guid;

	public string StringGuid => _guid.ToString();

	public string BlockName { get; }

	public Guid Guid => _guid;

	public bool Prototype { get; }

	public CSBlockAttribute(string blockName, Guid guid)
	{
		BlockName = blockName;
		_guid = guid;
		Prototype = false;
	}

	public CSBlockAttribute(string blockName, string stringGuid)
	{
		BlockName = blockName;
		try
		{
			_guid = new Guid(stringGuid);
		}
		catch (FormatException)
		{
			RhUtil.RhinoMessageBox($"Unable to parse GUID in:  {blockName}", "Error", 0u);
		}
		Prototype = false;
	}

	public CSBlockAttribute(string blockName, string stringGuid, bool prototype)
	{
		BlockName = blockName;
		try
		{
			_guid = new Guid(stringGuid);
		}
		catch (FormatException)
		{
			RhUtil.RhinoMessageBox($"Unable to parse GUID in:  {blockName}", "Error", 0u);
		}
		Prototype = prototype;
	}
}
