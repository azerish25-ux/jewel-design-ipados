using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("RingProfileSide", "{9EC7F979-E16E-4435-A6CA-1CFCF12EB1A9}")]
public sealed class CSRingProfileSide : CSBlock
{
	public CSInputChannel InRail { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InZOffset { get; }

	public CSInputChannel InCenterZOffset { get; }

	public CSInputChannel InLength { get; }

	public CSOutputChannel OutObject { get; }

	public CSRingProfileSide(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InCenterZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		NurbsCurve nurbsCurve2 = InProfile.NurbsCurve;
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		double dz = InZOffset.Double;
		double num = InCenterZOffset.Double;
		double num2 = InLength.Double;
		nurbsCurve2.MakeDeformable();
		Curve curve = nurbsCurve2.DuplicateCurve();
		Curve curve2 = nurbsCurve2.DuplicateCurve();
		if (curve == null)
		{
			return UpdateError("profile failed");
		}
		Transform xform = Transform.Translation(0.0, 0.0, (0.0 - num2) / 2.0);
		curve.Transform(xform);
		xform = Transform.Translation(0.0, 0.0, num2 / 2.0);
		curve2.Transform(xform);
		Curve[] curves;
		if (num == 0.0)
		{
			curves = new Curve[2] { curve, curve2 };
		}
		else
		{
			Curve curve3 = nurbsCurve2.DuplicateCurve();
			xform = Transform.Translation(0.0, num, 0.0);
			curve3.Transform(xform);
			curves = new Curve[3] { curve, curve3, curve2 };
		}
		Brep[] array = Brep.CreateFromLoft(curves, Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: true);
		if (array.Length < 1)
		{
			UpdateError("Loft Error");
			return false;
		}
		Brep brep = array[0];
		Transform transform = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(nurbsCurve, 0.0, ref x, 1.0);
		Transform transform2 = Transform.Translation(0.0, 0.0, dz) * x * transform;
		brep.Transform(transform2);
		OutObject.SetData(new CSOnGeometryData(brep, transform2, applyTransform: false));
		return base.Update();
	}
}
