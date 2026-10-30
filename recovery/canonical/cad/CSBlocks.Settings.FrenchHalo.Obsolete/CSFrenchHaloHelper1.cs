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
		rail = null;
		switch (gemDatas.Length)
		{
		case 0:
			return;
		case 1:
		{
			CSChannelGemData cSChannelGemData = gemDatas[0];
			double num = CSApplicationHelper.Application.Tolerance / 2.0;
			Point3d point3d = cSChannelGemData.Center + cSChannelGemData.North * num;
			Point3d to = cSChannelGemData.Center + cSChannelGemData.South * num;
			rail = new LineCurve(point3d, to);
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
				Curve item = Curve.CreateInterpolatedCurve(list3, 3, CurveKnotStyle.Chord);
				list4.Add(item);
				list3.Clear();
				list3.Add(list2[num2]);
			}
		}
		if (list3.Count == list.Count)
		{
			rail = Curve.CreateInterpolatedCurve(list3, 3, CurveKnotStyle.ChordPeriodic);
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
					Curve curve = list4[index];
					double num4 = list6[index];
					Curve curve2 = list4[num3];
					double num5 = list6[num3];
					double length = ((blendAmount > num4) ? num4 : blendAmount.Value);
					double length2 = ((blendAmount > num5) ? num5 : blendAmount.Value);
					curve = curve.Trim(CurveEnd.End, length);
					curve2 = curve2.Trim(CurveEnd.Start, length2);
					GemOffsetCurve.BlendCorner(ref curve, ref curve2);
					list4[index] = curve;
					list4[num3] = curve2;
				}
			}
		}
		Curve[] array = Curve.JoinCurves(list4);
		rail = array[0];
	}

	public static Point3d LeftPointAngled(CSChannelGemData gemData, double scale, double wallWidth)
	{
		return PointAngled(gemData, gemData.UnitLeftVector, scale, wallWidth);
	}

	public static Point3d RightPointAngled(CSChannelGemData gemData, double scale, double wallWidth)
	{
		return PointAngled(gemData, gemData.UnitRightVector, scale, wallWidth);
	}

	private static Point3d PointAngled(CSChannelGemData gemData, Vector3d dir, double scale, double wallWidth)
	{
		double num = gemData.VectorLength + wallWidth;
		double num2 = ((gemData.Corner == ChannelCornerType.corner90) ? scale : 1.0) / Math.Sin(gemData.CornerAngle / 2.0);
		Vector3d vector3d = dir * num;
		vector3d.X *= num2;
		vector3d.Y *= num2;
		return gemData.Center + vector3d;
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
		int num = gemDatas.Length;
		int num2 = num;
		for (int i = 0; i < num2; i++)
		{
			Vector3d vector3d = gemDatas[(i - 1 + num) % num].Center - gemDatas[i].Center;
			Vector3d vector3d2 = gemDatas[(i + 1) % num].Center - gemDatas[i].Center;
			vector3d.Unitize();
			vector3d2.Unitize();
			Vector3d unitForwardVector = vector3d2 - vector3d;
			unitForwardVector.Unitize();
			gemDatas[i].UnitForwardVector = unitForwardVector;
			gemDatas[i].CornerAngle = CSValueHandleHelper.AngleBetweenVectors(vector3d, vector3d2);
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
		crv0.NormalizedLengthParameter(cornerSplitPercent, out var t);
		crv1.NormalizedLengthParameter(1.0 - cornerSplitPercent, out var t2);
		crv0 = crv0.Trim(crv0.Domain.Min, t);
		crv1 = crv1.Trim(t2, crv1.Domain.Max);
	}

	public static Curve TrimCrvToPoints(Curve crv, Point3d p0, Point3d p1)
	{
		Curve curve = crv.DuplicateCurve();
		curve.ClosestPoint(p0, out var t);
		curve.ChangeClosedCurveSeam(t);
		curve.ClosestPoint(p1, out t);
		return curve.Trim(curve.Domain.Min, t);
	}
}
