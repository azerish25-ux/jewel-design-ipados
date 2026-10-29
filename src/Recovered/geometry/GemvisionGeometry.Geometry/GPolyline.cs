using System;
using System.Collections.Generic;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public class GPolyline : GList<GPoint3D>
{
	public int PointCount => base.Size;

	public GBoundingBox BoundingBox => new GBoundingBox(MItems);

	public GPoint3D this[int index]
	{
		get
		{
			return MItems[index];
		}
		set
		{
			MItems[index] = value;
		}
	}

	public bool IsValid
	{
		get
		{
			if (base.Size < 2)
			{
				return false;
			}
			if (!MItems[0].IsValid)
			{
				return false;
			}
			for (int i = 1; i < base.Size; i++)
			{
				if (!MItems[i].IsValid)
				{
					return false;
				}
				if (MItems[i] == MItems[i - 1])
				{
					return false;
				}
			}
			if (base.Size < 4 && IsClosed)
			{
				return false;
			}
			return true;
		}
	}

	public bool IsClosed
	{
		get
		{
			if (base.Size <= 2)
			{
				return false;
			}
			return base.First == base.Last;
		}
	}

	public GPolyline()
	{
		MItems = new List<GPoint3D>();
	}

	public void Add(GPoint3D p)
	{
		MItems.Add(p);
	}

	public void RemoveLinearPts()
	{
		for (int i = 0; i < PointCount - 2; i++)
		{
			GVector3D inputVector = MItems[i + 1] - MItems[i];
			GVector3D inputVector2 = MItems[i + 2] - MItems[i];
			if (Math.Abs(GMath.AngleBetweenVectors(inputVector, inputVector2)) < 0.001)
			{
				MItems.RemoveAt(i + 1);
				i--;
			}
		}
	}
}
