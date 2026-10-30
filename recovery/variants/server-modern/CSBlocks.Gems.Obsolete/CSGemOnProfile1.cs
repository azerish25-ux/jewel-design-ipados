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
[CSBlock("GemOnProfile_V1", "{EDDBC082-E81F-4bf6-A971-46C6E75E5770}")]
public sealed class CSGemOnProfile1 : CSBlock
{
	public readonly CSInputChannel InCurve;

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

	public CSGemOnProfile1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
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
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		List<Curve> curveList = InCurve.CurveList;
		if (curveList == null)
		{
			return UpdateMissing("Curve Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double num4 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		Transform val = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.XAxis, Point3d.Origin);
		Transform val2 = default(Transform);
		val2 = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0, num2));
		Transform val3 = default(Transform);
		val3 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		Transform val4 = val * val3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val4))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val5 = default(Transform);
		val5 = ((!boolean) ? Transform.Translation(0.0, num3, num2) : Transform.Translation(0.0, 0.0 - num3, num2));
		Transform val6 = default(Transform);
		val6 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform val7 = val * val6;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, val7))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val8 = Transform.Translation(0.0, num3, num2);
		Transform val9 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform val10 = val * val9;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, val10))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num8 = default(double);
		for (int i = 0; i < curveList.Count; i++)
		{
			NurbsCurve val11 = curveList[i].ToNurbsCurve();
			Interval domain = ((Curve)val11).Domain;
			Point3d val12 = ((Curve)val11).PointAt(((Interval)(ref domain)).Min);
			domain = ((Curve)val11).Domain;
			Point3d val13 = ((Curve)val11).PointAt(((Interval)(ref domain)).Mid);
			domain = ((Curve)val11).Domain;
			Vector3d val14 = Vector3d.CrossProduct(((Curve)val11).TangentAt(((Interval)(ref domain)).Min), val13 - val12);
			double length = ((Curve)val11).GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return UpdateError("Curve Length Failed");
			}
			double num5 = num4;
			Point3d val15 = ((BoundingBox)(ref boundingBox)).Max;
			double num6 = Math.Abs(((Point3d)(ref val15)).X);
			val15 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num5 - (num6 - Math.Abs(((Point3d)(ref val15)).X)) / (length * 2.0);
			if (integer > 0 && integer % 2 != 0)
			{
				double num7 = num4;
				if (num7 > 1.0)
				{
					num7 -= 1.0;
				}
				if (!((Curve)val11).NormalizedLengthParameter(num7, ref num8))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination = Vector3d.CrossProduct(((Curve)val11).TangentAt(num8), val14);
				Transform val16 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, ((Curve)val11).PointAt(num8), val14, destination);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, val16 * val2 * val4);
				cSDataArray.Append(data);
			}
			int num9 = integer / 2;
			for (int j = 0; j < num9; j++)
			{
				double num10 = 0.0;
				if (integer % 2 != 0)
				{
					if (boolean2)
					{
						val15 = ((BoundingBox)(ref boundingBox)).Max;
						double num11 = Math.Abs(((Point3d)(ref val15)).X);
						val15 = ((BoundingBox)(ref boundingBox2)).Min;
						num10 = num11 + Math.Abs(((Point3d)(ref val15)).X) + num;
					}
					else
					{
						val15 = ((BoundingBox)(ref boundingBox)).Max;
						double num12 = Math.Abs(((Point3d)(ref val15)).X);
						val15 = ((BoundingBox)(ref boundingBox2)).Min;
						num10 = num12 + Math.Abs(((Point3d)(ref val15)).X) + num;
					}
				}
				else if (boolean2)
				{
					val15 = ((BoundingBox)(ref boundingBox2)).Min;
					num10 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
				}
				else
				{
					val15 = ((BoundingBox)(ref boundingBox2)).Max;
					num10 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
				}
				double num13 = num4;
				double num14 = num10;
				double num15 = j;
				val15 = ((BoundingBox)(ref boundingBox2)).Max;
				double x = ((Point3d)(ref val15)).X;
				val15 = ((BoundingBox)(ref boundingBox2)).Min;
				double num16 = num13 + (num14 + num15 * (x - ((Point3d)(ref val15)).X + num)) / length;
				double num17 = 0.0;
				if (num16 > 1.0)
				{
					num16 -= 1.0;
				}
				if (!((Curve)val11).NormalizedLengthParameter(num16, ref num17))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination2 = Vector3d.CrossProduct(((Curve)val11).TangentAt(num17), val14);
				Transform val17 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, ((Curve)val11).PointAt(num17), val14, destination2);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, val17 * val5 * val7);
				cSDataArray.Append(data2);
			}
			int num18 = integer / 2;
			for (int k = 0; k < num18; k++)
			{
				double num19 = 0.0;
				if (integer % 2 != 0)
				{
					if (boolean2)
					{
						val15 = ((BoundingBox)(ref boundingBox)).Min;
						double num20 = Math.Abs(((Point3d)(ref val15)).X);
						val15 = ((BoundingBox)(ref boundingBox3)).Max;
						num19 = num20 + Math.Abs(((Point3d)(ref val15)).X) + num;
					}
					else
					{
						val15 = ((BoundingBox)(ref boundingBox)).Min;
						double num21 = Math.Abs(((Point3d)(ref val15)).X);
						val15 = ((BoundingBox)(ref boundingBox3)).Max;
						num19 = num21 + Math.Abs(((Point3d)(ref val15)).X) + num;
					}
				}
				else if (boolean2)
				{
					val15 = ((BoundingBox)(ref boundingBox3)).Max;
					num19 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
				}
				else
				{
					val15 = ((BoundingBox)(ref boundingBox3)).Min;
					num19 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
				}
				double num22 = num4;
				double num23 = num19;
				double num24 = k;
				val15 = ((BoundingBox)(ref boundingBox3)).Max;
				double x2 = ((Point3d)(ref val15)).X;
				val15 = ((BoundingBox)(ref boundingBox3)).Min;
				double num25 = num22 + (0.0 - (num23 + num24 * (x2 - ((Point3d)(ref val15)).X + num)) / length);
				double num26 = 0.0;
				if (num25 < 0.0)
				{
					num25 += 1.0;
				}
				if (!((Curve)val11).NormalizedLengthParameter(num25, ref num26))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination3 = Vector3d.CrossProduct(((Curve)val11).TangentAt(num26), val14);
				Transform val18 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, ((Curve)val11).PointAt(num26), val14, destination3);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, val18 * val8 * val10);
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
