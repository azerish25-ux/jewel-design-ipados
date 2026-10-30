using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnRingRailSide_V2", "{4FCFCDE7-1594-49f7-9E18-A5C3BE77E85B}")]
public sealed class CSGemOnRingRailSide2 : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemPairCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSOutputChannel OutGem;

	public CSGemOnRingRailSide2(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemPairCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateError("Side Gem On Ring Rail: Missing Gem");
		}
		CSGemData cSGemData = gemList[gemList.Count - 1];
		CSGemData cSGemData2 = gemList[0];
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPairCount.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemAngle.Double;
		num4 = 1.0 - num4 / 100.0;
		double angleDegrees = InGemRotation.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		Transform transform = default(Transform);
		transform = ((!boolean) ? Transform.Translation(0.0, num3, cSGemData2.Center.Z + num2) : Transform.Translation(0.0, 0.0 - num3, cSGemData2.Center.X + num2));
		Transform transform2 = default(Transform);
		transform2 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData2.Center, cSGemData2.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform3))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, cSGemData2.Center, cSGemData2.Up, Vector3d.YAxis);
		Point3d point3d = new Point3d(boundingBox2.Max.X + num / 2.0, 0.0, 0.0);
		point3d.Transform(xform);
		double num5 = Math.Atan((boundingBox.Max.X + num / 2.0) / (cSGemData2.Center.Z + num2));
		double num6 = Math.Atan((0.0 - boundingBox.Min.X + num / 2.0) / (cSGemData2.Center.Z + num2));
		double num7 = num5 + num6;
		double num8 = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d.X, point3d.Z) + num6 + num7 * (double)(integer - 1);
		for (int i = 0; i < integer; i++)
		{
			Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(num8 - (double)i * num7, Vector3d.YAxis, Point3d.Origin);
			Transform transform5 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num8 - (double)i * num7) * num4, Vector3d.YAxis, Point3d.Origin);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, transform4 * transform * transform5 * transform2);
			cSDataArray.Append(data);
		}
		Transform transform6 = Transform.Translation(0.0, num3, cSGemData.Center.Z + num2);
		Transform transform7 = default(Transform);
		transform7 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.ZAxis, Point3d.Origin));
		CSData cSData2 = gem.Duplicate();
		CSDataHelper.Transform(cSData2, transform7);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSData2.GetTightBoundingBox(ref boundingBox3))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform8 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox4, doGrow: false, transform8))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, cSGemData.Center, cSGemData.Up, Vector3d.YAxis);
		Point3d point3d2 = new Point3d(boundingBox4.Min.X - num / 2.0, 0.0, 0.0);
		point3d2.Transform(xform2);
		double num9 = Math.Atan((boundingBox3.Max.X + num / 2.0) / (cSGemData.Center.Z + num2));
		double num10 = Math.Atan((0.0 - boundingBox3.Min.X + num / 2.0) / (cSGemData.Center.Z + num2));
		double num11 = num9 + num10;
		double num12 = Math.PI * 2.0 - CSBlockUtil.CSUtilMath.AngleOfPointD(point3d2.X, point3d2.Z) + num9;
		for (int j = 0; j < integer; j++)
		{
			Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num12 - (double)j * num11, Vector3d.YAxis, Point3d.Origin);
			Transform transform10 = CSBlockUtil.CSUtilMath.Rotation((0.0 - (0.0 - num12 - (double)j * num11)) * num4, Vector3d.YAxis, Point3d.Origin);
			CSData data2 = gem.Duplicate();
			CSDataHelper.Transform(data2, transform9 * transform6 * transform10 * transform7);
			cSDataArray.Append(data2);
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
