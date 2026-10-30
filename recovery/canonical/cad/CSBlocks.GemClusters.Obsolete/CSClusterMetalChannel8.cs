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
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetalChannel_V8", "{470BEBB7-3303-4DAE-8061-46236587EFB0}")]
public sealed class CSClusterMetalChannel8 : CSBlock
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

	public CSInputChannel iChannelSpacing { get; private set; }

	public CSInputChannel iChannelWallDepth { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iCornerBlend { get; }

	public CSInputChannel iGemSizeZ { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iXYScaleToGem { get; }

	public CSInputChannel iCenterGemShapeOverrideMode { get; }

	public CSInputChannel iOffsetFix { get; }

	public CSInputChannel iInnerWallExtra { get; }

	public CSInputChannel iOuterWallExtra { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailZ { get; }

	public CSClusterMetalChannel8(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverride = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverride), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		if (!(flattenArray.GetArrayItem(0).Duplicate() is CSGemData centerGem))
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData centerGemShapeOverride = iCenterGemShapeOverride.Data as CSGemData;
		OnNurbsCurve profile = iProfile.GetNurbsCurve().Duplicate();
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
		iChannelWallDepth.GetDouble(ref value6);
		double value7 = 0.0;
		iAngle.GetDouble(ref value7);
		value7 = CSBlockUtil.CSUtilMath.ToRadians(value7);
		double value8 = 0.0;
		iPlacement.GetDouble(ref value8);
		double value9 = 0.0;
		iCornerBlend.GetDouble(ref value9);
		double value10 = 0.0;
		iGemSizeZ.GetDouble(ref value10);
		double value11 = 0.0;
		iOffsetFix.GetDouble(ref value11);
		value += value11;
		bool value12 = false;
		iCenterGemShapeOverrideEnabled.GetBoolean(ref value12);
		bool value13 = true;
		iShowChannel.GetBoolean(ref value13);
		bool value14 = false;
		iXYScaleToGem.GetBoolean(ref value14);
		double value15 = 0.0;
		iInnerWallExtra.GetDouble(ref value15);
		double value16 = 0.0;
		iOuterWallExtra.GetDouble(ref value16);
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		iCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		CSDataArray flattenArray2 = iRailX.GetFlattenArray();
		double value17 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value17);
		}
		value += value17;
		CSDataArray flattenArray3 = iRailZ.GetFlattenArray();
		double value18 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value18);
		}
		value8 += value18;
		if (!MakeCluster(centerGem, value12, centerGemShapeOverride, value, profile, value2, value3, value4, value5, value6, value7, value8, value9, value10, value13, value14, whichItem, value15, value16, out var outputMetals, out var outputTransforms, out var outError))
		{
			return UpdateError(outError);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < outputMetals.Count; i++)
		{
			cSDataArray.Append(new CSOnGeometryData(outputMetals[i].Convert(), outputTransforms[i].Convert(), applyTransform: false));
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

	private static bool MakeCluster(CSGemData centerGem, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out string outError)
	{
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		profile.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform3 = new OnXform();
		onXform3.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform(onXform4 * onXform3 * onXform2 * onXform);
		OnCurve onCurve9;
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve(new On3dPoint(channelWallThicknessInner, -1.0, 0.0), new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve(new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest(profile, c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest(profile, c2, ref param2))
			{
				outError = "Intersect for channel Failed";
				return false;
			}
			On3dPoint on3dPoint = profile.PointAt(param);
			On3dPoint on3dPoint2 = profile.PointAt(param2);
			double num = ((on3dPoint.y > on3dPoint2.y) ? on3dPoint.y : on3dPoint2.y);
			OnXform onXform5 = new OnXform();
			onXform5.Scale(1.0, profileHeight / num, 1.0);
			profile.Transform(onXform5);
			OnCurve onCurve = profile.Duplicate();
			onCurve.Trim(new OnInterval(profile.Domain().Min(), param));
			OnCurve onCurve2 = profile.Duplicate();
			onCurve2.Trim(new OnInterval(param2, profile.Domain().Max()));
			double x = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double y = profileHeight - gemSizeZ;
			On3dPoint on3dPoint3 = new On3dPoint(x, y, 0.0);
			OnCurve onCurve7;
			OnCurve onCurve8;
			IOnCurve[] input_curves;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				On3dPoint on3dPoint4 = onCurve.PointAtEnd();
				OnCurve onCurve3 = new OnLineCurve(on3dPoint4, new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0));
				OnCurve onCurve4 = new OnLineCurve(new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0), on3dPoint3);
				CSUtilGeometry.TrimEndOfCrv(onCurve3, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve4, cornerBlend);
				OnCurve item = CSUtilCurves.BlendCurve(onCurve3, onCurve4);
				On3dPoint on3dPoint5 = onCurve2.PointAtStart();
				OnCurve onCurve5 = new OnLineCurve(on3dPoint3, new On3dPoint(on3dPoint5.x, on3dPoint5.y - channelWallDepth, 0.0));
				OnCurve onCurve6 = new OnLineCurve(new On3dPoint(on3dPoint5.x, on3dPoint5.y - channelWallDepth, 0.0), on3dPoint5);
				CSUtilGeometry.TrimEndOfCrv(onCurve5, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve6, cornerBlend);
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
			CSUtilGeometry.TrimEndOfCrv(onCurve, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve7, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve8, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(onCurve2, cornerBlend);
			OnCurve item3 = CSUtilCurves.BlendCurve(onCurve, onCurve7);
			OnCurve item4 = CSUtilCurves.BlendCurve(onCurve7, onCurve8);
			OnCurve item5 = CSUtilCurves.BlendCurve(onCurve8, onCurve2);
			List<OnCurve> obj3 = new List<OnCurve> { onCurve, item3, onCurve7, item4, onCurve8, item5, onCurve2 };
			OnCurve[] output2 = null;
			input_curves = obj3.ToArray();
			if (!RhUtil.RhinoMergeCurves(input_curves, ref output2))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (output2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			onCurve9 = output2[0];
		}
		else
		{
			OnXform onXform6 = new OnXform();
			onXform6.Scale(1.0, profileHeight, 1.0);
			profile.Transform(onXform6);
			onCurve9 = profile;
		}
		OnXform onXform7 = new OnXform();
		onXform7.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform onXform8 = new OnXform();
		onXform8.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		onCurve9.Transform(onXform8 * onXform7);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double scaleFactorX;
				switch (centerGemShapeOverrideMode)
				{
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
					scaleFactorX = centerGem.GemXSize / centerGemShapeOverride.GemXSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
					scaleFactorX = centerGem.GemYSize / centerGemShapeOverride.GemYSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
					scaleFactorX = ((!(centerGem.GemXSize > centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
					scaleFactorX = ((!(centerGem.GemXSize < centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				default:
					outError = "Invalid Gem Cluster Center Shape Override Mode";
					return false;
				}
				centerGemShapeOverride.Scale(scaleFactorX);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out var xUp, out var xDown))
		{
			outError = "Gem X form";
			return false;
		}
		CSGemData cSGemData = new CSGemData(centerGem);
		cSGemData.ApplyTransform(xDown);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox))
		{
			outError = "Gem Invalid Bounding Box";
			return false;
		}
		On3dVector on3dVector = new On3dVector(cSGemData.Up.Convert());
		on3dVector.Unitize();
		if (!GemOffsetCurveV2.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, 0.0, 0.0, 0.0))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		onCurve9.GetNurbForm(ref nurbs_curve2);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.Create(3, is_rat: true, nurbs_curve2.m_order, nurbs_curve.m_order, nurbs_curve2.m_cv_count, nurbs_curve.m_cv_count);
		bool flag = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		for (int i = 0; i < nurbs_curve2.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			nurbs_curve2.GetCV(i, ref vertex_value);
			if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, 0.0, 0.0, 0.0))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			curve.Translate(new On3dVector(on3dVector) * (placement + vertex_value.y));
			list.Add(curve);
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve3);
			if (nurbs_curve.CVCount() != nurbs_curve3.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < nurbs_curve.CVCount(); j++)
				{
					On3dPoint vertex_value2 = new On3dPoint();
					nurbs_curve3.GetCV(j, ref vertex_value2);
					double num2 = nurbs_curve2.Weight(i) * nurbs_curve3.Weight(j);
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num2, vertex_value2.y * num2, vertex_value2.z * num2, num2));
				}
			}
		}
		if (!flag)
		{
			for (int k = 0; k < nurbs_curve2.KnotCount(); k++)
			{
				onNurbsSurface.SetKnot(0, k, nurbs_curve2.Knot(k));
			}
			for (int l = 0; l < nurbs_curve.KnotCount(); l++)
			{
				onNurbsSurface.SetKnot(1, l, nurbs_curve.Knot(l));
			}
			OnBrep brep = new OnBrep();
			onNurbsSurface.BrepForm(brep);
			brep.Transform(xUp.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep brep2 = Interop.FromOnBrep(brep);
				brep2.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!brep2.IsSolid)
				{
					brep2.Flip();
				}
				brep = brep2.Convert();
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xUp.Convert());
		}
		else
		{
			OnBrep brep3 = CSUtilSweep.Loft(list.ToArray(), IArgsRhinoLoft.eLoftType.ltLoose);
			if (brep3 != null)
			{
				brep3.Transform(xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xUp.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
