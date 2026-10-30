using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemUtils
{
	public static void GemCurve(Curve crv, Curve lastCrv, CSGemData gemDef, double gemSize, double gemSpacing, int startType, int endType, double startSpacing, double endSpacing, int cornerMetalType, Vector3d GemUp, Transform xTransLocal, ref CSDataArray outputGems)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		double length = crv.GetLength();
		double num = length;
		int num2 = 0;
		double num3 = 1.0;
		double num4 = 0.0;
		if (startType != 0 && endType != 0)
		{
			num -= startSpacing / 2.0 + endSpacing / 2.0;
			num2 = (int)(num / (gemSize + gemSpacing));
			num3 = num / (double)num2;
			num4 = endSpacing / 2.0 + num3 / 2.0;
		}
		else if (startType == 0 && endType == 0)
		{
			num2 = (int)(num / (gemSize + gemSpacing));
			num3 = num / (double)num2;
			num4 = 0.0;
		}
		else if (startType == 0 && endType != 0)
		{
			num -= endSpacing / 2.0;
			num2 = (int)(num / ((gemSize + gemSpacing) / 2.0));
			if (num2 % 2 == 0)
			{
				num2--;
			}
			num3 = num / (double)num2;
			num2 = (num2 + 1) / 2;
			num4 = 0.0;
			num3 *= 2.0;
		}
		else if (startType != 0 && endType == 0)
		{
			num -= startSpacing / 2.0;
			num2 = (int)(num / ((gemSize + gemSpacing) / 2.0));
			if (num2 % 2 == 0)
			{
				num2--;
			}
			num3 = num / (double)num2;
			num2 = (num2 + 1) / 2 - 1;
			num4 = startSpacing / 2.0 + num3;
			num3 *= 2.0;
		}
		if (startType == 2)
		{
			Vector3d destination = crv.TangentAtStart + lastCrv.TangentAtEnd;
			((Vector3d)(ref destination)).Unitize();
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDef.Up, crv.PointAtStart, destination, GemUp);
			CSGemData cSGemData = (CSGemData)gemDef.Duplicate();
			Transform val2 = Transform.Scale(Plane.WorldXY, startSpacing / cSGemData.GemXSize, 1.0, 1.0);
			cSGemData.SetUserString("Corner", cornerMetalType.ToString());
			cSGemData.SetUserString("IsBead", "1");
			CSDataHelper.Transform(cSGemData, val * xTransLocal * val2);
			outputGems.Append(cSGemData);
		}
		double[] array = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			double num5 = num4 / length;
			array[i] = ((num5 > 1.0) ? 1.0 : num5);
			num4 += num3;
		}
		double[] array2 = crv.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num2; j++)
		{
			CSGemData cSGemData2 = (CSGemData)gemDef.Duplicate();
			Vector3d destination2;
			if (startType == 0 && j == 0)
			{
				destination2 = crv.TangentAtStart + lastCrv.TangentAtEnd;
				((Vector3d)(ref destination2)).Unitize();
				cSGemData2.SetUserString("Corner", cornerMetalType.ToString());
			}
			else
			{
				destination2 = crv.TangentAt(array2[j]);
				cSGemData2.SetUserString("Corner", "0");
			}
			Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDef.Up, crv.PointAt(array2[j]), destination2, GemUp);
			CSDataHelper.Transform(cSGemData2, val3 * xTransLocal);
			outputGems.Append(cSGemData2);
		}
	}

	public static bool PlaceGemCorner(Curve curveLhs, Curve curveRhs, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, double angle, CSDataArray outputGems)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtEnd = curveLhs.PointAtEnd;
		Point3d pointAtStart = curveRhs.PointAtStart;
		if (((Point3d)(ref pointAtEnd)).DistanceTo(pointAtStart) > 0.001)
		{
			return false;
		}
		LineCurve val = new LineCurve(pointAtEnd, pointAtStart);
		Interval domain = ((Curve)val).Domain;
		Point3d destination = ((Curve)val).PointAt(((Interval)(ref domain)).Mid);
		domain = curveLhs.Domain;
		Vector3d val2 = curveLhs.TangentAt(((Interval)(ref domain)).Max);
		domain = curveRhs.Domain;
		Vector3d val3 = curveRhs.TangentAt(((Interval)(ref domain)).Min);
		Vector3d destination2 = val2 + val3;
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, destination, destination2, centerGem.Up);
		CSData data = gemDefinition.Duplicate();
		Vector3d val5 = centerGem.Up;
		((Vector3d)(ref val5)).Unitize();
		val5 = val5 * Math.Sin(angle) / 2.0;
		Transform val6 = Transform.Translation(val5);
		CSDataHelper.Transform(data, val4 * xTransformLocal * val6);
		outputGems.Append(data);
		return true;
	}

	public static bool PlaceGemCorners(IList<Curve> curves, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems, double angle)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < curves.Count; i++)
		{
			int num = i;
			int num2 = num + 1;
			if (num2 >= curves.Count)
			{
				num2 -= curves.Count;
			}
			if (!PlaceGemCorner(curves[num], curves[num2], centerGem, gemDefinition, xTransformLocal, angle, outputGems))
			{
				return false;
			}
		}
		return true;
	}

	public static bool PlaceGemsCurveSide(Curve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, Transform xTransformLocal, CSDataArray outputGems)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		double num = curve.GetLength();
		if (style == 1 && centerGem.GemShape == "Princess")
		{
			num -= 3.0 * gemDefinitionSizeX / 7.0;
		}
		int num2 = (int)(num / (gemDefinitionSizeX + spacing));
		if (num < (double)num2 * gemDefinitionSizeX + (double)Math.Max(num2 - 1, 0) * spacing)
		{
			num2--;
		}
		double[] array = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = GetUnnormalizedParam(i, num2, style);
		}
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = ((style == 0) ? 1 : 0); j < num2; j++)
		{
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, val * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static double GetUnnormalizedParam(int gemCurrentIndex, int gemCount, int style)
	{
		if (style == 0)
		{
			return (double)gemCurrentIndex / (double)gemCount;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (gemCount - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gemCurrentIndex;
	}
}
