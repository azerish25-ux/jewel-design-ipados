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
		NurbsCurve nurbsCurve = iRingRail.NurbsCurve;
		CSGemData gem = iGemDefinition.Gem;
		int integer = iGemCount.Integer;
		int integer2 = iGemPlacementMethod.Integer;
		double num = iGemLength.Double;
		double num2 = iGemWidthTop.Double;
		double num3 = iGemWidthBottom.Double;
		double num4 = iZOffset.Double;
		double num5 = iSpacing.Double;
		double t = iPosition.Double;
		double num6 = iAngleToSideGem.Double;
		double angleDegrees = iAngleToRingRail.Double;
		CSDataArray outputGems = new CSDataArray();
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		double num7 = num / 2.0;
		double num8 = num2 / 2.0;
		double num9 = num3 / 2.0;
		if (!nurbsCurve.NormalizedLengthParameter(t, out t))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Vector3d destination = Vector3d.CrossProduct(nurbsCurve.TangentAt(t), Vector3d.YAxis);
		destination.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, nurbsCurve.PointAt(t), Vector3d.YAxis, destination);
		if (integer % 2 != 0)
		{
			Transform transform2 = Transform.Translation(0.0, 0.0, zOffset + num4);
			Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			CSData cSData = gem.Duplicate();
			CSDataHelper.Transform(cSData, transform * transform2 * transform3 * transform4);
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
				unset.Transform(transform * transform2 * transform3 * transform4);
				unset2 = line2;
				unset2.Transform(transform * transform2 * transform3 * transform4);
				num10 = ((double)integer - 1.0) / 2.0;
				for (int i = 0; (double)i < num10; i++)
				{
					SideGemLhs(ref cSData2, ref unset, ref unset2, num6, num5, ref outputGems);
				}
				cSData2 = cSData.Duplicate();
				unset = line;
				unset.Transform(transform * transform2 * transform3 * transform4);
				unset2 = line2;
				unset2.Transform(transform * transform2 * transform3 * transform4);
				num10 = ((double)integer - 1.0) / 2.0 - ((double)integer - 1.0) % 2.0;
				for (int j = 0; (double)j < num10; j++)
				{
					SideGemRhs(ref cSData2, ref unset, ref unset2, num6, num5, ref outputGems);
				}
			}
		}
		else
		{
			Line line3 = new Line(new Point3d(0.0 - num9, 0.0 - num7, 0.0), new Point3d(0.0 - num8, num7, 0.0));
			Line line4 = new Line(new Point3d(num9, 0.0 - num7, 0.0), new Point3d(num8, num7, 0.0));
			double num11 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(line3.Direction, line4.Direction);
			if (num3 > num2)
			{
				num11 = 0.0 - num11;
			}
			Transform transform5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.YAxis, Point3d.Origin);
			CSData cSData3 = gem.Duplicate();
			Transform transform6 = CSBlockUtil.CSUtilMath.Rotation(num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			Line line5 = line4;
			line5.Transform(transform6);
			Transform transform7 = Transform.Translation(0.0, 0.0 - (line5.FromY + num5 / 2.0), zOffset + num4);
			Transform transform8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num6 / 2.0), line5.Direction, line5.From);
			CSDataHelper.Transform(cSData3, transform * transform7 * transform5 * transform8 * transform6);
			outputGems.Append(cSData3.Duplicate());
			CSData cSData4 = gem.Duplicate();
			Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(0.0 - num11 / 2.0 + CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
			Line line6 = line3;
			line6.Transform(transform9);
			Transform transform10 = Transform.Translation(0.0, 0.0 - (line6.FromY - num5 / 2.0), zOffset + num4);
			Transform transform11 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num6 / 2.0), line6.Direction, line6.From);
			CSDataHelper.Transform(cSData4, transform * transform10 * transform5 * transform11 * transform9);
			outputGems.Append(cSData4.Duplicate());
			if (integer > 2)
			{
				double num12 = 0.0;
				CSData gemSide = cSData3.Duplicate();
				line6 = line3;
				line6.Transform(transform * transform7 * transform5 * transform8 * transform6);
				line5 = line4;
				line5.Transform(transform * transform7 * transform5 * transform8 * transform6);
				num12 = ((double)integer - 2.0) / 2.0;
				for (int k = 0; (double)k < num12; k++)
				{
					SideGemLhs(ref gemSide, ref line6, ref line5, num6, num5, ref outputGems);
				}
				gemSide = cSData4.Duplicate();
				line6 = line3;
				line6.Transform(transform * transform10 * transform5 * transform11 * transform9);
				line5 = line4;
				line5.Transform(transform * transform10 * transform5 * transform11 * transform9);
				num12 = ((double)integer - 2.0) / 2.0 - ((double)integer - 2.0) % 2.0;
				for (int l = 0; (double)l < num12; l++)
				{
					SideGemRhs(ref gemSide, ref line6, ref line5, num6, num5, ref outputGems);
				}
			}
		}
		oGem.SetData(outputGems);
		return base.Update();
	}

	private static void SideGemLhs(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		Vector3d vector3d = Vector3d.CrossProduct(currentEdgeLHS.Direction, currentEdgeRHS.From - currentEdgeLHS.From);
		vector3d.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(currentEdgeLHS.Direction, vector3d);
		vector3d2.Unitize();
		Line line = currentEdgeLHS;
		Transform xform = Transform.Translation(vector3d2 * spacing);
		line.Transform(xform);
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(currentEdgeRHS.From, currentEdgeRHS.Direction, vector3d, line.From, line.Direction, vector3d);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), currentEdgeLHS.Direction, currentEdgeLHS.From);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle / 2.0), line.Direction, line.From);
		CSDataHelper.Transform(gemSide, transform2 * transform3 * transform);
		outputGems.Append(gemSide.Duplicate());
		currentEdgeLHS.Transform(transform2 * transform3 * transform);
		currentEdgeRHS = line;
	}

	private static void SideGemRhs(ref CSData gemSide, ref Line currentEdgeLHS, ref Line currentEdgeRHS, double angle, double spacing, ref CSDataArray outputGems)
	{
		Vector3d vector3d = Vector3d.CrossProduct(currentEdgeRHS.Direction, currentEdgeLHS.From - currentEdgeRHS.From);
		vector3d.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(currentEdgeRHS.Direction, vector3d);
		vector3d2.Unitize();
		Line line = currentEdgeRHS;
		Transform xform = Transform.Translation(vector3d2 * spacing);
		line.Transform(xform);
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(currentEdgeLHS.From, currentEdgeLHS.Direction, vector3d, line.From, line.Direction, vector3d);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), currentEdgeRHS.Direction, currentEdgeRHS.From);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - angle / 2.0), line.Direction, line.From);
		CSDataHelper.Transform(gemSide, transform2 * transform3 * transform);
		outputGems.Append(gemSide.Duplicate());
		currentEdgeRHS.Transform(transform2 * transform3 * transform);
		currentEdgeLHS = line;
	}
}
