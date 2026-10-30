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
[CSBlock("GemOnRingRailSide_V1", "{DDEE5482-BAA1-4e8e-A17E-E45D8A8B7995}")]
public sealed class CSGemOnRingRailSide1 : CSBlock
{
	public readonly CSInputChannel InRingRail;

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

	public CSGemOnRingRailSide1(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
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
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRingRail.NurbsCurve;
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
		Transform val = default(Transform);
		BoundingBox boundingBox;
		Point3d val2;
		if (boolean)
		{
			double num5 = 0.0 - num3;
			boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			val = Transform.Translation(0.0, num5, ((Point3d)(ref val2)).Z + num2);
		}
		else
		{
			boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			val = Transform.Translation(0.0, num3, ((Point3d)(ref val2)).Z + num2);
		}
		Transform val3 = default(Transform);
		val3 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, val3);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData2.Center, cSGemData2.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, cSGemData2.Center, cSGemData2.Up, Vector3d.YAxis);
		val2 = ((BoundingBox)(ref boundingBox3)).Max;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(((Point3d)(ref val2)).X + num / 2.0, 0.0, 0.0);
		((Point3d)(ref val5)).Transform(val4);
		val2 = ((BoundingBox)(ref boundingBox2)).Max;
		double num6 = ((Point3d)(ref val2)).X + num / 2.0;
		boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double num7 = Math.Atan(num6 / (((Point3d)(ref val2)).Z + num2));
		val2 = ((BoundingBox)(ref boundingBox2)).Min;
		double num8 = 0.0 - ((Point3d)(ref val2)).X + num / 2.0;
		boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double num9 = Math.Atan(num8 / (((Point3d)(ref val2)).Z + num2));
		double num10 = num7 + num9;
		double num11 = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val5)).X, ((Point3d)(ref val5)).Z) + num9 + num10 * (double)(integer - 1);
		for (int i = 0; i < integer; i++)
		{
			Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num11 - (double)i * num10, Vector3d.YAxis, Point3d.Origin);
			Transform val7 = CSBlockUtil.CSUtilMath.Rotation((0.0 - num11 - (double)i * num10) * num4, Vector3d.YAxis, Point3d.Origin);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, val6 * val * val7 * val3);
			cSDataArray.Append(data);
		}
		boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		Transform val8 = Transform.Translation(0.0, num3, ((Point3d)(ref val2)).Z + num2);
		Transform val9 = default(Transform);
		val9 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees, Vector3d.ZAxis, Point3d.Origin));
		CSData cSData2 = gem.Duplicate();
		CSDataHelper.Transform(cSData2, val9);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSData2.GetTightBoundingBox(ref boundingBox4))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		BoundingBox boundingBox5 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox5, doGrow: false, transform2))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform val10 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, cSGemData.Center, cSGemData.Up, Vector3d.YAxis);
		val2 = ((BoundingBox)(ref boundingBox5)).Min;
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(((Point3d)(ref val2)).X - num / 2.0, 0.0, 0.0);
		((Point3d)(ref val11)).Transform(val10);
		val2 = ((BoundingBox)(ref boundingBox4)).Max;
		double num12 = ((Point3d)(ref val2)).X + num / 2.0;
		boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double num13 = Math.Atan(num12 / (((Point3d)(ref val2)).Z + num2));
		val2 = ((BoundingBox)(ref boundingBox4)).Min;
		double num14 = 0.0 - ((Point3d)(ref val2)).X + num / 2.0;
		boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double num15 = Math.Atan(num14 / (((Point3d)(ref val2)).Z + num2));
		double num16 = num13 + num15;
		double num17 = Math.PI * 2.0 - CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val11)).X, ((Point3d)(ref val11)).Z) + num13;
		for (int j = 0; j < integer; j++)
		{
			Transform val12 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num17 - (double)j * num16, Vector3d.YAxis, Point3d.Origin);
			Transform val13 = CSBlockUtil.CSUtilMath.Rotation((0.0 - (0.0 - num17 - (double)j * num16)) * num4, Vector3d.YAxis, Point3d.Origin);
			CSData data2 = gem.Duplicate();
			CSDataHelper.Transform(data2, val12 * val8 * val13 * val9);
			cSDataArray.Append(data2);
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
