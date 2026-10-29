using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeJapan : RingSize
{
	public override int Count => 37;

	public override string RingSizeTypeAbrv => "JP";

	public override ERingSizeOption RingSizeType => ERingSizeOption.Japan;

	public override int GetIndex(double d)
	{
		return (int)Math.Round((d - 13.0) * 3.0 + 1.0, 0);
	}

	public override double MmDiameter(int n)
	{
		return Math.Round((double)(n - 1) / 3.0 + 13.0, 2);
	}

	public override string TxtSize(int n)
	{
		return n.ToString();
	}
}
