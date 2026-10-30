using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSGemInstance;

namespace CSBlocks.Gems;

[CSBlock("CustomEmerald", "{A022BB6C-5E1A-457A-9BF8-C3518843893E}")]
public sealed class CSCustomEmerald : CSBlock
{
	public CSInputChannel InLength { get; }

	public CSInputChannel InWidth { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InGirdleThickness { get; }

	public CSInputChannel InCorner { get; }

	public CSOutputChannel OutGem { get; }

	public CSCustomEmerald(string identifier)
		: base(identifier)
	{
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGirdleThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCorner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		double length = InLength.Double;
		double width = InWidth.Double;
		double thickness = InThickness.Double;
		double girdleThickness = InGirdleThickness.Double;
		double corner = InCorner.Double;
		CSGemInstanceDefinitionCustomEmerald definition = new CSGemInstanceDefinitionCustomEmerald(length, width, thickness, girdleThickness, corner);
		OutGem.SetData(new CSGemData(definition));
		return base.Update();
	}
}
