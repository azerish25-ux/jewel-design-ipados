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
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
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
		if (!CSUtilGeometry.OrientToCurveNalp((Curve)(object)value, 0.0, ref x))
		{
			return UpdateError("OrientToCurveNalp Failed");
		}
		Transform val = Transform.Translation(0.0, 0.0, value4);
		Transform val2 = Transform.Translation(0.0, value5, 0.0);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y - ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double x2 = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		double x3 = ((Point3d)(ref val4)).X;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(x2 * (1.0 - value7 / 100.0), 0.0, 0.0);
		((Point3d)(ref val5)).Transform(x * val);
		Transform val7;
		if (flag)
		{
			Vector3d val6 = new Vector3d(val5);
			double length = ((Vector3d)(ref val6)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value8, value9, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), val5, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val7 = Transform.Identity;
		}
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(x3 * (1.0 - value7 / 100.0), 0.0, 0.0);
		((Point3d)(ref val8)).Transform(val7 * x * val);
		Vector3d vectorStore = default(Vector3d);
		((Vector3d)(ref vectorStore))._002Ector(val8);
		((Vector3d)(ref vectorStore)).Unitize();
		double num2 = num + value3;
		double num3 = num2 * (value2 - 1.0) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; (double)i < value2; i++)
		{
			Transform val9 = Transform.Translation(0.0, num3 - (double)i * num2, 0.0);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, val9 * val7 * x * val * val2 * val3);
			cSDataArray.Append(data);
		}
		OutGem.SetData(cSDataArray);
		OutRightRailPoint.SetData(new CSPointData(val8));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
