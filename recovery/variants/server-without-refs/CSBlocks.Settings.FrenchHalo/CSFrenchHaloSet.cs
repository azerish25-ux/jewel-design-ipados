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
using Rhino.Runtime;

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
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
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
		double num5 = RhinoMath.ToRadians(InClusterAngle.Double);
		CSFrenchGeometry cSFrenchGeometry = null;
		List<Mesh> value3;
		if (InFrenchObj.GetValues(out List<Brep> value2) && value2.Count > 0)
		{
			cSFrenchGeometry = new CSFrenchGeometry((IEnumerable<GeometryBase>)value2, null, "c", isMesh: false, isCamMode);
		}
		else if (InFrenchObj.GetValues(out value3) && value3.Count > 0)
		{
			cSFrenchGeometry = new CSFrenchGeometry((IEnumerable<GeometryBase>)value3, null, "c", isMesh: true, isCamMode);
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
			cSFrenchGeometry2 = new CSFrenchGeometry((IEnumerable<GeometryBase>)value2, null, "9", isMesh: false, isCamMode);
		}
		else if (InFrenchObj.GetValues(out value4) && value4.Count > 0)
		{
			cSFrenchGeometry2 = new CSFrenchGeometry((IEnumerable<GeometryBase>)value4, null, "9", isMesh: true, isCamMode);
		}
		if (!cSFrenchGeometry.SetUpGeometry(halo: true, out var bottomSize, out var midSize, out var topSize, out var _, out var ySize))
		{
			return UpdateError("Bounding Box not found");
		}
		double cornerXSize = 1.0;
		double cornerYSize = 1.0;
		Point3d val;
		Vector3d val2;
		if (cSFrenchGeometry2 != null)
		{
			BoundingBox boundingBox = cSFrenchGeometry2.GAll.GetBoundingBox(true);
			CSFrenchGeometry cSFrenchGeometry3 = cSFrenchGeometry2;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num6 = 0.0 - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num7 = 0.0 - ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			cSFrenchGeometry3.Trans(Transform.Translation(num6, num7, 0.0 - ((Point3d)(ref val)).Z));
			CSFrenchGeometry cSFrenchGeometry4 = cSFrenchGeometry2;
			Plane worldXY = Plane.WorldXY;
			double num8 = 1.0 / ySize;
			double num9 = 1.0 / ySize;
			val = ((BoundingBox)(ref boundingBox)).Min;
			cSFrenchGeometry4.Trans(Transform.Scale(worldXY, num8, num9, 1.0 / (0.0 - ((Point3d)(ref val)).Z)));
			val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
			cornerXSize = ((Vector3d)(ref val2)).X / ySize;
			val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
			cornerYSize = ((Vector3d)(ref val2)).Y / ySize;
		}
		CSChannelGemData[] array = list.ToArray();
		double gemZMax = array[0].GemZMax;
		double num10 = num - array[0].GemZMax;
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
			cSChannelGemData2.ApplyTransform(Transform.Translation(cSChannelGemData2.Down * num10));
			array3[j] = cSChannelGemData2;
		}
		CSFrenchHaloHelper.SetUpFrenchGems(array3);
		for (int k = 0; k < array.Length; k++)
		{
			array2[k].Corner = array[k].Corner;
			array3[k].Corner = array[k].Corner;
		}
		Curve[] array4 = (Curve[])(object)new Curve[4];
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
				int num11 = (l + 1) % array.Length;
				int num12 = (l - 1 + array.Length) % array.Length;
				if (array[l].Corner == ChannelCornerType.corner90 && array[num12].Corner != ChannelCornerType.corner90 && array[num11].Corner != ChannelCornerType.corner90)
				{
					CSFrenchHaloMorphCorner.DoCornerMorph(l, array, array2, array3, cSFrenchGeometry2, cornerXSize, cornerYSize, array4, leftCornerScale, rightCornerScale, wallWidth, cornerSplitPercent, num, bottomSize, midSize, topSize, ref nextm);
				}
				else if (array[l].Corner != ChannelCornerType.corner90 && array[num11].Corner != ChannelCornerType.corner90)
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
				((GeometryBase)array5[m]).Transform(xDown);
			}
			BoundingBox boundingBox2 = ((GeometryBase)curve).GetBoundingBox(true);
			Curve curve2 = cSGemData.GetCurve();
			Plane val3 = default(Plane);
			((Plane)(ref val3))._002Ector(curve2.PointAt(cSGemData.BezelParam), curve2.TangentAt(cSGemData.BezelParam));
			CurveIntersections val4 = Intersection.CurvePlane(array4[3], val3, 0.01);
			double num13 = default(double);
			array4[3].ClosestPoint(val4[0].PointA, ref num13);
			CurveIntersections val5 = Intersection.CurvePlane(array4[1], val3, 0.01);
			double num14 = default(double);
			array4[1].ClosestPoint(val5[0].PointA, ref num14);
			CurveIntersections val6 = Intersection.CurvePlane(array4[0], val3, 0.01);
			double num15 = default(double);
			array4[0].ClosestPoint(val6[0].PointA, ref num15);
			val2 = array4[0].PointAt(num15) - array4[1].PointAt(num14);
			double length = ((Vector3d)(ref val2)).Length;
			val2 = array4[1].PointAt(num14) - array4[3].PointAt(num13);
			double length2 = ((Vector3d)(ref val2)).Length;
			Plane worldXY2 = Plane.WorldXY;
			val2 = ((BoundingBox)(ref boundingBox2)).Diagonal;
			double num16 = length / ((Vector3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox2)).Diagonal;
			Transform val7 = Transform.Scale(worldXY2, num16, length2 / ((Vector3d)(ref val2)).Y, 1.0);
			val = ((BoundingBox)(ref boundingBox2)).Min;
			double num17 = 0.0 - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			double num18 = 0.0 - ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox2)).Min;
			Transform val8 = Transform.Translation(num17, num18, 0.0 - ((Point3d)(ref val)).Z);
			Transform val9 = Transform.Rotation(num5, Vector3d.ZAxis, Point3d.Origin);
			((GeometryBase)curve).Transform(val7 * val8);
			((GeometryBase)curve).Transform(val9);
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out curve2, doTransformUp: false, out string error, num4))
			{
				UpdateError("GemOffset Error");
			}
			NurbsCurve val10 = curve2.ToNurbsCurve();
			NurbsCurve val11 = curve.ToNurbsCurve();
			List<Point3d> list2 = new List<Point3d>();
			foreach (ControlPoint item3 in (IEnumerable<ControlPoint>)val11.Points)
			{
				ControlPoint current3 = item3;
				list2.Add(((ControlPoint)(ref current3)).Location);
			}
			NurbsSurface val12 = NurbsSurface.Create(3, true, val11.Order, val10.Order, val11.Points.Count, val10.Points.Count);
			for (int n = 0; n < list2.Count; n++)
			{
				Point3d val13 = list2[n];
				double distance = ((Point3d)(ref val13)).X + num2;
				if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve2, doTransformUp: false, out error, num4))
				{
					UpdateError("GemOffset Error");
				}
				((GeometryBase)curve2).Translate(cSGemData.Up * (((Point3d)(ref val13)).Y + num3));
				NurbsCurve val14 = curve2.ToNurbsCurve();
				if (val10.Points.Count != val14.Points.Count)
				{
					UpdateError("Control Pt Count Error");
				}
				for (int num19 = 0; num19 < val10.Points.Count; num19++)
				{
					ControlPoint val15 = val14.Points[num19];
					Point3d location = ((ControlPoint)(ref val15)).Location;
					val15 = val11.Points[n];
					double weight = ((ControlPoint)(ref val15)).Weight;
					val15 = val14.Points[num19];
					double num20 = weight * ((ControlPoint)(ref val15)).Weight;
					val12.Points.SetControlPoint(n, num19, new ControlPoint(((Point3d)(ref location)).X, ((Point3d)(ref location)).Y, ((Point3d)(ref location)).Z, num20));
				}
			}
			for (int num21 = 0; num21 < val11.Knots.Count; num21++)
			{
				val12.KnotsU[num21] = val11.Knots[num21];
			}
			for (int num22 = 0; num22 < val10.Knots.Count; num22++)
			{
				val12.KnotsV[num22] = val10.Knots[num22];
			}
			if (val12 == null || !((CommonObject)val12).IsValid)
			{
				UpdateError("Error Making gemless halo");
			}
			if (val12 != null)
			{
				((GeometryBase)val12).Transform(xUp);
			}
			Brep brep = ((val12 != null) ? ((Surface)val12).ToBrep() : null);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			cSDataArray.Append(new CSOnGeometryData((brep != null) ? ((GeometryBase)brep).Duplicate() : null));
		}
		OutFrenchSet.SetData(cSDataArray);
		return base.Update();
	}
}
