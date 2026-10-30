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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		InFirstGemAngle.GetValue(out double value);
		InLastGemAngle.GetValue(out double value2);
		CSDataHelper.GetCurve(InGemCurve, out var value3);
		Point3d pointAtStart = value3.PointAtStart;
		Point3d val = value3.PointAtNormalizedLength(0.5);
		Point3d pointAtStart2 = value3.PointAtStart;
		Point3d val2 = new Point3d(0.0, ((Point3d)(ref pointAtStart2)).Y + 3.0, 0.0);
		pointAtStart2 = value3.PointAtStart;
		Point3d val3 = new Point3d(0.0, ((Point3d)(ref pointAtStart2)).Y + 3.0, ((Point3d)(ref pointAtStart)).DistanceTo(val));
		pointAtStart2 = value3.PointAtStart;
		Point3d val4 = new Point3d(0.0, ((Point3d)(ref pointAtStart2)).Y - 3.0, ((Point3d)(ref pointAtStart)).DistanceTo(val));
		pointAtStart2 = value3.PointAtStart;
		Surface val5 = (Surface)(object)NurbsSurface.CreateFromCorners(val2, val3, val4, new Point3d(0.0, ((Point3d)(ref pointAtStart2)).Y - 3.0, 0.0));
		Surface val6 = (Surface)((GeometryBase)val5).Duplicate();
		double num = default(double);
		value3.ClosestPoint(value3.PointAtStart, ref num);
		double number;
		if (value == 0.0)
		{
			number = 0.0;
		}
		else
		{
			((GeometryBase)val5).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.YAxis, Point3d.Origin));
			Point3d pointA = Intersection.CurveSurface(value3, val5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance)[0].PointA;
			double num2 = default(double);
			value3.ClosestPoint(pointA, ref num2);
			number = value3.Trim(num, num2).GetLength() / value3.GetLength();
		}
		((GeometryBase)val6).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin));
		Point3d pointA2 = Intersection.CurveSurface(value3, val6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance)[0].PointA;
		double num3 = default(double);
		value3.ClosestPoint(pointA2, ref num3);
		double number2 = value3.Trim(num, num3).GetLength() / value3.GetLength();
		OutNormalizedStart.SetData(new CSNumberData(number));
		OutNormalizedEnd.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
