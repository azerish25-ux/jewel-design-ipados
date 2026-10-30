using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("OutsideRail_V2", "{342781AD-D61B-4986-B868-046655761C54}")]
public sealed class CSOutsideRail2 : CSBlock
{
	public CSInputChannel InRingRailRadius { get; }

	public CSInputChannel InShape { get; }

	public CSInputChannel InThicknessTop { get; }

	public CSInputChannel InThicknessSide { get; }

	public CSInputChannel InThicknessBottom { get; }

	public CSInputChannel InEnableLock { get; }

	public CSOutputChannel OutOutsideRingRail { get; }

	public CSOutsideRail2(string identifier)
		: base(identifier)
	{
		InRingRailRadius = new CSInputChannel(GetPropertyName(() => InRingRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShape = new CSInputChannel(GetPropertyName(() => InShape), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InThicknessTop = new CSInputChannel(GetPropertyName(() => InThicknessTop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessSide = new CSInputChannel(GetPropertyName(() => InThicknessSide), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessBottom = new CSInputChannel(GetPropertyName(() => InThicknessBottom), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEnableLock = new CSInputChannel(GetPropertyName(() => InEnableLock), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutOutsideRingRail = new CSOutputChannel(GetPropertyName(() => OutOutsideRingRail), this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_04f4: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		double value = 3.0;
		InThicknessTop.GetDouble(ref value);
		double value2 = 2.0;
		InThicknessSide.GetDouble(ref value2);
		double value3 = 1.0;
		InThicknessBottom.GetDouble(ref value3);
		bool value4 = false;
		InEnableLock.GetBoolean(ref value4);
		double value5 = 0.0;
		if (!InRingRailRadius.GetDouble(ref value5))
		{
			OnNurbsCurve nurbsCurve = InRingRailRadius.GetNurbsCurve();
			if (nurbsCurve == null)
			{
				return UpdateMissing("Outside Ring Rail: Missing Ring Rail\n");
			}
			OnBoundingBox val = new OnBoundingBox();
			((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val);
			value5 = val.m_max.z;
		}
		double num = value5;
		double num2 = 0.0 - value5;
		double num3 = value5;
		double num4 = value5;
		OnNurbsCurve nurbsCurve2 = InShape.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Outside Ring Rail: Missing Custom Rail (No Curve)\n");
		}
		CSUtilGeometry.GetMinMax((IOnCurve)(object)nurbsCurve2, OnUtil.On_origin, 50, out var min, out var _);
		if (min == 0.0 || min == double.MaxValue)
		{
			return UpdateError("Intersect of custom Rail Failed\n");
		}
		if (num4 == 0.0)
		{
			return UpdateError("Intersect of custom Rail Failed\n");
		}
		OnNurbsCurve val2 = nurbsCurve2.Duplicate();
		val2.MakeDeformable();
		((OnGeometry)val2).Scale(num4 / min);
		CSUtilRingRail.GetRingRailPointExtents((IOnCurve)(object)val2, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var pointExtentLeft);
		if (!value4)
		{
			double num5 = pointExtentTop - pointExtentBottom;
			double num6 = num + value - pointExtentBottom;
			On3dPoint val3 = new On3dPoint(0.0, 0.0, pointExtentBottom);
			On3dPoint val4 = new On3dPoint(0.0, 0.0, pointExtentTop);
			OnXform val5 = new OnXform();
			val5.Scale((IOnPlane)new OnPlane((IOn3dPoint)(object)val3, OnUtil.On_xaxis, OnUtil.On_zaxis), 1.0, num6 / num5, 1.0);
			val2.Transform((IOnXform)(object)val5);
			CSUtilRingRail.GetRingRailPointExtents((IOnCurve)(object)val2, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft);
			val3.z = pointExtentBottom;
			val4.z = pointExtentTop;
			num5 = pointExtentTop - pointExtentBottom;
			num6 = pointExtentTop - (num2 - value3);
			OnXform val6 = new OnXform();
			val6.Scale((IOnPlane)new OnPlane((IOn3dPoint)(object)val4, OnUtil.On_xaxis, OnUtil.On_zaxis), 1.0, num6 / num5, 1.0);
			val2.Transform((IOnXform)(object)val6);
			CSUtilRingRail.GetRingRailPointExtents((IOnCurve)(object)val2, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft);
			num5 = pointExtentRight;
			num6 = num3 + value2;
			OnXform val7 = new OnXform();
			val7.Scale(num6 / num5, 1.0, 1.0);
			val2.Transform((IOnXform)(object)val7);
		}
		else
		{
			OnLineCurve val8 = new OnLineCurve((IOn3dPoint)new On3dPoint(-200.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(200.0, 0.0, 0.0));
			double num7 = pointExtentRight;
			double num8 = num3 + value2;
			OnXform val9 = new OnXform();
			val9.Scale(num8 / num7, 1.0, 1.0);
			val2.Transform((IOnXform)(object)val9);
			ArrayOnX_EVENT val10 = new ArrayOnX_EVENT();
			((OnCurve)val2).IntersectCurve((IOnCurve)(object)val8, ref val10);
			if (val10.Count() != 2)
			{
				return UpdateError("Intersect of custom Rail Failed\n");
			}
			OnNurbsCurve val11 = val2.Duplicate();
			((OnCurve)val11).ChangeClosedCurveSeam(val10[0][0]);
			OnNurbsCurve val12 = val11.Duplicate();
			((OnCurve)val12).Reverse();
			double num9 = 0.0;
			((OnCurve)val11).GetClosestPoint((IOn3dPoint)(object)val10[1][0], ref num9);
			((OnCurve)val11).Trim((IOnInterval)new OnInterval(((OnCurve)val11).Domain().Min(), num9));
			((OnCurve)val12).GetClosestPoint((IOn3dPoint)(object)val10[1][0], ref num9);
			((OnCurve)val12).Trim((IOnInterval)new OnInterval(((OnCurve)val12).Domain().Min(), num9));
			if (((OnCurve)val11).PointAt(((OnCurve)val11).Domain().Mid()).z < 0.0)
			{
				OnNurbsCurve obj = val11;
				val11 = val12;
				val12 = obj;
			}
			OnXform val13 = new OnXform();
			val13.Scale(1.0, 1.0, (value + num) / pointExtentTop);
			OnXform val14 = new OnXform();
			val14.Scale(1.0, 1.0, (0.0 - value3 + num2) / pointExtentBottom);
			val11.Transform((IOnXform)(object)val13);
			val12.Transform((IOnXform)(object)val14);
			OnCurve[] obj2 = new OnCurve[2]
			{
				(OnCurve)val11,
				(OnCurve)val12
			};
			OnCurve[] array = null;
			IOnCurve[] array2 = (IOnCurve[])(object)obj2;
			RhUtil.RhinoMergeCurves(array2, ref array);
			if (array.Length != 0)
			{
				array[0].NurbsCurve(val2);
			}
			OnLineCurve val15 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0));
			ArrayOnX_EVENT val16 = new ArrayOnX_EVENT();
			((OnCurve)val2).IntersectCurve((IOnCurve)(object)val15, ref val16);
			if (val16.Count() > 0)
			{
				num9 = val16[0][0];
			}
			else
			{
				((OnCurve)val2).GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0), ref num9);
			}
			((OnCurve)val2).ChangeClosedCurveSeam(num9);
			if (((OnCurve)val2).TangentAt(((OnCurve)val2).Domain().Min()) * OnUtil.On_xaxis < 0.0)
			{
				((OnCurve)val2).Reverse();
			}
		}
		OutOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)val2.Convert()));
		return base.Update();
	}
}
