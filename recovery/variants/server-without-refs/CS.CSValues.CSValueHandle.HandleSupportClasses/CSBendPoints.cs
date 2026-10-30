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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _basePoint;
		}
		set
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(value - _basePoint);
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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = value - _basePoint;
			double length = ((Vector3d)(ref val)).Length;
			val = _endPoint - _basePoint;
			double num = length / ((Vector3d)(ref val)).Length;
			Transform val2 = Transform.Rotation(_endPoint - _basePoint, value - _basePoint, _basePoint);
			((Point3d)(ref _bendPoint)).Transform(val2);
			val2 = Transform.Scale(_basePoint, num);
			((Point3d)(ref _bendPoint)).Transform(val2);
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

	public CSBendPoints()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		_basePoint = new Point3d(0.0, 0.0, 0.0);
		_endPoint = new Point3d(0.0, 0.0, 0.0);
		_bendPoint = new Point3d(0.0, 0.0, 0.0);
		_lastBendPoint = new Point3d(0.0, 0.0, 0.0);
		_lastEndPoint = new Point3d(0.0, 0.0, 0.0);
		_lastBasePoint = new Point3d(0.0, 0.0, 0.0);
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

	public bool GetArc(out Arc arc)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return ArcFromPoints(_basePoint, _endPoint, _bendPoint, out arc);
	}

	public NurbsCurve GetDisplayCurve()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (GetArc(out var arc))
		{
			return ((Arc)(ref arc)).ToNurbsCurve();
		}
		return ((Curve)new LineCurve(_basePoint, _bendPoint)).ToNurbsCurve();
	}

	public NurbsCurve GetDisplayMirrorCurve()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (GetMirrorArc(out var arc))
		{
			return ((Arc)(ref arc)).ToNurbsCurve();
		}
		return ((Curve)new LineCurve(_basePoint, _basePoint - (_bendPoint - _basePoint))).ToNurbsCurve();
	}

	public bool HasChanged()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (IsBending())
		{
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
		return false;
	}

	public bool IsAllZero()
	{
		if (Math.Abs(((Point3d)(ref _basePoint)).X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _basePoint)).Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _basePoint)).Z) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _endPoint)).X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _endPoint)).Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _endPoint)).Z) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _bendPoint)).X) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _bendPoint)).Y) > 0.001)
		{
			return false;
		}
		if (Math.Abs(((Point3d)(ref _bendPoint)).Z) > 0.001)
		{
			return false;
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

	public static CSBendPoints NewBp(CSBendPoints copy)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		On3dPoint value = new On3dPoint();
		switch (version)
		{
		case 1:
		{
			archive.ReadPoint(ref value);
			_basePoint = CSConvert.On3dPointCopyToPoint3d(value);
			archive.ReadPoint(ref value);
			_endPoint = CSConvert.On3dPointCopyToPoint3d(value);
			archive.ReadPoint(ref value);
			_bendPoint = CSConvert.On3dPointCopyToPoint3d(value);
			byte[] array = new byte[1];
			archive.ReadBytes(ref array);
			_bendMode = array[0];
			break;
		}
		case 2:
		case 3:
		case 4:
		case 5:
			archive.ReadPoint(ref value);
			_basePoint = CSConvert.On3dPointCopyToPoint3d(value);
			archive.ReadPoint(ref value);
			_endPoint = CSConvert.On3dPointCopyToPoint3d(value);
			archive.ReadPoint(ref value);
			_bendPoint = CSConvert.On3dPointCopyToPoint3d(value);
			archive.ReadInt(ref _bendMode);
			break;
		}
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(sourcePoint - basePoint);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(bendPoint - basePoint);
		((Vector3d)(ref val2)).Unitize();
		Vector3d val3 = Vector3d.CrossProduct(val, val2);
		if (Math.Abs(((Vector3d)(ref val3)).Length) < 0.001)
		{
			arc = Arc.Unset;
			return false;
		}
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
		arc = new Arc(basePoint, val8, bendPoint);
		return true;
	}

	private bool GetMirrorArc(out Arc arc)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d bendPoint = default(Point3d);
		Point3d sourcePoint;
		if (_bendMode == 2)
		{
			sourcePoint = _basePoint - (_endPoint - _basePoint);
			bendPoint = _basePoint - (_bendPoint - _basePoint);
			return ArcFromPoints(_basePoint, sourcePoint, bendPoint, out arc);
		}
		sourcePoint = _basePoint - (_endPoint - _basePoint);
		((Point3d)(ref bendPoint))._002Ector(_bendPoint);
		Vector3d val = _endPoint - _basePoint;
		((Vector3d)(ref val)).Unitize();
		Transform val2 = Transform.Mirror(_basePoint, val);
		((Point3d)(ref bendPoint)).Transform(val2);
		return ArcFromPoints(_basePoint, sourcePoint, bendPoint, out arc);
	}
}
