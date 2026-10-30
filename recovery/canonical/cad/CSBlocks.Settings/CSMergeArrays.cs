using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Settings;

[CSBlock("MergeArrays", "{297BE06A-1326-4802-8708-6824A846A63A}")]
public sealed class CSMergeArrays : CSBlock
{
	public readonly CSInputChannel InObjects1;

	public readonly CSInputChannel InObjects2;

	public readonly CSOutputChannel OutObjects;

	public CSMergeArrays(string identifier)
		: base(identifier)
	{
		InObjects1 = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InObjects2 = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSData data = InObjects1.Data;
		CSData data2 = InObjects2.Data;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
		{
			CSDataArray cSDataArray2 = new CSDataArray();
			CSData arrayItem = CSDataHelper.GetArrayItem(data, i);
			for (int j = 0; j < CSDataHelper.GetArrayCount(arrayItem); j++)
			{
				cSDataArray2.Append(CSDataHelper.GetArrayItem(arrayItem, j));
			}
			CSData arrayItem2 = CSDataHelper.GetArrayItem(data2, i);
			for (int k = 0; k < CSDataHelper.GetArrayCount(arrayItem2); k++)
			{
				cSDataArray2.Append(CSDataHelper.GetArrayItem(arrayItem2, k));
			}
			cSDataArray.Append(cSDataArray2);
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}
}
