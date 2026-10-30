using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("OutsideRail_V1", "{F9F6400F-67C2-4883-8C44-A91F411F2EE3}")]
public sealed class CSOutsideRail1 : CSBlock
{
	public readonly CSInputChannel iRingRadius;

	public readonly CSInputChannel iOutsideCurve;

	public readonly CSInputChannel iTopThickness;

	public readonly CSInputChannel iSideThickness;

	public readonly CSInputChannel iBottomThickness;

	public readonly CSInputChannel iLockOn;

	public readonly CSOutputChannel oOutsideRingRail;

	public readonly CSOutputChannel oOutsideScale;

	public CSOutsideRail1(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSideThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLockOn = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		oOutsideScale = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iOutsideCurve.Data, out var value);
		if (value == null)
		{
			return UpdateError("Outside Ring Rail: Missing Custom Rail (No Curve)");
		}
		double value2 = 3.0;
		double value3 = 2.0;
		double value4 = 1.0;
		bool value5 = false;
		iTopThickness.GetDouble(ref value2);
		iSideThickness.GetDouble(ref value3);
		iBottomThickness.GetDouble(ref value4);
		iLockOn.GetBoolean(ref value5);
		double value6 = 0.0;
		Point3d val;
		if (!iRingRadius.GetDouble(ref value6))
		{
			CSDataHelper.GetNurbsCurve(iRingRadius.Data, out var value7);
			if (value7 == null)
			{
				return UpdateError("Missing Ring Rail");
			}
			BoundingBox boundingBox = ((GeometryBase)value7).GetBoundingBox(true);
			val = ((BoundingBox)(ref boundingBox)).Max;
			value6 = ((Point3d)(ref val)).Z;
		}
		double num = value6;
		double num2 = 0.0 - value6;
		double num3 = value6;
		NurbsCurve val2 = ((Curve)value).DuplicateCurve().ToNurbsCurve();
		((GeometryBase)val2).MakeDeformable();
		CSUtilGeometry.GetMinMax((Curve)(object)val2, Point3d.Origin, 50, out var min, out var _);
		if (min == 0.0 || min == double.MaxValue)
		{
			return UpdateError("Intersect of custom Rail Failed");
		}
		if (num == 0.0)
		{
			return UpdateError("Intersect of custom Rail Failed");
		}
		Transform val3 = Transform.Scale(Point3d.Origin, num / min);
		((GeometryBase)val2).Transform(val3);
		if (!CSUtilRingRail.GetRingRailPointExtents((Curve)(object)val2, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var pointExtentLeft))
		{
			return UpdateError("Failed to get rail curve size");
		}
		if (!value5)
		{
			double num4 = pointExtentTop - pointExtentBottom;
			double num5 = num + value2 - pointExtentBottom;
			Point3d val4 = default(Point3d);
			((Point3d)(ref val4))._002Ector(0.0, 0.0, pointExtentBottom);
			Point3d val5 = default(Point3d);
			((Point3d)(ref val5))._002Ector(0.0, 0.0, pointExtentTop);
			Transform val6 = Transform.Scale(new Plane(val4, Vector3d.XAxis, Vector3d.ZAxis), 1.0, num5 / num4, 1.0);
			((GeometryBase)val2).Transform(val6);
			if (!CSUtilRingRail.GetRingRailPointExtents((Curve)(object)val2, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft))
			{
				return UpdateError("Failed to get rail curve size");
			}
			((Point3d)(ref val4)).Z = pointExtentBottom;
			((Point3d)(ref val5)).Z = pointExtentTop;
			num4 = pointExtentTop - pointExtentBottom;
			num5 = pointExtentTop - (num2 - value4);
			Transform val7 = Transform.Scale(new Plane(val5, Vector3d.XAxis, Vector3d.ZAxis), 1.0, num5 / num4, 1.0);
			((GeometryBase)val2).Transform(val7);
			if (!CSUtilRingRail.GetRingRailPointExtents((Curve)(object)val2, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft))
			{
				return UpdateError("Failed to get rail curve size");
			}
			num4 = pointExtentRight;
			num5 = num3 + value3;
			Transform val8 = Transform.Scale(Plane.WorldXY, num5 / num4, 1.0, 1.0);
			((GeometryBase)val2).Transform(val8);
		}
		else
		{
			LineCurve val9 = new LineCurve(new Point3d(-200.0, 0.0, 0.0), new Point3d(200.0, 0.0, 0.0));
			double num6 = pointExtentRight;
			double num7 = num3 + value3;
			Transform val10 = Transform.Scale(Plane.WorldXY, num7 / num6, 1.0, 1.0);
			((GeometryBase)val2).Transform(val10);
			CurveIntersections val11 = Intersection.CurveCurve((Curve)(object)val2, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val11.Count != 2)
			{
				return UpdateError("Intersect of custom Rail Failed");
			}
			NurbsCurve val12 = ((Curve)val2).DuplicateCurve().ToNurbsCurve();
			((Curve)val12).ChangeClosedCurveSeam(val11[0].ParameterA);
			NurbsCurve val13 = ((Curve)val12).DuplicateCurve().ToNurbsCurve();
			((Curve)val13).Reverse();
			double num8 = 0.0;
			((Curve)val12).ClosestPoint(val11[1].PointA, ref num8);
			NurbsCurve obj = val12;
			Interval domain = ((Curve)val12).Domain;
			val12 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, num8)).ToNurbsCurve();
			((Curve)val13).ClosestPoint(val11[1].PointA, ref num8);
			NurbsCurve obj2 = val13;
			domain = ((Curve)val13).Domain;
			val13 = ((Curve)obj2).Trim(new Interval(((Interval)(ref domain)).Min, num8)).ToNurbsCurve();
			NurbsCurve obj3 = val12;
			domain = ((Curve)val12).Domain;
			val = ((Curve)obj3).PointAt(((Interval)(ref domain)).Mid);
			if (((Point3d)(ref val)).Z < 0.0)
			{
				NurbsCurve obj4 = val12;
				val12 = val13;
				val13 = obj4;
			}
			Transform val14 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value2 + num) / pointExtentTop);
			Transform val15 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (0.0 - value4 + num2) / pointExtentBottom);
			((GeometryBase)val12).Transform(val14);
			((GeometryBase)val13).Transform(val15);
			Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
			{
				(Curve)val12,
				(Curve)val13
			});
			if (array.Length != 0)
			{
				val2 = array[0].ToNurbsCurve();
			}
			LineCurve val16 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val17 = Intersection.CurveCurve((Curve)(object)val2, (Curve)(object)val16, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val17.Count > 0)
			{
				num8 = val17[0].ParameterA;
			}
			else
			{
				((Curve)val2).ClosestPoint(new Point3d(0.0, 0.0, 100.0), ref num8);
			}
			((Curve)val2).ChangeClosedCurveSeam(num8);
			NurbsCurve obj5 = val2;
			domain = ((Curve)val2).Domain;
			if (((Curve)obj5).TangentAt(((Interval)(ref domain)).Min) * Vector3d.XAxis < 0.0)
			{
				((Curve)val2).Reverse();
			}
		}
		OnCurve val18 = (OnCurve)Interop.ToOnCurve((Curve)(object)val2);
		OnNurbsCurve nCrv = new OnNurbsCurve();
		val18.GetNurbForm(ref nCrv);
		oOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)nCrv.Convert()));
		oOutsideScale.SetData(new CSNumberData(0.0));
		return base.Update();
	}
}
