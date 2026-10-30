using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSDatas;
using CSDatas.Interfaces;
using CSGemInstance;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadShape_V10", "{47F83109-1F45-4384-89D6-2DA92EC6BC5E}")]
public sealed class CSHeadShape10 : CSBlock
{
	public CSInputChannel InGems { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InAbove { get; }

	public CSInputChannel InBelow { get; }

	public CSInputChannel InBaseObject { get; }

	public CSInputChannel InBaseOffset { get; }

	public CSInputChannel InAngleX { get; }

	public CSInputChannel InAngleY { get; }

	public CSInputChannel InTopAngle { get; }

	public CSInputChannel InBaseAngle { get; }

	public CSInputChannel InBaseAngleControl { get; }

	public CSInputChannel InProngRotateAngle { get; }

	public CSInputChannel InBaseStyle { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailX2 { get; }

	public CSInputChannel InEnableSecondControl { get; }

	public CSInputChannel InFastMulti { get; }

	public CSOutputChannel OutHeadGirdle { get; }

	public CSOutputChannel OutHeadTop { get; }

	public CSOutputChannel OutHeadBase { get; }

	public CSOutputChannel OutHeadSideX { get; }

	public CSOutputChannel OutHeadSideY { get; }

	public CSOutputChannel OutHeadProngLines { get; }

	public CSOutputChannel OutBaseObject { get; }

	public CSOutputChannel OutGemHeight { get; }

	public CSHeadShape10(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InAbove = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBelow = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InBaseOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InAngleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InTopAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseAngleControl = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngRotateAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InRailX2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InEnableSecondControl = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFastMulti = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutHeadGirdle = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutHeadTop = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutHeadBase = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutHeadSideX = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutHeadSideY = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutHeadProngLines = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutBaseObject = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutGemHeight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		if (gemList.Count < 1)
		{
			return UpdateMissing("Missing Gem");
		}
		bool boolean = InFastMulti.Boolean;
		HeadStyle integer = (HeadStyle)InStyle.Integer;
		double num = InAbove.Double;
		if (num < 0.01)
		{
			num = 0.01;
		}
		double vBelowInput = InBelow.Double;
		double vBaseOffset = InBaseOffset.Double;
		CSDataArray flattenArray = InBaseObject.GetFlattenArray();
		double num2 = InAngleX.Double;
		double num3 = InAngleY.Double;
		double num4 = InTopAngle.Double;
		double num5 = InBaseAngle.Double;
		double num6 = InBaseAngleControl.Double;
		double num7 = InProngRotateAngle.Double;
		double num8 = InRailX.Double;
		double num9 = InRailX2.Double;
		if (InEnableSecondControl.Boolean)
		{
			num8 = num9;
		}
		num8 = num8 * 2.0 + 0.1;
		ValueListItem valueListItem = InBaseStyle.ValueListItem;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		CSDataArray cSDataArray6 = new CSDataArray();
		int num10 = gemList.Count;
		if (boolean)
		{
			num10 = 1;
		}
		for (int i = 0; i < num10; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				continue;
			}
			double drop = GetDrop(vBelowInput, flattenArray, vBaseOffset, cSGemData);
			if (!CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR) || !cSGemData.GetGemXformRC(out var xDown, out var _))
			{
				continue;
			}
			double gemXSize = cSGemData.GemXSize;
			double gemYSize = cSGemData.GemYSize;
			double gemXMax = cSGemData.GemXMax;
			double gemYMax = cSGemData.GemYMax;
			Curve curve = cSGemData.GetCurve();
			if (curve == null)
			{
				continue;
			}
			curve.Transform(xDown);
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve2, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			curve2.MakeDeformable();
			double num11 = drop * Math.Tan(RhinoMath.ToRadians(num2));
			double num12 = drop * Math.Tan(RhinoMath.ToRadians(num3));
			double num13 = gemXSize + num11 * 2.0;
			if (num13 < num8)
			{
				num13 = num8;
				num2 = RhinoMath.ToDegrees(Math.Atan((num13 - gemXSize) / 2.0 / drop));
			}
			double num14 = gemYSize + num12 * 2.0;
			if (num14 < num8)
			{
				num14 = num8;
				num3 = RhinoMath.ToDegrees(Math.Atan((num14 - gemYSize) / 2.0 / drop));
			}
			if (valueListItem != ValueListItem.HeadBuilderProngHead)
			{
				num13 = 0.05;
				num14 = 0.05;
				num2 = RhinoMath.ToDegrees(Math.Atan((num13 - gemXSize) / 2.0 / drop));
				num3 = RhinoMath.ToDegrees(Math.Atan((num14 - gemYSize) / 2.0 / drop));
			}
			Transform transform = Transform.Scale(Plane.WorldXY, num13 / gemXSize, num14 / gemYSize, 1.0);
			Transform transform2 = Transform.Translation(0.0, 0.0, drop) * transform;
			Curve curve3 = curve2.DuplicateCurve();
			curve3.Transform(transform2);
			if (num6 < 0.0)
			{
				num6 = 0.0;
			}
			if (num6 > 1.0)
			{
				num6 = 1.0;
			}
			double num15 = (0.0 - drop) * num6 * Math.Tan(RhinoMath.ToRadians(num2 + num5));
			double num16 = (0.0 - drop) * num6 * Math.Tan(RhinoMath.ToRadians(num3 + num5));
			Transform transform3 = Transform.Scale(Plane.WorldXY, (num13 + num15 * 2.0) / gemXSize, (num14 + num16 * 2.0) / gemYSize, 1.0);
			Transform xform = Transform.Translation(0.0, 0.0, drop * (1.0 - num6)) * transform3;
			curve2.DuplicateCurve().Transform(xform);
			double num17 = num * Math.Tan(RhinoMath.ToRadians(num2 + num4));
			double num18 = num * Math.Tan(RhinoMath.ToRadians(num3 + num4));
			Transform transform4 = Transform.Scale(Plane.WorldXY, (gemXSize + num17 * 2.0) / gemXSize, (gemYSize + num18 * 2.0) / gemYSize, 1.0);
			Transform transform5 = Transform.Translation(0.0, 0.0, num) * transform4;
			Curve curve4 = curve2.DuplicateCurve();
			curve4.Transform(transform5);
			Point3d item = new Point3d(gemXMax, 0.0, 0.0);
			item.Transform(transform5);
			Point3d item2 = new Point3d(gemXMax, 0.0, 0.0);
			Point3d item3 = new Point3d(gemXMax, 0.0, 0.0);
			item3.Transform(xform);
			Point3d item4 = new Point3d(gemXMax, 0.0, 0.0);
			item4.Transform(transform2);
			Curve geometry = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
			Point3d item5 = new Point3d(0.0, gemYMax, 0.0);
			item5.Transform(transform5);
			Point3d item6 = new Point3d(0.0, gemYMax, 0.0);
			Point3d item7 = new Point3d(0.0, gemYMax, 0.0);
			item7.Transform(xform);
			Point3d item8 = new Point3d(0.0, gemYMax, 0.0);
			item8.Transform(transform2);
			Curve geometry2 = ProngInterpCurve(new List<Point3d> { item5, item6, item7, item8 });
			List<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			IEnumerable<CSHeadStyles.HeadProngInfo> headProngInfo = CSHeadStyles.GetHeadProngInfo(integer);
			if (headProngInfo == null)
			{
				return UpdateError("Prong Info Missing");
			}
			CSDataArray cSDataArray7 = new CSDataArray();
			foreach (CSHeadStyles.HeadProngInfo item12 in headProngInfo)
			{
				if (item12.ProngParamIndex >= 0 && item12.ProngParamIndex < list.Count)
				{
					Point3d point3d = curve.PointAt(list[item12.ProngParamIndex].ProngParam);
					LineCurve lineCurve = new LineCurve(Point3d.Origin, point3d * 50.0);
					Transform xform2 = Transform.Rotation(RhinoMath.ToRadians(num7 * (double)item12.MoveDir), Vector3d.ZAxis, Point3d.Origin);
					lineCurve.Transform(xform2);
					CurveIntersections curveIntersections = Intersection.CurveCurve(curve2, lineCurve, 0.001, 0.001);
					if (curveIntersections.Count > 0)
					{
						double parameterA = curveIntersections[0].ParameterA;
						Point3d point3d2 = new Point3d(curve2.PointAt(parameterA));
						Point3d item9 = new Point3d(point3d2);
						item9.Transform(transform2);
						Point3d item10 = new Point3d(point3d2);
						item10.Transform(xform);
						Point3d item11 = new Point3d(point3d2);
						item11.Transform(transform5);
						Curve geometry3 = ProngInterpCurve(new List<Point3d> { item11, point3d2, item10, item9 });
						cSDataArray7.Append(new CSOnGeometryData(geometry3));
					}
				}
			}
			cSDataArray.Append(new CSOnGeometryData(curve2));
			cSDataArray2.Append(new CSOnGeometryData(curve4, transform5, applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData(curve3, transform2, applyTransform: false));
			cSDataArray4.Append(new CSOnGeometryData(geometry));
			cSDataArray5.Append(new CSOnGeometryData(geometry2));
			cSDataArray6.Append(cSDataArray7);
			if (i == 0)
			{
				OutGemHeight.SetData(new CSNumberData(cSGemData.GemZMin));
			}
		}
		OutHeadGirdle.SetData(cSDataArray);
		OutHeadTop.SetData(cSDataArray2);
		OutHeadBase.SetData(cSDataArray3);
		OutHeadSideX.SetData(cSDataArray4);
		OutHeadSideY.SetData(cSDataArray5);
		OutHeadProngLines.SetData(cSDataArray6);
		OutBaseObject.SetData(flattenArray);
		return base.Update();
	}

	private static double GetDrop(double vBelowInput, ICSArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		if (vBaseObject.GetArrayCount() < 1)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep brep = null;
		Curve curve = (arrayItem as ICSCurve)?.GetCurve();
		if (curve != null)
		{
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Translate(Vector3d.YAxis * 50.0);
			curve3.Translate(Vector3d.YAxis * -50.0);
			brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
		}
		if (brep == null && arrayItem is ICSBrep iCSBrep)
		{
			brep = iCSBrep.GetBrep();
		}
		if (brep == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		lineCurve.Transform(xUp);
		Point3d point3d = default(Point3d);
		point3d.Transform(xUp);
		Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
		vector3d.Transform(xUp);
		vector3d.Unitize();
		if (!Intersection.CurveBrep(lineCurve, brep, CSApplicationHelper.Application.Tolerance, out var overlapCurves, out var intersectionPoints))
		{
			return vBelowInput + vBaseOffset;
		}
		if (intersectionPoints.Length == 0 && overlapCurves.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d point3d2 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array = overlapCurves;
		foreach (Curve obj in array)
		{
			obj.ClosestPoint(point3d, out var t);
			Point3d point3d3 = obj.PointAt(t);
			t = point3d3.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > t)
			{
				point3d2 = point3d3;
				num = t;
			}
		}
		Point3d[] array2 = intersectionPoints;
		for (int i = 0; i < array2.Length; i++)
		{
			Point3d point3d4 = array2[i];
			double num2 = point3d4.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				point3d2 = point3d4;
				num = num2;
			}
		}
		if (vector3d * (point3d - point3d2) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static Curve ProngInterpCurve(IList<Point3d> listPoints)
	{
		for (int i = 1; i < listPoints.Count - 1; i++)
		{
			if (listPoints[i].DistanceTo(listPoints[i - 1]) < 0.01)
			{
				listPoints.RemoveAt(i);
				i--;
			}
			else if (listPoints[i].DistanceTo(listPoints[i + 1]) < 0.01)
			{
				listPoints.RemoveAt(i);
				i--;
			}
		}
		List<Point3d> list = new List<Point3d>();
		if (listPoints.Count > 2)
		{
			list.AddRange(listPoints);
		}
		else
		{
			LineCurve lineCurve = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(lineCurve.PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return Curve.CreateInterpolatedCurve(list, 3, CurveKnotStyle.Chord) ?? new LineCurve(listPoints[0], listPoints[listPoints.Count - 1]);
	}
}
