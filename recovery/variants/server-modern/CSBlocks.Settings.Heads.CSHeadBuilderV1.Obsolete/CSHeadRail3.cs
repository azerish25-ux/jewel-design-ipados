using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadRail_V3", "{09ED7B62-82EF-4740-BF63-137AAC03DA64}")]
public sealed class CSHeadRail3 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iHeadSideX { get; }

	public CSInputChannel iHeadSideY { get; }

	public CSInputChannel iEnableRail { get; }

	public CSInputChannel iRailZPos { get; }

	public CSInputChannel iRailProfile { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailY { get; }

	public CSInputChannel iEnableSecondControl { get; }

	public CSInputChannel iRailProfile2 { get; }

	public CSInputChannel iRailX2 { get; }

	public CSInputChannel iRailY2 { get; }

	public CSInputChannel iObjects { get; }

	public CSInputChannel iRailOffset { get; }

	public CSInputChannel iRailOffset2 { get; }

	public CSInputChannel iEnableModeDoItYourself { get; }

	public CSInputChannel iShearMultiplier { get; }

	public CSOutputChannel oObjects { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadRail3(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iHeadSideX = new CSInputChannel(GetPropertyName(() => iHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iHeadSideY = new CSInputChannel(GetPropertyName(() => iHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEnableRail = new CSInputChannel(GetPropertyName(() => iEnableRail), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iRailZPos = new CSInputChannel(GetPropertyName(() => iRailZPos), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailProfile = new CSInputChannel(GetPropertyName(() => iRailProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(GetPropertyName(() => iRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailY = new CSInputChannel(GetPropertyName(() => iRailY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableSecondControl = new CSInputChannel(GetPropertyName(() => iEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iRailProfile2 = new CSInputChannel(GetPropertyName(() => iRailProfile2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailX2 = new CSInputChannel(GetPropertyName(() => iRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailY2 = new CSInputChannel(GetPropertyName(() => iRailY2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iRailOffset = new CSInputChannel(GetPropertyName(() => iRailOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailOffset2 = new CSInputChannel(GetPropertyName(() => iRailOffset2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableModeDoItYourself = new CSInputChannel(GetPropertyName(() => iEnableModeDoItYourself), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iShearMultiplier = new CSInputChannel(GetPropertyName(() => iShearMultiplier), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_069b: Expected O, but got Unknown
		//IL_069b: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		double value = 0.0;
		iRailZPos.GetDouble(ref value);
		bool value2 = false;
		iEnableRail.GetBoolean(ref value2);
		bool value3 = false;
		iEnableSecondControl.GetBoolean(ref value3);
		bool value4 = false;
		iEnableModeDoItYourself.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		double value7 = 0.0;
		OnNurbsCurve nurbsCurve;
		if (!value3)
		{
			nurbsCurve = iRailProfile.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Rail Profile");
			}
			iRailX.GetDouble(ref value5);
			iRailY.GetDouble(ref value6);
			iRailOffset.GetDouble(ref value7);
		}
		else
		{
			nurbsCurve = iRailProfile2.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Rail Profile 2");
			}
			iRailX2.GetDouble(ref value5);
			iRailY2.GetDouble(ref value6);
			iRailOffset2.GetDouble(ref value7);
		}
		double value8 = 1.0;
		iShearMultiplier.GetDouble(ref value8);
		ICSArray iCSArray = iObjects.Data as ICSArray;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataArray cSDataArray3 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2))
				{
					continue;
				}
				for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
				{
					cSDataArray3.Append(iCSArray2.GetArrayItem(j));
				}
			}
			if (value2)
			{
				if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
				{
					return UpdateMissing("Invalid Gem");
				}
				CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
				OnNurbsCurve val = iHeadSideX.GetArrayItem(i).GetNurbsCurve().Convert();
				if (val == null)
				{
					return UpdateMissing("Invalid X side profile");
				}
				OnNurbsCurve val2 = iHeadSideY.GetArrayItem(i).GetNurbsCurve().Convert();
				if (val2 == null)
				{
					return UpdateMissing("Invalid Y side profile");
				}
				double num = ((OnCurve)val).PointAtEnd().z * (1.0 - value);
				OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(100.0, 0.0, num));
				ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
				((OnCurve)val3).IntersectCurve((IOnCurve)(object)val, ref val4);
				if (val4.Count() <= 0)
				{
					return UpdateError("Unable to intersect X side");
				}
				OnLineCurve val5 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(0.0, 100.0, num));
				ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
				((OnCurve)val5).IntersectCurve((IOnCurve)(object)val2, ref val6);
				if (val6.Count() <= 0)
				{
					return UpdateError("Unable to intersect Y side");
				}
				double x = val4[0][0].x;
				double y = val6[0][0].y;
				double num2 = val4[0][0];
				double num3 = (0.0 - ((OnCurve)val).TangentAt(num2).x) * value8;
				OnXform val7 = new OnXform();
				val7.Scale((IOn3dVector)new On3dVector(x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(val7.Convert() * xDown.Convert());
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value7, out OnCurve curve, false, out string error, 0.0))
				{
					return UpdateError("Gem Offset:  " + error);
				}
				if (curve == null)
				{
					return UpdateError("Gem Offset Outside");
				}
				if (!GemOffsetCurveV2.GemOffset(cSGemData2, value7 + (0.0 - value5), out OnCurve curve2, false, out string error2, 0.0))
				{
					return UpdateError("Gem Offset Inside:  " + error2);
				}
				if (curve2 == null)
				{
					return UpdateError("Gem Offset Inside");
				}
				On3dPoint val8 = ((OnCurve)cSGemData2.GetNurbsCurve().Convert()).PointAt(cSGemData.BezelParam);
				double num4 = 0.0;
				curve.GetClosestPoint((IOn3dPoint)(object)val8, ref num4);
				On3dPoint val9 = curve.PointAt(num4);
				On3dVector val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)curve.TangentAt(num4), OnUtil.On_zaxis);
				val10.Unitize();
				val10 *= value5;
				double num5 = 0.0;
				double num6 = value7 + (0.0 - value5);
				if (num6 < 0.0 && (Math.Abs(num6) > cSGemData2.GemXSize / 2.0 || Math.Abs(num6) > cSGemData2.GemYSize / 2.0))
				{
					curve2.GetClosestPoint((IOn3dPoint)(object)(val9 - (IOn3dVector)(object)val10), ref num5);
				}
				else
				{
					curve2.GetClosestPoint((IOn3dPoint)(object)(val9 + (IOn3dVector)(object)val10), ref num5);
				}
				OnNurbsCurve val11 = nurbsCurve.Duplicate();
				OnBoundingBox val12 = new OnBoundingBox();
				((OnGeometry)val11).GetTightBoundingBox(ref val12);
				OnXform val13 = new OnXform();
				val13.Translation(0.0 - val12.m_min.x, 0.0 - val12.m_min.y, 0.0 - val12.m_min.z);
				OnXform val14 = new OnXform();
				val14.Scale(value5 / (val12.m_max.x - val12.m_min.x), value6 / (val12.m_max.y - val12.m_min.y), 0.0);
				val11.Transform((IOnXform)(object)(val14 * (IOnXform)(object)val13));
				for (int k = 0; k < val11.m_cv_count; k++)
				{
					On4dPoint val15 = new On4dPoint();
					val11.GetCV(k, ref val15);
					val15.x += num3 * val15.y;
					val11.SetCV(k, (IOn4dPoint)(object)val15);
				}
				double num7 = 0.0;
				curve.GetClosestPoint((IOn3dPoint)(object)val8, ref num7);
				On3dPoint destination = curve.PointAt(num7);
				OnXform val16 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(value5, 0.0, 0.0), (IOn3dVector)new On3dVector(-1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)(object)destination, (IOn3dVector)(object)val10, OnUtil.On_zaxis);
				val11.Transform((IOnXform)(object)val16);
				OnXform val17 = new OnXform();
				val17.Translation(0.0, 0.0, num);
				((OnGeometry)curve).Transform((IOnXform)(object)(xUp * (IOnXform)(object)val17));
				((OnGeometry)curve2).Transform((IOnXform)(object)(xUp * (IOnXform)(object)val17));
				val11.Transform((IOnXform)(object)(xUp * (IOnXform)(object)val17));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve.Convert(), xUp.Convert() * val17.Convert(), applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)curve2.Convert(), xUp.Convert() * val17.Convert(), applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val11.Convert(), xUp.Convert() * val17.Convert(), applyTransform: false));
				if (!value4)
				{
					IOnCurve[] profiles = (IOnCurve[])(object)new IOnCurve[1] { (IOnCurve)val11 };
					double[] parameters1In = new double[1] { num4 };
					double[] parameters2In = new double[1] { num5 };
					if (!CSUtilSweep.Sweep2((IOnCurve)(object)curve, (IOnCurve)(object)curve2, profiles, parameters1In, parameters2In, out var breps, closed: true, maintainHeight: true))
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					if (breps.Length == 0)
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)breps[0].Convert()));
				}
			}
			cSDataArray.Append(cSDataArray3);
		}
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
		return base.Update();
	}
}
