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
			CSUtilGeometry.OrientToCurveNalp(nurbsCurve, param, ref x);
			Transform transform = Transform.Translation(0.0, 0.0, zOffset + num4);
			Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			CSData cSData = gem.Duplicate();
			CSDataHelper.Transform(cSData, x * transform * transform2 * transform3);
			outputGems.Append(cSData.Duplicate());
			if (integer > 1)
			{
				CSData cSData2 = null;
				Line line = new Line(new Point3d(0.0 - num9, 0.0 - num7, 0.0), new Point3d(0.0 - num8, num7, 0.0));
				Line line2 = new Line(new Point3d(num9, 0.0 - num7, 0.0), new Point3d(num8, num7, 0.0));
				Line unset = Line.Unset;
				Line unset2 = Line.Unset;
				double num10 = 0.0;
				cSData2 = cSData.Duplicate();
				unset = line;
				unset.Transform(x * transform * transform2 * transform3);
				unset2 = line2;
				unset2.Transform(x * transform * transform2 * transform3);
				num10 = ((double)integer - 1.0) / 2.0;
				for (int i = 0; (double)i < num10; i++)
				{
					SideGemLHS(ref cSData2, ref unset, ref unset2, num6, num5, ref outputGems);
				}
				cSData2 = cSData.Duplicate();
				unset = line;
				unset.Transform(x * transform * transform2 * transform3);
				unset2 = line2;
				unset2.Transform(x * transform * transform2 * transform3);
				num10 = ((double)integer - 1.0) / 2.0 - ((double)integer - 1.0) % 2.0;
				for (int j = 0; (double)j < num10; j++)
				{
					SideGemRHS(ref cSData2, ref unset, ref unset2, num6, num5, ref outputGems);
				}
			}
		}
		else
		{
			Line line3 = new Line(new Point3d(0.0 - num9, 0.0 - num7, 0.0), new Point3d(0.0 - num8, num7, 0.0));
			Line line4 = new Line(new Point3d(num9, 0.0 - num7, 0.0), new Point3d(num8, num7, 0.0));
			Line unset3 = Line.Unset;
			Line unset4 = Line.Unset;
			double num11 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(line3.Direction, line4.Direction);
			if (num3 > num2)
			{
				num11 = 0.0 - num11;
			}
			Transform x2 = default(Transform);
			CSUtilGeometry.OrientToCurveNalp(nurbsCurve, param, ref x2);
			Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			CSData cSData3 = gem.Duplicate();
			Transform transform5 = CSBlockUtil.CSUtilMath.Rotation(num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			unset4 = line4;
			unset4.Transform(transform5);
			Transform transform6 = Transform.Translation(0.0, 0.0 - (unset4.From.Y + num5 / 2.0), zOffset + num4);
			Transform transform7 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num6 / 2.0), unset4.Direction, unset4.From);
			CSDataHelper.Transform(cSData3, x2 * transform6 * transform4 * transform7 * transform5);
			outputGems.Append(cSData3.Duplicate());
			CSData cSData4 = gem.Duplicate();
			Transform transform8 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			unset3 = line3;
			unset3.Transform(transform8);
			Transform transform9 = Transform.Translation(0.0, 0.0 - (unset3.From.Y - num5 / 2.0), zOffset + num4);
			Transform transform10 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num6 / 2.0), unset3.Direction, unset3.From);
			CSDataHelper.Transform(cSData4, x2 * transform9 * transform4 * transform10 * transform8);
			outputGems.Append(cSData4.Duplicate());
			if (integer > 2)
			{
				CSData cSData5 = null;
				double num12 = 0.0;
				cSData5 = cSData3.Duplicate();
				unset3 = line3;
				unset3.Transform(x2 * transform6 * transform4 * transform7 * transform5);
				unset4 = line4;
				unset4.Transform(x2 * transform6 * transform4 * transform7 * transform5);
				num12 = ((double)integer - 2.0) / 2.0;
				for (int k = 0; (double)k < num12; k++)
				{
					SideGemLHS(ref cSData5, ref unset3, ref unset4, num6, num5, ref outputGems);
				}
				cSData5 = cSData4.Duplicate();
				unset3 = line3;
				unset3.Transform(x2 * transform9 * transform4 * transform10 * transform8);
				unset4 = line4;
				unset4.Transform(x2 * transform9 * transform4 * transform10 * transform8);
				num12 = ((double)integer - 2.0) / 2.0 - ((double)integer - 2.0) % 2.0;
				for (int l = 0; (double)l < num12; l++)
				{
					SideGemRHS(ref cSData5, ref unset3, ref unset4, num6, num5, ref outputGems);
				}
			}
		}
		OutGem.SetData(outputGems);
		return base.Update();
	}

	private static void SideGemLHS(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		Vector3d vector3d = Vector3d.CrossProduct(currentEdgeLHS.Direction, currentEdgeRHS.From - currentEdgeLHS.From);
		vector3d.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(currentEdgeLHS.Direction, vector3d);
		vector3d2.Unitize();
		Line line = currentEdgeLHS;
		line.Transform(Transform.Translation(vector3d2 * spacing));
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(currentEdgeRHS.From, currentEdgeRHS.Direction, vector3d, line.From, line.Direction, vector3d);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), currentEdgeLHS.Direction, currentEdgeLHS.From);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), line.Direction, line.From);
		CSDataHelper.Transform(gemSide, transform2 * transform3 * transform);
		outputGems.Append(gemSide.Duplicate());
		currentEdgeLHS.Transform(transform2 * transform3 * transform);
		currentEdgeRHS = line;
	}

	private static void SideGemRHS(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		Vector3d vector3d = Vector3d.CrossProduct(currentEdgeRHS.Direction, currentEdgeLHS.From - currentEdgeRHS.From);
		vector3d.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(currentEdgeRHS.Direction, vector3d);
		vector3d2.Unitize();
		Line line = currentEdgeRHS;
		line.Transform(Transform.Translation(vector3d2 * spacing));
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(currentEdgeLHS.From, currentEdgeLHS.Direction, vector3d, line.From, line.Direction, vector3d);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), currentEdgeRHS.Direction, currentEdgeRHS.From);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), line.Direction, line.From);
		CSDataHelper.Transform(gemSide, transform2 * transform3 * transform);
		outputGems.Append(gemSide.Duplicate());
		currentEdgeRHS.Transform(transform2 * transform3 * transform);
		currentEdgeLHS = line;
	}
}
