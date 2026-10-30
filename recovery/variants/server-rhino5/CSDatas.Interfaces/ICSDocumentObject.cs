using System.Collections.Concurrent;
using CSMaterials;
using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSDocumentObject
{
	void AddToDocument();

	bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions);
}
