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

[CSBlock("GemPlacerSpan", "{CE5690A6-DEBD-4c86-92A9-5F4B6638337E}")]
public sealed class CSGemPlacerSpan : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemSpan;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSInputChannel InGemOnOff;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutDisplayGem;

	public CSGemPlacerSpan(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPlacementMethod.Integer;
		double angleDegrees = InGemSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees2 = InGemAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleDegrees3 = InGemRotation.Double;
		angleDegrees3 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees3);
		double num5 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		Point3d point3d = new Point3d(Point3d.Origin);
		point3d.Transform(value);
		Vector3d vector3d;
		Vector3d vector3d2;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			vector3d = new Vector3d(Vector3d.ZAxis);
			vector3d.Transform(value);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(vector3d, nurbsCurve.TangentAt(nurbsCurve.Domain.Min));
			vector3d2.Unitize();
		}
		else
		{
			vector3d = new Vector3d(Vector3d.YAxis);
			vector3d.Transform(value);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), vector3d);
			vector3d2.Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve curve = CSUtilGeometry.OffsetOld(nurbsCurve, -vector3d2, point3d, zOffset + num2, closed: true);
		if (curve == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = curve.GetLength();
		if (length <= 0.0)
		{
			return UpdateError("Offset Curve Length");
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = ((!boolean) ? Transform.Translation(0.0, num3, 0.0) : Transform.Translation(0.0, 0.0, 0.0));
		Transform transform3 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		Transform transform4 = transform * transform3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, transform4))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		Transform transform5 = ((!boolean) ? Transform.Translation(0.0, num3, 0.0) : Transform.Translation(0.0, 0.0 - num3, 0.0));
		Transform transform6 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees3, Vector3d.ZAxis, Point3d.Origin));
		Transform transform7 = transform * transform6;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform7))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		Transform transform8 = Transform.Translation(0.0, num3, 0.0);
		Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform10 = transform * transform9;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform10))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		LineCurve lineCurve = new LineCurve(point3d, point3d + new Vector3d(0.0, 0.0, length));
		lineCurve.Rotate(angleDegrees / 2.0, Vector3d.YAxis, Point3d.Origin);
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count < 1)
		{
			return UpdateError("Intersection");
		}
		double length2 = curve.GetLength(CSApplicationHelper.Application.Tolerance, new Interval(curve.Domain.Min, curveIntersections[0].ParameterA));
		if (double.IsNaN(length2) || length2 < 0.0)
		{
			return UpdateError("Intersection Length");
		}
		int num6 = (int)(length2 / (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num));
		double num7 = length2 % (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num);
		int num8 = (int)(length2 / (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num));
		double num9 = length2 % (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num);
		int num10 = (int)((num7 + num9) / (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X)));
		int num11 = (num6 + num8) / 2;
		num5 = ((num10 <= 0) ? (num5 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length) : (num5 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length / 2.0));
		double num12 = ((num10 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
		for (int num13 = num11 - 1; num13 >= 0; num13--)
		{
			double num14 = num5 + (num12 + (double)num13 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / length;
			if (num14 > 1.0)
			{
				num14 -= 1.0;
			}
			if (num14 < 0.0)
			{
				num14 += 1.0;
			}
			if (curve.NormalizedLengthParameter(num14, out var t))
			{
				Vector3d vector3d3 = Vector3d.CrossProduct(curve.TangentAt(t), vector3d2);
				vector3d3.Unitize();
				Vector3d destination = vector3d3 * num4 + vector3d * (1.0 - num4);
				if (destination.Length < 0.001)
				{
					destination = new Vector3d(Vector3d.ZAxis);
				}
				else
				{
					destination.Unitize();
				}
				Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, curve.PointAt(t), vector3d2, destination);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, transform11 * transform5 * transform7);
				cSDataArray.Append(data);
			}
		}
		if (num10 > 0)
		{
			double num15 = num5;
			if (num15 > 1.0)
			{
				num15 -= 1.0;
			}
			if (num15 < 0.0)
			{
				num15 += 1.0;
			}
			if (!curve.NormalizedLengthParameter(num15, out var t2))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d vector3d4 = Vector3d.CrossProduct(curve.TangentAt(t2), vector3d2);
			vector3d4.Unitize();
			Vector3d destination2 = vector3d4 * num4 + vector3d * (1.0 - num4);
			if (destination2.Length < 0.001)
			{
				destination2 = new Vector3d(Vector3d.ZAxis);
			}
			else
			{
				destination2.Unitize();
			}
			Transform transform12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, curve.PointAt(t2), vector3d2, destination2);
			CSData data2 = gem.Duplicate();
			CSDataHelper.Transform(data2, transform12 * transform2 * transform4);
			cSDataArray.Append(data2);
		}
		double num16 = ((num10 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
		for (int i = 0; i < num11; i++)
		{
			double num17 = num5 + (0.0 - (num16 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
			if (num17 > 1.0)
			{
				num17 -= 1.0;
			}
			if (num17 < 0.0)
			{
				num17 += 1.0;
			}
			if (curve.NormalizedLengthParameter(num17, out var t3))
			{
				Vector3d vector3d5 = Vector3d.CrossProduct(curve.TangentAt(t3), vector3d2);
				vector3d5.Unitize();
				Vector3d destination3 = vector3d5 * num4 + vector3d * (1.0 - num4);
				if (destination3.Length < 0.001)
				{
					destination3 = new Vector3d(Vector3d.ZAxis);
				}
				else
				{
					destination3.Unitize();
				}
				Transform transform13 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, curve.PointAt(t3), vector3d2, destination3);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, transform13 * transform8 * transform10);
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		if (InGemOnOff.Boolean)
		{
			OutDisplayGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
