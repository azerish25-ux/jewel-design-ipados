using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Rails;

[CSBlock("RingRailBypass", "{6C082A4B-B104-4f71-B2A4-C9BF0FFC8E1B}")]
public sealed class CSRingRailBypass : CSBlock
{
	public readonly CSInputChannel iRingRadius;

	public readonly CSInputChannel iRailTube;

	public readonly CSInputChannel iX0;

	public readonly CSInputChannel iY0;

	public readonly CSInputChannel iX1;

	public readonly CSInputChannel iY1;

	public readonly CSOutputChannel oRingRail;

	public CSRingRailBypass(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailTube = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iX0 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY0 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iX1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		double value = 0.0;
		iRingRadius.GetDouble(ref value);
		double value2 = 0.0;
		iX0.GetDouble(ref value2);
		double value3 = 0.0;
		iY0.GetDouble(ref value3);
		double value4 = 0.0;
		iX1.GetDouble(ref value4);
		double value5 = 0.0;
		iY1.GetDouble(ref value5);
		NurbsSurface nurbsSurface = iRailTube.Data.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Missing ring rail tube");
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0 - value, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0 - value + value2, value3, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0 - value + value2 + value4, value3 + value5, 0.0);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Point3dList val5 = new Point3dList();
		((RhinoList<Point3d>)val5).Add(val);
		((RhinoList<Point3d>)val5).Add(val2);
		((RhinoList<Point3d>)val5).Add(val3);
		NurbsCurve val6 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val5, 3, (CurveKnotStyle)1, val4, Vector3d.Unset).ToNurbsCurve();
		if (val6 == null)
		{
			return UpdateError("RhinoInterpCurve failed");
		}
		Curve val7 = ((Curve)val6).DuplicateCurve();
		((GeometryBase)val7).Translate(new Vector3d(0.0, 0.0, 100.0));
		Brep obj = ((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)val6, val7)).ToBrep();
		Brep val8 = ((Surface)nurbsSurface).ToBrep();
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		Intersection.BrepBrep(obj, val8, 0.01, ref array, ref array2);
		Curve val9 = null;
		if (array.Length == 1)
		{
			val9 = array[0];
			if (val9 == null)
			{
				return UpdateError("RhinoIntersectBreps failed");
			}
		}
		else if (array.Length > 1)
		{
			Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)array);
			if (array3.Length >= 0)
			{
				val9 = array3[0];
			}
			if (val9 == null)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
		}
		if (val9 == null)
		{
			return UpdateError("Unknown failed");
		}
		Curve val10 = val9.DuplicateCurve();
		((GeometryBase)val10).Rotate(Math.PI, Vector3d.ZAxis, Point3d.Origin);
		ArcCurve val11 = new ArcCurve(new Arc(new Point3d(0.0 - value, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0 - value), new Point3d(value, 0.0, 0.0)));
		Curve[] array4 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			val9,
			(Curve)val11,
			val10
		});
		if (array4.Length == 0)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		Curve val12 = array4[0];
		val12.Reverse();
		oRingRail.SetData(new CSRingRailData((Curve)(object)val12.ToNurbsCurve(), value));
		return base.Update();
	}
}
