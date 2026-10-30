using CSDatas.Interfaces;

namespace CSDatas;

public sealed class CSStringData : CSData, ICSString
{
	private readonly string _data;

	public CSStringData(string input)
	{
		_data = input;
	}

	public CSStringData(CSStringData copy)
		: base(copy)
	{
		_data = copy._data;
	}

	public string GetString()
	{
		return _data;
	}

	public override CSData Duplicate()
	{
		return new CSStringData(this);
	}

	public override bool GetString(ref string output)
	{
		output = _data;
		return true;
	}

	public override string Report()
	{
		return _data;
	}
}
