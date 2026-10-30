using System;
using CS;
using CS.CSDataUtilities;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemRound
{
	public static bool PlaceGems(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems);
		}
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		curve.Translate(centerGem.Normal * offsetRailZ);
		int num = (int)(curve.GetLength() / (gemDefinitionSizeX + spacing));
		if (isEven && num % 2 == 1)
		{
			num--;
		}
		double[] array = new double[num];
		double num2 = 1.0 / (double)num;
		double num3 = fixPosition % num2;
		for (int i = 0; i < num; i++)
		{
			array[i] = num3;
			num3 += num2;
			if (num3 > 1.0)
			{
				num3 -= 1.0;
			}
		}
		Array.Sort(array);
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num; j++)
		{
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSGemData cSGemData = (CSGemData)gemDefinition.Duplicate();
			cSGemData.SetUserString("Corner", "0");
			CSDataHelper.Transform(cSGemData, transform * xTransformLocal);
			outputGems.Append(cSGemData);
		}
		return true;
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		curve.Translate(centerGem.Normal * offsetRailZ);
		double num = inCluster.InProngCornerPosition[0];
		if (num > 0.01)
		{
			curve.NormalizedLengthParameter(num, out var t);
			curve.ChangeClosedCurveSeam(t);
		}
		Curve[] array = new Curve[inCluster.InProngCornerPosition.Count];
		for (int i = 0; i < inCluster.InProngCornerPosition.Count; i++)
		{
			curve.NormalizedLengthParameter(inCluster.InProngCornerPosition[i] - num, out var t2);
			double t3;
			if (i == inCluster.InProngCornerPosition.Count - 1)
			{
				t3 = curve.Domain.Max;
			}
			else
			{
				curve.NormalizedLengthParameter(inCluster.InProngCornerPosition[i + 1] - num, out t3);
			}
			array[i] = curve.Trim(t2, t3);
		}
		for (int j = 0; j < array.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array[j], array[(j + array.Length - 1) % array.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
