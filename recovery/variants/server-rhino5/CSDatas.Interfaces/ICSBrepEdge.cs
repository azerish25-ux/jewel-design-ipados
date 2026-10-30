using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSBrepEdge
{
	BrepEdge GetBrepEdge();

	bool HasBrepEdge();
}
