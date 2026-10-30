using System.Collections.Concurrent;
using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSMeshable
{
	bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions);
}
