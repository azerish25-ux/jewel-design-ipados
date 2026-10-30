using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("MicroPaveCutter", "{A36ED2AD-47DC-4ed2-A53D-314FF2D512EF}")]
public sealed class CSMicroPaveCutter : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InVPlacement { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InWidth { get; }

	public CSInputChannel InBend { get; }

	public CSInputChannel InCenterScale { get; }

	public CSInputChannel InProfile { get; }

	public CSOutputChannel OutCutters { get; }

	public CSMicroPaveCutter(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InVPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InBend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InCenterScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCutters = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("MicroPave Cutter:  Missing Gems");
		}
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		double num = InBend.Double;
		if (Math.Abs(num) < 0.1)
		{
			num = 0.1;
		}
		double num2 = InWidth.Double;
		double num3 = InHeight.Double;
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		((GeometryBase)nurbsCurve).Translate(new Point3d(0.0, 0.0, 0.0) - ((BoundingBox)(ref boundingBox)).Center);
		Plane worldXY = Plane.WorldXY;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double val2 = ((Point3d)(ref val)).X * 2.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num4 = num2 / Math.Max(val2, (0.0 - ((Point3d)(ref val)).X) * 2.0);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double val3 = ((Point3d)(ref val)).Y * 2.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Transform val4 = Transform.Scale(worldXY, num4, num3 / Math.Max(val3, (0.0 - ((Point3d)(ref val)).Y) * 2.0), 0.0);
		((GeometryBase)nurbsCurve).Transform(val4);
		double num5 = InLength.Double;
		double num6 = InCenterScale.Double;
		double num7 = InVPlacement.Double;
		Point3d val5 = new Point3d(0.0, num5 / 2.0, num7);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(0.0, (0.0 - num5) / 2.0, num7);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(0.0, 0.0, num + num7);
		ArcCurve val8 = new ArcCurve(new Arc(val5, val7, val6));
		NurbsCurve val9 = ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve();
		NurbsCurve val10 = ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve();
		NurbsCurve val11 = ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve();
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		Vector3d xAxis = Vector3d.XAxis;
		Point3d pointAtStart = ((Curve)val8).PointAtStart;
		Interval domain = ((Curve)val8).Domain;
		Transform val12 = CSBlockUtil.CSUtilMath.Orient3Point(origin, zAxis, xAxis, pointAtStart, ((Curve)val8).TangentAt(((Interval)(ref domain)).Min), Vector3d.XAxis);
		Point3d origin2 = Point3d.Origin;
		Vector3d zAxis2 = Vector3d.ZAxis;
		Vector3d xAxis2 = Vector3d.XAxis;
		Point3d pointAtEnd = ((Curve)val8).PointAtEnd;
		domain = ((Curve)val8).Domain;
		Transform val13 = CSBlockUtil.CSUtilMath.Orient3Point(origin2, zAxis2, xAxis2, pointAtEnd, ((Curve)val8).TangentAt(((Interval)(ref domain)).Max), Vector3d.XAxis);
		Point3d origin3 = Point3d.Origin;
		Vector3d zAxis3 = Vector3d.ZAxis;
		Vector3d xAxis3 = Vector3d.XAxis;
		domain = ((Curve)val8).Domain;
		Point3d destination = ((Curve)val8).PointAt(((Interval)(ref domain)).Mid);
		domain = ((Curve)val8).Domain;
		Transform val14 = CSBlockUtil.CSUtilMath.Orient3Point(origin3, zAxis3, xAxis3, destination, ((Curve)val8).TangentAt(((Interval)(ref domain)).Mid), Vector3d.XAxis);
		((GeometryBase)val9).Transform(val12);
		((GeometryBase)val10).Transform(val13);
		((GeometryBase)val11).Scale(num6);
		((GeometryBase)val11).Transform(val14);
		Curve[] profiles = (Curve[])(object)new Curve[3]
		{
			(Curve)val9,
			(Curve)val11,
			(Curve)val10
		};
		double[] array = new double[3];
		domain = ((Curve)val8).Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = ((Curve)val8).Domain;
		array[1] = ((Interval)(ref domain)).Mid;
		domain = ((Curve)val8).Domain;
		array[2] = ((Interval)(ref domain)).Max;
		Brep[] breps = (Brep[])(object)new Brep[1];
		CSUtilSweepRC.Sweep1((Curve)(object)val8, profiles, array, out breps, closed: false, globalShapeBlend: false);
		Brep val15 = null;
		if (breps != null && breps.Length != 0)
		{
			val15 = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (breps != null && breps.Length != 0)
		{
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var _);
					Brep val16 = ((val15 == null) ? breps[0].DuplicateBrep() : val15.DuplicateBrep());
					((GeometryBase)val16).Transform(xUp);
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val16, xUp, applyTransform: false));
				}
			}
		}
		OutCutters.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(IOnNurbsCurve curveNC, double centerSpacing, double placement, ref OnNurbsCurve outputCurve)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		OnCurve val = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)curveNC, centerSpacing, closed: true);
		if (val == null)
		{
			val = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)curveNC, 0.0, closed: true);
			if (val == null)
			{
				CS.CSUtil.Print("Bezel Cutter Offset Curve Unable to Create Output\n");
				return false;
			}
		}
		val.GetNurbForm(ref outputCurve);
		OnXform val2 = new OnXform();
		val2.Translation(0.0, 0.0, placement);
		outputCurve.Transform((IOnXform)(object)val2);
		return true;
	}
}
