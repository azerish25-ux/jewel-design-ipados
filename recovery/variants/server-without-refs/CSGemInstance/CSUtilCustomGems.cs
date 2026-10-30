using System;
using System.Collections.Generic;
using System.Linq;
using CSGemInstance.GemTableData;
using Rhino.Geometry;

namespace CSGemInstance;

public static class CSUtilCustomGems
{
	private static readonly List<Tuple<CSGemInstanceDefinition, int>> _generatedCSGemDataCache = new List<Tuple<CSGemInstanceDefinition, int>>();

	public static IEnumerable<Point3d> CalculateProngPoints(Point3d ptStart, Point3d ptEnd, int segmentCount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Line val = default(Line);
		((Line)(ref val))._002Ector(ptStart, ptEnd);
		Vector3d direction = ((Line)(ref val)).Direction;
		((Vector3d)(ref direction)).Unitize();
		double num = ((Line)(ref val)).Length / (double)segmentCount;
		List<Point3d> list = new List<Point3d>();
		for (int i = 0; i < segmentCount; i++)
		{
			list.Add(ptStart + direction * ((double)i * num));
		}
		return list;
	}

	public static CSGemInstanceDefinition CreateCustomGemFromShapeAndType(GemTableType gemTableType, GemTableShape gemTableShape, int gemTableSizeIndex, int gemTableVersion)
	{
		CSGemInstanceDefinition cSGemInstanceDefinition = _generatedCSGemDataCache.FirstOrDefault((Tuple<CSGemInstanceDefinition, int> gemData) => gemData.Item1.GemTableType == gemTableType && gemData.Item1.GemTableShape == gemTableShape && gemData.Item2 == gemTableSizeIndex)?.Item1;
		if (cSGemInstanceDefinition != null)
		{
			return cSGemInstanceDefinition;
		}
		if (gemTableType == GemTableType.Diamond && gemTableShape == GemTableShape.BaguetteTapered && GemTableHelper.GetGem(gemTableType, gemTableShape).SizeTable(gemTableVersion).ElementAt(gemTableSizeIndex) is GemSizeTapered gemSizeTapered)
		{
			double num = 0.6 * ((gemSizeTapered.Width + gemSizeTapered.Width2) / 2.0);
			double girdleThickness = 0.05 * num;
			cSGemInstanceDefinition = new CSGemInstanceDefinitionCustomBaguetteTapered(gemSizeTapered.Height, gemSizeTapered.Width2, gemSizeTapered.Width, num, girdleThickness);
		}
		_generatedCSGemDataCache.Add(new Tuple<CSGemInstanceDefinition, int>(cSGemInstanceDefinition, gemTableSizeIndex));
		return cSGemInstanceDefinition;
	}
}
