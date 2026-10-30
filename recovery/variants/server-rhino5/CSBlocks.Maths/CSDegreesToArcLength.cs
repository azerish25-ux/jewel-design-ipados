using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Maths;

[CSBlock("DegreesToArcLength", "{C4C09E7A-B67D-4276-B060-41702227FC00}")]
public sealed class CSDegreesToArcLength : CSBlock
{
	public readonly CSInputChannel InFirstGemAngle;

	public readonly CSInputChannel InLastGemAngle;

	public readonly CSInputChannel InGemCurve;

	public readonly CSOutputChannel OutNormalizedStart;

	public readonly CSOutputChannel OutNormalizedEnd;

	public CSDegreesToArcLength(string identifier)
		: base(identifier)
	{
		InFirstGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutNormalizedStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutNormalizedEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		InFirstGemAngle.GetValue(out double value);
		InLastGemAngle.GetValue(out double value2);
		CSDataHelper.GetCurve(InGemCurve, out var value3);
		Point3d pointAtStart = value3.PointAtStart;
		Point3d other = value3.PointAtNormalizedLength(0.5);
		Surface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, value3.PointAtStart.Y + 3.0, 0.0), new Point3d(0.0, value3.PointAtStart.Y + 3.0, pointAtStart.DistanceTo(other)), new Point3d(0.0, value3.PointAtStart.Y - 3.0, pointAtStart.DistanceTo(other)), new Point3d(0.0, value3.PointAtStart.Y - 3.0, 0.0));
		Surface surface2 = (Surface)surface.Duplicate();
		value3.ClosestPoint(value3.PointAtStart, out var t);
		double number;
		if (value == 0.0)
		{
			number = 0.0;
		}
		else
		{
			surface.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.YAxis, Point3d.Origin));
			Point3d pointA = Intersection.CurveSurface(value3, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance)[0].PointA;
			value3.ClosestPoint(pointA, out var t2);
			number = value3.Trim(t, t2).GetLength() / value3.GetLength();
		}
		surface2.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin));
		Point3d pointA2 = Intersection.CurveSurface(value3, surface2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance)[0].PointA;
		value3.ClosestPoint(pointA2, out var t3);
		double number2 = value3.Trim(t, t3).GetLength() / value3.GetLength();
		OutNormalizedStart.SetData(new CSNumberData(number));
		OutNormalizedEnd.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
