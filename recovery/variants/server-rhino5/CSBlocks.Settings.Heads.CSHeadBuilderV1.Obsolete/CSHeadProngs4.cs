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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V4", "{4F1D12DB-9A3A-4702-B865-BE11C4E468A8}")]
public sealed class CSHeadProngs4 : CSBlock
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

	public CSInputChannel iProngTopNudge { get; }

	public CSInputChannel iProngVTopAngle { get; }

	public CSInputChannel iEnableBase { get; }

	public CSInputChannel iProngBaseProfile { get; }

	public CSInputChannel iProngBaseX { get; }

	public CSInputChannel iProngBaseY { get; }

	public CSInputChannel iProngBaseNudge { get; }

	public CSInputChannel iProngVBaseAngle { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iGameChanger { get; }

	public CSHeadProngs4(string identifier)
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
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Expected O, but got Unknown
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = iProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		bool value2 = false;
		iEnableVProngs.GetBoolean(ref value2);
		double value3 = 2.0;
		iProngTopX.GetDouble(ref value3);
		double value4 = 2.0;
		iProngTopY.GetDouble(ref value4);
		double value5 = 0.0;
		iProngTopNudge.GetDouble(ref value5);
		double value6 = 0.0;
		iProngVTopAngle.GetDouble(ref value6);
		bool value7 = false;
		iEnableBase.GetBoolean(ref value7);
		bool value8 = false;
		iGameChanger.GetBoolean(ref value8);
		ICSArray iCSArray = iObjects.Data as ICSArray;
		double value9 = 2.0;
		double value10 = 2.0;
		double value11 = 0.0;
		double value12 = 0.0;
		OnNurbsCurve val;
		if (value7)
		{
			val = iProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			iProngBaseX.GetDouble(ref value9);
			iProngBaseY.GetDouble(ref value10);
			iProngBaseNudge.GetDouble(ref value11);
			iProngVBaseAngle.GetDouble(ref value12);
		}
		else
		{
			val = nurbsCurve.Duplicate();
			value9 = value3;
			value10 = value4;
			value11 = value5;
			value12 = value6;
		}
		OnXform val2 = new OnXform();
		OnXform val3 = new OnXform();
		OnBoundingBox val4 = new OnBoundingBox();
		((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val4);
		val2.Scale(value3 / (val4.Max().x - val4.Min().x), value4 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value5, 0.0);
		OnNurbsCurve val5 = nurbsCurve.Duplicate();
		nurbsCurve.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnXform val6 = new OnXform();
		val6.Translation(0.0, (0.0 - value4) / 2.0, 0.0);
		val5.Transform((IOnXform)(object)(val6 * (IOnXform)(object)val2));
		((OnGeometry)val).GetTightBoundingBox(ref val4);
		val2.Scale(value9 / (val4.Max().x - val4.Min().x), value10 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value11, 0.0);
		val.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnNurbsSurface val7 = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)CSUtilGeometry.CreateDome(0.5), (IOnNurbsCurve)(object)val5, 1);
		val6.Translation(0.0, value4 / 2.0 + value5, 0.0);
		val7.Transform((IOnXform)(object)val6);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("GemArray missing");
			}
			OnNurbsCurve val8 = iHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val8 == null)
			{
				return UpdateMissing("HeadTop curve missing");
			}
			OnNurbsCurve val9 = iHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val9 == null)
			{
				return UpdateMissing("HeadBase curve missing");
			}
			if (!(iProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateMissing("ProngLines missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				ICSArray iCSArray3 = iCSArray.GetArrayItem(i) as ICSArray;
				for (int j = 0; j < iCSArray3.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray3.GetArrayItem(j));
				}
			}
			List<CSHeadStyles2.HeadProngInfo> list = (value8 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value, cSGemData.VersionMajor));
			if (list == null)
			{
				return UpdateError("Head Style Invalid");
			}
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve prongRail = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.Print("Invalid Head Crv\n");
					continue;
				}
				On3dPoint val10 = ((OnCurve)prongRail).PointAtStart();
				On3dPoint val11 = ((OnCurve)prongRail).PointAtEnd();
				double num = 0.0;
				((OnCurve)val8).GetClosestPoint((IOn3dPoint)(object)val10, ref num);
				double num2 = 0.0;
				((OnCurve)val9).GetClosestPoint((IOn3dPoint)(object)val11, ref num2);
				IOnCurve[] profiles;
				if (list[k].Type == CSHeadStyles2.EHeadProngType.Straight && value2)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val8, num, value6, value4, value5, out var vProng))
					{
						return UpdateError("Error Making V Prong Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val9, num2, value12, value10, value11, out var vProng2))
					{
						return UpdateError("Error Making V Prong Top");
					}
					OnNurbsCurve val12 = new OnNurbsCurve();
					vProng.GetNurbForm(ref val12);
					OnNurbsCurve val13 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref val13);
					On3dPoint val14 = ((OnCurve)prongRail).PointAtStart();
					On3dVector val15 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
					val15.Unitize();
					On3dPoint val16 = val14 - (IOn3dVector)(object)(val15 * 0.5);
					OnLineCurve val17 = new OnLineCurve((IOn3dPoint)(object)val14, (IOn3dPoint)(object)val16);
					CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
					{
						(IOnCurve)prongRail,
						(IOnCurve)val17
					}, out OnCurve mergedCrv, 0.01);
					if ((mergedCrv.PointAtStart() - (IOn3dPoint)(object)val16).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					OnXform val18 = new OnXform();
					val18.Translation((IOn3dVector)(object)(val16 - (IOn3dPoint)(object)val14));
					val12.Transform((IOnXform)(object)val18);
					OnCurve[] array = (OnCurve[])(object)new OnCurve[2]
					{
						(OnCurve)val12,
						(OnCurve)val13
					};
					double[] inputParameters = new double[2]
					{
						mergedCrv.Domain().Min(),
						mergedCrv.Domain().Max()
					};
					OnCurve inputRailCurve = mergedCrv;
					profiles = (IOnCurve[])(object)array;
					if (!CSUtilSweep.Sweep1((IOnCurve)(object)inputRailCurve, profiles, inputParameters, out var breps, closed: false))
					{
						return UpdateError("HeadProngs Failed Sweep");
					}
					OnBrep obj = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
					((OnGeometry)obj).Transform((IOnXform)(object)xUp.Convert());
					CSData data = new CSOnGeometryData(obj.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
					continue;
				}
				OnInterval val19 = ((OnCurve)val8).Domain();
				double num3 = (val19.Max() - val19.Min()) / 100.0;
				double num4 = num - num3;
				double num5 = num + num3;
				if (num4 < val19.Min())
				{
					num4 = val19.Max() + (num4 - val19.Min());
				}
				if (num5 > val19.Max())
				{
					num5 = val19.Min() + (num5 - val19.Max());
				}
				On3dVector val20 = ((OnCurve)val8).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)val8).TangentAt(num5);
				val20.Unitize();
				val19 = ((OnCurve)val9).Domain();
				num3 = (val19.Max() - val19.Min()) / 100.0;
				num4 = num2 - num3;
				num5 = num2 + num3;
				if (num4 < val19.Min())
				{
					num4 = val19.Max() + (num4 - val19.Min());
				}
				if (num5 > val19.Max())
				{
					num5 = val19.Min() + (num5 - val19.Max());
				}
				On3dVector val21 = ((OnCurve)val9).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)val9).TangentAt(num5);
				val21.Unitize();
				OnXform val22 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val10, (IOn3dVector)(object)val20, OnUtil.On_zaxis);
				OnXform val23 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val11, (IOn3dVector)(object)val21, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform((IOnXform)(object)val22);
				OnNurbsCurve val24 = val.Duplicate();
				val24.Transform((IOnXform)(object)val23);
				OnCurve[] array2 = (OnCurve[])(object)new OnCurve[2]
				{
					(OnCurve)prongTopProfile,
					(OnCurve)val24
				};
				double[] inputParameters2 = new double[2]
				{
					((OnCurve)prongRail).Domain().Min(),
					((OnCurve)prongRail).Domain().Max()
				};
				if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
				{
					return UpdateError("Unable to create CAM prong extension.");
				}
				OnNurbsCurve inputRailCurve2 = prongRail;
				profiles = (IOnCurve[])(object)array2;
				if (!CSUtilSweep.Sweep1((IOnCurve)(object)inputRailCurve2, profiles, inputParameters2, out var breps2, closed: false))
				{
					return UpdateError("HeadProngs Sweep Failed");
				}
				OnNurbsSurface val25 = val7.Duplicate();
				val10 = ((OnCurve)prongRail).PointAt(((OnCurve)prongRail).Domain().Min());
				val10.z = 0.0;
				On3dVector val26 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
				val26.z = 0.0;
				On3dVector val27 = new On3dVector((IOn3dPoint)(object)val10);
				double num6 = val26.Length();
				if (val27.Length() < (val27 + (IOn3dVector)(object)val26).Length())
				{
					num6 = 0.0 - num6;
				}
				for (int l = 0; l < val25.CVCount(0); l++)
				{
					for (int m = 0; m < val25.CVCount(1); m++)
					{
						On4dPoint val28 = On4dPoint.op_Implicit((IOn3dPoint)new On3dPoint());
						val25.GetCV(l, m, ref val28);
						val28.y += num6 * val28.z;
						val25.SetCV(l, m, (IOn4dPoint)(object)val28);
					}
				}
				val25.Transform((IOnXform)(object)val22);
				List<OnBrep> list2 = new List<OnBrep>();
				if (breps2.Length != 0)
				{
					list2.Add(breps2[0]);
				}
				list2.Add(((OnGeometry)val25).BrepForm());
				OnBrep obj2 = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(list2.ToArray(), 0.01), 0.01);
				((OnGeometry)obj2).Transform((IOnXform)(object)xUp.Convert());
				CSData data2 = new CSOnGeometryData(obj2.Convert(), xUp, applyTransform: false);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oObjects.SetData(cSDataArray);
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
