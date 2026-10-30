using System;
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

namespace CSBlocks.SMI.Obsolete;

[Obsolete]
[CSBlock("SMICutterBand_V1", "{988903BB-0F38-4b8c-866B-AA2C49387011}")]
public sealed class CSSMICutterBand1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iProfilePlacement { get; }

	public CSInputChannel iProfileShape { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSubCount { get; }

	public CSOutputChannel oObject { get; }

	public CSOutputChannel oString { get; }

	public CSSMICutterBand1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfilePlacement = new CSInputChannel(GetPropertyName(() => iProfilePlacement), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		iProfileShape = new CSInputChannel(GetPropertyName(() => iProfileShape), this, ChannelDataType.Any, ChannelArrayType.Any);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSubCount = new CSInputChannel(GetPropertyName(() => iSubCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
		oString = new CSOutputChannel(GetPropertyName(() => oString), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iRail.Data;
		if (data == null)
		{
			return UpdateMissing("Missing rail data");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing rail curve");
		}
		CSData data2 = iProfilePlacement.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing placement curve data");
		}
		OnCurve val2 = (OnCurve)(object)data2.GetNurbsCurve().Convert();
		if (val2 == null)
		{
			return UpdateMissing("Missing placement curve curve");
		}
		CSData data3 = iProfileShape.Data;
		if (data3 == null)
		{
			return UpdateMissing("Missing profile shape data");
		}
		OnCurve val3 = (OnCurve)(object)data3.GetNurbsCurve().Convert();
		if (val3 == null)
		{
			return UpdateMissing("Missing profile shape curve");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 45.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		int value3 = 1;
		iSubCount.GetInteger(ref value3);
		string input = "[SMI Cutter Band]" + Environment.NewLine + "Radius = " + value + "mm" + Environment.NewLine + "Angle = " + value2 + "Â°" + Environment.NewLine + "Sub-Count = " + value3;
		OnCurve val4 = null;
		OnCurve val5 = null;
		if (!val3.Split(val3.Domain().Mid(), ref val4, ref val5))
		{
			return UpdateError("Split failed");
		}
		int num = 2 * value3;
		double num2 = OnUtil.On_PI / (double)value3;
		double num3 = value2 / 2.0;
		double num4 = (OnUtil.On_PI - OnUtil.On_PI / (double)value3) / 2.0;
		On3dPoint val6 = new On3dPoint(0.0, value, 0.0);
		OnCurve val7 = val4.DuplicateCurve();
		((OnGeometry)val7).Rotate(num3, OnUtil.On_xaxis, (IOn3dPoint)(object)val6);
		OnCurve val8 = val4.DuplicateCurve();
		((OnGeometry)val8).Rotate(num3 / 2.0, OnUtil.On_xaxis, (IOn3dPoint)(object)val6);
		OnCurve val9 = val4.DuplicateCurve();
		((OnGeometry)val9).Rotate(0.0 - num4, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBoundingBox val10 = new OnBoundingBox();
		if (!((OnGeometry)val7).GetTightBoundingBox(ref val10))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		OnBoundingBox val11 = new OnBoundingBox();
		if (!((OnGeometry)val9).GetTightBoundingBox(ref val11))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num5 = Math.Abs(val10.Max().x - val10.Min().x);
		double num6 = Math.Abs(val11.Max().x - val11.Min().x);
		double num7 = num5 / num6;
		OnXform val12 = new OnXform();
		val12.Scale(num7, 1.0, num7);
		((OnGeometry)val9).Transform((IOnXform)(object)val12);
		if (!CSUtilSweep.Sweep1((IOnCurve)(object)OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val7.PointAtStart(), (IOn3dPoint)(object)val8.PointAtStart(), (IOn3dPoint)(object)val9.PointAtStart())), (IOnCurve)(object)val7, (IOnCurve)(object)val9, out var breps))
		{
			return UpdateError("Sweep1 failed");
		}
		if (breps.Length != 1)
		{
			return UpdateError("Sweep1 results failed");
		}
		OnBrep val13 = breps[0];
		OnXform val14 = new OnXform();
		val14.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		OnBrep val15 = val13.Duplicate();
		((OnGeometry)val15).Transform((IOnXform)(object)val14);
		OnBrep val16 = CSUtilRhino.JoinAllBreps((OnBrep[])(object)new OnBrep[2] { val13, val15 }, CSApplicationHelper.Application.Tolerance);
		if (val16 == null)
		{
			return UpdateError("JoinBreps failed");
		}
		OnXform val17 = new OnXform();
		val17.Rotation(num2, OnUtil.On_yaxis, OnUtil.On_origin);
		OnBrep[] array = (OnBrep[])(object)new OnBrep[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = val16.Duplicate();
			((OnGeometry)val16).Transform((IOnXform)(object)val17);
		}
		OnBrep val18 = CSUtilRhino.JoinAllBreps(array, CSApplicationHelper.Application.Tolerance);
		if (!val18.IsValidGeometry())
		{
			return UpdateError("JoinBreps failed");
		}
		OnBrep val19 = CSUtilRhino.CapPlanarHoles(val18, CSApplicationHelper.Application.Tolerance);
		if (!val19.IsValidGeometry())
		{
			return UpdateError("CapPlanarHoles failed");
		}
		CSDataHelper.GetTransformNoArray(iProfilePlacement.Data, out var value4);
		Transform transform = default(Transform);
		((Transform)(ref value4)).TryGetInverse(ref transform);
		((OnGeometry)val2).Transform((IOnXform)(object)transform.Convert());
		OnBoundingBox val20 = new OnBoundingBox();
		if (!((OnGeometry)val2).GetTightBoundingBox(ref val20))
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num8 = Math.Abs(val20.Max().y - val20.Min().y);
		OnXform val21 = new OnXform();
		val21.Translation(0.0, num8, 0.0);
		OnXform val22 = new OnXform();
		val22.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_yaxis, OnUtil.On_origin);
		OnXform x = new OnXform();
		if (!CSUtilGeometry.OrientProfileToCurveNalp((IOnCurve)(object)val, 0.0, ref x, 1.0))
		{
			return UpdateError("OrientProfileToCurveNalp failed");
		}
		((OnGeometry)val19).Transform((IOnXform)(object)(x * (IOnXform)(object)val22 * (IOnXform)(object)val21));
		oObject.SetData(new CSOnGeometryData((GeometryBase)(object)val19.Convert(), x.Convert() * val22.Convert() * val21.Convert(), applyTransform: false));
		oString.SetData(new CSStringData(input));
		return base.Update();
	}
}
