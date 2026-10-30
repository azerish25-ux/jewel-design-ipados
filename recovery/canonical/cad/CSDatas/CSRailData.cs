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

public abstract class CSRailData : CSXformData, ICSCurve, ICSDocumentObject
{
	protected NurbsCurve _Crv;

	protected CSRailData(Curve crv)
	{
		SetCurve(crv);
	}

	protected CSRailData(CSRailData copy)
		: base(copy)
	{
		SetCurve(copy._Crv);
	}

	public override Curve GetCurve()
	{
		return _Crv.DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		return _Crv?.ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		if (_Crv == null)
		{
			return false;
		}
		return _Crv.HasNurbsForm() > 0;
	}

	public void AddToDocument()
	{
		if (_Crv != null)
		{
			RhinoDoc.ActiveDoc.Objects.AddCurve(_Crv);
		}
	}

	public virtual bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Rhino.Geometry.Transform transform, CSOptionsExport exportOptions)
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
		if (_Crv == null)
		{
			return false;
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject,
			Name = "Rail"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(_Crv, attributes);
		if (guid != Guid.Empty)
		{
			rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		}
		return true;
	}

	public override bool ApplyTransform(Rhino.Geometry.Transform transform)
	{
		if (transform == Rhino.Geometry.Transform.Unset)
		{
			return false;
		}
		if (_Crv == null)
		{
			return false;
		}
		_Crv.MakeDeformable();
		_Crv.Transform(transform);
		return base.ApplyTransform(transform);
	}

	public override bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public override bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		if (!displayOptions.DoShowCurves && !displayOptions.IsMouseDown && !displayOptions.IsSelectionDisplayModeEnabled)
		{
			return false;
		}
		if (_Crv == null)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		System.Drawing.Color diffuse = csDisplayMaterial.Mat.Diffuse;
		displayPipeline.DrawCurve(_Crv, diffuse);
		if (!displayOptions.DoShowStarts)
		{
			return true;
		}
		System.Drawing.Color white = System.Drawing.Color.White;
		displayPipeline.DrawLine(_Crv.PointAtStart, _Crv.PointAtStart + _Crv.TangentAt(_Crv.Domain.Min), white, 1);
		displayPipeline.DrawLine(_Crv.PointAtStart, _Crv.PointAtStart + Vector3d.ZAxis, white, 1);
		return true;
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		return TryGetBoundingBox(_Crv, doAccurate: false, doGrow: false, Rhino.Geometry.Transform.Unset, ref boundingBox);
	}

	public override NurbsCurve GetCurveSegment(int segmentNumber, int segmentCount, bool fromStart)
	{
		if (segmentCount == GetCurveSegmentCount(fromStart))
		{
			return GetCurveSegment(segmentNumber, fromStart);
		}
		return new CSOnGeometryData(_Crv).GetCurveSegment(segmentNumber, segmentCount, fromStart);
	}

	public override GeometryBase GetGeometry()
	{
		return _Crv.Duplicate();
	}

	public override bool GetPoint(out Point3d point)
	{
		point = Point3d.Unset;
		if (_Crv == null)
		{
			return false;
		}
		point = _Crv.PointAtStart;
		return true;
	}

	public override PointCollection GetPolyline(double precision)
	{
		PointCollection pointCollection = null;
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
			pointCollection = new PointCollection(polyline.Count);
			foreach (Point3d item2 in polyline)
			{
				pointCollection.Add(new System.Windows.Point(item2.X, 25.0 - item2.Y));
			}
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
		return TryGetBoundingBox(_Crv, doAccurate: true, doGrow, transform, ref boundingBox);
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_Crv == null)
		{
			return false;
		}
		if (!_Crv.GetUserStrings().AllKeys.Contains(key))
		{
			return false;
		}
		value = _Crv.GetUserString(key);
		return true;
	}

	public override bool HasPoint()
	{
		return _Crv != null;
	}

	public bool IsPlanar(ref bool isPlanar)
	{
		if (_Crv == null)
		{
			return false;
		}
		isPlanar = _Crv.IsPlanar();
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (_Crv == null)
		{
			return false;
		}
		string[] allKeys = _Crv.GetUserStrings().AllKeys;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = allKeys;
		foreach (string key in array)
		{
			string userString = _Crv.GetUserString(key);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(key))
			{
				dictionary.Add(key, userString);
			}
		}
		_Crv.MakeDeformable();
		spaceMorph.Tolerance = 0.05;
		bool result = spaceMorph.Morph(_Crv);
		_Crv.GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			_Crv.SetUserString(item.Key, item.Value);
		}
		return result;
	}

	public override string Report()
	{
		return "RingRail";
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			try
			{
				_Crv?.Dispose();
				_Crv = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		Image image = base.GetImage(sizeX, sizeY, color, background);
		if (image != null)
		{
			return image;
		}
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
				PolylineCurve polylineCurve = nurbsCurve.ToPolyline(20, 5, 5.0, 1.0, 1.0, 0.1, 0.1, 1.0, keepStartPoint: false);
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

	private void SetCurve(Curve crv)
	{
		if (crv == null)
		{
			_Crv = null;
			return;
		}
		_Crv = crv.ToNurbsCurve();
		string[] allKeys = crv.GetUserStrings().AllKeys;
		foreach (string key in allKeys)
		{
			string userString = crv.GetUserString(key);
			if (!string.IsNullOrEmpty(userString))
			{
				_Crv.SetUserString(key, userString);
			}
		}
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

	~CSRailData()
	{
		Dispose(disposing: false);
	}
}
