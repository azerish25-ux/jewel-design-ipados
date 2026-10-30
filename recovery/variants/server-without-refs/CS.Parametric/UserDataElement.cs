using System;
using RMA.OpenNURBS;

namespace CS.Parametric;

[Serializable]
public sealed class UserDataElement : IEquatable<UserDataElement>
{
	private string _type;

	private string _key;

	private string _value;

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

	public string Key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = value;
		}
	}

	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public UserDataElement()
	{
		_type = "";
		_key = "";
		_value = "";
	}

	public UserDataElement(string t, string k, string v)
	{
		_type = t;
		_key = k;
		_value = v;
		if (_type == null)
		{
			_type = "";
		}
		if (_key == null)
		{
			_key = "";
		}
		if (_value == null)
		{
			_value = "";
		}
	}

	public bool Equals(UserDataElement other)
	{
		if (Key == other.Key && Value == other.Value)
		{
			return Type == other.Type;
		}
		return false;
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		return (byte)(1u & (archive.ReadString(ref _type) ? 1u : 0u) & (archive.ReadString(ref _key) ? 1u : 0u) & (archive.ReadString(ref _value) ? 1u : 0u)) != 0;
	}

	public override string ToString()
	{
		return _type + " " + _key + " " + _value;
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		return (byte)(1u & (archive.WriteString(_type) ? 1u : 0u) & (archive.WriteString(_key) ? 1u : 0u) & (archive.WriteString(_value) ? 1u : 0u)) != 0;
	}
}
