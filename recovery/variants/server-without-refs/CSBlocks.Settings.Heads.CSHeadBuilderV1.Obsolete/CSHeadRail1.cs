using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadRail_V1", "{6865A811-1C30-4cc3-A55D-513C06A2D09E}")]
public sealed class CSHeadRail1 : CSBlock
{
	public CSInputChannel IGem { get; }

	public CSInputChannel IHeadSideX { get; }

	public CSInputChannel IHeadSideY { get; }

	public CSInputChannel IWhichRail { get; }

	public CSInputChannel IRailCount { get; }

	public CSInputChannel IRailZPos { get; }

	public CSInputChannel IRailProfile { get; }

	public CSInputChannel IRailX { get; }

	public CSInputChannel IRailY { get; }

	public CSInputChannel IEnableSecondControl { get; }

	public CSInputChannel IRailProfile2 { get; }

	public CSInputChannel IRailX2 { get; }

	public CSInputChannel IRailY2 { get; }

	public CSOutputChannel ORails { get; }

	public CSHeadRail1(string identifier)
		: base(identifier)
	{
		IGem = new CSInputChannel(GetPropertyName(() => IGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		IHeadSideX = new CSInputChannel(GetPropertyName(() => IHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IHeadSideY = new CSInputChannel(GetPropertyName(() => IHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IWhichRail = new CSInputChannel(GetPropertyName(() => IWhichRail), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailCount = new CSInputChannel(GetPropertyName(() => IRailCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailZPos = new CSInputChannel(GetPropertyName(() => IRailZPos), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailProfile = new CSInputChannel(GetPropertyName(() => IRailProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IRailX = new CSInputChannel(GetPropertyName(() => IRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailY = new CSInputChannel(GetPropertyName(() => IRailY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IEnableSecondControl = new CSInputChannel(GetPropertyName(() => IEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IRailProfile2 = new CSInputChannel(GetPropertyName(() => IRailProfile2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		IRailX2 = new CSInputChannel(GetPropertyName(() => IRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IRailY2 = new CSInputChannel(GetPropertyName(() => IRailY2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		ORails = new CSOutputChannel(GetPropertyName(() => ORails), this, ChannelDataType.Any, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01d3: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbsCurve = IHeadSideX.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Head X Curve");
		}
		OnNurbsCurve nurbsCurve2 = IHeadSideY.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Missing Head Y Curve");
		}
		double value = 0.0;
		IWhichRail.GetDouble(ref value);
		double value2 = 0.0;
		IRailCount.GetDouble(ref value2);
		if (value > value2)
		{
			return base.Update();
		}
		double value3 = 0.0;
		IRailZPos.GetDouble(ref value3);
		bool value4 = false;
		IEnableSecondControl.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		OnNurbsCurve nurbsCurve3;
		if (!value4)
		{
			nurbsCurve3 = IRailProfile.GetNurbsCurve();
			if (nurbsCurve3 == null)
			{
				return UpdateMissing("Missing Rail Profile");
			}
			IRailX.GetDouble(ref value5);
			IRailY.GetDouble(ref value6);
		}
		else
		{
			nurbsCurve3 = IRailProfile2.GetNurbsCurve();
			if (nurbsCurve3 == null)
			{
				return UpdateMissing("Missing Rail Profile 2");
			}
			IRailX2.GetDouble(ref value5);
			IRailY2.GetDouble(ref value6);
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
		double num = ((OnCurve)nurbsCurve).PointAtEnd().z * (1.0 - value3);
		OnLineCurve val = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(100.0, 0.0, num));
		OnLineCurve val2 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(0.0, 100.0, num));
		ArrayOnX_EVENT val3 = new ArrayOnX_EVENT();
		ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectCurve((IOnCurve)(object)nurbsCurve, ref val3);
		((OnCurve)val2).IntersectCurve((IOnCurve)(object)nurbsCurve2, ref val4);
		if (val3.Count() <= 0)
		{
			return UpdateError("Intersect X");
		}
		if (val4.Count() <= 0)
		{
			return UpdateError("Intersect Y");
		}
		double x = val3[0][0].x;
		double y = val4[0][0].y;
		double num2 = val3[0][0];
		double num3 = 0.0 - ((OnCurve)nurbsCurve).TangentAt(num2).x;
		CSGemData cSGemData2 = new CSGemData(cSGemData);
		cSGemData2.ApplyTransform(xDown.Convert());
		OnXform val5 = new OnXform();
		val5.Scale((IOn3dVector)new On3dVector(x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
		cSGemData2.ApplyTransform(val5.Convert());
		if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0, out OnCurve curve, doTransformUp: false, out string error))
		{
			return UpdateError(error);
		}
		OnNurbsCurve val6 = new OnNurbsCurve();
		curve.GetNurbForm(ref val6);
		if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0 - value5, out curve, doTransformUp: false, out error))
		{
			return UpdateError(error);
		}
		OnNurbsCurve val7 = new OnNurbsCurve();
		curve.GetNurbForm(ref val7);
		On3dPoint val8 = ((Curve)cSGemData2.GetNurbsCurve()).PointAt(cSGemData.BezelParam).Convert();
		double num4 = 0.0;
		((OnCurve)val6).GetClosestPoint((IOn3dPoint)(object)val8, ref num4);
		On3dPoint val9 = ((OnCurve)val6).PointAt(num4);
		On3dVector val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)((OnCurve)val6).TangentAt(num4), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0));
		val10.Unitize();
		val10 *= value5;
		double num5 = 0.0;
		((OnCurve)val7).GetClosestPoint((IOn3dPoint)(object)(val9 + (IOn3dVector)(object)val10), ref num5);
		OnBoundingBox val11 = new OnBoundingBox();
		((OnGeometry)nurbsCurve3).GetTightBoundingBox(ref val11);
		OnXform val12 = new OnXform();
		val12.Translation(0.0 - val11.m_min.x, 0.0 - val11.m_min.y, 0.0 - val11.m_min.z);
		OnXform val13 = new OnXform();
		val13.Scale(value5 / (val11.m_max.x - val11.m_min.x), value6 / (val11.m_max.y - val11.m_min.y), 0.0);
		OnXform val14 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(value5, 0.0, 0.0), (IOn3dVector)new On3dVector(-1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)(object)val8, (IOn3dVector)(object)val10, (IOn3dVector)new On3dVector(0.0, 0.0, 1.0));
		nurbsCurve3.Transform((IOnXform)(object)(val13 * (IOnXform)(object)val12));
		for (int i = 0; i < nurbsCurve3.m_cv_count; i++)
		{
			On4dPoint val15 = new On4dPoint();
			nurbsCurve3.GetCV(i, ref val15);
			val15.x += num3 * val15.y;
			nurbsCurve3.SetCV(i, (IOn4dPoint)(object)val15);
		}
		nurbsCurve3.Transform((IOnXform)(object)val14);
		if (!CSUtilSweep.Sweep2(profiles: (IOnCurve[])(object)new IOnCurve[1] { (IOnCurve)nurbsCurve3 }, parameters1In: new double[1] { num4 }, parameters2In: new double[1] { num5 }, railCurve1: (IOnCurve)(object)val6, railCurve2: (IOnCurve)(object)val7, breps: out var breps, closed: true, maintainHeight: true))
		{
			return UpdateError("Sweep 2");
		}
		OnXform val16 = new OnXform();
		val16.Translation(0.0, 0.0, num);
		val6.Transform((IOnXform)(object)val16);
		val7.Transform((IOnXform)(object)val16);
		nurbsCurve3.Transform((IOnXform)(object)val16);
		((OnGeometry)breps[0]).Transform((IOnXform)(object)val16);
		((OnGeometry)breps[0]).Transform((IOnXform)(object)xUp);
		ORails.SetData(new CSOnGeometryData((GeometryBase)(object)breps[0].Convert(), xUp.Convert() * val16.Convert(), applyTransform: false));
		return base.Update();
	}
}
