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
[CSBlock("HeadProngs_V3", "{1F38016B-4A69-4aca-A2BF-BA916700E93F}")]
public sealed class CSHeadProngs3 : CSBlock
{
	public CSInputChannel IGems { get; }

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

	public CSInputChannel IObjects { get; }

	public CSOutputChannel OObjects { get; }

	public CSHeadProngs3(string identifier)
		: base(identifier)
	{
		IGems = new CSInputChannel(GetPropertyName(() => IGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		IObjects = new CSInputChannel(GetPropertyName(() => IObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		OObjects = new CSOutputChannel(GetPropertyName(() => OObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Expected O, but got Unknown
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Expected O, but got Unknown
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Expected O, but got Unknown
		CSDataArray flattenArray = IGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems missing");
		}
		int value = 0;
		IStyle.GetInteger(ref value);
		OnNurbsCurve nurbsCurve = IProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("ProngTopProfile missing");
		}
		double value2 = 2.0;
		IProngTopX.GetDouble(ref value2);
		double value3 = 2.0;
		IProngTopY.GetDouble(ref value3);
		double value4 = 0.0;
		IProngTopNudge.GetDouble(ref value4);
		bool value5 = false;
		IEnableBase.GetBoolean(ref value5);
		ICSArray iCSArray = IObjects.Data as ICSArray;
		double value6 = 2.0;
		double value7 = 2.0;
		double value8 = 0.0;
		OnNurbsCurve val;
		if (value5)
		{
			val = IProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("ProngBaseProfile missing");
			}
			IProngBaseX.GetDouble(ref value6);
			IProngBaseY.GetDouble(ref value7);
			IProngBaseNudge.GetDouble(ref value8);
		}
		else
		{
			val = nurbsCurve.Duplicate();
			value6 = value2;
			value7 = value3;
			value8 = value4;
		}
		OnXform val2 = new OnXform();
		OnXform val3 = new OnXform();
		OnBoundingBox val4 = new OnBoundingBox();
		((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val4);
		val2.Scale(value2 / (val4.Max().x - val4.Min().x), value3 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value4, 0.0);
		OnNurbsCurve val5 = nurbsCurve.Duplicate();
		nurbsCurve.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnXform val6 = new OnXform();
		val6.Translation(0.0, (0.0 - value3) / 2.0, 0.0);
		val5.Transform((IOnXform)(object)(val6 * (IOnXform)(object)val2));
		((OnGeometry)val).GetTightBoundingBox(ref val4);
		val2.Scale(value6 / (val4.Max().x - val4.Min().x), value7 / (val4.Max().y - val4.Min().y), 0.0);
		val3.Translation(0.0, value8, 0.0);
		val.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		new OnNurbsCurve();
		OnNurbsCurve profile = CSUtilGeometry.CreateDome(0.5);
		OnNurbsSurface val7 = new OnNurbsSurface();
		val7 = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)val5, 1);
		val6.Translation(0.0, value3 / 2.0 + value4, 0.0);
		val7.Transform((IOnXform)(object)val6);
		On3dPoint source = new On3dPoint(0.0, 0.0, 0.0);
		On3dVector source2 = new On3dVector(1.0, 0.0, 0.0);
		On3dVector val8 = new On3dVector(0.0, 0.0, 1.0);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateError("GemData invalid");
			}
			OnNurbsCurve val9 = IHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val9 == null)
			{
				return UpdateError("HeadTop curve invalid");
			}
			OnNurbsCurve val10 = IHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val10 == null)
			{
				return UpdateError("HeadBase curve invalid");
			}
			if (!(IProngLines.GetArrayItem(i) is ICSArray iCSArray2))
			{
				return UpdateError("ProngLines invalid");
			}
			cSGemData.GetGemXformRC(out var _, out var xUp);
			_ = cSGemData.GemXSize;
			_ = cSGemData.GemYSize;
			_ = cSGemData.GemXMax;
			_ = cSGemData.GemYMax;
			CSHeadStyles1.GetHeadData1(cSGemData.GemShape, value, out var position, out var _, out var _);
			if (position == null)
			{
				return UpdateError("HeadStyle invalid");
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
			for (int k = 0; k < position.Length; k++)
			{
				OnNurbsCurve prongRail = iCSArray2.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.Print("Invalid Head Crv\n");
					continue;
				}
				On3dPoint val11 = ((OnCurve)prongRail).PointAtStart();
				On3dPoint val12 = ((OnCurve)prongRail).PointAtEnd();
				double num = 0.0;
				((OnCurve)val9).GetClosestPoint((IOn3dPoint)(object)val11, ref num);
				double num2 = 0.0;
				((OnCurve)val10).GetClosestPoint((IOn3dPoint)(object)val12, ref num2);
				OnInterval val13 = ((OnCurve)val9).Domain();
				double num3 = (val13.Max() - val13.Min()) / 100.0;
				double num4 = num - num3;
				double num5 = num + num3;
				if (num4 < val13.Min())
				{
					num4 = val13.Max() + (num4 - val13.Min());
				}
				if (num5 > val13.Max())
				{
					num5 = val13.Min() + (num5 - val13.Max());
				}
				On3dVector val14 = ((OnCurve)val9).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)val9).TangentAt(num5);
				val14.Unitize();
				val13 = ((OnCurve)val10).Domain();
				num3 = (val13.Max() - val13.Min()) / 100.0;
				num4 = num2 - num3;
				num5 = num2 + num3;
				if (num4 < val13.Min())
				{
					num4 = val13.Max() + (num4 - val13.Min());
				}
				if (num5 > val13.Max())
				{
					num5 = val13.Min() + (num5 - val13.Max());
				}
				On3dVector val15 = ((OnCurve)val10).TangentAt(num4) + (IOn3dVector)(object)((OnCurve)val10).TangentAt(num5);
				val15.Unitize();
				OnXform val16 = new OnXform();
				OnXform val17 = new OnXform();
				val16 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)source, (IOn3dVector)(object)source2, (IOn3dVector)(object)val8, (IOn3dPoint)(object)val11, (IOn3dVector)(object)val14, (IOn3dVector)(object)val8);
				val17 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)(object)source, (IOn3dVector)(object)source2, (IOn3dVector)(object)val8, (IOn3dPoint)(object)val12, (IOn3dVector)(object)val15, (IOn3dVector)(object)val8);
				OnNurbsCurve val18 = new OnNurbsCurve();
				OnNurbsCurve val19 = new OnNurbsCurve();
				val18 = nurbsCurve.Duplicate();
				val18.Transform((IOnXform)(object)val16);
				val19 = val.Duplicate();
				val19.Transform((IOnXform)(object)val17);
				OnCurve[] array = (OnCurve[])(object)new OnCurve[2]
				{
					(OnCurve)val18,
					(OnCurve)val19
				};
				double[] inputParameters = new double[2]
				{
					((OnCurve)prongRail).Domain().Min(),
					((OnCurve)prongRail).Domain().Max()
				};
				OnBrep[] breps = null;
				if (CSApplicationHelper.Application.IsCamMode && !CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref val18))
				{
					return UpdateError("Unable to create CAM prong extension.");
				}
				OnNurbsCurve inputRailCurve = prongRail;
				IOnCurve[] profiles = (IOnCurve[])(object)array;
				if (!CSUtilSweep.Sweep1((IOnCurve)(object)inputRailCurve, profiles, inputParameters, out breps, closed: false))
				{
					return UpdateError("HeadProng Sweep failed");
				}
				OnNurbsSurface val20 = new OnNurbsSurface();
				val20 = val7.Duplicate();
				On3dVector val21 = new On3dVector();
				val11 = ((OnCurve)prongRail).PointAt(((OnCurve)prongRail).Domain().Min());
				val21 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
				val11.z = 0.0;
				val21.z = 0.0;
				On3dVector val22 = new On3dVector((IOn3dPoint)(object)val11);
				double num6 = val21.Length();
				if (val22.Length() < (val22 + (IOn3dVector)(object)val21).Length())
				{
					num6 = 0.0 - num6;
				}
				for (int l = 0; l < val20.CVCount(0); l++)
				{
					for (int m = 0; m < val20.CVCount(1); m++)
					{
						On4dPoint val23 = On4dPoint.op_Implicit((IOn3dPoint)new On3dPoint());
						val20.GetCV(l, m, ref val23);
						val23.y += num6 * val23.z;
						val20.SetCV(l, m, (IOn4dPoint)(object)val23);
					}
				}
				val20.Transform((IOnXform)(object)val16);
				List<OnBrep> list = new List<OnBrep>();
				if (breps.Length != 0)
				{
					list.Add(breps[0]);
				}
				list.Add(((OnGeometry)val20).BrepForm());
				OnBrep val24 = CSUtilRhino.CapPlanarHoles(CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01), 0.01);
				if (val24 != null)
				{
					((OnGeometry)val24).Transform((IOnXform)(object)xUp.Convert());
					CSData data = new CSOnGeometryData(val24.Convert(), xUp, applyTransform: false);
					cSDataArray2.Append(data);
				}
			}
			cSDataArray.Append(cSDataArray2);
		}
		OObjects.SetData(cSDataArray);
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
