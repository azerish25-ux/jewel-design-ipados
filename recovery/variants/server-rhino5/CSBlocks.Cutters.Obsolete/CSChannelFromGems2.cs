using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("ChannelFromGems_V2", "{AB2D01E0-7AAD-4749-8743-CC0DA116DDC3}")]
public sealed class CSChannelFromGems2 : CSBlock
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

	public CSInputChannel InScaleXStart { get; }

	public CSInputChannel InScaleXEnd { get; }

	public CSInputChannel InScaleYStart { get; }

	public CSInputChannel InScaleYEnd { get; }

	public CSInputChannel InVOffsetStart { get; }

	public CSInputChannel InVOffsetEnd { get; }

	public CSInputChannel InStartSizeScalar { get; }

	public CSInputChannel InEndSizeScalar { get; }

	public CSInputChannel InStartTaper { get; }

	public CSInputChannel InEndTaper { get; }

	public CSInputChannel InStartEndType { get; }

	public CSInputChannel InEndEndType { get; }

	public CSInputChannel InMiddleProfileCount { get; }

	public CSInputChannel InMirror { get; }

	public CSOutputChannel OutBrep { get; }

	public CSChannelFromGems2(string identifier)
		: base(identifier)
	{
		InSortedGemArray = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InScaleXStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleXEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleYStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleYEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartSizeScalar = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSizeScalar = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleProfileCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Expected O, but got Unknown
		if (!(InSortedGemArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems Count 0");
		}
		CSDataHelper.SetSettingType(cSDataArray, GemSettingType.CH);
		OnNurbsCurve nurbsCurve = InProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile");
		}
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val);
		OnCurve val2 = CSUtilGemRails.Rail(cSDataArray.Array).DuplicateCurve();
		OnCurve val3 = CSUtilGemRails.Right(cSDataArray, val2).DuplicateCurve();
		OnCurve val4 = CSUtilGemRails.Left(cSDataArray, val2).DuplicateCurve();
		if (val3 == null || val4 == null)
		{
			return UpdateError("Failed Duplicate");
		}
		double num = 0.0;
		double num2 = 0.0;
		RhUtil.RhinoExtendCurve(ref val3, (Type)0, 0, num);
		RhUtil.RhinoExtendCurve(ref val2, (Type)0, 0, num);
		RhUtil.RhinoExtendCurve(ref val4, (Type)0, 0, num);
		RhUtil.RhinoExtendCurve(ref val3, (Type)0, 1, num2);
		RhUtil.RhinoExtendCurve(ref val2, (Type)0, 1, num2);
		RhUtil.RhinoExtendCurve(ref val4, (Type)0, 1, num2);
		OnXform val5 = new OnXform();
		OnXform val6 = new OnXform();
		double value = 1.0;
		double value2 = 1.0;
		InScaleXEnd.GetDouble(ref value2);
		InScaleXStart.GetDouble(ref value);
		double value3 = 1.0;
		double value4 = 1.0;
		InScaleYEnd.GetDouble(ref value4);
		InScaleYStart.GetDouble(ref value3);
		double value5 = 0.0;
		double value6 = 0.0;
		InVOffsetEnd.GetDouble(ref value6);
		InVOffsetStart.GetDouble(ref value5);
		double value7 = 1.0;
		InEndSizeScalar.GetDouble(ref value7);
		double value8 = 1.0;
		InStartSizeScalar.GetDouble(ref value8);
		double value9 = 0.0;
		InEndTaper.GetDouble(ref value9);
		double value10 = 0.0;
		InStartTaper.GetDouble(ref value10);
		int value11 = 0;
		InMiddleProfileCount.GetInteger(ref value11);
		int value12 = 0;
		InEndEndType.GetInteger(ref value12);
		int value13 = 0;
		InStartEndType.GetInteger(ref value13);
		bool value14 = false;
		InMirror.GetBoolean(ref value14);
		if (!value14)
		{
			value2 = value;
			value4 = value3;
			value6 = value5;
			value7 = value8;
			value9 = value10;
			value12 = value13;
		}
		val5.Scale(CSUtilGemRails.SizeStart(cSDataArray) / val.Max().x * value, value3 / val.Max().y, 1.0);
		val6.Scale(CSUtilGemRails.SizeEnd(cSDataArray) / val.Max().x * value2, value4 / val.Max().y, 1.0);
		OnXform val7 = new OnXform();
		OnXform val8 = new OnXform();
		val7.Translation(0.0, value5, 0.0);
		val8.Translation(0.0, value6, 0.0);
		IOn3dVector val9 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Up.Convert();
		IOn3dVector val10 = (IOn3dVector)(object)val2.TangentAt(val2.Domain().Min());
		IOn3dVector val11 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val10, val9);
		val9 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val10);
		OnCurve val12 = CSUtilGemRails.Rail(cSDataArray.Array).DuplicateCurve();
		OnCurve val13 = CSUtilGemRails.Right(cSDataArray, val12).DuplicateCurve();
		OnCurve val14 = CSUtilGemRails.Left(cSDataArray, val12).DuplicateCurve();
		OnXform val15 = new OnXform();
		val15.ChangeBasis((IOn3dPoint)(object)val12.PointAtStart(), val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val16 = nurbsCurve.Duplicate();
		val16.Transform((IOnXform)(object)(val15 * (IOnXform)(object)val7 * (IOnXform)(object)val5));
		int num3 = (cSDataArray.Array.Count - 1) / (value11 + 1);
		if (num3 < 1)
		{
			num3 = 1;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		List<IOn3dPoint> list2 = new List<IOn3dPoint>();
		List<IOn3dPoint> list3 = new List<IOn3dPoint>();
		OnPolyline val17 = new OnPolyline();
		((ArrayOn3dPoint)val17).Append((IOn3dPoint)new On3dPoint(value, value3, 0.0));
		((ArrayOn3dPoint)val17).Append((IOn3dPoint)new On3dPoint(value2, value4, 0.0));
		OnPolyline val18 = new OnPolyline();
		((ArrayOn3dPoint)val18).Append((IOn3dPoint)new On3dPoint(0.0, value5, 0.0));
		((ArrayOn3dPoint)val18).Append((IOn3dPoint)new On3dPoint(0.0, value6, 0.0));
		for (int i = num3; i < cSDataArray.Array.Count - 1; i += num3)
		{
			OnXform val19 = new OnXform();
			OnXform val20 = new OnXform();
			double num4 = (double)i / (double)(cSDataArray.Array.Count - 1);
			double x = val17.PointAt(num4).x;
			double y = val17.PointAt(num4).y;
			double y2 = val18.PointAt(num4).y;
			double size = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Size;
			val20.Translation(0.0, y2, 0.0);
			val19.Scale(size / val.Max().x * x, y / val.Max().y, 0.0);
			IOn3dPoint val21 = (IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Center.Convert();
			val9 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Up.Convert();
			val11 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).UnitRightVector.Convert();
			val10 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val9);
			OnXform val22 = new OnXform();
			val22.ChangeBasis(val21, val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			OnNurbsCurve val23 = nurbsCurve.Duplicate();
			val23.Transform((IOnXform)(object)(val22 * (IOnXform)(object)val20 * (IOnXform)(object)val19));
			list.Add((IOnCurve)(object)val23);
			list2.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Right.Convert());
			list3.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Left.Convert());
		}
		val9 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.Array.Count - 1).Up.Convert();
		val10 = (IOn3dVector)(object)val2.TangentAt(val2.Domain().Max());
		val11 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val10, val9);
		val9 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val10);
		OnXform val24 = new OnXform();
		val24.ChangeBasis((IOn3dPoint)(object)val12.PointAtEnd(), val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val25 = nurbsCurve.Duplicate();
		val25.Transform((IOnXform)(object)(val24 * (IOnXform)(object)val8 * (IOnXform)(object)val6));
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		List<IOnCurve> list6 = new List<IOnCurve>();
		list4.Add(val13.Domain().Min());
		list5.Add(val14.Domain().Min());
		list6.Add((IOnCurve)(object)val16);
		for (int j = 0; j < list.Count; j++)
		{
			double item = 0.0;
			val13.GetClosestPoint(list2[j], ref item);
			list4.Add(item);
			val14.GetClosestPoint(list3[j], ref item);
			list5.Add(item);
			list6.Add(list[j]);
		}
		bool num5 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		if (!num5)
		{
			list6.Add((IOnCurve)(object)val25);
			list4.Add(val13.Domain().Max());
			list5.Add(val14.Domain().Max());
		}
		CSUtilSweep.Sweep2((IOnCurve)(object)val13, (IOnCurve)(object)val14, list6.ToArray(), list4.ToArray(), list5.ToArray(), out var breps);
		List<OnBrep> list7 = new List<OnBrep>(breps);
		if (!num5)
		{
			OnBrep val26 = BuildEnd((IOnCurve)(object)nurbsCurve, val24 * (IOnXform)(object)val8, (IOnXform)(object)val6, value2, reverse: false, value7, value7 * CSUtilGemRails.SizeEnd(cSDataArray), value9, (EndStyle)value12);
			OnBrep val27 = BuildEnd((IOnCurve)(object)nurbsCurve, val15 * (IOnXform)(object)val7, (IOnXform)(object)val5, value, reverse: true, value8, value8 * CSUtilGemRails.SizeStart(cSDataArray), value10, (EndStyle)value13);
			if (val26 != null)
			{
				list7.Add(val26);
			}
			if (val27 != null)
			{
				list7.Add(val27);
			}
		}
		OnBrep val28 = CSUtilRhino.JoinAllBreps(list7.ToArray(), 0.01);
		if (val28 != null)
		{
			OnBrep val29 = CSUtilRhino.CapPlanarHoles(val28, 0.01);
			if (val29 != null)
			{
				val28 = val29;
			}
		}
		if (val28 != null)
		{
			On3dPoint brepPoint = new On3dPoint();
			On3dVector brepNormal = new On3dVector();
			CSUtilGeometry.GetClosestPointToBrep((IOnBrep)(object)val28, (IOn3dPoint)(object)val2.PointAtStart(), ref brepPoint, ref brepNormal);
			if (new On3dVector((IOn3dVector)(object)(brepPoint - (IOn3dPoint)(object)val2.PointAtStart())) * (IOn3dVector)(object)brepNormal < 0.0)
			{
				val28.Flip();
			}
		}
		if (val28 != null)
		{
			OutBrep.SetData(new CSOnGeometryData(val28.Convert()));
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
