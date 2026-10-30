using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("CustomBaguetteRowOnRingRail", "{F127C1EF-1DDB-4550-A8E6-589791ABC510}")]
public sealed class CSCustomBaguetteRowOnRingRail : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iGemCount;

	public readonly CSInputChannel iGemLength;

	public readonly CSInputChannel iGemWidthTop;

	public readonly CSInputChannel iGemWidthBottom;

	public readonly CSInputChannel iZOffset;

	public readonly CSInputChannel iSpacing;

	public readonly CSInputChannel iPosition;

	public readonly CSInputChannel iAngleToSideGem;

	public readonly CSInputChannel iAngleToRingRail;

	public readonly CSInputChannel iGemPlacementMethod;

	public readonly CSOutputChannel oGem;

	public CSCustomBaguetteRowOnRingRail(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 1);
		iGemLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iGemWidthTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iGemWidthBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iAngleToSideGem = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iAngleToRingRail = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = iRingRail.NurbsCurve;
		CSGemData gem = iGemDefinition.Gem;
		int integer = iGemCount.Integer;
		int integer2 = iGemPlacementMethod.Integer;
		double num = iGemLength.Double;
		double num2 = iGemWidthTop.Double;
		double num3 = iGemWidthBottom.Double;
		double num4 = iZOffset.Double;
		double num5 = iSpacing.Double;
		double num6 = iPosition.Double;
		double num7 = iAngleToSideGem.Double;
		double angleDegrees = iAngleToRingRail.Double;
		CSDataArray outputGems = new CSDataArray();
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		double num8 = num / 2.0;
		double num9 = num2 / 2.0;
		double num10 = num3 / 2.0;
		if (!((Curve)nurbsCurve).NormalizedLengthParameter(num6, ref num6))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Vector3d destination = Vector3d.CrossProduct(((Curve)nurbsCurve).TangentAt(num6), Vector3d.YAxis);
		((Vector3d)(ref destination)).Unitize();
		Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, ((Curve)nurbsCurve).PointAt(num6), Vector3d.YAxis, destination);
		if (integer % 2 != 0)
		{
			Transform val2 = Transform.Translation(0.0, 0.0, zOffset + num4);
			Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			CSData cSData = gem.Duplicate();
			CSDataHelper.Transform(cSData, val * val2 * val3 * val4);
			outputGems.Append(cSData.Duplicate());
			if (integer > 1)
			{
				CSData cSData2 = null;
				Line val5 = default(Line);
				((Line)(ref val5))._002Ector(new Point3d(0.0 - num10, 0.0 - num8, 0.0), new Point3d(0.0 - num9, num8, 0.0));
				Line val6 = default(Line);
				((Line)(ref val6))._002Ector(new Point3d(num10, 0.0 - num8, 0.0), new Point3d(num9, num8, 0.0));
				Line currentEdgeLHS = Line.Unset;
				Line currentEdgeRHS = Line.Unset;
				double num11 = 0.0;
				cSData2 = cSData.Duplicate();
				currentEdgeLHS = val5;
				((Line)(ref currentEdgeLHS)).Transform(val * val2 * val3 * val4);
				currentEdgeRHS = val6;
				((Line)(ref currentEdgeRHS)).Transform(val * val2 * val3 * val4);
				num11 = ((double)integer - 1.0) / 2.0;
				for (int i = 0; (double)i < num11; i++)
				{
					SideGemLhs(ref cSData2, ref currentEdgeLHS, ref currentEdgeRHS, num7, num5, ref outputGems);
				}
				cSData2 = cSData.Duplicate();
				currentEdgeLHS = val5;
				((Line)(ref currentEdgeLHS)).Transform(val * val2 * val3 * val4);
				currentEdgeRHS = val6;
				((Line)(ref currentEdgeRHS)).Transform(val * val2 * val3 * val4);
				num11 = ((double)integer - 1.0) / 2.0 - ((double)integer - 1.0) % 2.0;
				for (int j = 0; (double)j < num11; j++)
				{
					SideGemRhs(ref cSData2, ref currentEdgeLHS, ref currentEdgeRHS, num7, num5, ref outputGems);
				}
			}
		}
		else
		{
			Line val7 = default(Line);
			((Line)(ref val7))._002Ector(new Point3d(0.0 - num10, 0.0 - num8, 0.0), new Point3d(0.0 - num9, num8, 0.0));
			Line val8 = default(Line);
			((Line)(ref val8))._002Ector(new Point3d(num10, 0.0 - num8, 0.0), new Point3d(num9, num8, 0.0));
			double num12 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(((Line)(ref val7)).Direction, ((Line)(ref val8)).Direction);
			if (num3 > num2)
			{
				num12 = 0.0 - num12;
			}
			Transform val9 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			CSData cSData3 = gem.Duplicate();
			Transform val10 = CSBlockUtil.CSUtilMath.Rotation(num12 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			Line currentEdgeRHS2 = val8;
			((Line)(ref currentEdgeRHS2)).Transform(val10);
			Transform val11 = Transform.Translation(0.0, 0.0 - (((Line)(ref currentEdgeRHS2)).FromY + num5 / 2.0), zOffset + num4);
			Transform val12 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num7 / 2.0), ((Line)(ref currentEdgeRHS2)).Direction, ((Line)(ref currentEdgeRHS2)).From);
			CSDataHelper.Transform(cSData3, val * val11 * val9 * val12 * val10);
			outputGems.Append(cSData3.Duplicate());
			CSData cSData4 = gem.Duplicate();
			Transform val13 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num12 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			Line currentEdgeLHS2 = val7;
			((Line)(ref currentEdgeLHS2)).Transform(val13);
			Transform val14 = Transform.Translation(0.0, 0.0 - (((Line)(ref currentEdgeLHS2)).FromY - num5 / 2.0), zOffset + num4);
			Transform val15 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num7 / 2.0), ((Line)(ref currentEdgeLHS2)).Direction, ((Line)(ref currentEdgeLHS2)).From);
			CSDataHelper.Transform(cSData4, val * val14 * val9 * val15 * val13);
			outputGems.Append(cSData4.Duplicate());
			if (integer > 2)
			{
				double num13 = 0.0;
				CSData gemSide = cSData3.Duplicate();
				currentEdgeLHS2 = val7;
				((Line)(ref currentEdgeLHS2)).Transform(val * val11 * val9 * val12 * val10);
				currentEdgeRHS2 = val8;
				((Line)(ref currentEdgeRHS2)).Transform(val * val11 * val9 * val12 * val10);
				num13 = ((double)integer - 2.0) / 2.0;
				for (int k = 0; (double)k < num13; k++)
				{
					SideGemLhs(ref gemSide, ref currentEdgeLHS2, ref currentEdgeRHS2, num7, num5, ref outputGems);
				}
				gemSide = cSData4.Duplicate();
				currentEdgeLHS2 = val7;
				((Line)(ref currentEdgeLHS2)).Transform(val * val14 * val9 * val15 * val13);
				currentEdgeRHS2 = val8;
				((Line)(ref currentEdgeRHS2)).Transform(val * val14 * val9 * val15 * val13);
				num13 = ((double)integer - 2.0) / 2.0 - ((double)integer - 2.0) % 2.0;
				for (int l = 0; (double)l < num13; l++)
				{
					SideGemRhs(ref gemSide, ref currentEdgeLHS2, ref currentEdgeRHS2, num7, num5, ref outputGems);
				}
			}
		}
		oGem.SetData(outputGems);
		return base.Update();
	}

	private static void SideGemLhs(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
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
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = Vector3d.CrossProduct(((Line)(ref currentEdgeLHS)).Direction, ((Line)(ref currentEdgeRHS)).From - ((Line)(ref currentEdgeLHS)).From);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = Vector3d.CrossProduct(((Line)(ref currentEdgeLHS)).Direction, val);
		((Vector3d)(ref val2)).Unitize();
		Line val3 = currentEdgeLHS;
		Transform val4 = Transform.Translation(val2 * spacing);
		((Line)(ref val3)).Transform(val4);
		Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(((Line)(ref currentEdgeRHS)).From, ((Line)(ref currentEdgeRHS)).Direction, val, ((Line)(ref val3)).From, ((Line)(ref val3)).Direction, val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), ((Line)(ref currentEdgeLHS)).Direction, ((Line)(ref currentEdgeLHS)).From);
		Transform val7 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), ((Line)(ref val3)).Direction, ((Line)(ref val3)).From);
		CSDataHelper.Transform(gemSide, val6 * val7 * val5);
		outputGems.Append(gemSide.Duplicate());
		((Line)(ref currentEdgeLHS)).Transform(val6 * val7 * val5);
		currentEdgeRHS = val3;
	}

	private static void SideGemRhs(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
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
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = Vector3d.CrossProduct(((Line)(ref currentEdgeRHS)).Direction, ((Line)(ref currentEdgeLHS)).From - ((Line)(ref currentEdgeRHS)).From);
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = Vector3d.CrossProduct(((Line)(ref currentEdgeRHS)).Direction, val);
		((Vector3d)(ref val2)).Unitize();
		Line val3 = currentEdgeRHS;
		Transform val4 = Transform.Translation(val2 * spacing);
		((Line)(ref val3)).Transform(val4);
		Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(((Line)(ref currentEdgeLHS)).From, ((Line)(ref currentEdgeLHS)).Direction, val, ((Line)(ref val3)).From, ((Line)(ref val3)).Direction, val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), ((Line)(ref currentEdgeRHS)).Direction, ((Line)(ref currentEdgeRHS)).From);
		Transform val7 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), ((Line)(ref val3)).Direction, ((Line)(ref val3)).From);
		CSDataHelper.Transform(gemSide, val6 * val7 * val5);
		outputGems.Append(gemSide.Duplicate());
		((Line)(ref currentEdgeRHS)).Transform(val6 * val7 * val5);
		currentEdgeLHS = val3;
	}
}
