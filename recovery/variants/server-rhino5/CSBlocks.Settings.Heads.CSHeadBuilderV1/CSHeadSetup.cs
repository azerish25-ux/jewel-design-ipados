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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

[CSBlock("HeadSetup", "{6B7126E7-7BD4-4AF7-A4C9-77012053A643}")]
public sealed class CSHeadSetup : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InStyle;

	public readonly CSInputChannel InHeadHeight;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InBaseOffset;

	public readonly CSInputChannel InAngleX;

	public readonly CSInputChannel InAngleY;

	public readonly CSInputChannel InMinimumBaseSpacing;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSInputChannel InGemTransform;

	public readonly CSOutputChannel OutBaseObject;

	public readonly CSOutputChannel OutGemShape;

	public readonly CSOutputChannel OutGemX;

	public readonly CSOutputChannel OutGemY;

	public readonly CSOutputChannel OutGemZ;

	public readonly CSOutputChannel OutGemZMin;

	public readonly CSOutputChannel OutGemZMax;

	public readonly CSOutputChannel OutProngParamIndex;

	public readonly CSOutputChannel OutProngOverlap;

	public readonly CSOutputChannel OutProngArrangment;

	public readonly CSOutputChannel OutProngType;

	public readonly CSOutputChannel OutProngMoveDir;

	public readonly CSOutputChannel OutAngleX;

	public readonly CSOutputChannel OutAngleY;

	public readonly CSOutputChannel OutRailDistance;

	public CSHeadSetup(string identifier)
		: base(identifier)
	{
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InHeadHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InBaseOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InAngleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InMinimumBaseSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		InGemTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Identity);
		OutBaseObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		OutGemShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		OutGemX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutProngParamIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array);
		OutProngArrangment = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngType = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutProngMoveDir = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array);
		OutAngleX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutAngleY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRailDistance = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSGemData gem = InGems.Gem;
		IEnumerable<CSHeadStyles.HeadProngInfo> headProngInfo = CSHeadStyles.GetHeadProngInfo((HeadStyle)InStyle.Integer);
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
		double vBelowInput = 0.0 - InHeadHeight.Double;
		CSData data = InBaseObject.Data;
		double vBaseOffset = InBaseOffset.Double;
		double num = InAngleX.Double;
		double num2 = InAngleY.Double;
		double num3 = InMinimumBaseSpacing.Double;
		num3 = InRailX.Double * 2.0 + num3;
		ValueListItem integer = (ValueListItem)InBaseStyle.Integer;
		Transform transform = InGemTransform.Transform;
		double drop = GetDrop(vBelowInput, data, vBaseOffset, gem, transform);
		double num4 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num));
		double num5 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num2));
		if (gem.GemXSize + num4 * 2.0 < num3)
		{
			num = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num3 - gem.GemXSize) / 2.0 / drop));
		}
		double num6 = gem.GemYSize + num5 * 2.0;
		if (num6 < num3)
		{
			num6 = num3;
			num2 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num6 - gem.GemYSize) / 2.0 / drop));
		}
		if (integer != ValueListItem.HeadBuilderProngHead)
		{
			num6 = 0.05;
			num = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((0.05 - gem.GemXSize) / 2.0 / drop));
			num2 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num6 - gem.GemYSize) / 2.0 / drop));
		}
		OutBaseObject.SetData(data?.Duplicate());
		OutGemShape.SetData(new CSStringData(gem.GemShape));
		OutGemX.SetData(new CSNumberData(gem.GemXSize));
		OutGemY.SetData(new CSNumberData(gem.GemYSize));
		OutGemZ.SetData(new CSNumberData(gem.GemZSize));
		OutGemZMin.SetData(new CSNumberData(gem.GemZMin));
		OutGemZMax.SetData(new CSNumberData(gem.GemZMax));
		OutProngParamIndex.SetData(cSDataArray);
		OutProngOverlap.SetData(cSDataArray2);
		OutProngArrangment.SetData(cSDataArray3);
		OutProngType.SetData(cSDataArray4);
		OutProngMoveDir.SetData(cSDataArray5);
		OutAngleX.SetData(new CSNumberData(num));
		OutAngleY.SetData(new CSNumberData(num2));
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
