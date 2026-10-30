using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("CurveFrom2ViewSimple", "{A33A708A-E462-4333-BC3D-C7EA9AB2F890}")]
public sealed class CSCurveFrom2ViewSimple : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSOutputChannel oCurve;

	public CSCurveFrom2ViewSimple(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve from two views curves 1");
		}
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve from two views curves 2");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curve1");
		}
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing Curve2");
		}
		if (!value.TryGetPlane(out var plane))
		{
			return UpdateMissing("Curve 1 is not planar");
		}
		if (!value2.TryGetPlane(out var plane2))
		{
			return UpdateMissing("Curve 2 is not planar");
		}
		BoundingBox boundingBox = value.GetBoundingBox(accurate: false);
		BoundingBox boundingBox2 = value2.GetBoundingBox(accurate: false);
		Point3d point = boundingBox.FurthestPoint(boundingBox2.Center);
		Point3d other = boundingBox2.FurthestPoint(point);
		double num = point.DistanceTo(other);
		Vector3d vector3d = new Vector3d(plane.Normal);
		Vector3d vector3d2 = new Vector3d(plane2.Normal);
		NurbsCurve nurbsCurve = value.ToNurbsCurve();
		NurbsCurve nurbsCurve2 = value.ToNurbsCurve();
		NurbsCurve nurbsCurve3 = value2.ToNurbsCurve();
		NurbsCurve nurbsCurve4 = value2.ToNurbsCurve();
		nurbsCurve.Translate(vector3d * num);
		nurbsCurve3.Translate(vector3d2 * num);
		nurbsCurve2.Translate(vector3d * (0.0 - num));
		nurbsCurve4.Translate(vector3d2 * (0.0 - num));
		NurbsSurface surfaceA = NurbsSurface.CreateRuledSurface(nurbsCurve, nurbsCurve2);
		NurbsSurface surfaceB = NurbsSurface.CreateRuledSurface(nurbsCurve3, nurbsCurve4);
		Intersection.SurfaceSurface(surfaceA, surfaceB, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _);
		Curve[] array = Curve.JoinCurves(intersectionCurves, CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			return UpdateError("Error with merge");
		}
		base.OutputChannels.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Curve curve = array[i];
			if (curve != null)
			{
				NurbsCurve nurbsCurve5 = curve.ToNurbsCurve();
				if (nurbsCurve5 != null)
				{
					new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Curve, ChannelArrayType.Individual).SetData(new CSOnGeometryData(nurbsCurve5));
				}
			}
		}
		return base.Update();
	}
}
