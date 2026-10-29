using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public sealed class Point4D
{
	[DataMember]
	public double W { get; set; }

	[DataMember]
	public double X { get; set; }

	[DataMember]
	public double Y { get; set; }

	[DataMember]
	public double Z { get; set; }

	public Point4D(Point4D p)
	{
		X = p.X;
		Y = p.Y;
		Z = p.Z;
		W = p.W;
	}

	public Point4D()
	{
	}
}
