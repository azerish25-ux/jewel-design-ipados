using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters;

[CSBlock("ChannelFromGemsNew", "{9916478A-DC9C-4BAE-AC66-ECDCBDADF1F1}")]
public sealed class CSChannelFromGemsNew : CSBlock
{
	private enum EndStyle
	{
		Capped,
		Round,
		Flat,
		SharpCorner
	}

	public CSInputChannel InSortedGemArray { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InOffsetXStart { get; }

	public CSInputChannel InOffsetXEnd { get; }

	public CSInputChannel InOffsetYStart { get; }

	public CSInputChannel InOffsetYEnd { get; }

	public CSInputChannel InVOffsetStart { get; }

	public CSInputChannel InVOffsetEnd { get; }

	public CSInputChannel InStartSizeOffset { get; }

	public CSInputChannel InEndSizeOffset { get; }

	public CSInputChannel InStartTaper { get; }

	public CSInputChannel InEndTaper { get; }

	public CSInputChannel InStartEndType { get; }

	public CSInputChannel InEndEndType { get; }

	public CSInputChannel InMiddleProfileCount { get; }

	public CSInputChannel InMirror { get; }

	public CSInputChannel InFollowCurve { get; }

	public CSInputChannel InProngOnOff { get; }

	public CSInputChannel InProfileProng { get; }

	public CSInputChannel InOffsetXStartProng { get; }

	public CSInputChannel InOffsetXEndProng { get; }

	public CSInputChannel InOffsetYStartProng { get; }

	public CSInputChannel InOffsetYEndProng { get; }

	public CSInputChannel InStartSizeOffsetProng { get; }

	public CSInputChannel InEndSizeOffsetProng { get; }

	public CSInputChannel InStartTaperProng { get; }

	public CSInputChannel InEndTaperProng { get; }

	public CSInputChannel InStartTypeProng { get; }

	public CSInputChannel InEndTypeProng { get; }

	public CSOutputChannel OutBrep { get; }

	public CSChannelFromGemsNew(string identifier)
		: base(identifier)
	{
		InSortedGemArray = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOffsetXStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetXEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetYStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetYEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartSizeOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSizeOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleProfileCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InFollowCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProngOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProfileProng = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOffsetXStartProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetXEndProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetYStartProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffsetYEndProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartSizeOffsetProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSizeOffsetProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartTaperProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Any);
		InEndTaperProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Any);
		InStartTypeProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndTypeProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Expected O, but got Unknown
		if (!(InSortedGemArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		CSGemData cSGemData = cSDataArray.Array[0] as CSGemData;
		CSGemData cSGemData2 = cSDataArray.Array[cSDataArray.Array.Count - 1] as CSGemData;
		if (cSGemData == null || cSGemData2 == null)
		{
			return UpdateMissing("Missing Gems");
		}
		bool value = false;
		InProngOnOff.GetBoolean(ref value);
		OnNurbsCurve val = (value ? InProfileProng.GetNurbsCurve() : InProfile.GetNurbsCurve());
		if (val == null)
		{
			return UpdateMissing("Profile");
		}
		OnBoundingBox val2 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val2);
		OnCurve obj = CSUtilGemRails.Rail(cSDataArray);
		OnCurve val3 = ((obj != null) ? obj.DuplicateCurve() : null);
		OnCurve obj2 = CSUtilGemRails.Right(cSDataArray, val3);
		OnCurve val4 = ((obj2 != null) ? obj2.DuplicateCurve() : null);
		OnCurve obj3 = CSUtilGemRails.Left(cSDataArray, val3);
		OnCurve val5 = ((obj3 != null) ? obj3.DuplicateCurve() : null);
		if (val4 == null || val5 == null)
		{
			return UpdateError("Failed Duplicate");
		}
		OnXform val6 = new OnXform();
		OnXform val7 = new OnXform();
		double num = 1.0;
		double num2 = 1.0;
		double value2 = 0.0;
		double value3 = 0.0;
		if (value ? InOffsetXEndProng.GetDouble(ref value2) : InOffsetXEnd.GetDouble(ref value2))
		{
			num2 = (cSGemData2.GemXSize + value2) / cSGemData2.GemXSize;
		}
		if (value ? InOffsetXStartProng.GetDouble(ref value3) : InOffsetXStart.GetDouble(ref value3))
		{
			num = (cSGemData.GemXSize + value3) / cSGemData.GemXSize;
		}
		double num3 = 1.0;
		double num4 = 1.0;
		double value4 = 1.0;
		double value5 = 1.0;
		if (value ? InOffsetYEndProng.GetDouble(ref value5) : InOffsetYEnd.GetDouble(ref value5))
		{
			num4 = (cSGemData2.GemZSize + value5) / cSGemData2.GemZSize;
		}
		if (value ? InOffsetYStartProng.GetDouble(ref value4) : InOffsetYStart.GetDouble(ref value4))
		{
			num3 = (cSGemData.GemZSize + value4) / cSGemData.GemZSize;
		}
		double value6 = 0.0;
		double value7 = 0.0;
		InVOffsetEnd.GetDouble(ref value7);
		InVOffsetStart.GetDouble(ref value6);
		double num5 = 1.0;
		double value8 = 0.0;
		if (value ? InEndSizeOffsetProng.GetDouble(ref value8) : InEndSizeOffset.GetDouble(ref value8))
		{
			num5 = (cSGemData2.GemYSize + value8) / cSGemData2.GemYSize;
		}
		double num6 = 1.0;
		double value9 = 0.0;
		if (value ? InStartSizeOffsetProng.GetDouble(ref value9) : InStartSizeOffset.GetDouble(ref value9))
		{
			num6 = (cSGemData.GemYSize + value9) / cSGemData.GemYSize;
		}
		double value10 = 0.0;
		InEndTaper.GetDouble(ref value10);
		double value11 = 0.0;
		InStartTaper.GetDouble(ref value11);
		int value12 = 0;
		InMiddleProfileCount.GetInteger(ref value12);
		int value13 = 0;
		if (value)
		{
			InEndTypeProng.GetInteger(ref value13);
		}
		else
		{
			InEndEndType.GetInteger(ref value13);
		}
		int value14 = 0;
		if (value)
		{
			InStartTypeProng.GetInteger(ref value14);
		}
		else
		{
			InStartEndType.GetInteger(ref value14);
		}
		bool value15 = false;
		InMirror.GetBoolean(ref value15);
		if (!value15)
		{
			num2 = num;
			num4 = num3;
			value7 = value6;
			num5 = num6;
			value10 = value11;
			value13 = value14;
		}
		val6.Scale(CSUtilGemRails.SizeStart(cSDataArray) / val2.Max().x * num, num3 / val2.Max().y, 1.0);
		val7.Scale(CSUtilGemRails.SizeEnd(cSDataArray) / val2.Max().x * num2, num4 / val2.Max().y, 1.0);
		OnXform val8 = new OnXform();
		OnXform val9 = new OnXform();
		val8.Translation(0.0, value6, 0.0);
		val9.Translation(0.0, value7, 0.0);
		IOn3dVector val10 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Up.Convert();
		IOn3dVector val11 = (IOn3dVector)(object)val3.TangentAt(val3.Domain().Min());
		IOn3dVector val12 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val10);
		val10 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val12, val11);
		OnXform val13 = new OnXform();
		val13.ChangeBasis((IOn3dPoint)(object)val3.PointAtStart(), val12, val10, val11, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val14 = val.Duplicate();
		val14.Transform((IOnXform)(object)(val13 * (IOnXform)(object)val8 * (IOnXform)(object)val6));
		int num7 = (cSDataArray.Array.Count - 1) / (value12 + 1);
		if (num7 < 1)
		{
			num7 = 1;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		List<IOn3dPoint> list2 = new List<IOn3dPoint>();
		List<IOn3dPoint> list3 = new List<IOn3dPoint>();
		OnPolyline val15 = new OnPolyline();
		((ArrayOn3dPoint)val15).Append((IOn3dPoint)new On3dPoint(num, num3, 0.0));
		((ArrayOn3dPoint)val15).Append((IOn3dPoint)new On3dPoint(num2, num4, 0.0));
		OnPolyline val16 = new OnPolyline();
		((ArrayOn3dPoint)val16).Append((IOn3dPoint)new On3dPoint(0.0, value6, 0.0));
		((ArrayOn3dPoint)val16).Append((IOn3dPoint)new On3dPoint(0.0, value7, 0.0));
		for (int i = num7; i < cSDataArray.Array.Count - 1; i += num7)
		{
			OnXform val17 = new OnXform();
			OnXform val18 = new OnXform();
			double num8 = (double)i / (double)(cSDataArray.Array.Count - 1);
			double x = val15.PointAt(num8).x;
			double y = val15.PointAt(num8).y;
			double y2 = val16.PointAt(num8).y;
			double size = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Size;
			val18.Translation(0.0, y2, 0.0);
			val17.Scale(size / val2.Max().x * x, y / val2.Max().y, 0.0);
			IOn3dPoint val19 = (IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Center.Convert();
			val10 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Up.Convert();
			val12 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).UnitRightVector.Convert();
			val11 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val12, val10);
			OnXform val20 = new OnXform();
			val20.ChangeBasis(val19, val12, val10, val11, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			OnNurbsCurve val21 = val.Duplicate();
			val21.Transform((IOnXform)(object)(val20 * (IOnXform)(object)val18 * (IOnXform)(object)val17));
			list.Add((IOnCurve)(object)val21);
			list2.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Right.Convert());
			list3.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Left.Convert());
		}
		val10 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.Array.Count - 1).Up.Convert();
		val11 = (IOn3dVector)(object)val3.TangentAt(val3.Domain().Max());
		val12 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val10);
		val10 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val12, val11);
		OnXform val22 = new OnXform();
		val22.ChangeBasis((IOn3dPoint)(object)val3.PointAtEnd(), val12, val10, val11, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val23 = val.Duplicate();
		val23.Transform((IOnXform)(object)(val22 * (IOnXform)(object)val9 * (IOnXform)(object)val7));
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		List<IOnCurve> list6 = new List<IOnCurve>();
		list4.Add(val4.Domain().Min());
		list5.Add(val5.Domain().Min());
		list6.Add((IOnCurve)(object)val14);
		for (int j = 0; j < list.Count; j++)
		{
			double item = 0.0;
			val4.GetClosestPoint(list2[j], ref item);
			list4.Add(item);
			val5.GetClosestPoint(list3[j], ref item);
			list5.Add(item);
			list6.Add(list[j]);
		}
		bool flag = CSUtilGemRails.IsClosed(cSDataArray.Array);
		if (!flag)
		{
			list6.Add((IOnCurve)(object)val23);
			list4.Add(val4.Domain().Max());
			list5.Add(val5.Domain().Max());
		}
		CSUtilSweep.Sweep2((IOnCurve)(object)val4, (IOnCurve)(object)val5, list6.ToArray(), list4.ToArray(), list5.ToArray(), out var breps, flag);
		List<OnBrep> list7 = new List<OnBrep>(breps);
		if (!flag)
		{
			OnBrep val24 = BuildEnd((IOnCurve)(object)val, val22 * (IOnXform)(object)val9, (IOnXform)(object)val7, num2, reverse: false, num5, num5 * CSUtilGemRails.SizeEnd(cSDataArray), value10, (EndStyle)value13);
			OnBrep val25 = BuildEnd((IOnCurve)(object)val, val13 * (IOnXform)(object)val8, (IOnXform)(object)val6, num, reverse: true, num6, num6 * CSUtilGemRails.SizeStart(cSDataArray), value11, (EndStyle)value14);
			if (val24 != null)
			{
				list7.Add(val24);
			}
			if (val25 != null)
			{
				list7.Add(val25);
			}
		}
		OnBrep val26 = CSUtilRhino.JoinAllBreps(list7.ToArray(), 0.01);
		if (val26 != null)
		{
			OnBrep val27 = CSUtilRhino.CapPlanarHoles(val26, 0.01);
			if (val27 != null)
			{
				val26 = val27;
			}
		}
		if (val26 != null)
		{
			On3dPoint brepPoint = new On3dPoint();
			On3dVector brepNormal = new On3dVector();
			CSUtilGeometry.GetClosestPointToBrep((IOnBrep)(object)val26, (IOn3dPoint)(object)val3.PointAtStart(), ref brepPoint, ref brepNormal);
			if (new On3dVector((IOn3dVector)(object)(brepPoint - (IOn3dPoint)(object)val3.PointAtStart())) * (IOn3dVector)(object)brepNormal < 0.0)
			{
				val26.Flip();
			}
		}
		if (val26 != null)
		{
			OutBrep.SetData(new CSOnGeometryData(val26.Convert()));
		}
		return base.Update();
	}

	private static void AdjustEndSurface(double amount, OnNurbsSurface surface)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		OnBoundingBox val = ((OnGeometry)surface).BoundingBox();
		On4dPoint val2 = new On4dPoint();
		for (int i = 0; i < surface.CVCount(0); i++)
		{
			for (int j = 0; j < surface.CVCount(1); j++)
			{
				surface.GetCV(i, j, ref val2);
				double w = val2.w;
				On4dPoint obj = val2;
				obj.y /= val2.w;
				On4dPoint obj2 = val2;
				obj2.x /= val2.w;
				On4dPoint obj3 = val2;
				obj3.z /= val2.w;
				val2.w = 1.0;
				OnXform val3 = new OnXform();
				double num = Math.Sin((val2.y - val.Min().y) / (val.Max().y - val.Min().y) * OnUtil.On_PI / 2.0);
				val3.Scale(1.0, 1.0, 1.0 * (1.0 - amount) + num * amount);
				if (Math.Abs(val2.z - val.Min().z) > 0.1)
				{
					val2.Transform((IOnXform)(object)val3);
				}
				val2.w = w;
				On4dPoint obj4 = val2;
				obj4.x *= val2.w;
				On4dPoint obj5 = val2;
				obj5.y *= val2.w;
				On4dPoint obj6 = val2;
				obj6.z *= val2.w;
				surface.SetCV(i, j, (IOn4dPoint)(object)val2);
			}
		}
	}

	private static OnBrep BuildEnd(IOnCurve profile, OnXform xform, IOnXform scale, double scaleX, bool reverse, double endSizeScalar, double endSize, double endTaper, EndStyle endStyle)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03fe: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		OnXform val = xform * scale;
		if (endSize < 0.01)
		{
			endStyle = EndStyle.Capped;
		}
		OnCurve leftSide = null;
		OnCurve rightSide = null;
		if (!CSUtilGeometry.SplitProfileInHalf(profile, ref leftSide, ref rightSide))
		{
			return null;
		}
		if (leftSide == null || rightSide == null)
		{
			return null;
		}
		OnNurbsCurve val2 = CSUtilGeometry.ChannelAdjustEndProfile(leftSide, endTaper);
		OnNurbsCurve val3 = CSUtilGeometry.ChannelAdjustEndProfile(rightSide, endTaper);
		if (endSize < 0.01)
		{
			endStyle = EndStyle.Flat;
		}
		switch (endStyle)
		{
		case EndStyle.Flat:
		{
			List<IOnCurve> list = new List<IOnCurve>();
			double num = Math.Atan(endSizeScalar / scaleX);
			OnXform val9 = new OnXform();
			val9.Scale(1.0 / Math.Cos(num), 1.0, 1.0);
			OnNurbsCurve val10 = leftSide.NurbsCurve();
			OnNurbsCurve val11 = val2.Duplicate();
			val10.Transform((IOnXform)(object)val);
			OnXform val12 = new OnXform();
			if (!reverse)
			{
				val12.Rotation(num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				val12.Rotation(0.0 - num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			OnXform val13 = xform * (IOnXform)(object)val12 * scale * (IOnXform)(object)val9;
			val11.Transform((IOnXform)(object)val13);
			((OnCurve)val11).SetStartPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart());
			((OnCurve)val11).SetEndPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd());
			list.Add((IOnCurve)(object)val10);
			list.Add((IOnCurve)(object)val11);
			OnNurbsCurve val14 = rightSide.NurbsCurve();
			OnNurbsCurve val15 = val3.Duplicate();
			val14.Transform((IOnXform)(object)val);
			OnXform val16 = new OnXform();
			if (!reverse)
			{
				val16.Rotation(0.0 - num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				val16.Rotation(num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			OnXform val17 = xform * (IOnXform)(object)val16 * scale * (IOnXform)(object)val9;
			val15.Transform((IOnXform)(object)val17);
			((OnCurve)val14).Reverse();
			((OnCurve)val15).Reverse();
			((OnCurve)val15).SetStartPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart());
			((OnCurve)val14).SetStartPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart());
			((OnCurve)val15).SetEndPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd());
			((OnCurve)val14).SetEndPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd());
			list.Add((IOnCurve)(object)val15);
			list.Add((IOnCurve)(object)val14);
			return CSUtilSweep.Loft(list.ToArray(), (eLoftType)3);
		}
		case EndStyle.Round:
		{
			OnNurbsCurve obj = leftSide.NurbsCurve();
			OnXform val4 = new OnXform();
			if (!reverse)
			{
				val4.Rotation(OnUtil.On_PI / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				val4.Rotation((0.0 - OnUtil.On_PI) / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			new OnXform().Scale(endSize / 2.0, 1.0, 1.0);
			OnLine val5 = new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 1.0, 0.0));
			OnInterval val6 = new OnInterval(0.0, OnUtil.On_PI);
			OnNurbsSurface val7 = RhUtil.RhinoRevolve((OnCurve)(object)obj, ref val5, ref val6).NurbsSurface();
			OnXform val8 = new OnXform();
			if (!reverse)
			{
				val8.Scale(1.0, 1.0, endSize / 4.0);
			}
			else
			{
				val8.Scale(1.0, 1.0, (0.0 - endSize) / 4.0);
			}
			if (val7 != null)
			{
				AdjustEndSurface(endTaper, val7);
				val7.Transform((IOnXform)(object)(xform * scale * (IOnXform)(object)val8));
				return ((OnGeometry)val7).BrepForm();
			}
			break;
		}
		}
		if (endStyle == EndStyle.SharpCorner)
		{
			OnNurbsCurve val18 = leftSide.NurbsCurve();
			val18.Transform((IOnXform)(object)val);
			OnNurbsCurve val19 = val2.Duplicate();
			double num2 = (reverse ? CSBlockUtil.CSUtilMath.ToRadians(-90.0) : CSBlockUtil.CSUtilMath.ToRadians(90.0));
			OnXform val20 = new OnXform();
			val20.Rotation(num2, (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0));
			double num3 = endSizeScalar / scaleX;
			OnXform val21 = new OnXform();
			val21.Scale(num3, 1.0, 1.0);
			val19.Transform((IOnXform)(object)(xform * (IOnXform)(object)val20 * scale * (IOnXform)(object)val21));
			List<IOnCurve> obj2 = new List<IOnCurve>
			{
				(IOnCurve)(object)val18,
				(IOnCurve)(object)val19
			};
			OnNurbsCurve val22 = rightSide.NurbsCurve();
			val22.Transform((IOnXform)(object)val);
			((OnCurve)val22).Reverse();
			if (((OnCurve)val22).PointAtStart().DistanceTo((IOn3dPoint)(object)((OnCurve)val18).PointAtStart()) > 0.01)
			{
				((OnCurve)val22).Reverse();
			}
			obj2.Add((IOnCurve)(object)val22);
			return CSUtilSweep.Loft(obj2.ToArray(), (eLoftType)3);
		}
		return null;
	}
}
