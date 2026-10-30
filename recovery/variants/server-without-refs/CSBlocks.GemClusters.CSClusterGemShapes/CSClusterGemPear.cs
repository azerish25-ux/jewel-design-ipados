using System.Collections.Generic;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

internal sealed class CSClusterGemPear
{
	public static bool PlaceGems(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems);
		}
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[0],
			array[1]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[2],
			array[3]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)array, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		((GeometryBase)mergedCrv).Translate(centerGem.Normal * offsetRailZ);
		double num = inCluster.InProngCornerPosition[0];
		if (num > 0.01)
		{
			double num2 = default(double);
			mergedCrv.NormalizedLengthParameter(num, ref num2);
			mergedCrv.ChangeClosedCurveSeam(num2);
		}
		Curve[] array2 = (Curve[])(object)new Curve[inCluster.InProngCornerPosition.Count];
		double num3 = default(double);
		double max = default(double);
		for (int i = 0; i < inCluster.InProngCornerPosition.Count; i++)
		{
			mergedCrv.NormalizedLengthParameter(inCluster.InProngCornerPosition[i] - num, ref num3);
			if (i == inCluster.InProngCornerPosition.Count - 1)
			{
				Interval domain = mergedCrv.Domain;
				max = ((Interval)(ref domain)).Max;
			}
			else
			{
				mergedCrv.NormalizedLengthParameter(inCluster.InProngCornerPosition[i + 1] - num, ref max);
			}
			array2[i] = mergedCrv.Trim(num3, max);
		}
		for (int j = 0; j < array2.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array2[j], array2[(j + array2.Length - 1) % array2.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array2.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array2.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
