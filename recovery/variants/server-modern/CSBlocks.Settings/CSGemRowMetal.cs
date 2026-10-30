using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("GemRowMetal", "{FEA7DD33-7719-430a-B7D4-416A6584F066}")]
public sealed class CSGemRowMetal : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InGirdleSpacing;

	public readonly CSInputChannel InEndSpacing;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InTopThickness;

	public readonly CSInputChannel InChannelDepth;

	public readonly CSInputChannel InSeatHeight;

	public readonly CSOutputChannel OutSurface;

	public CSGemRowMetal(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGem.GemList;
		if (gemList == null)
		{
			return UpdateMissing("Missing gem data");
		}
		if (gemList.Count < 1)
		{
			return UpdateMissing("Missing gem");
		}
		double num = InGirdleSpacing.Double;
		double num2 = InEndSpacing.Double;
		double num3 = InPlacement.Double;
		double angleDegrees = InAngle.Double;
		double num4 = InHeight.Double;
		double num5 = InTopThickness.Double;
		double num6 = InChannelDepth.Double;
		double num7 = InSeatHeight.Double;
		if (num6 >= num4 - num7)
		{
			num6 = num4 - num7 - 0.1;
		}
		Transform val = Transform.Translation(0.0, 0.0, num3);
		CSDataArray cSDataArray = new CSDataArray();
		Point3d val2 = default(Point3d);
		Point3d val5 = default(Point3d);
		Point3d val7 = default(Point3d);
		Point3d val8 = default(Point3d);
		Point3d val11 = default(Point3d);
		Point3d val12 = default(Point3d);
		Vector3d val16 = default(Vector3d);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("Invalid gem");
			}
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
			((Point3d)(ref val2))._002Ector(cSGemData.Center);
			((Point3d)(ref val2)).Y = 0.0;
			Point3d destination = val2;
			Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination, cSGemData.Up, Vector3d.YAxis);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!cSGemData.GetTightBoundingBox(ref boundingBox, doGrow: false, transform))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			NurbsCurve nurbsCurve = InProfile.NurbsCurve;
			if (nurbsCurve == null)
			{
				return UpdateError("Missing profile");
			}
			double num8 = gemList.Count;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val2)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num9 = y - ((Point3d)(ref val2)).Y;
			double num10 = num8 * num9 + 2.0 * num2;
			if (num8 > 1.0)
			{
				CSGemData cSGemData2 = gemList[1];
				if (cSGemData2 == null)
				{
					return UpdateError("Invalid gem");
				}
				val2 = cSGemData.Center;
				double num11 = ((Point3d)(ref val2)).DistanceTo(cSGemData2.Center) - num9;
				num10 += (num8 - 1.0) * num11;
			}
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			Point3d val4 = new Point3d(((Point3d)(ref val2)).X - num, num10 / 2.0, 0.0 - num6);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref val5))._002Ector(((Point3d)(ref val2)).X + num, num10 / 2.0, 0.0 - num6);
			LineCurve val6 = new LineCurve(val4, val5);
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			((Point3d)(ref val7))._002Ector(((Point3d)(ref val2)).X - num, num10 / 2.0, num7);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref val8))._002Ector(((Point3d)(ref val2)).X + num, num10 / 2.0, num7);
			LineCurve val9 = new LineCurve(val4, val7);
			LineCurve val10 = new LineCurve(val5, val8);
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			((Point3d)(ref val11))._002Ector(((Point3d)(ref val2)).X - num - num5, num10 / 2.0, num7);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref val12))._002Ector(((Point3d)(ref val2)).X + num + num5, num10 / 2.0, num7);
			LineCurve val13 = new LineCurve(val7, val11);
			LineCurve val14 = new LineCurve(val8, val12);
			Transform val15 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
			((GeometryBase)nurbsCurve).Transform(val15);
			((Vector3d)(ref val16))._002Ector(((Curve)nurbsCurve).PointAtStart);
			((Vector3d)(ref val16)).Reverse();
			Transform val17 = Transform.Translation(val16);
			((GeometryBase)nurbsCurve).Transform(val17);
			val2 = ((Curve)nurbsCurve).PointAtEnd;
			double x = ((Point3d)(ref val2)).X;
			val2 = ((Curve)nurbsCurve).PointAtEnd;
			double num12 = CSBlockUtil.CSUtilMath.AngleOfPointD(x, 0.0 - ((Point3d)(ref val2)).Z);
			Transform val18 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees) + num12, Vector3d.YAxis, Point3d.Origin);
			((GeometryBase)nurbsCurve).Transform(val18);
			val2 = ((Curve)nurbsCurve).PointAtEnd;
			double num13 = 0.0 - ((Point3d)(ref val2)).Z;
			double num14 = num4 / num13;
			Transform val19 = Transform.Scale(Plane.WorldXY, num14, num14, num14);
			((GeometryBase)nurbsCurve).Transform(val19);
			Transform val20 = Transform.Translation(val12 - ((Curve)nurbsCurve).PointAtStart);
			((GeometryBase)nurbsCurve).Transform(val20);
			Transform val21 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
			NurbsCurve val22 = ((Curve)nurbsCurve).ToNurbsCurve();
			((GeometryBase)val22).Transform(val21);
			LineCurve val23 = new LineCurve(((Curve)nurbsCurve).PointAtEnd, ((Curve)val22).PointAtEnd);
			Curve[] obj = new Curve[8]
			{
				(Curve)val6,
				(Curve)val9,
				(Curve)val13,
				(Curve)val22,
				(Curve)val23,
				(Curve)nurbsCurve,
				(Curve)val14,
				(Curve)val10
			};
			Curve[] array = null;
			array = Curve.JoinCurves((IEnumerable<Curve>)(object)obj);
			if (array.Length != 1)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
			Curve obj2 = array[0];
			Curve val24 = obj2.DuplicateCurve();
			Transform val25 = Transform.Translation(0.0, 0.0 - num10, 0.0);
			((GeometryBase)val24).Transform(val25);
			Brep val26 = ((Surface)NurbsSurface.CreateRuledSurface(obj2, val24)).ToBrep();
			if (val26 == null)
			{
				return UpdateError("BrepForm failed");
			}
			Brep val27 = CSUtilRhino.CapPlanarHoles(val26, CSApplicationHelper.Application.Tolerance);
			if (val27 == null)
			{
				return UpdateError("CapPlanarHoles failed");
			}
			((GeometryBase)val27).Transform(val3 * val);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val27, val3 * val, applyTransform: false));
		}
		OutSurface.SetData(cSDataArray);
		return base.Update();
	}
}
