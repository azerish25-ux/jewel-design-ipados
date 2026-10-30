using System;

namespace Geomagic.Utilities;

public class ArgGroup
{
	private string _Name = string.Empty;

	private Type _Type;

	private int _NumParameters;

	private object _DefaultValue;

	private string _Help = string.Empty;

	public string Name
	{
		get
		{
			return _Name;
		}
		set
		{
			_Name = value;
		}
	}

	public Type Type
	{
		get
		{
			return _Type;
		}
		set
		{
			_Type = value;
		}
	}

	public bool IsCollection
	{
		get
		{
			if (NumParameters >= 0)
			{
				return NumParameters > 1;
			}
			return true;
		}
	}

	public int NumParameters
	{
		get
		{
			return _NumParameters;
		}
		set
		{
			_NumParameters = value;
		}
	}

	public object DefaultValue
	{
		get
		{
			return _DefaultValue;
		}
		set
		{
			_DefaultValue = value;
		}
	}

	public string Help
	{
		get
		{
			return _Help;
		}
		set
		{
			_Help = value;
		}
	}
}
