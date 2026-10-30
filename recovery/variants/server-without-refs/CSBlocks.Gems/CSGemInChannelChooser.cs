using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;

namespace CSBlocks.Gems;

[CSBlock("GemInChannelChooser", "{439EDE81-D5DD-4212-A4BB-CDBC41AAA773}")]
public sealed class CSGemInChannelChooser : CSBlock
{
	public readonly CSInputChannel iChannelMode;

	public readonly CSInputChannel iGemSpacingProng;

	public readonly CSInputChannel iWallDropProng;

	public readonly CSInputChannel iChannelWallProng;

	public readonly CSInputChannel iChannelSpacingProng;

	public readonly CSInputChannel iChannelEndSpacingProng;

	public readonly CSInputChannel iGemSpacingChannel;

	public readonly CSInputChannel iWallDropChannel;

	public readonly CSInputChannel iChannelWallChannel;

	public readonly CSInputChannel iChannelSpacingChannel;

	public readonly CSInputChannel iChannelEndSpacingChannel;

	public readonly CSOutputChannel oGemSpacing;

	public readonly CSOutputChannel oWallDrop;

	public readonly CSOutputChannel oChannelWall;

	public readonly CSOutputChannel oChannelSpacing;

	public readonly CSOutputChannel oChannelEndSpacing;

	public CSGemInChannelChooser(string identifier)
		: base(identifier)
	{
		iChannelMode = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		iGemSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingProng = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWallDropChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacingChannel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemSpacing = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oWallDrop = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelWall = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oChannelEndSpacing = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		if (iChannelMode.Boolean)
		{
			oGemSpacing.SetData(iGemSpacingChannel.Data);
			oWallDrop.SetData(iWallDropChannel.Data);
			oChannelWall.SetData(iChannelWallChannel.Data);
			oChannelSpacing.SetData(iChannelSpacingChannel.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingChannel.Data);
		}
		else
		{
			oGemSpacing.SetData(iGemSpacingProng.Data);
			oWallDrop.SetData(iWallDropProng.Data);
			oChannelWall.SetData(iChannelWallProng.Data);
			oChannelSpacing.SetData(iChannelSpacingProng.Data);
			oChannelEndSpacing.SetData(iChannelEndSpacingProng.Data);
		}
		return base.Update();
	}
}
