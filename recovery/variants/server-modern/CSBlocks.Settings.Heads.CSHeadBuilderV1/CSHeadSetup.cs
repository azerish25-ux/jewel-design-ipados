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
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
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
