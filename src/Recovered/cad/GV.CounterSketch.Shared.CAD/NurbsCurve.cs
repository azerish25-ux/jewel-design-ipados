using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.CAD;

[DataContract]
public sealed class NurbsCurve
{
	public int CVCount => Points.Length;

	[DataMember]
	public int Degree { get; set; }

	[DataMember]
	public int Dimension { get; set; }

	[DataMember]
	public double[] Domain { get; set; }

	[DataMember]
	public bool IsClamped { get; set; }

	[DataMember]
	public bool IsClosed { get; set; }

	[DataMember]
	public int IsRational { get; set; }

	[DataMember]
	public double[] Knots { get; set; }

	[DataMember]
	public int Order { get; set; }

	[DataMember]
	public Point4D[] Points { get; set; }

	public NurbsCurve Duplicate()
	{
		NurbsCurve nurbsCurve = new NurbsCurve
		{
			Points = new Point4D[Points.Length]
		};
		for (int i = 0; i < Points.Length; i++)
		{
			nurbsCurve.Points[i] = new Point4D(Points[i]);
		}
		nurbsCurve.Knots = new double[Knots.Length];
		for (int j = 0; j < Knots.Length; j++)
		{
			nurbsCurve.Knots[j] = Knots[j];
		}
		nurbsCurve.Domain = new double[Domain.Length];
		for (int k = 0; k < Domain.Length; k++)
		{
			nurbsCurve.Domain[k] = Domain[k];
		}
		nurbsCurve.Degree = Degree;
		nurbsCurve.IsClosed = IsClosed;
		nurbsCurve.IsClamped = IsClamped;
		nurbsCurve.Dimension = Dimension;
		nurbsCurve.IsRational = IsRational;
		nurbsCurve.Order = Order;
		return nurbsCurve;
	}

	public void GetCV(int i, ref Point3D point)
	{
		point.X = Points[i].X;
		point.Y = Points[i].Y;
		point.Z = Points[i].Z;
	}

	public void GetCV(int i, ref Point4D point)
	{
		point.X = Points[i].X;
		point.Y = Points[i].Y;
		point.Z = Points[i].Z;
		point.W = Points[i].W;
	}

	public void SetCV(int i, Point4D p1)
	{
		Points[i].X = p1.X;
		Points[i].Y = p1.Y;
		Points[i].Z = p1.Z;
		Points[i].W = p1.W;
	}
}
