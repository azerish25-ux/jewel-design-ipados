namespace GemvisionGeometry.Helpers;

public struct GInterval
{
	private readonly double _t0;

	private readonly double _t1;

	public double Max
	{
		get
		{
			if (!(_t0 > _t1))
			{
				return _t1;
			}
			return _t0;
		}
	}

	public double Min
	{
		get
		{
			if (!(_t0 > _t1))
			{
				return _t0;
			}
			return _t1;
		}
	}

	public bool IsValid
	{
		get
		{
			if (!double.IsNaN(_t0))
			{
				return !double.IsNaN(_t1);
			}
			return false;
		}
	}

	public GInterval(double t0, double t1)
	{
		_t0 = t0;
		_t1 = t1;
	}
}
