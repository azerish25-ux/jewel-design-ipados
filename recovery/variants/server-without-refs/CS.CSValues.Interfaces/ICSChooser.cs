using CS.CSObjects.CSBlocks;

namespace CS.CSValues.Interfaces;

public interface ICSChooser
{
	int ChooserCount();

	bool DeleteUnused();

	CSBlock GetChooserSource();

	void SetChooserSource(CSBlock csBlock, bool isMaintainingUndo = true);
}
