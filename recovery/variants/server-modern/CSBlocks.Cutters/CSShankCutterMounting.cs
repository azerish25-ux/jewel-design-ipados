using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Cutters;

[CSBlock("ShankCutterMounting", "{2A7E8748-93E0-43e1-B84C-13896077DE92}")]
public sealed class CSShankCutterMounting : CSBlock
{
	public CSInputChannel InRail0 { get; }

	public CSInputChannel InRail1 { get; }

	public CSInputChannel InOffset { get; }

	public CSInputChannel InBlendLength { get; }

	public CSInputChannel InDoMountingCutoutTop { get; }

	public CSInputChannel InBaseAngleTop { get; }

	public CSInputChannel InMountingAngleTop { get; }

	public CSInputChannel InDoMountingCutoutBottom { get; }

	public CSInputChannel InBaseAngleBottom { get; }

	public CSInputChannel InMountingAngleBottom { get; }

	public CSOutputChannel OutRail0 { get; }

	public CSShankCutterMounting(string identifier)
		: base(identifier)
	{
		InRail0 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBlendLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDoMountingCutoutTop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseAngleTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InMountingAngleTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDoMountingCutoutBottom = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBaseAngleBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InMountingAngleBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutRail0 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	private static bool MakeOffset(NurbsCurve rail0, NurbsCurve rail1, double vOffset, double blendLength, ref NurbsCurve shankCutterCurve, out string error)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Scale(Plane.WorldXY, 1.0, 0.0, 1.0);
		((GeometryBase)rail0).Transform(val);
		((GeometryBase)rail1).Transform(val);
		Point3d pointAtStart = default(Point3d);
		((Point3d)(ref pointAtStart))._002Ector(((Curve)rail0).PointAtStart);
		Point3d pointAtStart2 = default(Point3d);
		((Point3d)(ref pointAtStart2))._002Ector(((Curve)rail1).PointAtStart);
		if (Math.Abs(((Point3d)(ref pointAtStart)).X) > 0.001 || ((Point3d)(ref pointAtStart)).Z < 0.0)
		{
			LineCurve val2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val3 = Intersection.CurveCurve((Curve)(object)rail0, (Curve)(object)val2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val3.Count == 0)
			{
				error = "CSShankCutter - Start Point of inside ring rail not at X=0";
				return false;
			}
			((Curve)rail0).ChangeClosedCurveSeam(val3[0].ParameterA);
			((Point3d)(ref pointAtStart))._002Ector(((Curve)rail0).PointAtStart);
			if (Math.Abs(((Point3d)(ref pointAtStart)).X) > 0.001)
			{
				error = "CSShankCutter - Start Point of inside ring rail not at X=0";
				return false;
			}
		}
		if (Math.Abs(((Point3d)(ref pointAtStart2)).X) > 0.001 || ((Point3d)(ref pointAtStart2)).Z < 0.0)
		{
			LineCurve val4 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val5 = Intersection.CurveCurve((Curve)(object)rail1, (Curve)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val5.Count == 0)
			{
				error = "CSShankCutter - Start Point of outside ring rail not at X=0";
				return false;
			}
			((Curve)rail1).ChangeClosedCurveSeam(val5[0].ParameterA);
			((Point3d)(ref pointAtStart2))._002Ector(((Curve)rail1).PointAtStart);
			if (Math.Abs(((Point3d)(ref pointAtStart2)).X) > 0.001)
			{
				error = "CSShankCutter - Start Point of outside ring rail not at X=0";
				return false;
			}
		}
		if (((Point3d)(ref pointAtStart)).Z > ((Point3d)(ref pointAtStart2)).Z)
		{
			error = "CSShankCutter - Inside Ring Rail is outside of Outside Ring Rail";
			return false;
		}
		Interval domain = ((Curve)rail0).Domain;
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(((Curve)rail0).TangentAt(((Interval)(ref domain)).Min));
		domain = ((Curve)rail1).Domain;
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(((Curve)rail1).TangentAt(((Interval)(ref domain)).Min));
		if (((Vector3d)(ref val6)).X < 0.0)
		{
			((Curve)rail0).Reverse();
		}
		if (((Vector3d)(ref val7)).X < 0.0)
		{
			((Curve)rail1).Reverse();
		}
		Curve val8 = CSUtilGeometry.OffsetOld((Curve)(object)rail0, new Vector3d(0.0, 1.0, 0.0), Point3d.Origin, 0.0 - vOffset, closed: true);
		Curve val9 = CSUtilGeometry.OffsetOld((Curve)(object)rail1, new Vector3d(0.0, 1.0, 0.0), Point3d.Origin, vOffset, closed: true);
		if (val8 == null)
		{
			error = "CSShankCutter - Inside Ring Rail Offset Failed";
			return false;
		}
		if (val9 == null)
		{
			error = "CSShankCutter - Outside Ring Rail Offset Failed";
			return false;
		}
		NurbsCurve val10 = val8.ToNurbsCurve();
		NurbsCurve val11 = val9.ToNurbsCurve();
		pointAtStart = ((Curve)val10).PointAtStart;
		pointAtStart2 = ((Curve)val11).PointAtStart;
		if (((Point3d)(ref pointAtStart)).Z + 0.1 > ((Point3d)(ref pointAtStart2)).Z)
		{
			error = "CSShankCutter - Offset to big";
			return false;
		}
		CurveIntersections val12 = Intersection.CurveCurve(val8, val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val12.Count == 0)
		{
			error = "CSShankCutter - Intersect Failed";
			return false;
		}
		Point3d val13 = default(Point3d);
		((Point3d)(ref val13))._002Ector(0.0, 0.0, -100.0);
		Point3d val14 = default(Point3d);
		((Point3d)(ref val14))._002Ector(0.0, 0.0, -100.0);
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		foreach (IntersectionEvent item in val12)
		{
			Point3d pointA = item.PointA;
			if (((Point3d)(ref pointA)).X > 0.0)
			{
				if (((Point3d)(ref pointA)).Z > ((Point3d)(ref val14)).Z)
				{
					val14 = pointA;
					num2 = item.ParameterA;
					num4 = item.ParameterB;
				}
			}
			else if (((Point3d)(ref pointA)).Z > ((Point3d)(ref val13)).Z)
			{
				val13 = pointA;
				num = item.ParameterA;
				num3 = item.ParameterB;
			}
		}
		domain = ((Curve)val10).Domain;
		NurbsCurve val15 = ((Curve)val10).Trim(new Interval(((Interval)(ref domain)).Min, num2)).ToNurbsCurve();
		double num5 = num;
		domain = ((Curve)val10).Domain;
		NurbsCurve val16 = ((Curve)val10).Trim(new Interval(num5, ((Interval)(ref domain)).Max)).ToNurbsCurve();
		domain = ((Curve)val11).Domain;
		NurbsCurve val17 = ((Curve)val11).Trim(new Interval(((Interval)(ref domain)).Min, num4)).ToNurbsCurve();
		double num6 = num3;
		domain = ((Curve)val11).Domain;
		NurbsCurve val18 = ((Curve)val11).Trim(new Interval(num6, ((Interval)(ref domain)).Max)).ToNurbsCurve();
		List<Curve> list = new List<Curve>();
		if (blendLength > 0.01)
		{
			double length = ((Curve)val15).GetLength();
			double num7 = default(double);
			((Curve)val15).NormalizedLengthParameter((length - blendLength) / length, ref num7);
			NurbsCurve obj = val15;
			domain = ((Curve)val15).Domain;
			val15 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, num7)).ToNurbsCurve();
			length = ((Curve)val16).GetLength();
			((Curve)val16).NormalizedLengthParameter(blendLength / length, ref num7);
			NurbsCurve obj2 = val16;
			double num8 = num7;
			domain = ((Curve)val16).Domain;
			val16 = ((Curve)obj2).Trim(new Interval(num8, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			length = ((Curve)val17).GetLength();
			((Curve)val17).NormalizedLengthParameter((length - blendLength) / length, ref num7);
			NurbsCurve obj3 = val17;
			domain = ((Curve)val17).Domain;
			val17 = ((Curve)obj3).Trim(new Interval(((Interval)(ref domain)).Min, num7)).ToNurbsCurve();
			length = ((Curve)val18).GetLength();
			((Curve)val18).NormalizedLengthParameter(blendLength / length, ref num7);
			NurbsCurve obj4 = val18;
			double num9 = num7;
			domain = ((Curve)val18).Domain;
			val18 = ((Curve)obj4).Trim(new Interval(num9, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			list.Add((Curve)(object)val15);
			Point3d pointAtEnd = ((Curve)val15).PointAtEnd;
			Point3d pointAtEnd2 = ((Curve)val17).PointAtEnd;
			NurbsCurve obj5 = val15;
			domain = ((Curve)val15).Domain;
			Vector3d vectorStart = ((Curve)obj5).TangentAt(((Interval)(ref domain)).Max);
			NurbsCurve obj6 = val17;
			domain = ((Curve)val17).Domain;
			list.Add((Curve)(object)CSUtilCurves.BlendCurve(pointAtEnd, pointAtEnd2, vectorStart, ((Curve)obj6).TangentAt(((Interval)(ref domain)).Max)));
			list.Add((Curve)(object)val17);
			list.Add((Curve)(object)val18);
			Point3d pointAtStart3 = ((Curve)val16).PointAtStart;
			Point3d pointAtStart4 = ((Curve)val18).PointAtStart;
			NurbsCurve obj7 = val16;
			domain = ((Curve)val16).Domain;
			Vector3d vectorStart2 = -((Curve)obj7).TangentAt(((Interval)(ref domain)).Min);
			NurbsCurve obj8 = val18;
			domain = ((Curve)val18).Domain;
			list.Add((Curve)(object)CSUtilCurves.BlendCurve(pointAtStart3, pointAtStart4, vectorStart2, -((Curve)obj8).TangentAt(((Interval)(ref domain)).Min)));
			list.Add((Curve)(object)val16);
		}
		else
		{
			list.Add((Curve)(object)val15);
			list.Add((Curve)(object)val17);
			list.Add((Curve)(object)val18);
			list.Add((Curve)(object)val16);
		}
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)list, 0.01);
		if (array == null || array.Length == 0 || array.Length > 1)
		{
			error = "CSShankCutter - Curve split and re-merge failed";
			return false;
		}
		shankCutterCurve = array[0].ToNurbsCurve();
		if (shankCutterCurve == null)
		{
			error = "CSShankCutter - Curve split and re-merge failed";
			return false;
		}
		error = string.Empty;
		return true;
	}

	public override bool Update()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Expected O, but got Unknown
		NurbsCurve nurbsCurve = InRail0.NurbsCurve;
		NurbsCurve nurbsCurve2 = InRail1.NurbsCurve;
		double vOffset = InOffset.Double;
		double blendLength = InBlendLength.Double;
		bool boolean = InDoMountingCutoutTop.Boolean;
		double num = InBaseAngleTop.Double;
		if (num < 0.01)
		{
			return UpdateError("Base Top Angle To Low");
		}
		double angleDegrees = InMountingAngleTop.Double;
		bool boolean2 = InDoMountingCutoutBottom.Boolean;
		double num2 = InBaseAngleBottom.Double;
		if (num2 < 0.01)
		{
			return UpdateError("Base Bottom Angle To Low");
		}
		double angleDegrees2 = InMountingAngleBottom.Double;
		NurbsCurve shankCutterCurve = null;
		if (!MakeOffset(nurbsCurve, nurbsCurve2, vOffset, blendLength, ref shankCutterCurve, out var error))
		{
			return UpdateError("Offset: " + error);
		}
		if (shankCutterCurve == null)
		{
			return UpdateError("Error with Cutter");
		}
		Point3d pointA;
		Interval domain;
		if (boolean)
		{
			LineCurve val = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val2 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val2.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int num3 = 0;
			for (int i = 1; i < val2.Count; i++)
			{
				pointA = val2[i].PointA;
				double z = ((Point3d)(ref pointA)).Z;
				pointA = val2[num3].PointA;
				if (z > ((Point3d)(ref pointA)).Z)
				{
					num3 = i;
				}
			}
			((Curve)shankCutterCurve).ChangeClosedCurveSeam(val2[num3].ParameterA);
			NurbsCurve obj = shankCutterCurve;
			domain = ((Curve)shankCutterCurve).Domain;
			if (((Curve)obj).TangentAt(((Interval)(ref domain)).Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				((Curve)shankCutterCurve).Reverse();
			}
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(0.0, 0.0, 50.0);
			((Vector3d)(ref val3)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num), Vector3d.YAxis);
			LineCurve val4 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + val3);
			((Vector3d)(ref val3)).X = 0.0 - ((Vector3d)(ref val3)).X;
			LineCurve val5 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + val3);
			CurveIntersections val6 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			CurveIntersections val7 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val6.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			if (val7.Count == 0)
			{
				return UpdateError("Error Intersect 2 Failed");
			}
			int num4 = 0;
			for (int j = 1; j < val6.Count; j++)
			{
				pointA = val6[j].PointA;
				double z2 = ((Point3d)(ref pointA)).Z;
				pointA = val6[num4].PointA;
				if (z2 > ((Point3d)(ref pointA)).Z)
				{
					num4 = j;
				}
			}
			int num5 = 0;
			for (int k = 1; k < val7.Count; k++)
			{
				pointA = val7[k].PointA;
				double z3 = ((Point3d)(ref pointA)).Z;
				pointA = val7[num5].PointA;
				if (z3 > ((Point3d)(ref pointA)).Z)
				{
					num5 = k;
				}
			}
			Vector3d val8 = ((Curve)shankCutterCurve).TangentAt(val7[num5].ParameterA);
			((Vector3d)(ref val8)).Unitize();
			double num6 = 0.0 - Math.Asin(((Vector3d)(ref val8)).Z);
			shankCutterCurve = ((Curve)shankCutterCurve).Trim(new Interval(val7[num5].ParameterA, val6[num4].ParameterA)).ToNurbsCurve();
			Vector3d val9 = default(Vector3d);
			((Vector3d)(ref val9))._002Ector(0.0, 0.0, 10.0);
			((Vector3d)(ref val9)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees) + num6, Vector3d.YAxis);
			LineCurve val10 = new LineCurve(((Curve)shankCutterCurve).PointAtStart, ((Curve)shankCutterCurve).PointAtStart + val9);
			((Vector3d)(ref val9)).X = 0.0 - ((Vector3d)(ref val9)).X;
			LineCurve val11 = new LineCurve(((Curve)shankCutterCurve).PointAtEnd, ((Curve)shankCutterCurve).PointAtEnd + val9);
			CurveIntersections val12 = Intersection.CurveCurve((Curve)(object)val10, (Curve)(object)val11, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val12.Count < 1)
			{
				LineCurve val13 = new LineCurve(((Curve)val10).PointAtEnd, ((Curve)val11).PointAtEnd);
				Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[4]
				{
					(Curve)shankCutterCurve,
					(Curve)val10,
					(Curve)val13,
					(Curve)val11
				});
				if (array != null && array.Length != 0)
				{
					shankCutterCurve = array[0].ToNurbsCurve();
				}
			}
			else
			{
				domain = ((Curve)val10).Domain;
				Curve val14 = ((Curve)val10).Trim(new Interval(((Interval)(ref domain)).Min, val12[0].ParameterA));
				domain = ((Curve)val10).Domain;
				Curve val15 = ((Curve)val11).Trim(new Interval(((Interval)(ref domain)).Min, val12[0].ParameterB));
				Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
				{
					(Curve)shankCutterCurve,
					val14,
					val15
				});
				if (array2 != null && array2.Length != 0)
				{
					shankCutterCurve = array2[0].ToNurbsCurve();
				}
			}
		}
		if (boolean2)
		{
			LineCurve val16 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections val17 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val16, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val17.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int num7 = 0;
			for (int l = 1; l < val17.Count; l++)
			{
				pointA = val17[l].PointA;
				double z4 = ((Point3d)(ref pointA)).Z;
				pointA = val17[num7].PointA;
				if (z4 < ((Point3d)(ref pointA)).Z)
				{
					num7 = l;
				}
			}
			((Curve)shankCutterCurve).ChangeClosedCurveSeam(val17[num7].ParameterA);
			NurbsCurve obj2 = shankCutterCurve;
			domain = ((Curve)shankCutterCurve).Domain;
			if (((Curve)obj2).TangentAt(((Interval)(ref domain)).Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				((Curve)shankCutterCurve).Reverse();
			}
			Vector3d val18 = default(Vector3d);
			((Vector3d)(ref val18))._002Ector(0.0, 0.0, 50.0);
			((Vector3d)(ref val18)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), Vector3d.YAxis);
			LineCurve val19 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + val18);
			((Vector3d)(ref val18)).X = 0.0 - ((Vector3d)(ref val18)).X;
			LineCurve val20 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0) + val18);
			CurveIntersections val21 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val19, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			CurveIntersections val22 = Intersection.CurveCurve((Curve)(object)shankCutterCurve, (Curve)(object)val20, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val21.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			if (val22.Count == 0)
			{
				return UpdateError("Error Intersect Failed");
			}
			int num8 = 0;
			for (int m = 1; m < val21.Count; m++)
			{
				pointA = val21[m].PointA;
				double z5 = ((Point3d)(ref pointA)).Z;
				pointA = val21[num8].PointA;
				if (z5 < ((Point3d)(ref pointA)).Z)
				{
					num8 = m;
				}
			}
			int num9 = 0;
			for (int n = 1; n < val22.Count; n++)
			{
				pointA = val22[n].PointA;
				double z6 = ((Point3d)(ref pointA)).Z;
				pointA = val22[num9].PointA;
				if (z6 < ((Point3d)(ref pointA)).Z)
				{
					num9 = n;
				}
			}
			Vector3d val23 = ((Curve)shankCutterCurve).TangentAt(val22[num9].ParameterA);
			((Vector3d)(ref val23)).Unitize();
			double num10 = 0.0 - Math.Asin(((Vector3d)(ref val23)).Z);
			shankCutterCurve = ((Curve)shankCutterCurve).Trim(new Interval(val22[num9].ParameterA, val21[num8].ParameterA)).ToNurbsCurve();
			Vector3d val24 = default(Vector3d);
			((Vector3d)(ref val24))._002Ector(0.0, 0.0, -10.0);
			((Vector3d)(ref val24)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2) + num10, Vector3d.YAxis);
			LineCurve val25 = new LineCurve(((Curve)shankCutterCurve).PointAtStart, ((Curve)shankCutterCurve).PointAtStart + val24);
			((Vector3d)(ref val24)).X = 0.0 - ((Vector3d)(ref val24)).X;
			LineCurve val26 = new LineCurve(((Curve)shankCutterCurve).PointAtEnd, ((Curve)shankCutterCurve).PointAtEnd + val24);
			CurveIntersections val27 = Intersection.CurveCurve((Curve)(object)val25, (Curve)(object)val26, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val27.Count < 1)
			{
				LineCurve val28 = new LineCurve(((Curve)val25).PointAtEnd, ((Curve)val26).PointAtEnd);
				Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[4]
				{
					(Curve)shankCutterCurve,
					(Curve)val25,
					(Curve)val28,
					(Curve)val26
				});
				if (array3 != null && array3.Length != 0)
				{
					shankCutterCurve = array3[0].ToNurbsCurve();
				}
			}
			else
			{
				domain = ((Curve)val25).Domain;
				Curve val29 = ((Curve)val25).Trim(new Interval(((Interval)(ref domain)).Min, val27[0].ParameterA));
				domain = ((Curve)val25).Domain;
				Curve val30 = ((Curve)val26).Trim(new Interval(((Interval)(ref domain)).Min, val27[0].ParameterB));
				Curve[] array4 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
				{
					(Curve)shankCutterCurve,
					val29,
					val30
				});
				if (array4 != null && array4.Length != 0)
				{
					shankCutterCurve = array4[0].ToNurbsCurve();
				}
			}
		}
		OutRail0.SetData(new CSOnGeometryData((GeometryBase)(object)shankCutterCurve));
		return base.Update();
	}
}
