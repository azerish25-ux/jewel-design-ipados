using System;

namespace GemvisionGeometry.Helpers;

public static class GNurbsMath
{
	public static void AllBernstein(int n, double u, out double[] b)
	{
		b = new double[n + 1];
		b[0] = 1.0;
		double num = 1.0 - u;
		for (int i = 1; i <= n; i++)
		{
			double num2 = 0.0;
			for (int j = 0; j < i; j++)
			{
				double num3 = b[j];
				b[j] = num2 + num * num3;
				num2 = u * num3;
			}
			b[i] = num2;
		}
	}

	public static double Bin(int i, int n, double u)
	{
		return (double)(GMath.Factorial(n) / (GMath.Factorial(i) * GMath.Factorial(n - i))) * Math.Pow(u, i) * Math.Pow(1.0 - u, n - i);
	}

	public static void Bin1_2(int i, int n, double u, out double b)
	{
		double[] array = new double[n + 1];
		for (int j = 0; j <= n; j++)
		{
			array[j] = 0.0;
		}
		array[n - i] = 1.0;
		double num = 1.0 - u;
		for (int k = 1; k < n; k++)
		{
			for (int num2 = n; num2 >= k; num2--)
			{
				array[num2] = num * array[num2] + u * array[num2 - 1];
			}
		}
		b = array[n];
	}

	internal static double Nip(int pointIndex, int degree, double[] knots, double parameter)
	{
		double[] array = new double[degree + 1];
		int num = knots.Length - 1;
		if ((pointIndex == 0 && parameter == knots[0]) || (pointIndex == num - degree - 1 && parameter == knots[num]))
		{
			return 1.0;
		}
		if (parameter < knots[pointIndex] || parameter >= knots[pointIndex + degree + 1])
		{
			return 0.0;
		}
		for (int i = 0; i <= degree; i++)
		{
			if (parameter >= knots[pointIndex + i] && parameter < knots[pointIndex + i + 1])
			{
				array[i] = 1.0;
			}
			else
			{
				array[i] = 0.0;
			}
		}
		for (int j = 1; j <= degree; j++)
		{
			double num2 = ((array[0] == 0.0) ? 0.0 : ((parameter - knots[pointIndex]) * array[0] / (knots[pointIndex + j] - knots[pointIndex])));
			for (int k = 0; k < degree - j + 1; k++)
			{
				double num3 = knots[pointIndex + k + 1];
				double num4 = knots[pointIndex + k + j + 1];
				if (array[k + 1] == 0.0)
				{
					array[k] = num2;
					num2 = 0.0;
				}
				else
				{
					double num5 = array[k + 1] / (num4 - num3);
					array[k] = num2 + (num4 - parameter) * num5;
					num2 = (parameter - num3) * num5;
				}
			}
		}
		return array[0];
	}
}
