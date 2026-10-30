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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V6", "{37D61A46-44B6-49B3-AE19-13BF2DAFF72F}")]
public sealed class CSHeadProngs6 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngLines { get; }

	public CSInputChannel iHeadTop { get; }

	public CSInputChannel iHeadBase { get; }

	public CSInputChannel iEnableVProngs { get; }

	public CSInputChannel iProngTopProfile { get; }

	public CSInputChannel iProngTopX { get; }

	public CSInputChannel iProngTopY { get; }

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iEnableModeGameChanger { get; }

	public CSInputChannel iProngDomeHeight { get; }

	public CSInputChannel iProngDomeSettingDistance { get; }

	public CSInputChannel iProngDomeSettingAngle { get; }

	public CSInputChannel iBaseToPoint { get; }

	public CSInputChannel iEnableModeDoItYourself { get; }

	public CSOutputChannel oObjects { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadProngs6(string identifier)
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
		iProngTopNudge = new CSInputChannel(GetPropertyName(() => iProngTopNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVTopAngle = new CSInputChannel(GetPropertyName(() => iProngVTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableBase = new CSInputChannel(GetPropertyName(() => iEnableBase), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngBaseProfile = new CSInputChannel(GetPropertyName(() => iProngBaseProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProngBaseX = new CSInputChannel(GetPropertyName(() => iProngBaseX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseY = new CSInputChannel(GetPropertyName(() => iProngBaseY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngBaseNudge = new CSInputChannel(GetPropertyName(() => iProngBaseNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVBaseAngle = new CSInputChannel(GetPropertyName(() => iProngVBaseAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iEnableModeGameChanger = new CSInputChannel(GetPropertyName(() => iEnableModeGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProngDomeHeight = new CSInputChannel(GetPropertyName(() => iProngDomeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingDistance = new CSInputChannel(GetPropertyName(() => iProngDomeSettingDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngDomeSettingAngle = new CSInputChannel(GetPropertyName(() => iProngDomeSettingAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseToPoint = new CSInputChannel(GetPropertyName(() => iBaseToPoint), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iEnableModeDoItYourself = new CSInputChannel(GetPropertyName(() => iEnableModeDoItYourself), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Expected O, but got Unknown
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Expected O, but got Unknown
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Expected O, but got Unknown
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
		iProngTopNudge.GetDouble(ref value3);
		double value4 = 0.0;
		iProngVTopAngle.GetDouble(ref value4);
		double num = 0.0 - value2 * value3;
		int value5 = 0;
		iStyle.GetInteger(ref value5);
		bool value6 = false;
		iEnableVProngs.GetBoolean(ref value6);
		bool value7 = false;
		iEnableBase.GetBoolean(ref value7);
		bool value8 = false;
		iEnableModeGameChanger.GetBoolean(ref value8);
		bool value9 = false;
		iEnableModeDoItYourself.GetBoolean(ref value9);
		double value10 = 2.0;
		double value11 = 2.0;
		double value12 = 0.0;
		double value13 = 0.0;
		OnNurbsCurve val;
		double num2;
		if (value7)
		{
			val = iProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value10);
			iProngBaseY.GetDouble(ref value11);
			iProngBaseNudge.GetDouble(ref value12);
			iProngVBaseAngle.GetDouble(ref value13);
			num2 = 0.0 - value11 * value12;
		}
		else
		{
			val = nurbsCurve.Duplicate();
			value10 = value;
			value11 = value2;
			value12 = value3;
			value13 = value4;
			num2 = num;
		}
		double value14 = 0.5;
		iProngDomeHeight.GetDouble(ref value14);
		double value15 = 0.0;
		iProngDomeSettingDistance.GetDouble(ref value15);
		double value16 = 0.0;
		iProngDomeSettingAngle.GetDouble(ref value16);
		bool value17 = false;
		iBaseToPoint.GetBoolean(ref value17);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("Gem Missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			OnNurbsCurve val2 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val2 == null)
			{
				return UpdateMissing("HeadTop Missing");
			}
			cSDataArray2.Append(new CSOnGeometryData(val2.Convert(), xUp));
			OnNurbsCurve val3 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val3 == null)
			{
				return UpdateMissing("HeadBase Missing");
			}
			cSDataArray2.Append(new CSOnGeometryData(val3.Convert(), xUp));
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines Missing");
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray3))
				{
					continue;
				}
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray3.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value8 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value5, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value5, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve prongRail = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.PrintLine("Invalid Head Crv");
					continue;
				}
				cSDataArray2.Append(new CSOnGeometryData(prongRail.Convert(), xUp));
				On3dPoint val4 = ((OnCurve)prongRail).PointAtStart();
				double num3 = 0.0;
				((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)val4, ref num3);
				On3dPoint val5 = ((OnCurve)prongRail).PointAtEnd();
				double num4 = 0.0;
				((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val5, ref num4);
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value6)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val2, num3, value4, value2, num, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val3, num4, value13, value11, num2, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve val6 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref val6);
					On3dPoint val7 = ((OnCurve)prongRail).PointAtStart();
					On3dVector val8 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
					val8.Unitize();
					On3dPoint val9 = val7 - (IOn3dVector)(object)(val8 * 0.5);
					OnXform val10 = new OnXform();
					val10.Translation((IOn3dVector)(object)(val9 - (IOn3dPoint)(object)val7));
					OnNurbsCurve val11 = new OnNurbsCurve();
					vProng.GetNurbForm(ref val11);
					val11.Transform((IOnXform)(object)val10);
					cSDataArray2.Append(new CSOnGeometryData(val6.Convert(), xUp));
					cSDataArray2.Append(new CSOnGeometryData(val11.Convert(), xUp));
					if (!value9)
					{
						OnLineCurve val12 = new OnLineCurve((IOn3dPoint)(object)val7, (IOn3dPoint)(object)val9);
						if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
						{
							(IOnCurve)prongRail,
							(IOnCurve)val12
						}, out OnCurve mergedCrv, 0.01))
						{
							return UpdateError("MergeCurves failed");
						}
						if ((mergedCrv.PointAtStart() - (IOn3dPoint)(object)val9).Length() > 0.01)
						{
							mergedCrv.Reverse();
						}
						if (!CSUtilSweep.Sweep1(profiles: (IOnCurve[])(object)new IOnCurve[2]
						{
							(IOnCurve)val11,
							(IOnCurve)val6
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
						cSDataArray3.Append(new CSOnGeometryData(val13.Convert(), xUp, applyTransform: false));
					}
					continue;
				}
				OnInterval val14 = ((OnCurve)val2).Domain();
				double num5 = (val14.Max() - val14.Min()) / 100.0;
				double num6 = num3 - num5;
				double num7 = num3 + num5;
				if (num6 < val14.Min())
				{
					num6 = val14.Max() + (num6 - val14.Min());
				}
				if (num7 > val14.Max())
				{
					num7 = val14.Min() + (num7 - val14.Max());
				}
				On3dVector val15 = ((OnCurve)val2).TangentAt(num6) + (IOn3dVector)(object)((OnCurve)val2).TangentAt(num7);
				val15.Unitize();
				OnBoundingBox val16 = new OnBoundingBox();
				((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val16);
				OnXform val17 = new OnXform();
				val17.Scale(value / (val16.Max().x - val16.Min().x), value2 / (val16.Max().y - val16.Min().y), 0.0);
				OnXform val18 = new OnXform();
				val18.Translation(0.0, num, 0.0);
				OnXform val19 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val4, (IOn3dVector)(object)val15, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform((IOnXform)(object)(val19 * (IOnXform)(object)val18 * (IOnXform)(object)val17));
				On3dVector val21;
				OnXform val22;
				if (!value17)
				{
					OnInterval val20 = ((OnCurve)val3).Domain();
					double num8 = (val20.Max() - val20.Min()) / 100.0;
					double num9 = num4 - num8;
					double num10 = num4 + num8;
					if (num9 < val20.Min())
					{
						num9 = val20.Max() + (num9 - val20.Min());
					}
					if (num10 > val20.Max())
					{
						num10 = val20.Min() + (num10 - val20.Max());
					}
					val21 = ((OnCurve)val3).TangentAt(num9) + (IOn3dVector)(object)((OnCurve)val3).TangentAt(num10);
					val21.Unitize();
					val22 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)val21, OnUtil.On_zaxis);
				}
				else
				{
					val22 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)val15, OnUtil.On_zaxis);
					val21 = val15;
				}
				On3dVector inputVector = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Max());
				On3dVector inputVector2 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
				double num11 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)inputVector2, (IOn3dVector)(object)val15);
				double value18 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)inputVector, (IOn3dVector)(object)val21);
				double num12 = 1.0 / Math.Cos(Math.Abs(value18) - Math.Abs(num11));
				if (num12 > 2.25)
				{
					num12 = 2.25;
				}
				OnBoundingBox val23 = new OnBoundingBox();
				((OnGeometry)val).GetTightBoundingBox(ref val23);
				OnXform val24 = new OnXform();
				val24.Scale(value10 / (val23.Max().x - val23.Min().x), value11 / (val23.Max().y - val23.Min().y) * num12, 0.0);
				OnXform val25 = new OnXform();
				val25.Translation(0.0, num2, 0.0);
				OnNurbsCurve val26 = val.Duplicate();
				val26.Transform((IOnXform)(object)(val22 * (IOnXform)(object)val25 * (IOnXform)(object)val24));
				cSDataArray2.Append(new CSOnGeometryData(val26.Convert(), xUp));
				cSDataArray2.Append(new CSOnGeometryData(prongTopProfile.Convert(), xUp));
				OnBrep dome = null;
				if (CSApplicationHelper.Application.IsCamMode && !value9)
				{
					if (!CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
					{
						return UpdateError("Unable to create CAM prong extension.");
					}
				}
				else
				{
					if (!CreateDome(nurbsCurve, value, value2, num11, value14, value15, value16, ref dome))
					{
						return UpdateError("Unable to create dome");
					}
					((OnGeometry)dome).Transform((IOnXform)(object)(val19 * (IOnXform)(object)val18));
					if (value9)
					{
						cSDataArray3.Append(new CSOnGeometryData(dome.Convert(), xUp));
						continue;
					}
				}
				if (!CSUtilSweep.Sweep1(profiles: (IOnCurve[])(object)new IOnCurve[2]
				{
					(IOnCurve)prongTopProfile,
					(IOnCurve)val26
				}, inputParameters: new double[2]
				{
					((OnCurve)prongRail).Domain().Min(),
					((OnCurve)prongRail).Domain().Max()
				}, inputRailCurve: (IOnCurve)(object)prongRail, breps: out var breps2, closed: false))
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
				List<OnBrep> list2 = new List<OnBrep>();
				if (!CSApplicationHelper.Application.IsCamMode)
				{
					list2.Add(dome);
				}
				list2.AddRange(breps2);
				OnBrep val27 = CSUtilRhino.JoinAllBreps(list2.ToArray(), 0.01);
				if (val27 == null)
				{
					return UpdateError("Failed JoinBreps");
				}
				val27 = CSUtilRhino.CapPlanarHoles(val27, 0.01);
				if (val27 == null)
				{
					return UpdateError("Failed CapPlanarHoles");
				}
				((OnGeometry)val27).Transform((IOnXform)(object)xUp.Convert());
				cSDataArray3.Append(new CSOnGeometryData(val27.Convert(), xUp, applyTransform: false));
			}
			cSDataArray.Append(cSDataArray3);
		}
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
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
