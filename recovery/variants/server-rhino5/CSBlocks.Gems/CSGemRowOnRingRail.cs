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

[CSBlock("GemRowOnRingRail", "{E29D0C44-BB21-4bbc-82BC-11284A5A372B}")]
public sealed class CSGemRowOnRingRail : CSBlock
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

	public CSGemRowOnRingRail(string identifier)
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
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
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
		double t = InGemPosition.Double;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		if (!nurbsCurve.NormalizedLengthParameter(t, out t))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Vector3d destination = Vector3d.CrossProduct(nurbsCurve.TangentAt(t), Vector3d.YAxis);
		destination.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, nurbsCurve.PointAt(t), Vector3d.YAxis, destination);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateError("Gem Bounding Box");
		}
		double num4 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y) + num;
		double num5 = num4 * (double)(integer - 1) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < integer; i++)
		{
			Transform transform3 = Transform.Translation(0.0, num3 + (num5 - (double)i * num4), zOffset + num2);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, transform * transform3 * transform2);
			cSDataArray.Append(data);
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
