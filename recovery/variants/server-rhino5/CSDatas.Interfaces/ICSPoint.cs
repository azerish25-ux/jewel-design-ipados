using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSPoint
{
	bool GetPoint(out Point3d value);

	bool HasPoint();
}
