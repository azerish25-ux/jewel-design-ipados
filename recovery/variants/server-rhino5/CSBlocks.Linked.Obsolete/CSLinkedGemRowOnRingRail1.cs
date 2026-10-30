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
[CSBlock("LinkedGemRowOnRingRail_V1", "{C18B1047-BD50-4cc3-A730-443E8F033B85}")]
public sealed class CSLinkedGemRowOnRingRail1 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iGemCount;

	public readonly CSInputChannel iGemSpacing;

	public readonly CSInputChannel iGemZOffset;

	public readonly CSInputChannel iGemRotation;

	public readonly CSInputChannel iGemNudge;

	public readonly CSInputChannel iLastRightRailPoint;

	public readonly CSInputChannel iLastRightRailVector;

	public readonly CSOutputChannel oGem;

	public readonly CSOutputChannel oRightRailPoint;

	public readonly CSOutputChannel oRightRailVector;

	public CSLinkedGemRowOnRingRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		oRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		oRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem Definition Missing");
		}
		double value2 = 0.0;
		iGemCount.GetDouble(ref value2);
		double value3 = 0.0;
		iGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		iGemZOffset.GetDouble(ref value4);
		double value5 = 0.0;
		iGemRotation.GetDouble(ref value5);
		double value6 = 0.0;
		iGemNudge.GetDouble(ref value6);
		bool flag = true;
		Point3d value7 = Point3d.Unset;
		CSDataHelper.GetPoint(iLastRightRailPoint.Data, out value7);
		if (value7 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value8 = Vector3d.Unset;
		CSDataHelper.GetVector(iLastRightRailVector.Data, out value8);
		if (value8 == Vector3d.Unset)
		{
			flag = false;
		}
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(value, 0.0, ref x))
		{
			return UpdateError("OrienttoCurveNalp failed");
		}
		Transform transform = Transform.Translation(0.0, 0.0, value4);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		double num = boundingBox.Max.Y - boundingBox.Min.Y;
		double x2 = boundingBox.Min.X;
		double x3 = boundingBox.Max.X;
		Point3d point3d = new Point3d(x2 * (1.0 - value6 / 100.0), 0.0, 0.0);
		point3d.Transform(x * transform);
		double length = new Vector3d(point3d).Length;
		Transform transform3;
		if (flag)
		{
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value7, value8, length, ref intersectPoint);
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform3 = Transform.Identity;
		}
		Point3d point3d2 = new Point3d(x3 * (1.0 - value6 / 100.0), 0.0, 0.0);
		Vector3d vector3d = default(Vector3d);
		point3d2.Transform(transform3 * x * transform);
		vector3d = new Vector3d(point3d2);
		vector3d.Unitize();
		double num2 = num + value3;
		double num3 = num2 * (value2 - 1.0) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; (double)i < value2; i++)
		{
			Transform transform4 = Transform.Translation(0.0, num3 - (double)i * num2, 0.0);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, transform4 * transform3 * x * transform * transform2);
			cSDataArray.Append(data);
		}
		oGem.SetData(cSDataArray);
		oRightRailPoint.SetData(new CSPointData(point3d2));
		oRightRailVector.SetData(new CSVectorData(vector3d));
		return base.Update();
	}
}
