using System;

namespace CSRingSizes.RingSize;

public sealed class RingSizeUSA : RingSize
{
	public override int Count => 65;

	public override string RingSizeTypeAbrv => "US";

	public override ERingSizeOption RingSizeType => ERingSizeOption.UnitedStates;

	public override int GetIndex(double d)
	{
		return (int)Math.Round((d / 25.4 * Math.PI - 1.43) / 0.102 * 4.0, 0);
	}

	public override bool GetIndexTxt(string size, ref int index)
	{
		try
		{
			if (base.GetIndexTxt(size, ref index))
			{
				return true;
			}
			double num = double.Parse(size) * 0.032467608390746645;
			num += 0.45518313724282067;
			num *= 25.4;
			index = GetIndex(num);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public override double MmDiameter(int n)
	{
		return Math.Round(((double)n / 4.0 * 0.102 + 1.43) / Math.PI * 25.4, 2);
	}

	public override string TxtSize(int n)
	{
		string text = (n / 4).ToString();
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
