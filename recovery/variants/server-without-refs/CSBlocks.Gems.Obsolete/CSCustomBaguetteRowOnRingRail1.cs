using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("CustomBaguetteRowOnRingRail_V1", "{1261F5E3-E274-4401-B839-7C17567C89CD}")]
public sealed class CSCustomBaguetteRowOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InRingRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemLength;

	public readonly CSInputChannel InGemWidthTop;

	public readonly CSInputChannel InGemWidthBottom;

	public readonly CSInputChannel InZOffset;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InAngleToSideGem;

	public readonly CSInputChannel InAngleToRingRail;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSCustomBaguetteRowOnRingRail1(string identifier)
		: base(identifier)
	{
		InRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemWidthTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemWidthBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngleToSideGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngleToRingRail = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRingRail.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = InGemLength.Double;
		double num2 = InGemWidthTop.Double;
		double num3 = InGemWidthBottom.Double;
		double num4 = InZOffset.Double;
		double num5 = InSpacing.Double;
		double param = InPosition.Double;
		double num6 = InAngleToSideGem.Double;
		double angleDegrees = InAngleToRingRail.Double;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		double num7 = num / 2.0;
		double num8 = num2 / 2.0;
		double num9 = num3 / 2.0;
		CSDataArray outputGems = new CSDataArray();
		if (integer % 2 != 0)
		{
			Transform x = default(Transform);
			CSUtilGeometry.OrientToCurveNalp((Curve)(object)nurbsCurve, param, ref x);
			Transform val = Transform.Translation(0.0, 0.0, zOffset + num4);
			Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			CSData cSData = gem.Duplicate();
			CSDataHelper.Transform(cSData, x * val * val2 * val3);
			outputGems.Append(cSData.Duplicate());
			if (integer > 1)
			{
				CSData cSData2 = null;
				Line val4 = default(Line);
				((Line)(ref val4))._002Ector(new Point3d(0.0 - num9, 0.0 - num7, 0.0), new Point3d(0.0 - num8, num7, 0.0));
				Line val5 = default(Line);
				((Line)(ref val5))._002Ector(new Point3d(num9, 0.0 - num7, 0.0), new Point3d(num8, num7, 0.0));
				Line currentEdgeLHS = Line.Unset;
				Line currentEdgeRHS = Line.Unset;
				double num10 = 0.0;
				cSData2 = cSData.Duplicate();
				currentEdgeLHS = val4;
				((Line)(ref currentEdgeLHS)).Transform(x * val * val2 * val3);
				currentEdgeRHS = val5;
				((Line)(ref currentEdgeRHS)).Transform(x * val * val2 * val3);
				num10 = ((double)integer - 1.0) / 2.0;
				for (int i = 0; (double)i < num10; i++)
				{
					SideGemLHS(ref cSData2, ref currentEdgeLHS, ref currentEdgeRHS, num6, num5, ref outputGems);
				}
				cSData2 = cSData.Duplicate();
				currentEdgeLHS = val4;
				((Line)(ref currentEdgeLHS)).Transform(x * val * val2 * val3);
				currentEdgeRHS = val5;
				((Line)(ref currentEdgeRHS)).Transform(x * val * val2 * val3);
				num10 = ((double)integer - 1.0) / 2.0 - ((double)integer - 1.0) % 2.0;
				for (int j = 0; (double)j < num10; j++)
				{
					SideGemRHS(ref cSData2, ref currentEdgeLHS, ref currentEdgeRHS, num6, num5, ref outputGems);
				}
			}
		}
		else
		{
			Line val6 = default(Line);
			((Line)(ref val6))._002Ector(new Point3d(0.0 - num9, 0.0 - num7, 0.0), new Point3d(0.0 - num8, num7, 0.0));
			Line val7 = default(Line);
			((Line)(ref val7))._002Ector(new Point3d(num9, 0.0 - num7, 0.0), new Point3d(num8, num7, 0.0));
			Line currentEdgeLHS2 = Line.Unset;
			Line currentEdgeRHS2 = Line.Unset;
			double num11 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(((Line)(ref val6)).Direction, ((Line)(ref val7)).Direction);
			if (num3 > num2)
			{
				num11 = 0.0 - num11;
			}
			Transform x2 = default(Transform);
			CSUtilGeometry.OrientToCurveNalp((Curve)(object)nurbsCurve, param, ref x2);
			Transform val8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			CSData cSData3 = gem.Duplicate();
			Transform val9 = CSBlockUtil.CSUtilMath.Rotation(num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			currentEdgeRHS2 = val7;
			((Line)(ref currentEdgeRHS2)).Transform(val9);
			Point3d val10 = ((Line)(ref currentEdgeRHS2)).From;
			Transform val11 = Transform.Translation(0.0, 0.0 - (((Point3d)(ref val10)).Y + num5 / 2.0), zOffset + num4);
			Transform val12 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num6 / 2.0), ((Line)(ref currentEdgeRHS2)).Direction, ((Line)(ref currentEdgeRHS2)).From);
			CSDataHelper.Transform(cSData3, x2 * val11 * val8 * val12 * val9);
			outputGems.Append(cSData3.Duplicate());
			CSData cSData4 = gem.Duplicate();
			Transform val13 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			currentEdgeLHS2 = val6;
			((Line)(ref currentEdgeLHS2)).Transform(val13);
			val10 = ((Line)(ref currentEdgeLHS2)).From;
			Transform val14 = Transform.Translation(0.0, 0.0 - (((Point3d)(ref val10)).Y - num5 / 2.0), zOffset + num4);
			Transform val15 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num6 / 2.0), ((Line)(ref currentEdgeLHS2)).Direction, ((Line)(ref currentEdgeLHS2)).From);
			CSDataHelper.Transform(cSData4, x2 * val14 * val8 * val15 * val13);
			outputGems.Append(cSData4.Duplicate());
			if (integer > 2)
			{
				CSData cSData5 = null;
				double num12 = 0.0;
				cSData5 = cSData3.Duplicate();
				currentEdgeLHS2 = val6;
				((Line)(ref currentEdgeLHS2)).Transform(x2 * val11 * val8 * val12 * val9);
				currentEdgeRHS2 = val7;
				((Line)(ref currentEdgeRHS2)).Transform(x2 * val11 * val8 * val12 * val9);
				num12 = ((double)integer - 2.0) / 2.0;
				for (int k = 0; (double)k < num12; k++)
				{
					SideGemLHS(ref cSData5, ref currentEdgeLHS2, ref currentEdgeRHS2, num6, num5, ref outputGems);
				}
				cSData5 = cSData4.Duplicate();
				currentEdgeLHS2 = val6;
				((Line)(ref currentEdgeLHS2)).Transform(x2 * val14 * val8 * val15 * val13);
				currentEdgeRHS2 = val7;
				((Line)(ref currentEdgeRHS2)).Transform(x2 * val14 * val8 * val15 * val13);
				num12 = ((double)integer - 2.0) / 2.0 - ((double)integer - 2.0) % 2.0;
				for (int l = 0; (double)l < num12; l++)
				{
					SideGemRHS(ref cSData5, ref currentEdgeLHS2, ref currentEdgeRHS2, num6, num5, ref outputGems);
				}
			}
		}
		OutGem.SetData(outputGems);
		return base.Update();
	}

	private static void SideGemLHS(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = Vector3d.CrossProduct(((Line)(ref currentEdgeLHS)).Direction, ((Line)(ref currentEdgeRHS)).From - ((Line)(ref currentEdgeLHS)).From);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = Vector3d.CrossProduct(((Line)(ref currentEdgeLHS)).Direction, val);
		((Vector3d)(ref val2)).Unitize();
		Line val3 = currentEdgeLHS;
		((Line)(ref val3)).Transform(Transform.Translation(val2 * spacing));
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(((Line)(ref currentEdgeRHS)).From, ((Line)(ref currentEdgeRHS)).Direction, val, ((Line)(ref val3)).From, ((Line)(ref val3)).Direction, val);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), ((Line)(ref currentEdgeLHS)).Direction, ((Line)(ref currentEdgeLHS)).From);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), ((Line)(ref val3)).Direction, ((Line)(ref val3)).From);
		CSDataHelper.Transform(gemSide, val5 * val6 * val4);
		outputGems.Append(gemSide.Duplicate());
		((Line)(ref currentEdgeLHS)).Transform(val5 * val6 * val4);
		currentEdgeRHS = val3;
	}

	private static void SideGemRHS(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = Vector3d.CrossProduct(((Line)(ref currentEdgeRHS)).Direction, ((Line)(ref currentEdgeLHS)).From - ((Line)(ref currentEdgeRHS)).From);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = Vector3d.CrossProduct(((Line)(ref currentEdgeRHS)).Direction, val);
		((Vector3d)(ref val2)).Unitize();
		Line val3 = currentEdgeRHS;
		((Line)(ref val3)).Transform(Transform.Translation(val2 * spacing));
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(((Line)(ref currentEdgeLHS)).From, ((Line)(ref currentEdgeLHS)).Direction, val, ((Line)(ref val3)).From, ((Line)(ref val3)).Direction, val);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), ((Line)(ref currentEdgeRHS)).Direction, ((Line)(ref currentEdgeRHS)).From);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), ((Line)(ref val3)).Direction, ((Line)(ref val3)).From);
		CSDataHelper.Transform(gemSide, val5 * val6 * val4);
		outputGems.Append(gemSide.Duplicate());
		((Line)(ref currentEdgeRHS)).Transform(val5 * val6 * val4);
		currentEdgeLHS = val3;
	}
}
