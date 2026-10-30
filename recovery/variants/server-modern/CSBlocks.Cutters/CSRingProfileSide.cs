using System.Collections.Generic;
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
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
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
		double num = InZOffset.Double;
		double num2 = InCenterZOffset.Double;
		double num3 = InLength.Double;
		((GeometryBase)nurbsCurve2).MakeDeformable();
		Curve val = ((Curve)nurbsCurve2).DuplicateCurve();
		Curve val2 = ((Curve)nurbsCurve2).DuplicateCurve();
		if (val == null)
		{
			return UpdateError("profile failed");
		}
		Transform val3 = Transform.Translation(0.0, 0.0, (0.0 - num3) / 2.0);
		((GeometryBase)val).Transform(val3);
		val3 = Transform.Translation(0.0, 0.0, num3 / 2.0);
		((GeometryBase)val2).Transform(val3);
		Curve[] array;
		if (num2 == 0.0)
		{
			array = (Curve[])(object)new Curve[2] { val, val2 };
		}
		else
		{
			Curve val4 = ((Curve)nurbsCurve2).DuplicateCurve();
			val3 = Transform.Translation(0.0, num2, 0.0);
			((GeometryBase)val4).Transform(val3);
			array = (Curve[])(object)new Curve[3] { val, val4, val2 };
		}
		Brep[] array2 = Brep.CreateFromLoft((IEnumerable<Curve>)array, Point3d.Unset, Point3d.Unset, (LoftType)3, true);
		if (array2.Length < 1)
		{
			UpdateError("Loft Error");
			return false;
		}
		Brep val5 = array2[0];
		Transform val6 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)nurbsCurve, 0.0, ref x, 1.0);
		Transform val7 = Transform.Translation(0.0, 0.0, num) * x * val6;
		((GeometryBase)val5).Transform(val7);
		OutObject.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val7, applyTransform: false));
		return base.Update();
	}
}
