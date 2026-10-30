using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("UVOnSurfaces", "{9C07D889-B3B6-480e-A48D-C126490CDA7F}")]
public sealed class CSUVOnSurfaces : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iSurfaces;

	public readonly CSInputChannel iU;

	public readonly CSInputChannel iV;

	public readonly CSInputChannel iAngle;

	public readonly CSOutputChannel oCurve;

	public CSUVOnSurfaces(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSurfaces = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iU = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iV = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaces.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		List<NurbsSurface> list = new List<NurbsSurface>();
		NurbsSurface nurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface();
		if (nurbsSurface == null)
		{
			CSDataHelper.GetBrep(flattenArray.GetArrayItem(0), out var value);
			if (value != null)
			{
				if (value.Faces.Count > 0)
				{
					nurbsSurface = value.Faces[0].ToNurbsSurface();
				}
				for (int i = 1; i < value.Faces.Count; i++)
				{
					list.Add(value.Faces[i].ToNurbsSurface());
				}
			}
		}
		if (nurbsSurface == null)
		{
			return UpdateMissing("Surface");
		}
		double value2 = 0.5;
		iU.GetDouble(ref value2);
		double value3 = 0.5;
		iV.GetDouble(ref value3);
		double value4 = 0.0;
		iAngle.GetDouble(ref value4);
		for (int j = 0; j < flattenArray2.GetArrayCount(); j++)
		{
			NurbsSurface nurbsSurface2 = flattenArray2.GetArrayItem(j).GetNurbsSurface();
			if (nurbsSurface2 != null)
			{
				list.Add(nurbsSurface2);
				continue;
			}
			CSDataHelper.GetBrep(flattenArray2.GetArrayItem(j), out var value5);
			if (value5 != null)
			{
				for (int k = 0; k < value5.Faces.Count; k++)
				{
					list.Add(value5.Faces[k].ToNurbsSurface());
				}
			}
		}
		Interval interval = nurbsSurface.Domain(0);
		Interval interval2 = nurbsSurface.Domain(1);
		Point3d point3d = new Point3d(interval.ParameterAt(value2), interval2.ParameterAt(value3), 0.0);
		Vector3d vector3d = new Vector3d(0.0, 1.0, 0.0);
		Transform transformation = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value4), Vector3d.ZAxis, Point3d.Origin);
		vector3d.Transform(transformation);
		Point3d point3d2 = point3d - vector3d * (interval.Length + interval2.Length);
		Point3d to = point3d + vector3d * (interval.Length + interval2.Length);
		Curve curve2D = new LineCurve(point3d2, to);
		curve2D = CSUtilGeometry.Trim2dLineCurveToSurface(curve2D, nurbsSurface);
		Curve curve = nurbsSurface.Pushup(curve2D, 0.01);
		if (curve == null)
		{
			return UpdateError("Error Pushup");
		}
		Curve curve2 = curve;
		List<Curve> list2 = new List<Curve>();
		list2.Add(curve2);
		while (list.Count > 0)
		{
			Point3d pointAtEnd = curve2.PointAtEnd;
			double u = 0.0;
			double v = 0.0;
			int num = -1;
			double num2 = double.MaxValue;
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].ClosestPoint(pointAtEnd, out u, out v) && list[l].PointAt(u, v).DistanceTo(pointAtEnd) < num2)
				{
					num = l;
				}
			}
			if (num < 0)
			{
				break;
			}
			Surface surface = list[num];
			list.RemoveAt(num);
			Vector3d vector3d2 = curve2.TangentAt(curve2.Domain.Max);
			surface.ClosestPoint(pointAtEnd, out u, out v);
			Point3d point3d3 = new Point3d(u, v, 0.0);
			surface.ClosestPoint(pointAtEnd + vector3d2, out u, out v);
			Vector3d vector3d3 = new Vector3d(u - point3d3.X, v - point3d3.Y, 0.0);
			vector3d3.Unitize();
			Interval interval3 = surface.Domain(0);
			Interval interval4 = surface.Domain(1);
			Curve curve2D2 = new LineCurve(point3d3, point3d3 + vector3d3 * (interval3.Length + interval4.Length));
			curve2D2 = CSUtilGeometry.Trim2dLineCurveToSurface(curve2D2, surface);
			Curve curve3 = surface.Pushup(curve2D2, 0.01);
			if (curve3 == null)
			{
				break;
			}
			list2.Add(curve3);
			curve2 = curve3;
		}
		Curve[] array = Curve.JoinCurves(list2.ToArray());
		if (array == null)
		{
			return UpdateError("Failed Merge");
		}
		if (array.Length == 0)
		{
			return UpdateError("Failed Merge");
		}
		NurbsCurve geometry = array[0].ToNurbsCurve();
		oCurve.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
