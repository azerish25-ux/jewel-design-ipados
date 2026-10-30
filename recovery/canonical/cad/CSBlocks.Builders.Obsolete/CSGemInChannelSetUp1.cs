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
		BoundingBox boundingBox = value.GetBoundingBox(accurate: true);
		Transform transform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
		Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / (boundingBox.Max.X - boundingBox.Min.X), 1.0 / (boundingBox.Max.Y - boundingBox.Min.Y), 0.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, value2, 1.0, 1.0);
		NurbsCurve nurbsCurve = new NurbsCurve(value);
		nurbsCurve.Transform(transform3 * transform2 * transform);
		double num = value2 / 2.0 - value3;
		LineCurve curveB = new LineCurve(new Point3d(num, -1.0, 0.0), new Point3d(num, 2.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(nurbsCurve, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num2 = 0.0;
		double number = 0.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointB = item.PointB;
			if (pointB.Y > num2)
			{
				num2 = pointB.Y;
				number = item.ParameterA;
			}
		}
		LineCurve curveB2 = new LineCurve(new Point3d(0.0 - num, -1.0, 0.0), new Point3d(0.0 - num, 2.0, 0.0));
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(nurbsCurve, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num3 = 0.0;
		double number2 = 0.0;
		foreach (IntersectionEvent item2 in curveIntersections2)
		{
			Point3d pointB2 = item2.PointB;
			if (pointB2.Y > num3)
			{
				num3 = pointB2.Y;
				number2 = item2.ParameterA;
			}
		}
		double number3 = (num2 + num3) / 2.0;
		oBaseGem.SetData(data2);
		oChannelHeight.SetData(new CSNumberData(number3));
		oProfileParamLeft.SetData(new CSNumberData(number2));
		oProfileParamRight.SetData(new CSNumberData(number));
		oProfile.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
