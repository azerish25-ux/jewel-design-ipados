using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters;

[CSBlock("SideGemClusterRowOnRingRail", "{3D39DFE0-8191-40f9-A7A4-1A87715D05B3}")]
public sealed class CSSideGemClusterRowOnRingRail : CSBlock
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

	public CSSideGemClusterRowOnRingRail(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 1);
		InGemXSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gem Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemXSpacing.Double;
		double num2 = InGemYSpacing.Double;
		double dz = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		bool value = false;
		InGemRotation.GetBoolean(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		CSGemData cSGemData = gemList[0];
		if (cSGemData == null)
		{
			return UpdateError("gem RHS");
		}
		double num4 = 0.0;
		int num5 = 0;
		Point3d point3d = new Point3d(cSGemData.Center.X, 0.0, cSGemData.Center.Z);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData2 = gemList[i];
			if (cSGemData2 != null)
			{
				Point3d other = new Point3d(cSGemData2.Center.X, 0.0, cSGemData2.Center.Z);
				if (!(point3d.DistanceTo(other) > 0.001))
				{
					num4 += cSGemData2.Center.Y;
					num5++;
				}
			}
		}
		double y = num4 / (double)num5;
		Point3d point3d2 = new Point3d(cSGemData.Center);
		point3d2.Y = y;
		Point3d point3d3 = point3d2;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(point3d3, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, point3d3, cSGemData.Up, Vector3d.YAxis);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox, doGrow: false, transform))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform transform3 = (value ? CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.YAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform4 * transform3);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2, doGrow: true, Transform.Identity))
		{
			return UpdateError("Boundingbox Error");
		}
		double num6 = Math.Abs(boundingBox2.Max.Y - boundingBox2.Min.Y) + num2;
		double num7 = num6 * (double)(integer - 1) / 2.0;
		for (int j = 0; j < integer; j++)
		{
			Transform transform5 = Transform.Translation(0.0 - boundingBox2.Min.X + num + boundingBox.Max.X, num3 + (num7 - (double)j * num6), dz);
			CSData cSData2 = gem.Duplicate();
			if (cSData2 != null)
			{
				CSDataHelper.Transform(cSData2, transform2 * transform5 * transform4 * transform3);
				cSDataArray.Append(cSData2);
			}
		}
		CSGemData cSGemData3 = gemList[gemList.Count - 1];
		if (cSGemData3 == null)
		{
			return UpdateError("gemLhs Error");
		}
		double num8 = 0.0;
		int num9 = 0;
		Point3d point3d4 = new Point3d(cSGemData3.Center.X, 0.0, cSGemData3.Center.Z);
		for (int k = 0; k < gemList.Count; k++)
		{
			CSGemData cSGemData4 = gemList[k];
			if (cSGemData4 != null)
			{
				Point3d other2 = new Point3d(cSGemData4.Center.X, 0.0, cSGemData4.Center.Z);
				if (!(point3d4.DistanceTo(other2) > 0.001))
				{
					num8 += cSGemData4.Center.Y;
					num9++;
				}
			}
		}
		double y2 = num8 / (double)num9;
		point3d2 = new Point3d(cSGemData3.Center);
		point3d2.Y = y2;
		Point3d point3d5 = point3d2;
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(point3d5, cSGemData3.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, point3d5, cSGemData3.Up, Vector3d.YAxis);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSGemData3.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform6))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform transform8 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.YAxis, Point3d.Origin);
		CSData cSData3 = gem.Duplicate();
		CSDataHelper.Transform(cSData3, transform9 * transform8);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSData3.GetTightBoundingBox(ref boundingBox4, doGrow: true, Transform.Identity))
		{
			return UpdateError("Boundingbox Error");
		}
		double num10 = Math.Abs(boundingBox4.Max.Y - boundingBox4.Min.Y) + num2;
		double num11 = num10 * (double)(integer - 1) / 2.0;
		for (int l = 0; l < integer; l++)
		{
			Transform transform10 = Transform.Translation(0.0 - (boundingBox4.Max.X + num + (0.0 - boundingBox3.Min.X)), num3 + (num11 - (double)l * num10), dz);
			if (gem.Duplicate() is CSGemData cSGemData5)
			{
				cSGemData5.ApplyTransform(transform7 * transform10 * transform9 * transform8);
				cSDataArray.Append(cSGemData5);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
