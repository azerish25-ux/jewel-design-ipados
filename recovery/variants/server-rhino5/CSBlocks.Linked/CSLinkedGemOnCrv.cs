using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedGemOnCrv", "{8AEB9A36-12DE-4aff-899E-024B7FCB1049}")]
public sealed class CSLinkedGemOnCrv : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemXOffset;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemNudge;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSInputChannel InParamStart;

	public readonly CSInputChannel InLastRightRailNormParam;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutRightRailNormParam;

	public CSLinkedGemOnCrv(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemXOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailNormParam = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		OutRightRailNormParam = new CSOutputChannel("LinkedRightRailNormParam", this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curve Input Missing");
		}
		if (flattenArray.GetArrayCount() == 0)
		{
			return UpdateError("Curve Input Invalid");
		}
		CSGemData gem = InGemDefinition.Gem;
		if (gem == null)
		{
			return UpdateMissing("Gem Definition Missing");
		}
		int integer = InGemPlacementMethod.Integer;
		double zOffset = InGemZOffset.Double;
		double dx = InGemXOffset.Double;
		double angleDegrees = InGemRotation.Double;
		double num = InGemNudge.Double;
		double num2 = InParamStart.Double;
		double num3 = InLastRightRailNormParam.Double;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Transform transform = Transform.Translation(dx, 0.0, zOffset);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		double length = value.GetLength();
		double num4 = Math.Abs(boundingBox.Min.Y);
		double num5 = Math.Abs(boundingBox.Max.Y);
		num2 += num3 + num4 / length;
		double t = 0.0;
		value.NormalizedLengthParameter(num2, out t);
		Point3d destination = value.PointAt(t);
		Vector3d destination2 = new Vector3d(Vector3d.ZAxis) * -1.0;
		Vector3d destination3 = value.TangentAt(t);
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(Vector3d.ZAxis) * -1.0, Vector3d.YAxis, destination, destination2, destination3);
		CSData data = gem.Duplicate();
		CSDataHelper.Transform(data, transform3 * transform * transform2);
		num2 += num5 * (1.0 - num / 100.0) / length;
		OutGem.SetData(data);
		OutRightRailNormParam.SetData(new CSNumberData(num2));
		return base.Update();
	}
}
