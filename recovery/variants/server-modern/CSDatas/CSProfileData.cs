using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino;
using Rhino.Collections;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSDatas;

public sealed class CSProfileData : CSData, ICSDisplayObject, ICSCurve, ICSDocumentObject, ICSPoint, ICSTransform, ICSTransformable
{
	private readonly List<double> _parameters;

	private NurbsCurve _crv;

	private List<NurbsCurve> _segmentBuffer;

	private Transform _transform;

	public string Name { get; }

	private CSProfileData(Curve crv, string name)
		: this(crv, null, Transform.Identity, name)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public CSProfileData(Curve crv, IEnumerable<double> parameters, Transform transform, string name)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (crv == null)
		{
			_crv = null;
		}
		else
		{
			_crv = crv.ToNurbsCurve();
			string[] allKeys = ((GeometryBase)crv).GetUserStrings().AllKeys;
			foreach (string text in allKeys)
			{
				string userString = ((GeometryBase)crv).GetUserString(text);
				if (!string.IsNullOrEmpty(userString))
				{
					((GeometryBase)_crv).SetUserString(text, userString);
				}
			}
		}
		if (parameters == null)
		{
			_parameters = new List<double>();
		}
		else
		{
			_parameters = new List<double>(parameters);
			_parameters.Sort();
		}
		_transform = (((Transform)(ref transform)).IsValid ? transform : Transform.Identity);
		Name = name;
		BuildSegmentBuffer();
	}

	private CSProfileData(CSProfileData copy)
		: base(copy)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (copy._crv == null)
		{
			_crv = null;
		}
		else
		{
			_crv = ((Curve)copy._crv).ToNurbsCurve();
			string[] allKeys = ((GeometryBase)copy._crv).GetUserStrings().AllKeys;
			foreach (string text in allKeys)
			{
				string userString = ((GeometryBase)copy._crv).GetUserString(text);
				if (!string.IsNullOrEmpty(userString))
				{
					((GeometryBase)_crv).SetUserString(text, userString);
				}
			}
		}
		if (copy._parameters == null)
		{
			_parameters = new List<double>();
		}
		else
		{
			_parameters = new List<double>(copy._parameters);
			_parameters.Sort();
		}
		_transform = ((copy._transform == Transform.Unset) ? Transform.Identity : copy._transform);
		Name = copy.Name;
		_segmentBuffer = new List<NurbsCurve>();
		if (copy._segmentBuffer == null)
		{
			return;
		}
		foreach (NurbsCurve item in copy._segmentBuffer)
		{
			_segmentBuffer.Add(((Curve)item).ToNurbsCurve());
		}
	}

	public override Curve GetCurve()
	{
		return ((Curve)_crv).DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		NurbsCurve crv = _crv;
		if (crv == null)
		{
			return null;
		}
		return ((Curve)crv).ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		if (_crv == null)
		{
			return false;
		}
		return ((Curve)_crv).HasNurbsForm() > 0;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (!displayOptions.DoShowCurves && !displayOptions.IsMouseDown && !displayOptions.IsSelectionDisplayModeEnabled)
		{
			return false;
		}
		if (_crv == null)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		System.Drawing.Color color = ((csDisplayMaterial != null) ? csDisplayMaterial.Mat.Diffuse : System.Drawing.Color.White);
		if (displayOptions.DoShowPoints)
		{
			foreach (double parameter in _parameters)
			{
				displayPipeline.DrawPoint(((Curve)_crv).PointAt(parameter), (PointStyle)1, 2, color);
			}
		}
		displayPipeline.DrawCurve((Curve)(object)_crv, color);
		return true;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public void AddToDocument()
	{
		if (_crv != null)
		{
			RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_crv);
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			if (cSData is ICSTransformable iCSTransformable)
			{
				iCSTransformable.ApplyTransform(transform);
			}
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Transform.Unset, exportOptions);
			}
		}
		if (_crv == null)
		{
			return false;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_crv, val);
		if (guid != Guid.Empty)
		{
			rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		}
		return true;
	}

	public override bool GetPoint(out Point3d point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		point = Point3d.Unset;
		if (_crv == null)
		{
			return false;
		}
		point = ((Curve)_crv).PointAtStart;
		return true;
	}

	public bool GetTransform(out Transform value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		value = _transform;
		return ((Transform)(ref value)).IsValid;
	}

	public bool ApplyTransform(Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (transform == Transform.Unset)
		{
			return false;
		}
		if (_crv == null)
		{
			return false;
		}
		((GeometryBase)_crv).MakeDeformable();
		((GeometryBase)_crv).Transform(transform);
		if (_segmentBuffer != null)
		{
			foreach (NurbsCurve item in _segmentBuffer)
			{
				if (item != null)
				{
					((GeometryBase)item).Transform(transform);
				}
			}
		}
		_transform = ((!((Transform)(ref _transform)).IsValid) ? transform : (transform * _transform));
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSProfileData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox((GeometryBase)(object)_crv, doAccurate: false, doGrow: false, Transform.Unset, ref boundingBox);
	}

	public static CSProfileData GetCSProfile(Curve crv, string name)
	{
		return new CSProfileData((Curve)(object)crv.ToNurbsCurve(), name);
	}

	public override double GetCurveParameter(int i)
	{
		if (i < _parameters.Count)
		{
			return _parameters[i];
		}
		return 0.0;
	}

	public override int GetCurveParameterCount()
	{
		return _parameters.Count;
	}

	public override NurbsCurve GetCurveSegment(int segmentNumber, int segmentCount, bool fromStart)
	{
		if (segmentCount == GetCurveSegmentCount(fromStart))
		{
			return GetCurveSegment(segmentNumber, fromStart);
		}
		return new CSOnGeometryData((GeometryBase)(object)_crv).GetCurveSegment(segmentNumber, segmentCount, fromStart);
	}

	public override GeometryBase GetGeometry()
	{
		return ((GeometryBase)_crv).Duplicate();
	}

	public override PointCollection GetPolyline(double precision)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
		((GeometryBase)nurbsCurve).Transform(objectDisplayXform);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		double num3 = 0.0;
		double num4 = 0.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val)).X < 0.0)
		{
			double num5 = num3;
			val = ((BoundingBox)(ref boundingBox)).Min;
			num3 = num5 - ((Point3d)(ref val)).X;
		}
		val = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val)).Y < 0.0)
		{
			double num6 = num4;
			val = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num6 - ((Point3d)(ref val)).Y;
		}
		double num7 = ((!(num > num2)) ? (20.0 / num2) : (20.0 / num));
		double num8 = (20.0 - num * num7) / 2.0;
		double num9 = (20.0 - num2 * num7) / 2.0;
		Transform val2 = Transform.Translation(num3, num4, 0.0);
		Transform val3 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
		Transform val4 = Transform.Translation(num8 + 2.5, num9 + 2.5, 0.0);
		((GeometryBase)nurbsCurve).Transform(val4 * val3 * val2);
		Polyline val5 = new Polyline();
		if (precision >= 1.0)
		{
			Interval domain = ((Curve)nurbsCurve).Domain;
			for (double num10 = 0.0; num10 <= 50.0; num10 += 1.0)
			{
				((RhinoList<Point3d>)(object)val5).Add(((Curve)nurbsCurve).PointAt(((Interval)(ref domain)).ParameterAt(num10 / 50.0)));
			}
		}
		else
		{
			PolylineCurve val6 = ((Curve)nurbsCurve).ToPolyline(5, 5, 3.14, 1.0, 1.0, precision, precision, precision * 20.0, false);
			if (val6 != null)
			{
				for (int i = 0; i < val6.PointCount; i++)
				{
					Point3d val7 = val6.Point(i);
					((RhinoList<Point3d>)(object)val5).Add(val7);
				}
			}
		}
		PointCollection pointCollection = new PointCollection(((RhinoList<Point3d>)(object)val5).Count);
		foreach (Point3d item in (RhinoList<Point3d>)(object)val5)
		{
			Point3d current = item;
			pointCollection.Add(new System.Windows.Point(((Point3d)(ref current)).X, 25.0 - ((Point3d)(ref current)).Y));
		}
		return pointCollection;
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow: false, Transform.Unset);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow, Transform.Unset);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox((GeometryBase)(object)_crv, doAccurate: true, doGrow, transform, ref boundingBox);
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_crv == null)
		{
			return false;
		}
		if (!((GeometryBase)_crv).GetUserStrings().AllKeys.Contains(key))
		{
			return false;
		}
		value = ((GeometryBase)_crv).GetUserString(key);
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (_crv == null)
		{
			return false;
		}
		string[] allKeys = ((GeometryBase)_crv).GetUserStrings().AllKeys;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = allKeys;
		foreach (string text in array)
		{
			string userString = ((GeometryBase)_crv).GetUserString(text);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(text))
			{
				dictionary.Add(text, userString);
			}
		}
		((GeometryBase)_crv).MakeDeformable();
		spaceMorph.Tolerance = 0.05;
		bool result = spaceMorph.Morph((GeometryBase)(object)_crv);
		((GeometryBase)_crv).GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			((GeometryBase)_crv).SetUserString(item.Key, item.Value);
		}
		return result;
	}

	public void SetCurveParameter(int i, double v)
	{
		if (i < _parameters.Count)
		{
			_parameters[i] = v;
		}
	}

	public void SetCurveParameterCount(int v)
	{
		while (_parameters.Count < v)
		{
			_parameters.Add(0.0);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		try
		{
			if (_crv != null)
			{
				((CommonObject)_crv).Dispose();
			}
			if (_segmentBuffer != null)
			{
				foreach (NurbsCurve item in _segmentBuffer)
				{
					if (item != null)
					{
						((CommonObject)item).Dispose();
					}
				}
			}
			_crv = null;
			_segmentBuffer = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	protected override NurbsCurve GetCurveSegment(int segmentNumber, bool fromStart)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (_parameters == null)
		{
			return _crv;
		}
		if (_crv == null)
		{
			return null;
		}
		if (_segmentBuffer != null && segmentNumber > 0 && segmentNumber < _segmentBuffer.Count)
		{
			return ((Curve)_segmentBuffer[segmentNumber]).ToNurbsCurve();
		}
		Interval domain;
		if (((Curve)_crv).IsClosed && !fromStart && segmentNumber == 0)
		{
			if (_parameters.Count <= 1)
			{
				return ((Curve)_crv).ToNurbsCurve();
			}
			double num = _parameters[_parameters.Count - 1];
			double num2 = _parameters[0];
			Point3d val = ((Curve)_crv).PointAt(num2);
			Curve val2 = ((Curve)_crv).DuplicateCurve();
			val2.ChangeClosedCurveSeam(num);
			double min = default(double);
			if (!val2.ClosestPoint(val, ref min))
			{
				domain = val2.Domain;
				min = ((Interval)(ref domain)).Min;
			}
			domain = val2.Domain;
			return (val2.Trim(new Interval(((Interval)(ref domain)).Min, min)) ?? val2).ToNurbsCurve();
		}
		domain = ((Curve)_crv).Domain;
		double num3 = ((Interval)(ref domain)).Min;
		domain = ((Curve)_crv).Domain;
		double num4 = ((Interval)(ref domain)).Max;
		int num5 = segmentNumber - 1;
		if (num5 >= 0 && num5 < _parameters.Count)
		{
			num3 = _parameters[num5];
		}
		if (segmentNumber >= 0 && segmentNumber < _parameters.Count)
		{
			num4 = _parameters[segmentNumber];
		}
		Curve val3 = ((Curve)_crv).DuplicateCurve();
		return (val3.Trim(new Interval(num3, num4)) ?? val3).ToNurbsCurve();
	}

	protected override int GetCurveSegmentCount(bool fromStart)
	{
		if (_crv == null)
		{
			return 0;
		}
		if (_parameters == null)
		{
			return 1;
		}
		if (((Curve)_crv).IsClosed && !fromStart)
		{
			return _parameters.Count;
		}
		return _parameters.Count + 1;
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		Bitmap bitmap = new Bitmap(sizeX, sizeY);
		Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(background);
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve != null)
		{
			BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
			((GeometryBase)nurbsCurve).Transform(objectDisplayXform);
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = x - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num2 = y - ((Point3d)(ref val)).Y;
			double num3 = 0.0;
			double num4 = 0.0;
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (((Point3d)(ref val)).X < 0.0)
			{
				double num5 = num3;
				val = ((BoundingBox)(ref boundingBox)).Min;
				num3 = num5 - ((Point3d)(ref val)).X;
			}
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (((Point3d)(ref val)).Y < 0.0)
			{
				double num6 = num4;
				val = ((BoundingBox)(ref boundingBox)).Min;
				num4 = num6 - ((Point3d)(ref val)).Y;
			}
			double num7 = (double)sizeX * 0.8;
			double num8 = (double)sizeY * 0.8;
			double num9 = ((!(num7 < num8)) ? (((double)sizeY - num8) / 2.0) : (((double)sizeX - num7) / 2.0));
			double num10 = ((num > num2) ? ((!(num7 > num8)) ? (num8 / num) : (num7 / num)) : ((!(num7 > num8)) ? (num8 / num2) : (num7 / num2)));
			double num11 = (num7 - num * num10) / 2.0;
			double num12 = (num8 - num2 * num10) / 2.0;
			graphics.TranslateTransform(0f, sizeY);
			graphics.ScaleTransform(1f, -1f);
			Polyline val2 = new Polyline();
			if (sizeX < 100 && sizeY < 100)
			{
				Interval domain = ((Curve)nurbsCurve).Domain;
				for (double num13 = 0.0; num13 <= 50.0; num13 += 1.0)
				{
					((RhinoList<Point3d>)(object)val2).Add(((Curve)nurbsCurve).PointAt(((Interval)(ref domain)).ParameterAt(num13 / 50.0)));
				}
			}
			else
			{
				PolylineCurve val3 = ((Curve)nurbsCurve).ToPolyline(20, 5, 3.14, 1.0, 1.0, 0.1, 0.1, 1.0, false);
				if (val3 != null)
				{
					for (int i = 0; i < val3.PointCount; i++)
					{
						Point3d val4 = val3.Point(i);
						((RhinoList<Point3d>)(object)val2).Add(val4);
					}
				}
			}
			System.Drawing.Pen pen = new System.Drawing.Pen(color, (float)(1.0 * (0.0 - num10)));
			for (int j = 1; j < ((RhinoList<Point3d>)(object)val2).Count; j++)
			{
				Point3d val5 = ((RhinoList<Point3d>)(object)val2)[j];
				Point3d val6 = ((RhinoList<Point3d>)(object)val2)[j - 1];
				float x2 = (float)((((Point3d)(ref val5)).X + num3) * num10 + num11 + num9);
				float y2 = (float)((((Point3d)(ref val5)).Y + num4) * num10 + num12 + num9);
				float x3 = (float)((((Point3d)(ref val6)).X + num3) * num10 + num11 + num9);
				float y3 = (float)((((Point3d)(ref val6)).Y + num4) * num10 + num12 + num9);
				graphics.DrawLine(pen, x2, y2, x3, y3);
			}
		}
		graphics.Flush();
		return bitmap;
	}

	private void BuildSegmentBuffer()
	{
		int curveSegmentCount = GetCurveSegmentCount(fromStart: false);
		List<NurbsCurve> list = new List<NurbsCurve>();
		for (int i = 0; i < curveSegmentCount; i++)
		{
			list.Add(GetCurveSegment(i, fromStart: false));
			if (list[i] == null)
			{
				return;
			}
		}
		_segmentBuffer = list;
	}

	private static bool TryGetBoundingBox(GeometryBase geometryBase, bool doAccurate, bool doGrow, Transform transform, ref BoundingBox boundingBox)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (geometryBase == null)
		{
			return false;
		}
		BoundingBox val = ((((Transform)(ref transform)).IsValid && !((Transform)(ref transform)).Equals(Transform.Identity)) ? geometryBase.GetBoundingBox(transform) : geometryBase.GetBoundingBox(doAccurate));
		if (!((BoundingBox)(ref val)).MakeValid())
		{
			return false;
		}
		if (((BoundingBox)(ref boundingBox)).IsValid && doGrow)
		{
			((BoundingBox)(ref boundingBox)).Union(val);
		}
		else
		{
			boundingBox = val;
		}
		return true;
	}

	~CSProfileData()
	{
		Dispose(disposing: false);
	}
}
