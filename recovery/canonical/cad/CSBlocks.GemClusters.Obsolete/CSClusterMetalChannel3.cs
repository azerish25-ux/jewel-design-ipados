using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetalChannel_V3", "{366CF44C-44D1-42CB-AB80-DFBF6FE94A00}")]
public sealed class CSClusterMetalChannel3 : CSBlock
{
	public CSOutputChannel oMetal { get; }

	public CSInputChannel iCenterGem { get; }

	public CSInputChannel iCenterGemShapeOverrideEnabled { get; }

	public CSInputChannel iCenterGemShapeOverride { get; }

	public CSInputChannel iCenterGemSpacing { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iProfileWidth { get; }

	public CSInputChannel iProfileHeight { get; }

	public CSInputChannel iChannelWallThicknessInner { get; }

	public CSInputChannel iChannelWallThicknessOuter { get; }

	public CSInputChannel iChannelSpacing { get; }

	public CSInputChannel iChannelWallDepth { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iCornerBlend { get; }

	public CSInputChannel iGemSizeZ { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iXYScaleToGem { get; }

	public CSClusterMetalChannel3(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(GetPropertyName(() => iCenterGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCenterGemShapeOverrideEnabled = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverrideEnabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverride = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverride), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemSpacing = new CSInputChannel(GetPropertyName(() => iCenterGemSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(GetPropertyName(() => iProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileHeight = new CSInputChannel(GetPropertyName(() => iProfileHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessInner = new CSInputChannel(GetPropertyName(() => iChannelWallThicknessInner), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessOuter = new CSInputChannel(GetPropertyName(() => iChannelWallThicknessOuter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(GetPropertyName(() => iChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallDepth = new CSInputChannel(GetPropertyName(() => iChannelWallDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCornerBlend = new CSInputChannel(GetPropertyName(() => iCornerBlend), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSizeZ = new CSInputChannel(GetPropertyName(() => iGemSizeZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(GetPropertyName(() => iShowChannel), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iXYScaleToGem = new CSInputChannel(GetPropertyName(() => iXYScaleToGem), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSGemData cSGemData = iCenterGemShapeOverride.Data as CSGemData;
		OnNurbsCurve onNurbsCurve = iProfile.GetNurbsCurve().Duplicate();
		double value = 0.0;
		iCenterGemSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iProfileWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iProfileHeight.GetDouble(ref value3);
		double value4 = 0.0;
		iChannelWallThicknessInner.GetDouble(ref value4);
		double value5 = 0.0;
		iChannelWallThicknessOuter.GetDouble(ref value5);
		double value6 = 0.0;
		iCenterGemSpacing.GetDouble(ref value6);
		double value7 = 0.0;
		iChannelWallDepth.GetDouble(ref value7);
		double value8 = 0.0;
		iAngle.GetDouble(ref value8);
		value8 = CSBlockUtil.CSUtilMath.ToRadians(value8);
		double value9 = 0.0;
		iPlacement.GetDouble(ref value9);
		double value10 = 0.0;
		iCornerBlend.GetDouble(ref value10);
		double value11 = 0.0;
		iGemSizeZ.GetDouble(ref value11);
		bool value12 = false;
		iCenterGemShapeOverrideEnabled.GetBoolean(ref value12);
		bool value13 = true;
		iShowChannel.GetBoolean(ref value13);
		bool value14 = false;
		iXYScaleToGem.GetBoolean(ref value14);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		onNurbsCurve.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform3 = new OnXform();
		onXform3.Scale(value2, 1.0, 1.0);
		onNurbsCurve.Transform(onXform3 * onXform2 * onXform);
		OnCurve onCurve9;
		if (value13)
		{
			OnLineCurve c = new OnLineCurve(new On3dPoint(value4, -1.0, 0.0), new On3dPoint(value4, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve(new On3dPoint(value2 - value5, -1.0, 0.0), new On3dPoint(value2 - value5, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest(onNurbsCurve, c, ref param))
			{
				return UpdateError("Intersect for channel Inner Failed");
			}
			if (!CurveIntersectHighest(onNurbsCurve, c2, ref param2))
			{
				return UpdateError("Intersect for channel Failed");
			}
			On3dPoint on3dPoint = onNurbsCurve.PointAt(param);
			On3dPoint on3dPoint2 = onNurbsCurve.PointAt(param2);
			double num = ((on3dPoint.y > on3dPoint2.y) ? on3dPoint.y : on3dPoint2.y);
			OnXform onXform4 = new OnXform();
			onXform4.Scale(1.0, value3 / num, 1.0);
			onNurbsCurve.Transform(onXform4);
			OnCurve onCurve = onNurbsCurve.Duplicate();
			onCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), param));
			OnCurve onCurve2 = onNurbsCurve.Duplicate();
			onCurve2.Trim(new OnInterval(param2, onNurbsCurve.Domain().Max()));
			double x = value4 + (value2 - value4 - value5) / 2.0;
			double y = value3 - value11;
			On3dPoint on3dPoint3 = new On3dPoint(x, y, 0.0);
			OnCurve onCurve7;
			OnCurve onCurve8;
			IOnCurve[] input_curves;
			if (value7 > value10 * 2.0)
			{
				On3dPoint on3dPoint4 = onCurve.PointAtEnd();
				OnCurve onCurve3 = new OnLineCurve(on3dPoint4, new On3dPoint(on3dPoint4.x, on3dPoint4.y - value7, 0.0));
				OnCurve onCurve4 = new OnLineCurve(new On3dPoint(on3dPoint4.x, on3dPoint4.y - value7, 0.0), on3dPoint3);
				CSUtilGeometry.TrimEndOfCrv(onCurve3, value10);
				CSUtilGeometry.TrimStartOfCrv(onCurve4, value10);
				OnCurve item = CSUtilCurves.BlendCurve(onCurve3, onCurve4);
				On3dPoint on3dPoint5 = onCurve2.PointAtStart();
				OnCurve onCurve5 = new OnLineCurve(on3dPoint3, new On3dPoint(on3dPoint5.x, on3dPoint5.y - value7, 0.0));
				OnCurve onCurve6 = new OnLineCurve(new On3dPoint(on3dPoint5.x, on3dPoint5.y - value7, 0.0), on3dPoint5);
				CSUtilGeometry.TrimEndOfCrv(onCurve5, value10);
				CSUtilGeometry.TrimStartOfCrv(onCurve6, value10);
				OnCurve item2 = CSUtilCurves.BlendCurve(onCurve5, onCurve6);
				List<OnCurve> obj = new List<OnCurve> { onCurve3, item, onCurve4 };
				OnCurve[] output = null;
				input_curves = obj.ToArray();
				RhUtil.RhinoMergeCurves(input_curves, ref output);
				onCurve7 = output[0];
				List<OnCurve> obj2 = new List<OnCurve> { onCurve5, item2, onCurve6 };
				output = null;
				input_curves = obj2.ToArray();
				RhUtil.RhinoMergeCurves(input_curves, ref output);
				onCurve8 = output[0];
			}
			else
			{
				onCurve7 = new OnLineCurve(onCurve.PointAtEnd(), on3dPoint3);
				onCurve8 = new OnLineCurve(on3dPoint3, onCurve2.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(onCurve, value10);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve7, value10);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve8, value10);
			CSUtilGeometry.TrimStartOfCrv(onCurve2, value10);
			OnCurve item3 = CSUtilCurves.BlendCurve(onCurve, onCurve7);
			OnCurve item4 = CSUtilCurves.BlendCurve(onCurve7, onCurve8);
			OnCurve item5 = CSUtilCurves.BlendCurve(onCurve8, onCurve2);
			List<OnCurve> obj3 = new List<OnCurve> { onCurve, item3, onCurve7, item4, onCurve8, item5, onCurve2 };
			OnCurve[] output2 = null;
			input_curves = obj3.ToArray();
			if (!RhUtil.RhinoMergeCurves(input_curves, ref output2))
			{
				return UpdateError("channel Merge failed");
			}
			if (output2.Length != 1)
			{
				return UpdateError("channel Merge failed");
			}
			onCurve9 = output2[0];
		}
		else
		{
			OnXform onXform5 = new OnXform();
			onXform5.Scale(1.0, value3, 1.0);
			onNurbsCurve.Transform(onXform5);
			onCurve9 = onNurbsCurve;
		}
		OnXform onXform6 = new OnXform();
		onXform6.Translation(0.0, 0.0 - value3, 0.0);
		OnXform onXform7 = new OnXform();
		onXform7.Rotation(value8, OnUtil.On_zaxis, OnUtil.On_origin);
		onCurve9.Transform(onXform7 * onXform6);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i).Duplicate() is CSGemData cSGemData2))
			{
				continue;
			}
			string gemShape;
			if (value12)
			{
				if (cSGemData == null)
				{
					cSGemData = new CSGemData(cSGemData2);
				}
				cSGemData.Xform = cSGemData2.Xform;
				gemShape = cSGemData.GemShape;
			}
			else
			{
				cSGemData = new CSGemData(cSGemData2);
				gemShape = cSGemData2.GemShape;
			}
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData2.Xform, out var xUp, out var xDown))
			{
				return UpdateMissing("Gem X form");
			}
			cSGemData2.ApplyTransform(xDown);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox))
			{
				return UpdateMissing("Gem Invalid Bounding Box");
			}
			On3dVector on3dVector = new On3dVector(cSGemData2.Up.Convert());
			on3dVector.Unitize();
			double num2 = value;
			double num3 = value9;
			if (!GemOffsetCurveV2.GemOffset(gemShape, cSGemData, num2, out OnCurve curve, false, out string error, 0.0, 0.0, 0.0))
			{
				return UpdateError("CurveTop Inside:" + error);
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve);
			OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
			onCurve9.GetNurbForm(ref nurbs_curve2);
			OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
			onNurbsSurface.Create(3, is_rat: true, nurbs_curve2.m_order, nurbs_curve.m_order, nurbs_curve2.m_cv_count, nurbs_curve.m_cv_count);
			bool flag = false;
			List<OnCurve> list = new List<OnCurve>();
			CSGemData gem = (value14 ? cSGemData2 : cSGemData);
			for (int j = 0; j < nurbs_curve2.CVCount(); j++)
			{
				On3dPoint vertex_value = new On3dPoint();
				nurbs_curve2.GetCV(j, ref vertex_value);
				if (!GemOffsetCurveV2.GemOffset(gemShape, gem, num2 + vertex_value.x, out curve, false, out error, 0.0, 0.0, 0.0))
				{
					return UpdateError("CurveTop Inside:" + error);
				}
				curve.Translate(new On3dVector(on3dVector) * (num3 + vertex_value.y));
				list.Add(curve);
				OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
				curve.GetNurbForm(ref nurbs_curve3);
				if (nurbs_curve.CVCount() != nurbs_curve3.CVCount())
				{
					flag = true;
				}
				if (!flag)
				{
					for (int k = 0; k < nurbs_curve.CVCount(); k++)
					{
						On3dPoint vertex_value2 = new On3dPoint();
						nurbs_curve3.GetCV(k, ref vertex_value2);
						double num4 = nurbs_curve2.Weight(j) * nurbs_curve3.Weight(k);
						onNurbsSurface.SetCV(j, k, new On4dPoint(vertex_value2.x * num4, vertex_value2.y * num4, vertex_value2.z * num4, num4));
					}
				}
			}
			if (!flag)
			{
				for (int l = 0; l < nurbs_curve2.KnotCount(); l++)
				{
					onNurbsSurface.SetKnot(0, l, nurbs_curve2.Knot(l));
				}
				for (int m = 0; m < nurbs_curve.KnotCount(); m++)
				{
					onNurbsSurface.SetKnot(1, m, nurbs_curve.Knot(m));
				}
				OnBrep onBrep = new OnBrep();
				onNurbsSurface.BrepForm(onBrep);
				onBrep.Transform(xUp.Convert());
				cSDataArray.Append(new CSOnGeometryData(onBrep.Convert(), xUp, applyTransform: false));
			}
			else
			{
				IOnCurve[] input_curves = list.ToArray();
				OnBrep onBrep2 = CSUtilSweep.Loft(input_curves, IArgsRhinoLoft.eLoftType.ltLoose);
				if (onBrep2 != null)
				{
					onBrep2.Transform(xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData(onBrep2.Convert(), xUp, applyTransform: false));
				}
			}
		}
		oMetal.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CurveIntersectHighest(IOnCurve c1, IOnCurve c2, ref double param)
	{
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		c1.IntersectCurve(c2, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() == 0)
		{
			return false;
		}
		double num = -999.0;
		foreach (OnX_EVENT item in x)
		{
			if (item.get_m_pointA(0).y > num)
			{
				num = item.get_m_pointA(0).y;
				param = item.get_m_a(0);
			}
		}
		return true;
	}
}
