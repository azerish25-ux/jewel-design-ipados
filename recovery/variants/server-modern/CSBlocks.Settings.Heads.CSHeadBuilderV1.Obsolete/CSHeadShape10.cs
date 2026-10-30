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
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d item = default(Point3d);
		Point3d item2 = default(Point3d);
		Point3d item3 = default(Point3d);
		Point3d item4 = default(Point3d);
		Point3d item5 = default(Point3d);
		Point3d item6 = default(Point3d);
		Point3d item7 = default(Point3d);
		Point3d item8 = default(Point3d);
		Point3d val13 = default(Point3d);
		Point3d item9 = default(Point3d);
		Point3d item10 = default(Point3d);
		Point3d item11 = default(Point3d);
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
			((GeometryBase)curve).Transform(xDown);
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve2, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			((GeometryBase)curve2).MakeDeformable();
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
			Transform val = Transform.Scale(Plane.WorldXY, num13 / gemXSize, num14 / gemYSize, 1.0);
			Transform val2 = Transform.Translation(0.0, 0.0, drop) * val;
			Curve val3 = curve2.DuplicateCurve();
			((GeometryBase)val3).Transform(val2);
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
			Transform val4 = Transform.Scale(Plane.WorldXY, (num13 + num15 * 2.0) / gemXSize, (num14 + num16 * 2.0) / gemYSize, 1.0);
			Transform val5 = Transform.Translation(0.0, 0.0, drop * (1.0 - num6)) * val4;
			((GeometryBase)curve2.DuplicateCurve()).Transform(val5);
			double num17 = num * Math.Tan(RhinoMath.ToRadians(num2 + num4));
			double num18 = num * Math.Tan(RhinoMath.ToRadians(num3 + num4));
			Transform val6 = Transform.Scale(Plane.WorldXY, (gemXSize + num17 * 2.0) / gemXSize, (gemYSize + num18 * 2.0) / gemYSize, 1.0);
			Transform val7 = Transform.Translation(0.0, 0.0, num) * val6;
			Curve val8 = curve2.DuplicateCurve();
			((GeometryBase)val8).Transform(val7);
			((Point3d)(ref item))._002Ector(gemXMax, 0.0, 0.0);
			((Point3d)(ref item)).Transform(val7);
			((Point3d)(ref item2))._002Ector(gemXMax, 0.0, 0.0);
			((Point3d)(ref item3))._002Ector(gemXMax, 0.0, 0.0);
			((Point3d)(ref item3)).Transform(val5);
			((Point3d)(ref item4))._002Ector(gemXMax, 0.0, 0.0);
			((Point3d)(ref item4)).Transform(val2);
			Curve geometry = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
			((Point3d)(ref item5))._002Ector(0.0, gemYMax, 0.0);
			((Point3d)(ref item5)).Transform(val7);
			((Point3d)(ref item6))._002Ector(0.0, gemYMax, 0.0);
			((Point3d)(ref item7))._002Ector(0.0, gemYMax, 0.0);
			((Point3d)(ref item7)).Transform(val5);
			((Point3d)(ref item8))._002Ector(0.0, gemYMax, 0.0);
			((Point3d)(ref item8)).Transform(val2);
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
					Point3d val9 = curve.PointAt(list[item12.ProngParamIndex].ProngParam);
					LineCurve val10 = new LineCurve(Point3d.Origin, val9 * 50.0);
					Transform val11 = Transform.Rotation(RhinoMath.ToRadians(num7 * (double)item12.MoveDir), Vector3d.ZAxis, Point3d.Origin);
					((GeometryBase)val10).Transform(val11);
					CurveIntersections val12 = Intersection.CurveCurve(curve2, (Curve)(object)val10, 0.001, 0.001);
					if (val12.Count > 0)
					{
						double parameterA = val12[0].ParameterA;
						((Point3d)(ref val13))._002Ector(curve2.PointAt(parameterA));
						((Point3d)(ref item9))._002Ector(val13);
						((Point3d)(ref item9)).Transform(val2);
						((Point3d)(ref item10))._002Ector(val13);
						((Point3d)(ref item10)).Transform(val5);
						((Point3d)(ref item11))._002Ector(val13);
						((Point3d)(ref item11)).Transform(val7);
						Curve geometry3 = ProngInterpCurve(new List<Point3d> { item11, val13, item10, item9 });
						cSDataArray7.Append(new CSOnGeometryData((GeometryBase)(object)geometry3));
					}
				}
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)curve2));
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val8, val7, applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)val3, val2, applyTransform: false));
			cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
			cSDataArray5.Append(new CSOnGeometryData((GeometryBase)(object)geometry2));
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		if (vBaseObject.GetArrayCount() < 1)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep val = null;
		Curve val2 = (arrayItem as ICSCurve)?.GetCurve();
		if (val2 != null)
		{
			Curve val3 = val2.DuplicateCurve();
			Curve val4 = val2.DuplicateCurve();
			((GeometryBase)val3).Translate(Vector3d.YAxis * 50.0);
			((GeometryBase)val4).Translate(Vector3d.YAxis * -50.0);
			val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
		}
		if (val == null && arrayItem is ICSBrep iCSBrep)
		{
			val = iCSBrep.GetBrep();
		}
		if (val == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve val5 = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		((GeometryBase)val5).Transform(xUp);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6)).Transform(xUp);
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(Vector3d.ZAxis);
		((Vector3d)(ref val7)).Transform(xUp);
		((Vector3d)(ref val7)).Unitize();
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.CurveBrep((Curve)val5, val, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return vBelowInput + vBaseOffset;
		}
		if (array2.Length == 0 && array.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d val8 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array3 = array;
		double num2 = default(double);
		foreach (Curve obj in array3)
		{
			obj.ClosestPoint(val6, ref num2);
			Point3d val9 = obj.PointAt(num2);
			num2 = ((Point3d)(ref val9)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				val8 = val9;
				num = num2;
			}
		}
		Point3d[] array4 = array2;
		for (int i = 0; i < array4.Length; i++)
		{
			Point3d val10 = array4[i];
			double num3 = ((Point3d)(ref val10)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num3)
			{
				val8 = val10;
				num = num3;
			}
		}
		if (val7 * (val6 - val8) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static Curve ProngInterpCurve(IList<Point3d> listPoints)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 1; i < listPoints.Count - 1; i++)
		{
			Point3d val = listPoints[i];
			if (((Point3d)(ref val)).DistanceTo(listPoints[i - 1]) < 0.01)
			{
				listPoints.RemoveAt(i);
				i--;
				continue;
			}
			val = listPoints[i];
			if (((Point3d)(ref val)).DistanceTo(listPoints[i + 1]) < 0.01)
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
			LineCurve val2 = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(((Curve)val2).PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return (Curve)(((object)Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list, 3, (CurveKnotStyle)1)) ?? ((object)new LineCurve(listPoints[0], listPoints[listPoints.Count - 1])));
	}
}
