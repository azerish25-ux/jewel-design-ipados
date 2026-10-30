using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.Shank.French;
using CSDatas;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.FrenchHalo;

[CSBlock("FrenchHaloSet", "{E27E52BF-FCFC-4161-853F-3CC385E94EE3}")]
public sealed class CSFrenchHaloSet : CSBlock
{
	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InFrenchObj;

	public readonly CSInputChannel InFrenchWallWidth;

	public readonly CSInputChannel InProfileHeight;

	public readonly CSInputChannel InCornerInnerScale;

	public readonly CSInputChannel InCornerOuterScale;

	public readonly CSInputChannel InCornerSplitPercent;

	public readonly CSInputChannel InGemsOn;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InCenterGem;

	public readonly CSInputChannel InCenterGemSpacing;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InProductionShow;

	public readonly CSInputChannel InCornerBlend;

	public readonly CSInputChannel InInnerCornerBlend;

	public readonly CSInputChannel InClusterAngle;

	public readonly CSOutputChannel OutFrenchSet;

	public CSFrenchHaloSet(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InFrenchObj = new CSInputChannel(this, ChannelDataType.Brep | ChannelDataType.Mesh, ChannelArrayType.Array);
		InFrenchWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2);
		InCornerInnerScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InCornerOuterScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InCornerSplitPercent = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.45);
		InGemsOn = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProductionShow = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InInnerCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InClusterAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutFrenchSet = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		if (InProductionShow.Boolean)
		{
			return base.Update();
		}
		bool isCamMode = CSApplicationHelper.Application.IsCamMode;
		List<CSGemData> gemList = InSortedGems.GemList;
		if (gemList == null)
		{
			return UpdateMissing("Gems");
		}
		if (gemList.Count <= 0)
		{
			OutFrenchSet.SetData(new CSDataArray());
			return base.Update();
		}
		List<CSChannelGemData> list = new List<CSChannelGemData>();
		foreach (CSGemData item in gemList)
		{
			if (item != null)
			{
				list.Add(new CSChannelGemData(item, null));
			}
		}
		CSUtilGemRails.CreateRails(list);
		double wallWidth = InFrenchWallWidth.Double;
		double num = InProfileHeight.Double;
		double leftCornerScale = InCornerOuterScale.Double;
		double rightCornerScale = InCornerInnerScale.Double;
		double cornerSplitPercent = InCornerSplitPercent.Double;
		bool boolean = InGemsOn.Boolean;
		Curve curve = InProfile.Curve;
		CSGemData gem = InCenterGem.Gem;
		double num2 = InCenterGemSpacing.Double;
		double num3 = InPlacement.Double;
		double num4 = InCornerBlend.Double;
		double value = InInnerCornerBlend.Double;
		double angleRadians = RhinoMath.ToRadians(InClusterAngle.Double);
		CSFrenchGeometry cSFrenchGeometry = null;
		List<Mesh> value3;
		if (InFrenchObj.GetValues(out List<Brep> value2) && value2.Count > 0)
		{
			cSFrenchGeometry = new CSFrenchGeometry(value2, null, "c", isMesh: false, isCamMode);
		}
		else if (InFrenchObj.GetValues(out value3) && value3.Count > 0)
		{
			cSFrenchGeometry = new CSFrenchGeometry(value3, null, "c", isMesh: true, isCamMode);
		}
		if (cSFrenchGeometry == null)
		{
			return UpdateMissing("Parts not found");
		}
		if (cSFrenchGeometry.GTop == null)
		{
			return UpdateMissing("Top mesh part not found");
		}
		if (cSFrenchGeometry.GBase == null)
		{
			return UpdateMissing("Base mesh part not found");
		}
		if (cSFrenchGeometry.GSide.Count == 0)
		{
			return UpdateMissing("Side mesh parts not found");
		}
		CSFrenchGeometry cSFrenchGeometry2 = null;
		List<Mesh> value4;
		if (InFrenchObj.GetValues(out value2) && value2.Count > 0)
		{
			cSFrenchGeometry2 = new CSFrenchGeometry(value2, null, "9", isMesh: false, isCamMode);
		}
		else if (InFrenchObj.GetValues(out value4) && value4.Count > 0)
		{
			cSFrenchGeometry2 = new CSFrenchGeometry(value4, null, "9", isMesh: true, isCamMode);
		}
		if (!cSFrenchGeometry.SetUpGeometry(halo: true, out var bottomSize, out var midSize, out var topSize, out var _, out var ySize))
		{
			return UpdateError("Bounding Box not found");
		}
		double cornerXSize = 1.0;
		double cornerYSize = 1.0;
		if (cSFrenchGeometry2 != null)
		{
			BoundingBox boundingBox = cSFrenchGeometry2.GAll.GetBoundingBox(accurate: true);
			cSFrenchGeometry2.Trans(Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0 - boundingBox.Min.Z));
			cSFrenchGeometry2.Trans(Transform.Scale(Plane.WorldXY, 1.0 / ySize, 1.0 / ySize, 1.0 / (0.0 - boundingBox.Min.Z)));
			cornerXSize = boundingBox.Diagonal.X / ySize;
			cornerYSize = boundingBox.Diagonal.Y / ySize;
		}
		CSChannelGemData[] array = list.ToArray();
		double gemZMax = array[0].GemZMax;
		double num5 = num - array[0].GemZMax;
		CSFrenchHaloHelper.SetUpFrenchGems(array);
		array = CSFrenchHaloHelper.SorttoFirstCorner(array);
		CSChannelGemData[] array2 = new CSChannelGemData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			CSChannelGemData cSChannelGemData = (CSChannelGemData)array[i].Duplicate();
			cSChannelGemData.ApplyTransform(Transform.Translation(cSChannelGemData.Up * gemZMax));
			array2[i] = cSChannelGemData;
		}
		CSFrenchHaloHelper.SetUpFrenchGems(array2);
		CSChannelGemData[] array3 = new CSChannelGemData[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			CSChannelGemData cSChannelGemData2 = (CSChannelGemData)array[j].Duplicate();
			cSChannelGemData2.ApplyTransform(Transform.Translation(cSChannelGemData2.Down * num5));
			array3[j] = cSChannelGemData2;
		}
		CSFrenchHaloHelper.SetUpFrenchGems(array3);
		for (int k = 0; k < array.Length; k++)
		{
			array2[k].Corner = array[k].Corner;
			array3[k].Corner = array[k].Corner;
		}
		Curve[] array4 = new Curve[4];
		CSFrenchHaloHelper.CalculateRail(array2, 1, rightCornerScale, leftCornerScale, wallWidth, num4, out array4[0]);
		CSFrenchHaloHelper.CalculateRail(array2, -1, rightCornerScale, leftCornerScale, wallWidth, value, out array4[1]);
		CSFrenchHaloHelper.CalculateRail(array3, 1, rightCornerScale, leftCornerScale, wallWidth, num4, out array4[2]);
		CSFrenchHaloHelper.CalculateRail(array3, -1, rightCornerScale, leftCornerScale, wallWidth, value, out array4[3]);
		List<GeometryBase> nextm = new List<GeometryBase>();
		CSDataArray cSDataArray = new CSDataArray();
		if (boolean)
		{
			for (int l = 0; l < array.Length; l++)
			{
				int num6 = (l + 1) % array.Length;
				int num7 = (l - 1 + array.Length) % array.Length;
				if (array[l].Corner == ChannelCornerType.corner90 && array[num7].Corner != ChannelCornerType.corner90 && array[num6].Corner != ChannelCornerType.corner90)
				{
					CSFrenchHaloMorphCorner.DoCornerMorph(l, array, array2, array3, cSFrenchGeometry2, cornerXSize, cornerYSize, array4, leftCornerScale, rightCornerScale, wallWidth, cornerSplitPercent, num, bottomSize, midSize, topSize, ref nextm);
				}
				else if (array[l].Corner != ChannelCornerType.corner90 && array[num6].Corner != ChannelCornerType.corner90)
				{
					CSFrenchHaloMorph.DoMorph(l, array, array2, array3, cSFrenchGeometry, array4, leftCornerScale, rightCornerScale, wallWidth, num, bottomSize, midSize, topSize, ref nextm);
				}
			}
			foreach (GeometryBase item2 in nextm)
			{
				cSDataArray.Append(new CSOnGeometryData(item2));
			}
		}
		else
		{
			CSGemData cSGemData = new CSGemData(gem);
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown))
			{
				UpdateError("Gem Xform");
				return false;
			}
			cSGemData.ApplyTransform(xDown);
			Curve[] array5 = array4;
			for (int m = 0; m < array5.Length; m++)
			{
				array5[m].Transform(xDown);
			}
			BoundingBox boundingBox2 = curve.GetBoundingBox(accurate: true);
			Curve curve2 = cSGemData.GetCurve();
			Plane plane = new Plane(curve2.PointAt(cSGemData.BezelParam), curve2.TangentAt(cSGemData.BezelParam));
			CurveIntersections curveIntersections = Intersection.CurvePlane(array4[3], plane, 0.01);
			array4[3].ClosestPoint(curveIntersections[0].PointA, out var t);
			CurveIntersections curveIntersections2 = Intersection.CurvePlane(array4[1], plane, 0.01);
			array4[1].ClosestPoint(curveIntersections2[0].PointA, out var t2);
			CurveIntersections curveIntersections3 = Intersection.CurvePlane(array4[0], plane, 0.01);
			array4[0].ClosestPoint(curveIntersections3[0].PointA, out var t3);
			double length = (array4[0].PointAt(t3) - array4[1].PointAt(t2)).Length;
			double length2 = (array4[1].PointAt(t2) - array4[3].PointAt(t)).Length;
			Transform transform = Transform.Scale(Plane.WorldXY, length / boundingBox2.Diagonal.X, length2 / boundingBox2.Diagonal.Y, 1.0);
			Transform transform2 = Transform.Translation(0.0 - boundingBox2.Min.X, 0.0 - boundingBox2.Max.Y, 0.0 - boundingBox2.Min.Z);
			Transform xform = Transform.Rotation(angleRadians, Vector3d.ZAxis, Point3d.Origin);
			curve.Transform(transform * transform2);
			curve.Transform(xform);
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out curve2, false, out string error, num4))
			{
				UpdateError("GemOffset Error");
			}
			NurbsCurve nurbsCurve = curve2.ToNurbsCurve();
			NurbsCurve nurbsCurve2 = curve.ToNurbsCurve();
			List<Point3d> list2 = new List<Point3d>();
			foreach (ControlPoint item3 in (IEnumerable<ControlPoint>)nurbsCurve2.Points)
			{
				list2.Add(item3.Location);
			}
			NurbsSurface nurbsSurface = NurbsSurface.Create(3, isRational: true, nurbsCurve2.Order, nurbsCurve.Order, nurbsCurve2.Points.Count, nurbsCurve.Points.Count);
			for (int n = 0; n < list2.Count; n++)
			{
				Point3d point3d = list2[n];
				double distance = point3d.X + num2;
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve2, false, out error, num4))
				{
					UpdateError("GemOffset Error");
				}
				curve2.Translate(cSGemData.Up * (point3d.Y + num3));
				NurbsCurve nurbsCurve3 = curve2.ToNurbsCurve();
				if (nurbsCurve.Points.Count != nurbsCurve3.Points.Count)
				{
					UpdateError("Control Pt Count Error");
				}
				for (int num8 = 0; num8 < nurbsCurve.Points.Count; num8++)
				{
					Point3d location = nurbsCurve3.Points[num8].Location;
					double weight = nurbsCurve2.Points[n].Weight * nurbsCurve3.Points[num8].Weight;
					nurbsSurface.Points.SetControlPoint(n, num8, new ControlPoint(location.X, location.Y, location.Z, weight));
				}
			}
			for (int num9 = 0; num9 < nurbsCurve2.Knots.Count; num9++)
			{
				nurbsSurface.KnotsU[num9] = nurbsCurve2.Knots[num9];
			}
			for (int num10 = 0; num10 < nurbsCurve.Knots.Count; num10++)
			{
				nurbsSurface.KnotsV[num10] = nurbsCurve.Knots[num10];
			}
			if (nurbsSurface == null || !nurbsSurface.IsValid)
			{
				UpdateError("Error Making gemless halo");
			}
			nurbsSurface?.Transform(xUp);
			Brep brep = nurbsSurface?.ToBrep();
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			cSDataArray.Append(new CSOnGeometryData(brep?.Duplicate()));
		}
		OutFrenchSet.SetData(cSDataArray);
		return base.Update();
	}
}
