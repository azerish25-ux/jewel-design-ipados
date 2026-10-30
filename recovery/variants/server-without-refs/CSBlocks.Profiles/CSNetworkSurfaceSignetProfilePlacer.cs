using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("NetworkSurfaceSignetProfilePlacer", "{A68013C9-0693-452c-8872-2AF99AB43B34}")]
public sealed class CSNetworkSurfaceSignetProfilePlacer : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iBottomCurve;

	public readonly CSInputChannel iTopCurve;

	public readonly CSInputChannel iPosition1;

	public readonly CSInputChannel iPosition2;

	public readonly CSInputChannel iPosition3;

	public readonly CSInputChannel iSidePosition1;

	public readonly CSInputChannel iSidePosition2;

	public readonly CSInputChannel iSidePosition3;

	public readonly CSInputChannel iTopThickness;

	public readonly CSInputChannel iTopWidth;

	public readonly CSInputChannel iTopLength;

	public readonly CSInputChannel iBottomThickness;

	public readonly CSInputChannel iBottomWidth;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iZFollowGem;

	public readonly CSOutputChannel oCurves;

	public readonly CSOutputChannel oTop;

	public CSNetworkSurfaceSignetProfilePlacer(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iBottomCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iTopCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPosition1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPosition2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPosition3 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSidePosition3 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iZFollowGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		oTop = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRingRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing ring rail");
		}
		BoundingBox boundingBox = ((GeometryBase)value).GetBoundingBox(true);
		CSDataHelper.GetNurbsCurve(iTopCurve.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing top curve");
		}
		CSDataHelper.GetNurbsCurve(iBottomCurve.Data, out var value3);
		if (value3 == null)
		{
			return UpdateMissing("Missing bottom curve");
		}
		double num = 0.0;
		double num2 = 0.0;
		CSData data = iGem.Data;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		double num3 = 0.0;
		Point3d val;
		if (data != null)
		{
			if (data.GetTightBoundingBox(ref boundingBox2))
			{
				val = ((BoundingBox)(ref boundingBox2)).Min;
				double val2 = 0.0 - ((Point3d)(ref val)).X;
				val = ((BoundingBox)(ref boundingBox2)).Max;
				num = Math.Max(val2, ((Point3d)(ref val)).X);
				val = ((BoundingBox)(ref boundingBox2)).Min;
				double val3 = 0.0 - ((Point3d)(ref val)).Y;
				val = ((BoundingBox)(ref boundingBox2)).Max;
				num2 = Math.Max(val3, ((Point3d)(ref val)).Y);
			}
			CSData arrayItem = CSDataHelper.GetFlattenArray(data).GetArrayItem(0);
			if (arrayItem != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
				if (((Transform)(ref value4)).IsValid)
				{
					Point3d val4 = default(Point3d);
					((Point3d)(ref val4))._002Ector(Point3d.Origin);
					((Point3d)(ref val4)).Transform(value4);
					num3 = ((Point3d)(ref val4)).Z;
				}
			}
		}
		BoundingBox boundingBox3 = ((GeometryBase)value2).GetBoundingBox(true);
		BoundingBox boundingBox4 = ((GeometryBase)value3).GetBoundingBox(true);
		double value5 = 0.0;
		iTopWidth.GetDouble(ref value5);
		double num5;
		if (num < 0.001)
		{
			double num4 = value5 / 2.0;
			val = ((BoundingBox)(ref boundingBox3)).Max;
			num5 = num4 / ((Point3d)(ref val)).X;
		}
		else
		{
			double num6 = value5 + num;
			val = ((BoundingBox)(ref boundingBox3)).Max;
			num5 = num6 / ((Point3d)(ref val)).X;
		}
		double value6 = 0.0;
		iBottomThickness.GetDouble(ref value6);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double num7 = ((Point3d)(ref val)).X + value6;
		val = ((BoundingBox)(ref boundingBox4)).Max;
		double num8 = num7 / ((Point3d)(ref val)).X;
		double value7 = 0.0;
		iTopLength.GetDouble(ref value7);
		double num10;
		if (num2 < 0.001)
		{
			double num9 = value7;
			val = ((BoundingBox)(ref boundingBox3)).Max;
			num10 = num9 / (((Point3d)(ref val)).Y * 2.0);
		}
		else
		{
			double num11 = value7 + num2;
			val = ((BoundingBox)(ref boundingBox3)).Max;
			num10 = num11 / ((Point3d)(ref val)).Y;
		}
		double value8 = 0.0;
		iBottomWidth.GetDouble(ref value8);
		double num12 = value8;
		val = ((BoundingBox)(ref boundingBox4)).Max;
		double num13 = num12 / (((Point3d)(ref val)).Y * 2.0);
		double value9 = 1.0;
		iTopThickness.GetDouble(ref value9);
		Transform val5 = Transform.Scale(Plane.WorldXY, num5, num10, 0.0);
		Transform val6 = Transform.Scale(Plane.WorldXY, num8, num13, 0.0);
		((GeometryBase)value3).Transform(val6);
		((GeometryBase)value2).Transform(val5);
		bool value10 = false;
		if (iZFollowGem != null)
		{
			iZFollowGem.GetBoolean(ref value10);
		}
		if (num3 > 0.01 && value10)
		{
			((GeometryBase)value2).Translate(new Vector3d(0.0, 0.0, value9 + num3));
		}
		else
		{
			NurbsCurve obj = value2;
			val = ((BoundingBox)(ref boundingBox)).Max;
			((GeometryBase)obj).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val)).Z + value9));
		}
		LineCurve val7 = new LineCurve(Point3d.Origin, new Point3d(0.0, 40.0, 0.0));
		LineCurve val8 = new LineCurve(Point3d.Origin, new Point3d(0.0, -40.0, 0.0));
		LineCurve val9 = new LineCurve(Point3d.Origin, new Point3d(-40.0, 0.0, 0.0));
		LineCurve val10 = new LineCurve(Point3d.Origin, new Point3d(40.0, 0.0, 0.0));
		CurveIntersections val11 = Intersection.CurveCurve((Curve)(object)value3, (Curve)(object)val7, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 1");
		}
		Point3d end = default(Point3d);
		((Point3d)(ref end))._002Ector(val11[0].PointA);
		val11.Dispose();
		val11 = Intersection.CurveCurve((Curve)(object)value3, (Curve)(object)val8, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 2");
		}
		Point3d end2 = default(Point3d);
		((Point3d)(ref end2))._002Ector(val11[0].PointA);
		val11.Dispose();
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 3");
		}
		val11 = Intersection.CurveCurve((Curve)(object)value3, (Curve)(object)val10, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		Point3d end3 = default(Point3d);
		((Point3d)(ref end3))._002Ector(val11[0].PointA);
		val11.Dispose();
		val11 = Intersection.CurveCurve((Curve)(object)value3, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 4");
		}
		Point3d end4 = default(Point3d);
		((Point3d)(ref end4))._002Ector(val11[0].PointA);
		val11.Dispose();
		BoundingBox boundingBox5 = ((GeometryBase)value2).GetBoundingBox(true);
		val = ((BoundingBox)(ref boundingBox5)).Center;
		((GeometryBase)val7).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val)).Z));
		boundingBox5 = ((GeometryBase)value2).GetBoundingBox(true);
		val = ((BoundingBox)(ref boundingBox5)).Center;
		((GeometryBase)val8).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val)).Z));
		boundingBox5 = ((GeometryBase)value2).GetBoundingBox(true);
		val = ((BoundingBox)(ref boundingBox5)).Center;
		((GeometryBase)val9).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val)).Z));
		boundingBox5 = ((GeometryBase)value2).GetBoundingBox(true);
		val = ((BoundingBox)(ref boundingBox5)).Center;
		((GeometryBase)val10).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val)).Z));
		val11 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val7, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 5");
		}
		Point3d start = default(Point3d);
		((Point3d)(ref start))._002Ector(val11[0].PointA);
		val11.Dispose();
		val11 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val8, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 6");
		}
		Point3d start2 = default(Point3d);
		((Point3d)(ref start2))._002Ector(val11[0].PointA);
		val11.Dispose();
		val11 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val10, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 7");
		}
		Point3d start3 = default(Point3d);
		((Point3d)(ref start3))._002Ector(val11[0].PointA);
		val11.Dispose();
		val11 = Intersection.CurveCurve((Curve)(object)value2, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val11 == null)
		{
			return UpdateError("IntersectCurve failed 8");
		}
		Point3d start4 = new Point3d(val11[0].PointA);
		val11.Dispose();
		double value11 = double.NaN;
		if (iPosition1 != null)
		{
			iPosition1.GetDouble(ref value11);
		}
		double value12 = double.NaN;
		if (iPosition2 != null)
		{
			iPosition2.GetDouble(ref value12);
		}
		double value13 = double.NaN;
		if (iPosition3 != null)
		{
			iPosition3.GetDouble(ref value13);
		}
		double value14 = double.NaN;
		double value15 = double.NaN;
		double value16 = double.NaN;
		if (iSidePosition1 != null)
		{
			iSidePosition1.GetDouble(ref value14);
		}
		if (iSidePosition2 != null)
		{
			iSidePosition2.GetDouble(ref value15);
		}
		if (iSidePosition3 != null)
		{
			iSidePosition3.GetDouble(ref value16);
		}
		Curve val12 = CreateSideProfile(start, end, new Vector3d(0.0, 1.0, 0.0), value14, value15, value16);
		Curve val13 = CreateSideProfile(start2, end2, new Vector3d(0.0, -1.0, 0.0), value14, value15, value16);
		Curve val14 = CreateSideProfile(start3, end3, new Vector3d(1.0, 0.0, 0.0), value11, value12, value13);
		Curve val15 = CreateSideProfile(start4, end4, new Vector3d(-1.0, 0.0, 0.0), value11, value12, value13);
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value2));
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value3));
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val12.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val13.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val14.ToNurbsCurve()));
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val15.ToNurbsCurve()));
		oCurves.SetData(cSDataArray);
		oTop.SetData(new CSOnGeometryData((GeometryBase)(object)value2));
		return base.Update();
	}

	private static Curve CreateSideProfile(Point3d start, Point3d end, Vector3d direction, double p1, double p2, double p3)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		Line val = default(Line);
		((Line)(ref val))._002Ector(start, end);
		if (double.IsNaN(p1))
		{
			return (Curve)new LineCurve(start, end);
		}
		if (double.IsNaN(p2))
		{
			Point3dList val2 = new Point3dList();
			((RhinoList<Point3d>)val2).Add(start);
			((RhinoList<Point3d>)val2).Add(((Line)(ref val)).PointAt(0.5) + direction * p1);
			((RhinoList<Point3d>)val2).Add(end);
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(0.0, 0.0, -1.0);
			return Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val2, 3, (CurveKnotStyle)1, Vector3d.Unset, val3);
		}
		if (double.IsNaN(p3))
		{
			Point3dList val4 = new Point3dList();
			((RhinoList<Point3d>)val4).Add(start);
			((RhinoList<Point3d>)val4).Add(((Line)(ref val)).PointAt(0.33) + direction * p1);
			((RhinoList<Point3d>)val4).Add(((Line)(ref val)).PointAt(0.66) + direction * p2);
			((RhinoList<Point3d>)val4).Add(end);
			Vector3d val5 = default(Vector3d);
			((Vector3d)(ref val5))._002Ector(0.0, 0.0, -1.0);
			return Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val4, 3, (CurveKnotStyle)1, Vector3d.Unset, val5);
		}
		Point3dList val6 = new Point3dList();
		((RhinoList<Point3d>)val6).Add(start);
		((RhinoList<Point3d>)val6).Add(((Line)(ref val)).PointAt(0.25) + direction * p1);
		((RhinoList<Point3d>)val6).Add(((Line)(ref val)).PointAt(0.5) + direction * p2);
		((RhinoList<Point3d>)val6).Add(((Line)(ref val)).PointAt(0.75) + direction * p2);
		((RhinoList<Point3d>)val6).Add(end);
		Vector3d val7 = end - start;
		((Vector3d)(ref val7)).Unitize();
		Vector3d val8 = default(Vector3d);
		((Vector3d)(ref val8))._002Ector(0.0, 0.0, -1.0);
		return Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val6, 3, (CurveKnotStyle)1, Vector3d.Unset, val8);
	}
}
