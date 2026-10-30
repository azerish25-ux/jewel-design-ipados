using System;
using System.Collections.Generic;
using System.Linq;

namespace GemvisionGeometry.Geometry;

public class CurveSegment : IEquatable<CurveSegment>
{
	public bool IsClosed
	{
		get
		{
			if (Points != null && Points.Count > 2)
			{
				return Points[0].SquareDistance(Points[Points.Count - 1]) < double.Epsilon;
			}
			return false;
		}
	}

	public bool IsEmpty
	{
		get
		{
			if (Points != null)
			{
				return Points.Count == 0;
			}
			return true;
		}
	}

	public bool IsLinear
	{
		get
		{
			if (Points != null)
			{
				return Points.Count == 2;
			}
			return false;
		}
	}

	public List<GPoint3F> Points { get; }

	public CurveSegment(List<GPoint3F> pts)
	{
		Points = pts;
	}

	public CurveSegment()
		: this(new List<GPoint3F>())
	{
	}

	public CurveSegment(CurveSegment other)
		: this(new List<GPoint3F>(other.Points))
	{
	}

	public bool Equals(CurveSegment other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this != other)
		{
			return object.Equals(Points, other.Points);
		}
		return true;
	}

	public void Divide(float divisor)
	{
		for (int i = 0; i < Points.Count; i++)
		{
			Points[i] /= divisor;
		}
	}

	public CurveSegment Duplicate()
	{
		return new CurveSegment(this);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() == GetType())
		{
			return Equals((CurveSegment)obj);
		}
		return false;
	}

	public GBoundingBox GetBounds()
	{
		return new GBoundingBox(new GPoint3D(Points.Min((GPoint3F _) => _.X), Points.Min((GPoint3F _) => _.Y), 0.0), new GPoint3D(Points.Max((GPoint3F _) => _.X), Points.Max((GPoint3F _) => _.Y), 0.0));
	}

	public override int GetHashCode()
	{
		if (Points == null)
		{
			return 0;
		}
		return Points.GetHashCode();
	}

	public bool IsValid()
	{
		if (IsEmpty || Points.Count < 2)
		{
			return false;
		}
		for (int i = 0; i < Points.Count - 1; i++)
		{
			if (Points[i].SquareDistance(Points[i + 1]) < double.Epsilon)
			{
				return false;
			}
		}
		return true;
	}

	public void Multiply(float factor)
	{
		for (int i = 0; i < Points.Count; i++)
		{
			Points[i] *= factor;
		}
	}

	public static CurveSegment operator /(CurveSegment segment, float divisor)
	{
		List<GPoint3F> pts = new List<GPoint3F>();
		segment.Points.ForEach(delegate(GPoint3F _)
		{
			pts.Add(_ / divisor);
		});
		return new CurveSegment(pts);
	}

	public static bool operator ==(CurveSegment segment1, CurveSegment segment2)
	{
		if ((object)segment1 == null && (object)segment2 == null)
		{
			return true;
		}
		if ((object)segment1 == null || (object)segment2 == null)
		{
			return false;
		}
		if (segment1.Points == null && segment2.Points == null)
		{
			return true;
		}
		if (segment1.Points == null || segment2.Points == null)
		{
			return false;
		}
		if (segment1.Points.Count != segment2.Points.Count)
		{
			return false;
		}
		return !segment1.Points.Where((GPoint3F t, int i) => t != segment2.Points[i]).Any();
	}

	public static bool operator !=(CurveSegment segment1, CurveSegment segment2)
	{
		return !(segment1 == segment2);
	}

	public static CurveSegment operator *(CurveSegment segment, float factor)
	{
		List<GPoint3F> pts = new List<GPoint3F>();
		segment.Points.ForEach(delegate(GPoint3F _)
		{
			pts.Add(_ * factor);
		});
		return new CurveSegment(pts);
	}

	public bool RemoveDuplicatedPoints()
	{
		if (Points == null || Points.Count < 2)
		{
			return false;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < Points.Count - 1; i++)
		{
			if (!(Points[i].SquareDistance(Points[i + 1]) > double.Epsilon))
			{
				list.Add(i);
			}
		}
		list.Reverse();
		for (int j = 0; j < list.Count; j++)
		{
			Points.RemoveAt(j);
		}
		return list.Any();
	}

	public void Reverse()
	{
		Points.Reverse();
	}

	public void Scale(GPoint3F scalePoint, float xFactor, float yFactor)
	{
		for (int i = 0; i < Points.Count; i++)
		{
			float x = (Points[i].X - scalePoint.X) * xFactor + scalePoint.X;
			float y = (Points[i].Y - scalePoint.Y) * yFactor + scalePoint.Y;
			Points[i] = new GPoint3F(x, y, Points[i].Z);
		}
	}

	public bool ShiftPoints()
	{
		if (!IsClosed)
		{
			return false;
		}
		GPoint3F item = Points[0];
		Points.RemoveAt(0);
		Points.Add(item);
		return true;
	}

	public void Translate(GVector3F vector)
	{
		for (int i = 0; i < Points.Count; i++)
		{
			Points[i] += vector;
		}
	}
}
