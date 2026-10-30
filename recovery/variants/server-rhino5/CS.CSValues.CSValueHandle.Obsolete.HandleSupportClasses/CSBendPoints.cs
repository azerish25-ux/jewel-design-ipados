using System;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.JSonDoc;
using CSCompatibility;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses;

public sealed class CSBendPoints
{
	public int _bendMode;

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
			Vector3d vector3d = value - _basePoint;
			_basePoint = new Point3d(value);
			_endPoint += vector3d;
			_bendPoint += vector3d;
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

	public Arc GetArc()
	{
		return ArcFromPoints(_basePoint, _endPoint, _bendPoint);
	}

	public Arc GetMirrorArc()
	{
		Point3d sourcePoint;
		Point3d bendPoint;
		if (_bendMode == 2)
		{
			sourcePoint = _basePoint - (_endPoint - _basePoint);
			bendPoint = _basePoint - (_bendPoint - _basePoint);
			return ArcFromPoints(_basePoint, sourcePoint, bendPoint);
		}
		sourcePoint = _basePoint - (_endPoint - _basePoint);
		bendPoint = new Point3d(_bendPoint);
		Vector3d normalToMirrorPlane = _endPoint - _basePoint;
		normalToMirrorPlane.Unitize();
		Transform xform = Transform.Mirror(_basePoint, normalToMirrorPlane);
		bendPoint.Transform(xform);
		return ArcFromPoints(_basePoint, sourcePoint, bendPoint);
	}

	public bool HasChanged()
	{
		if (!((_basePoint - _lastBasePoint).Length > 0.001) && !((_endPoint - _lastEndPoint).Length > 0.001) && !((_bendPoint - _lastBendPoint).Length > 0.001))
		{
			return _bendMode != _lastBendMode;
		}
		return true;
	}

	public bool IsBending()
	{
		Vector3d inputVector = _endPoint - _basePoint;
		Vector3d inputVector2 = _bendPoint - _basePoint;
		return CSValueHandleHelper.AngleBetweenVectors(inputVector, inputVector2) > Math.PI / 180.0;
	}

	public bool Read(JToken archive)
	{
		bool result = true;
		JObject jObject = (JObject)archive;
		if (jObject.ContainsKey("BasePoint"))
		{
			_basePoint = jObject.GetPoint("BasePoint");
		}
		else
		{
			result = false;
		}
		if (jObject.ContainsKey("EndPoint"))
		{
			_endPoint = jObject.GetPoint("EndPoint");
		}
		else
		{
			result = false;
		}
		if (jObject.ContainsKey("BendPoint"))
		{
			_bendPoint = jObject.GetPoint("BendPoint");
		}
		else
		{
			result = false;
		}
		if (jObject.ContainsKey("BendMode"))
		{
			_bendMode = (int)archive["BendMode"];
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void Read(ref OnBinaryArchive archive)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		On3dPoint value = new On3dPoint();
		archive.ReadPoint(ref value);
		_basePoint = CSConvert.On3dPointCopyToPoint3d(value);
		archive.ReadPoint(ref value);
		_endPoint = CSConvert.On3dPointCopyToPoint3d(value);
		archive.ReadPoint(ref value);
		_bendPoint = CSConvert.On3dPointCopyToPoint3d(value);
		archive.ReadInt(ref _bendMode);
	}

	public Point3d ReadPt(string id, JObject archive)
	{
		return new Point3d((double)archive[id + "X"], (double)archive[id + "Y"], (double)archive[id + "Z"]);
	}

	public void Write(ref OnBinaryArchive archive)
	{
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_basePoint));
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_endPoint));
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_bendPoint));
		archive.WriteInt(_bendMode);
	}

	public JObject WriteJson()
	{
		JObject jObject = new JObject();
		jObject.Add(new JObject("BasePoint", _basePoint));
		jObject.Add(new JObject("EndPoint", _endPoint));
		jObject.Add(new JObject("BendPoint", _bendPoint));
		jObject.Add(new JObject("BendMode", _bendMode));
		return jObject;
	}

	private static Arc ArcFromPoints(Point3d basePoint, Point3d sourcePoint, Point3d bendPoint)
	{
		Vector3d vector3d = new Vector3d(sourcePoint - basePoint);
		vector3d.Unitize();
		Vector3d b = new Vector3d(bendPoint - basePoint);
		b.Unitize();
		Vector3d a = Vector3d.CrossProduct(vector3d, b);
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
		return new Arc(basePoint, pointOnInterior, bendPoint);
	}
}
