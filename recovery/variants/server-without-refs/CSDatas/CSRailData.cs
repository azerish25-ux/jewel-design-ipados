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

public abstract class CSRailData : CSXformData, ICSCurve, ICSDocumentObject
{
	protected NurbsCurve _Crv;

	protected CSRailData(Curve crv)
	{
		SetCurve(crv);
	}

	protected CSRailData(CSRailData copy)
		: base((CSXformData)copy)
	{
		SetCurve((Curve)(object)copy._Crv);
	}

	public override Curve GetCurve()
	{
		return ((Curve)_Crv).DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		NurbsCurve crv = _Crv;
		if (crv == null)
		{
			return null;
		}
		return ((Curve)crv).ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		if (_Crv == null)
		{
			return false;
		}
		return ((Curve)_Crv).HasNurbsForm() > 0;
	}

	public void AddToDocument()
	{
		if (_Crv != null)
		{
			RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_Crv);
		}
	}

	public virtual bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
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
		if (_Crv == null)
		{
			return false;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1,
			Name = "Rail"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_Crv, val);
		if (guid != Guid.Empty)
		{
			rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		}
		return true;
	}

	public override bool ApplyTransform(Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (transform == Transform.Unset)
		{
			return false;
		}
		if (_Crv == null)
		{
			return false;
		}
		((GeometryBase)_Crv).MakeDeformable();
		((GeometryBase)_Crv).Transform(transform);
		return base.ApplyTransform(transform);
	}

	public override bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public override bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
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
		displayPipeline.DrawCurve((Curve)(object)_Crv, diffuse);
		if (!displayOptions.DoShowStarts)
		{
			return true;
		}
		System.Drawing.Color white = System.Drawing.Color.White;
		Point3d pointAtStart = ((Curve)_Crv).PointAtStart;
		Point3d pointAtStart2 = ((Curve)_Crv).PointAtStart;
		NurbsCurve crv = _Crv;
		Interval domain = ((Curve)_Crv).Domain;
		displayPipeline.DrawLine(pointAtStart, pointAtStart2 + ((Curve)crv).TangentAt(((Interval)(ref domain)).Min), white, 1);
		displayPipeline.DrawLine(((Curve)_Crv).PointAtStart, ((Curve)_Crv).PointAtStart + Vector3d.ZAxis, white, 1);
		return true;
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox((GeometryBase)(object)_Crv, doAccurate: false, doGrow: false, Transform.Unset, ref boundingBox);
	}

	public override NurbsCurve GetCurveSegment(int segmentNumber, int segmentCount, bool fromStart)
	{
		if (segmentCount == GetCurveSegmentCount(fromStart))
		{
			return GetCurveSegment(segmentNumber, fromStart);
		}
		return new CSOnGeometryData((GeometryBase)(object)_Crv).GetCurveSegment(segmentNumber, segmentCount, fromStart);
	}

	public override GeometryBase GetGeometry()
	{
		return ((GeometryBase)_Crv).Duplicate();
	}

	public override bool GetPoint(out Point3d point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		point = Point3d.Unset;
		if (_Crv == null)
		{
			return false;
		}
		point = ((Curve)_Crv).PointAtStart;
		return true;
	}

	public override PointCollection GetPolyline(double precision)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		PointCollection pointCollection = null;
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
			pointCollection = new PointCollection(((RhinoList<Point3d>)(object)val5).Count);
			foreach (Point3d item in (RhinoList<Point3d>)(object)val5)
			{
				Point3d current = item;
				pointCollection.Add(new System.Windows.Point(((Point3d)(ref current)).X, 25.0 - ((Point3d)(ref current)).Y));
			}
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
		return TryGetBoundingBox((GeometryBase)(object)_Crv, doAccurate: true, doGrow, transform, ref boundingBox);
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_Crv == null)
		{
			return false;
		}
		if (!((GeometryBase)_Crv).GetUserStrings().AllKeys.Contains(key))
		{
			return false;
		}
		value = ((GeometryBase)_Crv).GetUserString(key);
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
		isPlanar = ((Curve)_Crv).IsPlanar();
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (_Crv == null)
		{
			return false;
		}
		string[] allKeys = ((GeometryBase)_Crv).GetUserStrings().AllKeys;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = allKeys;
		foreach (string text in array)
		{
			string userString = ((GeometryBase)_Crv).GetUserString(text);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(text))
			{
				dictionary.Add(text, userString);
			}
		}
		((GeometryBase)_Crv).MakeDeformable();
		spaceMorph.Tolerance = 0.05;
		bool result = spaceMorph.Morph((GeometryBase)(object)_Crv);
		((GeometryBase)_Crv).GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			((GeometryBase)_Crv).SetUserString(item.Key, item.Value);
		}
		return result;
	}

	public override string Report()
	{
		return "RingRail";
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		try
		{
			NurbsCurve crv = _Crv;
			if (crv != null)
			{
				((CommonObject)crv).Dispose();
			}
			_Crv = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
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
				PolylineCurve val3 = ((Curve)nurbsCurve).ToPolyline(20, 5, 5.0, 1.0, 1.0, 0.1, 0.1, 1.0, false);
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

	private void SetCurve(Curve crv)
	{
		if (crv == null)
		{
			_Crv = null;
			return;
		}
		_Crv = crv.ToNurbsCurve();
		string[] allKeys = ((GeometryBase)crv).GetUserStrings().AllKeys;
		foreach (string text in allKeys)
		{
			string userString = ((GeometryBase)crv).GetUserString(text);
			if (!string.IsNullOrEmpty(userString))
			{
				((GeometryBase)_Crv).SetUserString(text, userString);
			}
		}
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

	~CSRailData()
	{
		Dispose(disposing: false);
	}
}
