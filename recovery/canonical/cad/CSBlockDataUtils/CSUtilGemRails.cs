using System;
using System.Collections.Generic;
using System.Linq;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockDataUtils;

public static class CSUtilGemRails
{
	private sealed class SortGem
	{
		public readonly CSData Gem;

		public readonly On3dPoint Pt;

		public SortGem(CSData gem, On3dPoint pt)
		{
			Gem = gem;
			Pt = pt;
		}
	}

	private static readonly double _test45 = Math.Sqrt(2.0) / 2.0;

	public static bool CalculateLeft(List<CSData> datas, OnCurve inRail, double offsetLeft, double offsetZ, out OnCurve rail)
	{
		bool flag = IsClosed(datas);
		rail = null;
		if (inRail == null)
		{
			return false;
		}
		List<CSChannelGemData> list = datas.OfType<CSChannelGemData>().ToList();
		if (list.Count == 0)
		{
			return false;
		}
		if (list.Count == 1)
		{
			CSChannelGemData cSChannelGemData = list[0];
			rail = inRail.DuplicateCurve();
			rail.Translate(new On3dVector(cSChannelGemData.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint(cSChannelGemData2.Left.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData2.Normal.Convert()) * offsetZ;
			list2.Add(item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint(cSChannelGemData3.Left.Convert()) + new On3dVector(cSChannelGemData3.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData3.Normal.Convert()) * offsetZ;
				if (CSUtilCurves.TrimCurve(inRail, cSChannelGemData2.Center.Convert(), cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Left);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Left);
					double num3 = (num + num2) / 2.0;
					On3dVector on3dVector = new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) + new On3dVector(cSChannelGemData3.UnitLeftVector.Convert());
					on3dVector.Unitize();
					On3dVector on3dVector2 = new On3dVector(cSChannelGemData2.Normal.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert());
					on3dVector2.Unitize();
					On3dPoint item3 = new On3dPoint(trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + on3dVector * (num3 + offsetLeft) + on3dVector2 * offsetZ;
					list2.Add(item3);
					list2.Add(item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add(new On3dPoint(list2[0]));
			}
			rail = ChannelInterpolateCurve(list2.ToArray());
		}
		if (rail != null)
		{
			return rail.IsValid();
		}
		return false;
	}

	public static bool CalculateLeft(List<CSData> datas, OnCurve inRail, double offsetLeft, double offsetZ, out Curve outRail)
	{
		bool flag = IsClosed(datas);
		outRail = null;
		if (inRail == null)
		{
			return false;
		}
		List<CSChannelGemData> list = datas.OfType<CSChannelGemData>().ToList();
		if (list.Count == 0)
		{
			return false;
		}
		OnCurve onCurve;
		if (list.Count == 1)
		{
			CSChannelGemData cSChannelGemData = list[0];
			onCurve = inRail.DuplicateCurve();
			onCurve.Translate(new On3dVector(cSChannelGemData.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint(cSChannelGemData2.Left.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData2.Normal.Convert()) * offsetZ;
			list2.Add(item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint(cSChannelGemData3.Left.Convert()) + new On3dVector(cSChannelGemData3.UnitLeftVector.Convert()) * offsetLeft + new On3dVector(cSChannelGemData3.Normal.Convert()) * offsetZ;
				if (CSUtilCurves.TrimCurve(inRail, cSChannelGemData2.Center.Convert(), cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Left);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Left);
					double num3 = (num + num2) / 2.0;
					On3dVector on3dVector = new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) + new On3dVector(cSChannelGemData3.UnitLeftVector.Convert());
					on3dVector.Unitize();
					On3dVector on3dVector2 = new On3dVector(cSChannelGemData2.Normal.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert());
					on3dVector2.Unitize();
					On3dPoint item3 = new On3dPoint(trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + on3dVector * (num3 + offsetLeft) + on3dVector2 * offsetZ;
					list2.Add(item3);
					list2.Add(item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add(new On3dPoint(list2[0]));
			}
			onCurve = ChannelInterpolateCurve(list2.ToArray());
		}
		outRail = Interop.FromOnCurve(onCurve);
		if (outRail != null)
		{
			return outRail.IsValid;
		}
		return false;
	}

	public static bool CalculateRail(List<CSData> datas, double offsetZ, out OnCurve rail)
	{
		return CalculateRail(datas.OfType<CSChannelGemData>().ToList(), offsetZ, out rail);
	}

	public static bool CalculateRight(List<CSData> datas, OnCurve inRail, double offsetRight, double offsetZ, out OnCurve rail)
	{
		bool flag = IsClosed(datas);
		rail = null;
		if (inRail == null)
		{
			return false;
		}
		List<CSChannelGemData> list = datas.OfType<CSChannelGemData>().ToList();
		if (list.Count == 0)
		{
			return false;
		}
		if (list.Count == 1)
		{
			CSChannelGemData cSChannelGemData = list[0];
			rail = inRail.DuplicateCurve();
			rail.Translate(new On3dVector(cSChannelGemData.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint(cSChannelGemData2.Right.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData2.Normal.Convert()) * offsetZ;
			list2.Add(item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint(cSChannelGemData3.Right.Convert()) + new On3dVector(cSChannelGemData3.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData3.Normal.Convert()) * offsetZ;
				if (CSUtilCurves.TrimCurve(inRail, cSChannelGemData2.Center.Convert(), cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Right);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Right);
					double num3 = (num + num2) / 2.0;
					On3dVector on3dVector = new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) + new On3dVector(cSChannelGemData3.UnitRightVector.Convert());
					on3dVector.Unitize();
					On3dVector on3dVector2 = new On3dVector(cSChannelGemData2.Normal.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert());
					on3dVector2.Unitize();
					On3dPoint item3 = new On3dPoint(trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + on3dVector * (num3 + offsetRight) + on3dVector2 * offsetZ;
					list2.Add(item3);
					list2.Add(item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add(new On3dPoint(list2[0]));
			}
			rail = ChannelInterpolateCurve(list2.ToArray());
		}
		if (rail != null)
		{
			return rail.IsValid();
		}
		return false;
	}

	public static bool CalculateRight(List<CSData> datas, OnCurve inRail, double offsetRight, double offsetZ, out Curve outRail)
	{
		bool flag = IsClosed(datas);
		OnCurve onCurve = null;
		outRail = null;
		if (inRail == null)
		{
			return false;
		}
		List<CSChannelGemData> list = datas.OfType<CSChannelGemData>().ToList();
		if (list.Count == 0)
		{
			return false;
		}
		if (list.Count == 1)
		{
			CSChannelGemData cSChannelGemData = list[0];
			onCurve = inRail.DuplicateCurve();
			onCurve.Translate(new On3dVector(cSChannelGemData.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint(cSChannelGemData2.Right.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData2.Normal.Convert()) * offsetZ;
			list2.Add(item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint(cSChannelGemData3.Right.Convert()) + new On3dVector(cSChannelGemData3.UnitRightVector.Convert()) * offsetRight + new On3dVector(cSChannelGemData3.Normal.Convert()) * offsetZ;
				if (CSUtilCurves.TrimCurve(inRail, cSChannelGemData2.Center.Convert(), cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Right);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Right);
					double num3 = (num + num2) / 2.0;
					On3dVector on3dVector = new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) + new On3dVector(cSChannelGemData3.UnitRightVector.Convert());
					on3dVector.Unitize();
					On3dVector on3dVector2 = new On3dVector(cSChannelGemData2.Normal.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert());
					on3dVector2.Unitize();
					On3dPoint item3 = new On3dPoint(trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + on3dVector * (num3 + offsetRight) + on3dVector2 * offsetZ;
					list2.Add(item3);
					list2.Add(item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add(new On3dPoint(list2[0]));
			}
			onCurve = ChannelInterpolateCurve(list2.ToArray());
		}
		outRail = Interop.FromOnCurve(onCurve);
		if (outRail != null)
		{
			return outRail.IsValid;
		}
		return false;
	}

	public static void CreateRails(CSDataArray cgda)
	{
		OnCurve rail = Rail(cgda.Array);
		foreach (CSChannelGemData item in cgda.Array.OfType<CSChannelGemData>())
		{
			CalculateSides(item, rail);
		}
	}

	public static void CreateRails(List<CSChannelGemData> cgda)
	{
		OnCurve rail = Rail(cgda);
		foreach (CSChannelGemData cgdum in cgda)
		{
			CalculateSides(cgdum, rail);
		}
	}

	public static CSChannelGemData GetChannelGemData(CSDataArray dArr, int index)
	{
		return dArr.GetArrayItem(index) as CSChannelGemData;
	}

	public static IList<CSChannelGemData> GetKeyChannelGems(List<CSData> array)
	{
		IList<CSChannelGemData> list = array.OfType<CSChannelGemData>().ToList();
		List<CSChannelGemData> list2 = new List<CSChannelGemData>();
		double num = -1.23432101234321E+308;
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = i;
			CSChannelGemData cSChannelGemData = list[i];
			if (i == 0)
			{
				list2.Add(cSChannelGemData);
				continue;
			}
			int index = i - 1;
			CSChannelGemData cSChannelGemData2 = list[index];
			double num3 = cSChannelGemData.Size - cSChannelGemData2.Size;
			if (RhinoMath.IsValidDouble(num) && Math.Abs(num3 - num) > 0.001)
			{
				list2.Add(cSChannelGemData2);
			}
			num = num3;
			if (num2 == list.Count - 1)
			{
				list2.Add(cSChannelGemData);
			}
		}
		return list2;
	}

	public static bool GetTransformNoArray(CSData data, out Transform value)
	{
		value = Transform.Unset;
		if (data == null)
		{
			return false;
		}
		if (data is CSDataArray)
		{
			return false;
		}
		if (data is ICSTransform iCSTransform)
		{
			return iCSTransform.GetTransform(out value);
		}
		return false;
	}

	public static bool IsClosed(CSDataArray array)
	{
		return IsClosed(array.Array);
	}

	public static bool IsClosed(List<CSData> array)
	{
		return IsClosed(array.OfType<CSChannelGemData>().ToList());
	}

	public static bool IsClosed(List<CSChannelGemData> sortedGems)
	{
		int count = sortedGems.Count;
		double num = 0.0;
		for (int i = 0; i < count - 1; i++)
		{
			num += sortedGems[i].Center.DistanceTo(sortedGems[i + 1].Center);
		}
		if (count > 3)
		{
			double num2 = sortedGems[0].Center.DistanceTo(sortedGems[count - 1].Center);
			if (num / (double)(count - 1) > num2 * 0.9)
			{
				return true;
			}
		}
		return false;
	}

	public static OnCurve Left(CSDataArray cgda, OnCurve rail)
	{
		if (CalculateLeft(cgda.Array, rail, 0.0, 0.0, out OnCurve rail2))
		{
			return rail2;
		}
		return null;
	}

	public static Curve LeftRc(CSDataArray cgda, Curve rail)
	{
		OnCurve inRail = ((rail != null) ? ((OnCurve)Interop.ToOnCurve(rail)) : null);
		if (CalculateLeft(cgda.Array, inRail, 0.0, 0.0, out OnCurve rail2))
		{
			return Interop.FromOnCurve(rail2);
		}
		return null;
	}

	public static OnCurve Rail(CSDataArray cgda)
	{
		return Rail(cgda.Array);
	}

	public static OnCurve Rail(List<CSData> datas)
	{
		return Rail(datas.OfType<CSChannelGemData>().ToList());
	}

	public static OnCurve Rail(List<CSChannelGemData> gemDatas)
	{
		Curve curve = null;
		if (gemDatas.Count >= 1)
		{
			curve = gemDatas[0].SourceRail;
		}
		if (((curve == null) ? CalculateRail(gemDatas, 0.0, out var rail) : CalculateRail(gemDatas, curve, 0.0, out rail)) && rail != null && rail.IsValid())
		{
			return rail;
		}
		return null;
	}

	public static Curve RailRc(CSDataArray cgda)
	{
		return RailRc(cgda.Array);
	}

	public static OnCurve Right(CSDataArray cgda, OnCurve rail)
	{
		if (CalculateRight(cgda.Array, rail, 0.0, 0.0, out OnCurve rail2))
		{
			return rail2;
		}
		return null;
	}

	public static Curve RightRc(CSDataArray cgda, Curve rail)
	{
		OnCurve inRail = ((rail != null) ? ((OnCurve)Interop.ToOnCurve(rail)) : null);
		if (CalculateRight(cgda.Array, inRail, 0.0, 0.0, out OnCurve rail2))
		{
			return Interop.FromOnCurve(rail2);
		}
		return null;
	}

	public static double SizeEnd(CSDataArray dArr)
	{
		return GetChannelGemData(dArr, dArr.GetArrayCount() - 1).Size;
	}

	public static double SizeStart(CSDataArray dArr)
	{
		return GetChannelGemData(dArr, 0).Size;
	}

	public static void SortGems(CSDataArray array, bool doKeepStart = true)
	{
		List<SortGem> list = new List<SortGem>();
		List<SortGem> list2 = new List<SortGem>();
		for (int i = 0; i < array.GetArrayCount(); i++)
		{
			CSData arrayItem = array.GetArrayItem(i);
			if (arrayItem != null)
			{
				GetTransformNoArray(arrayItem, out var value);
				if (value.IsValid)
				{
					Point3d origin = Point3d.Origin;
					origin.Transform(value);
					SortGem item = new SortGem(arrayItem, origin.Convert());
					list.Add(item);
					list2.Add(item);
				}
			}
		}
		List<SortGem> list3 = new List<SortGem>();
		if (list.Count < 1)
		{
			return;
		}
		SortGem sortGem = list[0];
		SortGem sortGem2 = list[list.Count - 1];
		list3.Add(list[0]);
		list.Remove(list3[0]);
		while (list.Count > 0)
		{
			SortGem sortGem3 = list3[list3.Count - 1];
			SortGem sortGem4 = list3[0];
			SortGem item2 = list[0];
			double num = list[0].Pt.DistanceTo(sortGem3.Pt);
			SortGem item3 = list[0];
			double num2 = list[0].Pt.DistanceTo(sortGem4.Pt);
			for (int j = 1; j < list.Count; j++)
			{
				double num3 = list[j].Pt.DistanceTo(sortGem4.Pt);
				double num4 = list[j].Pt.DistanceTo(sortGem3.Pt);
				if (num3 < num2)
				{
					item3 = list[j];
					num2 = num3;
				}
				if (num4 < num)
				{
					item2 = list[j];
					num = num4;
				}
			}
			if (num2 * 1.2 < num)
			{
				list3.Insert(0, item3);
				list.Remove(item3);
			}
			else
			{
				list3.Add(item2);
				list.Remove(item2);
			}
		}
		bool flag = false;
		double num5 = 0.0;
		for (int k = 0; k < list3.Count - 1; k++)
		{
			num5 += list3[k].Pt.DistanceTo(list3[k + 1].Pt);
		}
		if (list3.Count > 3)
		{
			double num6 = list3[0].Pt.DistanceTo(list3[list3.Count - 1].Pt);
			if (num5 / (double)(list3.Count - 1) > num6 * 0.9)
			{
				flag = true;
			}
		}
		if (!flag && doKeepStart && list3[0] != sortGem)
		{
			for (int l = 0; l < list2.Count && list2[l] != list3[0]; l++)
			{
				if (list2[l] == list3[list3.Count - 1])
				{
					list3.Reverse();
					break;
				}
			}
		}
		if (list3.Count > 2)
		{
			double num7 = list3[0].Pt.DistanceTo(list3[1].Pt);
			double num8 = list3[0].Pt.DistanceTo(list3[list3.Count - 1].Pt);
			if (Math.Abs(num7 - num8) / num7 < 0.05 && list3[1] == sortGem2)
			{
				List<SortGem> list4 = new List<SortGem>();
				for (int m = 1; m < list3.Count; m++)
				{
					list4.Add(list3[m]);
				}
				list4.Reverse();
				foreach (SortGem item4 in list4)
				{
					list3.Remove(item4);
				}
				list3.AddRange(list4);
			}
		}
		array.Array.Clear();
		foreach (SortGem item5 in list3)
		{
			array.Array.Add(item5.Gem);
		}
	}

	private static bool CalculateRail(List<CSChannelGemData> gemDatas, double offsetZ, out OnCurve rail)
	{
		bool flag = IsClosed(gemDatas);
		rail = null;
		if (gemDatas.Count == 0)
		{
			return false;
		}
		if (gemDatas.Count == 1)
		{
			CSChannelGemData cSChannelGemData = gemDatas[0];
			double num = CSUtil.Tolerance / 2.0;
			On3dPoint arg = new On3dPoint(cSChannelGemData.Center.Convert()) + new On3dVector(cSChannelGemData.North.Convert()) * num;
			On3dPoint arg2 = new On3dPoint(cSChannelGemData.Center.Convert()) + new On3dVector(cSChannelGemData.South.Convert()) * num;
			rail = new OnLineCurve(arg, arg2);
			rail.Translate(new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list = gemDatas.Select((CSChannelGemData gemData) => new On3dPoint(gemData.Center.Convert()) + new On3dVector(gemData.Normal.Convert()) * offsetZ).Cast<IOn3dPoint>().ToList();
			if (flag && list.Count > 0)
			{
				list.Add(new On3dPoint(list[0]));
			}
			rail = ChannelInterpolateCurve(list.ToArray());
		}
		if (rail != null)
		{
			return rail.IsValid();
		}
		return false;
	}

	private static bool CalculateRail(List<CSData> datas, Curve sourceRailRc, double offsetZ, out OnCurve rail)
	{
		return CalculateRail(datas.OfType<CSChannelGemData>().ToList(), sourceRailRc, offsetZ, out rail);
	}

	private static bool CalculateRail(List<CSChannelGemData> gemDatas, Curve sourceRailRc, double offsetZ, out OnCurve rail)
	{
		bool flag = IsClosed(gemDatas);
		OnCurve onCurve = (OnCurve)Interop.ToOnCurve(sourceRailRc);
		rail = null;
		if (onCurve == null)
		{
			return false;
		}
		if (gemDatas.Count == 0)
		{
			return false;
		}
		if (gemDatas.Count == 1)
		{
			CSChannelGemData cSChannelGemData = gemDatas[0];
			double t = -1.23432101234321E+308;
			if (!onCurve.GetClosestPoint(cSChannelGemData.Center.Convert(), ref t))
			{
				return false;
			}
			On3dVector on3dVector = onCurve.TangentAt(t);
			double num = CSUtil.Tolerance / 2.0;
			On3dPoint arg = new On3dPoint(cSChannelGemData.Center.Convert()) + on3dVector * (0.0 - num);
			On3dPoint arg2 = new On3dPoint(cSChannelGemData.Center.Convert()) + on3dVector * num;
			rail = new OnLineCurve(arg, arg2);
			rail.Translate(new On3dVector(cSChannelGemData.Normal.Convert()) * offsetZ);
		}
		else
		{
			List<IOn3dPoint> list = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = gemDatas[0];
			double t2 = -1.23432101234321E+308;
			if (!onCurve.GetClosestPoint(cSChannelGemData2.Center.Convert(), ref t2))
			{
				return false;
			}
			On3dPoint on3dPoint = onCurve.PointAt(t2);
			list.Add(new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.Normal.Convert()) * offsetZ);
			for (int i = 1; i < gemDatas.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = gemDatas[i];
				double t3 = -1.23432101234321E+308;
				if (onCurve.GetClosestPoint(cSChannelGemData3.Center.Convert(), ref t3))
				{
					On3dPoint on3dPoint2 = onCurve.PointAt(t3);
					if (CSUtilCurves.TrimCurve(onCurve, on3dPoint, on3dPoint2, out var trimmedCrv))
					{
						double num2 = on3dPoint.DistanceTo(cSChannelGemData2.Center.Convert());
						double num3 = on3dPoint2.DistanceTo(cSChannelGemData3.Center.Convert());
						double num4 = (num2 + num3) / 2.0;
						On3dVector on3dVector2 = new On3dPoint(cSChannelGemData2.Center.Convert()) - on3dPoint + (new On3dPoint(cSChannelGemData3.Center.Convert()) - on3dPoint2);
						on3dVector2.Unitize();
						On3dVector on3dVector3 = new On3dVector(cSChannelGemData2.Normal.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert());
						on3dVector3.Unitize();
						list.Add(trimmedCrv.PointAt(trimmedCrv.Domain().Mid()) + on3dVector2 * num4 + on3dVector3 * offsetZ);
						list.Add(new On3dPoint(cSChannelGemData3.Center.Convert()) + new On3dVector(cSChannelGemData3.Normal.Convert()) * offsetZ);
						cSChannelGemData2 = cSChannelGemData3;
						on3dPoint = on3dPoint2;
					}
				}
			}
			if (flag && list.Count > 0)
			{
				list.Add(new On3dPoint(list[0]));
			}
			rail = ChannelInterpolateCurve(list.ToArray());
		}
		if (rail != null)
		{
			return rail.IsValid();
		}
		return false;
	}

	private static void CalculateSides(CSChannelGemData gemD, IOnCurve rail)
	{
		gemD.SizeForward = gemD.GemYSize / 2.0;
		gemD.UnitForwardVector = Vector3d.XAxis;
		gemD.VectorLength = gemD.Size;
		if (rail == null)
		{
			return;
		}
		double t = 0.0;
		if (!rail.GetClosestPoint(gemD.Center.Convert(), ref t))
		{
			return;
		}
		On3dVector on3dVector = rail.TangentAt(t);
		double num = 1.0;
		bool flag = false;
		if (rail.IsClosed() && rail.PointAtStart().DistanceTo(rail.PointAt(t)) < 0.001)
		{
			double num2 = rail.Domain().Length() / 1000.0;
			On3dVector on3dVector2 = rail.TangentAt(rail.Domain().Min() + num2);
			on3dVector2.Unitize();
			On3dVector on3dVector3 = rail.TangentAt(rail.Domain().Max() - num2);
			on3dVector3.Unitize();
			on3dVector = (on3dVector2 + on3dVector3) / 2.0;
			flag = true;
		}
		on3dVector.Unitize();
		if (!flag)
		{
			double t2 = 0.0;
			double num3 = rail.Domain().Length() / 1000.0;
			if (rail.GetNextDiscontinuity(IOn.continuity.C1_continuous, t - num3, t + num3, ref t2))
			{
				On3dVector on3dVector4 = rail.TangentAt(t2 - num3 / 2.0);
				on3dVector4.Unitize();
				On3dVector on3dVector5 = rail.TangentAt(t2 + num3 / 2.0);
				on3dVector5.Unitize();
				if (on3dVector4.IsValid() && on3dVector5.IsValid())
				{
					on3dVector = (on3dVector4 + on3dVector5) / 2.0;
				}
				else if (on3dVector4.IsValid())
				{
					on3dVector = on3dVector4;
				}
				else if (on3dVector5.IsValid())
				{
					on3dVector = on3dVector5;
				}
				on3dVector4.Unitize();
				on3dVector5.Unitize();
				num = Math.Acos(on3dVector4 * on3dVector5);
			}
		}
		on3dVector.Unitize();
		gemD.UnitForwardVector = on3dVector.Convert();
		On3dVector on3dVector6 = new On3dVector(1.0, 0.0, 0.0);
		on3dVector6.Transform(gemD.Xform.Convert());
		on3dVector6.Unitize();
		double num4 = gemD.UnitForwardVector.Convert() * on3dVector6;
		if (num4 > _test45 || num4 < 0.0 - _test45)
		{
			gemD.Size = gemD.GemYSize / 2.0;
			gemD.SizeForward = gemD.GemXSize / 2.0;
		}
		else
		{
			gemD.Size = gemD.GemXSize / 2.0;
			gemD.SizeForward = gemD.GemYSize / 2.0;
		}
		gemD.VectorLength = gemD.Size * num;
	}

	private static OnCurve ChannelInterpolateCurve(IReadOnlyList<IOn3dPoint> points)
	{
		ArrayOn3dPoint arrayOn3dPoint = new ArrayOn3dPoint();
		for (int i = 0; i < points.Count; i++)
		{
			arrayOn3dPoint.Append(points[i]);
		}
		return RhUtilHelper.RhinoInterpCurve(3, arrayOn3dPoint, null, null, 4);
	}

	private static Curve RailRc(List<CSData> datas)
	{
		if (CalculateRail(datas, 0.0, out var rail) && rail != null && rail.IsValid())
		{
			return Interop.FromOnCurve(rail);
		}
		return null;
	}
}
