using System;

namespace CSBlocks.Core;

[Serializable]
public struct CSGroupOfGemsIdentifier
{
	private int _gemTableSizeVersion;

	private string _shape;

	private string _type;

	private int _versionMajor;

	private int _versionMinor;

	public string Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public string Shape
	{
		get
		{
			return _shape;
		}
		set
		{
			_shape = value;
		}
	}

	public int VersionMajor
	{
		get
		{
			return _versionMajor;
		}
		set
		{
			_versionMajor = value;
		}
	}

	public int VersionMinor
	{
		get
		{
			return _versionMinor;
		}
		set
		{
			_versionMinor = value;
		}
	}

	public int GemTableSizeVersion
	{
		get
		{
			return _gemTableSizeVersion;
		}
		set
		{
			_gemTableSizeVersion = value;
		}
	}

	public CSGroupOfGemsIdentifier(string type, string shape, int versionMajor, int versionMinor, int gemTableSizeVersion = 0)
	{
		_type = type;
		_shape = shape;
		_versionMajor = versionMajor;
		_versionMinor = versionMinor;
		_gemTableSizeVersion = gemTableSizeVersion;
	}

	public new string ToString()
	{
		return _type + "_" + _shape + " - " + _versionMajor + "." + _versionMinor + " : " + _gemTableSizeVersion;
	}
}
