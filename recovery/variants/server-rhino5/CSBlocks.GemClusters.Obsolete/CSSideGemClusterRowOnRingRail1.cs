using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("SideGemClusterRowOnRingRail_V1", "{8A65D126-F080-4b8e-8B08-D4BFB547F4E9}")]
public sealed class CSSideGemClusterRowOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemXSpacing;

	public readonly CSInputChannel InGemYSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSOutputChannel OutGem;

	public CSSideGemClusterRowOnRingRail1(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemXSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemYSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InGem.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Gem Input");
		}
		if (iCSArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem Input");
		}
		if (!(iCSArray.GetArrayItem(iCSArray.GetArrayCount() - 1) is CSGemData cSGemData))
		{
			return UpdateError("gemLhs Missing");
		}
		if (!(iCSArray.GetArrayItem(0) is CSGemData cSGemData2))
		{
			return UpdateError("gemRhs Missing");
		}
		if (!(InGemDefinition.Data is CSGemData cSGemData3))
		{
			return UpdateMissing("Gem Definition");
		}
		int value = 1;
		InGemCount.GetInteger(ref value);
		double value2 = 0.0;
		InGemXSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		InGemYSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		InGemZOffset.GetDouble(ref value4);
		double value5 = 0.0;
		InGemYOffset.GetDouble(ref value5);
		double value6 = 0.0;
		InGemAngle.GetDouble(ref value6);
		value6 = CSBlockUtil.CSUtilMath.ToRadians(value6);
		double value7 = 0.0;
		InGemRotation.GetDouble(ref value7);
		value7 = CSBlockUtil.CSUtilMath.ToRadians(value7);
		bool value8 = false;
		InGemRotation.GetBoolean(ref value8);
		CSDataArray cSDataArray = new CSDataArray();
		Point3d point3d = new Point3d(cSGemData2.Center);
		point3d.Y = 0.0;
		Point3d point3d2 = point3d;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(point3d2, cSGemData2.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, point3d2, cSGemData2.Up, Vector3d.YAxis);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox, doGrow: false, transform))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform transform3 = (value8 ? CSBlockUtil.CSUtilMath.Rotation(0.0 - value7, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(value7, Vector3d.ZAxis, Point3d.Origin));
		Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.YAxis, Point3d.Origin);
		CSData cSData = cSGemData3.Duplicate();
		CSDataHelper.Transform(cSData, transform4 * transform3);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2))
		{
			return UpdateError("Boundbox Error");
		}
		double num = Math.Abs(boundingBox2.Max.Y - boundingBox2.Min.Y) + value3;
		double num2 = num * (double)(value - 1) / 2.0;
		for (int i = 0; i < value; i++)
		{
			Transform transform5 = Transform.Translation(0.0 - boundingBox2.Min.X + value2 + boundingBox.Max.X, value5 + (num2 - (double)i * num), value4);
			if (cSGemData3.Duplicate() is CSGemData cSGemData4)
			{
				cSGemData4.ApplyTransform(transform2 * transform5 * transform4 * transform3);
				cSDataArray.Append(cSGemData4);
			}
		}
		point3d = new Point3d(cSGemData.Center);
		point3d.Y = 0.0;
		Point3d point3d3 = point3d;
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(point3d3, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, point3d3, cSGemData.Up, Vector3d.YAxis);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform6))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform transform8 = CSBlockUtil.CSUtilMath.Rotation(value7, Vector3d.ZAxis, Point3d.Origin);
		Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(0.0 - value6, Vector3d.YAxis, Point3d.Origin);
		CSData cSData2 = cSGemData3.Duplicate();
		CSDataHelper.Transform(cSData2, transform9 * transform8);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSData2.GetTightBoundingBox(ref boundingBox4))
		{
			return UpdateError("Boundingbox Error");
		}
		double num3 = Math.Abs(boundingBox4.Max.Y - boundingBox4.Min.Y) + value3;
		double num4 = num3 * (double)(value - 1) / 2.0;
		for (int j = 0; j < value; j++)
		{
			Transform transform10 = Transform.Translation(0.0 - (boundingBox4.Max.X + value2 + (0.0 - boundingBox3.Min.X)), value5 + (num4 - (double)j * num3), value4);
			if (cSGemData3.Duplicate() is CSGemData cSGemData5)
			{
				cSGemData5.ApplyTransform(transform7 * transform10 * transform9 * transform8);
				cSDataArray.Append(cSGemData5);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
