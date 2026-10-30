using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterEmerald_V1", "{4FC13BF8-59F2-47af-82C6-ADDB5179B07D}")]
public sealed class CSClusterEmerald1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iWidth { get; }

	public CSInputChannel iCorner { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iNudge { get; }

	public CSInputChannel iProfileHeight { get; }

	public CSInputChannel iProfileWidth { get; }

	public CSInputChannel iDepth { get; }

	public CSInputChannel iOffset { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iSplitMiddle { get; }

	public CSOutputChannel oMetal { get; }

	public CSOutputChannel oGems { get; }

	public CSClusterEmerald1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(GetPropertyName(() => iWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCorner = new CSInputChannel(GetPropertyName(() => iCorner), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iNudge = new CSInputChannel(GetPropertyName(() => iNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileHeight = new CSInputChannel(GetPropertyName(() => iProfileHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(GetPropertyName(() => iProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDepth = new CSInputChannel(GetPropertyName(() => iDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSplitMiddle = new CSInputChannel(GetPropertyName(() => iSplitMiddle), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
		oGems = new CSOutputChannel(GetPropertyName(() => oGems), this, ChannelDataType.Gem, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		OnNurbsCurve sideProfile = null;
		CSGemData cSGemData = null;
		cSGemData = iGemDefinition.Data as CSGemData;
		double value = 1.0;
		double value2 = 1.0;
		double value3 = 0.0;
		double value4 = 0.0;
		double value5 = 0.5;
		double value6 = 0.5;
		double value7 = 0.5;
		double value8 = 1.0;
		double value9 = 0.0;
		bool value10 = false;
		if (cSGemData == null)
		{
			return UpdateMissing("Cluster Emerald: Missing Gem");
		}
		iHeight.GetDouble(ref value);
		iWidth.GetDouble(ref value2);
		iAngle.GetDouble(ref value3);
		iNudge.GetDouble(ref value4);
		iProfileHeight.GetDouble(ref value5);
		iProfileWidth.GetDouble(ref value6);
		iOffset.GetDouble(ref value7);
		iSplitMiddle.GetBoolean(ref value10);
		iDepth.GetDouble(ref value8);
		iTaper.GetDouble(ref value9);
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			cSGemData2.GetGemXformRC(out var _, out var xUp);
			double num = cSGemData2.GemXSize / 2.0;
			double num2 = cSGemData2.GemYSize / 2.0;
			double value11 = Math.Min(num, num2) / 8.0;
			iCorner?.GetDouble(ref value11);
			double num3 = value + num2;
			double num4 = value2 + num;
			On3dVector val = new On3dVector(num4, num3, 0.0);
			On3dVector val2 = new On3dVector(num4, 0.0 - num3, 0.0);
			On3dVector val3 = new On3dVector(value11, 0.0, 0.0);
			On3dVector val4 = new On3dVector(0.0, value11, 0.0);
			OnPolyline val5 = new OnPolyline();
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(val - (IOn3dVector)(object)val3)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(val - (IOn3dVector)(object)val4)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(val2 + (IOn3dVector)(object)val4)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(val2 - (IOn3dVector)(object)val3)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(-val + (IOn3dVector)(object)val3)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(-val + (IOn3dVector)(object)val4)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(-val2 - (IOn3dVector)(object)val4)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(-val2 + (IOn3dVector)(object)val3)));
			((ArrayOn3dPoint)val5).Append((IOn3dPoint)new On3dPoint((IOn3dVector)(object)(val - (IOn3dVector)(object)val3)));
			List<OnCircle> list = new List<OnCircle>();
			List<OnCircle> list2 = new List<OnCircle>();
			List<OnCircle> list3 = new List<OnCircle>();
			List<OnCircle> list4 = new List<OnCircle>();
			double num5 = ((cSGemData.GemXSize > cSGemData.GemYSize) ? cSGemData.GemXSize : cSGemData.GemYSize);
			double num6 = LayoutStones(new On3dPoint(0.0, num3, 0.0), ((ArrayOn3dPoint)val5)[0], (((ArrayOn3dPoint)val5)[0] + (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[1]) / 2.0, splitFirst: true, value10, includeLast: true, num5, list);
			double num7 = LayoutStones(new On3dPoint(0.0, num3, 0.0), ((ArrayOn3dPoint)val5)[0], (((ArrayOn3dPoint)val5)[0] + (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[1]) / 2.0, splitFirst: false, value10, includeLast: true, num5, list2);
			double num8 = LayoutStones(new On3dPoint(num4, 0.0, 0.0), ((ArrayOn3dPoint)val5)[1], (((ArrayOn3dPoint)val5)[0] + (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[1]) / 2.0, splitFirst: true, value10, value10, num5, list3);
			double num9 = LayoutStones(new On3dPoint(num4, 0.0, 0.0), ((ArrayOn3dPoint)val5)[1], (((ArrayOn3dPoint)val5)[0] + (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[1]) / 2.0, splitFirst: false, value10, value10, num5, list4);
			List<OnCircle> list5 = new List<OnCircle>();
			list5.AddRange((num6 < num7) ? list : list2);
			list5.AddRange((num8 < num9) ? list3 : list4);
			AdjustNormals(val5, num5, CSBlockUtil.CSUtilMath.ToRadians(value3), list5);
			OnNurbsCurve input = BuildProfile(num5, value4, value7, value6, value5, value8, value9, (IOnNurbsCurve)(object)sideProfile);
			MapCurves((IOnNurbsCurve)(object)input, out var left, out var right, CSBlockUtil.CSUtilMath.ToRadians(value3), value11, num3, num4);
			OnBrep[] array = BuildLofts((OnCurve)(object)left, (OnCurve)(object)right);
			CSDataArray cSDataArray3 = new CSDataArray();
			if (array != null)
			{
				for (int j = 0; j < array.Length; j++)
				{
					((OnGeometry)array[j]).Transform((IOnXform)(object)xUp.Convert());
					cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)array[j].Convert(), xUp, applyTransform: false));
				}
			}
			cSDataArray2.Append(cSDataArray3);
			CSDataArray cSDataArray4 = new CSDataArray();
			list5 = QuadFlipCircles(list5);
			for (int k = 0; k < list5.Count; k++)
			{
				CSGemData cSGemData3 = cSGemData.Duplicate() as CSGemData;
				On3dVector val6 = new On3dVector(list5[k].plane.Normal());
				if (val6.z < 0.0)
				{
					val6.Reverse();
				}
				cSGemData3.SetXform(list5[k].Center().Convert(), val6.Convert(), list5[k].Diameter());
				cSGemData3.ApplyTransform(xUp);
				cSDataArray4.Append(cSGemData3);
			}
			cSDataArray.Append(cSDataArray4);
		}
		oMetal.SetData(cSDataArray2);
		oGems.SetData(cSDataArray);
		return base.Update();
	}

	private bool AdjustNormals(OnPolyline mainCurve, double size, double angleRadians, List<OnCircle> circles)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		for (int i = 0; i < circles.Count; i++)
		{
			IOn3dPoint val = circles[i].Center();
			On3dVector val2 = NormalAt(val, size, mainCurve, angleRadians);
			circles[i].plane = new OnPlane(val, (IOn3dVector)(object)val2);
		}
		return true;
	}

	private static OnBrep[] BuildLofts(OnCurve left, OnCurve right)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		List<OnCurve> list = new List<OnCurve>(9);
		list.Add(left);
		list.Add(right);
		OnXform val = new OnXform();
		val.Scale(1.0, -1.0, 1.0);
		list.Add(right.DuplicateCurve());
		list.Add(left.DuplicateCurve());
		((OnGeometry)list[2]).Transform((IOnXform)(object)val);
		((OnGeometry)list[3]).Transform((IOnXform)(object)val);
		val.Scale(-1.0, -1.0, 1.0);
		list.Add(left.DuplicateCurve());
		list.Add(right.DuplicateCurve());
		((OnGeometry)list[4]).Transform((IOnXform)(object)val);
		((OnGeometry)list[5]).Transform((IOnXform)(object)val);
		val.Scale(-1.0, 1.0, 1.0);
		list.Add(right.DuplicateCurve());
		list.Add(left.DuplicateCurve());
		((OnGeometry)list[6]).Transform((IOnXform)(object)val);
		((OnGeometry)list[7]).Transform((IOnXform)(object)val);
		list.Add(left.DuplicateCurve());
		OnCurve[] array = (OnCurve[])(object)new OnCurve[2];
		List<OnBrep> list2 = new List<OnBrep>();
		for (int i = 0; i < list.Count - 1; i++)
		{
			array[0] = list[i];
			array[1] = list[i + 1];
			IOnCurve[] profiles = (IOnCurve[])(object)array;
			OnBrep val2 = CSUtilSweep.Loft(profiles, (eLoftType)3);
			if (val2 != null)
			{
				list2.Add(val2);
			}
		}
		return list2.ToArray();
	}

	private static OnNurbsCurve BuildProfile(double stoneSize, double nudge, double offset, double profileSize, double profileHeight, double depth, double bottomTaper, IOnNurbsCurve sideProfile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		OnPolyCurve val = new OnPolyCurve();
		On3dPoint val2 = new On3dPoint((0.0 - stoneSize) / 2.0 * (1.0 - nudge), offset, 0.0);
		On3dPoint val3 = new On3dPoint(stoneSize / 2.0 * (1.0 - nudge), offset, 0.0);
		On3dPoint val4 = val2 - (IOn3dVector)new On3dVector(profileSize, 0.0, 0.0);
		On3dPoint obj = val3 + (IOn3dVector)new On3dVector(profileSize, 0.0, 0.0);
		On3dVector val5 = new On3dVector(0.0, 0.0 - depth, 0.0);
		On3dVector val6 = new On3dVector(0.0, 0.0 - profileHeight, 0.0);
		new OnPolyCurve();
		On3dPoint val7 = obj + (IOn3dVector)(object)val6;
		On3dPoint val8 = obj;
		On3dPoint obj2 = val7;
		obj2.x -= bottomTaper;
		OnCurve val9 = null;
		if (sideProfile != null)
		{
			val9 = ((IOnCurve)sideProfile).DuplicateCurve();
			OnXform val10 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility((IOn3dPoint)(object)((IOnCurve)sideProfile).PointAtStart(), (IOn3dPoint)(object)(((IOnCurve)sideProfile).PointAtStart() + OnUtil.On_zaxis), (IOn3dPoint)(object)((IOnCurve)sideProfile).PointAtEnd(), (IOn3dPoint)(object)val8, (IOn3dPoint)(object)(val8 + OnUtil.On_zaxis), (IOn3dPoint)(object)val7);
			OnXform val11 = new OnXform();
			val11.Scale((IOn3dPoint)(object)val8, (val8.y - val7.y) / (((IOnCurve)sideProfile).PointAtStart().y - ((IOnCurve)sideProfile).PointAtEnd().y));
			if (((OnObject)val9).IsValid())
			{
				((OnGeometry)val9).Transform((IOnXform)(object)(val11 * (IOnXform)(object)val10));
				val7 = val9.PointAtEnd();
			}
		}
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)val4, (IOn3dPoint)(object)val2));
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)val2, (IOn3dPoint)(object)(val2 + (IOn3dVector)(object)val5)));
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)(val2 + (IOn3dVector)(object)val5), (IOn3dPoint)(object)(val3 + (IOn3dVector)(object)val5)));
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)(val3 + (IOn3dVector)(object)val5), (IOn3dPoint)(object)val3));
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)val3, (IOn3dPoint)(object)val8));
		if (val9 != null && ((OnObject)val9).IsValid())
		{
			val.Append(val9);
		}
		else
		{
			val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)val8, (IOn3dPoint)(object)val7));
		}
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)val7, (IOn3dPoint)(object)(val4 + (IOn3dVector)(object)val6)));
		val.Append((OnCurve)new OnLineCurve((IOn3dPoint)(object)(val4 + (IOn3dVector)(object)val6), (IOn3dPoint)(object)val4));
		return ((OnCurve)val).NurbsCurve();
	}

	private static double LayoutStones(On3dPoint p1, On3dPoint p2, On3dPoint p3, bool splitFirst, bool splitSecond, bool includeLast, double stoneSize, List<OnCircle> circles)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		OnLine val = new OnLine((IOn3dPoint)(object)p1, (IOn3dPoint)(object)p2);
		OnLine val2 = new OnLine((IOn3dPoint)(object)p2, (IOn3dPoint)(object)p3);
		On3dPoint val3 = val.PointAt(0.0);
		On3dPoint val4 = val2.PointAt(1.0);
		On3dPointArray val5 = new On3dPointArray(500);
		for (double num = 0.0; num < 1.0; num += 1.0 / 160.0)
		{
			if ((!splitFirst || val.PointAt(num).DistanceTo((IOn3dPoint)(object)val3) > (stoneSize + 1.0 / 160.0) / 2.0) && (!splitSecond || val.PointAt(num).DistanceTo((IOn3dPoint)(object)val4) > (stoneSize + 1.0 / 160.0) / 2.0))
			{
				((ArrayOn3dPoint)val5).Append((IOn3dPoint)(object)val.PointAt(num));
			}
		}
		for (double num2 = 0.0; num2 < 1.0; num2 += 1.0 / 160.0)
		{
			if ((!splitFirst || val2.PointAt(num2).DistanceTo((IOn3dPoint)(object)val3) > (stoneSize + 1.0 / 160.0) / 2.0) && (!splitSecond || val2.PointAt(num2).DistanceTo((IOn3dPoint)(object)val4) > (stoneSize + 1.0 / 160.0) / 2.0))
			{
				((ArrayOn3dPoint)val5).Append((IOn3dPoint)(object)val2.PointAt(num2));
			}
		}
		if (((ArrayOn3dPoint)val5).Count() < 10)
		{
			return double.MaxValue;
		}
		On3dPoint point = ((ArrayOn3dPoint)val5)[0];
		On3dPoint point2 = ((ArrayOn3dPoint)val5).Last();
		On3dPointArray val6 = new On3dPointArray();
		double num3 = 0.0;
		On3dPoint val7 = new On3dPoint(0.0, 0.0, 0.0);
		On3dPointArray val8 = new On3dPointArray(500);
		On3dPointArray val9 = new On3dPointArray(500);
		for (double num4 = 0.0; num4 < stoneSize / 4.0; num4 += stoneSize / 256.0)
		{
			((ArrayOn3dPoint)val8).Empty();
			((ArrayOn3dPoint)val9).Empty();
			((ArrayOn3dPoint)val8).SetCapacity(500);
			((ArrayOn3dPoint)val9).SetCapacity(500);
			double num5 = (num4 + 0.01) * (num4 + 0.01);
			for (int i = 0; i < ((ArrayOn3dPoint)val5).Count(); i++)
			{
				bool flag = true;
				if (splitFirst)
				{
					flag &= CSBlockUtil.CSUtilMath.DistanceSquared((IOn3dPoint)(object)point, (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[i]) >= num5;
				}
				if (splitSecond)
				{
					flag &= CSBlockUtil.CSUtilMath.DistanceSquared((IOn3dPoint)(object)point2, (IOn3dPoint)(object)((ArrayOn3dPoint)val5)[i]) >= num5;
				}
				if (flag)
				{
					((ArrayOn3dPoint)val9).Append((IOn3dPoint)(object)((ArrayOn3dPoint)val5)[i]);
				}
			}
			if (((ArrayOn3dPoint)val9).Count() < 5)
			{
				continue;
			}
			((ArrayOn3dPoint)val8).Append((IOn3dPoint)(object)((ArrayOn3dPoint)val9)[0]);
			double num6 = (stoneSize + num4 + 0.01) * (stoneSize + num4 + 0.01);
			for (int j = 0; j < ((ArrayOn3dPoint)val9).Count(); j++)
			{
				if (CSBlockUtil.CSUtilMath.DistanceSquared((IOn3dPoint)(object)((ArrayOn3dPoint)val8).Last(), (IOn3dPoint)(object)((ArrayOn3dPoint)val9)[j]) > num6)
				{
					((ArrayOn3dPoint)val8).Append((IOn3dPoint)(object)((ArrayOn3dPoint)val9)[j]);
				}
			}
			if (((ArrayOn3dPoint)val6).Count() == 0)
			{
				val6 = new On3dPointArray((IOn3dPointArray)(object)val8);
				num3 = double.MaxValue;
				val7 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val9).Last());
				continue;
			}
			double num7 = ((ArrayOn3dPoint)val8).Last().DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val9).Last());
			if (num7 < num3)
			{
				num3 = num7;
				val6 = new On3dPointArray((IOn3dPointArray)(object)val8);
				val7 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val9).Last());
			}
		}
		((ArrayOn3dPoint)val6).Remove();
		((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)val7);
		if (!includeLast)
		{
			((ArrayOn3dPoint)val6).Remove();
		}
		for (int k = 0; k < ((ArrayOn3dPoint)val6).Count(); k++)
		{
			circles.Add(new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)((ArrayOn3dPoint)val6)[k], OnUtil.On_zaxis), stoneSize / 2.0));
		}
		return num3;
	}

	private void MapCurves(IOnNurbsCurve input, out OnNurbsCurve left, out OnNurbsCurve right, double angleRadians, double cornerSize, double height, double width)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		left = OnNurbsCurve.New(input);
		right = OnNurbsCurve.New(input);
		OnXform val = new OnXform();
		val.Rotation(angleRadians, OnUtil.On_zaxis, OnUtil.On_origin);
		left.Transform((IOnXform)(object)val);
		right.Transform((IOnXform)(object)val);
		On3dPoint left2 = new On3dPoint();
		On3dPoint right2 = new On3dPoint();
		for (int i = 0; i < input.CVCount(); i++)
		{
			On3dPoint p = new On3dPoint();
			left.GetCV(i, ref p);
			MapPoint(p, out left2, out right2, cornerSize, height, width);
			left.SetCV(i, (IOn3dPoint)(object)left2);
			right.SetCV(i, (IOn3dPoint)(object)right2);
		}
	}

	private static void MapPoint(On3dPoint p, out On3dPoint left, out On3dPoint right, double cornerSize, double height, double width)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		double x = p.x;
		double num = x * 0.41421356;
		num = ((!(num - cornerSize > x)) ? (num - cornerSize) : x);
		right = new On3dPoint(width + x, height + num, p.y);
		left = new On3dPoint(width + num, height + x, p.y);
		On3dVector val = new On3dVector(0.0, 0.0, 0.0);
		if (left.DistanceTo((IOn3dPoint)(object)right) < 0.02)
		{
			val = new On3dVector(0.02, -0.02, 0.0);
		}
		right += (IOn3dVector)(object)val;
		left += (IOn3dVector)(object)val;
	}

	private On3dVector NormalAt(IOn3dPoint p, double size, OnPolyline mainCurve, double angleRadians)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		double num = 0.0;
		mainCurve.ClosestPointTo(p, ref num);
		On3dVector val = ((ArrayOn3dPoint)mainCurve)[(int)num + 1] - (IOn3dPoint)(object)((ArrayOn3dPoint)mainCurve)[(int)num];
		val.Unitize();
		if (size < 0.01)
		{
			On3dVector val2 = new On3dVector(OnUtil.On_zaxis);
			val2.Rotate(angleRadians, (IOn3dVector)(object)val);
			return val2;
		}
		On3dVector obj = NormalAt((IOn3dPoint)(object)(new On3dPoint(p) + (IOn3dVector)(object)(val * size / 2.0)), 0.0, mainCurve, angleRadians);
		On3dVector val3 = NormalAt((IOn3dPoint)(object)(new On3dPoint(p) - (IOn3dVector)(object)(val * size / 2.0)), 0.0, mainCurve, angleRadians);
		On3dVector obj2 = obj + (IOn3dVector)(object)val3;
		obj2.Unitize();
		return obj2;
	}

	private static List<OnCircle> QuadFlipCircles(List<OnCircle> circles)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		List<OnCircle> list = new List<OnCircle>();
		for (int i = 0; i < circles.Count; i++)
		{
			bool flag = true;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Center().DistanceTo(circles[i].Center()) < list[j].radius + circles[i].radius - 0.5)
				{
					flag = false;
				}
			}
			if (flag)
			{
				list.Add(circles[i]);
			}
		}
		OnXform val = new OnXform();
		val.Scale(-1.0, 1.0, 1.0);
		for (int k = 0; k < circles.Count; k++)
		{
			OnCircle val2 = new OnCircle((IOnCircle)(object)circles[k]);
			val2.Transform((IOnXform)(object)val);
			bool flag2 = true;
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Center().DistanceTo(val2.Center()) < list[l].radius + val2.radius - 0.5)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				list.Add(val2);
			}
		}
		val.Scale(-1.0, -1.0, 1.0);
		for (int m = 0; m < circles.Count; m++)
		{
			OnCircle val3 = new OnCircle((IOnCircle)(object)circles[m]);
			val3.Transform((IOnXform)(object)val);
			bool flag3 = true;
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Center().DistanceTo(val3.Center()) < list[n].radius + val3.radius - 0.5)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				list.Add(val3);
			}
		}
		val.Scale(1.0, -1.0, 1.0);
		for (int num = 0; num < circles.Count; num++)
		{
			OnCircle val4 = new OnCircle((IOnCircle)(object)circles[num]);
			val4.Transform((IOnXform)(object)val);
			bool flag4 = true;
			for (int num2 = 0; num2 < list.Count; num2++)
			{
				if (list[num2].Center().DistanceTo(val4.Center()) < list[num2].radius + val4.radius - 0.5)
				{
					flag4 = false;
				}
			}
			if (flag4)
			{
				list.Add(val4);
			}
		}
		return list;
	}
}
