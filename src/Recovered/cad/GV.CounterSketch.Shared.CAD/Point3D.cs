using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public sealed class Point3D
{
	[DataMember]
	public double X { get; set; }

	[DataMember]
	public double Y { get; set; }

	[DataMember]
	public double Z { get; set; }

	public Point3D(double x, double y, double z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public Point3D()
	{
	}
}
