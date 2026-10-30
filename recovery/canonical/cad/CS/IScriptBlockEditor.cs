using CS.CSValues;

namespace CS;

public interface IScriptBlockEditor
{
	void CloseScriptEditor();

	bool IsScriptEditorOpen();

	void OpenScriptEditor(ref CSScript csScript);
}
