using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedCollarOnRingRail", "{55488183-91D6-4BD7-B167-923291361BEF}")]
public sealed class CSLinkedCollarOnRingRail : CSBlock
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

	public CSLinkedCollarOnRingRail(string identifier)
		: base(identifier)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCollarX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarDome = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutCollar = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		NurbsCurve nurbsCurve2 = InProfile.NurbsCurve;
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double num = InCollarX.Double;
		double num2 = InCollarY.Double;
		double num3 = InCollarZ.Double;
		double num4 = InCollarDome.Double;
		double num5 = InAOffset.Double;
		bool flag = true;
		Point3d point3d = InLastRightRailPoint.Point3d;
		if (point3d == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value);
		if (value == Vector3d.Unset)
		{
			flag = false;
		}
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve2).GetBoundingBox(true);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num6 = 0.0 - ((Point3d)(ref center)).X;
		center = ((BoundingBox)(ref boundingBox)).Center;
		Transform val = Transform.Translation(num6, 0.0 - ((Point3d)(ref center)).Y, 0.0);
		Plane worldXY = Plane.WorldXY;
		Vector3d val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num7 = num / ((Vector3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val3 = Transform.Scale(worldXY, num7, num2 / ((Vector3d)(ref val2)).Y, 0.0);
		((GeometryBase)nurbsCurve2).Transform(val3 * val);
		Transform val4 = Transform.Translation(0.0, 0.0, num3 / 2.0);
		NurbsCurve val5 = CSUtilGeometry.CreateDomeRC(num4);
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
		val4 = Transform.Translation(0.0, num2 / 2.0, 0.0);
		NurbsSurface val9 = CSUtilRailRevolve.RailRevolve(inprofile, nurbsCurve2, 0.5, 0);
		((GeometryBase)val9).Transform(val4);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(0.0, 0.0, (0.0 - num3) / 2.0 - num4);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(0.0, 1.0, (0.0 - num3) / 2.0 - num4);
		Point3d val12 = default(Point3d);
		((Point3d)(ref val12))._002Ector(0.0, 0.0, num3 / 2.0 + num4);
		Point3d val13 = default(Point3d);
		((Point3d)(ref val13))._002Ector(0.0, 1.0, num3 / 2.0 + num4);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)nurbsCurve, 0.0, ref x, 0.0);
		((GeometryBase)val9).Transform(x);
		((Point3d)(ref val10)).Transform(x);
		((Point3d)(ref val11)).Transform(x);
		((Point3d)(ref val12)).Transform(x);
		((Point3d)(ref val13)).Transform(x);
		Vector3d inputVC = val11 - val10;
		((Vector3d)(ref inputVC)).Unitize();
		val2 = new Vector3d(val10);
		Transform val14 = CSBlockUtil.CSUtilMath.Rotation(num5 / ((Vector3d)(ref val2)).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform val15;
		if (flag)
		{
			val2 = new Vector3d(point3d);
			double length = ((Vector3d)(ref val2)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val10, inputVC, length, ref intersectPoint);
			val15 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0));
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
