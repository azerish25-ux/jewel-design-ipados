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
[CSBlock("LinkedGemOnRingRail_V2", "{2714D62D-F3E0-4f52-BC17-3539FA0C7C3A}")]
public sealed class CSLinkedGemOnRingRail2 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemNudge;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedGemOnRingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
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
			return UpdateMissing("Rail Input missing");
		}
		if (!(InGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem Definition missing");
		}
		double value2 = 0.0;
		InGemZOffset.GetDouble(ref value2);
		double value3 = 0.0;
		InGemYOffset.GetDouble(ref value3);
		double value4 = 0.0;
		InGemRotation.GetDouble(ref value4);
		double value5 = 0.0;
		InGemNudge.GetDouble(ref value5);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint, out var value6);
		if (value6 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value7);
		if (value7 == Vector3d.Unset)
		{
			flag = false;
		}
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(value, 0.0, ref x))
		{
			return UpdateError("OrientToCurveNalp Failed");
		}
		Transform transform = Transform.Translation(0.0, 0.0, value2);
		Transform transform2 = Transform.Translation(0.0, value3, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Failed");
		}
		double x2 = boundingBox.Min.X;
		double x3 = boundingBox.Max.X;
		Point3d point3d = new Point3d(x2 * (1.0 - value5 / 100.0), 0.0, 0.0);
		point3d.Transform(x * transform);
		Transform transform4;
		if (flag)
		{
			double length = new Vector3d(point3d).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value6, value7, length, ref intersectPoint);
			transform4 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform4 = Transform.Identity;
		}
		Point3d point3d2 = new Point3d(x3 * (1.0 - value5 / 100.0), 0.0, 0.0);
		point3d2.Transform(transform4 * x * transform);
		Vector3d vectorStore = new Vector3d(point3d2);
		vectorStore.Unitize();
		CSData data = cSGemData.Duplicate();
		CSDataHelper.Transform(data, transform4 * x * transform * transform2 * transform3);
		OutGem.SetData(data);
		OutRightRailPoint.SetData(new CSPointData(point3d2));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
