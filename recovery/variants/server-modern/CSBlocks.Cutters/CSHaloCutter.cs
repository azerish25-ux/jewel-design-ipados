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

namespace CSBlocks.Cutters;

[CSBlock("HaloCutter", "{AE7C8085-3F52-4af2-89C0-9CAB22291DCC}")]
public sealed class CSHaloCutter : CSBlock
{
	public CSInputChannel InHalo { get; }

	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InRingRailOffset { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InFlowSurfaceEnable { get; }

	public CSInputChannel InFlowSurfaceParamStart { get; }

	public CSInputChannel InFlowSurfaceParamEnd { get; }

	public CSOutputChannel OutCutter { get; }

	public CSOutputChannel OutCurve { get; }

	public CSOutputChannel OutSurface { get; }

	public CSHaloCutter(string identifier)
		: base(identifier)
	{
		InHalo = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRingRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50.0);
		InFlowSurfaceEnable = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFlowSurfaceParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFlowSurfaceParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutCutter = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InHalo.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Halo");
		}
		Brep brep = InHalo.Brep;
		if (brep == null)
		{
			return UpdateMissing("Missing Halo");
		}
		double num = InRingRailRadius.Double;
		double num2 = InRingRailOffset.Double;
		double height = InHeight.Double;
		double num3 = InLength.Double;
		double num4 = InFlowSurfaceParamStart.Double;
		double num5 = InFlowSurfaceParamEnd.Double;
		bool boolean = InFlowSurfaceEnable.Boolean;
		CSDataHelper.GetTransformNoArray(data, out var value);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(Vector3d.ZAxis);
		((Vector3d)(ref val)).Transform(value);
		((Vector3d)(ref val)).Y = 0.0;
		Transform val2 = Transform.Rotation(Vector3d.ZAxis, val, Point3d.Origin);
		Curve cutterProfile = null;
		if (!MakeCutterProfile(num + num2, height, ref cutterProfile))
		{
			return UpdateError("Error Making Profile");
		}
		((GeometryBase)cutterProfile).Transform(val2);
		double num6 = num3 / 2.0;
		Curve obj = cutterProfile.DuplicateCurve();
		((GeometryBase)obj).Translate(new Vector3d(0.0, 0.0 - num6, 0.0));
		Curve val3 = cutterProfile.DuplicateCurve();
		((GeometryBase)val3).Translate(new Vector3d(0.0, num6, 0.0));
		Brep val4 = CSUtilRhino.CapPlanarHoles(((Surface)NurbsSurface.CreateRuledSurface(obj, val3)).ToBrep(), CSApplicationHelper.Application.Tolerance);
		if (val4 == null)
		{
			UpdateError("Capping Cutter");
		}
		OutCutter.SetData(new CSOnGeometryData((GeometryBase)(object)val4));
		if (boolean)
		{
			Curve[] array = default(Curve[]);
			Point3d[] array2 = default(Point3d[]);
			if (!Intersection.BrepBrep(brep, val4, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
			{
				return UpdateError("Error Intersect");
			}
			if (array.Length < 4)
			{
				return UpdateError("Too many intersects");
			}
			double num7 = 0.0;
			Curve[] array3 = array;
			Point3d pointAtStart;
			foreach (Curve val5 in array3)
			{
				double num8 = num7;
				pointAtStart = val5.PointAtStart;
				num7 = num8 + ((Point3d)(ref pointAtStart)).Z;
			}
			num7 /= (double)array.Length;
			int num9 = -1;
			int num10 = -1;
			for (int j = 0; j < array.Length; j++)
			{
				pointAtStart = array[j].PointAtStart;
				if (((Point3d)(ref pointAtStart)).Z > num7)
				{
					if (num9 != -1)
					{
						pointAtStart = array[j].PointAtStart;
						double y = ((Point3d)(ref pointAtStart)).Y;
						pointAtStart = array[num9].PointAtStart;
						if (!(y > ((Point3d)(ref pointAtStart)).Y))
						{
							continue;
						}
					}
					num9 = j;
					continue;
				}
				if (num10 != -1)
				{
					pointAtStart = array[j].PointAtStart;
					double y2 = ((Point3d)(ref pointAtStart)).Y;
					pointAtStart = array[num10].PointAtStart;
					if (!(y2 > ((Point3d)(ref pointAtStart)).Y))
					{
						continue;
					}
				}
				num10 = j;
			}
			double num11 = 0.0;
			double num12 = 0.0;
			NurbsCurve val6 = array[num9].ToNurbsCurve();
			NurbsCurve val7 = array[num10].ToNurbsCurve();
			NurbsCurve obj2 = val6;
			Interval domain = ((Curve)val6).Domain;
			Vector3d val8 = ((Curve)obj2).TangentAt(((Interval)(ref domain)).Min);
			NurbsCurve obj3 = val7;
			domain = ((Curve)val7).Domain;
			if (Vector3d.Multiply(val8, ((Curve)obj3).TangentAt(((Interval)(ref domain)).Min)) < 0.0)
			{
				((Curve)val7).Reverse();
			}
			((Curve)val6).NormalizedLengthParameter(num4, ref num11);
			((Curve)val6).NormalizedLengthParameter(num5, ref num12);
			val6 = ((Curve)val6).Trim(new Interval(num11, num12)).ToNurbsCurve();
			((Curve)val7).NormalizedLengthParameter(num4, ref num11);
			((Curve)val7).NormalizedLengthParameter(num5, ref num12);
			val7 = ((Curve)val7).Trim(new Interval(num11, num12)).ToNurbsCurve();
			CSDataArray cSDataArray = new CSDataArray();
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val6));
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val7));
			OutCurve.SetData(cSDataArray);
			if (!CSUtilSweep.Sweep2NoProfile((IOnCurve)(object)val6.Convert(), (IOnCurve)(object)val7.Convert(), out var breps))
			{
				return UpdateError("Sweep");
			}
			if (breps.Length != 1)
			{
				return UpdateError("Sweep To Many Surfaes");
			}
			OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)breps[0].Convert()));
		}
		return base.Update();
	}

	private static bool MakeCutterProfile(double arcRadius, double height, ref Curve cutterProfile)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		Point3d val = new Point3d(arcRadius, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 0.0, arcRadius);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0 - arcRadius, 0.0, 0.0);
		ArcCurve val4 = new ArcCurve(new Arc(val, val2, val3));
		PolylineCurve val5 = new PolylineCurve((IEnumerable<Point3d>)new List<Point3d>
		{
			new Point3d(0.0 - arcRadius, 0.0, 0.0),
			new Point3d(0.0 - arcRadius - 1.0, 0.0, 0.0),
			new Point3d(0.0 - arcRadius - 1.0, 0.0, arcRadius + height),
			new Point3d(arcRadius + 1.0, 0.0, arcRadius + height),
			new Point3d(arcRadius + 1.0, 0.0, 0.0),
			new Point3d(arcRadius, 0.0, 0.0)
		});
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			(Curve)val4,
			(Curve)val5
		}, CSApplicationHelper.Application.Tolerance);
		if (array.Length != 1)
		{
			return false;
		}
		cutterProfile = array[0];
		return true;
	}
}
