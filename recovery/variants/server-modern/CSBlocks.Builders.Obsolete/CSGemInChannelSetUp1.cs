using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Builders.Obsolete;

[Obsolete]
[CSBlock("GemInChannelSetUp_V1", "{7E2EFBE3-9EB9-47C5-A9CA-FBACB8EF259F}")]
public sealed class CSGemInChannelSetUp1 : CSBlock
{
	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iProfileWidth;

	public readonly CSInputChannel iChannelWall;

	public readonly CSInputChannel iChannelSpacing;

	public readonly CSOutputChannel oBaseGem;

	public readonly CSOutputChannel oChannelHeight;

	public readonly CSOutputChannel oProfileParamLeft;

	public readonly CSOutputChannel oProfileParamRight;

	public readonly CSOutputChannel oProfile;

	public CSGemInChannelSetUp1(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWall = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBaseGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oChannelHeight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamLeft = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamRight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetNurbsCurve(data, out var value);
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double value2 = 0.0;
		iProfileWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iChannelWall.GetDouble(ref value3);
		double value4 = 0.0;
		iChannelSpacing.GetDouble(ref value4);
		double requestedSizeX = value2 - (value3 + value4) * 2.0;
		if (!CSUtilGem.GetGemScaleFromTableX(cSGemData, requestedSizeX, false, false, out Transform xBestScale))
		{
			return UpdateError("GetGemSizeFromTableX failed");
		}
		CSData data2 = cSGemData.Duplicate();
		CSDataHelper.Transform(data2, xBestScale);
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(true);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = (0.0 - (x + ((Point3d)(ref val)).X)) / 2.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val2 = Transform.Translation(num, 0.0 - ((Point3d)(ref val)).Y, 0.0);
		Plane worldXY = Plane.WorldXY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = 1.0 / (x2 - ((Point3d)(ref val)).X);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val3 = Transform.Scale(worldXY, num2, 1.0 / (y - ((Point3d)(ref val)).Y), 0.0);
		Transform val4 = Transform.Scale(Plane.WorldXY, value2, 1.0, 1.0);
		NurbsCurve val5 = new NurbsCurve(value);
		((GeometryBase)val5).Transform(val4 * val3 * val2);
		double num3 = value2 / 2.0 - value3;
		LineCurve val6 = new LineCurve(new Point3d(num3, -1.0, 0.0), new Point3d(num3, 2.0, 0.0));
		CurveIntersections obj = Intersection.CurveCurve((Curve)(object)val5, (Curve)(object)val6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num4 = 0.0;
		double number = 0.0;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointB = item.PointB;
			if (((Point3d)(ref pointB)).Y > num4)
			{
				num4 = ((Point3d)(ref pointB)).Y;
				number = item.ParameterA;
			}
		}
		LineCurve val7 = new LineCurve(new Point3d(0.0 - num3, -1.0, 0.0), new Point3d(0.0 - num3, 2.0, 0.0));
		CurveIntersections obj2 = Intersection.CurveCurve((Curve)(object)val5, (Curve)(object)val7, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num5 = 0.0;
		double number2 = 0.0;
		foreach (IntersectionEvent item2 in obj2)
		{
			Point3d pointB2 = item2.PointB;
			if (((Point3d)(ref pointB2)).Y > num5)
			{
				num5 = ((Point3d)(ref pointB2)).Y;
				number2 = item2.ParameterA;
			}
		}
		double number3 = (num4 + num5) / 2.0;
		oBaseGem.SetData(data2);
		oChannelHeight.SetData(new CSNumberData(number3));
		oProfileParamLeft.SetData(new CSNumberData(number2));
		oProfileParamRight.SetData(new CSNumberData(number));
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)val5));
		return base.Update();
	}
}
