using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemPlacer", "{897F521E-A9EE-4688-94C5-C1B6A32B694F}")]
public sealed class CSGemPlacer : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

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

	public readonly CSOutputChannel OutDisplayGems;

	public CSGemPlacer(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
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
		OutDisplayGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		Curve nurbsCurve = InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double num5 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		origin.Transform(value);
		Vector3d vector3d;
		Vector3d vector3d2;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			vector3d = Vector3d.ZAxis;
			vector3d.Transform(value);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(vector3d, nurbsCurve.TangentAt(nurbsCurve.Domain.Min));
			vector3d2.Unitize();
		}
		else
		{
			vector3d = Vector3d.YAxis;
			vector3d.Transform(value);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), vector3d);
			vector3d2.Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		Curve curve = CSUtilGeometry.OffsetOld(nurbsCurve, vector3d2, origin, 0.0 - (zOffset + num2), closed: true);
		if (curve == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = curve.GetLength();
		Transform transform = Transform.Rotation(angleDegrees, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = Transform.Translation(0.0, boolean ? 0.0 : num3, 0.0);
		Transform transform3 = Transform.Rotation(boolean2 ? 0.0 : angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform transform4 = transform * transform3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, transform4))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		Transform transform5 = (boolean ? Transform.Translation(0.0, 0.0 - num3, 0.0) : Transform.Translation(0.0, num3, 0.0));
		Transform transform6 = (boolean2 ? Transform.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : Transform.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform transform7 = transform * transform6;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform7))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		Transform transform8 = Transform.Translation(0.0, num3, 0.0);
		Transform transform9 = Transform.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform transform10 = transform * transform9;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform10))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		int num6 = integer % 2;
		int num7 = integer / 2;
		num5 = ((num6 <= 0) ? (num5 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length) : (num5 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length / 2.0));
		double num8 = ((num6 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
		for (int num9 = num7 - 1; num9 >= 0; num9--)
		{
			double num10 = num5 + (num8 + (double)num9 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / length;
			if (num10 > 1.0)
			{
				num10 -= 1.0;
			}
			if (num10 < 0.0)
			{
				num10 += 1.0;
			}
			if (curve.NormalizedLengthParameter(num10, out var t))
			{
				Vector3d vector3d3 = Vector3d.CrossProduct(curve.TangentAt(t), vector3d2);
				vector3d3.Unitize();
				Vector3d destination = vector3d3 * num4 + vector3d * (1.0 - num4);
				if (destination.Length < 0.001)
				{
					destination = Vector3d.ZAxis;
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
		if (num6 > 0)
		{
			double num11 = num5;
			if (num11 > 1.0)
			{
				num11 -= 1.0;
			}
			if (num11 < 0.0)
			{
				num11 += 1.0;
			}
			if (!curve.NormalizedLengthParameter(num11, out var t2))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d vector3d4 = Vector3d.CrossProduct(curve.TangentAt(t2), vector3d2);
			vector3d4.Unitize();
			Vector3d destination2 = vector3d4 * num4 + vector3d * (1.0 - num4);
			if (destination2.Length < 0.001)
			{
				destination2 = Vector3d.ZAxis;
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
		double num12 = ((num6 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
		for (int i = 0; i < num7; i++)
		{
			double num13 = num5 + (0.0 - (num12 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
			if (num13 > 1.0)
			{
				num13 -= 1.0;
			}
			if (num13 < 0.0)
			{
				num13 += 1.0;
			}
			if (curve.NormalizedLengthParameter(num13, out var t3))
			{
				Vector3d vector3d5 = Vector3d.CrossProduct(curve.TangentAt(t3), vector3d2);
				vector3d5.Unitize();
				Vector3d destination3 = vector3d5 * num4 + vector3d * (1.0 - num4);
				if (destination3.Length < 0.001)
				{
					destination3 = Vector3d.ZAxis;
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
			OutDisplayGems.SetData(cSDataArray);
		}
		return base.Update();
	}
}
