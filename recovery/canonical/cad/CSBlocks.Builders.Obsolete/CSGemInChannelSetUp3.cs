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
[CSBlock("GemInChannelSetUp_V3", "{FF4F5337-8D03-4285-8EC7-6497F6E17CA8}")]
public sealed class CSGemInChannelSetUp3 : CSBlock
{
	public CSInputChannel iChannelMode;

	public CSInputChannel iGemSpacingProng;

	public CSInputChannel iWallDropProng;

	public CSInputChannel iChannelWallProng;

	public CSInputChannel iChannelSpacingProng;

	public CSInputChannel iChannelEndSpacingProng;

	public CSInputChannel iGemSpacingChannel;

	public CSInputChannel iWallDropChannel;

	public CSInputChannel iChannelWallChannel;

	public CSInputChannel iChannelSpacingChannel;

	public CSInputChannel iChannelEndSpacingChannel;

	public CSInputChannel iProfile;

	public CSInputChannel iGemDefinition;

	public CSInputChannel iProfileWidth;

	public CSInputChannel iGemSize;

	public CSInputChannel iExtraWidth;

	public CSOutputChannel oBaseGem;

	public CSOutputChannel oChannelHeight;

	public CSOutputChannel oProfileParamLeft;

	public CSOutputChannel oProfileParamRight;

	public CSOutputChannel oProfile;

	public CSOutputChannel oGemSpacing;

	public CSOutputChannel oWallDrop;

	public CSOutputChannel oChannelSpacing;

	public CSOutputChannel oChannelEndSpacing;

	public CSOutputChannel oGemSizeLoopBack;

	public CSOutputChannel oProfileWidthLoopBack;

	public CSGemInChannelSetUp3(string identifier)
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
		BeenUpdatedAfterLastUpdate(iGemSize.OutputChannelBlock);
		double value3 = 0.0;
		iProfileWidth.GetDouble(ref value3);
		double value4 = 0.0;
		double value5 = 0.0;
		bool num = BeenUpdatedAfterLastUpdate(iProfileWidth.OutputChannelBlock);
		bool flag;
		bool flag2;
		if (value)
		{
			flag = BeenUpdatedAfterLastUpdate(iChannelWallChannel.OutputChannelBlock);
			flag2 = BeenUpdatedAfterLastUpdate(iChannelSpacingChannel.OutputChannelBlock);
			iChannelWallChannel.GetDouble(ref value4);
			iChannelSpacingChannel.GetDouble(ref value5);
		}
		else
		{
			flag = BeenUpdatedAfterLastUpdate(iChannelWallProng.OutputChannelBlock);
			flag2 = BeenUpdatedAfterLastUpdate(iChannelSpacingProng.OutputChannelBlock);
			iChannelWallProng.GetDouble(ref value4);
			iChannelSpacingProng.GetDouble(ref value5);
		}
		double value6 = 0.0;
		double requestedSizeY = 0.0;
		if (iGemSize.Data is CSGemData cSGemData2)
		{
			value6 = cSGemData2.GemXSize;
			requestedSizeY = cSGemData2.GemYSize;
		}
		else
		{
			iGemSize.GetDouble(ref value6);
		}
		if (num || flag || flag2)
		{
			value6 = value3 - (value4 + value5) * 2.0;
			CSUtilGem.GetGemSizeFromTable(cSGemData, value6, requestedSizeY, useLargestPossible: false, useIterationOnly: false, out var bestSize);
			value6 = bestSize.X;
			requestedSizeY = bestSize.Y;
		}
		else
		{
			value3 = value6 + (value4 + value5) * 2.0;
		}
		oGemSizeLoopBack.SetData(new CSNumberData(value6));
		oProfileWidthLoopBack.SetData(new CSNumberData(value3));
		double value7 = 0.0;
		iExtraWidth.GetDouble(ref value7);
		value3 += value7;
		value4 += value7 / 2.0;
		if (!CSUtilGem.GetGemScaleFromTable(cSGemData, value6, requestedSizeY, false, false, out Transform xBestScale))
		{
			return UpdateError("GetGemSizeFromTable failed");
		}
		CSData data2 = cSGemData.Duplicate();
		CSDataHelper.Transform(data2, xBestScale);
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		Transform transform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
		Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / (boundingBox.Max.X - boundingBox.Min.X), 1.0 / (boundingBox.Max.Y - boundingBox.Min.Y), 0.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, value3, 1.0, 1.0);
		NurbsCurve nurbsCurve = new NurbsCurve(value2);
		nurbsCurve.Transform(transform3 * transform2 * transform);
		double num2 = value3 / 2.0 - value4;
		LineCurve curveB = new LineCurve(new Point3d(num2, -1.0, 0.0), new Point3d(num2, 2.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(nurbsCurve, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num3 = 0.0;
		double number = 0.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointB = item.PointB;
			if (pointB.Y > num3)
			{
				num3 = pointB.Y;
				number = item.ParameterA;
			}
		}
		LineCurve curveB2 = new LineCurve(new Point3d(0.0 - num2, -1.0, 0.0), new Point3d(0.0 - num2, 2.0, 0.0));
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(nurbsCurve, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num4 = 0.0;
		double number2 = 0.0;
		foreach (IntersectionEvent item2 in curveIntersections2)
		{
			Point3d pointB2 = item2.PointB;
			if (pointB2.Y > num4)
			{
				num4 = pointB2.Y;
				number2 = item2.ParameterA;
			}
		}
		double number3 = (num3 + num4) / 2.0;
		oBaseGem.SetData(data2);
		oChannelHeight.SetData(new CSNumberData(number3));
		oProfileParamLeft.SetData(new CSNumberData(number2));
		oProfileParamRight.SetData(new CSNumberData(number));
		oProfile.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
