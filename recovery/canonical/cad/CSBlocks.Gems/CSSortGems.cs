using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("SortGems", "{E35DBAC8-C82B-4792-A547-19E841C25367}")]
public sealed class CSSortGems : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InCurve;

	public readonly CSOutputChannel OutSortedGems;

	public CSSortGems(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutSortedGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		foreach (CSGemData gem in InGems.GemList)
		{
			if (gem != null)
			{
				cSDataArray.Append(new CSChannelGemData(gem, nurbsCurve));
				CSUtilGemRails.SortGems(cSDataArray);
			}
		}
		CSUtilGemRails.CreateRails(cSDataArray);
		OutSortedGems.SetData(cSDataArray);
		return base.Update();
	}
}
