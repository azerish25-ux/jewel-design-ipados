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
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSProfileData : CSData, ICSDisplayObject, ICSCurve, ICSDocumentObject, ICSPoint, ICSTransform, ICSTransformable
{
	private readonly List<double> _parameters;

	private NurbsCurve _crv;

	private List<NurbsCurve> _segmentBuffer;

	private Rhino.Geometry.Transform _transform;

	public string Name { get; }

	private CSProfileData(Curve crv, string name)
		: this(crv, null, Rhino.Geometry.Transform.Identity, name)
	{
	}

	public CSProfileData(Curve crv, IEnumerable<double> parameters, Rhino.Geometry.Transform transform, string name)
	{
		if (crv == null)
		{
			_crv = null;
		}
		else
		{
			_crv = crv.ToNurbsCurve();
			string[] allKeys = crv.GetUserStrings().AllKeys;
			foreach (string key in allKeys)
			{
				string userString = crv.GetUserString(key);
				if (!string.IsNullOrEmpty(userString))
				{
					_crv.SetUserString(key, userString);
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
		_transform = (transform.IsValid ? transform : Rhino.Geometry.Transform.Identity);
		Name = name;
		BuildSegmentBuffer();
	}

	private CSProfileData(CSProfileData copy)
		: base(copy)
	{
		if (copy._crv == null)
		{
			_crv = null;
		}
		else
		{
			_crv = copy._crv.ToNurbsCurve();
			string[] allKeys = copy._crv.GetUserStrings().AllKeys;
			foreach (string key in allKeys)
			{
				string userString = copy._crv.GetUserString(key);
				if (!string.IsNullOrEmpty(userString))
				{
					_crv.SetUserString(key, userString);
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
		_transform = ((copy._transform == Rhino.Geometry.Transform.Unset) ? Rhino.Geometry.Transform.Identity : copy._transform);
		Name = copy.Name;
		_segmentBuffer = new List<NurbsCurve>();
		if (copy._segmentBuffer == null)
		{
			return;
		}
		foreach (NurbsCurve item in copy._segmentBuffer)
		{
			_segmentBuffer.Add(item.ToNurbsCurve());
		}
	}

	public override Curve GetCurve()
	{
		return _crv.DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		return _crv?.ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		if (_crv == null)
		{
			return false;
		}
		return _crv.HasNurbsForm() > 0;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
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
		System.Drawing.Color color = csDisplayMaterial?.Mat.Diffuse ?? System.Drawing.Color.White;
		if (displayOptions.DoShowPoints)
		{
			foreach (double parameter in _parameters)
			{
				displayPipeline.DrawPoint(_crv.PointAt(parameter), PointStyle.ControlPoint, 2, color);
			}
		}
		displayPipeline.DrawCurve(_crv, color);
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
			RhinoDoc.ActiveDoc.Objects.AddCurve(_crv);
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Rhino.Geometry.Transform transform, CSOptionsExport exportOptions)
	{
		if (transform != Rhino.Geometry.Transform.Unset && transform != Rhino.Geometry.Transform.Identity)
		{
			CSData cSData = Duplicate();
			if (cSData is ICSTransformable iCSTransformable)
			{
				iCSTransformable.ApplyTransform(transform);
			}
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Rhino.Geometry.Transform.Unset, exportOptions);
			}
		}
		if (_crv == null)
		{
			return false;
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(_crv, attributes);
		if (guid != Guid.Empty)
		{
			rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		}
		return true;
	}

	public override bool GetPoint(out Point3d point)
	{
		point = Point3d.Unset;
		if (_crv == null)
		{
			return false;
		}
		point = _crv.PointAtStart;
		return true;
	}

	public bool GetTransform(out Rhino.Geometry.Transform value)
	{
		value = _transform;
		return value.IsValid;
	}

	public bool ApplyTransform(Rhino.Geometry.Transform transform)
	{
		if (transform == Rhino.Geometry.Transform.Unset)
		{
			return false;
		}
		if (_crv == null)
		{
			return false;
		}
		_crv.MakeDeformable();
		_crv.Transform(transform);
		if (_segmentBuffer != null)
		{
			foreach (NurbsCurve item in _segmentBuffer)
			{
				item?.Transform(transform);
			}
		}
		_transform = ((!_transform.IsValid) ? transform : (transform * _transform));
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSProfileData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		return TryGetBoundingBox(_crv, doAccurate: false, doGrow: false, Rhino.Geometry.Transform.Unset, ref boundingBox);
	}

	public static CSProfileData GetCSProfile(Curve crv, string name)
	{
		return new CSProfileData(crv.ToNurbsCurve(), name);
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
		return new CSOnGeometryData(_crv).GetCurveSegment(segmentNumber, segmentCount, fromStart);
	}

	public override GeometryBase GetGeometry()
	{
		return _crv.Duplicate();
	}

	public override PointCollection GetPolyline(double precision)
	{
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		Rhino.Geometry.Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
		nurbsCurve.Transform(objectDisplayXform);
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = 0.0;
		double num4 = 0.0;
		if (boundingBox.Min.X < 0.0)
		{
			num3 -= boundingBox.Min.X;
		}
		if (boundingBox.Min.Y < 0.0)
		{
			num4 -= boundingBox.Min.Y;
		}
		double num5 = ((!(num > num2)) ? (20.0 / num2) : (20.0 / num));
		double num6 = (20.0 - num * num5) / 2.0;
		double num7 = (20.0 - num2 * num5) / 2.0;
		Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Translation(num3, num4, 0.0);
		Rhino.Geometry.Transform transform2 = Rhino.Geometry.Transform.Scale(Plane.WorldXY, num5, num5, num5);
		Rhino.Geometry.Transform transform3 = Rhino.Geometry.Transform.Translation(num6 + 2.5, num7 + 2.5, 0.0);
		nurbsCurve.Transform(transform3 * transform2 * transform);
		Polyline polyline = new Polyline();
		if (precision >= 1.0)
		{
			Interval domain = nurbsCurve.Domain;
			for (double num8 = 0.0; num8 <= 50.0; num8 += 1.0)
			{
				polyline.Add(nurbsCurve.PointAt(domain.ParameterAt(num8 / 50.0)));
			}
		}
		else
		{
			PolylineCurve polylineCurve = nurbsCurve.ToPolyline(5, 5, 3.14, 1.0, 1.0, precision, precision, precision * 20.0, keepStartPoint: false);
			if (polylineCurve != null)
			{
				for (int i = 0; i < polylineCurve.PointCount; i++)
				{
					Point3d item = polylineCurve.Point(i);
					polyline.Add(item);
				}
			}
		}
		PointCollection pointCollection = new PointCollection(polyline.Count);
		foreach (Point3d item2 in polyline)
		{
			pointCollection.Add(new System.Windows.Point(item2.X, 25.0 - item2.Y));
		}
		return pointCollection;
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		return GetTightBoundingBox(ref boundingBox, doGrow: false, Rhino.Geometry.Transform.Unset);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		return GetTightBoundingBox(ref boundingBox, doGrow, Rhino.Geometry.Transform.Unset);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Rhino.Geometry.Transform transform)
	{
		return TryGetBoundingBox(_crv, doAccurate: true, doGrow, transform, ref boundingBox);
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_crv == null)
		{
			return false;
		}
		if (!_crv.GetUserStrings().AllKeys.Contains(key))
		{
			return false;
		}
		value = _crv.GetUserString(key);
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (_crv == null)
		{
			return false;
		}
		string[] allKeys = _crv.GetUserStrings().AllKeys;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = allKeys;
		foreach (string key in array)
		{
			string userString = _crv.GetUserString(key);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(key))
			{
				dictionary.Add(key, userString);
			}
		}
		_crv.MakeDeformable();
		spaceMorph.Tolerance = 0.05;
		bool result = spaceMorph.Morph(_crv);
		_crv.GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			_crv.SetUserString(item.Key, item.Value);
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
				_crv.Dispose();
			}
			if (_segmentBuffer != null)
			{
				foreach (NurbsCurve item in _segmentBuffer)
				{
					item?.Dispose();
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
			return _segmentBuffer[segmentNumber].ToNurbsCurve();
		}
		if (_crv.IsClosed && !fromStart && segmentNumber == 0)
		{
			if (_parameters.Count <= 1)
			{
				return _crv.ToNurbsCurve();
			}
			double t = _parameters[_parameters.Count - 1];
			double t2 = _parameters[0];
			Point3d testPoint = _crv.PointAt(t2);
			Curve curve = _crv.DuplicateCurve();
			curve.ChangeClosedCurveSeam(t);
			if (!curve.ClosestPoint(testPoint, out var t3))
			{
				t3 = curve.Domain.Min;
			}
			return (curve.Trim(new Interval(curve.Domain.Min, t3)) ?? curve).ToNurbsCurve();
		}
		double t4 = _crv.Domain.Min;
		double t5 = _crv.Domain.Max;
		int num = segmentNumber - 1;
		if (num >= 0 && num < _parameters.Count)
		{
			t4 = _parameters[num];
		}
		if (segmentNumber >= 0 && segmentNumber < _parameters.Count)
		{
			t5 = _parameters[segmentNumber];
		}
		Curve curve2 = _crv.DuplicateCurve();
		return (curve2.Trim(new Interval(t4, t5)) ?? curve2).ToNurbsCurve();
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
		if (_crv.IsClosed && !fromStart)
		{
			return _parameters.Count;
		}
		return _parameters.Count + 1;
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		Bitmap bitmap = new Bitmap(sizeX, sizeY);
		Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(background);
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve != null)
		{
			BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
			Rhino.Geometry.Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
			nurbsCurve.Transform(objectDisplayXform);
			double num = boundingBox.Max.X - boundingBox.Min.X;
			double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
			double num3 = 0.0;
			double num4 = 0.0;
			if (boundingBox.Min.X < 0.0)
			{
				num3 -= boundingBox.Min.X;
			}
			if (boundingBox.Min.Y < 0.0)
			{
				num4 -= boundingBox.Min.Y;
			}
			double num5 = (double)sizeX * 0.8;
			double num6 = (double)sizeY * 0.8;
			double num7 = ((!(num5 < num6)) ? (((double)sizeY - num6) / 2.0) : (((double)sizeX - num5) / 2.0));
			double num8 = ((num > num2) ? ((!(num5 > num6)) ? (num6 / num) : (num5 / num)) : ((!(num5 > num6)) ? (num6 / num2) : (num5 / num2)));
			double num9 = (num5 - num * num8) / 2.0;
			double num10 = (num6 - num2 * num8) / 2.0;
			graphics.TranslateTransform(0f, sizeY);
			graphics.ScaleTransform(1f, -1f);
			Polyline polyline = new Polyline();
			if (sizeX < 100 && sizeY < 100)
			{
				Interval domain = nurbsCurve.Domain;
				for (double num11 = 0.0; num11 <= 50.0; num11 += 1.0)
				{
					polyline.Add(nurbsCurve.PointAt(domain.ParameterAt(num11 / 50.0)));
				}
			}
			else
			{
				PolylineCurve polylineCurve = nurbsCurve.ToPolyline(20, 5, 3.14, 1.0, 1.0, 0.1, 0.1, 1.0, keepStartPoint: false);
				if (polylineCurve != null)
				{
					for (int i = 0; i < polylineCurve.PointCount; i++)
					{
						Point3d item = polylineCurve.Point(i);
						polyline.Add(item);
					}
				}
			}
			System.Drawing.Pen pen = new System.Drawing.Pen(color, (float)(1.0 * (0.0 - num8)));
			for (int j = 1; j < polyline.Count; j++)
			{
				Point3d point3d = polyline[j];
				Point3d point3d2 = polyline[j - 1];
				float x = (float)((point3d.X + num3) * num8 + num9 + num7);
				float y = (float)((point3d.Y + num4) * num8 + num10 + num7);
				float x2 = (float)((point3d2.X + num3) * num8 + num9 + num7);
				float y2 = (float)((point3d2.Y + num4) * num8 + num10 + num7);
				graphics.DrawLine(pen, x, y, x2, y2);
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

	private static bool TryGetBoundingBox(GeometryBase geometryBase, bool doAccurate, bool doGrow, Rhino.Geometry.Transform transform, ref BoundingBox boundingBox)
	{
		if (geometryBase == null)
		{
			return false;
		}
		BoundingBox boundingBox2 = ((transform.IsValid && !transform.Equals(Rhino.Geometry.Transform.Identity)) ? geometryBase.GetBoundingBox(transform) : geometryBase.GetBoundingBox(doAccurate));
		if (!boundingBox2.MakeValid())
		{
			return false;
		}
		if (boundingBox.IsValid && doGrow)
		{
			boundingBox.Union(boundingBox2);
		}
		else
		{
			boundingBox = boundingBox2;
		}
		return true;
	}

	~CSProfileData()
	{
		Dispose(disposing: false);
	}
}
