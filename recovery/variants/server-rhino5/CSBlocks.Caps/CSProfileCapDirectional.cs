using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Caps;

[CSBlock("ProfileCapDirectional", "{45ebe88f-ca61-4e2d-85e0-d0bf67940c85}")]
public sealed class CSProfileCapDirectional : CSBlock
{
	public CSInputChannel iDomeEnabled { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iDomeThickness { get; }

	public CSInputChannel iDirectionCurves { get; }

	public CSInputChannel iWhichEndOfCurves { get; }

	public CSInputChannel iDirection { get; }

	public CSOutputChannel oBrep { get; }

	public CSProfileCapDirectional(string identifier)
		: base(identifier)
	{
		iDomeEnabled = new CSInputChannel(GetPropertyName(() => iDomeEnabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iDomeThickness = new CSInputChannel(GetPropertyName(() => iDomeThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDirectionCurves = new CSInputChannel(GetPropertyName(() => iDirectionCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
		iWhichEndOfCurves = new CSInputChannel(GetPropertyName(() => iWhichEndOfCurves), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(GetPropertyName(() => iDirection), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public static NurbsSurface RailRevolve(NurbsCurve domecrv, NurbsCurve profile, Vector3d[] v)
	{
		BoundingBox boundingBox = profile.GetBoundingBox(accurate: true);
		NurbsSurface nurbsSurface = NurbsSurface.Create(3, isRational: true, domecrv.Order, profile.Order, domecrv.Points.Count, profile.Points.Count);
		int count = profile.Points.Count;
		int count2 = domecrv.Points.Count;
		for (int i = 0; i < count2; i++)
		{
			Point3d location = domecrv.Points[i].Location;
			Point3d point3d = new Point3d(location);
			point3d.Z = 0.0;
			Point3d point3d2 = point3d;
			double num = point3d2.DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				Point3d p = profile.Points[j].Location;
				p.X *= num;
				p.Y *= num;
				p.Z = location.Z;
				SetTangent(ref p, v, boundingBox);
				double weight = domecrv.Points[i].Weight * profile.Points[j].Weight;
				nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(p, weight));
			}
		}
		for (int k = 0; k < domecrv.Knots.Count; k++)
		{
			nurbsSurface.KnotsU[k] = domecrv.Knots[k];
		}
		for (int l = 0; l < profile.Knots.Count; l++)
		{
			nurbsSurface.KnotsV[l] = profile.Knots[l];
		}
		return nurbsSurface;
	}

	public override bool Update()
	{
		oBrep.SetData(null);
		bool value = false;
		iDomeEnabled.GetBoolean(ref value);
		if (!value)
		{
			return base.Update();
		}
		CSDataHelper.GetCurve(iCurve, out var value2);
		if (value2 == null)
		{
			return base.Update();
		}
		CSData data = iDirectionCurves.Data;
		if (data == null)
		{
			return base.Update();
		}
		if (!(data is CSDataArray cSDataArray))
		{
			return base.Update();
		}
		double value3 = 0.5;
		if (iDomeThickness != null)
		{
			iDomeThickness.GetDouble(ref value3);
		}
		int value4 = 0;
		iWhichEndOfCurves.GetInteger(ref value4);
		int value5 = 1;
		iDirection.GetInteger(ref value5);
		if (value5 == -1)
		{
			value4 = 1 - value4;
		}
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(0), out var value6);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(1), out var value7);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(2), out var value8);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(3), out var value9);
		if (value4 == 1)
		{
			value6.Reverse();
			value7.Reverse();
			value8.Reverse();
			value9.Reverse();
		}
		Plane plane = new Plane(value9.PointAtStart, value6.PointAtStart, value8.PointAtStart);
		if (value4 == 1)
		{
			Curve curve = value6;
			value6 = value9;
			value9 = curve;
			Curve curve2 = value7;
			value7 = value8;
			value8 = curve2;
			value3 = 0.0 - value3;
		}
		NurbsCurve nurbsCurve = value2.ToNurbsCurve();
		Transform transform = Transform.ChangeBasis(Plane.WorldXY, plane);
		nurbsCurve.Transform(transform);
		Transform transform2 = Transform.Translation(new Vector3d(-nurbsCurve.GetBoundingBox(accurate: true).Center));
		nurbsCurve.Transform(transform2);
		Transform xform = transform2 * transform;
		xform.TryGetInverse(out var inverseTransform);
		value6.Transform(xform);
		value7.Transform(xform);
		value8.Transform(xform);
		value9.Transform(xform);
		Vector3d tangentAtStart = value6.TangentAtStart;
		Vector3d tangentAtStart2 = value7.TangentAtStart;
		Vector3d tangentAtStart3 = value8.TangentAtStart;
		Vector3d tangentAtStart4 = value9.TangentAtStart;
		NurbsCurve domecrv = CSUtilGeometry.CreateDomeRC(value3, 1.0);
		Vector3d[] v = new Vector3d[4] { tangentAtStart, tangentAtStart2, tangentAtStart3, tangentAtStart4 };
		NurbsSurface nurbsSurface = RailRevolve(domecrv, nurbsCurve, v);
		nurbsSurface.Transform(inverseTransform);
		oBrep.SetData(new CSOnGeometryData(nurbsSurface.ToBrep(), inverseTransform, applyTransform: false));
		return base.Update();
	}

	private static void SetTangent(ref Point3d p, Vector3d[] v, BoundingBox BB)
	{
		double x = p.X;
		double y = p.Y;
		x -= BB.Min.X;
		y -= BB.Min.Y;
		x /= BB.Diagonal.X;
		y /= BB.Diagonal.Y;
		Vector3d vector3d = v[0] * (1.0 - x) * (1.0 - y) + v[1] * (1.0 - x) * y + v[2] * x * y + v[3] * x * (1.0 - y);
		p.X += vector3d.X * p.Z;
		p.Y += vector3d.Y * p.Z;
	}
}
