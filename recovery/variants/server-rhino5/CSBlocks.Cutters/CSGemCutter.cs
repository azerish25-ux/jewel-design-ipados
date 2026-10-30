using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("GemCutter", "{B5A9EE57-8223-4e8a-AD69-42AD25B1F3E5}")]
public sealed class CSGemCutter : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InTopX { get; }

	public CSInputChannel InTopZ { get; }

	public CSInputChannel InSeatX { get; }

	public CSInputChannel InSeatZ { get; }

	public CSInputChannel InBottomX { get; }

	public CSInputChannel InBottomZ { get; }

	public CSOutputChannel OutBrep { get; }

	public CSGemCutter(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InTopZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InSeatX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InSeatZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBottomX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBottomZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		double num = InGirdleSpacing.Double;
		double num2 = InPlacement.Double;
		double num3 = InTopX.Double;
		double num4 = InTopZ.Double;
		double num5 = InSeatX.Double;
		double num6 = InSeatZ.Double;
		double num7 = InBottomX.Double;
		double num8 = InBottomZ.Double;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData) || !CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown))
			{
				continue;
			}
			double num9 = Math.Min(cSGemData.GemXMax, cSGemData.GemYMax);
			NurbsCurve nurbsCurve = cSGemData.GetNurbsCurve();
			nurbsCurve.Transform(xDown);
			double num10 = (num + num9) / num9;
			if (num10 <= 0.01)
			{
				num10 = 0.01;
			}
			if (!OffsetZ(nurbsCurve, num10, num2, out var crvOffset))
			{
				continue;
			}
			double num11 = (num + num3 + num9) / num9;
			if (num11 <= 0.01)
			{
				num11 = 0.01;
			}
			if (!OffsetZ(nurbsCurve, num11, num2 + num4, out var crvOffset2))
			{
				continue;
			}
			double num12 = (num + num5 + num9) / num9;
			if (num12 <= 0.01)
			{
				num12 = 0.01;
			}
			if (OffsetZ(nurbsCurve, num12, num2 + num6, out var crvOffset3))
			{
				double num13 = (num + num5 + num7 + num9) / num9;
				if (num13 <= 0.01)
				{
					num13 = 0.01;
				}
				if (OffsetZ(nurbsCurve, num13, num2 + num6 + num8, out var crvOffset4))
				{
					Brep brep = CSUtilSweepRC.Loft(new Curve[4] { crvOffset2, crvOffset, crvOffset3, crvOffset4 }, LoftType.Straight, doCapped: true);
					brep.Transform(xUp);
					cSDataArray.Append(new CSOnGeometryData(brep, xUp, applyTransform: false));
				}
			}
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(Curve crv, double scale, double placement, out NurbsCurve crvOffset)
	{
		Transform transform = Transform.Scale(Plane.WorldXY, scale, scale, 1.0);
		Transform transform2 = Transform.Translation(0.0, 0.0, placement);
		crvOffset = crv.ToNurbsCurve();
		crvOffset.Transform(transform2 * transform);
		return true;
	}
}
