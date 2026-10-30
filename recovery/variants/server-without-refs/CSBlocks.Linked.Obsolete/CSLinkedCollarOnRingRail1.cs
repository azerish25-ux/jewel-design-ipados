using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked.Obsolete;

[Obsolete]
[CSBlock("LinkedCollarOnRingRail_V1", "{2F80F6CF-4CBC-4782-B387-87EA0C73DA70}")]
public sealed class CSLinkedCollarOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InCollarX;

	public readonly CSInputChannel InCollarY;

	public readonly CSInputChannel InCollarZ;

	public readonly CSInputChannel InCollarDome;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutCollar;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedCollarOnRingRail1(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCollarX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarDome = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutCollar = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		CSDataHelper.GetNurbsCurve(InProfile.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double value3 = 0.0;
		InCollarX.GetDouble(ref value3);
		double value4 = 0.0;
		InCollarY.GetDouble(ref value4);
		double value5 = 0.0;
		InCollarZ.GetDouble(ref value5);
		double value6 = 0.0;
		InCollarDome.GetDouble(ref value6);
		double value7 = 0.0;
		InAOffset.GetDouble(ref value7);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint, out var value8);
		if (value8 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value9);
		if (value9 == Vector3d.Unset)
		{
			flag = false;
		}
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num = 0.0 - ((Point3d)(ref center)).X;
		center = ((BoundingBox)(ref boundingBox)).Center;
		Transform val = Transform.Translation(num, 0.0 - ((Point3d)(ref center)).Y, 0.0);
		Plane worldXY = Plane.WorldXY;
		double num2 = value3;
		Vector3d val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num3 = num2 / ((Vector3d)(ref val2)).X;
		double num4 = value4;
		val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val3 = Transform.Scale(worldXY, num3, num4 / ((Vector3d)(ref val2)).Y, 0.0);
		((GeometryBase)value2).Transform(val3 * val);
		Transform val4 = Transform.Translation(0.0, 0.0, value5 / 2.0);
		NurbsCurve val5 = CSUtilGeometry.CreateDomeRC(value6);
		((GeometryBase)val5).Transform(val4);
		Transform val6 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
		NurbsCurve val7 = ((Curve)val5).DuplicateCurve().ToNurbsCurve();
		((GeometryBase)val7).Transform(val6);
		LineCurve val8 = new LineCurve(((Curve)val5).PointAtStart, ((Curve)val7).PointAtStart);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			(Curve)val5,
			(Curve)val8,
			(Curve)val7
		}, 0.01);
		if (array == null)
		{
			return UpdateError("Error Merging Curves");
		}
		NurbsCurve inprofile = array[0].ToNurbsCurve();
		val4 = Transform.Translation(0.0, value4 / 2.0, 0.0);
		NurbsSurface val9 = CSUtilRailRevolve.RailRevolve(inprofile, value2, 0.5, 0);
		((GeometryBase)val9).Transform(val4);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(0.0, 0.0, (0.0 - value5) / 2.0 - value6);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(0.0, 1.0, (0.0 - value5) / 2.0 - value6);
		Point3d val12 = default(Point3d);
		((Point3d)(ref val12))._002Ector(0.0, 0.0, value5 / 2.0 + value6);
		Point3d val13 = default(Point3d);
		((Point3d)(ref val13))._002Ector(0.0, 1.0, value5 / 2.0 + value6);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, 0.0, ref x, 0.0);
		((GeometryBase)val9).Transform(x);
		((Point3d)(ref val10)).Transform(x);
		((Point3d)(ref val11)).Transform(x);
		((Point3d)(ref val12)).Transform(x);
		((Point3d)(ref val13)).Transform(x);
		Vector3d inputVC = val11 - val10;
		((Vector3d)(ref inputVC)).Unitize();
		double num5 = value7;
		val2 = new Vector3d(val10);
		Transform val14 = CSBlockUtil.CSUtilMath.Rotation(num5 / ((Vector3d)(ref val2)).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform val15;
		if (flag)
		{
			val2 = new Vector3d(value8);
			double length = ((Vector3d)(ref val2)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val10, inputVC, length, ref intersectPoint);
			val15 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), value8, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val15 = Transform.Identity;
		}
		Transform val16 = val15 * val14;
		((GeometryBase)val9).Transform(val16);
		((Point3d)(ref val12)).Transform(val16);
		((Point3d)(ref val13)).Transform(val16);
		Vector3d vectorStore = val13 - val12;
		((Vector3d)(ref vectorStore)).Unitize();
		OutCollar.SetData(new CSOnGeometryData((GeometryBase)(object)((Surface)val9).ToBrep(), val16, applyTransform: false));
		OutRightRailPoint.SetData(new CSPointData(val12));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
