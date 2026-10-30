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
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData cSData = vBaseObject.Duplicate();
		Brep brep = null;
		Curve curve = (cSData as ICSCurve)?.GetCurve();
		if (curve != null)
		{
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Translate(Vector3d.YAxis * 50.0);
			curve3.Translate(Vector3d.YAxis * -50.0);
			brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
		}
		if (brep == null && cSData is ICSBrep iCSBrep)
		{
			brep = iCSBrep.GetBrep();
		}
		if (brep == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		if (gemTransform != Transform.Identity)
		{
			xUp = xUp * gemTransform * Transform.Translation(0.0, 0.0, 0.0 - vBelowInput);
		}
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
}
