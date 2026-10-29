using System;

namespace CS.Parametric.Channels;

[Flags]
public enum ChannelDataType
{
	Any = 1,
	Boolean = 2,
	Integer = 4,
	Double = 8,
	String = 0x10,
	Transform = 0x20,
	Point = 0x40,
	Vector = 0x80,
	Plane = 0x100,
	Curve = 0x200,
	Surface = 0x400,
	Brep = 0x800,
	Mesh = 0x1000,
	RingRail = 0x2000,
	RingRailPlanar = 0x4000,
	OutsideRingRail = 0x8000,
	OutsideRingRailPlanar = 0x10000,
	Profile = 0x20000,
	Gem = 0x40000,
	BrepEdge = 0x80000,
	Unknown = 0x100000
}
