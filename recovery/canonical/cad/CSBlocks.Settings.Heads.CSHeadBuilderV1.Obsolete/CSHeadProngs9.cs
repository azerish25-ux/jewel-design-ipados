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
		OnNurbsCurve onNurbsCurve;
		double num2;
		if (value6)
		{
			onNurbsCurve = InProngBaseProfile.GetNurbsCurve();
			if (onNurbsCurve == null)
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
			onNurbsCurve = nurbsCurve.Duplicate();
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
			OnNurbsCurve onNurbsCurve2 = InHeadTop.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve2 == null)
			{
				return UpdateMissing("HeadTop Input Missing");
			}
			outputGeometries.Append(new CSOnGeometryData(onNurbsCurve2.Convert(), xUp));
			OnNurbsCurve onNurbsCurve3 = InHeadBase.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				return UpdateMissing("HeadBase Input Missing");
			}
			outputGeometries.Append(new CSOnGeometryData(onNurbsCurve3.Convert(), xUp));
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
				double t = 0.0;
				onNurbsCurve2.GetClosestPoint(prongRail.PointAtStart(), ref t);
				double t2 = 0.0;
				onNurbsCurve3.GetClosestPoint(prongRail.PointAtEnd(), ref t2);
				if (headProngInfo.Type == CSHeadStyles5.EHeadProngType.Straight && value5)
				{
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve2, t, value4, value2, num, out var vProng))
					{
						return UpdateError("Error Making Straight Prong Profile Top");
					}
					if (!HelperMakeProngProfileStraightObsolete.MakeProngProfileStraightObsolete(onNurbsCurve3, t2, value11, value9, num2, out var vProng2))
					{
						return UpdateError("Error Making Straight Prong Profile Base");
					}
					On3dVector on3dVector = prongRail.TangentAt(prongRail.Domain().Min());
					on3dVector.Unitize();
					On3dPoint on3dPoint = prongRail.PointAtStart();
					On3dPoint on3dPoint2 = on3dPoint - on3dVector * value12;
					OnXform onXform = new OnXform();
					onXform.Translation(on3dPoint2 - on3dPoint);
					OnNurbsCurve nurbs_curve = new OnNurbsCurve();
					vProng.GetNurbForm(ref nurbs_curve);
					nurbs_curve.Transform(onXform);
					OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
					vProng2.GetNurbForm(ref nurbs_curve2);
					OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
					if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { prongRail, onLineCurve }, out OnCurve mergedCrv, 0.01))
					{
						return UpdateError("Merge curves failed");
					}
					if ((mergedCrv.PointAtStart() - on3dPoint2).Length() > 0.01)
					{
						mergedCrv.Reverse();
					}
					outputGeometries.Append(new CSOnGeometryData(prongRail.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(mergedCrv.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(nurbs_curve.Convert(), xUp));
					outputGeometries.Append(new CSOnGeometryData(nurbs_curve2.Convert(), xUp));
					if (!value7)
					{
						IOnCurve[] profiles = new IOnCurve[2] { nurbs_curve, nurbs_curve2 };
						double[] inputParameters = new double[2]
						{
							mergedCrv.Domain().Min(),
							mergedCrv.Domain().Max()
						};
						if (!CSUtilSweep.Sweep1(mergedCrv, profiles, inputParameters, out var breps, closed: false))
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
						brep.Transform(xUp.Convert());
						outputGeometries2.Append(new CSOnGeometryData(brep.Convert(), xUp, applyTransform: false));
					}
					continue;
				}
				On3dPoint destination = prongRail.PointAtStart();
				On3dPoint destination2 = prongRail.PointAtEnd();
				On3dVector on3dVector2 = CSBlockUtil.CSUtilMath.AverageTangentAt(onNurbsCurve2, t);
				on3dVector2.Unitize();
				On3dVector on3dVector3 = CSBlockUtil.CSUtilMath.AverageNormalAt(OnUtil.On_zaxis, on3dVector2);
				on3dVector3.Unitize();
				OnBoundingBox tight_bbox = new OnBoundingBox();
				if (!nurbsCurve.GetTightBoundingBox(ref tight_bbox))
				{
					return UpdateError("Failed to get bounding box for top profile");
				}
				double num3 = tight_bbox.Max().x - tight_bbox.Min().x;
				double num4 = tight_bbox.Max().y - tight_bbox.Min().y;
				OnXform onXform2 = new OnXform();
				onXform2.Scale(value / num3, value2 / num4, 0.0);
				OnXform onXform3 = new OnXform();
				onXform3.Translation(0.0, num, 0.0);
				OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, destination, on3dVector2, OnUtil.On_zaxis);
				OnNurbsCurve prongTopProfile = nurbsCurve.Duplicate();
				prongTopProfile.Transform(onXform4 * onXform3 * onXform2);
				On3dVector on3dVector4;
				if (value16)
				{
					on3dVector4 = on3dVector2;
				}
				else
				{
					on3dVector4 = CSBlockUtil.CSUtilMath.AverageTangentAt(onNurbsCurve3, t2);
					on3dVector4.Unitize();
				}
				OnBoundingBox tight_bbox2 = new OnBoundingBox();
				onNurbsCurve.GetTightBoundingBox(ref tight_bbox2);
				double num5 = tight_bbox2.Max().x - tight_bbox2.Min().x;
				double num6 = tight_bbox2.Max().y - tight_bbox2.Min().y;
				double num7 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain().Min()), on3dVector2);
				double value17 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain().Max()), on3dVector4);
				double num8 = 1.0 / Math.Cos(Math.Abs(value17) - Math.Abs(num7));
				if (num8 > 2.25)
				{
					num8 = 2.25;
				}
				OnXform onXform5 = new OnXform();
				onXform5.Scale(value8 / num5, value9 / num6 * num8, 0.0);
				OnXform onXform6 = new OnXform();
				onXform6.Translation(0.0, num2, 0.0);
				OnXform onXform7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, destination2, value16 ? on3dVector2 : on3dVector4, OnUtil.On_zaxis);
				OnNurbsCurve prongBaseProfile = onNurbsCurve.Duplicate();
				prongBaseProfile.Transform(onXform7 * onXform6 * onXform5);
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
					dome.Transform(onXform4 * onXform3);
					if (!value16 && value15 > 0.25)
					{
						if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, value15))
						{
							UpdateError("ReduceProngRailLength failed");
						}
						onNurbsCurve3.GetClosestPoint(prongRail.PointAtEnd(), ref t2);
						on3dVector4 = CSBlockUtil.CSUtilMath.AverageTangentAt(onNurbsCurve3, t2);
						on3dVector4.Unitize();
						value17 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new On3dVector(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain().Max()), on3dVector4);
						if (!CreateDomeBase(onNurbsCurve, value8, value9, value15, value17, num8, ref dome2))
						{
							return UpdateError("Unable to create base dome");
						}
						destination2 = new On3dPoint(prongRail.PointAtEnd());
						OnXform onXform8 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, destination2, on3dVector4, OnUtil.On_zaxis);
						dome2.Transform(onXform8 * onXform6);
					}
				}
				IOnGeometry[] geometries = new IOnGeometry[3] { prongRail, prongTopProfile, prongBaseProfile };
				if (!ArrangeGeometry(headProngInfo, xUp, on3dVector2, on3dVector3, num3, geometries, ref outputGeometries))
				{
					return UpdateError("Invalid head style");
				}
				if (value7)
				{
					if (!ArrangeGeometry(geometries: (value16 || !(value15 > 0.25)) ? new IOnGeometry[1] { dome } : new IOnGeometry[2] { dome, dome2 }, headProngInfo: headProngInfo, xUp: xUp, headTopTan: on3dVector2, headTopNrm: on3dVector3, prongProfileTopX: num3, outputGeometries: ref outputGeometries2))
					{
						return UpdateError("Invalid head style");
					}
					continue;
				}
				IOnCurve[] profiles2 = new IOnCurve[2] { prongTopProfile, prongBaseProfile };
				double[] inputParameters2 = new double[2]
				{
					prongRail.Domain().Min(),
					prongRail.Domain().Max()
				};
				if (!CSUtilSweep.Sweep1(prongRail, profiles2, inputParameters2, out var breps2, closed: false))
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
				OnBrep onBrep = CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01);
				if (onBrep == null)
				{
					return UpdateError("Failed JoinBreps");
				}
				OnBrep onBrep2;
				if (value16 || value15 <= 0.25 || CSApplicationHelper.Application.IsCamMode)
				{
					onBrep2 = CSUtilRhino.CapPlanarHoles(onBrep, 0.01);
					if (onBrep2 == null)
					{
						return UpdateError("Failed CapPlanarHoles");
					}
				}
				else
				{
					onBrep2 = onBrep;
				}
				if (!ArrangeGeometry(headProngInfo, xUp, on3dVector2, on3dVector3, num3, onBrep2, ref outputGeometries2))
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
		IOnGeometry[] geometries = new IOnGeometry[1] { geometry };
		return ArrangeGeometry(headProngInfo, xUp, headTopTan, headTopNrm, prongProfileTopX, geometries, ref outputGeometries);
	}

	private static bool ArrangeGeometry(CSHeadStyles5.HeadProngInfo headProngInfo, Transform xUp, On3dVector headTopTan, On3dVector headTopNrm, double prongProfileTopX, IOnGeometry[] geometries, ref CSDataArray outputGeometries)
	{
		switch (headProngInfo.Arrangment)
		{
		case CSHeadStyles5.EHeadProngArrangment.Single:
		{
			IOnGeometry[] array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj4 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(obj4.Convert(), xUp));
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
				outputGeometries.Append(new CSOnGeometryData(obj5.Convert(), xUp * transform4));
			}
			Transform transform5 = Transform.Translation(headTopTan.Convert() * -1.0 * num3);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj6 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(obj6.Convert(), xUp * transform5));
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
				outputGeometries.Append(new CSOnGeometryData(obj.Convert(), xUp * transform));
			}
			Transform transform2 = Transform.Translation(headTopTan.Convert() * -1.0 * num);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj2 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(obj2.Convert(), xUp * transform2));
			}
			double num2 = headProngInfo.Overlap * prongProfileTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform transform3 = Transform.Translation(headTopNrm.Convert() * num2);
			array = geometries;
			for (int i = 0; i < array.Length; i++)
			{
				OnGeometry obj3 = (OnGeometry)array[i];
				outputGeometries.Append(new CSOnGeometryData(obj3.Convert(), xUp * transform3));
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
		OnBoundingBox tight_bbox = new OnBoundingBox();
		prongBaseProfile.GetTightBoundingBox(ref tight_bbox);
		double num = tight_bbox.Max().x - tight_bbox.Min().x;
		double num2 = tight_bbox.Max().y - tight_bbox.Min().y;
		OnXform onXform = new OnXform();
		onXform.Scale(prongBaseX / num, prongBaseY / num2 * fixScale, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(0.0, (0.0 - prongBaseY) / 2.0, 0.0);
		OnNurbsCurve onNurbsCurve = prongBaseProfile.Duplicate();
		onNurbsCurve.Transform(onXform2 * onXform);
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, -50.0, 0.0), new On3dPoint(0.0, 50.0, 0.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectCurve(onLineCurve, ref x);
		if (x.Count() <= 0)
		{
			return false;
		}
		double t = x[0].get_m_a(0);
		double t2 = x[1].get_m_a(0);
		onNurbsCurve.ChangeClosedCurveSeam(t);
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onNurbsCurve.Split(t2, ref left_side, ref right_side))
		{
			return false;
		}
		if (left_side == null || right_side == null)
		{
			return false;
		}
		On3dVector on3dVector = onLineCurve.TangentAt(onLineCurve.Domain().Mid());
		on3dVector.Unitize();
		On3dVector on3dVector2 = left_side.TangentAt(left_side.Domain().Mid());
		on3dVector2.Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, on3dVector2) < 0.0)
		{
			left_side.Reverse();
		}
		right_side.TangentAt(right_side.Domain().Mid()).Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, right_side.TangentAt(right_side.Domain().Mid())) < 0.0)
		{
			right_side.Reverse();
		}
		On3dVector on3dVector3 = new On3dVector(OnUtil.On_zaxis);
		on3dVector3.Rotate(angleBase, OnUtil.On_xaxis);
		on3dVector3.Unitize();
		On3dPoint on3dPoint = tight_bbox.Center();
		on3dPoint.Transform(onXform2 * onXform);
		on3dPoint += (IOn3dVector)(on3dVector3 * (prongDomeHeight / Math.Cos(angleBase)));
		On3dVector on3dVector4 = new On3dVector(OnUtil.On_yaxis) * -1.0;
		on3dVector4.Unitize();
		On3dVector on3dVector5 = new On3dVector(on3dVector4);
		on3dVector5.Reverse();
		OnNurbsCurve onNurbsCurve2 = CSUtilCurves.BlendCurve(left_side.PointAtStart(), on3dPoint, on3dVector3, on3dVector4);
		OnNurbsCurve onNurbsCurve3 = CSUtilCurves.BlendCurve(left_side.PointAtEnd(), on3dPoint, on3dVector3, on3dVector5);
		IOnCurve[] input_curves = new IOnCurve[2] { onNurbsCurve2, onNurbsCurve3 };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output == null)
		{
			return false;
		}
		if (output.Length == 0)
		{
			return false;
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(-50.0, 0.0, 0.0), new On3dPoint(50.0, 0.0, 0.0));
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		left_side.IntersectCurve(curveB, ref x2);
		if (x2.Count() <= 0)
		{
			return false;
		}
		double t3 = x2[0].get_m_a(0);
		OnCurve left_side2 = null;
		OnCurve right_side2 = null;
		if (!left_side.Split(t3, ref left_side2, ref right_side2))
		{
			return false;
		}
		if (left_side2 == null || right_side2 == null)
		{
			return false;
		}
		ArrayOnX_EVENT x3 = new ArrayOnX_EVENT();
		right_side.IntersectCurve(curveB, ref x3);
		if (x3.Count() <= 0)
		{
			return false;
		}
		double t4 = x3[0].get_m_a(0);
		OnCurve left_side3 = null;
		OnCurve right_side3 = null;
		if (!right_side.Split(t4, ref left_side3, ref right_side3))
		{
			return false;
		}
		if (left_side3 == null || right_side3 == null)
		{
			return false;
		}
		OnCurve railCurve = left_side2;
		OnCurve railCurve2 = right_side2;
		IOnCurve[] profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve, railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		OnCurve railCurve3 = left_side3;
		OnCurve railCurve4 = right_side3;
		profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve3, railCurve4, profiles, out var breps2, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		List<OnBrep> list = new List<OnBrep>();
		list.AddRange(breps);
		list.AddRange(breps2);
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (onBrep == null)
		{
			return false;
		}
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(180.0), OnUtil.On_yaxis, tight_bbox.Center());
		onBrep.Transform(onXform3);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, prongBaseY / 2.0, 0.0);
		onBrep.Transform(onXform4);
		dome = onBrep;
		return true;
	}

	private static bool CreateDomeTop(OnNurbsCurve prongTopProfile, double prongTopX, double prongTopY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, ref OnBrep dome)
	{
		OnBoundingBox tight_bbox = new OnBoundingBox();
		prongTopProfile.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Scale(prongTopX / (tight_bbox.Max().x - tight_bbox.Min().x), prongTopY / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(0.0, (0.0 - prongTopY) / 2.0, 0.0);
		OnNurbsCurve onNurbsCurve = prongTopProfile.Duplicate();
		onNurbsCurve.Transform(onXform2 * onXform);
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, -50.0, 0.0), new On3dPoint(0.0, 50.0, 0.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectCurve(onLineCurve, ref x);
		if (x.Count() <= 0)
		{
			return false;
		}
		double t = x[0].get_m_a(0);
		double t2 = x[1].get_m_a(0);
		onNurbsCurve.ChangeClosedCurveSeam(t);
		OnCurve left_side = null;
		OnCurve right_side = null;
		if (!onNurbsCurve.Split(t2, ref left_side, ref right_side))
		{
			return false;
		}
		if (left_side == null || right_side == null)
		{
			return false;
		}
		On3dVector on3dVector = onLineCurve.TangentAt(onLineCurve.Domain().Mid());
		on3dVector.Unitize();
		On3dVector on3dVector2 = left_side.TangentAt(left_side.Domain().Mid());
		on3dVector2.Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, on3dVector2) < 0.0)
		{
			left_side.Reverse();
		}
		right_side.TangentAt(right_side.Domain().Mid()).Unitize();
		if (OnUtil.ON_DotProduct(on3dVector, right_side.TangentAt(right_side.Domain().Mid())) < 0.0)
		{
			right_side.Reverse();
		}
		On3dVector on3dVector3 = new On3dVector(OnUtil.On_zaxis);
		on3dVector3.Rotate(0.0 - angle, OnUtil.On_xaxis);
		on3dVector3.Unitize();
		On3dPoint on3dPoint = tight_bbox.Center();
		on3dPoint.Transform(onXform2 * onXform);
		on3dPoint += (IOn3dVector)(on3dVector3 * (prongDomeHeight / Math.Cos(angle)));
		on3dPoint += (IOn3dVector)(new On3dVector(OnUtil.On_yaxis) * prongDomeSettingDistance);
		On3dVector on3dVector4 = new On3dVector(OnUtil.On_yaxis) * -1.0;
		on3dVector4.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongDomeSettingAngle), OnUtil.On_xaxis);
		on3dVector4.Unitize();
		On3dVector on3dVector5 = new On3dVector(on3dVector4);
		on3dVector5.Reverse();
		OnNurbsCurve onNurbsCurve2 = CSUtilCurves.BlendCurve(left_side.PointAtStart(), on3dPoint, on3dVector3, on3dVector4);
		OnNurbsCurve onNurbsCurve3 = CSUtilCurves.BlendCurve(left_side.PointAtEnd(), on3dPoint, on3dVector3, on3dVector5);
		IOnCurve[] input_curves = new IOnCurve[2] { onNurbsCurve2, onNurbsCurve3 };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output == null)
		{
			return false;
		}
		if (output.Length == 0)
		{
			return false;
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(-50.0, 0.0, 0.0), new On3dPoint(50.0, 0.0, 0.0));
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		left_side.IntersectCurve(curveB, ref x2);
		if (x2.Count() <= 0)
		{
			return false;
		}
		double t3 = x2[0].get_m_a(0);
		OnCurve left_side2 = null;
		OnCurve right_side2 = null;
		if (!left_side.Split(t3, ref left_side2, ref right_side2))
		{
			return false;
		}
		if (left_side2 == null || right_side2 == null)
		{
			return false;
		}
		ArrayOnX_EVENT x3 = new ArrayOnX_EVENT();
		right_side.IntersectCurve(curveB, ref x3);
		if (x3.Count() <= 0)
		{
			return false;
		}
		double t4 = x3[0].get_m_a(0);
		OnCurve left_side3 = null;
		OnCurve right_side3 = null;
		if (!right_side.Split(t4, ref left_side3, ref right_side3))
		{
			return false;
		}
		if (left_side3 == null || right_side3 == null)
		{
			return false;
		}
		OnCurve railCurve = left_side2;
		OnCurve railCurve2 = right_side2;
		IOnCurve[] profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve, railCurve2, profiles, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		OnCurve railCurve3 = left_side3;
		OnCurve railCurve4 = right_side3;
		profiles = output;
		if (!CSUtilSweep.Sweep2NoParam(railCurve3, railCurve4, profiles, out var breps2, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			return false;
		}
		List<OnBrep> list = new List<OnBrep>();
		list.AddRange(breps);
		list.AddRange(breps2);
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (onBrep == null)
		{
			return false;
		}
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, prongTopY / 2.0, 0.0);
		onBrep.Transform(onXform3);
		dome = onBrep;
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref OnNurbsCurve prongRail, ref OnNurbsCurve prongTopProfile)
	{
		On3dVector on3dVector = prongRail.TangentAt(prongRail.Domain().Min());
		on3dVector.Unitize();
		On3dPoint on3dPoint = prongRail.PointAtStart();
		On3dPoint on3dPoint2 = on3dPoint - on3dVector * (prongExtensionHeight + CSApplicationHelper.Application.CamProngExtensionHeight);
		OnXform onXform = new OnXform();
		onXform.Translation(on3dPoint2 - on3dPoint);
		prongTopProfile.Transform(onXform);
		OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { prongRail, onLineCurve }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if ((mergedCrv.PointAtStart() - on3dPoint2).Length() > 0.01)
		{
			mergedCrv.Reverse();
		}
		return true;
	}

	private bool ReduceProngRailLength(ref OnNurbsCurve prongRail, ref OnNurbsCurve prongBaseProfile, double trimLength)
	{
		OnCurve left_side = new OnNurbsCurve();
		OnCurve right_side = new OnNurbsCurve();
		if (!prongRail.Split(prongRail.Domain().Max() - trimLength, ref left_side, ref right_side))
		{
			return false;
		}
		On3dVector translation_vector = new On3dVector(right_side.PointAtStart() - right_side.PointAtEnd());
		prongBaseProfile.Translate(translation_vector);
		prongRail = left_side.NurbsCurve().Duplicate();
		return true;
	}
}
