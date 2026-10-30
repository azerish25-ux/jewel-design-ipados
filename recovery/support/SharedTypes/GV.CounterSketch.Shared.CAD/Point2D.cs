using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public class Point2D
{
	[DataMember]
	public double X { get; set; }

	[DataMember]
	public double Y { get; set; }

	public Point2D(double sx, double sy)
	{
		X = sx;
		Y = sy;
	}

	public Point2D()
	{
	}
}
