using Rhino.Geometry;

namespace CSDatas.Interfaces;

public interface ICSCurve
{
	Curve GetCurve();

	NurbsCurve GetNurbsCurve();

	bool HasNurbsCurve();
}
