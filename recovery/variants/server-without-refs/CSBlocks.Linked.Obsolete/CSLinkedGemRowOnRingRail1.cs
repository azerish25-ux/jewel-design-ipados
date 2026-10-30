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
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
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
		if (!CSUtilGeometry.OrientToCurveNalp((Curve)(object)value, 0.0, ref x))
		{
			return UpdateError("OrienttoCurveNalp failed");
		}
		Transform val = Transform.Translation(0.0, 0.0, value4);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value5), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		Point3d val3 = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y - ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref boundingBox)).Min;
		double x2 = ((Point3d)(ref val3)).X;
		val3 = ((BoundingBox)(ref boundingBox)).Max;
		double x3 = ((Point3d)(ref val3)).X;
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(x2 * (1.0 - value6 / 100.0), 0.0, 0.0);
		((Point3d)(ref val4)).Transform(x * val);
		Vector3d val5 = new Vector3d(val4);
		double length = ((Vector3d)(ref val5)).Length;
		Transform val6;
		if (flag)
		{
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(value7, value8, length, ref intersectPoint);
			val6 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), val4, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val6 = Transform.Identity;
		}
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(x3 * (1.0 - value6 / 100.0), 0.0, 0.0);
		Vector3d vectorStore = default(Vector3d);
		((Point3d)(ref val7)).Transform(val6 * x * val);
		((Vector3d)(ref vectorStore))._002Ector(val7);
		((Vector3d)(ref vectorStore)).Unitize();
		double num2 = num + value3;
		double num3 = num2 * (value2 - 1.0) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; (double)i < value2; i++)
		{
			Transform val8 = Transform.Translation(0.0, num3 - (double)i * num2, 0.0);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, val8 * val6 * x * val * val2);
			cSDataArray.Append(data);
		}
		oGem.SetData(cSDataArray);
		oRightRailPoint.SetData(new CSPointData(val7));
		oRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
