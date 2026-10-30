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
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00bb: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0120: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01c9: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fa: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
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
			((OnGeometry)rail).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.UnitLeftVector.Convert()) * offsetLeft + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ)));
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * offsetLeft) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * offsetZ);
			list2.Add((IOn3dPoint)(object)item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitLeftVector.Convert()) * offsetLeft) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) * offsetZ);
				if (CSUtilCurves.TrimCurve((IOnCurve)(object)inRail, (IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Left);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Left);
					double num3 = (num + num2) / 2.0;
					On3dVector val = new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitLeftVector.Convert());
					val.Unitize();
					On3dVector val2 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert());
					val2.Unitize();
					On3dPoint item3 = new On3dPoint((IOn3dPoint)(object)trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + (IOn3dVector)(object)(val * (num3 + offsetLeft)) + (IOn3dVector)(object)(val2 * offsetZ);
					list2.Add((IOn3dPoint)(object)item3);
					list2.Add((IOn3dPoint)(object)item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add((IOn3dPoint)new On3dPoint(list2[0]));
			}
			rail = ChannelInterpolateCurve(list2.ToArray());
		}
		if (rail != null)
		{
			return ((OnObject)rail).IsValid();
		}
		return false;
	}

	public static bool CalculateLeft(List<CSData> datas, OnCurve inRail, double offsetLeft, double offsetZ, out Curve outRail)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b8: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011e: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
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
		OnCurve val;
		if (list.Count == 1)
		{
			CSChannelGemData cSChannelGemData = list[0];
			val = inRail.DuplicateCurve();
			((OnGeometry)val).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.UnitLeftVector.Convert()) * offsetLeft + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ)));
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * offsetLeft) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * offsetZ);
			list2.Add((IOn3dPoint)(object)item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitLeftVector.Convert()) * offsetLeft) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) * offsetZ);
				if (CSUtilCurves.TrimCurve((IOnCurve)(object)inRail, (IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Left);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Left);
					double num3 = (num + num2) / 2.0;
					On3dVector val2 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitLeftVector.Convert());
					val2.Unitize();
					On3dVector val3 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert());
					val3.Unitize();
					On3dPoint item3 = new On3dPoint((IOn3dPoint)(object)trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + (IOn3dVector)(object)(val2 * (num3 + offsetLeft)) + (IOn3dVector)(object)(val3 * offsetZ);
					list2.Add((IOn3dPoint)(object)item3);
					list2.Add((IOn3dPoint)(object)item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add((IOn3dPoint)new On3dPoint(list2[0]));
			}
			val = ChannelInterpolateCurve(list2.ToArray());
		}
		outRail = Interop.FromOnCurve(val);
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
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00bb: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0120: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01c9: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fa: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
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
			((OnGeometry)rail).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.UnitRightVector.Convert()) * offsetRight + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ)));
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * offsetRight) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * offsetZ);
			list2.Add((IOn3dPoint)(object)item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitRightVector.Convert()) * offsetRight) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) * offsetZ);
				if (CSUtilCurves.TrimCurve((IOnCurve)(object)inRail, (IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Right);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Right);
					double num3 = (num + num2) / 2.0;
					On3dVector val = new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitRightVector.Convert());
					val.Unitize();
					On3dVector val2 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert());
					val2.Unitize();
					On3dPoint item3 = new On3dPoint((IOn3dPoint)(object)trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + (IOn3dVector)(object)(val * (num3 + offsetRight)) + (IOn3dVector)(object)(val2 * offsetZ);
					list2.Add((IOn3dPoint)(object)item3);
					list2.Add((IOn3dPoint)(object)item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add((IOn3dPoint)new On3dPoint(list2[0]));
			}
			rail = ChannelInterpolateCurve(list2.ToArray());
		}
		if (rail != null)
		{
			return ((OnObject)rail).IsValid();
		}
		return false;
	}

	public static bool CalculateRight(List<CSData> datas, OnCurve inRail, double offsetRight, double offsetZ, out Curve outRail)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00ba: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0120: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01c9: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fa: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		bool flag = IsClosed(datas);
		OnCurve val = null;
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
			val = inRail.DuplicateCurve();
			((OnGeometry)val).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.UnitRightVector.Convert()) * offsetRight + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ)));
		}
		else
		{
			List<IOn3dPoint> list2 = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = list[0];
			On3dPoint item = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * offsetRight) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * offsetZ);
			list2.Add((IOn3dPoint)(object)item);
			for (int i = 1; i < list.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = list[i];
				On3dPoint item2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitRightVector.Convert()) * offsetRight) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) * offsetZ);
				if (CSUtilCurves.TrimCurve((IOnCurve)(object)inRail, (IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), out var trimmedCrv))
				{
					double num = cSChannelGemData2.Center.DistanceTo(cSChannelGemData2.Right);
					double num2 = cSChannelGemData3.Center.DistanceTo(cSChannelGemData3.Right);
					double num3 = (num + num2) / 2.0;
					On3dVector val2 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.UnitRightVector.Convert());
					val2.Unitize();
					On3dVector val3 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert());
					val3.Unitize();
					On3dPoint item3 = new On3dPoint((IOn3dPoint)(object)trimmedCrv.PointAt(trimmedCrv.Domain().Mid())) + (IOn3dVector)(object)(val2 * (num3 + offsetRight)) + (IOn3dVector)(object)(val3 * offsetZ);
					list2.Add((IOn3dPoint)(object)item3);
					list2.Add((IOn3dPoint)(object)item2);
					cSChannelGemData2 = cSChannelGemData3;
				}
			}
			if (flag && list2.Count > 0)
			{
				list2.Add((IOn3dPoint)new On3dPoint(list2[0]));
			}
			val = ChannelInterpolateCurve(list2.ToArray());
		}
		outRail = Interop.FromOnCurve(val);
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
			CalculateSides(item, (IOnCurve)(object)rail);
		}
	}

	public static void CreateRails(List<CSChannelGemData> cgda)
	{
		OnCurve rail = Rail(cgda);
		foreach (CSChannelGemData cgdum in cgda)
		{
			CalculateSides(cgdum, (IOnCurve)(object)rail);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		OnCurve inRail = ((rail == null) ? ((OnCurve)null) : ((OnCurve)Interop.ToOnCurve(rail)));
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
		if (((curve == null) ? CalculateRail(gemDatas, 0.0, out var rail) : CalculateRail(gemDatas, curve, 0.0, out rail)) && rail != null && ((OnObject)rail).IsValid())
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		OnCurve inRail = ((rail == null) ? ((OnCurve)null) : ((OnCurve)Interop.ToOnCurve(rail)));
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
			double num = list[0].Pt.DistanceTo((IOn3dPoint)(object)sortGem3.Pt);
			SortGem item3 = list[0];
			double num2 = list[0].Pt.DistanceTo((IOn3dPoint)(object)sortGem4.Pt);
			for (int j = 1; j < list.Count; j++)
			{
				double num3 = list[j].Pt.DistanceTo((IOn3dPoint)(object)sortGem4.Pt);
				double num4 = list[j].Pt.DistanceTo((IOn3dPoint)(object)sortGem3.Pt);
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
			num5 += list3[k].Pt.DistanceTo((IOn3dPoint)(object)list3[k + 1].Pt);
		}
		if (list3.Count > 3)
		{
			double num6 = list3[0].Pt.DistanceTo((IOn3dPoint)(object)list3[list3.Count - 1].Pt);
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
			double num7 = list3[0].Pt.DistanceTo((IOn3dPoint)(object)list3[1].Pt);
			double num8 = list3[0].Pt.DistanceTo((IOn3dPoint)(object)list3[list3.Count - 1].Pt);
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0070: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009d: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
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
			On3dPoint val = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.North.Convert()) * num);
			On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.South.Convert()) * num);
			rail = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
			((OnGeometry)rail).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ));
		}
		else
		{
			List<IOn3dPoint> list = gemDatas.Select((CSChannelGemData gemData) => new On3dPoint((IOn3dPoint)(object)gemData.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)gemData.Normal.Convert()) * offsetZ)).Cast<IOn3dPoint>().ToList();
			if (flag && list.Count > 0)
			{
				list.Add((IOn3dPoint)new On3dPoint(list[0]));
			}
			rail = ChannelInterpolateCurve(list.ToArray());
		}
		if (rail != null)
		{
			return ((OnObject)rail).IsValid();
		}
		return false;
	}

	private static bool CalculateRail(List<CSData> datas, Curve sourceRailRc, double offsetZ, out OnCurve rail)
	{
		return CalculateRail(datas.OfType<CSChannelGemData>().ToList(), sourceRailRc, offsetZ, out rail);
	}

	private static bool CalculateRail(List<CSChannelGemData> gemDatas, Curve sourceRailRc, double offsetZ, out OnCurve rail)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014f: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0242: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02b0: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		bool flag = IsClosed(gemDatas);
		OnCurve val = (OnCurve)Interop.ToOnCurve(sourceRailRc);
		rail = null;
		if (val == null)
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
			double num = -1.23432101234321E+308;
			if (!val.GetClosestPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), ref num))
			{
				return false;
			}
			On3dVector val2 = val.TangentAt(num);
			double num2 = CSUtil.Tolerance / 2.0;
			On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) + (IOn3dVector)(object)(val2 * (0.0 - num2));
			On3dPoint val4 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData.Center.Convert()) + (IOn3dVector)(object)(val2 * num2);
			rail = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val3, (IOn3dPoint)(object)val4);
			((OnGeometry)rail).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData.Normal.Convert()) * offsetZ));
		}
		else
		{
			List<IOn3dPoint> list = new List<IOn3dPoint>();
			CSChannelGemData cSChannelGemData2 = gemDatas[0];
			double num3 = -1.23432101234321E+308;
			if (!val.GetClosestPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), ref num3))
			{
				return false;
			}
			On3dPoint val5 = val.PointAt(num3);
			list.Add((IOn3dPoint)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * offsetZ)));
			for (int i = 1; i < gemDatas.Count; i++)
			{
				CSChannelGemData cSChannelGemData3 = gemDatas[i];
				double num4 = -1.23432101234321E+308;
				if (val.GetClosestPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), ref num4))
				{
					On3dPoint val6 = val.PointAt(num4);
					if (CSUtilCurves.TrimCurve((IOnCurve)(object)val, (IOn3dPoint)(object)val5, (IOn3dPoint)(object)val6, out var trimmedCrv))
					{
						double num5 = val5.DistanceTo((IOn3dPoint)(object)cSChannelGemData2.Center.Convert());
						double num6 = val6.DistanceTo((IOn3dPoint)(object)cSChannelGemData3.Center.Convert());
						double num7 = (num5 + num6) / 2.0;
						On3dVector val7 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) - (IOn3dPoint)(object)val5 + (IOn3dVector)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) - (IOn3dPoint)(object)val6);
						val7.Unitize();
						On3dVector val8 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert());
						val8.Unitize();
						list.Add((IOn3dPoint)(object)(trimmedCrv.PointAt(trimmedCrv.Domain().Mid()) + (IOn3dVector)(object)(val7 * num7) + (IOn3dVector)(object)(val8 * offsetZ)));
						list.Add((IOn3dPoint)(object)(new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) * offsetZ)));
						cSChannelGemData2 = cSChannelGemData3;
						val5 = val6;
					}
				}
			}
			if (flag && list.Count > 0)
			{
				list.Add((IOn3dPoint)new On3dPoint(list[0]));
			}
			rail = ChannelInterpolateCurve(list.ToArray());
		}
		if (rail != null)
		{
			return ((OnObject)rail).IsValid();
		}
		return false;
	}

	private static void CalculateSides(CSChannelGemData gemD, IOnCurve rail)
	{
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		gemD.SizeForward = gemD.GemYSize / 2.0;
		gemD.UnitForwardVector = Vector3d.XAxis;
		gemD.VectorLength = gemD.Size;
		if (rail == null)
		{
			return;
		}
		double num = 0.0;
		if (!rail.GetClosestPoint((IOn3dPoint)(object)gemD.Center.Convert(), ref num))
		{
			return;
		}
		On3dVector val = rail.TangentAt(num);
		double num2 = 1.0;
		bool flag = false;
		if (rail.IsClosed() && rail.PointAtStart().DistanceTo((IOn3dPoint)(object)rail.PointAt(num)) < 0.001)
		{
			double num3 = rail.Domain().Length() / 1000.0;
			On3dVector obj = rail.TangentAt(rail.Domain().Min() + num3);
			obj.Unitize();
			On3dVector val2 = rail.TangentAt(rail.Domain().Max() - num3);
			val2.Unitize();
			val = (obj + (IOn3dVector)(object)val2) / 2.0;
			flag = true;
		}
		val.Unitize();
		if (!flag)
		{
			double num4 = 0.0;
			double num5 = rail.Domain().Length() / 1000.0;
			if (rail.GetNextDiscontinuity((continuity)2, num - num5, num + num5, ref num4))
			{
				On3dVector val3 = rail.TangentAt(num4 - num5 / 2.0);
				val3.Unitize();
				On3dVector val4 = rail.TangentAt(num4 + num5 / 2.0);
				val4.Unitize();
				if (val3.IsValid() && val4.IsValid())
				{
					val = (val3 + (IOn3dVector)(object)val4) / 2.0;
				}
				else if (val3.IsValid())
				{
					val = val3;
				}
				else if (val4.IsValid())
				{
					val = val4;
				}
				val3.Unitize();
				val4.Unitize();
				num2 = Math.Acos(val3 * (IOn3dVector)(object)val4);
			}
		}
		val.Unitize();
		gemD.UnitForwardVector = val.Convert();
		On3dVector val5 = new On3dVector(1.0, 0.0, 0.0);
		val5.Transform((IOnXform)(object)gemD.Xform.Convert());
		val5.Unitize();
		double num6 = gemD.UnitForwardVector.Convert() * (IOn3dVector)(object)val5;
		if (num6 > _test45 || num6 < 0.0 - _test45)
		{
			gemD.Size = gemD.GemYSize / 2.0;
			gemD.SizeForward = gemD.GemXSize / 2.0;
		}
		else
		{
			gemD.Size = gemD.GemXSize / 2.0;
			gemD.SizeForward = gemD.GemYSize / 2.0;
		}
		gemD.VectorLength = gemD.Size * num2;
	}

	private static OnCurve ChannelInterpolateCurve(IReadOnlyList<IOn3dPoint> points)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ArrayOn3dPoint val = new ArrayOn3dPoint();
		for (int i = 0; i < points.Count; i++)
		{
			val.Append(points[i]);
		}
		return RhUtilHelper.RhinoInterpCurve(3, val, null, null, 4);
	}

	private static Curve RailRc(List<CSData> datas)
	{
		if (CalculateRail(datas, 0.0, out var rail) && rail != null && ((OnObject)rail).IsValid())
		{
			return Interop.FromOnCurve(rail);
		}
		return null;
	}
}
