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
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadProngs_V9", "{357E3271-1C27-4464-8415-99BAD70743C0}")]
public sealed class CSHeadProngs9 : CSBlock
{
	public CSInputChannel InBaseToPoint { get; }

	public CSInputChannel InEnableBase { get; }

	public CSInputChannel InEnableModeDoItYourself { get; }

	public CSInputChannel InEnableVProngs { get; }

	public CSInputChannel InGems { get; }

	public CSInputChannel InHeadBase { get; }

	public CSInputChannel InHeadTop { get; }

	public CSInputChannel InObjects { get; }

	public CSInputChannel InProngBaseDomeHeight { get; }

	public CSInputChannel InProngBaseNudge { get; }

	public CSInputChannel InProngBaseProfile { get; }

	public CSInputChannel InProngBaseX { get; }

	public CSInputChannel InProngBaseY { get; }

	public CSInputChannel InProngDomeHeight { get; }

	public CSInputChannel InProngDomeSettingAngle { get; }

	public CSInputChannel InProngDomeSettingDistance { get; }

	public CSInputChannel InProngRails { get; }

	public CSInputChannel InProngTopNudge { get; }

	public CSInputChannel InProngTopProfile { get; }

	public CSInputChannel InProngTopX { get; }

	public CSInputChannel InProngTopY { get; }

	public CSInputChannel InProngVBaseAngle { get; }

	public CSInputChannel InProngVTopAngle { get; }

	public CSInputChannel InStyle { get; }

	public CSOutputChannel OutCurves { get; }

	public CSOutputChannel OutObjects { get; }

	public CSHeadProngs9(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InProngRails = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InHeadTop = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeadBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InEnableVProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProngTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngTopY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngTopNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngVTopAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEnableBase = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProngBaseProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngBaseX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngBaseY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngBaseNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngVBaseAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InProngDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngDomeSettingDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProngDomeSettingAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBaseToPoint = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableModeDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProngBaseDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Expected O, but got Unknown
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Expected O, but got Unknown
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Expected O, but got Unknown
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Expected O, but got Unknown
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Expected O, but got Unknown
		CSDataArray flattenArray = InGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		OnNurbsCurve nurbsCurve = InProngTopProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		ICSArray iCSArray = InObjects.Data as ICSArray;
		double value = 2.0;
		InProngTopX.GetDouble(ref value);
		double value2 = 2.0;
		InProngTopY.GetDouble(ref value2);
		double value3 = 0.0;
		InProngTopNudge.GetDouble(ref value3);
		double value4 = 0.0;
		InProngVTopAngle.GetDouble(ref value4);
		double num = 0.0 - value2 * value3;
		ValueListItem whichItem = ValueListItem.HeadStyleCenter1;
		InStyle.GetValueListItem(ref whichItem);
		bool value5 = false;
		InEnableVProngs.GetBoolean(ref value5);
		bool value6 = false;
		InEnableBase.GetBoolean(ref value6);
		bool value7 = false;
		InEnableModeDoItYourself.GetBoolean(ref value7);
		double value8 = 2.0;
		double value9 = 2.0;
		double value10 = 0.0;
		double value11 = 0.0;
		OnNurbsCurve val;
		double num2;
		if (value6)
		{
			val = InProngBaseProfile.GetNurbsCurve();
			if (val == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			InProngBaseX.GetDouble(ref value8);
			InProngBaseY.GetDouble(ref value9);
			InProngBaseNudge.GetDouble(ref value10);
			InProngVBaseAngle.GetDouble(ref value11);
			num2 = 0.0 - value9 * value10;
		}
		else
		{
			val = nurbsCurve.Duplicate();
			value8 = value;
			value9 = value2;
			value10 = value3;
			value11 = value4;
			num2 = num;
		}
		double value12 = 0.5;
		InProngDomeHeight.GetDouble(ref value12);
		double value13 = 0.0;
		InProngDomeSettingDistance.GetDouble(ref value13);
		double value14 = 0.0;
		InProngDomeSettingAngle.GetDouble(ref value14);
		double value15 = 0.0;
		InProngBaseDomeHeight.GetDouble(ref value15);
		bool value16 = false;
		InBaseToPoint.GetBoolean(ref value16);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataArray outputGeometries = new CSDataArray();
			CSDataArray outputGeometries2 = new CSDataArray();
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				return UpdateMissing("GemData Input Missing");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			OnNurbsCurve val2 = InHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val2 == null)
			{
				return UpdateMissing("HeadTop Input Missing");
			}
			outputGeometries.Append(new CSOnGeometryData(val2.Convert(), xUp));
			OnNurbsCurve val3 = InHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val3 == null)
			{
				return UpdateMissing("HeadBase Input Missing");
			}
			outputGeometries.Append(new CSOnGeometryData(val3.Convert(), xUp));
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2))
				{
					continue;
				}
				for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
				{
					outputGeometries2.Append(iCSArray2.GetArrayItem(j));
				}
			}
			if (!(InProngRails.GetArrayItem(i) is ICSArray iCSArray3))
			{
				return UpdateMissing("ProngRails Input Missing");
			}
			List<CSHeadStyles5.HeadProngInfo> headStyle = CSHeadStyles5.GetHeadStyle(cSGemData.GemShape, whichItem);
			if (headStyle == null)
			{
				return UpdateError("Head Styles Invalid");
			}
			for (int k = 0; k < headStyle.Count; k++)
			{
				CSHeadStyles5.HeadProngInfo headProngInfo = headStyle[k];
				OnNurbsCurve prongRail = iCSArray3.GetArrayItem(k).GetNurbsCurve().Convert();
				if (prongRail == null)
				{
					CS.CSUtil.Print("Missing Prong Rail\n");
					continue;
				}
				double param = 0.0;
				((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)((OnCurve)prongRail).PointAtStart(), ref param);
				double param2 = 0.0;
				((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)((OnCurve)prongRail).PointAtEnd(), ref param2);
				if (headProngInfo.Type == CSHeadStyles5.EHeadProngType.Straight && value5)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val2, param, value4, value2, num, out var vProng))
					{
						return UpdateError("Error Making Straight Prong Profile Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(val3, param2, value11, value9, num2, out var vProng2))
					{
						return UpdateError("Error Making Straight Prong Profile Base");
					}
					On3dVector val4 = ((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min());
					val4.Unitize();
					On3dPoint val5 = ((OnCurve)prongRail).PointAtStart();
					On3dPoint val6 = val5 - (IOn3dVector)(object)(val4 * value12);
					OnXform val7 = new OnXform();
					val7.Translation((IOn3dVector)(object)(val6 - (IOn3dPoint)(object)val5));
					OnNurbsCurve val8 = new OnNurbsCurve();
					vProng.GetNurbForm(ref val8);
					val8.Transform((IOnXform)(object)val7);
					OnNurbsCurve val9 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref val9);
					OnLineCurve val10 = new OnLineCurve((IOn3dPoint)(object)val5, (IOn3dPoint)(object)val6);
					if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
					{
						(IOnCurve)prongRail,
						(IOnCurve)val10
					}, out OnCurve mergedCrv, 0.01))
					{
						return UpdateError("Merge curves failed");
					}
					if ((mergedCrv.PointAtStart() - (IOn3dPoint)(object)val6).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					outputGeometries.Append(new CSOnGeometryData(prongRail.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(mergedCrv.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(val8.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(val9.Convert(), xUp));
					if (!value7)
					{
						IOnCurve[] profiles = (IOnCurve[])(object)new IOnCurve[2]
						{
							(IOnCurve)val8,
							(IOnCurve)val9
						};
						double[] inputParameters = new double[2]
						{
							mergedCrv.Domain().Min(),
							mergedCrv.Domain().Max()
						};
						if (!CSUtilSweep.Sweep1((IOnCurve)(object)mergedCrv, profiles, inputParameters, out var breps, closed: false))
						{
							return UpdateError("Failed Sweep1");
						}
						if (breps == null || breps.Length == 0)
						{
							return UpdateError("Failed Sweep1: No results");
						}
						OnBrep brep = CSUtilRhino.CapPlanarHoles(breps[0], 0.01);
						if (brep == null)
						{
							return UpdateError("Failed CapPlanarHoles");
						}
						CSUtilGeometry.FixBrepSolidOrientation(ref brep);
						((OnGeometry)brep).Transform((IOnXform)(object)xUp.Convert());
						outputGeometries2.Append(new CSOnGeometryData(brep.Convert(), xUp, applyTransform: false));
					}
					continue;
				}
				On3dPoint destination = ((OnCurve)prongRail).PointAtStart();
				On3dPoint destination2 = ((OnCurve)prongRail).PointAtEnd();
				On3dVector val11 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)val2, param);
				val11.Unitize();
				On3dVector val12 = CSBlockUtil.CSUtilMath.AverageNormalAt(OnUtil.On_zaxis, (IOn3dVector)(object)val11);
				val12.Unitize();
				OnBoundingBox val13 = new OnBoundingBox();
				if (!((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val13))
				{
					return UpdateError("Failed to get bounding box for top profile");
				}
				double num3 = val13.Max().x - val13.Min().x;
				double num4 = val13.Max().y - val13.Min().y;
				OnXform val14 = new OnXform();
				val14.Scale(value / num3, value2 / num4, 0.0);
				OnXform val15 = new OnXform();
				val15.Translation(0.0, num, 0.0);
				OnXform val16 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination, (IOn3dVector)(object)val11, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform((IOnXform)(object)(val16 * (IOnXform)(object)val15 * (IOnXform)(object)val14));
				On3dVector val17;
				if (value16)
				{
					val17 = val11;
				}
				else
				{
					val17 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)val3, param2);
					val17.Unitize();
				}
				OnBoundingBox val18 = new OnBoundingBox();
				((OnGeometry)val).GetTightBoundingBox(ref val18);
				double num5 = val18.Max().x - val18.Min().x;
				double num6 = val18.Max().y - val18.Min().y;
				double num7 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Min()), (IOn3dVector)(object)val11);
				double value17 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Max()), (IOn3dVector)(object)val17);
				double num8 = 1.0 / Math.Cos(Math.Abs(value17) - Math.Abs(num7));
				if (num8 > 2.25)
				{
					num8 = 2.25;
				}
				OnXform val19 = new OnXform();
				val19.Scale(value8 / num5, value9 / num6 * num8, 0.0);
				OnXform val20 = new OnXform();
				val20.Translation(0.0, num2, 0.0);
				OnXform val21 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)(value16 ? val11 : val17), OnUtil.On_zaxis);
				OnNurbsCurve prongBaseProfile = val.Duplicate();
				prongBaseProfile.Transform((IOnXform)(object)(val21 * (IOnXform)(object)val20 * (IOnXform)(object)val19));
				OnBrep dome = null;
				OnBrep dome2 = null;
				if (CSApplicationHelper.Application.IsCamMode && !value7)
				{
					if (!CreateProngExtension(cSGemData.GemZMax, ref prongRail, ref prongTopProfile))
					{
						return UpdateError("Unable to create CAM prong extension.");
					}
				}
				else
				{
					if (!CreateDomeTop(nurbsCurve, value, value2, num7, value12, value13, value14, ref dome))
					{
						return UpdateError("Unable to create top dome");
					}
					((OnGeometry)dome).Transform((IOnXform)(object)(val16 * (IOnXform)(object)val15));
					if (!value16 && value15 > 0.25)
					{
						if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, value15))
						{
							UpdateError("ReduceProngRailLength failed");
						}
						((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)((OnCurve)prongRail).PointAtEnd(), ref param2);
						val17 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)val3, param2);
						val17.Unitize();
						value17 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection((IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)(object)((OnCurve)prongRail).TangentAt(((OnCurve)prongRail).Domain().Max()), (IOn3dVector)(object)val17);
						if (!CreateDomeBase(val, value8, value9, value15, value17, num8, ref dome2))
						{
							return UpdateError("Unable to create base dome");
						}
						destination2 = new On3dPoint((IOn3dPoint)(object)((OnCurve)prongRail).PointAtEnd());
						OnXform val22 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val17, OnUtil.On_zaxis);
						((OnGeometry)dome2).Transform((IOnXform)(object)(val22 * (IOnXform)(object)val20));
					}
				}
				IOnGeometry[] geometries = (IOnGeometry[])(object)new IOnGeometry[3]
				{
					(IOnGeometry)prongRail,
					(IOnGeometry)prongTopProfile,
					(IOnGeometry)prongBaseProfile
				};
				if (!ArrangeGeometry(headProngInfo, xUp, val11, val12, num3, geometries, ref outputGeometries))
				{
					return UpdateError("Invalid head style");
				}
				if (value7)
				{
					if (!ArrangeGeometry(geometries: (IOnGeometry[])(object)((value16 || !(value15 > 0.25)) ? new IOnGeometry[1] { (IOnGeometry)dome } : new IOnGeometry[2]
					{
						(IOnGeometry)dome,
						(IOnGeometry)dome2
					}), headProngInfo: headProngInfo, xUp: xUp, headTopTan: val11, headTopNrm: val12, prongProfileTopX: num3, outputGeometries: ref outputGeometries2))
					{
						return UpdateError("Invalid head style");
					}
					continue;
				}
				IOnCurve[] profiles2 = (IOnCurve[])(object)new IOnCurve[2]
				{
					(IOnCurve)prongTopProfile,
					(IOnCurve)prongBaseProfile
				};
				double[] inputParameters2 = new double[2]
				{
					((OnCurve)prongRail).Domain().Min(),
					((OnCurve)prongRail).Domain().Max()
				};
				if (!CSUtilSweep.Sweep1((IOnCurve)(object)prongRail, profiles2, inputParameters2, out var breps2, closed: false))
				{
					return UpdateError("Failed Sweep1");
				}
				if (breps2 == null || breps2.Length == 0)
				{
					return UpdateError("Failed Sweep1: No results");
				}
				List<OnBrep> list = new List<OnBrep>();
				if (!CSApplicationHelper.Application.IsCamMode)
				{
					list.Add(dome);
					if (!value16 && value15 > 0.25)
					{
						list.Add(dome2);
					}
				}
				list.AddRange(breps2);
				OnBrep val23 = CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01);
				if (val23 == null)
				{
					return UpdateError("Failed JoinBreps");
				}
				OnBrep val24;
				if (value16 || value15 <= 0.25 || CSApplicationHelper.Application.IsCamMode)
				{
					val24 = CSUtilRhino.CapPlanarHoles(val23, 0.01);
					if (val24 == null)
					{
						return UpdateError("Failed CapPlanarHoles");
					}
				}
				else
				{
					val24 = val23;
				}
				if (!ArrangeGeometry(headProngInfo, xUp, val11, val12, num3, (IOnGeometry)(object)val24, ref outputGeometries2))
				{
					return UpdateError("Invalid head style");
				}
			}
			cSDataArray.Append(outputGeometries);
			cSDataArray2.Append(outputGeometries2);
		}
		OutObjects.SetData(cSDataArray2);
		OutCurves.SetData(cSDataArray);
		return base.Update();
	}

	private static bool ArrangeGeometry(CSHeadStyles5.HeadProngInfo headProngInfo, Transform xUp, On3dVector headTopTan, On3dVector headTopNrm, double prongProfileTopX, IOnGeometry geometry, ref CSDataArray outputGeometries)
	{
		IOnGeometry[] geometries = (IOnGeometry[])(object)new IOnGeometry[1] { geometry };
		return ArrangeGeometry(headProngInfo, xUp, headTopTan, headTopNrm, prongProfileTopX, geometries, ref outputGeometries);
	}

	private static bool ArrangeGeometry(CSHeadStyles5.HeadProngInfo headProngInfo, Transform xUp, On3dVector headTopTan, On3dVector headTopNrm, double prongProfileTopX, IOnGeometry[] geometries, ref CSDataArray outputGeometries)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		switch (headProngInfo.Arrangment)
		{
		case CSHeadStyles5.EHeadProngArrangment.Single:
		{
			IOnGeometry[] array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj4 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj4).Convert(), xUp));
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Double:
		{
			double num3 = headProngInfo.Overlap * prongProfileTopX;
			Transform transform4 = Transform.Translation(headTopTan.Convert() * 1.0 * num3);
			IOnGeometry[] array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj5 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj5).Convert(), xUp * transform4));
			}
			Transform transform5 = Transform.Translation(headTopTan.Convert() * -1.0 * num3);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj6 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj6).Convert(), xUp * transform5));
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Triple:
		{
			double num = headProngInfo.Overlap * prongProfileTopX;
			Transform transform = Transform.Translation(headTopTan.Convert() * 1.0 * num);
			IOnGeometry[] array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj).Convert(), xUp * transform));
			}
			Transform transform2 = Transform.Translation(headTopTan.Convert() * -1.0 * num);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj2 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj2).Convert(), xUp * transform2));
			}
			double num2 = headProngInfo.Overlap * prongProfileTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform transform3 = Transform.Translation(headTopNrm.Convert() * num2);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj3 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(((OnObject)(object)obj3).Convert(), xUp * transform3));
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static bool CreateDomeBase(OnNurbsCurve prongBaseProfile, double prongBaseX, double prongBaseY, double prongDomeHeight, double angleBase, double fixScale, ref OnBrep dome)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_033c: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)prongBaseProfile).GetTightBoundingBox(ref val);
		double num = val.Max().x - val.Min().x;
		double num2 = val.Max().y - val.Min().y;
		OnXform val2 = new OnXform();
		val2.Scale(prongBaseX / num, prongBaseY / num2 * fixScale, 0.0);
		OnXform val3 = new OnXform();
		val3.Translation(0.0, (0.0 - prongBaseY) / 2.0, 0.0);
		OnNurbsCurve val4 = prongBaseProfile.Duplicate();
		val4.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		OnLineCurve val5 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, -50.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 50.0, 0.0));
		ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
		((OnCurve)val4).IntersectCurve((IOnCurve)(object)val5, ref val6);
		if (val6.Count() <= 0)
		{
			return false;
		}
		double num3 = val6[0][0];
		double num4 = val6[1][0];
		((OnCurve)val4).ChangeClosedCurveSeam(num3);
		OnCurve val7 = null;
		OnCurve val8 = null;
		if (!((OnCurve)val4).Split(num4, ref val7, ref val8))
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
		val10.Rotate(angleBase, OnUtil.On_xaxis);
		val10.Unitize();
		On3dPoint val11 = val.Center();
		val11.Transform((IOnXform)(object)(val3 * (IOnXform)(object)val2));
		val11 += (IOn3dVector)(object)(val10 * (prongDomeHeight / Math.Cos(angleBase)));
		On3dVector val12 = new On3dVector(OnUtil.On_yaxis) * -1.0;
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
		double num5 = val17[0][0];
		OnCurve val18 = null;
		OnCurve val19 = null;
		if (!val7.Split(num5, ref val18, ref val19))
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
		double num6 = val20[0][0];
		OnCurve val21 = null;
		OnCurve val22 = null;
		if (!val8.Split(num6, ref val21, ref val22))
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
		val24.Rotation(CSBlockUtil.CSUtilMath.ToRadians(180.0), OnUtil.On_yaxis, (IOn3dPoint)(object)val.Center());
		((OnGeometry)val23).Transform((IOnXform)(object)val24);
		OnXform val25 = new OnXform();
		val25.Translation(0.0, prongBaseY / 2.0, 0.0);
		((OnGeometry)val23).Transform((IOnXform)(object)val25);
		dome = val23;
		return true;
	}

	private static bool CreateDomeTop(OnNurbsCurve prongTopProfile, double prongTopX, double prongTopY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, ref OnBrep dome)
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

	private bool ReduceProngRailLength(ref OnNurbsCurve prongRail, ref OnNurbsCurve prongBaseProfile, double trimLength)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		OnCurve val = (OnCurve)new OnNurbsCurve();
		OnCurve val2 = (OnCurve)new OnNurbsCurve();
		if (!((OnCurve)prongRail).Split(((OnCurve)prongRail).Domain().Max() - trimLength, ref val, ref val2))
		{
			return false;
		}
		On3dVector val3 = new On3dVector((IOn3dVector)(object)(val2.PointAtStart() - (IOn3dPoint)(object)val2.PointAtEnd()));
		((OnGeometry)prongBaseProfile).Translate((IOn3dVector)(object)val3);
		prongRail = val.NurbsCurve().Duplicate();
		return true;
	}
}
