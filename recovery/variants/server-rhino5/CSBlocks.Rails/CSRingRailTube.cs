using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails;

[CSBlock("RingRailTube", "{2D87963C-25DA-4d6a-850E-5229B36BD53A}")]
public sealed class CSRingRailTube : CSBlock
{
	public CSInputChannel InRingRail { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InOffset { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InAngleOffset { get; }

	public CSOutputChannel OutSurface { get; }

	public CSOutputChannel OutTopLine { get; }

	public CSRingRailTube(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(GetPropertyName(() => InRingRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InLength = new CSInputChannel(GetPropertyName(() => InLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRingRailRadius = new CSInputChannel(GetPropertyName(() => InRingRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(GetPropertyName(() => InOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(GetPropertyName(() => InAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngleOffset = new CSInputChannel(GetPropertyName(() => InAngleOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(GetPropertyName(() => OutSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutTopLine = new CSOutputChannel(GetPropertyName(() => OutTopLine), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Expected O, but got Unknown
		//IL_05ae: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_0624: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		OnCurve val = (OnCurve)(object)InRingRail.GetNurbsCurve();
		if (val == null)
		{
			double value = 0.0;
			InRingRailRadius.GetDouble(ref value);
			OnCircle val2 = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve val3 = new OnNurbsCurve();
			val2.GetNurbForm(ref val3);
			val = (OnCurve)(object)val3;
			if (val == null)
			{
				return UpdateError("Failed to create ring rail from radius");
			}
		}
		double value2 = 60.0;
		InLength.GetDouble(ref value2);
		double value3 = 0.0;
		InOffset.GetDouble(ref value3);
		double value4 = 360.0;
		InAngle.GetDouble(ref value4);
		double value5 = 0.0;
		InAngleOffset.GetDouble(ref value5);
		if (Math.Abs(value3) > 0.01)
		{
			IOn3dVector normalDir = OnUtil.On_yaxis;
			OnBoundingBox val4 = new OnBoundingBox();
			((OnGeometry)val).GetBoundingBox(ref val4);
			IOn3dPoint origin = (IOn3dPoint)(object)val4.Center();
			OnPlane val5 = new OnPlane();
			if (val.IsPlanar(val5))
			{
				normalDir = (IOn3dVector)new On3dVector(val5.Normal());
			}
			OnCurve val6 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, normalDir, origin, value3, closed: true);
			if (val6 != null)
			{
				val = val6;
			}
		}
		if (value4 < 360.0 && value4 > 0.0)
		{
			OnBoundingBox val7 = new OnBoundingBox();
			((OnGeometry)val).GetBoundingBox(ref val7);
			OnLine val8 = new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, val7.Max().z * 2.0));
			OnXform val9 = new OnXform();
			val9.Rotation(CSBlockUtil.CSUtilMath.ToRadians((0.0 - value4) / 2.0), OnUtil.On_yaxis, OnUtil.On_origin);
			val8.Transform((IOnXform)(object)val9);
			OnLine val10 = new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, val7.Max().z * 2.0));
			OnXform val11 = new OnXform();
			val11.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4 / 2.0), OnUtil.On_yaxis, OnUtil.On_origin);
			val10.Transform((IOnXform)(object)val11);
			On3dPointArray val12 = new On3dPointArray();
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val8.from);
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val8.to);
			On3dPoint val13 = new On3dPoint(val8.to.x + val7.Diagonal().x, val8.to.y, val8.to.z);
			if (val8.to.x < 0.0)
			{
				val13 = new On3dPoint(val8.to.x - val7.Diagonal().x, val8.to.y, val8.to.z);
			}
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val13);
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)new On3dPoint(val13.x, val13.y, val13.z + val7.Diagonal().z * 2.0));
			On3dPoint val14 = new On3dPoint(val10.to.x + val7.Diagonal().x, val10.to.y, val10.to.z);
			if (val10.to.x < 0.0)
			{
				val14 = new On3dPoint(val10.to.x - val7.Diagonal().x, val10.to.y, val10.to.z);
			}
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)new On3dPoint(val14.x, val14.y, val14.z + val7.Diagonal().z * 2.0));
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val14);
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val10.to);
			((ArrayOn3dPoint)val12).Append((IOn3dPoint)(object)val8.from);
			OnPolylineCurve val15 = new OnPolylineCurve((IOn3dPointArray)val12);
			((OnGeometry)val15).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value5), OnUtil.On_yaxis, OnUtil.On_origin);
			IOnCurve[] array = (IOnCurve[])(object)RhUtil.RhinoCurveBooleanIntersection((IOnCurve)(object)val15, (IOnCurve)(object)val);
			IOnCurve[] array2 = array;
			if (array2 == null)
			{
				RhUtil.RhinoApp().ActiveDoc().AddCurveObject((IOnCurve)(object)val15);
			}
			OnCurve[] array3 = (OnCurve[])(object)new OnCurve[0];
			if (array2 != null)
			{
				RhUtil.RhinoMergeCurves(array2, ref array3);
				if (array3.Length == 1)
				{
					val = array3[0];
				}
			}
		}
		OnSurface val16 = RhUtil.RhinoExtrudeCurveStraight((IOnCurve)(object)val, (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), value2);
		if (val16 == null)
		{
			return UpdateError("RhinoExtrudeCurveStraight failed");
		}
		double num = 0.0;
		double num2 = 0.0;
		val16.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0), ref num, ref num2);
		if (val16.NormalAt(num, num2) * (IOn3dVector)new On3dVector(0.0, 0.0, 1.0) < 0.0)
		{
			val16.Reverse(0);
		}
		OnXform val17 = new OnXform();
		val17.Translation((IOn3dVector)new On3dVector(0.0, 0.0 - value2 / 2.0, 0.0));
		((OnGeometry)val16).Transform((IOnXform)(object)val17);
		OutSurface.SetData(new CSOnGeometryData(val16.Convert(), val17.Convert(), applyTransform: false));
		OnLineCurve val18 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT val19 = new ArrayOnX_EVENT();
		val.IntersectCurve((IOnCurve)(object)val18, ref val19);
		OnNurbsCurve nCrv = new OnNurbsCurve();
		if (val19.Count() > 0)
		{
			double z = val19[0][0].z;
			((OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, (0.0 - value2) / 2.0, z), (IOn3dPoint)new On3dPoint(0.0, value2 / 2.0, z))).GetNurbForm(ref nCrv);
			OutTopLine.SetData(new CSOnGeometryData(nCrv.Convert()));
		}
		return base.Update();
	}
}
