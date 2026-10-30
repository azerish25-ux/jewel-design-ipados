using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSSurface
{
	NurbsSurface GetNurbsSurface();

	Surface GetSurface();

	bool HasNurbsSurface();
}
