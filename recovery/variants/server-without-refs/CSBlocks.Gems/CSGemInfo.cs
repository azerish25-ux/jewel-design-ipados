using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;

namespace CSBlocks.Gems;

[CSBlock("GemInfo", "{086F2017-0809-42A3-94D1-6104B2DB94F6}")]
public sealed class CSGemInfo : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSOutputChannel OutMaxSizeX { get; }

	public CSOutputChannel OutMinSizeX { get; }

	public CSOutputChannel OutSizeX { get; }

	public CSOutputChannel OutMaxSizeY { get; }

	public CSOutputChannel OutMinSizeY { get; }

	public CSOutputChannel OutSizeY { get; }

	public CSOutputChannel OutMaxSizeZ { get; }

	public CSOutputChannel OutMinSizeZ { get; }

	public CSOutputChannel OutSizeZ { get; }

	public CSOutputChannel OutXDownUnscaled { get; }

	public CSOutputChannel OutXUpUnscaled { get; }

	public CSOutputChannel OutType { get; }

	public CSOutputChannel OutShape { get; }

	public CSOutputChannel OutSettingType { get; }

	public CSGemInfo(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutMaxSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMaxSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutXDownUnscaled = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutXUpUnscaled = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutType = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		OutShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		OutSettingType = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		CSGemData gem = InGem.Gem;
		if (gem == null)
		{
			return base.Update();
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown))
		{
			return UpdateError("GetUnscaledXforms failed");
		}
		OutMaxSizeX.SetData(new CSNumberData(gem.GemXMax));
		OutMinSizeX.SetData(new CSNumberData(gem.GemXMin));
		OutSizeX.SetData(new CSNumberData(gem.GemXSize));
		OutMaxSizeY.SetData(new CSNumberData(gem.GemYMax));
		OutMinSizeY.SetData(new CSNumberData(gem.GemYMin));
		OutSizeY.SetData(new CSNumberData(gem.GemYSize));
		OutMaxSizeZ.SetData(new CSNumberData(gem.GemZMax));
		OutMinSizeZ.SetData(new CSNumberData(gem.GemZMin));
		OutSizeZ.SetData(new CSNumberData(gem.GemZSize));
		OutXDownUnscaled.SetData(new CSXformData(xDown));
		OutXUpUnscaled.SetData(new CSXformData(xUp));
		OutType.SetData(new CSStringData(gem.GemType));
		OutShape.SetData(new CSStringData(gem.GemShape));
		OutSettingType.SetData(new CSStringData(gem.SettingType.ToString()));
		return base.Update();
	}
}
