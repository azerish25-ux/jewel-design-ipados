using System;

namespace CS.Reporting;

[Flags]
public enum IOReportingTypes
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
	MiscArray = 0x200
}
