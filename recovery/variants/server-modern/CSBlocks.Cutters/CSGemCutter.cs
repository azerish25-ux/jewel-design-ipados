using System;
using System.Collections.Generic;
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
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
			((GeometryBase)nurbsCurve).Transform(xDown);
			double num10 = (num + num9) / num9;
			if (num10 <= 0.01)
			{
				num10 = 0.01;
			}
			if (!OffsetZ((Curve)(object)nurbsCurve, num10, num2, out var crvOffset))
			{
				continue;
			}
			double num11 = (num + num3 + num9) / num9;
			if (num11 <= 0.01)
			{
				num11 = 0.01;
			}
			if (!OffsetZ((Curve)(object)nurbsCurve, num11, num2 + num4, out var crvOffset2))
			{
				continue;
			}
			double num12 = (num + num5 + num9) / num9;
			if (num12 <= 0.01)
			{
				num12 = 0.01;
			}
			if (OffsetZ((Curve)(object)nurbsCurve, num12, num2 + num6, out var crvOffset3))
			{
				double num13 = (num + num5 + num7 + num9) / num9;
				if (num13 <= 0.01)
				{
					num13 = 0.01;
				}
				if (OffsetZ((Curve)(object)nurbsCurve, num13, num2 + num6 + num8, out var crvOffset4))
				{
					Brep val = CSUtilSweepRC.Loft((IEnumerable<Curve>)(object)new Curve[4]
					{
						(Curve)crvOffset2,
						(Curve)crvOffset,
						(Curve)crvOffset3,
						(Curve)crvOffset4
					}, (LoftType)3, doCapped: true);
					((GeometryBase)val).Transform(xUp);
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val, xUp, applyTransform: false));
				}
			}
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(Curve crv, double scale, double placement, out NurbsCurve crvOffset)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Scale(Plane.WorldXY, scale, scale, 1.0);
		Transform val2 = Transform.Translation(0.0, 0.0, placement);
		crvOffset = crv.ToNurbsCurve();
		((GeometryBase)crvOffset).Transform(val2 * val);
		return true;
	}
}
