using System.Collections.Generic;
using System.Linq;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public class GNurbsCurve
{
	private readonly GPoint4D[] _points;

	private readonly double[] _knots;

	public GInterval Domain => new GInterval(_knots[0], _knots[_knots.Length - 1]);

	public int Degree { get; }

	public List<GPoint4D> Points => _points.ToList();

	public List<double> Knots => _knots.ToList();

	public GNurbsCurve()
	{
	}

	public GNurbsCurve(int points, int degree)
	{
		Degree = degree;
		_points = new GPoint4D[points];
		_knots = new double[points + Degree + 1];
	}

	public GPoint3D PointAt(double param)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double[] array = new double[_points.Length];
		for (int i = 0; i < _points.Length; i++)
		{
			array[i] = GNurbsMath.Nip(i, Degree, _knots, param);
			double num5 = array[i] * _points[i].W;
			num4 += num5;
		}
		for (int j = 0; j < _points.Length; j++)
		{
			double num6 = array[j] / num4;
			num += _points[j].X * _points[j].W * num6;
			num2 += _points[j].Y * _points[j].W * num6;
			num3 += _points[j].Z * _points[j].W * num6;
		}
		return new GPoint3D(num, num2, num3);
	}

	public void SetKnot(int index, double value)
	{
		_knots[index] = value;
	}

	public void SetPoint(int index, GPoint4D point)
	{
		_points[index] = point;
	}
}
