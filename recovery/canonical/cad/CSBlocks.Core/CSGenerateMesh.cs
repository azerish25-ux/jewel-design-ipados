using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Core;

[CSBlock("GenerateMesh", "{B988BE38-8E83-4596-AF37-45CF65D4EBD7}")]
public sealed class CSGenerateMesh : CSBlock
{
	public readonly CSInputChannel InObjects;

	public readonly CSOutputChannel OutObjects;

	public CSGenerateMesh(string identifier)
		: base(identifier)
	{
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (InObjects == null)
		{
			return UpdateMissing("Missing Objects");
		}
		CSDataArray cSDataArray = InObjects.GetFlattenArray().Duplicate() as CSDataArray;
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (cSDataArray.GetArrayItem(i) is CSOnGeometryData cSOnGeometryData)
			{
				cSOnGeometryData.PreProcessDisplayInfo();
			}
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			OutObjects.SetData(null);
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			OutObjects.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutObjects.SetData(cSDataArray);
		}
		return base.Update();
	}
}
