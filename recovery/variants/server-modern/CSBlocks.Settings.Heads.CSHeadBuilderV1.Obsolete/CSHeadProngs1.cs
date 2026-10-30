using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V1", "{0A4C2741-A988-4498-88A5-EAB94A116465}")]
public sealed class CSHeadProngs1 : CSBlock
{
	public CSInputChannel IGem { get; }

	public CSInputChannel IStyle { get; }

	public CSInputChannel IProngLines { get; }

	public CSInputChannel IHeadTop { get; }

	public CSInputChannel IHeadBase { get; }

	public CSInputChannel IProngTopProfile { get; }

	public CSInputChannel IProngTopX { get; }

	public CSInputChannel IProngTopY { get; }

	public CSInputChannel IProngTopNudge { get; }

	public CSInputChannel IEnableBase { get; }

	public CSInputChannel IProngBaseProfile { get; }

	public CSInputChannel IProngBaseX { get; }

	public CSInputChannel IProngBaseY { get; }

	public CSInputChannel IProngBaseNudge { get; }

	public CSOutputChannel OProngs { get; }

	public CSHeadProngs1(string identifier)
		: base(identifier)
	{
		IGem = new CSInputChannel(GetPropertyName(() => IGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		IStyle = new CSInputChannel(GetPropertyName(() => IStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngLines = new CSInputChannel(GetPropertyName(() => IProngLines), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		IHeadTop = new CSInputChannel(GetPropertyName(() => IHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IHeadBase = new CSInputChannel(GetPropertyName(() => IHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngTopProfile = new CSInputChannel(GetPropertyName(() => IProngTopProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngTopX = new CSInputChannel(GetPropertyName(() => IProngTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngTopY = new CSInputChannel(GetPropertyName(() => IProngTopY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngTopNudge = new CSInputChannel(GetPropertyName(() => IProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IEnableBase = new CSInputChannel(GetPropertyName(() => IEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IProngBaseProfile = new CSInputChannel(GetPropertyName(() => IProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IProngBaseX = new CSInputChannel(GetPropertyName(() => IProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngBaseY = new CSInputChannel(GetPropertyName(() => IProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngBaseNudge = new CSInputChannel(GetPropertyName(() => IProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OProngs = new CSOutputChannel(GetPropertyName(() => OProngs), this, ChannelDataType.Brep, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Input Gem Missing");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Input Gem Mssing");
		}
		int value = 0;
		IStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = IHeadTop.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("HeadTop Curve Missing");
		}
		OnNurbsCurve nurbsCurve2 = IHeadBase.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("HeadBase Curve Missing");
		}
		OnNurbsCurve nurbsCurve3 = IProngTopProfile.GetNurbsCurve();
		if (nurbsCurve3 == null)
		{
			return UpdateMissing("ProngTopProfile Curve Missing");
		}
		CSDataArray flattenArray2 = IProngLines.GetFlattenArray();
		if (flattenArray2 == null)
		{
			return UpdateMissing("ProngLines Missing");
		}
		double value2 = 2.0;
		IProngTopX.GetDouble(ref value2);
		double value3 = 2.0;
		IProngTopY.GetDouble(ref value3);
		double value4 = 0.0;
		IProngTopNudge.GetDouble(ref value4);
		bool value5 = false;
		IEnableBase.GetBoolean(ref value5);
		double value6 = 2.0;
		double value7 = 2.0;
		double value8 = 0.0;
		OnNurbsCurve val;
		if (value5)
		{
			val = IProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("ProngBaseProfile Missing");
			}
			IProngBaseX.GetDouble(ref value6);
			IProngBaseY.GetDouble(ref value7);
			IProngBaseNudge.GetDouble(ref value8);
		}
		else
		{
			val = nurbsCurve3.Duplicate();
			value6 = value2;
			value7 = value3;
			value8 = value4;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var _);
		CSHeadStyles1.GetHeadData1(cSGemData.GemShape, value, out var position, out var _, out var _);
		if (position == null)
		{
			return UpdateError("Head Style Missing");
		}
		OnXform val2 = new OnXform();
		OnXform val3 = new OnXform();
		OnBoundingBox val4 = new OnBoundingBox();
		((OnGeometry)nurbsCurve3).GetTightBoundingBox(ref val4);
		val2.Scale(value2 / (val4.Max().x - val4.Min().x), value3 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value4, 0.0);
		OnNurbsCurve val5 = nurbsCurve3.Duplicate();
		nurbsCurve3.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnXform val6 = new OnXform();
		val6.Translation(0.0, (0.0 - value3) / 2.0, 0.0);
		val5.Transform((IOnXform)(object)(val6 * (IOnXform)(object)val2));
		((OnGeometry)val).GetTightBoundingBox(ref val4);
		val2.Scale(value6 / (val4.Max().x - val4.Min().x), value7 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value8, 0.0);
		val.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnNurbsSurface val7 = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)CSUtilGeometry.CreateDome(0.5), (IOnNurbsCurve)(object)val5, 1);
		val6.Translation(0.0, value3 / 2.0 + value4, 0.0);
		val7.Transform((IOnXform)(object)val6);
		On3dPoint source = new On3dPoint(0.0, 0.0, 0.0);
		On3dVector source2 = new On3dVector(1.0, 0.0, 0.0);
		On3dVector val8 = new On3dVector(0.0, 0.0, 1.0);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < position.Length; i++)
		{
			OnNurbsCurve prongRail = flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
			On3dPoint val9 = ((OnCurve)prongRail).PointAtStart();
			On3dPoint val10 = ((OnCurve)prongRail).PointAtEnd();
			double num = 0.0;
			((OnCurve)nurbsCurve).GetClosestPoint((IOn3dPoint)(object)val9, ref num);
			double num2 = 0.0;
			((OnCurve)nurbsCurve2).GetClosestPoint((IOn3dPoint)(object)val10, ref num2);
			OnInterval val11 = ((OnCurve)nurbsCurve).Domain();
			double num3 = (val11.Max() - val11.Min()) / 100.0;
			double num4 = num - num3;
			double num5 = num + num3;
			if (num4 < val11.Min())
			{
				num4 = val11.Max() + (num4 - val11.Min());
			}
			if (num5 > val11.Max())
			{
				num5 = val11.Min() + (num5 - val11.Max());
			}
			On3dVector val12 = ((OnCurve)nurbsCurve).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)nurbsCurve).TangentAt(num5);
			val12.Unitize();
			val11 = ((OnCurve)nurbsCurve2).Domain();
			num3 = (val11.Max() - val11.Min()) / 100.0;
			num4 = num2 - num3;
			num5 = num2 + num3;
			if (num4 < val11.Min())
			{
				num4 = val11.Max() + (num4 - val11.Min());
			}
			if (num5 > val11.Max())
			{
				num5 = val11.Min() + (num5 - val11.Max());
			}
			On3dVector val13 = ((OnCurve)nurbsCurve2).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)nurbsCurve2).TangentAt(num5);
			val13.Unitize();
			OnXform val14 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)source, (IOn3dVector)(object)source2, (IOn3dVector)(object)val8, (IOn3dPoint)(object)val9, (IOn3dVector)(object)val12, (IOn3dVector)(object)val8);
			OnXform val15 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)source, (IOn3dVector)(object)source2, (IOn3dVector)(object)val8, (IOn3dPoint)(object)val10, (IOn3dVector)(object)val13, (IOn3dVector)(object)val8);
			OnNurbsCurve prongTopProfile = nurbsCurve3.Duplicate();
			prongTopProfile.Transform((IOnXform)(object)val14);
			OnNurbsCurve val16 = val.Duplicate();
			val16.Transform((IOnXform)(object)val15);
			OnCurve[] array = (OnCurve[])(object)new OnCurve[2]
			{
				(OnCurve)prongTopProfile,
				(OnCurve)val16
			};
			double[] inputParameters = new double[2]
			{
				((OnCurve)prongRail).Domain().Min(),
				((OnCurve)prongRail).Domain().Max()
			};
			OnBrep[] breps = null;
			if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
			{
				return UpdateError("Unable to create CAM prong extension.");
			}
			OnNurbsCurve inputRailCurve = prongRail;
			IOnCurve[] profiles = (IOnCurve[])(object)array;
			if (!CSUtilSweep.Sweep1((IOnCurve)(object)inputRailCurve, profiles, inputParameters, out breps, closed: false))
			{
				return UpdateError("Head Prong Sweep Failed");
			}
			((OnGeometry)breps[0]).Transform((IOnXform)(object)xUp);
			OnNurbsSurface val17 = val7.Duplicate();
			val9 = ((OnCurve)prongRail).PointAt(((OnCurve)prongRail).Domain().Min());
			On3dVector val18 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
			val9.z = 0.0;
			val18.z = 0.0;
			On3dVector val19 = new On3dVector((IOn3dPoint)(object)val9);
			double num6 = val18.Length();
			if (val19.Length() < (val19 + (IOn3dVector)(object)val18).Length())
			{
				num6 = 0.0 - num6;
			}
			for (int j = 0; j < val17.CVCount(0); j++)
			{
				for (int k = 0; k < val17.CVCount(1); k++)
				{
					On4dPoint val20 = On4dPoint.op_Implicit((IOn3dPoint)new On3dPoint());
					val17.GetCV(j, k, ref val20);
					val20.y += num6 * val20.z;
					val17.SetCV(j, k, (IOn4dPoint)(object)val20);
				}
			}
			val17.Transform((IOnXform)(object)val14);
			val17.Transform((IOnXform)(object)xUp);
			OnBrep brep = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(new List<OnBrep>
			{
				breps[0],
				((OnGeometry)val17).BrepForm()
			}.ToArray(), 0.01), 0.01);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
		}
		OProngs.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref OnNurbsCurve prongRail, ref OnNurbsCurve prongTopProfile)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		On3dVector val = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
		val.Unitize();
		On3dPoint val2 = ((OnCurve)prongRail).PointAtStart();
		On3dPoint val3 = val2 - (IOn3dVector)(object)(val * (prongExtensionHeight + CSApplicationHelper.Application.CamProngExtensionHeight));
		OnXform val4 = new OnXform();
		val4.Translation((IOn3dVector)(object)(val3 - (IOn3dPoint)(object)val2));
		prongTopProfile.Transform((IOnXform)(object)val4);
		OnLineCurve val5 = new OnLineCurve((IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3);
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)prongRail,
			(IOnCurve)val5
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if ((mergedCrv.PointAtStart() - (IOn3dPoint)(object)val3).Length() > 0.01)
		{
			mergedCrv.Reverse();
		}
		return true;
	}
}
