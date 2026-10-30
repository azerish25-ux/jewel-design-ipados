using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSTransformable
{
	bool ApplyTransform(Transform transform);
}
