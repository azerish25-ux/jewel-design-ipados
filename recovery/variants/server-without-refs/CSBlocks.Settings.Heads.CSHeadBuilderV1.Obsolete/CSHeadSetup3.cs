using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadSetup_V3", "{1BC95095-CF73-46C2-9503-6DB5B9CCC563}")]
public sealed class CSHeadSetup3 : CSBlock
{
	public CSInputChannel InGems { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InHeadHeight { get; }

	public CSInputChannel InBaseObject { get; }

	public CSInputChannel InBaseOffset { get; }

	public CSInputChannel InAngleX { get; }

	public CSInputChannel InAngleY { get; }

	public CSInputChannel InMinimumBaseSpacing { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InBaseStyle { get; }

	public CSInputChannel InGemTransform { get; }

	public CSOutputChannel OutBaseObject { get; }

	public CSOutputChannel OutGemShape { get; }

	public CSOutputChannel OutGemX { get; }

	public CSOutputChannel OutGemY { get; }

	public CSOutputChannel OutGemZ { get; }

	public CSOutputChannel OutGemZMin { get; }

	public CSOutputChannel OutProngParamIndex { get; }

	public CSOutputChannel OutProngOverlap { get; }

	public CSOutputChannel OutProngArrangment { get; }

	public CSOutputChannel OutProngType { get; }

	public CSOutputChannel OutProngMoveDir { get; }

	public CSOutputChannel OutAngleX { get; }

	public CSOutputChannel OutAngleY { get; }

	public CSOutputChannel OutRailDistance { get; }

	public CSHeadSetup3(string identifier)
		: base(identifier)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InHeadHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBaseOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMinimumBaseSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		InGemTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Identity);
		OutBaseObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		OutGemShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		OutGemX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutProngParamIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutProngArrangment = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngType = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngMoveDir = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutAngleX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutAngleY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRailDistance = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		if (!InGems.GetValue(out CSGemData value))
		{
			return InputError();
		}
		HeadStyle whichHeadStyle = HeadStyle.Unknown;
		if (!InStyle.GetValueHeadStyle(ref whichHeadStyle))
		{
			return InputError();
		}
		IEnumerable<CSHeadStyles.HeadProngInfo> headProngInfo = CSHeadStyles.GetHeadProngInfo(whichHeadStyle);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		foreach (CSHeadStyles.HeadProngInfo item in headProngInfo)
		{
			cSDataArray.Array.Add(new CSNumberData(item.ProngParamIndex));
			cSDataArray2.Array.Add(new CSNumberData(item.Overlap));
			cSDataArray3.Array.Add(new CSNumberData((double)item.Arrangment));
			cSDataArray4.Array.Add(new CSNumberData((double)item.Type));
			cSDataArray5.Array.Add(new CSNumberData(item.MoveDir));
		}
		InHeadHeight.GetValue(out double value2);
		value2 = 0.0 - value2;
		CSData data = InBaseObject.Data;
		InBaseOffset.GetValue(out double value3);
		InAngleX.GetValue(out double value4);
		InAngleY.GetValue(out double value5);
		InMinimumBaseSpacing.GetValue(out double value6);
		InRailX.GetValue(out double value7);
		value6 = value7 * 2.0 + value6;
		InBaseStyle.GetValue(out int value8);
		int num = value8;
		InGemTransform.GetValue(out Transform value9);
		double drop = GetDrop(value2, data, value3, value, value9);
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4));
		double num3 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value5));
		if (value.GemXSize + num2 * 2.0 < value6)
		{
			value4 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((value6 - value.GemXSize) / 2.0 / drop));
		}
		double num4 = value.GemYSize + num3 * 2.0;
		if (num4 < value6)
		{
			num4 = value6;
			value5 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num4 - value.GemYSize) / 2.0 / drop));
		}
		if (num != 122)
		{
			num4 = 0.05;
			value4 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((0.05 - value.GemXSize) / 2.0 / drop));
			value5 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num4 - value.GemYSize) / 2.0 / drop));
		}
		OutBaseObject.SetData(data);
		OutGemShape.SetData(new CSStringData(value.GemShape));
		OutGemX.SetData(new CSNumberData(value.GemXSize));
		OutGemY.SetData(new CSNumberData(value.GemYSize));
		OutGemZ.SetData(new CSNumberData(value.GemZSize));
		OutGemZMin.SetData(new CSNumberData(value.GemZMin));
		OutProngParamIndex.SetData(cSDataArray);
		OutProngOverlap.SetData(cSDataArray2);
		OutProngArrangment.SetData(cSDataArray3);
		OutProngType.SetData(cSDataArray4);
		OutProngMoveDir.SetData(cSDataArray5);
		OutAngleX.SetData(new CSNumberData(value4));
		OutAngleY.SetData(new CSNumberData(value5));
		OutRailDistance.SetData(new CSNumberData(drop * -1.0));
		return base.Update();
	}

	private static double GetDrop(double vBelowInput, CSData vBaseObject, double vBaseOffset, CSGemData gem, Transform gemTransform)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData cSData = vBaseObject.Duplicate();
		Brep val = null;
		Curve val2 = (cSData as ICSCurve)?.GetCurve();
		if (val2 != null)
		{
			Curve val3 = val2.DuplicateCurve();
			Curve val4 = val2.DuplicateCurve();
			((GeometryBase)val3).Translate(Vector3d.YAxis * 50.0);
			((GeometryBase)val4).Translate(Vector3d.YAxis * -50.0);
			val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
		}
		if (val == null && cSData is ICSBrep iCSBrep)
		{
			val = iCSBrep.GetBrep();
		}
		if (val == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		if (gemTransform != Transform.Identity)
		{
			xUp = xUp * gemTransform * Transform.Translation(0.0, 0.0, 0.0 - vBelowInput);
		}
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
}
