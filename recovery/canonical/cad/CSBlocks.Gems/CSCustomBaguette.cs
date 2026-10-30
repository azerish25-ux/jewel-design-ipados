using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSGemInstance;

namespace CSBlocks.Gems;

[CSBlock("CustomBaguette", "{F53BB796-4521-49A7-BB9F-F04688CE0C87}")]
public sealed class CSCustomBaguette : CSBlock
{
	public CSInputChannel InLength { get; }

	public CSInputChannel InWidthBottom { get; }

	public CSInputChannel InWidthTop { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InGirdleThickness { get; }

	public CSOutputChannel OutGem { get; }

	public CSCustomBaguette(string identifier)
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
		CSGemInstanceDefinitionCustomBaguetteTapered definition = new CSGemInstanceDefinitionCustomBaguetteTapered(length, widthBottom, widthTop, thickness, girdleThickness);
		OutGem.SetData(new CSGemData(definition));
		return base.Update();
	}
}
