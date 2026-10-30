using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;

namespace CSBlocks;

[CSBlock("CSNullBlock", "{00000000-0000-0000-0000-000000000000}")]
public sealed class CSNullBlock : CSBlockExpandableInput
{
	protected override ChannelArrayType ExpandableInputChannelArrayType { get; }

	protected override ChannelDataType ExpandableInputChannelDataType { get; }

	protected override string ExpandableInputChannelIdentifier { get; }

	public CSNullBlock(string identifier)
		: base(identifier)
	{
	}

	public override bool Update()
	{
		return UpdateError("Block No Longer Supported");
	}
}
