using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeGerman : RingSize
{
	public override int Count => 38;

	public override string RingSizeTypeAbrv => "DE";

	public override ERingSizeOption RingSizeType => ERingSizeOption.German;

	public override int GetIndex(double d)
	{
		return (int)Math.Round(d * 4.0 - 53.0, 0);
	}

	public override double MmDiameter(int n)
	{
		return Math.Round((double)(n + 53) / 4.0, 2);
	}

	public override string TxtSize(int n)
	{
		string text = ((n + 53) / 4).ToString();
		switch ((n + 53) % 4)
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
