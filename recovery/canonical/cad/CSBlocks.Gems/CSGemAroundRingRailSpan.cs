using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Gems;

[CSBlock("GemAroundRingRailSpan", "{CACCABCC-B8C5-46e6-9450-6DB2B7B407AB}")]
public sealed class CSGemAroundRingRailSpan : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InSpan;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InZOffset;

	public readonly CSInputChannel InYOffset;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InRotation;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSGemAroundRingRailSpan(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		double angleDegrees = InSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InSpacing.Double;
		double num2 = InZOffset.Double;
		double dy = InYOffset.Double;
		double angleDegrees2 = InAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(InRotation.Double);
		double num3 = InPosition.Double;
		int integer = InPlacementMethod.Integer;
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.ZAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, dy, 0.0);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		double num4 = boundingBox.Max.X - boundingBox.Min.X;
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		origin.Transform(value);
		Vector3d vector3d;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			Vector3d zAxis = Vector3d.ZAxis;
			zAxis.Transform(value);
			zAxis.Unitize();
			vector3d = Vector3d.CrossProduct(zAxis, nurbsCurve.TangentAt(nurbsCurve.Domain.Min));
			vector3d.Unitize();
		}
		else
		{
			Vector3d zAxis = Vector3d.YAxis;
			zAxis.Transform(value);
			zAxis.Unitize();
			vector3d = Vector3d.CrossProduct(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), zAxis);
			vector3d.Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve curve = CSUtilGeometry.OffsetOld(nurbsCurve, -vector3d, origin, zOffset + num2, closed: true);
		if (curve == null)
		{
			return UpdateError("Offset Rail");
		}
		double num5 = double.NaN;
		num5 = curve.GetLength();
		if (double.IsNaN(num5))
		{
			return UpdateError("Offset Rail Length");
		}
		double num6;
		if (angleDegrees <= 0.0)
		{
			num6 = 0.0;
		}
		else if (angleDegrees >= Math.PI * 2.0)
		{
			num6 = num5;
		}
		else
		{
			LineCurve lineCurve = new LineCurve(origin, origin + new Vector3d(0.0, 0.0, num5));
			lineCurve.Rotate(angleDegrees, Vector3d.YAxis, Point3d.Origin);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count != 1)
			{
				return UpdateError("Intersection");
			}
			num6 = curve.GetLength(CSApplicationHelper.Application.Tolerance, new Interval(curve.Domain.Min, curveIntersections[0].ParameterA));
			if (double.IsNaN(num6) || num6 <= 0.0)
			{
				return UpdateError("Intersection Length");
			}
		}
		int num7 = (int)(num5 / (num4 + num));
		int num8 = (int)(num6 / (num4 + num));
		CSDataArray cSDataArray = new CSDataArray();
		if (num8 > 0)
		{
			int num9 = num8 % 2;
			int num10 = num8 / 2;
			double num11 = 1.0 / (double)num7;
			double num12 = num11 / 2.0;
			double num13 = (double)(1 - num9) * num12 % num11;
			for (int i = 0; i < num7; i++)
			{
				if (i < num9 + num10 || i > num7 - 1 - num10)
				{
					double num14 = num3 + num13;
					if (num14 > 1.0)
					{
						num14 -= 1.0;
					}
					if (num14 < 0.0)
					{
						num14 += 1.0;
					}
					double t = 0.0;
					if (!curve.NormalizedLengthParameter(num14, out t))
					{
						continue;
					}
					Vector3d destination = Vector3d.CrossProduct(curve.TangentAt(t), vector3d);
					destination.Unitize();
					Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, curve.PointAt(t), vector3d, destination);
					CSData data = gem.Duplicate();
					CSDataHelper.Transform(data, transform4 * transform3 * transform * transform2);
					cSDataArray.Append(data);
				}
				num13 += num11;
			}
		}
		else
		{
			UpdateError("Count contains Zero Elements - Cannot create gems.");
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
