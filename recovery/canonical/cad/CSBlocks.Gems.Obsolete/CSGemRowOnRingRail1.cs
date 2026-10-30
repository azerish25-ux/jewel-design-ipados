using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemRowOnRingRail_V1", "{742C0478-9805-4869-AE94-A9415CC520B6}")]
public sealed class CSGemRowOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemPosition;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSGemRowOnRingRail1(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRingRail.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double angleDegrees = InGemRotation.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double param = InGemPosition.Double;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(nurbsCurve, param, ref x))
		{
			return UpdateError("Orient to Curve");
		}
		Transform transform = default(Transform);
		transform = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		double num4 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y) + num;
		double num5 = num4 * (double)(integer - 1) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < integer; i++)
		{
			Transform transform2 = Transform.Translation(0.0, num3 + (num5 - (double)i * num4), zOffset + num2);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, x * transform2 * transform);
			cSDataArray.Append(data);
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
