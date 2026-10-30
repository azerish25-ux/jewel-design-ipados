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
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
				OnNurbsCurve onNurbsCurve = iHeadSideX.GetArrayItem(i).GetNurbsCurve().Convert();
				if (onNurbsCurve == null)
				{
					return UpdateMissing("Invalid X side profile");
				}
				OnNurbsCurve onNurbsCurve2 = iHeadSideY.GetArrayItem(i).GetNurbsCurve().Convert();
				if (onNurbsCurve2 == null)
				{
					return UpdateMissing("Invalid Y side profile");
				}
				double num = onNurbsCurve.PointAtEnd().z * (1.0 - value);
				OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(100.0, 0.0, num));
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				onLineCurve.IntersectCurve(onNurbsCurve, ref x);
				if (x.Count() <= 0)
				{
					return UpdateError("Unable to intersect X side");
				}
				OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, 0.0, num), new On3dPoint(0.0, 100.0, num));
				ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
				onLineCurve2.IntersectCurve(onNurbsCurve2, ref x2);
				if (x2.Count() <= 0)
				{
					return UpdateError("Unable to intersect Y side");
				}
				double x3 = x[0].get_m_pointA(0).x;
				double y = x2[0].get_m_pointA(0).y;
				double t = x[0].get_m_b(0);
				double num2 = (0.0 - onNurbsCurve.TangentAt(t).x) * value8;
				OnXform onXform = new OnXform();
				onXform.Scale(new On3dVector(x3 / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(onXform.Convert() * xDown.Convert());
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
				On3dPoint test_point = cSGemData2.GetNurbsCurve().Convert().PointAt(cSGemData.BezelParam);
				double t2 = 0.0;
				curve.GetClosestPoint(test_point, ref t2);
				On3dPoint on3dPoint = curve.PointAt(t2);
				On3dVector on3dVector = OnUtil.ON_CrossProduct(curve.TangentAt(t2), OnUtil.On_zaxis);
				on3dVector.Unitize();
				on3dVector *= value5;
				double t3 = 0.0;
				double num3 = value7 + (0.0 - value5);
				if (num3 < 0.0 && (Math.Abs(num3) > cSGemData2.GemXSize / 2.0 || Math.Abs(num3) > cSGemData2.GemYSize / 2.0))
				{
					curve2.GetClosestPoint(on3dPoint - on3dVector, ref t3);
				}
				else
				{
					curve2.GetClosestPoint(on3dPoint + on3dVector, ref t3);
				}
				OnNurbsCurve onNurbsCurve3 = nurbsCurve.Duplicate();
				OnBoundingBox tight_bbox = new OnBoundingBox();
				onNurbsCurve3.GetTightBoundingBox(ref tight_bbox);
				OnXform onXform2 = new OnXform();
				onXform2.Translation(0.0 - tight_bbox.m_min.x, 0.0 - tight_bbox.m_min.y, 0.0 - tight_bbox.m_min.z);
				OnXform onXform3 = new OnXform();
				onXform3.Scale(value5 / (tight_bbox.m_max.x - tight_bbox.m_min.x), value6 / (tight_bbox.m_max.y - tight_bbox.m_min.y), 0.0);
				onNurbsCurve3.Transform(onXform3 * onXform2);
				for (int k = 0; k < onNurbsCurve3.m_cv_count; k++)
				{
					On4dPoint vertex_value = new On4dPoint();
					onNurbsCurve3.GetCV(k, ref vertex_value);
					vertex_value.x += num2 * vertex_value.y;
					onNurbsCurve3.SetCV(k, vertex_value);
				}
				double t4 = 0.0;
				curve.GetClosestPoint(test_point, ref t4);
				On3dPoint destination = curve.PointAt(t4);
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(value5, 0.0, 0.0), new On3dVector(-1.0, 0.0, 0.0), new On3dVector(0.0, 1.0, 0.0), destination, on3dVector, OnUtil.On_zaxis);
				onNurbsCurve3.Transform(xform);
				OnXform onXform4 = new OnXform();
				onXform4.Translation(0.0, 0.0, num);
				curve.Transform(xUp * onXform4);
				curve2.Transform(xUp * onXform4);
				onNurbsCurve3.Transform(xUp * onXform4);
				cSDataArray2.Append(new CSOnGeometryData(curve.Convert(), xUp.Convert() * onXform4.Convert(), applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(curve2.Convert(), xUp.Convert() * onXform4.Convert(), applyTransform: false));
				cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve3.Convert(), xUp.Convert() * onXform4.Convert(), applyTransform: false));
				if (!value4)
				{
					IOnCurve[] profiles = new IOnCurve[1] { onNurbsCurve3 };
					double[] parameters1In = new double[1] { t2 };
					double[] parameters2In = new double[1] { t3 };
					if (!CSUtilSweep.Sweep2(curve, curve2, profiles, parameters1In, parameters2In, out var breps, closed: true, maintainHeight: true))
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					if (breps.Length == 0)
					{
						return UpdateError("Failed Sweep1 on rails");
					}
					cSDataArray3.Append(new CSOnGeometryData(breps[0].Convert()));
				}
			}
			cSDataArray.Append(cSDataArray3);
		}
		oObjects.SetData(cSDataArray);
		oCurves.SetData(cSDataArray2);
		return base.Update();
	}
}
