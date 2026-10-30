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
using Rhino.Geometry;

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
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d3: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InProfileIn.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Profile");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			return UpdateMissing("Missing Xform");
		}
		OnNurbsCurve val = data.GetNurbsCurve().Convert();
		val = val.Duplicate();
		if (val == null)
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
		OnLineCurve val2 = new OnLineCurve((IOn3dPoint)new On3dPoint(value3, -100.0, 0.0), (IOn3dPoint)new On3dPoint(value3, 100.0, 0.0));
		((OnGeometry)val2).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), OnUtil.On_zaxis, (IOn3dPoint)new On3dPoint(0.0, ((OnGeometry)val).BoundingBox().Center().y, 0.0));
		Transform transform = default(Transform);
		((Transform)(ref value)).TryGetInverse(ref transform);
		val.Transform((IOnXform)(object)transform.Convert());
		ArrayOnX_EVENT val3 = new ArrayOnX_EVENT();
		OnXform val4 = new OnXform();
		val4.Scale(1.0, 1.0, 0.0);
		val.Transform((IOnXform)(object)val4);
		((OnCurve)val).IntersectCurve((IOnCurve)(object)val2, ref val3);
		if (val3.Count() < 2)
		{
			return UpdateError("ProfileSPlit : failed intersect\n");
		}
		int num = val3.Count() / 2 - 1;
		int num2 = val3.Count() / 2;
		((OnCurve)val).ChangeClosedCurveSeam(val3[num][0]);
		OnCurve val5 = null;
		OnCurve val6 = null;
		double num3 = 0.0;
		((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)val3[num2][0], ref num3);
		if (!((OnCurve)val).Split(num3, ref val5, ref val6))
		{
			return UpdateError("ProfileSplit : Failed to compute left and right\n");
		}
		if (val5 == null || val6 == null)
		{
			return UpdateError("ProfileSplit : Failed to compute left and right\n");
		}
		OnLineCurve item = new OnLineCurve((IOn3dPoint)(object)val5.PointAtStart(), (IOn3dPoint)(object)val5.PointAtEnd());
		List<IOnCurve> obj = new List<IOnCurve>
		{
			(IOnCurve)(object)val5,
			(IOnCurve)(object)item
		};
		OnCurve[] array = null;
		RhUtil.RhinoMergeCurves(obj.ToArray(), ref array);
		if (array != null && array.Length != 0 && array[0] != null)
		{
			OnNurbsCurve val7 = new OnNurbsCurve();
			array[0].GetNurbForm(ref val7);
			val7.Transform((IOnXform)(object)value.Convert());
			OutRight.SetData(new CSProfileData((Curve)(object)val7.Convert(), null, value, null));
		}
		obj.Clear();
		obj.Add((IOnCurve)(object)val6);
		obj.Add((IOnCurve)(object)item);
		array = null;
		RhUtil.RhinoMergeCurves(obj.ToArray(), ref array);
		if (array != null && array.Length != 0 && array[0] != null)
		{
			OnNurbsCurve val8 = new OnNurbsCurve();
			array[0].GetNurbForm(ref val8);
			val8.Transform((IOnXform)(object)value.Convert());
			OutLeft.SetData(new CSProfileData((Curve)(object)val8.Convert(), null, value, null));
		}
		return base.Update();
	}
}
