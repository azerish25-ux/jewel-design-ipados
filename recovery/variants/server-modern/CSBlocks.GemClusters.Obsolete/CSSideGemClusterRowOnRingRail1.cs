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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(cSGemData2.Center);
		((Point3d)(ref val)).Y = 0.0;
		Point3d val2 = val;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(val2, cSGemData2.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform val3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val2, cSGemData2.Up, Vector3d.YAxis);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox, doGrow: false, transform))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform val4 = (value8 ? CSBlockUtil.CSUtilMath.Rotation(0.0 - value7, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(value7, Vector3d.ZAxis, Point3d.Origin));
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.YAxis, Point3d.Origin);
		CSData cSData = cSGemData3.Duplicate();
		CSDataHelper.Transform(cSData, val5 * val4);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2))
		{
			return UpdateError("Boundbox Error");
		}
		val = ((BoundingBox)(ref boundingBox2)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		double num = Math.Abs(y - ((Point3d)(ref val)).Y) + value3;
		double num2 = num * (double)(value - 1) / 2.0;
		for (int i = 0; i < value; i++)
		{
			val = ((BoundingBox)(ref boundingBox2)).Min;
			double num3 = 0.0 - ((Point3d)(ref val)).X + value2;
			val = ((BoundingBox)(ref boundingBox)).Max;
			Transform val6 = Transform.Translation(num3 + ((Point3d)(ref val)).X, value5 + (num2 - (double)i * num), value4);
			if (cSGemData3.Duplicate() is CSGemData cSGemData4)
			{
				cSGemData4.ApplyTransform(val3 * val6 * val5 * val4);
				cSDataArray.Append(cSGemData4);
			}
		}
		((Point3d)(ref val))._002Ector(cSGemData.Center);
		((Point3d)(ref val)).Y = 0.0;
		Point3d val7 = val;
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(val7, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform val8 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val7, cSGemData.Up, Vector3d.YAxis);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform2))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform val9 = CSBlockUtil.CSUtilMath.Rotation(value7, Vector3d.ZAxis, Point3d.Origin);
		Transform val10 = CSBlockUtil.CSUtilMath.Rotation(0.0 - value6, Vector3d.YAxis, Point3d.Origin);
		CSData cSData2 = cSGemData3.Duplicate();
		CSDataHelper.Transform(cSData2, val10 * val9);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSData2.GetTightBoundingBox(ref boundingBox4))
		{
			return UpdateError("Boundingbox Error");
		}
		val = ((BoundingBox)(ref boundingBox4)).Max;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox4)).Min;
		double num4 = Math.Abs(y2 - ((Point3d)(ref val)).Y) + value3;
		double num5 = num4 * (double)(value - 1) / 2.0;
		for (int j = 0; j < value; j++)
		{
			val = ((BoundingBox)(ref boundingBox4)).Max;
			double num6 = ((Point3d)(ref val)).X + value2;
			val = ((BoundingBox)(ref boundingBox3)).Min;
			Transform val11 = Transform.Translation(0.0 - (num6 + (0.0 - ((Point3d)(ref val)).X)), value5 + (num5 - (double)j * num4), value4);
			if (cSGemData3.Duplicate() is CSGemData cSGemData5)
			{
				cSGemData5.ApplyTransform(val8 * val11 * val10 * val9);
				cSDataArray.Append(cSGemData5);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
