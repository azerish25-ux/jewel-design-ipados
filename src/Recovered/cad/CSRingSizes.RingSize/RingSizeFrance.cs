using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeFrance : RingSize
{
	public override int Count => 160;

	public override string RingSizeTypeAbrv => "FR";

	public override ERingSizeOption RingSizeType => ERingSizeOption.France;

	public override int GetIndex(double d)
	{
		return (int)Math.Round((d * Math.PI - 41.0) * 4.0, 0);
	}

	public override double MmDiameter(int n)
	{
		return Math.Round(((double)n / 4.0 + 41.0) / Math.PI, 2);
	}

	public override string TxtSize(int n)
	{
		string text = (n / 4 + 41).ToString();
		switch (n % 4)
		{
		case 1:
			text += " 1/4";
			break;
		case 2:
			text += " 1/2";
			break;
		case 3:
			text += " 3/4";
			break;
		}
		return text;
	}
}
