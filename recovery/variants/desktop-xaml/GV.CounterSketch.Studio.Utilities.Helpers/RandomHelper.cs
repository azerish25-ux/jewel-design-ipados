using System;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class RandomHelper
{
	private static readonly Random random = new Random();

	public static double GetRandomDouble(double minimum, double maximum)
	{
		return new Random(random.Next()).NextDouble() * (maximum - minimum) + minimum;
	}

	public static int GetRandomInteger(int minimum, int maximum)
	{
		return new Random(random.Next()).Next(minimum, maximum);
	}
}
