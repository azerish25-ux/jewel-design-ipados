using System.Collections.Generic;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemSquare
{
	public static bool PlaceGems(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double cornerBlend, double angle, ref CSDataArray outputGems)
	{
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, cornerBlend, ref outputGems);
		}
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true, cornerBlend);
		if (array.Length != 5)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve curve5 = array[4];
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve, curve5 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners(new Curve[4] { mergedCrv, curve2, curve3, curve4 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
		{
			return false;
		}
		int num = cSDataArray2.GetArrayCount() / 2;
		for (int j = num; j < cSDataArray2.GetArrayCount(); j++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(j));
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(0));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double cornerBlend, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true, cornerBlend);
		if (array.Length != 5)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve[] array3 = new Curve[4]
		{
			null,
			array[1],
			array[2],
			array[3]
		};
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2]
		{
			array[0],
			array[4]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		array3[0] = mergedCrv;
		array = array3;
		for (int j = 0; j < array.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array[j], array[(j + array.Length - 1) % array.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
