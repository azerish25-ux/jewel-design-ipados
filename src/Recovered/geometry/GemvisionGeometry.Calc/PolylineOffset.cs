using System;
using System.Linq;
using System.Reflection;
using GemvisionGeometry.Geometry;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Calc;

public class PolylineOffset
{
	private const double _minimumOffsetTolerance = 0.001;

	private const double _halfPi = Math.PI / 2.0;

	private const double _minimumEdgeAbsoluteLength = 0.001;

	private GPoint3D[] _cornerPoints;

	private GVector3D[] _cornerDirections;

	private double[] _cornerOffsetScale;

	private GPoint3D[] _intersectionPoints;

	private EdgeType[] _edgesType;

	private double[] _edgesMaxOffset;

	private double _minEdgeLength = 0.1;

	private int PointCount { get; set; }

	private double MaxOffset => _edgesMaxOffset?.Min() ?? double.NaN;

	private double MinEdgeLength
	{
		get
		{
			return _minEdgeLength;
		}
		set
		{
			_minEdgeLength = value;
			if (_minEdgeLength < 0.001)
			{
				_minEdgeLength = 0.001;
			}
		}
	}

	private double OffsetDistance { get; set; }

	private PolylineOffset(GPolyline plc)
	{
		if (plc == null || !plc.IsClosed || !plc.IsValid)
		{
			throw new Exception($"{GetType().Name} : {MethodBase.GetCurrentMethod().Name} passed a non valid curve.");
		}
		PointCount = plc.PointCount - 1;
		_cornerPoints = new GPoint3D[PointCount];
		for (int i = 0; i < PointCount; i++)
		{
			_cornerPoints[i] = plc[i];
		}
	}

	public static GPolyline PerformOffset(GPolyline plc, double minEdgeLength, double offsetDistance, out double performedDistance)
	{
		return PerformOffset(plc, minEdgeLength, offsetDistance, startPointCheck: false, out performedDistance);
	}

	public static GPolyline PerformOffset(GPolyline plc, double minEdgeLength, double offsetDistance, bool startPointCheck, out double performedDistance)
	{
		PolylineOffset polylineOffset = new PolylineOffset(plc)
		{
			MinEdgeLength = minEdgeLength,
			OffsetDistance = offsetDistance
		};
		bool flag = false;
		double breakPos = 0.0;
		if (startPointCheck)
		{
			flag = polylineOffset.testStartPoint(out breakPos);
		}
		if (!polylineOffset.PerformOffset(out performedDistance))
		{
			return null;
		}
		if (flag)
		{
			polylineOffset.reAddStartPoint(breakPos);
		}
		return polylineOffset.GetPolyline();
	}

	private void GetPtDirections(int ptIndex, out GVector3D backward, out GVector3D forward)
	{
		backward = GetCornerPoint(ptIndex - 1) - GetCornerPoint(ptIndex);
		forward = GetCornerPoint(ptIndex + 1) - GetCornerPoint(ptIndex);
	}

	private bool testStartPoint(out double breakPos)
	{
		GetPtDirections(0, out var backward, out var forward);
		if (Math.Abs(GMath.AngleBetweenVectors(backward, forward) - Math.PI) > 0.001)
		{
			breakPos = 0.0;
			return false;
		}
		GPoint3D[] cornerPoints = _cornerPoints;
		PointCount--;
		_cornerPoints = new GPoint3D[PointCount];
		Array.Copy(cornerPoints, 1, _cornerPoints, 0, PointCount);
		breakPos = backward.Length / (backward.Length + forward.Length);
		return true;
	}

	private void reAddStartPoint(double breakpos)
	{
		GVector3D gVector3D = GetCornerPoint(0) - GetCornerPoint(-1);
		GPoint3D[] cornerPoints = _cornerPoints;
		PointCount++;
		_cornerPoints = new GPoint3D[PointCount];
		Array.Copy(cornerPoints, 0, _cornerPoints, 1, PointCount - 1);
		gVector3D *= breakpos;
		_cornerPoints[0] = GetCornerPoint(-1) + gVector3D;
	}

	private GPolyline GetPolyline()
	{
		GPolyline gPolyline = new GPolyline();
		for (int i = 0; i < PointCount; i++)
		{
			gPolyline.Add(_cornerPoints[i]);
		}
		gPolyline.Add(_cornerPoints[0]);
		return gPolyline;
	}

	private bool PerformOffset(out double distancePerformed)
	{
		distancePerformed = 0.0;
		if (!PreCompute())
		{
			return true;
		}
		if (!IsValid())
		{
			return false;
		}
		double num = OffsetDistance;
		do
		{
			if (!PreCompute())
			{
				return true;
			}
			double num2 = ((num > MaxOffset) ? MaxOffset : num);
			distancePerformed += num2;
			for (int i = 0; i < PointCount; i++)
			{
				_cornerPoints[i] += _cornerDirections[i] * num2 * _cornerOffsetScale[i];
			}
			num -= num2;
		}
		while (num > 0.001 && !double.IsNaN(MaxOffset));
		return true;
	}

	private bool IsValid()
	{
		if (PointCount < 3)
		{
			return false;
		}
		if (_cornerPoints == null || _cornerPoints.Length != PointCount)
		{
			return false;
		}
		if (_cornerDirections == null || _cornerDirections.Length != PointCount)
		{
			return false;
		}
		if (_cornerOffsetScale == null || _cornerOffsetScale.Length != PointCount)
		{
			return false;
		}
		if (_intersectionPoints == null || _intersectionPoints.Length != PointCount)
		{
			return false;
		}
		if (_edgesType == null || _edgesType.Length != PointCount)
		{
			return false;
		}
		if (_edgesMaxOffset == null || _edgesMaxOffset.Length != PointCount)
		{
			return false;
		}
		return true;
	}

	private bool PreCompute()
	{
		if (CalcEdgeTypes() < 3)
		{
			return false;
		}
		CalcCornerDirs();
		CalcIntersectPoints();
		CalcEdgeMaxOffsets();
		return true;
	}

	private GPoint3D GetCornerPoint(int i)
	{
		return _cornerPoints[(i + PointCount) % PointCount];
	}

	private EdgeType GetEdgeType(int i)
	{
		return _edgesType[(i + PointCount) % PointCount];
	}

	private GVector3D GetCornerDir(int i)
	{
		return _cornerDirections[(i + PointCount) % PointCount];
	}

	private int CalcEdgeTypes()
	{
		int num = 0;
		_edgesType = new EdgeType[PointCount];
		for (int i = 0; i < PointCount; i++)
		{
			if (Math.Abs((GetCornerPoint(i + 1) - GetCornerPoint(i)).Length - _minEdgeLength) < 0.001)
			{
				_edgesType[i] = EdgeType.ShortLine;
				continue;
			}
			num++;
			_edgesType[i] = EdgeType.Line;
		}
		return num;
	}

	private void CalcCornerDirs()
	{
		_cornerDirections = new GVector3D[PointCount];
		_cornerOffsetScale = new double[PointCount];
		for (int i = 0; i < PointCount; i++)
		{
			int j;
			for (j = i; GetEdgeType(j) == EdgeType.ShortLine; j++)
			{
			}
			int num = i - 1;
			while (GetEdgeType(num) == EdgeType.ShortLine)
			{
				num--;
			}
			GVector3D gVector3D = GetCornerPoint(j + 1) - GetCornerPoint(j);
			gVector3D.Unitize();
			GVector3D gVector3D2 = GetCornerPoint(num) - GetCornerPoint(num + 1);
			gVector3D2.Unitize();
			GVector3D gVector3D3 = gVector3D + gVector3D2;
			gVector3D3.Unitize();
			if (GVector3D.CrossProduct(gVector3D, gVector3D2).Z > 0.0)
			{
				gVector3D3.Reverse();
			}
			_cornerDirections[i] = gVector3D3;
			double num2 = GMath.AngleBetweenVectors(gVector3D, gVector3D2);
			_cornerOffsetScale[i] = 1.0 / Math.Sin(num2 / 2.0);
		}
	}

	private void CalcIntersectPoints()
	{
		_intersectionPoints = new GPoint3D[PointCount];
		for (int i = 0; i < PointCount; i++)
		{
			if (GetEdgeType(i) != EdgeType.Line)
			{
				_intersectionPoints[i] = GPoint3D.Unset;
				continue;
			}
			GPoint3D cornerPoint = GetCornerPoint(i);
			GPoint3D cornerPoint2 = GetCornerPoint(i + 1);
			GVector3D cornerDir = GetCornerDir(i);
			GVector3D cornerDir2 = GetCornerDir(i + 1);
			_intersectionPoints[i] = GMath.LineInt(cornerPoint, cornerDir, cornerPoint2, cornerDir2);
		}
	}

	private void CalcEdgeMaxOffsets()
	{
		_edgesMaxOffset = new double[PointCount];
		for (int i = 0; i < PointCount; i++)
		{
			if (GetEdgeType(i) != EdgeType.Line)
			{
				_edgesMaxOffset[i] = double.MaxValue;
				continue;
			}
			GPoint3D cornerPoint = GetCornerPoint(i);
			GPoint3D cornerPoint2 = GetCornerPoint(i + 1);
			GPoint3D gPoint3D = _intersectionPoints[i];
			double length = (cornerPoint2 - cornerPoint).Length;
			double num = _minEdgeLength / length;
			GVector3D gVector3D = cornerPoint - gPoint3D;
			double length2 = gVector3D.Length;
			gVector3D *= num;
			_edgesMaxOffset[i] = (length2 - gVector3D.Length) / _cornerOffsetScale[i];
		}
	}
}
