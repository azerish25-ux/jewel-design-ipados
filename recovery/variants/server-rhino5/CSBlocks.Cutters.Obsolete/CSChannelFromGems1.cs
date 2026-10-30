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
[CSBlock("ChannelFromGems_V1", "{5FDE6939-BFE2-421d-BB00-6720CA131F6B}")]
public sealed class CSChannelFromGems1 : CSBlock
{
	private enum EndStyle
	{
		Capped,
		Round,
		Flat,
		SharpCorner
	}

	public CSInputChannel iSortedGemArray { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iScaleXStart { get; }

	public CSInputChannel iScaleXEnd { get; }

	public CSInputChannel iScaleYStart { get; }

	public CSInputChannel iScaleYEnd { get; }

	public CSInputChannel iVOffsetStart { get; }

	public CSInputChannel iVOffsetEnd { get; }

	public CSInputChannel iStartSizeScalar { get; }

	public CSInputChannel iEndSizeScalar { get; }

	public CSInputChannel iStartTaper { get; }

	public CSInputChannel iEndTaper { get; }

	public CSInputChannel iStartEndType { get; }

	public CSInputChannel iEndEndType { get; }

	public CSInputChannel iMiddleProfileCount { get; }

	public CSInputChannel iMirror { get; }

	public CSOutputChannel oBrep { get; }

	public CSChannelFromGems1(string identifier)
		: base(identifier)
	{
		iSortedGemArray = new CSInputChannel(GetPropertyName(() => iSortedGemArray), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iScaleXStart = new CSInputChannel(GetPropertyName(() => iScaleXStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleXEnd = new CSInputChannel(GetPropertyName(() => iScaleXEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleYStart = new CSInputChannel(GetPropertyName(() => iScaleYStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleYEnd = new CSInputChannel(GetPropertyName(() => iScaleYEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iVOffsetStart = new CSInputChannel(GetPropertyName(() => iVOffsetStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iVOffsetEnd = new CSInputChannel(GetPropertyName(() => iVOffsetEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartSizeScalar = new CSInputChannel(GetPropertyName(() => iStartSizeScalar), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndSizeScalar = new CSInputChannel(GetPropertyName(() => iEndSizeScalar), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartTaper = new CSInputChannel(GetPropertyName(() => iStartTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndTaper = new CSInputChannel(GetPropertyName(() => iEndTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartEndType = new CSInputChannel(GetPropertyName(() => iStartEndType), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndEndType = new CSInputChannel(GetPropertyName(() => iEndEndType), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMiddleProfileCount = new CSInputChannel(GetPropertyName(() => iMiddleProfileCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMirror = new CSInputChannel(GetPropertyName(() => iMirror), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public static bool AdjustEndSurface(double amount, OnNurbsSurface surface)
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
		return true;
	}

	public override bool Update()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		if (!(iSortedGemArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		CSDataHelper.SetSettingType(cSDataArray, GemSettingType.CH);
		OnNurbsCurve nurbsCurve = iProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile");
		}
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val);
		OnCurve obj = CSUtilGemRails.Rail(cSDataArray);
		OnCurve val2 = ((obj != null) ? obj.DuplicateCurve() : null);
		OnCurve obj2 = CSUtilGemRails.Right(cSDataArray, val2);
		OnCurve val3 = ((obj2 != null) ? obj2.DuplicateCurve() : null);
		OnCurve obj3 = CSUtilGemRails.Left(cSDataArray, val2);
		OnCurve val4 = ((obj3 != null) ? obj3.DuplicateCurve() : null);
		if (val2 == null || val4 == null || val3 == null)
		{
			return UpdateError("Failed to get channel gem data rails");
		}
		RhUtil.RhinoExtendCurve(ref val3, (Type)0, 0, 0.0);
		RhUtil.RhinoExtendCurve(ref val2, (Type)0, 0, 0.0);
		RhUtil.RhinoExtendCurve(ref val4, (Type)0, 0, 0.0);
		RhUtil.RhinoExtendCurve(ref val3, (Type)0, 1, 0.0);
		RhUtil.RhinoExtendCurve(ref val2, (Type)0, 1, 0.0);
		RhUtil.RhinoExtendCurve(ref val4, (Type)0, 1, 0.0);
		OnXform val5 = new OnXform();
		OnXform val6 = new OnXform();
		double value = 1.0;
		double value2 = 1.0;
		iScaleXEnd.GetDouble(ref value2);
		iScaleXStart.GetDouble(ref value);
		double value3 = 1.0;
		double value4 = 1.0;
		iScaleYEnd.GetDouble(ref value4);
		iScaleYStart.GetDouble(ref value3);
		double value5 = 0.0;
		double value6 = 0.0;
		iVOffsetEnd.GetDouble(ref value6);
		iVOffsetStart.GetDouble(ref value5);
		double value7 = 1.0;
		iEndSizeScalar.GetDouble(ref value7);
		double value8 = 1.0;
		iStartSizeScalar.GetDouble(ref value8);
		double value9 = 0.0;
		iEndTaper.GetDouble(ref value9);
		double value10 = 0.0;
		iStartTaper.GetDouble(ref value10);
		int value11 = 0;
		iMiddleProfileCount.GetInteger(ref value11);
		int value12 = 0;
		iEndEndType.GetInteger(ref value12);
		int value13 = 0;
		iStartEndType.GetInteger(ref value13);
		bool value14 = false;
		iMirror.GetBoolean(ref value14);
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
		OnCurve obj4 = CSUtilGemRails.Rail(cSDataArray);
		OnCurve val12 = ((obj4 != null) ? obj4.DuplicateCurve() : null);
		OnXform val13 = new OnXform();
		val13.ChangeBasis((IOn3dPoint)(object)val12.PointAtStart(), val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val14 = nurbsCurve.Duplicate();
		val14.Transform((IOnXform)(object)(val13 * (IOnXform)(object)val7 * (IOnXform)(object)val5));
		int num = cSDataArray.Array.Count / (value11 + 2);
		if (num < 1)
		{
			num = 1;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		List<IOn3dPoint> list2 = new List<IOn3dPoint>();
		List<IOn3dPoint> list3 = new List<IOn3dPoint>();
		OnPolyline val15 = new OnPolyline();
		((ArrayOn3dPoint)val15).Append((IOn3dPoint)new On3dPoint(value, value3, 0.0));
		((ArrayOn3dPoint)val15).Append((IOn3dPoint)new On3dPoint(value2, value4, 0.0));
		OnPolyline val16 = new OnPolyline();
		((ArrayOn3dPoint)val16).Append((IOn3dPoint)new On3dPoint(0.0, value5, 0.0));
		((ArrayOn3dPoint)val16).Append((IOn3dPoint)new On3dPoint(0.0, value6, 0.0));
		for (int i = num; i < cSDataArray.Array.Count - 1; i += num)
		{
			OnXform val17 = new OnXform();
			OnXform val18 = new OnXform();
			double num2 = (double)i / (double)(cSDataArray.Array.Count - 1);
			double x = val15.PointAt(num2).x;
			double y = val15.PointAt(num2).y;
			double y2 = val16.PointAt(num2).y;
			double size = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Size;
			val18.Translation(0.0, y2, 0.0);
			val17.Scale(size / val.Max().x * x, y / val.Max().y, 0.0);
			IOn3dPoint val19 = (IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Center.Convert();
			val9 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Up.Convert();
			val11 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).UnitRightVector.Convert();
			val10 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val9);
			OnXform val20 = new OnXform();
			val20.ChangeBasis(val19, val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			OnNurbsCurve val21 = nurbsCurve.Duplicate();
			val21.Transform((IOnXform)(object)(val20 * (IOnXform)(object)val18 * (IOnXform)(object)val17));
			list.Add((IOnCurve)(object)val21);
			list2.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Right.Convert());
			list3.Add((IOn3dPoint)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, i).Left.Convert());
		}
		val9 = (IOn3dVector)(object)CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.Array.Count - 1).Up.Convert();
		val10 = (IOn3dVector)(object)val2.TangentAt(val2.Domain().Max());
		val11 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val10, val9);
		val9 = (IOn3dVector)(object)OnUtil.ON_CrossProduct(val11, val10);
		OnXform val22 = new OnXform();
		val22.ChangeBasis((IOn3dPoint)(object)val12.PointAtEnd(), val11, val9, val10, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve val23 = nurbsCurve.Duplicate();
		val23.Transform((IOnXform)(object)(val22 * (IOnXform)(object)val8 * (IOnXform)(object)val6));
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		List<IOnCurve> list6 = new List<IOnCurve>();
		OnCurve obj5 = CSUtilGemRails.Right(cSDataArray, val12);
		OnCurve val24 = ((obj5 != null) ? obj5.DuplicateCurve() : null);
		OnCurve obj6 = CSUtilGemRails.Left(cSDataArray, val12);
		OnCurve val25 = ((obj6 != null) ? obj6.DuplicateCurve() : null);
		list4.Add(val24.Domain().Min());
		list5.Add(val25.Domain().Min());
		list6.Add((IOnCurve)(object)val14);
		for (int j = 0; j < list.Count; j++)
		{
			double item = 0.0;
			val24.GetClosestPoint(list2[j], ref item);
			list4.Add(item);
			val25.GetClosestPoint(list3[j], ref item);
			list5.Add(item);
			list6.Add(list[j]);
		}
		bool flag = CSUtilGemRails.IsClosed(cSDataArray.Array);
		if (!flag)
		{
			list6.Add((IOnCurve)(object)val23);
			list4.Add(val24.Domain().Max());
			list5.Add(val25.Domain().Max());
		}
		CSUtilSweep.Sweep2((IOnCurve)(object)val24, (IOnCurve)(object)val25, list6.ToArray(), list4.ToArray(), list5.ToArray(), out var breps, flag);
		List<OnBrep> list7 = new List<OnBrep>(breps);
		if (!flag)
		{
			OnBrep val26 = BuildEnd(nurbsCurve, val22 * (IOnXform)(object)val8, val6, value2, reverse: false, value7, value7 * CSUtilGemRails.SizeEnd(cSDataArray), value9, (EndStyle)value12);
			OnBrep val27 = BuildEnd(nurbsCurve, val13 * (IOnXform)(object)val7, val5, value, reverse: true, value8, value8 * CSUtilGemRails.SizeStart(cSDataArray), value10, (EndStyle)value13);
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
			oBrep.SetData(new CSOnGeometryData(val28.Convert()));
		}
		return base.Update();
	}

	private static OnBrep BuildEnd(OnNurbsCurve profile, OnXform xform, OnXform scale, double scaleX, bool reverse, double endSizeScalar, double endSize, double endTaper, EndStyle endStyle)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		OnXform val = xform * (IOnXform)(object)scale;
		if (endSize < 0.01)
		{
			endStyle = EndStyle.Capped;
		}
		OnCurve leftSide = null;
		OnCurve rightSide = null;
		if (!CSUtilGeometry.SplitProfileInHalf((IOnCurve)(object)profile, ref leftSide, ref rightSide))
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
			OnXform val13 = xform * (IOnXform)(object)val12 * (IOnXform)(object)scale * (IOnXform)(object)val9;
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
			OnXform val17 = xform * (IOnXform)(object)val16 * (IOnXform)(object)scale * (IOnXform)(object)val9;
			val15.Transform((IOnXform)(object)val17);
			((OnCurve)val14).Reverse();
			((OnCurve)val15).Reverse();
			((OnCurve)val15).SetStartPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart());
			((OnCurve)val14).SetStartPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart());
			((OnCurve)val15).SetEndPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd());
			((OnCurve)val14).SetEndPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd());
			list.Add((IOnCurve)(object)val15);
			list.Add((IOnCurve)(object)val14);
			OnBrep brep = CSUtilSweep.Loft(list.ToArray(), (eLoftType)3);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			return brep;
		}
		case EndStyle.Round:
		{
			OnNurbsCurve obj = leftSide.NurbsCurve();
			val2.Duplicate();
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
				val7.Transform((IOnXform)(object)(xform * (IOnXform)(object)scale * (IOnXform)(object)val8));
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
			val19.Transform((IOnXform)(object)(xform * (IOnXform)(object)val20 * (IOnXform)(object)scale * (IOnXform)(object)val21));
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
