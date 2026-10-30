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
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
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
		double num = InGemXOffset.Double;
		double angleDegrees = InGemRotation.Double;
		double num2 = InGemNudge.Double;
		double num3 = InParamStart.Double;
		double num4 = InLastRightRailNormParam.Double;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Transform val = Transform.Translation(num, 0.0, zOffset);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, val2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		double length = ((Curve)value).GetLength();
		Point3d val3 = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = Math.Abs(((Point3d)(ref val3)).Y);
		val3 = ((BoundingBox)(ref boundingBox)).Max;
		double num6 = Math.Abs(((Point3d)(ref val3)).Y);
		num3 += num4 + num5 / length;
		double num7 = 0.0;
		((Curve)value).NormalizedLengthParameter(num3, ref num7);
		Point3d destination = ((Curve)value).PointAt(num7);
		Vector3d destination2 = new Vector3d(Vector3d.ZAxis) * -1.0;
		Vector3d destination3 = ((Curve)value).TangentAt(num7);
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(Vector3d.ZAxis) * -1.0, Vector3d.YAxis, destination, destination2, destination3);
		CSData data = gem.Duplicate();
		CSDataHelper.Transform(data, val4 * val * val2);
		num3 += num6 * (1.0 - num2 / 100.0) / length;
		OutGem.SetData(data);
		OutRightRailNormParam.SetData(new CSNumberData(num3));
		return base.Update();
	}
}
