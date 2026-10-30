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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _basePoint;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = value - _basePoint;
			_basePoint = new Point3d(value);
			_endPoint += val;
			_bendPoint += val;
		}
	}

	public Point3d EndPoint
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _endPoint;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_endPoint = new Point3d(value);
		}
	}

	public Point3d BendPoint
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _bendPoint;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_bendPoint = new Point3d(value);
		}
	}

	public CSBendPoints(Point3d basePoint, Point3d endPoint, Point3d bendPoint, int bendMode)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_lastBasePoint = new Point3d(_basePoint);
		_lastEndPoint = new Point3d(_endPoint);
		_lastBendPoint = new Point3d(_bendPoint);
		_lastBendMode = _bendMode;
	}

	public Arc GetArc()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return ArcFromPoints(_basePoint, _endPoint, _bendPoint);
	}

	public Arc GetMirrorArc()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Point3d bendPoint = default(Point3d);
		Point3d sourcePoint;
		if (_bendMode == 2)
		{
			sourcePoint = _basePoint - (_endPoint - _basePoint);
			bendPoint = _basePoint - (_bendPoint - _basePoint);
			return ArcFromPoints(_basePoint, sourcePoint, bendPoint);
		}
		sourcePoint = _basePoint - (_endPoint - _basePoint);
		((Point3d)(ref bendPoint))._002Ector(_bendPoint);
		Vector3d val = _endPoint - _basePoint;
		((Vector3d)(ref val)).Unitize();
		Transform val2 = Transform.Mirror(_basePoint, val);
		((Point3d)(ref bendPoint)).Transform(val2);
		return ArcFromPoints(_basePoint, sourcePoint, bendPoint);
	}

	public bool HasChanged()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = _basePoint - _lastBasePoint;
		if (!(((Vector3d)(ref val)).Length > 0.001))
		{
			val = _endPoint - _lastEndPoint;
			if (!(((Vector3d)(ref val)).Length > 0.001))
			{
				val = _bendPoint - _lastBendPoint;
				if (!(((Vector3d)(ref val)).Length > 0.001))
				{
					return _bendMode != _lastBendMode;
				}
			}
		}
		return true;
	}

	public bool IsBending()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector3d inputVector = _endPoint - _basePoint;
		Vector3d inputVector2 = _bendPoint - _basePoint;
		return CSValueHandleHelper.AngleBetweenVectors(inputVector, inputVector2) > Math.PI / 180.0;
	}

	public bool Read(JToken archive)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		return new Point3d((double)archive[id + "X"], (double)archive[id + "Y"], (double)archive[id + "Z"]);
	}

	public void Write(ref OnBinaryArchive archive)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_basePoint));
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_endPoint));
		archive.WritePoint((IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(_bendPoint));
		archive.WriteInt(_bendMode);
	}

	public JObject WriteJson()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		JObject jObject = new JObject();
		jObject.Add(new JObject("BasePoint", _basePoint));
		jObject.Add(new JObject("EndPoint", _endPoint));
		jObject.Add(new JObject("BendPoint", _bendPoint));
		jObject.Add(new JObject("BendMode", _bendMode));
		return jObject;
	}

	private static Arc ArcFromPoints(Point3d basePoint, Point3d sourcePoint, Point3d bendPoint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(sourcePoint - basePoint);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(bendPoint - basePoint);
		((Vector3d)(ref val2)).Unitize();
		Vector3d val3 = Vector3d.CrossProduct(val, val2);
		((Vector3d)(ref val3)).Unitize();
		Vector3d val4 = Vector3d.CrossProduct(val3, val);
		((Vector3d)(ref val4)).Unitize();
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(bendPoint - basePoint);
		((Vector3d)(ref val5)).Unitize();
		Vector3d val6 = Vector3d.CrossProduct(val3, val5);
		((Vector3d)(ref val6)).Unitize();
		double a = CSValueHandleHelper.AngleBetweenVectors(val6, val4);
		Vector3d val7 = bendPoint - basePoint;
		double num = ((Vector3d)(ref val7)).Length / 2.0 / Math.Sin(a);
		Point3d val8 = basePoint + val4 * num - val6 * num;
		return new Arc(basePoint, val8, bendPoint);
	}
}
