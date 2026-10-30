using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Curves;

[CSBlock("CurveFrom2View", "{6CB790A8-EDCB-4508-8AAA-49BA74CCD4D2}")]
public sealed class CSCurveFrom2View : CSBlock
{
	public CSInputChannel iCurve1 { get; }

	public CSInputChannel iCurve2 { get; }

	public CSInputChannel iMatchCurve { get; }

	public CSOutputChannel oCurve { get; }

	public CSCurveFrom2View(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(GetPropertyName(() => iCurve1), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(GetPropertyName(() => iCurve2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iMatchCurve = new CSInputChannel(GetPropertyName(() => iMatchCurve), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve 1 is missing");
		}
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve 2 is missing");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve val2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Curve 1 is missing");
		}
		if (val2 == null)
		{
			return UpdateMissing("Curve 2 is missing");
		}
		OnPlane val3 = new OnPlane();
		OnPlane val4 = new OnPlane();
		if (!((OnCurve)val).IsPlanar(val3))
		{
			return UpdateMissing("Curve 1 is not planar");
		}
		if (!((OnCurve)val2).IsPlanar(val4))
		{
			return UpdateMissing("Curve 2 is not planar");
		}
		ValueListItem whichItem = ValueListItem.OrientCurveSecond;
		iMatchCurve.GetValueListItem(ref whichItem);
		OnBoundingBox val5 = new OnBoundingBox();
		OnBoundingBox val6 = new OnBoundingBox();
		((OnGeometry)val).GetBoundingBox(ref val5);
		((OnGeometry)val2).GetBoundingBox(ref val6);
		On3dPoint val7 = new On3dPoint();
		On3dPoint val8 = new On3dPoint();
		val5.GetFarPoint((IOnBoundingBox)(object)val6, ref val7, ref val8);
		double num = val7.DistanceTo((IOn3dPoint)(object)val8);
		On3dVector val9 = new On3dVector(val3.Normal());
		On3dVector val10 = new On3dVector(val4.Normal());
		OnNurbsCurve val11 = val.Duplicate();
		OnNurbsCurve val12 = val.Duplicate();
		OnNurbsCurve val13 = val2.Duplicate();
		OnNurbsCurve val14 = val2.Duplicate();
		((OnGeometry)val11).Translate((IOn3dVector)(object)(val9 * num));
		((OnGeometry)val13).Translate((IOn3dVector)(object)(val10 * num));
		((OnGeometry)val12).Translate((IOn3dVector)(object)(val9 * (0.0 - num)));
		((OnGeometry)val14).Translate((IOn3dVector)(object)(val10 * (0.0 - num)));
		OnNurbsSurface val15 = new OnNurbsSurface();
		val15.CreateRuledSurface((IOnCurve)(object)val11, (IOnCurve)(object)val12);
		OnNurbsSurface val16 = new OnNurbsSurface();
		val16.CreateRuledSurface((IOnCurve)(object)val13, (IOnCurve)(object)val14);
		OnCurve[] array = default(OnCurve[]);
		On3dPointArray val17 = default(On3dPointArray);
		RhUtil.RhinoIntersectSurfaces((IOnSurface)(object)val15, (IOnSurface)(object)val16, CSApplicationHelper.Application.Tolerance, ref array, ref val17);
		OnCurve[] array2 = (OnCurve[])(object)new OnCurve[1];
		IOnCurve[] array3 = (IOnCurve[])(object)array;
		RhUtil.RhinoMergeCurves(array3, ref array2);
		if (array2 == null)
		{
			return UpdateError("Error with merge");
		}
		OnLineCurve val18 = ((whichItem == ValueListItem.Unknown) ? new OnLineCurve((IOn3dPoint)(object)((OnCurve)val11).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val12).PointAtStart()) : new OnLineCurve((IOn3dPoint)(object)((OnCurve)val13).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val14).PointAtStart()));
		base.OutputChannels.Clear();
		for (int i = 0; i < array2.Length; i++)
		{
			OnCurve val19 = array2[i];
			if (val19 == null)
			{
				continue;
			}
			ArrayOnX_EVENT val20 = new ArrayOnX_EVENT();
			val19.IntersectCurve((IOnCurve)(object)val18, ref val20);
			if (val20.Count() == 1)
			{
				val19.ChangeClosedCurveSeam(val20[0][0]);
			}
			OnNurbsCurve val21 = new OnNurbsCurve();
			val19.GetNurbForm(ref val21);
			if (val21 != null)
			{
				double num2 = 0.0;
				double num3 = 0.0;
				On3dPoint val22 = ((OnCurve)val21).PointAtStart();
				((OnSurface)val15).GetClosestPoint((IOn3dPoint)(object)val22, ref num2, ref num3);
				On3dVector val23 = ((OnCurve)val21).TangentAt(((OnCurve)val21).Domain().Min());
				double num4 = 0.0;
				double num5 = 0.0;
				((OnSurface)val15).GetClosestPoint((IOn3dPoint)(object)(val22 + (IOn3dVector)(object)(val23 / 100.0)), ref num4, ref num5);
				double num6 = 0.0;
				double num7 = 0.0;
				((OnSurface)val15).GetDomain(0, ref num6, ref num7);
				double num8 = Math.Abs(num6 - num7);
				bool flag = Math.Abs(num2 - num4) > num8 * 0.9;
				if (num2 > num4 != flag)
				{
					((OnCurve)val21).Reverse();
				}
				new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData(val21.Convert()));
			}
		}
		return base.Update();
	}
}
