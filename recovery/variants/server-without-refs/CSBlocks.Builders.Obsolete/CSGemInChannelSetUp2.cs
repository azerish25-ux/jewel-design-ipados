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
[CSBlock("GemInChannelSetUp_V2", "{CD50CDE5-C0B3-46E1-A7F2-2973FE6680CC}")]
public sealed class CSGemInChannelSetUp2 : CSBlock
{
	public readonly CSInputChannel iChannelMode;

	public readonly CSInputChannel iGemSpacingProng;

	public readonly CSInputChannel iWallDropProng;

	public readonly CSInputChannel iChannelWallProng;

	public readonly CSInputChannel iChannelSpacingProng;

	public readonly CSInputChannel iChannelEndSpacingProng;

	public readonly CSInputChannel iGemSpacingChannel;

	public readonly CSInputChannel iWallDropChannel;

	public readonly CSInputChannel iChannelWallChannel;

	public readonly CSInputChannel iChannelSpacingChannel;

	public readonly CSInputChannel iChannelEndSpacingChannel;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iProfileWidth;

	public readonly CSInputChannel iGemSize;

	public readonly CSInputChannel iExtraWidth;

	public readonly CSOutputChannel oBaseGem;

	public readonly CSOutputChannel oChannelHeight;

	public readonly CSOutputChannel oProfileParamLeft;

	public readonly CSOutputChannel oProfileParamRight;

	public readonly CSOutputChannel oProfile;

	public readonly CSOutputChannel oGemSpacing;

	public readonly CSOutputChannel oWallDrop;

	public readonly CSOutputChannel oChannelSpacing;

	public readonly CSOutputChannel oChannelEndSpacing;

	public readonly CSOutputChannel oGemSizeLoopBack;

	public readonly CSOutputChannel oProfileWidthLoopBack;

	public CSGemInChannelSetUp2(string identifier)
		: base(identifier)
	{
		iChannelMode = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iGemSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iExtraWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBaseGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oChannelHeight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamLeft = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileParamRight = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oGemSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oWallDrop = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelEndSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemSizeLoopBack = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfileWidthLoopBack = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		bool value = false;
		iChannelMode.GetBoolean(ref value);
		if (value)
		{
			oGemSpacing.SetData(iGemSpacingChannel.Data);
			oWallDrop.SetData(iWallDropChannel.Data);
			oChannelSpacing.SetData(iChannelSpacingChannel.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingChannel.Data);
		}
		else
		{
			oGemSpacing.SetData(iGemSpacingProng.Data);
			oWallDrop.SetData(iWallDropProng.Data);
			oChannelSpacing.SetData(iChannelSpacingProng.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingProng.Data);
		}
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetNurbsCurve(data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		bool flag = BeenUpdatedAfterLastUpdate(iGemSize.OutputChannelBlock);
		double value3 = 0.0;
		iProfileWidth.GetDouble(ref value3);
		double value4 = 0.0;
		double value5 = 0.0;
		bool num = BeenUpdatedAfterLastUpdate(iProfileWidth.OutputChannelBlock);
		bool flag2;
		bool flag3;
		if (value)
		{
			flag2 = BeenUpdatedAfterLastUpdate(iChannelWallChannel.OutputChannelBlock);
			flag3 = BeenUpdatedAfterLastUpdate(iChannelSpacingChannel.OutputChannelBlock);
			iChannelWallChannel.GetDouble(ref value4);
			iChannelSpacingChannel.GetDouble(ref value5);
		}
		else
		{
			flag2 = BeenUpdatedAfterLastUpdate(iChannelWallProng.OutputChannelBlock);
			flag3 = BeenUpdatedAfterLastUpdate(iChannelSpacingProng.OutputChannelBlock);
			iChannelWallProng.GetDouble(ref value4);
			iChannelSpacingProng.GetDouble(ref value5);
		}
		double value6 = 0.0;
		if (iGemSize.Data is CSGemData cSGemData2)
		{
			value6 = cSGemData2.GemXSize;
		}
		iGemSize.GetDouble(ref value6);
		if (!(num || flag2 || flag3))
		{
			value3 = ((!flag) ? (value6 + (value4 + value5) * 2.0) : (value6 + (value4 + value5) * 2.0));
		}
		else
		{
			value6 = value3 - (value4 + value5) * 2.0;
			CSUtilGem.GetGemSizeFromTableX(cSGemData, value6, useLargestPossible: false, useIterationOnly: false, out var bestSize);
			value6 = bestSize.X;
		}
		oGemSizeLoopBack.SetData(new CSNumberData(value6));
		oProfileWidthLoopBack.SetData(new CSNumberData(value3));
		double value7 = 0.0;
		iExtraWidth.GetDouble(ref value7);
		value3 += value7;
		value4 += value7 / 2.0;
		if (!CSUtilGem.GetGemScaleFromTableX(cSGemData, value6, useLargestPossible: false, useIterationOnly: false, out Transform xBestScale))
		{
			return UpdateError("GetGemSizeFromTableX failed");
		}
		CSData data2 = cSGemData.Duplicate();
		CSDataHelper.Transform(data2, xBestScale);
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = (0.0 - (x + ((Point3d)(ref val)).X)) / 2.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val2 = Transform.Translation(num2, 0.0 - ((Point3d)(ref val)).Y, 0.0);
		Plane worldXY = Plane.WorldXY;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = 1.0 / (x2 - ((Point3d)(ref val)).X);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val3 = Transform.Scale(worldXY, num3, 1.0 / (y - ((Point3d)(ref val)).Y), 0.0);
		Transform val4 = Transform.Scale(Plane.WorldXY, value3, 1.0, 1.0);
		NurbsCurve val5 = new NurbsCurve(value2);
		((GeometryBase)val5).Transform(val4 * val3 * val2);
		double num4 = value3 / 2.0 - value4;
		LineCurve val6 = new LineCurve(new Point3d(num4, -1.0, 0.0), new Point3d(num4, 2.0, 0.0));
		CurveIntersections obj = Intersection.CurveCurve((Curve)(object)val5, (Curve)(object)val6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num5 = 0.0;
		double number = 0.0;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointB = item.PointB;
			if (((Point3d)(ref pointB)).Y > num5)
			{
				num5 = ((Point3d)(ref pointB)).Y;
				number = item.ParameterA;
			}
		}
		LineCurve val7 = new LineCurve(new Point3d(0.0 - num4, -1.0, 0.0), new Point3d(0.0 - num4, 2.0, 0.0));
		CurveIntersections obj2 = Intersection.CurveCurve((Curve)(object)val5, (Curve)(object)val7, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num6 = 0.0;
		double number2 = 0.0;
		foreach (IntersectionEvent item2 in obj2)
		{
			Point3d pointB2 = item2.PointB;
			if (((Point3d)(ref pointB2)).Y > num6)
			{
				num6 = ((Point3d)(ref pointB2)).Y;
				number2 = item2.ParameterA;
			}
		}
		double number3 = (num5 + num6) / 2.0;
		oBaseGem.SetData(data2);
		oChannelHeight.SetData(new CSNumberData(number3));
		oProfileParamLeft.SetData(new CSNumberData(number2));
		oProfileParamRight.SetData(new CSNumberData(number));
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)val5));
		return base.Update();
	}
}
