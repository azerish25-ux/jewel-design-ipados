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
			On3dVector on3dVector = new On3dVector(num4, num3, 0.0);
			On3dVector on3dVector2 = new On3dVector(num4, 0.0 - num3, 0.0);
			On3dVector on3dVector3 = new On3dVector(value11, 0.0, 0.0);
			On3dVector on3dVector4 = new On3dVector(0.0, value11, 0.0);
			OnPolyline onPolyline = new OnPolyline();
			onPolyline.Append(new On3dPoint(on3dVector - on3dVector3));
			onPolyline.Append(new On3dPoint(on3dVector - on3dVector4));
			onPolyline.Append(new On3dPoint(on3dVector2 + on3dVector4));
			onPolyline.Append(new On3dPoint(on3dVector2 - on3dVector3));
			onPolyline.Append(new On3dPoint(-on3dVector + on3dVector3));
			onPolyline.Append(new On3dPoint(-on3dVector + on3dVector4));
			onPolyline.Append(new On3dPoint(-on3dVector2 - on3dVector4));
			onPolyline.Append(new On3dPoint(-on3dVector2 + on3dVector3));
			onPolyline.Append(new On3dPoint(on3dVector - on3dVector3));
			List<OnCircle> list = new List<OnCircle>();
			List<OnCircle> list2 = new List<OnCircle>();
			List<OnCircle> list3 = new List<OnCircle>();
			List<OnCircle> list4 = new List<OnCircle>();
			double num5 = ((cSGemData.GemXSize > cSGemData.GemYSize) ? cSGemData.GemXSize : cSGemData.GemYSize);
			double num6 = LayoutStones(new On3dPoint(0.0, num3, 0.0), onPolyline[0], (onPolyline[0] + onPolyline[1]) / 2.0, splitFirst: true, value10, includeLast: true, num5, list);
			double num7 = LayoutStones(new On3dPoint(0.0, num3, 0.0), onPolyline[0], (onPolyline[0] + onPolyline[1]) / 2.0, splitFirst: false, value10, includeLast: true, num5, list2);
			double num8 = LayoutStones(new On3dPoint(num4, 0.0, 0.0), onPolyline[1], (onPolyline[0] + onPolyline[1]) / 2.0, splitFirst: true, value10, value10, num5, list3);
			double num9 = LayoutStones(new On3dPoint(num4, 0.0, 0.0), onPolyline[1], (onPolyline[0] + onPolyline[1]) / 2.0, splitFirst: false, value10, value10, num5, list4);
			List<OnCircle> list5 = new List<OnCircle>();
			list5.AddRange((num6 < num7) ? list : list2);
			list5.AddRange((num8 < num9) ? list3 : list4);
			AdjustNormals(onPolyline, num5, CSBlockUtil.CSUtilMath.ToRadians(value3), list5);
			OnNurbsCurve input = BuildProfile(num5, value4, value7, value6, value5, value8, value9, sideProfile);
			MapCurves(input, out var left, out var right, CSBlockUtil.CSUtilMath.ToRadians(value3), value11, num3, num4);
			OnBrep[] array = BuildLofts(left, right);
			CSDataArray cSDataArray3 = new CSDataArray();
			if (array != null)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Transform(xUp.Convert());
					cSDataArray3.Append(new CSOnGeometryData(array[j].Convert(), xUp, applyTransform: false));
				}
			}
			cSDataArray2.Append(cSDataArray3);
			CSDataArray cSDataArray4 = new CSDataArray();
			list5 = QuadFlipCircles(list5);
			for (int k = 0; k < list5.Count; k++)
			{
				CSGemData cSGemData3 = cSGemData.Duplicate() as CSGemData;
				On3dVector on3dVector5 = new On3dVector(list5[k].plane.Normal());
				if (on3dVector5.z < 0.0)
				{
					on3dVector5.Reverse();
				}
				cSGemData3.SetXform(list5[k].Center().Convert(), on3dVector5.Convert(), list5[k].Diameter());
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
		for (int i = 0; i < circles.Count; i++)
		{
			IOn3dPoint on3dPoint = circles[i].Center();
			On3dVector normal = NormalAt(on3dPoint, size, mainCurve, angleRadians);
			circles[i].plane = new OnPlane(on3dPoint, normal);
		}
		return true;
	}

	private static OnBrep[] BuildLofts(OnCurve left, OnCurve right)
	{
		List<OnCurve> list = new List<OnCurve>(9);
		list.Add(left);
		list.Add(right);
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, -1.0, 1.0);
		list.Add(right.DuplicateCurve());
		list.Add(left.DuplicateCurve());
		list[2].Transform(onXform);
		list[3].Transform(onXform);
		onXform.Scale(-1.0, -1.0, 1.0);
		list.Add(left.DuplicateCurve());
		list.Add(right.DuplicateCurve());
		list[4].Transform(onXform);
		list[5].Transform(onXform);
		onXform.Scale(-1.0, 1.0, 1.0);
		list.Add(right.DuplicateCurve());
		list.Add(left.DuplicateCurve());
		list[6].Transform(onXform);
		list[7].Transform(onXform);
		list.Add(left.DuplicateCurve());
		OnCurve[] array = new OnCurve[2];
		List<OnBrep> list2 = new List<OnBrep>();
		for (int i = 0; i < list.Count - 1; i++)
		{
			array[0] = list[i];
			array[1] = list[i + 1];
			IOnCurve[] profiles = array;
			OnBrep onBrep = CSUtilSweep.Loft(profiles, IArgsRhinoLoft.eLoftType.ltStraight);
			if (onBrep != null)
			{
				list2.Add(onBrep);
			}
		}
		return list2.ToArray();
	}

	private static OnNurbsCurve BuildProfile(double stoneSize, double nudge, double offset, double profileSize, double profileHeight, double depth, double bottomTaper, IOnNurbsCurve sideProfile)
	{
		OnPolyCurve onPolyCurve = new OnPolyCurve();
		On3dPoint on3dPoint = new On3dPoint((0.0 - stoneSize) / 2.0 * (1.0 - nudge), offset, 0.0);
		On3dPoint on3dPoint2 = new On3dPoint(stoneSize / 2.0 * (1.0 - nudge), offset, 0.0);
		On3dPoint on3dPoint3 = on3dPoint - new On3dVector(profileSize, 0.0, 0.0);
		On3dPoint on3dPoint4 = on3dPoint2 + new On3dVector(profileSize, 0.0, 0.0);
		On3dVector on3dVector = new On3dVector(0.0, 0.0 - depth, 0.0);
		On3dVector on3dVector2 = new On3dVector(0.0, 0.0 - profileHeight, 0.0);
		new OnPolyCurve();
		On3dPoint on3dPoint5 = on3dPoint4 + on3dVector2;
		On3dPoint on3dPoint6 = on3dPoint4;
		on3dPoint5.x -= bottomTaper;
		OnCurve onCurve = null;
		if (sideProfile != null)
		{
			onCurve = sideProfile.DuplicateCurve();
			OnXform onXform = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(sideProfile.PointAtStart(), sideProfile.PointAtStart() + OnUtil.On_zaxis, sideProfile.PointAtEnd(), on3dPoint6, on3dPoint6 + OnUtil.On_zaxis, on3dPoint5);
			OnXform onXform2 = new OnXform();
			onXform2.Scale(on3dPoint6, (on3dPoint6.y - on3dPoint5.y) / (sideProfile.PointAtStart().y - sideProfile.PointAtEnd().y));
			if (onCurve.IsValid())
			{
				onCurve.Transform(onXform2 * onXform);
				on3dPoint5 = onCurve.PointAtEnd();
			}
		}
		onPolyCurve.Append(new OnLineCurve(on3dPoint3, on3dPoint));
		onPolyCurve.Append(new OnLineCurve(on3dPoint, on3dPoint + on3dVector));
		onPolyCurve.Append(new OnLineCurve(on3dPoint + on3dVector, on3dPoint2 + on3dVector));
		onPolyCurve.Append(new OnLineCurve(on3dPoint2 + on3dVector, on3dPoint2));
		onPolyCurve.Append(new OnLineCurve(on3dPoint2, on3dPoint6));
		if (onCurve != null && onCurve.IsValid())
		{
			onPolyCurve.Append(onCurve);
		}
		else
		{
			onPolyCurve.Append(new OnLineCurve(on3dPoint6, on3dPoint5));
		}
		onPolyCurve.Append(new OnLineCurve(on3dPoint5, on3dPoint3 + on3dVector2));
		onPolyCurve.Append(new OnLineCurve(on3dPoint3 + on3dVector2, on3dPoint3));
		return onPolyCurve.NurbsCurve();
	}

	private static double LayoutStones(On3dPoint p1, On3dPoint p2, On3dPoint p3, bool splitFirst, bool splitSecond, bool includeLast, double stoneSize, List<OnCircle> circles)
	{
		OnLine onLine = new OnLine(p1, p2);
		OnLine onLine2 = new OnLine(p2, p3);
		On3dPoint arg = onLine.PointAt(0.0);
		On3dPoint arg2 = onLine2.PointAt(1.0);
		On3dPointArray on3dPointArray = new On3dPointArray(500);
		for (double num = 0.0; num < 1.0; num += 1.0 / 160.0)
		{
			if ((!splitFirst || onLine.PointAt(num).DistanceTo(arg) > (stoneSize + 1.0 / 160.0) / 2.0) && (!splitSecond || onLine.PointAt(num).DistanceTo(arg2) > (stoneSize + 1.0 / 160.0) / 2.0))
			{
				on3dPointArray.Append(onLine.PointAt(num));
			}
		}
		for (double num2 = 0.0; num2 < 1.0; num2 += 1.0 / 160.0)
		{
			if ((!splitFirst || onLine2.PointAt(num2).DistanceTo(arg) > (stoneSize + 1.0 / 160.0) / 2.0) && (!splitSecond || onLine2.PointAt(num2).DistanceTo(arg2) > (stoneSize + 1.0 / 160.0) / 2.0))
			{
				on3dPointArray.Append(onLine2.PointAt(num2));
			}
		}
		if (on3dPointArray.Count() < 10)
		{
			return double.MaxValue;
		}
		On3dPoint point = on3dPointArray[0];
		On3dPoint point2 = on3dPointArray.Last();
		On3dPointArray on3dPointArray2 = new On3dPointArray();
		double num3 = 0.0;
		On3dPoint val = new On3dPoint(0.0, 0.0, 0.0);
		On3dPointArray on3dPointArray3 = new On3dPointArray(500);
		On3dPointArray on3dPointArray4 = new On3dPointArray(500);
		for (double num4 = 0.0; num4 < stoneSize / 4.0; num4 += stoneSize / 256.0)
		{
			on3dPointArray3.Empty();
			on3dPointArray4.Empty();
			on3dPointArray3.SetCapacity(500);
			on3dPointArray4.SetCapacity(500);
			double num5 = (num4 + 0.01) * (num4 + 0.01);
			for (int i = 0; i < on3dPointArray.Count(); i++)
			{
				bool flag = true;
				if (splitFirst)
				{
					flag &= CSBlockUtil.CSUtilMath.DistanceSquared(point, on3dPointArray[i]) >= num5;
				}
				if (splitSecond)
				{
					flag &= CSBlockUtil.CSUtilMath.DistanceSquared(point2, on3dPointArray[i]) >= num5;
				}
				if (flag)
				{
					on3dPointArray4.Append(on3dPointArray[i]);
				}
			}
			if (on3dPointArray4.Count() < 5)
			{
				continue;
			}
			on3dPointArray3.Append(on3dPointArray4[0]);
			double num6 = (stoneSize + num4 + 0.01) * (stoneSize + num4 + 0.01);
			for (int j = 0; j < on3dPointArray4.Count(); j++)
			{
				if (CSBlockUtil.CSUtilMath.DistanceSquared(on3dPointArray3.Last(), on3dPointArray4[j]) > num6)
				{
					on3dPointArray3.Append(on3dPointArray4[j]);
				}
			}
			if (on3dPointArray2.Count() == 0)
			{
				on3dPointArray2 = new On3dPointArray(on3dPointArray3);
				num3 = double.MaxValue;
				val = new On3dPoint(on3dPointArray4.Last());
				continue;
			}
			double num7 = on3dPointArray3.Last().DistanceTo(on3dPointArray4.Last());
			if (num7 < num3)
			{
				num3 = num7;
				on3dPointArray2 = new On3dPointArray(on3dPointArray3);
				val = new On3dPoint(on3dPointArray4.Last());
			}
		}
		on3dPointArray2.Remove();
		on3dPointArray2.Append(val);
		if (!includeLast)
		{
			on3dPointArray2.Remove();
		}
		for (int k = 0; k < on3dPointArray2.Count(); k++)
		{
			circles.Add(new OnCircle(new OnPlane(on3dPointArray2[k], OnUtil.On_zaxis), stoneSize / 2.0));
		}
		return num3;
	}

	private void MapCurves(IOnNurbsCurve input, out OnNurbsCurve left, out OnNurbsCurve right, double angleRadians, double cornerSize, double height, double width)
	{
		left = OnNurbsCurve.New(input);
		right = OnNurbsCurve.New(input);
		OnXform onXform = new OnXform();
		onXform.Rotation(angleRadians, OnUtil.On_zaxis, OnUtil.On_origin);
		left.Transform(onXform);
		right.Transform(onXform);
		On3dPoint left2 = new On3dPoint();
		On3dPoint right2 = new On3dPoint();
		for (int i = 0; i < input.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			left.GetCV(i, ref vertex_value);
			MapPoint(vertex_value, out left2, out right2, cornerSize, height, width);
			left.SetCV(i, left2);
			right.SetCV(i, right2);
		}
	}

	private static void MapPoint(On3dPoint p, out On3dPoint left, out On3dPoint right, double cornerSize, double height, double width)
	{
		double x = p.x;
		double num = x * 0.41421356;
		num = ((!(num - cornerSize > x)) ? (num - cornerSize) : x);
		right = new On3dPoint(width + x, height + num, p.y);
		left = new On3dPoint(width + num, height + x, p.y);
		On3dVector on3dVector = new On3dVector(0.0, 0.0, 0.0);
		if (left.DistanceTo(right) < 0.02)
		{
			on3dVector = new On3dVector(0.02, -0.02, 0.0);
		}
		right += (IOn3dVector)on3dVector;
		left += (IOn3dVector)on3dVector;
	}

	private On3dVector NormalAt(IOn3dPoint p, double size, OnPolyline mainCurve, double angleRadians)
	{
		double t = 0.0;
		mainCurve.ClosestPointTo(p, ref t);
		On3dVector on3dVector = mainCurve[(int)t + 1] - mainCurve[(int)t];
		on3dVector.Unitize();
		if (size < 0.01)
		{
			On3dVector on3dVector2 = new On3dVector(OnUtil.On_zaxis);
			on3dVector2.Rotate(angleRadians, on3dVector);
			return on3dVector2;
		}
		On3dVector on3dVector3 = NormalAt(new On3dPoint(p) + on3dVector * size / 2.0, 0.0, mainCurve, angleRadians);
		On3dVector on3dVector4 = NormalAt(new On3dPoint(p) - on3dVector * size / 2.0, 0.0, mainCurve, angleRadians);
		On3dVector on3dVector5 = on3dVector3 + on3dVector4;
		on3dVector5.Unitize();
		return on3dVector5;
	}

	private static List<OnCircle> QuadFlipCircles(List<OnCircle> circles)
	{
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
		OnXform onXform = new OnXform();
		onXform.Scale(-1.0, 1.0, 1.0);
		for (int k = 0; k < circles.Count; k++)
		{
			OnCircle onCircle = new OnCircle(circles[k]);
			onCircle.Transform(onXform);
			bool flag2 = true;
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Center().DistanceTo(onCircle.Center()) < list[l].radius + onCircle.radius - 0.5)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				list.Add(onCircle);
			}
		}
		onXform.Scale(-1.0, -1.0, 1.0);
		for (int m = 0; m < circles.Count; m++)
		{
			OnCircle onCircle2 = new OnCircle(circles[m]);
			onCircle2.Transform(onXform);
			bool flag3 = true;
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Center().DistanceTo(onCircle2.Center()) < list[n].radius + onCircle2.radius - 0.5)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				list.Add(onCircle2);
			}
		}
		onXform.Scale(1.0, -1.0, 1.0);
		for (int num = 0; num < circles.Count; num++)
		{
			OnCircle onCircle3 = new OnCircle(circles[num]);
			onCircle3.Transform(onXform);
			bool flag4 = true;
			for (int num2 = 0; num2 < list.Count; num2++)
			{
				if (list[num2].Center().DistanceTo(onCircle3.Center()) < list[num2].radius + onCircle3.radius - 0.5)
				{
					flag4 = false;
				}
			}
			if (flag4)
			{
				list.Add(onCircle3);
			}
		}
		return list;
	}
}
