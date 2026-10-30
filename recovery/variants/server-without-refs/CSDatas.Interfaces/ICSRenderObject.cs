using System.Collections.Concurrent;
using CSMaterials;
using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSRenderObject
{
	bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> renderObjects, Transform transform);
}
