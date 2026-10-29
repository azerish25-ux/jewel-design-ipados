using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public class GNurbsSurface
{
	private readonly int _degreeU;

	private readonly int _degreeV;

	private readonly GPoint4D[,] _points;

	private readonly double[] _knotsU;

	private readonly double[] _knotsV;

	public GNurbsSurface(int pointsU, int pointsV, int degreeU, int degreeV)
	{
		_degreeU = degreeU;
		_degreeV = degreeV;
		_points = new GPoint4D[pointsU, pointsV];
		_knotsU = new double[pointsU + degreeU + 1];
		_knotsV = new double[pointsV + degreeV + 1];
	}

	public GPoint3D PointAt(double u, double v)
	{
		GNurbsCurve gNurbsCurve = new GNurbsCurve(_points.GetLength(1), _degreeV);
		for (int i = 0; i < _points.GetLength(1); i++)
		{
			GPoint4D point = PointAt(i, u);
			gNurbsCurve.SetPoint(i, point);
		}
		for (int j = 0; j < _knotsV.Length; j++)
		{
			gNurbsCurve.SetKnot(j, _knotsV[j]);
		}
		return gNurbsCurve.PointAt(v);
	}

	public void SetKnot(int which, int index, double value)
	{
		if (which == 0)
		{
			_knotsU[index] = value;
		}
		else
		{
			_knotsV[index] = value;
		}
	}

	public void SetPoint(int indexU, int indexV, GPoint4D point)
	{
		_points[indexU, indexV] = point;
	}

	private GPoint4D PointAt(int indexV, double paramU)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		int length = _points.GetLength(0);
		double[] array = new double[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = GNurbsMath.Nip(i, _degreeU, _knotsU, paramU);
			double num6 = array[i] * _points[i, indexV].W;
			num5 += num6;
		}
		for (int j = 0; j < length; j++)
		{
			double num7 = array[j] / num5;
			num += _points[j, indexV].X * num7 * _points[j, indexV].W;
			num2 += _points[j, indexV].Y * num7 * _points[j, indexV].W;
			num3 += _points[j, indexV].Z * num7 * _points[j, indexV].W;
			num4 += _points[j, indexV].W * num7 * _points[j, indexV].W;
		}
		return new GPoint4D(num, num2, num3, num4);
	}
}
