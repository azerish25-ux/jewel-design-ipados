using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("RingRailOpen", "{682CFB86-0EBF-4a50-AEB2-710CE3644040}")]
public sealed class CSRingRailOpen : CSBlock
{
	public readonly CSInputChannel IRingRadius;

	public readonly CSInputChannel ICurve;

	public readonly CSInputChannel ISpan;

	public readonly CSInputChannel IBlend;

	public readonly CSInputChannel IWidthLock;

	public readonly CSInputChannel IWidth;

	public readonly CSInputChannel IMirror;

	public readonly CSOutputChannel ORingRail;

	public CSRingRailOpen(string identifier)
		: base(identifier)
	{
		IRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		ICurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		ISpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IWidthLock = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		ORingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		if (!IRingRadius.GetDouble(ref value))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Ring Rail\n");
		}
		CSDataHelper.GetNurbsCurve(ICurve.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Ring Rail Custom:  Missing Curve\n");
		}
		double value3 = 0.0;
		if (!ISpan.GetDouble(ref value3))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Span\n");
		}
		double value4 = 0.0;
		if (!IBlend.GetDouble(ref value4))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Blend\n");
		}
		bool value5 = false;
		IMirror.GetBoolean(ref value5);
		bool value6 = false;
		IWidthLock.GetBoolean(ref value6);
		double value7 = 0.0;
		IWidth.GetDouble(ref value7);
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		value2.Transform(xform);
		if (value2.PointAtStart.X > value2.PointAtEnd.X)
		{
			value2.Reverse();
		}
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		double num = boundingBox.Max.X - boundingBox.Min.X;
		_ = boundingBox.Max.Y;
		_ = boundingBox.Min.Y;
		value3 = value3 / 2.0 / 10.0;
		value4 /= 10.0;
		xform = Transform.Translation(0.0 - boundingBox.Min.X, 0.0, 0.0 - boundingBox.Min.Z);
		value2.Transform(xform);
		xform = Transform.Scale(Plane.WorldXY, value4 / num, 1.0, 0.0);
		value2.Transform(xform);
		xform = Transform.Translation(value3, 0.0, 0.0);
		value2.Transform(xform);
		NurbsCurve nurbsCurve = new NurbsCurve(value2);
		xform = ((!value5) ? Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0) : Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0));
		nurbsCurve.Transform(xform);
		xform = Transform.Translation(36.0, 0.0, 0.0);
		nurbsCurve.Transform(xform);
		Point3d point3d = default(Point3d);
		Vector3d vector3d = default(Vector3d);
		Vector3d vector3d2 = default(Vector3d);
		Point3d pointAtEnd = value2.PointAtEnd;
		point3d = nurbsCurve.PointAtEnd;
		vector3d = value2.TangentAt(value2.Domain.Max);
		vector3d2 = nurbsCurve.TangentAt(nurbsCurve.Domain.Max);
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(pointAtEnd, point3d, vector3d, vector3d2);
		Curve[] array = Curve.JoinCurves(new Curve[3] { value2, nurbsCurve2, nurbsCurve });
		if (array.Length != 1)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		value2 = array[0].ToNurbsCurve();
		Transform xform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		value2.Transform(xform2);
		BoundingBox boundingBox2 = value2.GetBoundingBox(accurate: true);
		if (!boundingBox2.IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		double num2 = boundingBox2.Max.X - boundingBox2.Min.X;
		Transform xform3 = Transform.Translation(0.0 - boundingBox2.Min.X, 0.0, 0.0);
		value2.Transform(xform3);
		Surface surface = Surface.CreateExtrusion(new Circle(Plane.WorldZX, Point3d.Origin, value).ToNurbsCurve(), Vector3d.YAxis * (0.0 - num2));
		if (surface == null)
		{
			return UpdateError("RhinoExtrudeCurveStraight failed");
		}
		xform = Transform.Translation(0.0, 0.0 - boundingBox2.Min.X, 0.0);
		surface.Transform(xform);
		xform = ((!value6) ? Transform.Scale(Plane.WorldXY, 1.0, value7 / num2, 1.0) : Transform.Scale(Plane.WorldXY, 1.0, 1.0, 1.0));
		surface.Transform(xform);
		Interval interval = surface.Domain(0);
		Interval interval2 = surface.Domain(1);
		Transform xform4 = Transform.Scale(Plane.WorldXY, (interval[1] - interval[0]) / num2, (interval2[1] - interval2[0]) / 36.0, 1.0);
		value2.Transform(xform4);
		Curve curve = surface.Pushup(value2, CSApplicationHelper.Application.Tolerance);
		if (curve == null)
		{
			return UpdateError("Pushup failed");
		}
		curve.ChangeDimension(3);
		ORingRail.SetData(new CSRingRailData(curve, value));
		return base.Update();
	}
}
