using System;

namespace CS.CSValues;

[Serializable]
public sealed class GemMinMaxInfo
{
	public string GemShape { get; set; }

	public string GemType { get; set; }

	public double MaxX { get; set; }

	public double MaxY { get; set; }

	public double MinX { get; set; }

	public double MinY { get; set; }

	public int GemTableVersion { get; set; }

	public bool IsGemSizeWithinMinMax(double x, double y)
	{
		if (x <= MaxX && x >= MinX && y <= MaxY && y >= MinY)
		{
			return true;
		}
		return false;
	}
}
