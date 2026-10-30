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
[CSBlock("LinkedProfileOnRingRail_V1", "{77268A3B-891E-4e8e-9BD3-8A4760F853B3}")]
public sealed class CSLinkedProfileOnRingRail1 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iProfileX;

	public readonly CSInputChannel iProfileY;

	public readonly CSInputChannel iAOffset;

	public readonly CSInputChannel iLastRightRailPoint;

	public readonly CSInputChannel iLastRightRailVector;

	public readonly CSOutputChannel oProfile;

	public readonly CSOutputChannel oRightRailPoint;

	public readonly CSOutputChannel oRightRailVector;

	public CSLinkedProfileOnRingRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		oRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		CSDataHelper.GetNurbsCurve(iProfile.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double value3 = 0.0;
		iProfileX.GetDouble(ref value3);
		double value4 = 0.0;
		iProfileY.GetDouble(ref value4);
		double value5 = 0.0;
		iAOffset.GetDouble(ref value5);
		bool flag = true;
		Point3d value6 = Point3d.Unset;
		CSDataHelper.GetPoint(iLastRightRailPoint.Data, out value6);
		if (value6 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value7 = Vector3d.Unset;
		CSDataHelper.GetVector(iLastRightRailVector.Data, out value7);
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		Plane worldXY = Plane.WorldXY;
		double num = value3;
		Vector3d val = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = num / ((Vector3d)(ref val)).X;
		double num3 = value4;
		val = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val2 = Transform.Scale(worldXY, num2, num3 / ((Vector3d)(ref val)).Y, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 0.0, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 1.0, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0, 0.0, 0.0);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(0.0, 1.0, 0.0);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, 0.0, ref x, 0.0);
		((Point3d)(ref val3)).Transform(x);
		((Point3d)(ref val4)).Transform(x);
		((Point3d)(ref val5)).Transform(x);
		((Point3d)(ref val6)).Transform(x);
		Vector3d inputVC = default(Vector3d);
		inputVC = val4 - val3;
		((Vector3d)(ref inputVC)).Unitize();
		double num4 = value5;
		val = new Vector3d(val3);
		Transform val7 = CSBlockUtil.CSUtilMath.Rotation(num4 / ((Vector3d)(ref val)).Length, Vector3d.YAxis, Point3d.Origin);
		Transform val8;
		if (flag)
		{
			val = new Vector3d(value6);
			double length = ((Vector3d)(ref val)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val3, inputVC, length, ref intersectPoint);
			val8 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value6, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val8 = Transform.Identity;
		}
		val8 *= val7;
		((Point3d)(ref val5)).Transform(val8);
		((Point3d)(ref val6)).Transform(val8);
		Vector3d vectorStore = default(Vector3d);
		vectorStore = val6 - val5;
		((Vector3d)(ref vectorStore)).Unitize();
		CSData data = iProfile.Data.Duplicate();
		CSDataHelper.Transform(data, val8 * x * val2);
		oProfile.SetData(data);
		oRightRailPoint.SetData(new CSPointData(val5));
		oRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
