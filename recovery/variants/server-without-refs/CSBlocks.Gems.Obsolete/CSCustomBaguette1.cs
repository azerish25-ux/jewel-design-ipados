using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSGemInstance;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("CustomBaguette_V1", "{EC59749A-3035-428e-98E1-9EC755130F07}")]
public sealed class CSCustomBaguette1 : CSBlock
{
	public CSInputChannel InLength { get; }

	public CSInputChannel InWidthBottom { get; }

	public CSInputChannel InWidthTop { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InGirdleThickness { get; }

	public CSOutputChannel OutGem { get; }

	public CSCustomBaguette1(string identifier)
		: base(identifier)
	{
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWidthBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWidthTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGirdleThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		double length = InLength.Double;
		double widthBottom = InWidthBottom.Double;
		double widthTop = InWidthTop.Double;
		double thickness = InThickness.Double;
		double girdleThickness = InGirdleThickness.Double;
		CSGemInstanceDefinitionCustomBaguetteTapered definition = new CSGemInstanceDefinitionCustomBaguetteTapered(length, widthBottom, widthTop, thickness, girdleThickness, useOldShapeName: true);
		OutGem.SetData(new CSGemData(definition));
		return base.Update();
	}
}
