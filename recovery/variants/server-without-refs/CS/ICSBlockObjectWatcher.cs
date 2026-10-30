using RMA.Rhino;

namespace CS;

public interface ICSBlockObjectWatcher
{
	bool CheckForChangeOnAdd(MRhinoObject obj);

	bool CheckForChangeOnDelete(MRhinoObject obj);

	bool CheckForChangeOnReplace(MRhinoObject oldObject, MRhinoObject obj);
}
