using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;

namespace CSBlocks.Shank.ShankV1;

[CSBlock("ShankPicker", "{F2237963-CE8F-4043-A427-00DFA07CC094}")]
public sealed class CSShankPicker : CSBlock
{
	public readonly CSInputChannel InWhich;

	public readonly CSInputChannel InSpacingProng;

	public readonly CSInputChannel InSpacingChannel;

	public readonly CSInputChannel InSpacingTolleranceProng;

	public readonly CSInputChannel InSpacingTolleranceChannel;

	public readonly CSInputChannel InWallWidthProng;

	public readonly CSInputChannel InWallWidthChannel;

	public readonly CSInputChannel InChannelWallGemOverlapProng;

	public readonly CSInputChannel InChannelWallGemOverlapChannel;

	public readonly CSInputChannel InChannelEndSlopeSpacingProng;

	public readonly CSInputChannel InChannelEndSlopeSpacingChannel;

	public readonly CSInputChannel InChannelWallDropProng;

	public readonly CSInputChannel InChannelWallDropChannel;

	public readonly CSInputChannel InChannelBlendProng;

	public readonly CSInputChannel InChannelBlendChannel;

	public readonly CSInputChannel InChannelPullDownProng;

	public readonly CSInputChannel InChannelPullDownChannel;

	public readonly CSOutputChannel OutSpacing;

	public readonly CSOutputChannel OutSpacingTollerance;

	public readonly CSOutputChannel OutWallWidth;

	public readonly CSOutputChannel OutChannelWallGemOverlap;

	public readonly CSOutputChannel OutChannelEndSlopeSpacing;

	public readonly CSOutputChannel OutChannelWallDrop;

	public readonly CSOutputChannel OutChannelBlend;

	public readonly CSOutputChannel OutChannelPullDown;

	public CSShankPicker(string identifier)
		: base(identifier)
	{
		InWhich = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTolleranceProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTolleranceChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InWallWidthProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InWallWidthChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallGemOverlapProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallGemOverlapChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSlopeSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSlopeSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDropProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDropChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBlendProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBlendChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelPullDownProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelPullDownChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSpacingTollerance = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutWallWidth = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelWallGemOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEndSlopeSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelWallDrop = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelBlend = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelPullDown = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = true;
		InWhich.GetBoolean(ref value);
		value = !value;
		OutSpacing.SetData(value ? InSpacingProng.Data : InSpacingChannel.Data);
		OutSpacingTollerance.SetData(value ? InSpacingTolleranceProng.Data : InSpacingTolleranceChannel.Data);
		OutWallWidth.SetData(value ? InWallWidthProng.Data : InWallWidthChannel.Data);
		OutChannelWallGemOverlap.SetData(value ? InChannelWallGemOverlapProng.Data : InChannelWallGemOverlapChannel.Data);
		OutChannelEndSlopeSpacing.SetData(value ? InChannelEndSlopeSpacingProng.Data : InChannelEndSlopeSpacingChannel.Data);
		OutChannelWallDrop.SetData(value ? InChannelWallDropProng.Data : InChannelWallDropChannel.Data);
		OutChannelBlend.SetData(value ? InChannelBlendProng.Data : InChannelBlendChannel.Data);
		OutChannelPullDown.SetData(value ? InChannelPullDownProng.Data : InChannelPullDownChannel.Data);
		return base.Update();
	}
}
