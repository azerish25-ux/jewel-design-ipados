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
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2]
		{
			array[0],
			array[1]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2]
		{
			array[2],
			array[3]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners(new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)array, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		mergedCrv.Translate(centerGem.Normal * offsetRailZ);
		double num = inCluster.InProngCornerPosition[0];
		if (num > 0.01)
		{
			mergedCrv.NormalizedLengthParameter(num, out var t);
			mergedCrv.ChangeClosedCurveSeam(t);
		}
		Curve[] array2 = new Curve[inCluster.InProngCornerPosition.Count];
		for (int i = 0; i < inCluster.InProngCornerPosition.Count; i++)
		{
			mergedCrv.NormalizedLengthParameter(inCluster.InProngCornerPosition[i] - num, out var t2);
			double t3;
			if (i == inCluster.InProngCornerPosition.Count - 1)
			{
				t3 = mergedCrv.Domain.Max;
			}
			else
			{
				mergedCrv.NormalizedLengthParameter(inCluster.InProngCornerPosition[i + 1] - num, out t3);
			}
			array2[i] = mergedCrv.Trim(t2, t3);
		}
		for (int j = 0; j < array2.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array2[j], array2[(j + array2.Length - 1) % array2.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array2.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array2.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
