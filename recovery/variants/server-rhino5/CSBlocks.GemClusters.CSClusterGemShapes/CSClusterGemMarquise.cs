using CSBlockDataUtils.CSGemOffsetCurves;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemMarquise
{
	public static bool PlaceGems(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems);
		}
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
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
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners(new Curve[2] { curve2, curve }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		for (int j = 0; j < array.Length; j++)
		{
			CSClusterGemUtils.GemCurve(array[j], array[(j + array.Length - 1) % array.Length], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerGemType[j], inCluster.InProngCornerGemType[(j + 1) % array.Length], inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array.Length], inCluster.InProngCornerMetalType[j], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
