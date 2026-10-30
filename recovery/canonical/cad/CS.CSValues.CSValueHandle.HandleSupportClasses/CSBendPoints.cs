using System;
using CS.JSonDoc;
using CSCompatibility;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle.HandleSupportClasses;

public sealed class CSBendPoints
{
	public int _bendMode;

	public bool _iconMouseOver;

	private Point3d _basePoint;

	private Point3d _bendPoint;

	private Point3d _endPoint;

	private Point3d _lastBasePoint;

	private int _lastBendMode;

	private Point3d _lastBendPoint;

	private Point3d _lastEndPoint;

	public Point3d BasePoint
	{
		get
		{
			return _basePoint;
		}
		set
		{
			Point3d point3d = new Point3d(value - _basePoint);
			_basePoint = new Point3d(value);
			_endPoint += point3d;
			_bendPoint += point3d;
		}
	}

	public Point3d EndPoint
	{
		get
		{
			return _endPoint;
		}
		set
		{
			double scaleFactor = (value - _basePoint).Length / (_endPoint - _basePoint).Length;
			Transform xform = Transform.Rotation(_endPoint - _basePoint, value - _basePoint, _basePoint);
			_bendPoint.Transform(xform);
			xform = Transform.Scale(_basePoint, scaleFactor);
			_bendPoint.Transform(xform);
			_endPoint = new Point3d(value);
		}
	}

	public Point3d BendPoint
	{
		get
		{
			return _bendPoint;
		}
		set
		{
			_bendPoint = new Point3d(value);
		}
	}

	public CSBendPoints()
	{
		_basePoint = new Point3d(0.0, 0.0, 0.0);
		_endPoint = new Point3d(0.0, 0.0, 0.0);
		_bendPoint = new Point3d(0.0, 0.0, 0.0);
		_lastBendPoint = new Point3d(0.0, 0.0, 0.0);
		_lastEndPoint = new Point3d(0.0, 0.0, 0.0);
		_lastBasePoint = new Point3d(0.0, 0.0, 0.0);
	}

	public CSBendPoints(Point3d basePoint, Point3d endPoint, Point3d bendPoint, int bendMode)
	{
		_basePoint = basePoint;
		_endPoint = endPoint;
		_bendPoint = bendPoint;
		_bendMode = bendMode;
		_lastBendPoint = new Point3d(0.0, 0.0, 0.0);
		_lastEndPoint = new Point3d(0.0, 0.0, 0.0);
		_lastBasePoint = new Point3d(0.0, 0.0, 0.0);
	}

	public void Changed()
	{
		_lastBasePoint = new Point3d(_basePoint);
		_lastEndPoint = new Point3d(_endPoint);
		_lastBendPoint = new Point3d(_bendPoint);
		_lastBendMode = _bendMode;
	}

	public bool GetArc(out Arc arc)
	{
		return ArcFromPoints(_basePoint, _endPoint, _bendPoint, out arc);
	}

	public NurbsCurve GetDisplayCurve()
	{
		if (GetArc(out var arc))
		{
			return arc.ToNurbsCurve();
		}
		return new LineCurve(_basePoint, _bendPoint).ToNurbsCurve();
	}

	public NurbsCurve GetDisplayMirrorCurve()
	{
		if (GetMirrorArc(out var arc))
		{
			return arc.ToNurbsCurve();
		}
		return new LineCurve(_basePoint, _basePoint - (_bendPoint - _basePoint)).ToNurbsCurve();
	}

	public bool HasChanged()
	{
		if (IsBending())
		{
			if (!((_basePoint - _lastBasePoint).Length > 0.001) && !((_endPoint - _lastEndPoint).Length > 0.001) && !((_bendPoint - _lastBendPoint).Length > 0.001))
			{
				return _bendMode != _lastBendMode;
			}
			return true;
		}
		return false;
	}

	public bool IsAllZero()
	{
		if (Math.Abs(_basePoint.X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_basePoint.Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_basePoint.Z) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_endPoint.X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_endPoint.Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_endPoint.Z) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_bendPoint.X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_bendPoint.Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(_bendPoint.Z) > 0.001)
		{
			return false;
		}
		return true;
	}

	public bool IsBending()
	{
		Vector3d inputVector = _endPoint - _basePoint;
		Vector3d inputVector2 = _bendPoint - _basePoint;
		return CSValueHandleHelper.AngleBetweenVectors(inputVector, inputVector2) > Math.PI / 180.0;
	}

	public static CSBendPoints NewBp(CSBendPoints copy)
	{
		if (copy == null)
		{
			return null;
		}
		return new CSBendPoints
		{
			_basePoint = copy._basePoint,
			_endPoint = copy._endPoint,
			_bendPoint = copy._bendPoint,
			_bendMode = copy._bendMode,
			_lastBasePoint = copy._lastBasePoint,
			_lastEndPoint = copy._lastEndPoint,
			_lastBendPoint = copy._lastBendPoint,
			_lastBendMode = copy._lastBendMode
		};
	}

	public static bool Read(JToken archive, out CSBendPoints newPoint)
	{
		CSBendPoints cSBendPoints = new CSBendPoints();
		bool flag = true;
		JObject jObject = (JObject)archive;
		if (jObject.ContainsKey("BasePoint"))
		{
			cSBendPoints._basePoint = jObject.GetPoint("BasePoint");
		}
		else
		{
			flag = false;
		}
		if (jObject.ContainsKey("EndPoint"))
		{
			cSBendPoints._endPoint = jObject.GetPoint("EndPoint");
		}
		else
		{
			flag = false;
		}
		if (jObject.ContainsKey("BendPoint"))
		{
			cSBendPoints._bendPoint = jObject.GetPoint("BendPoint");
		}
		else
		{
			flag = false;
		}
		if (jObject.ContainsKey("BendMode"))
		{
			cSBendPoints._bendMode = (int)archive["BendMode"];
		}
		else
		{
			flag = false;
		}
		newPoint = (flag ? cSBendPoints : null);
		return flag;
	}

	public void Read(ref OnBinaryArchive archive, int version)
	{
		On3dPoint pt = new On3dPoint();
		switch (version)
		{
		case 1:
		{
			archive.ReadPoint(ref pt);
			_basePoint = CSConvert.On3dPointCopyToPoint3d(pt);
			archive.ReadPoint(ref pt);
			_endPoint = CSConvert.On3dPointCopyToPoint3d(pt);
			archive.ReadPoint(ref pt);
			_bendPoint = CSConvert.On3dPointCopyToPoint3d(pt);
			byte[] bytes = new byte[1];
			archive.ReadBytes(ref bytes);
			_bendMode = bytes[0];
			break;
		}
		case 2:
		case 3:
		case 4:
		case 5:
			archive.ReadPoint(ref pt);
			_basePoint = CSConvert.On3dPointCopyToPoint3d(pt);
			archive.ReadPoint(ref pt);
			_endPoint = CSConvert.On3dPointCopyToPoint3d(pt);
			archive.ReadPoint(ref pt);
			_bendPoint = CSConvert.On3dPointCopyToPoint3d(pt);
			archive.ReadInt(ref _bendMode);
			break;
		}
	}

	public void Write(ref OnBinaryArchive archive)
	{
		archive.WritePoint(CSConvert.Point3dCopyToOn3dPoint(_basePoint));
		archive.WritePoint(CSConvert.Point3dCopyToOn3dPoint(_endPoint));
		archive.WritePoint(CSConvert.Point3dCopyToOn3dPoint(_bendPoint));
		archive.WriteInt(_bendMode);
	}

	public JObject WriteJson()
	{
		return new JObject
		{
			{ "BasePoint", _basePoint },
			{ "EndPoint", _endPoint },
			{ "BendPoint", _bendPoint },
			{ "BendMode", _bendMode }
		};
	}

	private static bool ArcFromPoints(Point3d basePoint, Point3d sourcePoint, Point3d bendPoint, out Arc arc)
	{
		Vector3d vector3d = new Vector3d(sourcePoint - basePoint);
		vector3d.Unitize();
		Vector3d b = new Vector3d(bendPoint - basePoint);
		b.Unitize();
		Vector3d a = Vector3d.CrossProduct(vector3d, b);
		if (Math.Abs(a.Length) < 0.001)
		{
			arc = Arc.Unset;
			return false;
		}
		a.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(a, vector3d);
		vector3d2.Unitize();
		Vector3d b2 = new Vector3d(bendPoint - basePoint);
		b2.Unitize();
		Vector3d vector3d3 = Vector3d.CrossProduct(a, b2);
		vector3d3.Unitize();
		double a2 = CSValueHandleHelper.AngleBetweenVectors(vector3d3, vector3d2);
		double num = (bendPoint - basePoint).Length / 2.0 / Math.Sin(a2);
		Point3d pointOnInterior = basePoint + vector3d2 * num - vector3d3 * num;
		arc = new Arc(basePoint, pointOnInterior, bendPoint);
		return true;
	}

	private bool GetMirrorArc(out Arc arc)
	{
		Point3d sourcePoint;
		Point3d bendPoint;
		if (_bendMode == 2)
		{
			sourcePoint = _basePoint - (_endPoint - _basePoint);
			bendPoint = _basePoint - (_bendPoint - _basePoint);
			return ArcFromPoints(_basePoint, sourcePoint, bendPoint, out arc);
		}
		sourcePoint = _basePoint - (_endPoint - _basePoint);
		bendPoint = new Point3d(_bendPoint);
		Vector3d normalToMirrorPlane = _endPoint - _basePoint;
		normalToMirrorPlane.Unitize();
		Transform xform = Transform.Mirror(_basePoint, normalToMirrorPlane);
		bendPoint.Transform(xform);
		return ArcFromPoints(_basePoint, sourcePoint, bendPoint, out arc);
	}
}
