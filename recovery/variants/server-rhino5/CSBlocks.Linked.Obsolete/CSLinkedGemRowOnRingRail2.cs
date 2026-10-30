using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked.Obsolete;

[Obsolete]
[CSBlock("LinkedGemRowOnRingRail_V2", "{F88636A4-10DF-40ec-8ADD-0F3CC274FD02}")]
public sealed class CSLinkedGemRowOnRingRail2 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemNudge;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedGemRowOnRingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		if (!(InGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem Definition Input Missing");
		}
		double value2 = 0.0;
		InGemCount.GetDouble(ref value2);
		double value3 = 0.0;
		InGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		InGemZOffset.GetDouble(ref value4);
		double value5 = 0.0;
		InGemYOffset.GetDouble(ref value5);
		double value6 = 0.0;
		InGemRotation.GetDouble(ref value6);
		double value7 = 0.0;
		InGemNudge.GetDouble(ref value7);
		bool flag = true;
		Point3d value8 = Point3d.Unset;
		CSDataHelper.GetPoint(InLastRightRailPoint, out value8);
		if (value8 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value9 = Vector3d.Unset;
		CSDataHelper.GetVector(InLastRightRailVector.Data, out value9);
		if (value9 == Vector3d.Unset)
		{
			flag = false;
		}
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(value, 0.0, ref x))
		{
			return UpdateError("OrientToCurveNalp Failed");
		}
		Transform transform = Transform.Translation(0.0, 0.0, value4);
		Transform transform2 = Transform.Translation(0.0, value5, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		double num = boundingBox.Max.Y - boundingBox.Min.Y;
		double x2 = boundingBox.Min.X;
		double x3 = boundingBox.Max.X;
		Point3d point3d = new Point3d(x2 * (1.0 - value7 / 100.0), 0.0, 0.0);
		point3d.Transform(x * transform);
		Transform transform4;
		if (flag)
		{
			double length = new Vector3d(point3d).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value8, value9, length, ref intersectPoint);
			transform4 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform4 = Transform.Identity;
		}
		Point3d point3d2 = new Point3d(x3 * (1.0 - value7 / 100.0), 0.0, 0.0);
		point3d2.Transform(transform4 * x * transform);
		Vector3d vectorStore = new Vector3d(point3d2);
		vectorStore.Unitize();
		double num2 = num + value3;
		double num3 = num2 * (value2 - 1.0) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; (double)i < value2; i++)
		{
			Transform transform5 = Transform.Translation(0.0, num3 - (double)i * num2, 0.0);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, transform5 * transform4 * x * transform * transform2 * transform3);
			cSDataArray.Append(data);
		}
		OutGem.SetData(cSDataArray);
		OutRightRailPoint.SetData(new CSPointData(point3d2));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
