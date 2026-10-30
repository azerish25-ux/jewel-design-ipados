using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("CurveOnRingRail", "{A201BFC3-CA86-4ce5-B13A-DCC84C763EE8}")]
public sealed class CSCurveOnRingRail : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iCurveAngle;

	public readonly CSInputChannel iCurveWidth;

	public readonly CSInputChannel iParam;

	public readonly CSInputChannel iYOffset;

	public readonly CSInputChannel iZRotate;

	public readonly CSOutputChannel oCurve;

	public CSCurveOnRingRail(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurveAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurveWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Ring Rail");
		}
		NurbsCurve value2 = null;
		CSDataHelper.GetNurbsCurve(iCurve.Data, out value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing Curve");
		}
		double value3 = 0.0;
		if (!iCurveAngle.GetDouble(ref value3))
		{
			return UpdateError("Missing Curve Angle");
		}
		double value4 = 0.0;
		if (!iCurveWidth.GetDouble(ref value4))
		{
			return UpdateError("Missing Curve Width");
		}
		double value5 = 0.0;
		if (!iParam.GetDouble(ref value5))
		{
			return UpdateError("Missing Shank Position");
		}
		if (value2.TangentAt(value2.Domain.Min).X < 0.0)
		{
			value2.Reverse();
		}
		double y = value2.PointAtStart.Y;
		double value6 = 0.0;
		iZRotate.GetDouble(ref value6);
		value6 = CSBlockUtil.CSUtilMath.ToRadians(value6);
		Transform identity = Transform.Identity;
		identity = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.ZAxis, value2.PointAtStart);
		value2.Transform(identity);
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("Invalid Curve Boudning Box");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		Transform xform = Transform.Translation(new Vector3d(0.0 - (boundingBox.Max.X - num / 2.0), 0.0, 0.0));
		value2.Transform(xform);
		Transform xform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		value2.Transform(xform2);
		boundingBox = value2.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("Invalid Tight Curve Boudning Box");
		}
		num = boundingBox.Max.X - boundingBox.Min.X;
		num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		Transform xform3 = Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0);
		value2.Transform(xform3);
		LineCurve lineCurve = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, 50.0));
		lineCurve.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.YAxis, Point3d.Origin);
		CurveIntersections curveIntersections = Intersection.CurveCurve(value, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count == 0)
		{
			return UpdateError("Rail doesn't Intersect curve");
		}
		Surface surface = Surface.CreateExtrusion(value.Trim(new Interval(0.0, curveIntersections[0].ParameterA)).ToNurbsCurve(), Vector3d.YAxis * (0.0 - value4));
		if (surface == null)
		{
			return UpdateError("Error Extrude");
		}
		Transform xform4 = Transform.Translation(new Vector3d(0.0, value4 / 2.0, 0.0));
		surface.Transform(xform4);
		Interval interval = surface.Domain(0);
		Interval interval2 = surface.Domain(1);
		Transform xform5 = Transform.Scale(Plane.WorldXY, (interval[1] - interval[0]) / num, (interval2[1] - interval2[0]) / num2, 1.0);
		value2.Transform(xform5);
		Transform xform6 = Transform.Translation(interval[0], interval2[0], 0.0);
		value2.Transform(xform6);
		Curve curve = surface.Pushup(value2.DuplicateCurve(), CSApplicationHelper.Application.Tolerance);
		if (curve == null)
		{
			return UpdateError("Error Push Up");
		}
		curve.ChangeDimension(3);
		double y2 = curve.PointAtStart.Y;
		Transform xform7 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0), Vector3d.XAxis, Point3d.Origin);
		curve.Transform(xform7);
		Transform xform8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0), Vector3d.YAxis, Point3d.Origin);
		curve.Transform(xform8);
		BoundingBox boundingBox2 = curve.GetBoundingBox(accurate: true);
		if (!boundingBox2.IsValid)
		{
			return UpdateError("Error Output Bounding Box");
		}
		Transform xform9 = Transform.Translation(0.0, 0.0 - boundingBox2.Max.Y, 0.0);
		curve.Transform(xform9);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(value, value5, ref x, 1.0);
		double value7 = 0.0;
		iYOffset.GetDouble(ref value7);
		Transform identity2 = Transform.Identity;
		identity2 = Transform.Translation(0.0 - y2 + y + value7, 0.0, 0.0);
		Transform transform = default(Transform);
		transform = x * identity2;
		curve.Transform(transform);
		oCurve.SetData(new CSOnGeometryData(curve, transform, applyTransform: false));
		return base.Update();
	}
}
