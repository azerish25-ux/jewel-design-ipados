using System.IO;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("GetProfileName", "{4D5ACF37-F646-4555-8668-57DB5FFBFEF7}")]
public sealed class CSGetProfileName : CSBlock
{
	public readonly CSInputChannel InProfiles;

	public readonly CSOutputChannel OutProfileNames;

	public CSGetProfileName(string identifier)
		: base(identifier)
	{
		InProfiles = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutProfileNames = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InProfiles.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSProfileData cSProfileData)
			{
				OutProfileNames.SetData(new CSStringData(Path.GetFileNameWithoutExtension(cSProfileData.Name)));
			}
		}
		return base.Update();
	}
}
