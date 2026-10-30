using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Core;

[CSBlock("LineFromTrimBrep", "{724CD432-1166-4730-B450-710ED91B6D78}")]
public sealed class CSLineFromTrimBrep : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iParam1;

	public readonly CSInputChannel iParam2;

	public readonly CSInputChannel iTrim;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oTick1;

	public readonly CSOutputChannel oTick2;

	public CSLineFromTrimBrep(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParam1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTrim = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iBrep.GetFlattenArray();
		Brep value = null;
		CSDataHelper.GetBrep(flattenArray.GetArrayItem(0), out value);
		if (value == null)
		{
			return UpdateMissing("No Brep Surface Input");
		}
		NurbsCurve value2 = null;
		CSDataHelper.GetNurbsCurve(iCurve1.Data, out value2);
		if (value2 == null)
		{
			return UpdateError("Error geting curve");
		}
		NurbsCurve value3 = null;
		CSDataHelper.GetNurbsCurve(iCurve2.Data, out value3);
		if (value3 == null)
		{
			return UpdateMissing("No Curve Input");
		}
		bool value4 = false;
		iTrim.GetBoolean(ref value4);
		double value5 = 0.0;
		iParam1.GetDouble(ref value5);
		double value6 = 0.0;
		iParam2.GetDouble(ref value6);
		double t = 0.0;
		value2.NormalizedLengthParameter(value5, out t);
		double t2 = 0.0;
		value3.NormalizedLengthParameter(value6, out t2);
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
			double u3 = 0.0;
			double v3 = 0.0;
			double u4 = 0.0;
			double v4 = 0.0;
			brepFace2.ClosestPoint(point3d, out u3, out v3);
			brepFace.ClosestPoint(point3d2, out u4, out v4);
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
			Curve curve = new LineCurve(new Point3d(u, v, 0.0), new Point3d(u2, v2, 0.0));
			if (value4)
			{
				List<NurbsCurve> list = new List<NurbsCurve>();
				for (int i = 0; i < brepFace5.Loops.Count; i++)
				{
					BrepLoop brepLoop = brepFace5.Loops[i];
					for (int j = 0; j < brepLoop.Trims.Count; j++)
					{
						list.Add(brepLoop.Trims[j].ToNurbsCurve());
					}
				}
				List<double> list2 = new List<double>();
				foreach (NurbsCurve item in list)
				{
					CurveIntersections curveIntersections = Intersection.CurveCurve(curve, item, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
					for (int k = 0; k < curveIntersections.Count; k++)
					{
						list2.Add(curveIntersections[k].ParameterA);
					}
				}
				if (list2.Count >= 2)
				{
					curve = curve.Trim(new Interval(list2[0], list2[list2.Count - 1]));
				}
			}
			Curve geometry3 = brepFace5.Pushup(curve, 0.01);
			oCurve.SetData(new CSOnGeometryData(geometry3));
			return base.Update();
		}
		return UpdateError("Unable to create line");
	}
}
