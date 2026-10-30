using CSGemInstance;

namespace CS.Parametric.Interfaces;

public interface IGemSource
{
	void ReloadGems();

	void ReplaceGem(CSGemInstanceDefinition gemInstanceDefinition);
}
