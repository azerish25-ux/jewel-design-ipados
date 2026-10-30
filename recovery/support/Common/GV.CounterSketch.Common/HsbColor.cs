using System;
using System.Windows.Media;

namespace GV.CounterSketch.Common;

public class HsbColor
{
	public double HueInverse
	{
		get
		{
			double num = 0.0;
			if (Hue < 180.0)
			{
				return Hue + 180.0;
			}
			return Hue - 180.0;
		}
	}

	public double Hue { get; set; }

	public double Saturation { get; set; }

	public double Brightness { get; set; }

	public byte Alpha { get; set; }

	public static HsbColor FromColor(Color rgbColor)
	{
		HsbColor hsbColor = new HsbColor();
		hsbColor.Alpha = rgbColor.A;
		double num = (double)(int)rgbColor.R / 255.0;
		double num2 = (double)(int)rgbColor.G / 255.0;
		double num3 = (double)(int)rgbColor.B / 255.0;
		double num4 = Math.Max(num, Math.Max(num2, num3));
		double num5 = Math.Min(num, Math.Min(num2, num3));
		if (num4 == num5)
		{
			hsbColor.Hue = 0.0;
		}
		else if (num4 == num)
		{
			hsbColor.Hue = (60.0 * (num2 - num3) / (num4 - num5) + 360.0) % 360.0;
		}
		else if (num4 == num2)
		{
			hsbColor.Hue = 60.0 * (num3 - num) / (num4 - num5) + 120.0;
		}
		else
		{
			hsbColor.Hue = 60.0 * (num - num2) / (num4 - num5) + 240.0;
		}
		if (num4 == 0.0)
		{
			hsbColor.Saturation = 0.0;
		}
		else
		{
			hsbColor.Saturation = 1.0 - num5 / num4;
		}
		hsbColor.Brightness = num4;
		return hsbColor;
	}

	public Color ToColor()
	{
		Color result = new Color
		{
			A = Alpha
		};
		int num = (int)Math.Floor(Hue / 60.0) % 6;
		double num2 = Hue / 60.0 - Math.Floor(Hue / 60.0);
		double num3 = Brightness * (1.0 - Saturation);
		double num4 = Brightness * (1.0 - num2 * Saturation);
		double num5 = Brightness * (1.0 - (1.0 - num2) * Saturation);
		switch (num)
		{
		case 0:
			result.R = (byte)(Brightness * 255.0);
			result.G = (byte)(num5 * 255.0);
			result.B = (byte)(num3 * 255.0);
			break;
		case 1:
			result.R = (byte)(num4 * 255.0);
			result.G = (byte)(Brightness * 255.0);
			result.B = (byte)(num3 * 255.0);
			break;
		case 2:
			result.R = (byte)(num3 * 255.0);
			result.G = (byte)(Brightness * 255.0);
			result.B = (byte)(num5 * 255.0);
			break;
		case 3:
			result.R = (byte)(num3 * 255.0);
			result.G = (byte)(num4 * 255.0);
			result.B = (byte)(Brightness * 255.0);
			break;
		case 4:
			result.R = (byte)(num5 * 255.0);
			result.G = (byte)(num3 * 255.0);
			result.B = (byte)(Brightness * 255.0);
			break;
		case 5:
			result.R = (byte)(Brightness * 255.0);
			result.G = (byte)(num3 * 255.0);
			result.B = (byte)(num4 * 255.0);
			break;
		}
		return result;
	}
}
