using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.SMI;

[CSBlock("SMICutterBand", "{E7861C52-7377-419e-AEFA-71D89DF6EC24}")]
public sealed class CSSMICutterBand : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSubCount { get; }

	public CSOutputChannel oObject { get; }

	public CSSMICutterBand(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubCount = new CSInputChannel(GetPropertyName(() => iSubCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 45.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		int value3 = 1;
		iSubCount.GetInteger(ref value3);
		OnCurve val2 = null;
		OnCurve val3 = null;
		if (!val.Split(val.Domain().Mid(), ref val2, ref val3))
		{
			return UpdateError("Split failed");
		}
		int num = 2 * value3;
		double num2 = OnUtil.On_PI / (double)value3;
		double num3 = value2 / 2.0;
		double num4 = (OnUtil.On_PI - OnUtil.On_PI / (double)value3) / 2.0;
		On3dPoint val4 = new On3dPoint(0.0, value, 0.0);
		OnCurve val5 = val2.DuplicateCurve();
		((OnGeometry)val5).Rotate(num3, OnUtil.On_xaxis, (IOn3dPoint)(object)val4);
		OnCurve val6 = val2.DuplicateCurve();
		((OnGeometry)val6).Rotate(num3 / 2.0, OnUtil.On_xaxis, (IOn3dPoint)(object)val4);
		OnCurve val7 = val2.DuplicateCurve();
		((OnGeometry)val7).Rotate(0.0 - num4, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBoundingBox val8 = new OnBoundingBox();
		if (!((OnGeometry)val5).GetTightBoundingBox(ref val8))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		OnBoundingBox val9 = new OnBoundingBox();
		if (!((OnGeometry)val7).GetTightBoundingBox(ref val9))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num5 = Math.Abs(val8.Max().x - val8.Min().x);
		double num6 = Math.Abs(val9.Max().x - val9.Min().x);
		double num7 = num5 / num6;
		OnXform val10 = new OnXform();
		val10.Scale(num7, 1.0, num7);
		((OnGeometry)val7).Transform((IOnXform)(object)val10);
		if (!CSUtilSweep.Sweep1((IOnCurve)(object)OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val5.PointAtStart(), (IOn3dPoint)(object)val6.PointAtStart(), (IOn3dPoint)(object)val7.PointAtStart())), (IOnCurve)(object)val5, (IOnCurve)(object)val7, out var breps))
		{
			return UpdateError("Sweep1 failed");
		}
		if (breps.Length != 1)
		{
			return UpdateError("Sweep1 failed");
		}
		OnBrep val11 = breps[0];
		OnXform val12 = new OnXform();
		val12.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		OnBrep val13 = val11.Duplicate();
		((OnGeometry)val13).Transform((IOnXform)(object)val12);
		OnBrep val14 = CSUtilRhino.JoinAllBreps((OnBrep[])(object)new OnBrep[2] { val11, val13 }, CSApplicationHelper.Application.Tolerance);
		if (val14 == null)
		{
			return UpdateError("RhinoJoinBreps failed");
		}
		OnXform val15 = new OnXform();
		val15.Rotation(num2, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBrep[] array = (OnBrep[])(object)new OnBrep[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = val14.Duplicate();
			((OnGeometry)val14).Transform((IOnXform)(object)val15);
		}
		OnBrep val16 = CSUtilRhino.JoinAllBreps(array, CSApplicationHelper.Application.Tolerance);
		if (!val16.IsValidGeometry())
		{
			return UpdateError("RhinoJoinBreps failed");
		}
		OnBrep val17 = CSUtilRhino.CapPlanarHoles(val16, CSApplicationHelper.Application.Tolerance);
		if (val17 == null)
		{
			return UpdateError("RhinoBrepCapPlanarHoles failed");
		}
		if (!val17.IsValidGeometry())
		{
			return UpdateError("RhinoBrepCapPlanarHoles failed");
		}
		oObject.SetData(new CSOnGeometryData(val17.Convert()));
		return base.Update();
	}
}
