using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacer2Curves", "{77B462BA-C38F-49CD-B9E1-D0212C8BD1EF}")]
public sealed class CSProfilePlacer2Curves : CSBlock
{
	public readonly CSInputChannel iRailBase;

	public readonly CSInputChannel iRailOutside;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iParameter;

	public readonly CSInputChannel iMethod;

	public readonly CSInputChannel iWidth;

	public readonly CSInputChannel iHeightAdd;

	public readonly CSInputChannel iTransform;

	public readonly CSOutputChannel oProfile;

	public CSProfilePlacer2Curves(string identifier)
		: base(identifier)
	{
		iRailBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailOutside = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.OrientationMethodClosestPointOnCurve);
		iWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5.0);
		iHeightAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Identity);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!iRailBase.GetValue(out Curve value))
		{
			return InputError();
		}
		if (!iRailOutside.GetValue(out Curve value2))
		{
			return InputError();
		}
		if (!iProfile.GetValue(out Curve value3))
		{
			return InputError();
		}
		if (!iParameter.GetValue(out double value4))
		{
			return InputError();
		}
		if (!iMethod.GetValue(out int value5))
		{
			return InputError();
		}
		if (!iWidth.GetValue(out double value6))
		{
			return InputError();
		}
		if (!iHeightAdd.GetValue(out double value7))
		{
			return InputError();
		}
		if (!iTransform.GetValue(out Transform value8))
		{
			return InputError();
		}
		if (value4 < 0.0)
		{
			value4 = 0.0;
		}
		if (value4 > 1.0)
		{
			value4 = 1.0;
		}
		value.NormalizedLengthParameter(value4, out var t);
		Point3d point3d = value.PointAt(t);
		Vector3d vector3d = value.TangentAt(t);
		Point3d point3d2;
		if (value5 == 140)
		{
			value2.ClosestPoint(point3d, out var t2);
			point3d2 = value2.PointAt(t2);
		}
		else
		{
			CurveIntersections obj = Intersection.CurvePlane(plane: new Plane(point3d, vector3d), curve: value2, tolerance: CSApplicationHelper.Application.Tolerance);
			double num = double.MaxValue;
			Point3d point3d3 = Point3d.Unset;
			foreach (IntersectionEvent item in obj)
			{
				if (item.IsPoint)
				{
					Point3d pointA = item.PointA;
					double length = (point3d - pointA).Length;
					if (length < num)
					{
						num = length;
						point3d3 = pointA;
					}
				}
			}
			if (!point3d3.IsValid)
			{
				value2.ClosestPoint(point3d, out var t3);
				point3d2 = value2.PointAt(t3);
			}
			else
			{
				point3d2 = point3d3;
			}
		}
		Vector3d vector3d2 = point3d2 - point3d;
		double length2 = vector3d2.Length;
		vector3d2.Unitize();
		Vector3d destination = Vector3d.CrossProduct(vector3d2, vector3d);
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, point3d, destination, vector3d2);
		BoundingBox boundingBox = value3.GetBoundingBox(accurate: true);
		Transform transform2 = Transform.Translation(0.0, 0.0 - boundingBox.Min.Y, 0.0 - boundingBox.Min.Z);
		Transform transform3 = Transform.Scale(Plane.WorldXY, value6 / boundingBox.Diagonal.X, (length2 + value7) / boundingBox.Diagonal.Y, 1.0);
		Transform xform = transform * value8 * transform3 * transform2;
		Curve curve = value3.DuplicateCurve();
		curve.Transform(xform);
		oProfile.SetData(new CSOnGeometryData(curve));
		return base.Update();
	}
}
