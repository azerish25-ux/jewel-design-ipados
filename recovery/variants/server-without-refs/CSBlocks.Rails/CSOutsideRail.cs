using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Rails;

[CSBlock("OutsideRail", "{594560C4-6033-4FAC-B990-68FBACEB1E0F}")]
public sealed class CSOutsideRail : CSBlock
{
	public readonly CSInputChannel InRingRailRadius;

	public readonly CSInputChannel InShape;

	public readonly CSInputChannel InThicknessTop;

	public readonly CSInputChannel InThicknessBottom;

	public readonly CSInputChannel InThicknessSide;

	public readonly CSInputChannel InEnableLock;

	public readonly CSOutputChannel OutOutsideRingRail;

	public CSOutsideRail(string identifier)
		: base(identifier)
	{
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShape = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InThicknessTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessSide = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEnableLock = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutOutsideRingRail = new CSOutputChannel(this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(InShape.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Shape");
		}
		if (!((Curve)value).IsPlanar())
		{
			return UpdateError("Shape must be planar");
		}
		double value2 = 8.667196228921199;
		InRingRailRadius.GetDouble(ref value2);
		double value3 = 3.0;
		InThicknessTop.GetDouble(ref value3);
		double value4 = 1.0;
		InThicknessBottom.GetDouble(ref value4);
		double value5 = 2.0;
		InThicknessSide.GetDouble(ref value5);
		bool value6 = false;
		InEnableLock.GetBoolean(ref value6);
		NurbsCurve ringRailOuter = ((Curve)value).DuplicateCurve().ToNurbsCurve();
		((GeometryBase)ringRailOuter).MakeDeformable();
		if (value6)
		{
			double num = value2;
			double num2 = 0.0 - value2;
			double num3 = value2;
			if (!CSUtilRingRail.GetRingRailPointExtents((Curve)(object)ringRailOuter, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var _))
			{
				return UpdateError("GetRingRailPointExtents failed");
			}
			LineCurve val = new LineCurve(new Point3d(-200.0, 0.0, 0.0), new Point3d(200.0, 0.0, 0.0));
			double num4 = pointExtentRight;
			double num5 = num3 + value5;
			Transform val2 = Transform.Scale(Plane.WorldXY, num5 / num4, 1.0, 1.0);
			((GeometryBase)ringRailOuter).Transform(val2);
			CurveIntersections val3 = Intersection.CurveCurve((Curve)(object)ringRailOuter, (Curve)(object)val, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val3.Count != 2)
			{
				return UpdateError("Intersect of custom Rail Failed\n");
			}
			NurbsCurve obj = ((Curve)ringRailOuter).DuplicateCurve().ToNurbsCurve();
			((Curve)obj).ChangeClosedCurveSeam(val3[0].ParameterA);
			NurbsCurve val4 = ((Curve)obj).DuplicateCurve().ToNurbsCurve();
			((Curve)val4).Reverse();
			double num6 = 0.0;
			((Curve)obj).ClosestPoint(val3[1].PointA, ref num6);
			Interval domain = ((Curve)obj).Domain;
			Curve val5 = (Curve)(object)((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, num6)).ToNurbsCurve();
			((Curve)val4).ClosestPoint(val3[1].PointA, ref num6);
			domain = ((Curve)val4).Domain;
			Curve val6 = (Curve)(object)((Curve)val4).Trim(new Interval(((Interval)(ref domain)).Min, num6)).ToNurbsCurve();
			Curve obj2 = val5;
			domain = val5.Domain;
			Point3d val7 = obj2.PointAt(((Interval)(ref domain)).Mid);
			if (((Point3d)(ref val7)).Z < 0.0)
			{
				Curve obj3 = val5;
				val5 = val6;
				val6 = obj3;
			}
			Transform val8 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value3 + num) / pointExtentTop);
			Transform val9 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (0.0 - value4 + num2) / pointExtentBottom);
			((GeometryBase)val5).Transform(val8);
			((GeometryBase)val6).Transform(val9);
			Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val5, val6 });
			if (array.Length != 0)
			{
				ringRailOuter = array[0].ToNurbsCurve();
			}
			LineCurve val10 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val11 = Intersection.CurveCurve((Curve)(object)ringRailOuter, (Curve)(object)val10, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val11.Count > 0)
			{
				num6 = val11[0].ParameterA;
			}
			else
			{
				((Curve)ringRailOuter).ClosestPoint(new Point3d(0.0, 0.0, 100.0), ref num6);
			}
			((Curve)ringRailOuter).ChangeClosedCurveSeam(num6);
			NurbsCurve obj4 = ringRailOuter;
			domain = ((Curve)ringRailOuter).Domain;
			if (((Curve)obj4).TangentAt(((Interval)(ref domain)).Min) * Vector3d.XAxis < 0.0)
			{
				((Curve)ringRailOuter).Reverse();
			}
		}
		else if (!CSUtilRingRail.CreateOutsideRingRail(value2, (Curve)(object)value, value3, value4, value5, out ringRailOuter))
		{
			return UpdateError("CreateOutsideRingRail failed");
		}
		OutOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)ringRailOuter));
		return base.Update();
	}
}
