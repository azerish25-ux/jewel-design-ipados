using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings.FrenchHalo.Obsolete;

[Obsolete]
public static class CSFrenchHaloHelper1
{
	public enum FrenchObjectPart
	{
		Bot = 1,
		Side,
		Top
	}

	public static void CalculateRail(CSChannelGemData[] gemDatas, int curvedir, double rightCornerScale, double leftCornerScale, double wallWidth, double? blendAmount, out Curve rail)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		rail = null;
		switch (gemDatas.Length)
		{
		case 0:
			return;
		case 1:
		{
			CSChannelGemData cSChannelGemData = gemDatas[0];
			double num = CSApplicationHelper.Application.Tolerance / 2.0;
			Point3d val = cSChannelGemData.Center + cSChannelGemData.North * num;
			Point3d val2 = cSChannelGemData.Center + cSChannelGemData.South * num;
			rail = (Curve)new LineCurve(val, val2);
			return;
		}
		}
		List<CSChannelGemData> list = gemDatas.ToList();
		list.Add(list[0]);
		List<Point3d> list2 = curvedir switch
		{
			1 => list.Select((CSChannelGemData gemData) => LeftPointAngled(gemData, leftCornerScale, wallWidth)).ToList(), 
			-1 => list.Select((CSChannelGemData gemData) => RightPointAngled(gemData, rightCornerScale, wallWidth)).ToList(), 
			_ => list.Select((CSChannelGemData gemData) => gemData.Center).ToList(), 
		};
		List<Point3d> list3 = new List<Point3d>();
		List<Curve> list4 = new List<Curve>();
		List<ChannelCornerType> list5 = new List<ChannelCornerType>();
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			list3.Add(list2[num2]);
			if (num2 != 0 && list[num2].Corner != ChannelCornerType.none)
			{
				list5.Add(list[num2].Corner);
				Curve item = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list3, 3, (CurveKnotStyle)1);
				list4.Add(item);
				list3.Clear();
				list3.Add(list2[num2]);
			}
		}
		if (list3.Count == list.Count)
		{
			rail = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list3, 3, (CurveKnotStyle)4);
			return;
		}
		if (list3.Count > 1)
		{
			throw new InvalidOperationException("Should be exactly 1 item left in the array");
		}
		if (list4.Count != list5.Count)
		{
			throw new InvalidOperationException("Corner Count does not match Curve Count");
		}
		if (blendAmount.HasValue && blendAmount > 0.001)
		{
			List<double> list6 = new List<double>();
			foreach (Curve item2 in list4)
			{
				list6.Add(item2.GetLength() / 2.0 * 0.9);
			}
			int count = list4.Count;
			for (int num3 = 0; num3 < count; num3++)
			{
				if (list5[num3] == ChannelCornerType.corner45)
				{
					int index = (num3 - 1 + count) % count;
					Curve val3 = list4[index];
					double num4 = list6[index];
					Curve val4 = list4[num3];
					double num5 = list6[num3];
					double num6 = ((blendAmount > num4) ? num4 : blendAmount.Value);
					double num7 = ((blendAmount > num5) ? num5 : blendAmount.Value);
					val3 = val3.Trim((CurveEnd)2, num6);
					val4 = val4.Trim((CurveEnd)1, num7);
					GemOffsetCurve.BlendCorner(ref val3, ref val4);
					list4[index] = val3;
					list4[num3] = val4;
				}
			}
		}
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)list4);
		rail = array[0];
	}

	public static Point3d LeftPointAngled(CSChannelGemData gemData, double scale, double wallWidth)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return PointAngled(gemData, gemData.UnitLeftVector, scale, wallWidth);
	}

	public static Point3d RightPointAngled(CSChannelGemData gemData, double scale, double wallWidth)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return PointAngled(gemData, gemData.UnitRightVector, scale, wallWidth);
	}

	private static Point3d PointAngled(CSChannelGemData gemData, Vector3d dir, double scale, double wallWidth)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		double num = gemData.VectorLength + wallWidth;
		double num2 = ((gemData.Corner == ChannelCornerType.corner90) ? scale : 1.0) / Math.Sin(gemData.CornerAngle / 2.0);
		Vector3d val = dir * num;
		((Vector3d)(ref val)).X = ((Vector3d)(ref val)).X * num2;
		((Vector3d)(ref val)).Y = ((Vector3d)(ref val)).Y * num2;
		return gemData.Center + val;
	}

	public static void ConvZ(ref double zPer, FrenchObjectPart whichPart, double bottomSize, double topSize, double profileHeight, double basePer, double midPer, double topPer, double scaleToGem)
	{
		double num = 0.0;
		if (whichPart == FrenchObjectPart.Bot)
		{
			double num2 = bottomSize * scaleToGem;
			num = zPer / basePer * num2;
		}
		if (whichPart == FrenchObjectPart.Side)
		{
			double num3 = bottomSize * scaleToGem;
			double num4 = topSize * scaleToGem;
			double num5 = profileHeight - (num3 + num4);
			double num6 = (zPer - basePer) / midPer;
			num = num3 + num5 * num6;
		}
		if (whichPart == FrenchObjectPart.Top)
		{
			double num7 = topSize * scaleToGem;
			num = profileHeight - (1.0 - zPer) / topPer * num7;
		}
		zPer = num / profileHeight;
	}

	public static void SetUpFrenchGems(CSChannelGemData[] gemDatas, double? corner = null, double? corner2 = null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		int num = gemDatas.Length;
		int num2 = num;
		for (int i = 0; i < num2; i++)
		{
			Vector3d val = gemDatas[(i - 1 + num) % num].Center - gemDatas[i].Center;
			Vector3d val2 = gemDatas[(i + 1) % num].Center - gemDatas[i].Center;
			((Vector3d)(ref val)).Unitize();
			((Vector3d)(ref val2)).Unitize();
			Vector3d unitForwardVector = val2 - val;
			((Vector3d)(ref unitForwardVector)).Unitize();
			gemDatas[i].UnitForwardVector = unitForwardVector;
			gemDatas[i].CornerAngle = CSValueHandleHelper.AngleBetweenVectors(val, val2);
			string value = "";
			if (gemDatas[i].GetUserString("Corner", ref value))
			{
				if (int.TryParse(value, out var result))
				{
					gemDatas[i].Corner = (ChannelCornerType)result;
				}
			}
			else if (corner.HasValue && corner2.HasValue)
			{
				if (gemDatas[i].CornerAngle < corner)
				{
					gemDatas[i].Corner = ChannelCornerType.corner90;
				}
				if (gemDatas[i].CornerAngle >= corner && gemDatas[i].CornerAngle < corner2)
				{
					gemDatas[i].Corner = ChannelCornerType.corner45;
				}
			}
		}
	}

	public static CSChannelGemData[] SorttoFirstCorner(CSChannelGemData[] gemDatas)
	{
		int num = gemDatas.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (gemDatas[i].Corner != ChannelCornerType.none)
			{
				num2 = i;
				break;
			}
		}
		CSChannelGemData[] array = new CSChannelGemData[num];
		for (int j = 0; j < num; j++)
		{
			array[j] = gemDatas[(j + num2) % num];
		}
		return array;
	}

	public static void TrimCrv(ref Curve crv0, ref Curve crv1, double cornerSplitPercent)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		double num = default(double);
		crv0.NormalizedLengthParameter(cornerSplitPercent, ref num);
		double num2 = default(double);
		crv1.NormalizedLengthParameter(1.0 - cornerSplitPercent, ref num2);
		Curve obj = crv0;
		Interval domain = crv0.Domain;
		crv0 = obj.Trim(((Interval)(ref domain)).Min, num);
		Curve obj2 = crv1;
		double num3 = num2;
		domain = crv1.Domain;
		crv1 = obj2.Trim(num3, ((Interval)(ref domain)).Max);
	}

	public static Curve TrimCrvToPoints(Curve crv, Point3d p0, Point3d p1)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = crv.DuplicateCurve();
		double num = default(double);
		obj.ClosestPoint(p0, ref num);
		obj.ChangeClosedCurveSeam(num);
		obj.ClosestPoint(p1, ref num);
		Interval domain = obj.Domain;
		return obj.Trim(((Interval)(ref domain)).Min, num);
	}
}
