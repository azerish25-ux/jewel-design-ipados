using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeEur : RingSize
{
	public override int Count => 39;

	public override string RingSizeTypeAbrv => "EU";

	public override ERingSizeOption RingSizeType => ERingSizeOption.European;

	public override int GetIndex(double d)
	{
		return (int)Math.Round(d * Math.PI - 41.0, 0);
	}

	public override double MmDiameter(int n)
	{
		return Math.Round((double)(n + 41) / Math.PI, 2);
	}

	public override string TxtSize(int n)
	{
		return (n + 41).ToString();
	}
}
