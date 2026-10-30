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
[CSBlock("HeadSetup_V1", "{113E106A-78F8-4C41-A4D7-821634142B7C}")]
public sealed class CSHeadSetup1 : CSBlock
{
	public readonly CSInputChannel iGems;

	public readonly CSInputChannel iStyle;

	public readonly CSInputChannel InHeadHeight;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InBaseOffset;

	public readonly CSInputChannel InAngleX;

	public readonly CSInputChannel InAngleY;

	public readonly CSInputChannel InMinimumBaseSpacing;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSOutputChannel OutBaseObject;

	public readonly CSOutputChannel oGemShape;

	public readonly CSOutputChannel oGemX;

	public readonly CSOutputChannel oGemY;

	public readonly CSOutputChannel oGemZ;

	public readonly CSOutputChannel oGemZMin;

	public readonly CSOutputChannel oProngParamIndex;

	public readonly CSOutputChannel oProngOverlap;

	public readonly CSOutputChannel oProngArrangment;

	public readonly CSOutputChannel oProngType;

	public readonly CSOutputChannel oProngMoveDir;

	public readonly CSOutputChannel OutAngleX;

	public readonly CSOutputChannel OutAngleY;

	public CSHeadSetup1(string identifier)
		: base(identifier)
	{
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		iGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InHeadHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBaseOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMinimumBaseSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		OutBaseObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		oGemShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		oGemX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProngParamIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		oProngOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		oProngArrangment = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		oProngType = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		oProngMoveDir = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutAngleX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutAngleY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!iGems.GetValue(out CSGemData value))
		{
			return InputError();
		}
		if (!iStyle.GetValue(out int value2))
		{
			return InputError();
		}
		ValueListItem whichStyle = (ValueListItem)value2;
		List<CSHeadStyles5.HeadProngInfo> headStyle = CSHeadStyles5.GetHeadStyle(value.GemShape, whichStyle);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		foreach (CSHeadStyles5.HeadProngInfo item in headStyle)
		{
			cSDataArray.Array.Add(new CSNumberData(item.ProngParamIndex));
			cSDataArray2.Array.Add(new CSNumberData(item.Overlap));
			cSDataArray3.Array.Add(new CSNumberData((double)item.Arrangment));
			cSDataArray4.Array.Add(new CSNumberData((double)item.Type));
			cSDataArray5.Array.Add(new CSNumberData(item.MoveDir));
		}
		InHeadHeight.GetValue(out double value3);
		value3 = 0.0 - value3;
		CSData data = InBaseObject.Data;
		InBaseOffset.GetValue(out double value4);
		InAngleX.GetValue(out double value5);
		InAngleY.GetValue(out double value6);
		InMinimumBaseSpacing.GetValue(out double value7);
		InRailX.GetValue(out double value8);
		value7 = value8 * 2.0 + value7;
		InBaseStyle.GetValue(out int value9);
		int num = value9;
		double drop = GetDrop(value3, data, value4, value);
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value5));
		double num3 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value6));
		if (value.GemXSize + num2 * 2.0 < value7)
		{
			value5 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((value7 - value.GemXSize) / 2.0 / drop));
		}
		double num4 = value.GemYSize + num3 * 2.0;
		if (num4 < value7)
		{
			num4 = value7;
			value6 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num4 - value.GemYSize) / 2.0 / drop));
		}
		if (num != 122)
		{
			num4 = 0.05;
			value5 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((0.05 - value.GemXSize) / 2.0 / drop));
			value6 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num4 - value.GemYSize) / 2.0 / drop));
		}
		OutBaseObject.SetData(data);
		oGemShape.SetData(new CSStringData(value.GemShape));
		oGemX.SetData(new CSNumberData(value.GemXSize));
		oGemY.SetData(new CSNumberData(value.GemYSize));
		oGemZ.SetData(new CSNumberData(value.GemZSize));
		oGemZMin.SetData(new CSNumberData(value.GemZMin));
		oProngParamIndex.SetData(cSDataArray);
		oProngOverlap.SetData(cSDataArray2);
		oProngArrangment.SetData(cSDataArray3);
		oProngType.SetData(cSDataArray4);
		oProngMoveDir.SetData(cSDataArray5);
		OutAngleX.SetData(new CSNumberData(value5));
		OutAngleY.SetData(new CSNumberData(value6));
		return base.Update();
	}

	private double GetDrop(double vBelowInput, CSData vBaseObject, double vBaseOffset, CSGemData gem)
	{
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData cSData = vBaseObject.Duplicate();
		Brep brep = null;
		if (cSData is ICSCurve iCSCurve)
		{
			Curve curve = iCSCurve.GetCurve();
			if (curve != null)
			{
				Curve curve2 = curve.DuplicateCurve();
				Curve curve3 = curve.DuplicateCurve();
				curve2.Translate(Vector3d.YAxis * 50.0);
				curve3.Translate(Vector3d.YAxis * -50.0);
				brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
			}
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
