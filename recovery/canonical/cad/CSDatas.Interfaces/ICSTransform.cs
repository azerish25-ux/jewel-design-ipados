using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSTransform
{
	bool GetTransform(out Transform value);
}
