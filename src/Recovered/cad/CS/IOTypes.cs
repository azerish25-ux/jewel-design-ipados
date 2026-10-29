using System;

namespace CS;

[Flags]
public enum IOTypes
{
	Nothing = 0,
	Everything = 1,
	Values = 2,
	Gems = 4,
	Curves = 8,
	Surfaces = 0x10,
	ValuesArray = 0x20,
	GemsArray = 0x40,
	CurvesArray = 0x80,
	SurfacesArray = 0x100,
	MiscArray = 0x200,
	RingRails = 0x1000,
	OutsideRingRails = 0x2000,
	PlanarRingRails = 0x4000,
	PlanarOutsideRingRails = 0x8000,
	Points = 0x10000,
	PointsArray = 0x20000,
	Xforms = 0x40000
}
