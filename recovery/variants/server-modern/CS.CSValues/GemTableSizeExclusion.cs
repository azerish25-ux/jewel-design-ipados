using System;
using System.Collections.Generic;
using CSGemInstance.GemTableData;

namespace CS.CSValues;

[Serializable]
public sealed class GemTableSizeExclusion
{
	public List<int> ExcludedGemTableSizeIndexes { get; set; }

	public int GemTableVersion { get; set; }

	public string GemShape { get; set; }

	public string GemType { get; set; }

	public GemTableSizeExclusion()
	{
	}

	public GemTableSizeExclusion(string gemShape, string gemType, int gemTableVersion)
	{
		GemShape = gemShape;
		GemType = gemType;
		GemTableVersion = gemTableVersion;
		ExcludedGemTableSizeIndexes = new List<int>();
	}

	public bool IsGemSizeExcluded(int index)
	{
		return ExcludedGemTableSizeIndexes.Contains(index);
	}

	public bool UpdatedCheckToClearExclusions()
	{
		return GemTableVersion != GemTableHelper.GetCurrentGemTableVersion(GemType, GemShape);
	}
}
