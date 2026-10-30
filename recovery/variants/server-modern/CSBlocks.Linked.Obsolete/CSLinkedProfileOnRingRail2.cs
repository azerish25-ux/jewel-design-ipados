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
[CSBlock("LinkedProfileOnRingRail_V2", "{86E5FE71-5DC5-4427-9613-11E9B93291C6}")]
public sealed class CSLinkedProfileOnRingRail2 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutXform;

	public readonly CSOutputChannel OutParam;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedProfileOnRingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		double value2 = 0.0;
		InAOffset.GetDouble(ref value2);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint.Data, out var value3);
		if (value3 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value4);
		if (value4 == Vector3d.Unset)
		{
			flag = false;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(Point3d.Origin);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(Vector3d.YAxis);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(Point3d.Origin);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(Vector3d.YAxis);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, 0.0, ref x, 0.0);
		((Point3d)(ref val)).Transform(x);
		((Point3d)(ref val2)).Transform(x);
		((Point3d)(ref val3)).Transform(x);
		((Point3d)(ref val4)).Transform(x);
		Vector3d inputVC = val2 - val;
		((Vector3d)(ref inputVC)).Unitize();
		double num = value2;
		Vector3d val5 = new Vector3d(val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num / ((Vector3d)(ref val5)).Length, Vector3d.YAxis, Point3d.Origin);
		Transform val7;
		if (flag)
		{
			val5 = new Vector3d(value3);
			double length = ((Vector3d)(ref val5)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val, inputVC, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value3, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val7 = Transform.Identity;
		}
		val7 *= val6;
		((Point3d)(ref val3)).Transform(val7);
		((Point3d)(ref val4)).Transform(val7);
		Vector3d vectorStore = val4 - val3;
		((Vector3d)(ref vectorStore)).Unitize();
		double number = default(double);
		((Curve)value).ClosestPoint(val3, ref number);
		OutXform.SetData(new CSXformData(val7 * x));
		OutParam.SetData(new CSNumberData(number));
		OutRightRailPoint.SetData(new CSPointData(val3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
