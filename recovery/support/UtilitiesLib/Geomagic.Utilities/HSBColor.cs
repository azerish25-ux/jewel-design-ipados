using System;
using System.Drawing;

namespace Geomagic.Utilities;

public class HSBColor
{
	private int _A;

	private float _H;

	private float _S;

	private float _B;

	public int A
	{
		get
		{
			return _A;
		}
		set
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentOutOfRangeException("value", value, "Alpha must be between 0 and 255.");
			}
			_A = value;
		}
	}

	public float H
	{
		get
		{
			return _H;
		}
		set
		{
			float num;
			for (num = value; num < 0f; num += 360f)
			{
			}
			while (num > 360f)
			{
				num -= 360f;
			}
			_H = num;
		}
	}

	public float S
	{
		get
		{
			return _S;
		}
		set
		{
			float num = value;
			num = ((num >= 0f) ? num : 0f);
			num = ((num <= 1f) ? num : 1f);
			_S = num;
		}
	}

	public float B
	{
		get
		{
			return _B;
		}
		set
		{
			float num = value;
			num = ((num >= 0f) ? num : 0f);
			num = ((num <= 1f) ? num : 1f);
			_B = num;
		}
	}

	public HSBColor()
	{
	}

	private HSBColor(int a, float h, float s, float b)
	{
		A = a;
		H = h;
		S = s;
		B = b;
	}

	public Color ToColor()
	{
		int a = A;
		float num = H;
		float s = S;
		float b = B;
		if (0f == s)
		{
			return Color.FromArgb(a, Convert.ToInt32(b * 255f), Convert.ToInt32(b * 255f), Convert.ToInt32(b * 255f));
		}
		float num2;
		float num3;
		if (0.5 < (double)b)
		{
			num2 = b - b * s + s;
			num3 = b + b * s - s;
		}
		else
		{
			num2 = b + b * s;
			num3 = b - b * s;
		}
		int num4 = (int)Math.Floor(num / 60f);
		if (300f <= num)
		{
			num -= 360f;
		}
		num /= 60f;
		num -= 2f * (float)Math.Floor(((float)num4 + 1f) % 6f / 2f);
		float num5 = ((num4 % 2 != 0) ? (num3 - num * (num2 - num3)) : (num * (num2 - num3) + num3));
		int num6 = Convert.ToInt32(num2 * 255f);
		int num7 = Convert.ToInt32(num5 * 255f);
		int num8 = Convert.ToInt32(num3 * 255f);
		return num4 switch
		{
			1 => Color.FromArgb(a, num7, num6, num8), 
			2 => Color.FromArgb(a, num8, num6, num7), 
			3 => Color.FromArgb(a, num8, num7, num6), 
			4 => Color.FromArgb(a, num7, num8, num6), 
			5 => Color.FromArgb(a, num6, num8, num7), 
			_ => Color.FromArgb(a, num6, num7, num8), 
		};
	}

	public static HSBColor FromAhsb(int a, float h, float s, float b)
	{
		return new HSBColor(a, h, s, b);
	}

	public static HSBColor FromArgb(int a, int r, int g, int b)
	{
		Color color = Color.FromArgb(a, r, g, b);
		return FromColor(color);
	}

	public static HSBColor FromColor(Color Color)
	{
		int a = Color.A;
		float hue = Color.GetHue();
		float saturation = Color.GetSaturation();
		float brightness = Color.GetBrightness();
		return FromAhsb(a, hue, saturation, brightness);
	}
}
