using System.Globalization;
using CSDatas.Interfaces;

namespace CSDatas;

public sealed class CSNumberData : CSData, ICSDouble, ICSInteger, ICSBoolean
{
	private readonly double _numberData;

	public CSNumberData(double number)
	{
		_numberData = number;
	}

	public CSNumberData(bool number)
	{
		_numberData = (number ? 1 : 0);
	}

	public CSNumberData(CSNumberData copy)
		: base(copy)
	{
		_numberData = copy._numberData;
	}

	public bool GetBoolean()
	{
		return _numberData > 0.5;
	}

	public double GetDouble()
	{
		return _numberData;
	}

	public int GetInteger()
	{
		return (int)_numberData;
	}

	public override CSData Duplicate()
	{
		return new CSNumberData(this);
	}

	public override bool GetBoolean(ref bool value)
	{
		value = _numberData > 0.5;
		return true;
	}

	public override bool GetDouble(ref double value)
	{
		value = _numberData;
		return true;
	}

	public override bool GetInteger(ref int value)
	{
		value = (int)_numberData;
		return true;
	}

	public override string Report()
	{
		double value = 0.0;
		GetDouble(ref value);
		return value.ToString(CultureInfo.InvariantCulture);
	}

	~CSNumberData()
	{
		Dispose(disposing: false);
	}
}
