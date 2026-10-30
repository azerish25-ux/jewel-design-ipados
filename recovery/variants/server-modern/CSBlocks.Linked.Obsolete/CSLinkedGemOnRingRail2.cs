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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
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
		if (!CSUtilGeometry.OrientToCurveNalp((Curve)(object)value, 0.0, ref x))
		{
			return UpdateError("OrientToCurveNalp Failed");
		}
		Transform val = Transform.Translation(0.0, 0.0, value2);
		Transform val2 = Transform.Translation(0.0, value3, 0.0);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Failed");
		}
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Min;
		double x2 = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		double x3 = ((Point3d)(ref val4)).X;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(x2 * (1.0 - value5 / 100.0), 0.0, 0.0);
		((Point3d)(ref val5)).Transform(x * val);
		Transform val7;
		if (flag)
		{
			Vector3d val6 = new Vector3d(val5);
			double length = ((Vector3d)(ref val6)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value6, value7, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), val5, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val7 = Transform.Identity;
		}
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(x3 * (1.0 - value5 / 100.0), 0.0, 0.0);
		((Point3d)(ref val8)).Transform(val7 * x * val);
		Vector3d vectorStore = default(Vector3d);
		((Vector3d)(ref vectorStore))._002Ector(val8);
		((Vector3d)(ref vectorStore)).Unitize();
		CSData data = cSGemData.Duplicate();
		CSDataHelper.Transform(data, val7 * x * val * val2 * val3);
		OutGem.SetData(data);
		OutRightRailPoint.SetData(new CSPointData(val8));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
