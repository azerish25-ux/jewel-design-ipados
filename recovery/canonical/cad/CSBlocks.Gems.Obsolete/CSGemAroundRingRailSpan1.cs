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

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemAroundRingRailSpan_V1", "{1ED8711D-7220-4973-9A3A-B17991E57A09}")]
public sealed class CSGemAroundRingRailSpan1 : CSBlock
{
	public readonly CSInputChannel InRail;

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

	public CSGemAroundRingRailSpan1(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail");
		}
		if (!(InGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double angleDegrees = InSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InSpacing.Double;
		double zOffset = InZOffset.Double;
		double dy = InYOffset.Double;
		double angleDegrees2 = InAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(InRotation.Double);
		double num2 = InPosition.Double;
		int integer = InPlacementMethod.Integer;
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.ZAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, dy, 0.0);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		double num3 = boundingBox.Max.X - boundingBox.Min.X;
		CSDataHelper.GetTransformNoArray(InRail.Data, out var value);
		Vector3d vector3d;
		if (!value.IsValid)
		{
			Vector3d a = new Vector3d(Vector3d.ZAxis);
			vector3d = Vector3d.CrossProduct(a, nurbsCurve.TangentAt(nurbsCurve.Domain.Min));
			vector3d.Unitize();
		}
		else
		{
			Vector3d a = new Vector3d(Vector3d.YAxis);
			a.Transform(value);
			a.Unitize();
			vector3d = Vector3d.CrossProduct(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), a);
			vector3d.Unitize();
		}
		CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve curve = CSUtilGeometry.OriginOffset(nurbsCurve, -vector3d, zOffset);
		if (curve == null)
		{
			return UpdateError("Offset Rail");
		}
		double length = curve.GetLength();
		if (double.IsNaN(length) || length <= 0.0)
		{
			return UpdateError("Offset Rail Length");
		}
		double num4;
		if (angleDegrees <= 0.0)
		{
			num4 = 0.0;
		}
		else if (angleDegrees >= Math.PI * 2.0)
		{
			num4 = length;
		}
		else
		{
			LineCurve lineCurve = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, length));
			lineCurve.Rotate(angleDegrees, Vector3d.YAxis, Point3d.Origin);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count != 1)
			{
				return UpdateError("Intersection");
			}
			num4 = curve.GetLength(CSApplicationHelper.Application.Tolerance, new Interval(curve.Domain.Min, curveIntersections[0].ParameterA));
		}
		int num5 = (int)(length / (num3 + num));
		int num6 = (int)(num4 / (num3 + num));
		CSDataArray cSDataArray = new CSDataArray();
		if (num6 > 0)
		{
			int num7 = num6 % 2;
			int num8 = num6 / 2;
			double num9 = 1.0 / (double)num5;
			double num10 = num9 / 2.0;
			double num11 = (double)(1 - num7) * num10 % num9;
			for (int i = 0; i < num5; i++)
			{
				if (i < num7 + num8 || i > num5 - 1 - num8)
				{
					double num12 = num2 + num11;
					if (num12 > 1.0)
					{
						num12 -= 1.0;
					}
					if (num12 < 0.0)
					{
						num12 += 1.0;
					}
					double t = 0.0;
					if (!curve.NormalizedLengthParameter(num12, out t))
					{
						continue;
					}
					Vector3d destination = Vector3d.CrossProduct(curve.TangentAt(t), vector3d);
					destination.Unitize();
					Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, cSGemData.North, cSGemData.Up, curve.PointAt(t), vector3d, destination);
					CSData data = cSGemData.Duplicate();
					CSDataHelper.Transform(data, transform4 * transform3 * transform * transform2);
					cSDataArray.Append(data);
				}
				num11 += num9;
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
