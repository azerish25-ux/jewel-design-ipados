using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSCompatibility;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("SortedGemCurves", "{2BBF00F3-3E1A-4050-BD1E-3F24B2DA19D4}")]
public sealed class CSSortedGemCurves : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InOffsetZ;

	public readonly CSInputChannel InOffsetRight;

	public readonly CSInputChannel InOffsetLeft;

	public readonly CSOutputChannel OutSortedGems;

	public readonly CSOutputChannel OutLeftCurve;

	public readonly CSOutputChannel OutRightCurve;

	public readonly CSOutputChannel OutMiddleCurve;

	public CSSortedGemCurves(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InOffsetZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetRight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetLeft = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutSortedGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutLeftCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutMiddleCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutRightCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		if (InGems.Data == null)
		{
			return UpdateMissing("Gems");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve curve = InCurve.Curve;
		List<CSGemData> gemList = InGems.GemList;
		for (int i = 0; i < gemList.Count; i++)
		{
			if (gemList[i] != null)
			{
				cSDataArray.Append(new CSChannelGemData(gemList[i], curve));
				CSUtilGemRails.SortGems(cSDataArray);
			}
		}
		CSUtilGemRails.CreateRails(cSDataArray);
		double offsetLeft = InOffsetLeft.Double;
		double offsetRight = InOffsetRight.Double;
		double offsetZ = InOffsetZ.Double;
		Curve crv = CSUtilGemRails.RailRc(cSDataArray);
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, crv.Convert(), offsetLeft, offsetZ, out Curve outRail))
		{
			return UpdateError("Error getting left curve");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, crv.Convert(), offsetRight, offsetZ, out Curve outRail2))
		{
			return UpdateError("Error getting right curve");
		}
		if (!CSUtilGemRails.CalculateRail(cSDataArray.Array, offsetZ, out var rail))
		{
			return UpdateError("Error getting middle curve");
		}
		OutLeftCurve.SetData(new CSOnGeometryData((GeometryBase)(object)outRail));
		OutMiddleCurve.SetData(new CSOnGeometryData((GeometryBase)(object)rail.Convert()));
		OutRightCurve.SetData(new CSOnGeometryData((GeometryBase)(object)outRail2));
		OutSortedGems.SetData(cSDataArray);
		return base.Update();
	}
}
