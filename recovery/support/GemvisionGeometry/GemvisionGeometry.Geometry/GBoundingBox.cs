using System.Collections.Generic;
using System.Linq;

namespace GemvisionGeometry.Geometry;

public struct GBoundingBox
{
	internal GPoint3D MMin;

	internal GPoint3D MMax;

	public GPoint3D Min => MMin;

	public GPoint3D Max => MMax;

	public static GBoundingBox Empty => new GBoundingBox(double.MaxValue, double.MaxValue, double.MaxValue, double.MinValue, double.MinValue, double.MinValue);

	public GPoint3D Center => new GPoint3D((MMax.Mx + MMin.Mx) / 2.0, (MMax.My + MMin.My) / 2.0, (MMax.Mz + MMin.Mz) / 2.0);

	public GPoint3D Size => new GPoint3D(MMax.Mx - MMin.Mx, MMax.My - MMin.My, MMax.Mz - MMin.Mz);

	public static GBoundingBox Unset => new GBoundingBox(GPoint3D.Unset, GPoint3D.Unset);

	public bool IsUnset
	{
		get
		{
			if (!Min.IsUnset())
			{
				return Max.IsUnset();
			}
			return true;
		}
	}

	public bool IsValid
	{
		get
		{
			if ((IsUnset || !(Size.X > 0.0)) && !(Size.Y > 0.0))
			{
				return Size.Z > 0.0;
			}
			return true;
		}
	}

	public GBoundingBox(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
	{
		MMin.Mx = minX;
		MMax.Mx = maxX;
		MMin.My = minY;
		MMax.My = maxY;
		MMin.Mz = minZ;
		MMax.Mz = maxZ;
	}

	public GBoundingBox(GPoint3D min, GPoint3D max)
	{
		MMin = min;
		MMax = max;
	}

	public GBoundingBox(GBoundingBox other)
	{
		MMin = other.Min.Duplicate();
		MMax = other.Max.Duplicate();
	}

	public GBoundingBox(List<CurveDefinition> crvs)
	{
		bool flag = crvs == null || !crvs.Any();
		MMin = (flag ? GPoint3D.Unset : new GPoint3D(double.MaxValue, double.MaxValue, 0.0));
		MMax = (flag ? GPoint3D.Unset : new GPoint3D(double.MinValue, double.MinValue, 0.0));
		if (flag)
		{
			return;
		}
		foreach (CurveDefinition crv in crvs)
		{
			if (!(crv == null))
			{
				GBoundingBox bounds = crv.GetBounds();
				if (bounds.Min.X < MMin.X)
				{
					MMin.X = bounds.Min.X;
				}
				if (bounds.Min.Y < MMin.Y)
				{
					MMin.Y = bounds.Min.Y;
				}
				if (bounds.Max.X > MMax.X)
				{
					MMax.X = bounds.Max.X;
				}
				if (bounds.Max.Y > MMax.Y)
				{
					MMax.Y = bounds.Max.Y;
				}
			}
		}
	}

	public GBoundingBox(IEnumerable<GPoint3D> pts)
	{
		MMin = new GPoint3D(pts.Min((GPoint3D _) => _.X), pts.Min((GPoint3D _) => _.Y), pts.Min((GPoint3D _) => _.Z));
		MMax = new GPoint3D(pts.Max((GPoint3D _) => _.X), pts.Max((GPoint3D _) => _.Y), pts.Max((GPoint3D _) => _.Z));
	}

	public GBoundingBox Duplicate()
	{
		return new GBoundingBox(this);
	}

	public GBoundingBox(List<GMesh> lstMesh)
	{
		MMin = new GPoint3D(double.MaxValue, double.MaxValue, double.MaxValue);
		MMax = new GPoint3D(double.MinValue, double.MinValue, double.MinValue);
		foreach (GMesh item in lstMesh)
		{
			foreach (GPoint3D vertex in item.Vertices)
			{
				if (vertex.Mx < MMin.Mx)
				{
					MMin.Mx = vertex.Mx;
				}
				if (vertex.My < MMin.My)
				{
					MMin.My = vertex.My;
				}
				if (vertex.Mz < MMin.Mz)
				{
					MMin.Mz = vertex.Mz;
				}
				if (vertex.Mx > MMax.Mx)
				{
					MMax.Mx = vertex.Mx;
				}
				if (vertex.My > MMax.My)
				{
					MMax.My = vertex.My;
				}
				if (vertex.Mz > MMax.Mz)
				{
					MMax.Mz = vertex.Mz;
				}
			}
		}
	}

	public void Inflate(double unitAmount)
	{
		double num = unitAmount * 0.5;
		MMin -= num;
		MMax += num;
	}

	public static GBoundingBox operator /(GBoundingBox box, double divisor)
	{
		return new GBoundingBox(box.MMin / divisor, box.MMax / divisor);
	}

	public static GBoundingBox operator *(GBoundingBox box, double factor)
	{
		return new GBoundingBox(box.MMin * factor, box.MMax * factor);
	}

	public static GBoundingBox Union(GBoundingBox box, GPoint3D point)
	{
		if (!box.IsValid)
		{
			return new GBoundingBox(point, point);
		}
		if (!point.IsValid)
		{
			return box;
		}
		return new GBoundingBox
		{
			MMin = 
			{
				Mx = ((box.MMin.Mx < point.Mx) ? box.MMin.Mx : point.Mx),
				My = ((box.MMin.My < point.My) ? box.MMin.My : point.My),
				Mz = ((box.MMin.Mz < point.Mz) ? box.MMin.Mz : point.Mz)
			},
			MMax = 
			{
				Mx = ((box.MMax.Mx > point.Mx) ? box.MMax.Mx : point.Mx),
				My = ((box.MMax.My > point.My) ? box.MMax.My : point.My),
				Mz = ((box.MMax.Mz > point.Mz) ? box.MMax.Mz : point.Mz)
			}
		};
	}

	public void Union(GBoundingBox box)
	{
		Union(box.MMin);
		Union(box.MMax);
	}

	public void Union(GPoint3D point)
	{
		GBoundingBox gBoundingBox = Union(this, point);
		MMin = gBoundingBox.Min;
		MMax = gBoundingBox.Max;
	}

	public void Transform(GTransform transform)
	{
		MMin.Transform(transform);
		MMax.Transform(transform);
	}
}
