using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Profiles;

[CSBlock("ProfileSplit", "{5BA6C989-69CB-43ae-A152-B8BE63D1E2A0}")]
public sealed class CSProfileSplit : CSBlock
{
	public readonly CSInputChannel InProfileIn;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InOffset;

	public readonly CSOutputChannel OutRight;

	public readonly CSOutputChannel OutLeft;

	public CSProfileSplit(string identifier)
		: base(identifier)
	{
		InProfileIn = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRight = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		OutLeft = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InProfileIn.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Profile");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			return UpdateMissing("Missing Xform");
		}
		OnNurbsCurve onNurbsCurve = data.GetNurbsCurve().Convert();
		onNurbsCurve = onNurbsCurve.Duplicate();
		if (onNurbsCurve == null)
		{
			return UpdateError("ProfileSplit : Failed to find crv\n");
		}
		double value2 = 0.0;
		if (InAngle != null)
		{
			InAngle.GetDouble(ref value2);
		}
		double value3 = 0.0;
		if (InOffset != null)
		{
			InOffset.GetDouble(ref value3);
		}
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(value3, -100.0, 0.0), new On3dPoint(value3, 100.0, 0.0));
		onLineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), OnUtil.On_zaxis, new On3dPoint(0.0, onNurbsCurve.BoundingBox().Center().y, 0.0));
		value.TryGetInverse(out var inverseTransform);
		onNurbsCurve.Transform(inverseTransform.Convert());
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, 1.0, 0.0);
		onNurbsCurve.Transform(onXform);
		onNurbsCurve.IntersectCurve(onLineCurve, ref x);
		if (x.Count() < 2)
		{
			return UpdateError("ProfileSPlit : failed intersect\n");
		}
		int index = x.Count() / 2 - 1;
		int index2 = x.Count() / 2;
		onNurbsCurve.ChangeClosedCurveSeam(x[index].get_m_a(0));
		OnCurve left_side = null;
		OnCurve right_side = null;
		double t = 0.0;
		onNurbsCurve.GetClosestPoint(x[index2].get_m_pointA(0), ref t);
		if (!onNurbsCurve.Split(t, ref left_side, ref right_side))
		{
			return UpdateError("ProfileSplit : Failed to compute left and right\n");
		}
		if (left_side == null || right_side == null)
		{
			return UpdateError("ProfileSplit : Failed to compute left and right\n");
		}
		OnLineCurve item = new OnLineCurve(left_side.PointAtStart(), left_side.PointAtEnd());
		List<IOnCurve> obj = new List<IOnCurve> { left_side, item };
		OnCurve[] output = null;
		RhUtil.RhinoMergeCurves(obj.ToArray(), ref output);
		if (output != null && output.Length != 0 && output[0] != null)
		{
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			output[0].GetNurbForm(ref nurbs_curve);
			nurbs_curve.Transform(value.Convert());
			OutRight.SetData(new CSProfileData(nurbs_curve.Convert(), null, value, null));
		}
		obj.Clear();
		obj.Add(right_side);
		obj.Add(item);
		output = null;
		RhUtil.RhinoMergeCurves(obj.ToArray(), ref output);
		if (output != null && output.Length != 0 && output[0] != null)
		{
			OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
			output[0].GetNurbForm(ref nurbs_curve2);
			nurbs_curve2.Transform(value.Convert());
			OutLeft.SetData(new CSProfileData(nurbs_curve2.Convert(), null, value, null));
		}
		return base.Update();
	}
}
