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
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
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
		if (!CSUtilGeometry.OrientToCurveNalp((Curve)(object)nurbsCurve, param, ref x))
		{
			return UpdateError("OrientToCurveNalp failed");
		}
		Transform val = default(Transform);
		val = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0, num2));
		Transform val2 = default(Transform);
		val2 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val2))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val3 = default(Transform);
		val3 = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0 - num3, num2));
		Transform val4 = default(Transform);
		val4 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.ZAxis, Point3d.Origin));
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, val4))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val5 = Transform.Translation(0.0, num3, num2);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, val6))
		{
			return UpdateError("Bounding Box Failed");
		}
		int num5 = integer % 2;
		int num6 = integer / 2;
		Point3d val7;
		BoundingBox boundingBox4;
		if (num5 > 0)
		{
			val7 = ((BoundingBox)(ref boundingBox)).Max;
			double num7 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num8 = Math.Atan(num7 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox2)).Min;
			double num9 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num10 = Math.Atan(num9 / (((Point3d)(ref val7)).Z + num2));
			double num11 = num8 + num10;
			val7 = ((BoundingBox)(ref boundingBox2)).Max;
			double num12 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num13 = Math.Atan(num12 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox2)).Min;
			double num14 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num15 = Math.Atan(num14 / (((Point3d)(ref val7)).Z + num2));
			double num16 = num13 + num15;
			val7 = ((BoundingBox)(ref boundingBox)).Min;
			double num17 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num18 = Math.Atan(num17 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox3)).Max;
			double num19 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num20 = Math.Atan(num19 / (((Point3d)(ref val7)).Z + num2));
			double num21 = num18 + num20;
			val7 = ((BoundingBox)(ref boundingBox3)).Max;
			double num22 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num23 = Math.Atan(num22 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox3)).Min;
			double num24 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num25 = Math.Atan(num24 / (((Point3d)(ref val7)).Z + num2));
			double num26 = num23 + num25;
			double num27 = 0.0;
			if (num6 > 0)
			{
				num27 += (num11 + num21 + num16 * (double)(num6 - 1) + num26 * (double)(num6 - 1)) / 2.0;
			}
			double num28 = num27;
			for (int i = 0; i < integer; i++)
			{
				Transform val8 = CSBlockUtil.CSUtilMath.Rotation(num28, Vector3d.YAxis, Point3d.Origin);
				Transform val9 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num28) * num4, Vector3d.YAxis, Point3d.Origin);
				CSData data = gem.Duplicate();
				if (i < num6 - 1)
				{
					CSDataHelper.Transform(data, val8 * x * val3 * val9 * val4);
					num28 -= num16;
				}
				else if (i == num6 - 1)
				{
					CSDataHelper.Transform(data, val8 * x * val3 * val9 * val4);
					num28 -= num11;
				}
				else if (i >= num6 + 1)
				{
					CSDataHelper.Transform(data, val8 * x * val5 * val9 * val6);
					num28 -= num26;
				}
				else
				{
					CSDataHelper.Transform(data, val8 * x * val * val9 * val2);
					num28 -= num21;
				}
				cSDataArray.Append(data);
			}
		}
		else
		{
			val7 = ((BoundingBox)(ref boundingBox2)).Min;
			double num29 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num30 = Math.Atan(num29 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox3)).Max;
			double num31 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num32 = Math.Atan(num31 / (((Point3d)(ref val7)).Z + num2));
			double num33 = num30 + num32;
			val7 = ((BoundingBox)(ref boundingBox2)).Max;
			double num34 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num35 = Math.Atan(num34 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox2)).Min;
			double num36 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num37 = Math.Atan(num36 / (((Point3d)(ref val7)).Z + num2));
			double num38 = num35 + num37;
			val7 = ((BoundingBox)(ref boundingBox3)).Max;
			double num39 = ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num40 = Math.Atan(num39 / (((Point3d)(ref val7)).Z + num2));
			val7 = ((BoundingBox)(ref boundingBox3)).Min;
			double num41 = 0.0 - ((Point3d)(ref val7)).X + num / 2.0;
			boundingBox4 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val7 = ((BoundingBox)(ref boundingBox4)).Max;
			double num42 = Math.Atan(num41 / (((Point3d)(ref val7)).Z + num2));
			double num43 = num40 + num42;
			double num44 = (num33 + num38 * (double)(num6 - 1) + num43 * (double)(num6 - 1)) / 2.0;
			for (int j = 0; j < integer; j++)
			{
				Transform val10 = CSBlockUtil.CSUtilMath.Rotation(num44, Vector3d.YAxis, Point3d.Origin);
				Transform val11 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num44) * num4, Vector3d.YAxis, Point3d.Origin);
				CSData data2 = gem.Duplicate();
				if (j < num6 - 1)
				{
					CSDataHelper.Transform(data2, val10 * x * val3 * val11 * val4);
					num44 -= num38;
				}
				else if (j >= num6)
				{
					CSDataHelper.Transform(data2, val10 * x * val5 * val11 * val6);
					num44 -= num43;
				}
				else
				{
					CSDataHelper.Transform(data2, val10 * x * val3 * val11 * val4);
					num44 -= num33;
				}
				cSDataArray.Append(data2);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
