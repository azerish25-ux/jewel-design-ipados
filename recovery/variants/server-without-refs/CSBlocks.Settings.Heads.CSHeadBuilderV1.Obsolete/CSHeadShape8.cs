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
[CSBlock("HeadShape_V8", "4BD123B1-FCC9-4A9A-91CE-81E76D0D15F3")]
public sealed class CSHeadShape8 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iAbove { get; }

	public CSInputChannel iBelow { get; }

	public CSInputChannel iBaseObject { get; }

	public CSInputChannel iBaseOffset { get; }

	public CSInputChannel iAngleX { get; }

	public CSInputChannel iAngleY { get; }

	public CSInputChannel iTopAngle { get; }

	public CSInputChannel iBaseAngle { get; }

	public CSInputChannel iBaseAngleControl { get; }

	public CSInputChannel iProngRotateAngle { get; }

	public CSInputChannel iBaseStyle { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailX2 { get; }

	public CSInputChannel iEnableSecondControl { get; }

	public CSInputChannel iFastMulti { get; }

	public CSOutputChannel oHeadGirdle { get; }

	public CSOutputChannel oHeadTop { get; }

	public CSOutputChannel oHeadBase { get; }

	public CSOutputChannel oHeadSideX { get; }

	public CSOutputChannel oHeadSideY { get; }

	public CSOutputChannel oHeadProngLines { get; }

	public CSOutputChannel oBaseObject { get; }

	public CSOutputChannel oGemHeight { get; }

	public CSHeadShape8(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iAbove = new CSInputChannel(GetPropertyName(() => iAbove), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBelow = new CSInputChannel(GetPropertyName(() => iBelow), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseObject = new CSInputChannel(GetPropertyName(() => iBaseObject), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBaseOffset = new CSInputChannel(GetPropertyName(() => iBaseOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleX = new CSInputChannel(GetPropertyName(() => iAngleX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleY = new CSInputChannel(GetPropertyName(() => iAngleY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopAngle = new CSInputChannel(GetPropertyName(() => iTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseAngle = new CSInputChannel(GetPropertyName(() => iBaseAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseAngleControl = new CSInputChannel(GetPropertyName(() => iBaseAngleControl), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngRotateAngle = new CSInputChannel(GetPropertyName(() => iProngRotateAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseStyle = new CSInputChannel(GetPropertyName(() => iBaseStyle), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(GetPropertyName(() => iRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailX2 = new CSInputChannel(GetPropertyName(() => iRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableSecondControl = new CSInputChannel(GetPropertyName(() => iEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iFastMulti = new CSInputChannel(GetPropertyName(() => iFastMulti), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oHeadGirdle = new CSOutputChannel(GetPropertyName(() => oHeadGirdle), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadTop = new CSOutputChannel(GetPropertyName(() => oHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadBase = new CSOutputChannel(GetPropertyName(() => oHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadSideX = new CSOutputChannel(GetPropertyName(() => oHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadSideY = new CSOutputChannel(GetPropertyName(() => oHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadProngLines = new CSOutputChannel(GetPropertyName(() => oHeadProngLines), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oBaseObject = new CSOutputChannel(GetPropertyName(() => oBaseObject), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oGemHeight = new CSOutputChannel(GetPropertyName(() => oGemHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gem");
		}
		bool value = false;
		iFastMulti.GetBoolean(ref value);
		ValueListItem whichItem = ValueListItem.HeadStyleCenter1;
		iStyle.GetValueListItem(ref whichItem);
		double value2 = 0.0;
		iAbove.GetDouble(ref value2);
		if (value2 < 0.01)
		{
			value2 = 0.01;
		}
		double value3 = 0.0;
		iBelow.GetDouble(ref value3);
		double value4 = 0.0;
		iBaseOffset.GetDouble(ref value4);
		CSDataArray flattenArray2 = iBaseObject.GetFlattenArray();
		double value5 = 10.0;
		iAngleX.GetDouble(ref value5);
		double value6 = value5;
		iAngleY.GetDouble(ref value6);
		double value7 = 0.0;
		iTopAngle.GetDouble(ref value7);
		double value8 = 0.0;
		iBaseAngle.GetDouble(ref value8);
		double value9 = 0.0;
		iBaseAngleControl.GetDouble(ref value9);
		double value10 = 0.0;
		iProngRotateAngle.GetDouble(ref value10);
		double value11 = 0.3;
		iRailX.GetDouble(ref value11);
		double value12 = 0.3;
		iRailX2.GetDouble(ref value12);
		bool value13 = false;
		iEnableSecondControl.GetBoolean(ref value13);
		if (value13)
		{
			value11 = value12;
		}
		value11 = value11 * 2.0 + 0.1;
		ValueListItem whichItem2 = ValueListItem.HeadBuilderProngHead;
		iBaseStyle.GetValueListItem(ref whichItem2);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		CSDataArray cSDataArray6 = new CSDataArray();
		int num = flattenArray.GetArrayCount();
		if (value)
		{
			num = 1;
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
		for (int i = 0; i < num; i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			double drop = GetDrop(value3, flattenArray2, value4, cSGemData);
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
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out Curve curve2, doTransformUp: false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			((GeometryBase)curve2).MakeDeformable();
			double num2 = drop * Math.Tan(RhinoMath.ToRadians(value5));
			double num3 = drop * Math.Tan(RhinoMath.ToRadians(value6));
			double num4 = gemXSize + num2 * 2.0;
			if (num4 < value11)
			{
				num4 = value11;
				value5 = RhinoMath.ToDegrees(Math.Atan((num4 - gemXSize) / 2.0 / drop));
			}
			double num5 = gemYSize + num3 * 2.0;
			if (num5 < value11)
			{
				num5 = value11;
				value6 = RhinoMath.ToDegrees(Math.Atan((num5 - gemYSize) / 2.0 / drop));
			}
			if (whichItem2 != ValueListItem.HeadBuilderProngHead)
			{
				num4 = 0.05;
				num5 = 0.05;
				value5 = RhinoMath.ToDegrees(Math.Atan((num4 - gemXSize) / 2.0 / drop));
				value6 = RhinoMath.ToDegrees(Math.Atan((num5 - gemYSize) / 2.0 / drop));
			}
			Transform val = Transform.Scale(Plane.WorldXY, num4 / gemXSize, num5 / gemYSize, 1.0);
			Transform val2 = Transform.Translation(0.0, 0.0, drop) * val;
			Curve val3 = curve2.DuplicateCurve();
			((GeometryBase)val3).Transform(val2);
			if (value9 < 0.0)
			{
				value9 = 0.0;
			}
			if (value9 > 1.0)
			{
				value9 = 1.0;
			}
			double num6 = (0.0 - drop) * value9 * Math.Tan(RhinoMath.ToRadians(value5 + value8));
			double num7 = (0.0 - drop) * value9 * Math.Tan(RhinoMath.ToRadians(value6 + value8));
			Transform val4 = Transform.Scale(Plane.WorldXY, (num4 + num6 * 2.0) / gemXSize, (num5 + num7 * 2.0) / gemYSize, 1.0);
			Transform val5 = Transform.Translation(0.0, 0.0, drop * (1.0 - value9)) * val4;
			((GeometryBase)curve2.DuplicateCurve()).Transform(val5);
			double num8 = value2 * Math.Tan(RhinoMath.ToRadians(value5 + value7));
			double num9 = value2 * Math.Tan(RhinoMath.ToRadians(value6 + value7));
			Transform val6 = Transform.Scale(Plane.WorldXY, (gemXSize + num8 * 2.0) / gemXSize, (gemYSize + num9 * 2.0) / gemYSize, 1.0);
			Transform val7 = Transform.Translation(0.0, 0.0, value2) * val6;
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
			List<CSHeadStyles5.HeadProngInfo> headStyle = CSHeadStyles5.GetHeadStyle(cSGemData.GemShape, whichItem);
			if (headStyle == null)
			{
				return UpdateError("Prong Info Missing");
			}
			CSDataArray cSDataArray7 = new CSDataArray();
			foreach (CSHeadStyles5.HeadProngInfo item12 in headStyle)
			{
				if (item12.ProngParamIndex >= 0 && item12.ProngParamIndex < list.Count)
				{
					Point3d val9 = curve.PointAt(list[item12.ProngParamIndex].ProngParam);
					LineCurve val10 = new LineCurve(Point3d.Origin, val9 * 50.0);
					Transform val11 = Transform.Rotation(RhinoMath.ToRadians(value10 * (double)item12.MoveDir), Vector3d.ZAxis, Point3d.Origin);
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
				oGemHeight.SetData(new CSNumberData(cSGemData.GemZMin));
			}
		}
		oHeadGirdle.SetData(cSDataArray);
		oHeadTop.SetData(cSDataArray2);
		oHeadBase.SetData(cSDataArray3);
		oHeadSideX.SetData(cSDataArray4);
		oHeadSideY.SetData(cSDataArray5);
		oHeadProngLines.SetData(cSDataArray6);
		oBaseObject.SetData(flattenArray2);
		return base.Update();
	}

	private double GetDrop(double vBelowInput, CSDataArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
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
		if (arrayItem is ICSCurve iCSCurve)
		{
			Curve curve = iCSCurve.GetCurve();
			if (curve != null)
			{
				Curve val2 = curve.DuplicateCurve();
				Curve val3 = curve.DuplicateCurve();
				((GeometryBase)val2).Translate(Vector3d.YAxis * 50.0);
				((GeometryBase)val3).Translate(Vector3d.YAxis * -50.0);
				val = ((Surface)NurbsSurface.CreateRuledSurface(val2, val3)).ToBrep();
			}
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
		LineCurve val4 = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		((GeometryBase)val4).Transform(xUp);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5)).Transform(xUp);
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(Vector3d.ZAxis);
		((Vector3d)(ref val6)).Transform(xUp);
		((Vector3d)(ref val6)).Unitize();
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.CurveBrep((Curve)val4, val, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return vBelowInput + vBaseOffset;
		}
		if (array2.Length == 0 && array.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d val7 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array3 = array;
		double num2 = default(double);
		foreach (Curve obj in array3)
		{
			obj.ClosestPoint(val5, ref num2);
			Point3d val8 = obj.PointAt(num2);
			num2 = ((Point3d)(ref val8)).DistanceTo(val5);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				val7 = val8;
				num = num2;
			}
		}
		Point3d[] array4 = array2;
		for (int i = 0; i < array4.Length; i++)
		{
			Point3d val9 = array4[i];
			double num3 = ((Point3d)(ref val9)).DistanceTo(val5);
			if (!RhinoMath.IsValidDouble(num) || num > num3)
			{
				val7 = val9;
				num = num3;
			}
		}
		if (val6 * (val5 - val7) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static Curve ProngInterpCurve(List<Point3d> listPoints)
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
