using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;
using CSCompatibility;
using CSDatas;
using CSGemInstance;
using RMA.OpenNURBS;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("HeadProngPoints_V1", "{77479DCE-BFD1-4D45-88D0-469DE8838B4E}")]
public sealed class CSHeadProngPoints1 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngAngle { get; }

	public CSInputChannel iGameChanger { get; }

	public CSOutputChannel oPoints { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadProngPoints1(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngAngle = new CSInputChannel(GetPropertyName(() => iProngAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(GetPropertyName(() => oPoints), this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		double value2 = 0.0;
		iProngAngle.GetDouble(ref value2);
		bool value3 = false;
		iGameChanger.GetBoolean(ref value3);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
			OnNurbsCurve onNurbsCurve = cSGemData.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			onNurbsCurve.Transform(xDown);
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve);
			IList<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			List<CSHeadStyles2.HeadProngInfo> list2 = (value3 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value, cSGemData.VersionMajor));
			if (list2 == null)
			{
				return UpdateError("Missing Prong Infos");
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			foreach (CSHeadStyles2.HeadProngInfo item in list2)
			{
				if (item.ProngParamIndex < 0 || item.ProngParamIndex >= list.Count)
				{
					continue;
				}
				double prongParam = list[item.ProngParamIndex].ProngParam;
				On3dPoint on3dPoint = onNurbsCurve.PointAt(prongParam);
				double angle_radians = CSBlockUtil.CSUtilMath.ToRadians(item.ProngParamAngleOffset + value2 * (double)Math.Sign(item.Mirror));
				OnXform onXform = new OnXform();
				onXform.Rotation(angle_radians, OnUtil.On_zaxis, OnUtil.On_origin);
				OnLineCurve onLineCurve = new OnLineCurve(OnUtil.On_origin, on3dPoint * 20.0);
				onLineCurve.Transform(onXform);
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				nurbs_curve.IntersectCurve(onLineCurve, ref x);
				if (x.Count() > 0)
				{
					double num = x[0].get_m_a(0);
					On3dPoint on3dPoint2 = new On3dPoint(nurbs_curve.PointAt(num));
					double num2 = num - 0.01;
					if (num2 < nurbs_curve.Domain().Min())
					{
						num2 += nurbs_curve.Domain().Max() - nurbs_curve.Domain().Min();
					}
					On3dVector obj = OnUtil.ON_CrossProduct(arg2: nurbs_curve.TangentAt(num2), arg1: OnUtil.On_zaxis);
					obj.Unitize();
					double num3 = num + 0.01;
					if (num3 > nurbs_curve.Domain().Max())
					{
						num3 -= nurbs_curve.Domain().Max() - nurbs_curve.Domain().Min();
					}
					On3dVector on3dVector = OnUtil.ON_CrossProduct(arg2: nurbs_curve.TangentAt(num3), arg1: OnUtil.On_zaxis);
					on3dVector.Unitize();
					On3dVector on3dVector2 = obj + on3dVector;
					on3dVector2.Unitize();
					On3dPoint on3dPoint3 = on3dPoint2 + on3dVector2 * item.Nudge;
					On3dPoint on3dPoint4 = new On3dPoint(on3dPoint3);
					on3dPoint4.Transform(xUp);
					cSDataArray3.Append(new CSPointData(on3dPoint4.Convert()));
					On3dVector on3dVector3 = new On3dVector(OnUtil.On_zaxis);
					on3dVector3.Unitize();
					On3dPoint arg = new On3dPoint(on3dPoint3) + on3dVector3 * 0.5;
					OnLineCurve onLineCurve2 = new OnLineCurve(on3dPoint3, arg);
					onLineCurve2.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve2.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector4 = new On3dVector(on3dVector2);
					on3dVector4.Unitize();
					On3dPoint arg2 = new On3dPoint(on3dPoint3) + on3dVector4 * 0.5;
					OnLineCurve onLineCurve3 = new OnLineCurve(on3dPoint3, arg2);
					onLineCurve3.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve3.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector5 = OnUtil.ON_CrossProduct(on3dVector4, on3dVector3);
					on3dVector5.Unitize();
					On3dPoint arg3 = new On3dPoint(on3dPoint3) + on3dVector5 * 0.5;
					OnLineCurve onLineCurve4 = new OnLineCurve(on3dPoint3, arg3);
					onLineCurve4.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve4.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector6 = new On3dVector(on3dVector3);
					on3dVector6.Reverse();
					On3dPoint arg4 = new On3dPoint(on3dPoint3) + on3dVector6 * 0.5;
					OnLineCurve onLineCurve5 = new OnLineCurve(on3dPoint3, arg4);
					onLineCurve5.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve5.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector7 = new On3dVector(on3dVector4);
					on3dVector7.Reverse();
					On3dPoint arg5 = new On3dPoint(on3dPoint3) + on3dVector7 * 0.5;
					OnLineCurve onLineCurve6 = new OnLineCurve(on3dPoint3, arg5);
					onLineCurve6.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve6.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector8 = new On3dVector(on3dVector5);
					on3dVector8.Reverse();
					On3dPoint arg6 = new On3dPoint(on3dPoint3) + on3dVector8 * 0.5;
					OnLineCurve onLineCurve7 = new OnLineCurve(on3dPoint3, arg6);
					onLineCurve7.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve7.Convert(), xUp.Convert(), applyTransform: false));
				}
			}
			cSDataArray.Append(cSDataArray3);
			cSDataArray2.Append(cSDataArray4);
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			oPoints.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			oPoints.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oPoints.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 0)
		{
			oCurves.SetData(null);
		}
		else if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurves.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurves.SetData(cSDataArray2);
		}
		return base.Update();
	}
}
