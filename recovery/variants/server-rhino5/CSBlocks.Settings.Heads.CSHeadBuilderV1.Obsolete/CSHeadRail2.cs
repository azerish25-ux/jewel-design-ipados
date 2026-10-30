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
[CSBlock("HeadRail_V2", "{357943F0-C64C-4226-87DE-DA19755111BD}")]
public sealed class CSHeadRail2 : CSBlock
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

	public CSInputChannel IObjects { get; }

	public CSOutputChannel OObjects { get; }

	public CSHeadRail2(string identifier)
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
		IObjects = new CSInputChannel(GetPropertyName(() => IObjects), this, ChannelDataType.Any, ChannelArrayType.Individual);
		OObjects = new CSOutputChannel(GetPropertyName(() => OObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_0296: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gem input missing");
		}
		double value = 0.0;
		IWhichRail.GetDouble(ref value);
		double value2 = 0.0;
		IRailCount.GetDouble(ref value2);
		double value3 = 0.0;
		IRailZPos.GetDouble(ref value3);
		bool value4 = false;
		IEnableSecondControl.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		OnNurbsCurve nurbsCurve;
		if (!value4)
		{
			nurbsCurve = IRailProfile.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Missing rail profile");
			}
			IRailX.GetDouble(ref value5);
			IRailY.GetDouble(ref value6);
		}
		else
		{
			nurbsCurve = IRailProfile2.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Missing rail profile 2");
			}
			IRailX2.GetDouble(ref value5);
			IRailY2.GetDouble(ref value6);
		}
		ICSArray iCSArray = IObjects.Data as ICSArray;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataArray cSDataArray2 = new CSDataArray();
			if (iCSArray != null && iCSArray.GetArrayCount() > 0)
			{
				if (!(iCSArray.GetArrayItem(i) is ICSArray iCSArray2))
				{
					continue;
				}
				for (int j = 0; j < iCSArray2.GetArrayCount(); j++)
				{
					cSDataArray2.Append(iCSArray2.GetArrayItem(j));
				}
			}
			if (value <= value2)
			{
				if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
				{
					return UpdateError("Invalid gem");
				}
				OnNurbsCurve val = IHeadSideX.GetArrayItem(i).GetNurbsCurve().Convert();
				if (val == null)
				{
					return UpdateError("Invalid side X curve");
				}
				OnNurbsCurve val2 = IHeadSideY.GetArrayItem(i).GetNurbsCurve().Convert();
				if (val2 == null)
				{
					return UpdateError("Invalid side Y curve");
				}
				CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
				double num = ((OnCurve)val).PointAtEnd().z * (1.0 - value3);
				OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(100.0, 0.0, num));
				OnLineCurve val4 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, num), (IOn3dPoint)new On3dPoint(0.0, 100.0, num));
				ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
				ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
				((OnCurve)val3).IntersectCurve((IOnCurve)(object)val, ref val5);
				((OnCurve)val4).IntersectCurve((IOnCurve)(object)val2, ref val6);
				if (val5.Count() <= 0)
				{
					return UpdateError("IntersectCurve X curve");
				}
				if (val6.Count() <= 0)
				{
					return UpdateError("IntersectCurve Y curve");
				}
				double x = val5[0][0].x;
				double y = val6[0][0].y;
				double num2 = val5[0][0];
				double num3 = 0.0 - ((OnCurve)val).TangentAt(num2).x;
				CSGemData cSGemData2 = new CSGemData(cSGemData);
				cSGemData2.ApplyTransform(xDown.Convert());
				OnXform val7 = new OnXform();
				val7.Scale((IOn3dVector)new On3dVector(x / cSGemData.GemXMax, y / cSGemData.GemYMax, 1.0));
				cSGemData2.ApplyTransform(val7.Convert());
				if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0, out OnCurve curve, false, out string error))
				{
					return UpdateError("Curve Rail:" + error);
				}
				if (!GemOffsetCurveV1.GemOffset(cSGemData2, 0.0 - value5, out OnCurve curve2, false, out error))
				{
					return UpdateError("Curve Rail In:" + error);
				}
				On3dPoint val8 = cSGemData2.GetNurbsCurve().PointAt(cSGemData.BezelParam).Convert();
				double num4 = 0.0;
				curve.GetClosestPoint((IOn3dPoint)(object)val8, ref num4);
				On3dPoint val9 = curve.PointAt(num4);
				On3dVector val10 = OnUtil.ON_CrossProduct((IOn3dVector)(object)curve.TangentAt(num4), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0));
				val10.Unitize();
				val10 *= value5;
				double num5 = 0.0;
				curve2.GetClosestPoint((IOn3dPoint)(object)(val9 + (IOn3dVector)(object)val10), ref num5);
				OnNurbsCurve val11 = nurbsCurve.Duplicate();
				OnBoundingBox val12 = new OnBoundingBox();
				((OnGeometry)val11).GetTightBoundingBox(ref val12);
				OnXform val13 = new OnXform();
				val13.Translation(0.0 - val12.m_min.x, 0.0 - val12.m_min.y, 0.0 - val12.m_min.z);
				OnXform val14 = new OnXform();
				val14.Scale(value5 / (val12.m_max.x - val12.m_min.x), value6 / (val12.m_max.y - val12.m_min.y), 0.0);
				OnXform val15 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(value5, 0.0, 0.0), (IOn3dVector)new On3dVector(-1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)(object)val8, (IOn3dVector)(object)val10, (IOn3dVector)new On3dVector(0.0, 0.0, 1.0));
				val11.Transform((IOnXform)(object)(val14 * (IOnXform)(object)val13));
				for (int k = 0; k < val11.m_cv_count; k++)
				{
					On4dPoint val16 = new On4dPoint();
					val11.GetCV(k, ref val16);
					val16.x += num3 * val16.y;
					val11.SetCV(k, (IOn4dPoint)(object)val16);
				}
				val11.Transform((IOnXform)(object)val15);
				if (!CSUtilSweep.Sweep2(profiles: (IOnCurve[])(object)new IOnCurve[1] { (IOnCurve)val11 }, parameters1In: new double[1] { num4 }, parameters2In: new double[1] { num5 }, railCurve1: (IOnCurve)(object)curve, railCurve2: (IOnCurve)(object)curve2, breps: out var breps, closed: true, maintainHeight: true))
				{
					return UpdateError("Sweep 2");
				}
				OnXform val17 = new OnXform();
				val17.Translation(0.0, 0.0, num);
				if (breps.Length == 0)
				{
					return UpdateError("Sweep 1 rails");
				}
				((OnGeometry)breps[0]).Transform((IOnXform)(object)(xUp * (IOnXform)(object)val17));
				CSData data = new CSOnGeometryData(breps[0].Convert(), xUp.Convert() * val17.Convert(), applyTransform: false);
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		OObjects.SetData(cSDataArray);
		return base.Update();
	}
}
