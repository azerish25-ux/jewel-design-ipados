using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V5", "{A7B36E38-6988-4CCD-A633-4F447E79572E}")]
public sealed class CSHeadProngs5 : CSBlock
{
	public CSOutputChannel oObjects { get; }

	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngLines { get; }

	public CSInputChannel iHeadTop { get; }

	public CSInputChannel iHeadBase { get; }

	public CSInputChannel iEnableVProngs { get; }

	public CSInputChannel iProngTopProfile { get; }

	public CSInputChannel iProngTopX { get; }

	public CSInputChannel iProngTopY { get; }

	public CSInputChannel iProngTopOffset { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseOffset { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iGameChanger { get; }

	public CSInputChannel iProngDomeHeight { get; }

	public CSInputChannel iProngDomeSettingDistance { get; }

	public CSInputChannel iProngDomeSettingAngle { get; }

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iBasetoPoint { get; }

	public CSHeadProngs5(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngLines = new CSInputChannel(GetPropertyName(() => iProngLines), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iHeadTop = new CSInputChannel(GetPropertyName(() => iHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iHeadBase = new CSInputChannel(GetPropertyName(() => iHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEnableVProngs = new CSInputChannel(GetPropertyName(() => iEnableVProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngTopProfile = new CSInputChannel(GetPropertyName(() => iProngTopProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngTopX = new CSInputChannel(GetPropertyName(() => iProngTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopY = new CSInputChannel(GetPropertyName(() => iProngTopY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopOffset = new CSInputChannel(GetPropertyName(() => iProngTopOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVTopAngle = new CSInputChannel(GetPropertyName(() => iProngVTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableBase = new CSInputChannel(GetPropertyName(() => iEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngBaseProfile = new CSInputChannel(GetPropertyName(() => iProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngBaseX = new CSInputChannel(GetPropertyName(() => iProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseY = new CSInputChannel(GetPropertyName(() => iProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseOffset = new CSInputChannel(GetPropertyName(() => iProngBaseOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVBaseAngle = new CSInputChannel(GetPropertyName(() => iProngVBaseAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngDomeHeight = new CSInputChannel(GetPropertyName(() => iProngDomeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingDistance = new CSInputChannel(GetPropertyName(() => iProngDomeSettingDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingAngle = new CSInputChannel(GetPropertyName(() => iProngDomeSettingAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngTopNudge = new CSInputChannel(GetPropertyName(() => iProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseNudge = new CSInputChannel(GetPropertyName(() => iProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBasetoPoint = new CSInputChannel(GetPropertyName(() => iBasetoPoint), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Expected O, but got Unknown
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Expected O, but got Unknown
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		OnNurbsCurve nurbsCurve = iProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		ICSArray iCSArray = iObjects.Data as ICSArray;
		double value = 2.0;
		iProngTopX.GetDouble(ref value);
		double value2 = 2.0;
		iProngTopY.GetDouble(ref value2);
		double value3 = 0.0;
		iProngTopOffset.GetDouble(ref value3);
		double value4 = 0.0;
		iProngVTopAngle.GetDouble(ref value4);
		double value5 = 0.0;
		iProngTopNudge.GetDouble(ref value5);
		value3 -= value2 * value5;
		int value6 = 0;
		iStyle.GetInteger(ref value6);
		bool value7 = false;
		iEnableVProngs.GetBoolean(ref value7);
		bool value8 = false;
		iEnableBase.GetBoolean(ref value8);
		bool value9 = false;
		iGameChanger.GetBoolean(ref value9);
		double value10 = 2.0;
		double value11 = 2.0;
		double value12 = 0.0;
		double value13 = 0.0;
		OnNurbsCurve val;
		if (value8)
		{
			val = iProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value10);
			iProngBaseY.GetDouble(ref value11);
			iProngBaseOffset.GetDouble(ref value12);
			iProngVBaseAngle.GetDouble(ref value13);
			double value14 = 0.0;
			iProngBaseNudge.GetDouble(ref value14);
			value12 -= value11 * value14;
		}
		else
		{
			val = nurbsCurve.Duplicate();
			value10 = value;
			value11 = value2;
			value12 = value3;
			value13 = value4;
		}
		double value15 = 0.5;
		iProngDomeHeight.GetDouble(ref value15);
		double value16 = 0.0;
		iProngDomeSettingDistance.GetDouble(ref value16);
		double value17 = 0.0;
		iProngDomeSettingAngle.GetDouble(ref value17);
		bool value18 = false;
		iBasetoPoint.GetBoolean(ref value18);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("GemData Missing");
			}
			OnNurbsCurve val2 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val2 == null)
			{
				return UpdateMissing("HeadTop Missing");
			}
			OnNurbsCurve val3 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val3 == null)
			{
				return UpdateMissing("HeadBase Missing");
			}
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines Missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray3))
				{
					continue;
				}
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value9 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value6, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value6, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve val4 = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (val4 == null)
				{
					CS.CSUtil.Print("Invalid Head Crv\n");
					continue;
				}
				On3dPoint val5 = ((OnCurve)val4).PointAtStart();
				double num = 0.0;
				((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)val5, ref num);
				On3dPoint val6 = ((OnCurve)val4).PointAtEnd();
				double num2 = 0.0;
				((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val6, ref num2);
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value7)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val2, num, value4, value2, value3, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val3, num2, value13, value11, value12, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve prongTopProfile = new OnNurbsCurve();
					vProng.GetNurbForm(ref prongTopProfile);
					OnNurbsCurve val7 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref val7);
					On3dPoint val8 = ((OnCurve)val4).PointAtStart();
					On3dVector val9 = ((OnCurve)val4).TangentAt(((OnCurve)val4).Domain().Min());
					val9.Unitize();
					On3dPoint val10 = val8 - (IOn3dVector)(object)(val9 * 0.5);
					OnLineCurve val11 = new OnLineCurve((IOn3dPoint)(object)val8, (IOn3dPoint)(object)val10);
					if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
					{
						(IOnCurve)val4,
						(IOnCurve)val11
					}, out OnCurve mergedCrv, 0.01))
					{
						return UpdateError("MergeCurves failed");
					}
					if ((mergedCrv.PointAtStart() - (IOn3dPoint)(object)val10).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref mergedCrv, ref prongTopProfile))
					{
						return UpdateError("Unable to create CAM prong extension.");
					}
					OnXform val12 = new OnXform();
					val12.Translation((IOn3dVector)(object)(val10 - (IOn3dPoint)(object)val8));
					prongTopProfile.Transform((IOnXform)(object)val12);
					if (!CSUtilSweep.Sweep1(profiles: (IOnCurve[])(object)new IOnCurve[2]
					{
						(IOnCurve)prongTopProfile,
						(IOnCurve)val7
					}, inputParameters: new double[2]
					{
						mergedCrv.Domain().Min(),
						mergedCrv.Domain().Max()
					}, inputRailCurve: (IOnCurve)(object)mergedCrv, breps: out var breps, closed: false))
					{
						return UpdateError("Failed Sweep1");
					}
					if (breps == null)
					{
						return UpdateError("Failed Sweep1: No results");
					}
					if (breps.Length == 0)
					{
						return UpdateError("Failed Sweep1: No results");
					}
					OnBrep val13 = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
					if (val13 == null)
					{
						return UpdateError("Failed CapPlanarHoles");
					}
					((OnGeometry)val13).Transform((IOnXform)(object)xUp.Convert());
					CSData data = new CSOnGeometryData((GeometryBase)(object)val13.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
					continue;
				}
				OnInterval val14 = ((OnCurve)val2).Domain();
				double num3 = (val14.Max() - val14.Min()) / 100.0;
				double num4 = num - num3;
				double num5 = num + num3;
				if (num4 < val14.Min())
				{
					num4 = val14.Max() + (num4 - val14.Min());
				}
				if (num5 > val14.Max())
				{
					num5 = val14.Min() + (num5 - val14.Max());
				}
				On3dVector val15 = ((OnCurve)val2).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)val2).TangentAt(num5);
				val15.Unitize();
				OnBoundingBox val16 = new OnBoundingBox();
				((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val16);
				OnXform val17 = new OnXform();
				val17.Scale(value / (val16.Max().x - val16.Min().x), value2 / (val16.Max().y - val16.Min().y), 0.0);
				OnXform val18 = new OnXform();
				val18.Translation(0.0, value3, 0.0);
				OnXform val19 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)val15, OnUtil.On_zaxis);
				OnNurbsCurve val20 = nurbsCurve.Duplicate();
				val20.Transform((IOnXform)(object)(val19 * (IOnXform)(object)val18 * (IOnXform)(object)val17));
				On3dVector val22;
				OnXform val23;
				if (!value18)
				{
					OnInterval val21 = ((OnCurve)val3).Domain();
					double num6 = (val21.Max() - val21.Min()) / 100.0;
					double num7 = num2 - num6;
					double num8 = num2 + num6;
					if (num7 < val21.Min())
					{
						num7 = val21.Max() + (num7 - val21.Min());
					}
					if (num8 > val21.Max())
					{
						num8 = val21.Min() + (num8 - val21.Max());
					}
					val22 = ((OnCurve)val3).TangentAt(num7) + (IOn3dVector)(object)((OnCurve)val3).TangentAt(num8);
					val22.Unitize();
					val23 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val6, (IOn3dVector)(object)val22, OnUtil.On_zaxis);
				}
				else
				{
					val23 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val6, (IOn3dVector)(object)val15, OnUtil.On_zaxis);
					val22 = val15;
				}
				OnXform val24 = val23.Inverse();
				val4.Duplicate().Transform((IOnXform)(object)val24);
				On3dVector inputVector = ((OnCurve)val4).TangentAt(((OnCurve)val4).Domain().Max());
				On3dVector inputVector2 = ((OnCurve)val4).TangentAt(((OnCurve)val4).Domain().Min());
				double num9 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)inputVector2, (IOn3dVector)(object)val15);
				double value19 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)inputVector, (IOn3dVector)(object)val22);
				double num10 = 1.0 / Math.Cos(Math.Abs(value19) - Math.Abs(num9));
				OnBoundingBox val25 = new OnBoundingBox();
				((OnGeometry)val).GetTightBoundingBox(ref val25);
				OnXform val26 = new OnXform();
				val26.Scale(value10 / (val25.Max().x - val25.Min().x), value11 / (val25.Max().y - val25.Min().y) * num10, 0.0);
				OnXform val27 = new OnXform();
				val27.Translation(0.0, value12, 0.0);
				OnNurbsCurve val28 = val.Duplicate();
				val28.Transform((IOnXform)(object)(val23 * (IOnXform)(object)val27 * (IOnXform)(object)val26));
				if (!CSUtilSweep.Sweep1((IOnCurve)(object)val4, (IOnCurve[])(object)new IOnCurve[2]
				{
					(IOnCurve)val20,
					(IOnCurve)val28
				}, new double[2]
				{
					((OnCurve)val4).Domain().Min(),
					((OnCurve)val4).Domain().Max()
				}, out var breps2, closed: false))
				{
					return UpdateError("Failed Sweep1");
				}
				if (breps2 == null)
				{
					return UpdateError("Failed Sweep1: No results");
				}
				if (breps2.Length == 0)
				{
					return UpdateError("Failed Sweep1: No results");
				}
				OnBrep dome = null;
				if (!CreateDome(nurbsCurve, value, value2, num9, value15, value16, value17, ref dome))
				{
					return UpdateError("Unable to create dome");
				}
				((OnGeometry)dome).Transform((IOnXform)(object)(val19 * (IOnXform)(object)val18));
				List<OnBrep> list2 = new List<OnBrep>();
				list2.AddRange(breps2);
				list2.Add(dome);
				OnBrep val29 = CSUtilRhino.JoinAllBreps(list2.ToArray(), 0.01);
				if (val29 == null)
				{
					return UpdateError("Failed JoinBreps");
				}
				val29 = CSUtilRhino.CapPlanarHoles(val29, 0.01);
				if (val29 == null)
				{
					return UpdateError("Failed CapPlanarHoles");
				}
				((OnGeometry)val29).Transform((IOnXform)(object)xUp.Convert());
				CSData data2 = new CSOnGeometryData((GeometryBase)(object)val29.Convert(), xUp, applyTransform: false);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oObjects.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CreateDome(OnNurbsCurve prongTopProfile, double prongTopX, double prongTopY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, ref OnBrep dome)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00da: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)prongTopProfile).GetTightBoundingBox(ref val);
		OnXform val2 = new OnXform();
		val2.Scale(prongTopX / (val.Max().x - val.Min().x), prongTopY / (val.Max().y - val.Min().y), 0.0);
		OnXform val3 = new OnXform();
		val3.Translation(0.0, (0.0 - prongTopY) / 2.0, 0.0);
		OnNurbsCurve val4 = prongTopProfile.Duplicate();
		val4.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnLineCurve val5 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, -50.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 50.0, 0.0));
		ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
		((OnCurve)val4).IntersectCurve((IOnCurve)(object)val5, ref val6);
		if (val6.Count() <= 0)
		{
			return false;
		}
		double num = val6[0][0];
		double num2 = val6[1][0];
		((OnCurve)val4).ChangeClosedCurveSeam(num);
		OnCurve val7 = null;
		OnCurve val8 = null;
		if (!((OnCurve)val4).Split(num2, ref val7, ref val8))
		{
			return false;
		}
		if (val7 == null || val8 == null)
		{
			return false;
		}
		On3dVector obj = ((OnCurve)val5).TangentAt(((OnCurve)val5).Domain().Mid());
		obj.Unitize();
		On3dVector val9 = val7.TangentAt(val7.Domain().Mid());
		val9.Unitize();
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj, (IOn3dVector)(object)val9) < 0.0)
		{
			val7.Reverse();
		}
		val8.TangentAt(val8.Domain().Mid()).Unitize();
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj, (IOn3dVector)(object)val8.TangentAt(val8.Domain().Mid())) < 0.0)
		{
			val8.Reverse();
		}
		On3dVector val10 = new On3dVector(OnUtil.On_zaxis);
		val10.Rotate(0.0 - angle, OnUtil.On_xaxis);
		val10.Unitize();
		On3dPoint val11 = val.Center();
		val11.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		val11 += (IOn3dVector)(object)(val10 * (prongDomeHeight / Math.Cos(angle)));
		val11 += (IOn3dVector)(object)(new On3dVector(OnUtil.On_yaxis) * prongDomeSettingDistance);
		On3dVector val12 = new On3dVector(OnUtil.On_yaxis) * -1.0;
		val12.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongDomeSettingAngle), OnUtil.On_xaxis);
		val12.Unitize();
		On3dVector val13 = new On3dVector((IOn3dVector)(object)val12);
		val13.Reverse();
		OnNurbsCurve val14 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)val7.PointAtStart(), (IOn3dPoint)(object)val11, (IOn3dVector)(object)val10, (IOn3dVector)(object)val12);
		OnNurbsCurve val15 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)val7.PointAtEnd(), (IOn3dPoint)(object)val11, (IOn3dVector)(object)val10, (IOn3dVector)(object)val13);
		IOnCurve[] obj2 = new IOnCurve[2]
		{
			(IOnCurve)val14,
			(IOnCurve)val15
		};
		OnCurve[] array = null;
		if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj2, ref array))
		{
			return false;
		}
		if (array == null)
		{
			return false;
		}
		if (array.Length == 0)
		{
			return false;
		}
		OnLineCurve val16 = new OnLineCurve((IOn3dPoint)new On3dPoint(-50.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(50.0, 0.0, 0.0));
		ArrayOnX_EVENT val17 = new ArrayOnX_EVENT();
		val7.IntersectCurve((IOnCurve)(object)val16, ref val17);
		if (val17.Count() <= 0)
		{
			return false;
		}
		double num3 = val17[0][0];
		OnCurve val18 = null;
		OnCurve val19 = null;
		if (!val7.Split(num3, ref val18, ref val19))
		{
			return false;
		}
		if (val18 == null || val19 == null)
		{
			return false;
		}
		ArrayOnX_EVENT val20 = new ArrayOnX_EVENT();
		val8.IntersectCurve((IOnCurve)(object)val16, ref val20);
		if (val20.Count() <= 0)
		{
			return false;
		}
		double num4 = val20[0][0];
		OnCurve val21 = null;
		OnCurve val22 = null;
		if (!val8.Split(num4, ref val21, ref val22))
		{
			return false;
		}
		if (val21 == null || val22 == null)
		{
			return false;
		}
		OnCurve railCurve = val18;
		OnCurve railCurve2 = val19;
		IOnCurve[] profiles = (IOnCurve[])(object)array;
		if (!CSUtilSweep.Sweep2NoParam((IOnCurve)(object)railCurve, (IOnCurve)(object)railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		OnCurve railCurve3 = val21;
		OnCurve railCurve4 = val22;
		profiles = (IOnCurve[])(object)array;
		if (!CSUtilSweep.Sweep2NoParam((IOnCurve)(object)railCurve3, (IOnCurve)(object)railCurve4, profiles, out var breps2, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		List<OnBrep> list = new List<OnBrep>();
		list.AddRange(breps);
		list.AddRange(breps2);
		OnBrep val23 = CSUtilRhino.JoinAllBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (val23 == null)
		{
			return false;
		}
		OnXform val24 = new OnXform();
		val24.Translation(0.0, prongTopY / 2.0, 0.0);
		((OnGeometry)val23).Transform((IOnXform)(object)val24);
		dome = val23;
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref OnCurve prongRail, ref OnNurbsCurve prongTopProfile)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		On3dVector val = prongRail.TangentAt(prongRail.Domain().Min());
		val.Unitize();
		On3dPoint val2 = prongRail.PointAtStart();
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
