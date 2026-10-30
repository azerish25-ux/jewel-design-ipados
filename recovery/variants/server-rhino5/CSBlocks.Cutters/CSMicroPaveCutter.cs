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
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		nurbsCurve.Translate(new Point3d(0.0, 0.0, 0.0) - boundingBox.Center);
		Transform xform = Transform.Scale(Plane.WorldXY, num2 / Math.Max(boundingBox.Max.X * 2.0, (0.0 - boundingBox.Min.X) * 2.0), num3 / Math.Max(boundingBox.Max.Y * 2.0, (0.0 - boundingBox.Min.Y) * 2.0), 0.0);
		nurbsCurve.Transform(xform);
		double num4 = InLength.Double;
		double scaleFactor = InCenterScale.Double;
		double num5 = InVPlacement.Double;
		Point3d startPoint = new Point3d(0.0, num4 / 2.0, num5);
		Point3d endPoint = new Point3d(0.0, (0.0 - num4) / 2.0, num5);
		Point3d pointOnInterior = new Point3d(0.0, 0.0, num + num5);
		ArcCurve arcCurve = new ArcCurve(new Arc(startPoint, pointOnInterior, endPoint));
		NurbsCurve nurbsCurve2 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		NurbsCurve nurbsCurve3 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, arcCurve.PointAtStart, arcCurve.TangentAt(arcCurve.Domain.Min), Vector3d.XAxis);
		Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, arcCurve.PointAtEnd, arcCurve.TangentAt(arcCurve.Domain.Max), Vector3d.XAxis);
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, arcCurve.PointAt(arcCurve.Domain.Mid), arcCurve.TangentAt(arcCurve.Domain.Mid), Vector3d.XAxis);
		nurbsCurve2.Transform(xform2);
		nurbsCurve3.Transform(xform3);
		nurbsCurve4.Scale(scaleFactor);
		nurbsCurve4.Transform(xform4);
		Curve[] profiles = new Curve[3] { nurbsCurve2, nurbsCurve4, nurbsCurve3 };
		double[] parameters = new double[3]
		{
			arcCurve.Domain.Min,
			arcCurve.Domain.Mid,
			arcCurve.Domain.Max
		};
		Brep[] breps = new Brep[1];
		CSUtilSweepRC.Sweep1(arcCurve, profiles, parameters, out breps, closed: false, globalShapeBlend: false);
		Brep brep = null;
		if (breps != null && breps.Length != 0)
		{
			brep = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (breps != null && breps.Length != 0)
		{
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
				{
					CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var _);
					Brep brep2 = ((brep == null) ? breps[0].DuplicateBrep() : brep.DuplicateBrep());
					brep2.Transform(xUp);
					cSDataArray.Append(new CSOnGeometryData(brep2, xUp, applyTransform: false));
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
