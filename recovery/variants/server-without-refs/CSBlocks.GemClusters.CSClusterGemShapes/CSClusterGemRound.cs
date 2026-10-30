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
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems);
		}
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, doTransformUp: true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		((GeometryBase)curve).Translate(centerGem.Normal * offsetRailZ);
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
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSGemData cSGemData = (CSGemData)gemDefinition.Duplicate();
			cSGemData.SetUserString("Corner", "0");
			CSDataHelper.Transform(cSGemData, val * xTransformLocal);
			outputGems.Append(cSGemData);
		}
		return true;
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, doTransformUp: true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		((GeometryBase)curve).Translate(centerGem.Normal * offsetRailZ);
		double num = inCluster.InProngCornerPosition[0];
		if (num > 0.01)
		{
			double num2 = default(double);
			curve.NormalizedLengthParameter(num, ref num2);
			curve.ChangeClosedCurveSeam(num2);
		}
		Curve[] array = (Curve[])(object)new Curve[inCluster.InProngCornerPosition.Count];
		double num3 = default(double);
		double max = default(double);
		for (int i = 0; i < inCluster.InProngCornerPosition.Count; i++)
		{
			curve.NormalizedLengthParameter(inCluster.InProngCornerPosition[i] - num, ref num3);
			if (i == inCluster.InProngCornerPosition.Count - 1)
			{
				Interval domain = curve.Domain;
				max = ((Interval)(ref domain)).Max;
			}
			else
			{
				curve.NormalizedLengthParameter(inCluster.InProngCornerPosition[i + 1] - num, ref max);
			}
			array[i] = curve.Trim(num3, max);
		}
		for (int j = 0; j < array.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array[j], array[(j + array.Length - 1) % array.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
