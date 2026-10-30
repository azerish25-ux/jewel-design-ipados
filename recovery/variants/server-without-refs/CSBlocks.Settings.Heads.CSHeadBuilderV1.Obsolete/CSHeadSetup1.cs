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
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData cSData = vBaseObject.Duplicate();
		Brep val = null;
		if (cSData is ICSCurve iCSCurve)
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
		if (val == null && cSData is ICSBrep iCSBrep)
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
}
