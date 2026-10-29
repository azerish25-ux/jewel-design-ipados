using System;
using System.Collections.Generic;
using System.Linq;

namespace GemvisionGeometry.Geometry;

public class CurveDefinition : IEquatable<CurveDefinition>
{
	public readonly List<CurveSegment> Segments;

	public bool IsClosed => GetLastPoint().SquareDistance(GetFirstPoint()) < double.Epsilon;

	public bool IsLinear
	{
		get
		{
			if (Segments.Any())
			{
				List<GPoint3F> points = Segments[0].Points;
				if (points == null)
				{
					return false;
				}
				return points.Count == 2;
			}
			return false;
		}
	}

	public CurveDefinition()
	{
		Segments = new List<CurveSegment>();
	}

	public CurveDefinition(CurveDefinition other)
	{
		Segments = new List<CurveSegment>();
		other.Segments.ForEach(delegate(CurveSegment _)
		{
			Segments.Add(_.Duplicate());
		});
	}

	public bool Equals(CurveDefinition other)
	{
		if ((object)other != null)
		{
			if ((object)this != other)
			{
				return object.Equals(Segments, other.Segments);
			}
			return true;
		}
		return false;
	}

	public bool Append(CurveSegment segment)
	{
		if (segment == null || !segment.IsValid())
		{
			return false;
		}
		Segments.Add(segment);
		return true;
	}

	public bool Append(CurveDefinition curve)
	{
		if (curve == null || !curve.IsValid())
		{
			return false;
		}
		bool result = true;
		foreach (CurveSegment segment in curve.Segments)
		{
			if (!Append(segment))
			{
				result = false;
			}
		}
		return result;
	}

	public bool CleanUp()
	{
		if (Segments == null || !Segments.Any())
		{
			return false;
		}
		List<int> list = new List<int>();
		bool flag = false;
		for (int i = 0; i < Segments.Count; i++)
		{
			flag &= Segments[i].RemoveDuplicatedPoints();
			if (Segments[i].IsEmpty)
			{
				list.Add(i);
				flag = true;
			}
		}
		list.Reverse();
		for (int j = 0; j < list.Count; j++)
		{
			Segments.RemoveAt(j);
		}
		return flag;
	}

	public void Divide(float divisor)
	{
		Segments.ForEach(delegate(CurveSegment _)
		{
			_.Divide(divisor);
		});
	}

	public CurveDefinition Duplicate()
	{
		return new CurveDefinition(this);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((CurveDefinition)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public GBoundingBox GetBounds()
	{
		GPoint3D min = new GPoint3D(double.MaxValue, double.MaxValue, 0.0);
		GPoint3D max = new GPoint3D(double.MinValue, double.MinValue, 0.0);
		foreach (CurveSegment segment in Segments)
		{
			GBoundingBox bounds = segment.GetBounds();
			if (bounds.MMin.X < min.X)
			{
				min.X = bounds.MMin.X;
			}
			if (bounds.MMin.Y < min.Y)
			{
				min.Y = bounds.MMin.Y;
			}
			if (bounds.MMax.X > max.X)
			{
				max.X = bounds.MMax.X;
			}
			if (bounds.MMax.Y > max.Y)
			{
				max.Y = bounds.MMax.Y;
			}
		}
		return new GBoundingBox(min, max);
	}

	public GPoint3F GetFirstPoint()
	{
		if (Segments.Any() && Segments[0].Points.Any())
		{
			return Segments[0].Points[0];
		}
		return GPoint3F.Unset;
	}

	public override int GetHashCode()
	{
		if (Segments == null)
		{
			return 0;
		}
		return Segments.GetHashCode();
	}

	public GPoint3F GetLastPoint()
	{
		if (Segments.Any() && Segments[Segments.Count - 1].Points.Any())
		{
			return Segments[Segments.Count - 1].Points[Segments[Segments.Count - 1].Points.Count - 1];
		}
		return GPoint3F.Unset;
	}

	public CurveDefinition GetSubCurve(int initialSegment, int finalSegment)
	{
		if (initialSegment < 0 || finalSegment > Segments.Count || finalSegment < initialSegment)
		{
			throw new ArgumentOutOfRangeException("initialSegment or finalSegmentnot valid argument");
		}
		CurveDefinition curveDefinition = new CurveDefinition();
		for (int i = initialSegment; i <= finalSegment; i++)
		{
			curveDefinition.Segments.Add(Segments[i].Duplicate());
		}
		return curveDefinition;
	}

	public bool IsValid()
	{
		if (Segments != null && Segments.Any())
		{
			return Segments.TrueForAll((CurveSegment _) => _.IsValid());
		}
		return false;
	}

	public static CurveDefinition Join(List<CurveDefinition> curvesToJoin)
	{
		if (curvesToJoin == null || !curvesToJoin.Any())
		{
			return null;
		}
		if (curvesToJoin.Count == 1)
		{
			return curvesToJoin[0];
		}
		CurveDefinition ret = new CurveDefinition();
		curvesToJoin.ForEach(delegate(CurveDefinition _)
		{
			ret.Append(_);
		});
		return ret;
	}

	public void Multiply(float factor)
	{
		Segments.ForEach(delegate(CurveSegment _)
		{
			_.Multiply(factor);
		});
	}

	public static CurveDefinition operator /(CurveDefinition curve, float divisor)
	{
		CurveDefinition curveDefinition = curve.Duplicate();
		curveDefinition.Divide(divisor);
		return curveDefinition;
	}

	public static bool operator ==(CurveDefinition curve1, CurveDefinition curve2)
	{
		if ((object)curve1 == null && (object)curve2 == null)
		{
			return true;
		}
		if ((object)curve1 == null || (object)curve2 == null)
		{
			return false;
		}
		if (curve1.Segments == null && curve2.Segments == null)
		{
			return true;
		}
		if (curve1.Segments == null || curve2.Segments == null)
		{
			return false;
		}
		if (curve1.Segments.Count != curve2.Segments.Count)
		{
			return true;
		}
		return !curve1.Segments.Where((CurveSegment t, int i) => t != curve2.Segments[i]).Any();
	}

	public static bool operator !=(CurveDefinition curve1, CurveDefinition curve2)
	{
		return !(curve1 == curve2);
	}

	public static CurveDefinition operator *(CurveDefinition curve, float factor)
	{
		CurveDefinition curveDefinition = curve.Duplicate();
		curveDefinition.Multiply(factor);
		return curveDefinition;
	}

	public void Reverse()
	{
		Segments?.ForEach(delegate(CurveSegment _)
		{
			_.Reverse();
		});
		Segments?.Reverse();
	}

	public void Scale(GPoint3F scalePoint, float xFactor, float yFactor)
	{
		Segments.ForEach(delegate(CurveSegment _)
		{
			_.Scale(scalePoint, xFactor, yFactor);
		});
	}

	public bool ShiftSeam()
	{
		if (Segments == null || Segments.Count == 0 || !IsClosed)
		{
			return false;
		}
		if (Segments.Count == 0)
		{
			return Segments[0].ShiftPoints();
		}
		CurveSegment item = Segments[0];
		Segments.RemoveAt(0);
		Segments.Add(item);
		return true;
	}

	public static bool SplitDisjointSegments(CurveDefinition curve, out List<CurveDefinition> disjointSegments)
	{
		disjointSegments = new List<CurveDefinition>();
		if (curve == null || curve.Segments == null || curve.Segments.Count < 2)
		{
			return false;
		}
		int initialSegment = 0;
		for (int i = 1; i < curve.Segments.Count; i++)
		{
			if (!(curve.Segments[i].Points[0].SquareDistance(curve.Segments[i - 1].Points[curve.Segments[i - 1].Points.Count - 1]) < double.Epsilon))
			{
				disjointSegments.Add(curve.GetSubCurve(initialSegment, i - 1));
				initialSegment = i;
			}
		}
		disjointSegments.Add(curve.GetSubCurve(initialSegment, curve.Segments.Count - 1));
		return disjointSegments.Count > 1;
	}

	public bool SplitDisjointSegments(out List<CurveDefinition> disjointSegments)
	{
		return SplitDisjointSegments(this, out disjointSegments);
	}

	public void Translate(GVector3F vector)
	{
		foreach (CurveSegment segment in Segments)
		{
			segment.Translate(vector);
		}
	}
}
