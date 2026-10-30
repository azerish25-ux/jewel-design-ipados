using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSVector
{
	bool GetVector(out Vector3d value);

	bool HasVector();
}
