using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Bails;

[CSBlock("BailPlacerXform", "{BA8AE98D-9D58-49FB-8D6B-FD07B0EA3A5C}")]
public sealed class CSBailPlacerXform : CSBlock
{
	public CSInputChannel InObject { get; }

	public CSOutputChannel OutXform { get; }

	public CSBailPlacerXform(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Mising Object");
		}
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			Brep brep = flattenArray.GetArrayItem(i)?.GetBrep();
			if (brep != null)
			{
				list.Add(brep);
			}
		}
		BoundingBox boundingBox = default(BoundingBox);
		foreach (Brep item2 in list)
		{
			boundingBox.Union(item2.GetBoundingBox(accurate: true));
		}
		Point3d corner = new Point3d(0.0, boundingBox.Min.Y, boundingBox.Min.Z);
		Point3d corner2 = new Point3d(0.0, boundingBox.Max.Y, boundingBox.Min.Z);
		Point3d corner3 = new Point3d(0.0, boundingBox.Max.Y, boundingBox.Max.Z);
		Point3d corner4 = new Point3d(0.0, boundingBox.Min.Y, boundingBox.Max.Z);
		NurbsSurface nurbsSurface = NurbsSurface.CreateFromCorners(corner, corner2, corner3, corner4);
		if (nurbsSurface == null)
		{
			return UpdateError("Intersection Failed");
		}
		Brep brep2 = nurbsSurface.ToBrep();
		if (brep2 == null)
		{
			return UpdateError("Intersection Failed to Brep");
		}
		List<Curve> list2 = new List<Curve>();
		List<Point3d> list3 = new List<Point3d>();
		BoundingBox empty = BoundingBox.Empty;
		foreach (Brep item3 in list)
		{
			if (Intersection.BrepBrep(item3, brep2, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var intersectionPoints))
			{
				Curve[] array = intersectionCurves;
				foreach (Curve curve in array)
				{
					list2.Add(curve);
					empty.Union(curve.GetBoundingBox(accurate: true));
				}
				list3.AddRange(intersectionPoints);
			}
		}
		Point3d corner5 = new Point3d(0.0, empty.Max.Y - 0.001, boundingBox.Min.Z);
		corner2 = new Point3d(0.0, empty.Max.Y, boundingBox.Min.Z);
		corner3 = new Point3d(0.0, empty.Max.Y, boundingBox.Max.Z);
		corner4 = new Point3d(0.0, empty.Max.Y - 0.001, boundingBox.Max.Z);
		nurbsSurface = NurbsSurface.CreateFromCorners(corner5, corner2, corner3, corner4);
		foreach (Curve item4 in list2)
		{
			CurveIntersections curveIntersections = Intersection.CurveSurface(item4, nurbsSurface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count <= 0)
			{
				continue;
			}
			Point3d item = curveIntersections[0].PointA;
			foreach (IntersectionEvent item5 in curveIntersections)
			{
				if (item5.PointA.Y >= item.Y)
				{
					item = item4.PointAt(item5.ParameterA);
				}
			}
			list3.Add(item);
		}
		if (list3.Count <= 0)
		{
			return UpdateError("Intersect Failed");
		}
		Point3d point3d = list3[0];
		foreach (Point3d item6 in list3)
		{
			if (item6.Y > point3d.Y)
			{
				point3d = item6;
			}
		}
		Transform transform = Transform.Translation(point3d.X, point3d.Y, 0.0);
		OutXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
