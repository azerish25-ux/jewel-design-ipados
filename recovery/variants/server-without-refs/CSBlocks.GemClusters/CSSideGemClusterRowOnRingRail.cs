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
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gem Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemXSpacing.Double;
		double num2 = InGemYSpacing.Double;
		double num3 = InGemZOffset.Double;
		double num4 = InGemYOffset.Double;
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
		double num5 = 0.0;
		int num6 = 0;
		Point3d val = cSGemData.Center;
		double x = ((Point3d)(ref val)).X;
		val = cSGemData.Center;
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(x, 0.0, ((Point3d)(ref val)).Z);
		Point3d val3 = default(Point3d);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData2 = gemList[i];
			if (cSGemData2 != null)
			{
				val = cSGemData2.Center;
				double x2 = ((Point3d)(ref val)).X;
				val = cSGemData2.Center;
				((Point3d)(ref val3))._002Ector(x2, 0.0, ((Point3d)(ref val)).Z);
				if (!(((Point3d)(ref val2)).DistanceTo(val3) > 0.001))
				{
					double num7 = num5;
					val = cSGemData2.Center;
					num5 = num7 + ((Point3d)(ref val)).Y;
					num6++;
				}
			}
		}
		double y = num5 / (double)num6;
		((Point3d)(ref val))._002Ector(cSGemData.Center);
		((Point3d)(ref val)).Y = y;
		Point3d val4 = val;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(val4, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val4, cSGemData.Up, Vector3d.YAxis);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox, doGrow: false, transform))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform val6 = (value ? CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform val7 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.YAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, val7 * val6);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2, doGrow: true, Transform.Identity))
		{
			return UpdateError("Boundingbox Error");
		}
		val = ((BoundingBox)(ref boundingBox2)).Max;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox2)).Min;
		double num8 = Math.Abs(y2 - ((Point3d)(ref val)).Y) + num2;
		double num9 = num8 * (double)(integer - 1) / 2.0;
		for (int j = 0; j < integer; j++)
		{
			val = ((BoundingBox)(ref boundingBox2)).Min;
			double num10 = 0.0 - ((Point3d)(ref val)).X + num;
			val = ((BoundingBox)(ref boundingBox)).Max;
			Transform val8 = Transform.Translation(num10 + ((Point3d)(ref val)).X, num4 + (num9 - (double)j * num8), num3);
			CSData cSData2 = gem.Duplicate();
			if (cSData2 != null)
			{
				CSDataHelper.Transform(cSData2, val5 * val8 * val7 * val6);
				cSDataArray.Append(cSData2);
			}
		}
		CSGemData cSGemData3 = gemList[gemList.Count - 1];
		if (cSGemData3 == null)
		{
			return UpdateError("gemLhs Error");
		}
		double num11 = 0.0;
		int num12 = 0;
		val = cSGemData3.Center;
		double x3 = ((Point3d)(ref val)).X;
		val = cSGemData3.Center;
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(x3, 0.0, ((Point3d)(ref val)).Z);
		Point3d val10 = default(Point3d);
		for (int k = 0; k < gemList.Count; k++)
		{
			CSGemData cSGemData4 = gemList[k];
			if (cSGemData4 != null)
			{
				val = cSGemData4.Center;
				double x4 = ((Point3d)(ref val)).X;
				val = cSGemData4.Center;
				((Point3d)(ref val10))._002Ector(x4, 0.0, ((Point3d)(ref val)).Z);
				if (!(((Point3d)(ref val9)).DistanceTo(val10) > 0.001))
				{
					double num13 = num11;
					val = cSGemData4.Center;
					num11 = num13 + ((Point3d)(ref val)).Y;
					num12++;
				}
			}
		}
		double y3 = num11 / (double)num12;
		((Point3d)(ref val))._002Ector(cSGemData3.Center);
		((Point3d)(ref val)).Y = y3;
		Point3d val11 = val;
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(val11, cSGemData3.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform val12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, val11, cSGemData3.Up, Vector3d.YAxis);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSGemData3.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform2))
		{
			return UpdateError("Boundingbox Error");
		}
		Transform val13 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform val14 = CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.YAxis, Point3d.Origin);
		CSData cSData3 = gem.Duplicate();
		CSDataHelper.Transform(cSData3, val14 * val13);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSData3.GetTightBoundingBox(ref boundingBox4, doGrow: true, Transform.Identity))
		{
			return UpdateError("Boundingbox Error");
		}
		val = ((BoundingBox)(ref boundingBox4)).Max;
		double y4 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox4)).Min;
		double num14 = Math.Abs(y4 - ((Point3d)(ref val)).Y) + num2;
		double num15 = num14 * (double)(integer - 1) / 2.0;
		for (int l = 0; l < integer; l++)
		{
			val = ((BoundingBox)(ref boundingBox4)).Max;
			double num16 = ((Point3d)(ref val)).X + num;
			val = ((BoundingBox)(ref boundingBox3)).Min;
			Transform val15 = Transform.Translation(0.0 - (num16 + (0.0 - ((Point3d)(ref val)).X)), num4 + (num15 - (double)l * num14), num3);
			if (gem.Duplicate() is CSGemData cSGemData5)
			{
				cSGemData5.ApplyTransform(val12 * val15 * val14 * val13);
				cSDataArray.Append(cSGemData5);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
