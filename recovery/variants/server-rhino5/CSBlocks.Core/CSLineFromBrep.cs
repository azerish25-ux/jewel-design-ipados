using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("LineFromBrep", "{9076B94E-E404-47cc-996E-B61B0FC2101F}")]
public sealed class CSLineFromBrep : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iParam1;

	public readonly CSInputChannel iParam2;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oTick1;

	public readonly CSOutputChannel oTick2;

	public CSLineFromBrep(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParam1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetBrep(iBrep.GetFlattenArray().GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("No Brep Surface Input");
		}
		CSDataHelper.GetNurbsCurve(iCurve1.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Curve1 Missing");
		}
		CSDataHelper.GetNurbsCurve(iCurve2.Data, out var value3);
		if (value3 == null)
		{
			return UpdateMissing("No Curve Input");
		}
		double value4 = 0.0;
		iParam1.GetDouble(ref value4);
		double value5 = 0.0;
		iParam2.GetDouble(ref value5);
		value2.NormalizedLengthParameter(value4, out var t);
		value3.NormalizedLengthParameter(value5, out var t2);
		Point3d point3d = value2.PointAt(t);
		Point3d point3d2 = value3.PointAt(t2);
		if (!CSUtilGeometry.GetClosestPointToBrep(value, point3d, out var faceIndex, out var u, out var v))
		{
			return UpdateError("Closest Point 1");
		}
		if (!CSUtilGeometry.GetClosestPointToBrep(value, point3d2, out var faceIndex2, out var u2, out var v2))
		{
			return UpdateError("Closest Point 2");
		}
		if (faceIndex != faceIndex2 && faceIndex2 != -1 && faceIndex != -1)
		{
			BrepFace brepFace = value.Faces[faceIndex];
			BrepFace brepFace2 = value.Faces[faceIndex2];
			brepFace2.ClosestPoint(point3d, out var u3, out var v3);
			brepFace.ClosestPoint(point3d2, out var u4, out var v4);
			if (brepFace2.PointAt(u3, v3).DistanceTo(brepFace.PointAt(u, v)) < brepFace.PointAt(u4, v4).DistanceTo(brepFace2.PointAt(u2, v2)))
			{
				faceIndex = faceIndex2;
				u = u3;
				v = v3;
			}
			else
			{
				faceIndex2 = faceIndex;
				u2 = u4;
				v2 = v4;
			}
		}
		if (faceIndex >= 0)
		{
			BrepFace brepFace3 = value.Faces[faceIndex];
			Point3d point3d3 = brepFace3.PointAt(u, v);
			Vector3d vector3d = brepFace3.NormalAt(u, v);
			if (value.Faces[faceIndex].OrientationIsReversed)
			{
				vector3d.Reverse();
			}
			LineCurve geometry = new LineCurve(point3d3, point3d3 + vector3d);
			oTick1.SetData(new CSOnGeometryData(geometry));
		}
		if (faceIndex2 >= 0)
		{
			BrepFace brepFace4 = value.Faces[faceIndex2];
			Point3d point3d4 = brepFace4.PointAt(u2, v2);
			Vector3d vector3d2 = brepFace4.NormalAt(u2, v2);
			if (value.Faces[faceIndex2].OrientationIsReversed)
			{
				vector3d2.Reverse();
			}
			LineCurve geometry2 = new LineCurve(point3d4, point3d4 + vector3d2);
			oTick2.SetData(new CSOnGeometryData(geometry2));
		}
		if (faceIndex == faceIndex2 && faceIndex >= 0)
		{
			BrepFace brepFace5 = value.Faces[faceIndex];
			LineCurve curve2d = new LineCurve(new Point3d(u, v, 0.0), new Point3d(u2, v2, 0.0));
			Curve geometry3 = brepFace5.Pushup(curve2d, 0.01);
			oCurve.SetData(new CSOnGeometryData(geometry3));
			return base.Update();
		}
		return UpdateError("Unable to create line");
	}
}
