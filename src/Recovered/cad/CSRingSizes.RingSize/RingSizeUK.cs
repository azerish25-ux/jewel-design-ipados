using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeUK : RingSize
{
	public override int Count => 66;

	public override string RingSizeTypeAbrv => "UK";

	public override ERingSizeOption RingSizeType => ERingSizeOption.Britain;

	public override int GetIndex(double d)
	{
		return (int)Math.Round((d * Math.PI - 37.55) / 1.25 * 2.0, 0);
	}

	public override double MmDiameter(int n)
	{
		return Math.Round(((double)n / 2.0 * 1.25 + 37.55) / Math.PI, 2);
	}

	public override string TxtSize(int n)
	{
		string text = "";
		text = ((n >= 52) ? ("Z+" + (n - 50) / 2) : ((char)(n / 2 + 65)).ToString());
		if (n % 2 == 1)
		{
			text += " 1/2";
		}
		return text;
	}
}
