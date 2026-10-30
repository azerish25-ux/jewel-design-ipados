using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnRingRail_V1", "{19F8C65C-9CB0-4382-9373-55EB0BB56232}")]
public sealed class CSGemOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSOutputChannel OutGem;

	public CSGemOnRingRail1(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRingRail.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemAngle.Double;
		num4 = 1.0 - num4 / 100.0;
		double angleDegrees = InGemRotation.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double param = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(nurbsCurve, param, ref x))
		{
			return UpdateError("OrientToCurveNalp failed");
		}
		Transform transform = default(Transform);
		transform = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0, num2));
		Transform transform2 = default(Transform);
		transform2 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, transform2))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform3 = default(Transform);
		transform3 = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0 - num3, num2));
		Transform transform4 = default(Transform);
		transform4 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.ZAxis, Point3d.Origin));
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform4))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform5 = Transform.Translation(0.0, num3, num2);
		Transform transform6 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform6))
		{
			return UpdateError("Bounding Box Failed");
		}
		int num5 = integer % 2;
		int num6 = integer / 2;
		if (num5 > 0)
		{
			double num7 = Math.Atan((boundingBox.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num8 = Math.Atan((0.0 - boundingBox2.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num9 = num7 + num8;
			double num10 = Math.Atan((boundingBox2.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num11 = Math.Atan((0.0 - boundingBox2.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num12 = num10 + num11;
			double num13 = Math.Atan((0.0 - boundingBox.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num14 = Math.Atan((boundingBox3.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num15 = num13 + num14;
			double num16 = Math.Atan((boundingBox3.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num17 = Math.Atan((0.0 - boundingBox3.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num18 = num16 + num17;
			double num19 = 0.0;
			if (num6 > 0)
			{
				num19 += (num9 + num15 + num12 * (double)(num6 - 1) + num18 * (double)(num6 - 1)) / 2.0;
			}
			double num20 = num19;
			for (int i = 0; i < integer; i++)
			{
				Transform transform7 = CSBlockUtil.CSUtilMath.Rotation(num20, Vector3d.YAxis, Point3d.Origin);
				Transform transform8 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num20) * num4, Vector3d.YAxis, Point3d.Origin);
				CSData data = gem.Duplicate();
				if (i < num6 - 1)
				{
					CSDataHelper.Transform(data, transform7 * x * transform3 * transform8 * transform4);
					num20 -= num12;
				}
				else if (i == num6 - 1)
				{
					CSDataHelper.Transform(data, transform7 * x * transform3 * transform8 * transform4);
					num20 -= num9;
				}
				else if (i >= num6 + 1)
				{
					CSDataHelper.Transform(data, transform7 * x * transform5 * transform8 * transform6);
					num20 -= num18;
				}
				else
				{
					CSDataHelper.Transform(data, transform7 * x * transform * transform8 * transform2);
					num20 -= num15;
				}
				cSDataArray.Append(data);
			}
		}
		else
		{
			double num21 = Math.Atan((0.0 - boundingBox2.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num22 = Math.Atan((boundingBox3.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num23 = num21 + num22;
			double num24 = Math.Atan((boundingBox2.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num25 = Math.Atan((0.0 - boundingBox2.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num26 = num24 + num25;
			double num27 = Math.Atan((boundingBox3.Max.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num28 = Math.Atan((0.0 - boundingBox3.Min.X + num / 2.0) / (nurbsCurve.GetBoundingBox(accurate: true).Max.Z + num2));
			double num29 = num27 + num28;
			double num30 = (num23 + num26 * (double)(num6 - 1) + num29 * (double)(num6 - 1)) / 2.0;
			for (int j = 0; j < integer; j++)
			{
				Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(num30, Vector3d.YAxis, Point3d.Origin);
				Transform transform10 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num30) * num4, Vector3d.YAxis, Point3d.Origin);
				CSData data2 = gem.Duplicate();
				if (j < num6 - 1)
				{
					CSDataHelper.Transform(data2, transform9 * x * transform3 * transform10 * transform4);
					num30 -= num26;
				}
				else if (j >= num6)
				{
					CSDataHelper.Transform(data2, transform9 * x * transform5 * transform10 * transform6);
					num30 -= num29;
				}
				else
				{
					CSDataHelper.Transform(data2, transform9 * x * transform3 * transform10 * transform4);
					num30 -= num23;
				}
				cSDataArray.Append(data2);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
