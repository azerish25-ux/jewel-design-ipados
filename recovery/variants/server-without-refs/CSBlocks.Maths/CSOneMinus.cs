using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("OneMinus", "{80319A85-43CF-4E2B-B770-D60FFEEC52C8}")]
public sealed class CSOneMinus : CSBlock
{
	public readonly CSInputChannel iValue;

	public readonly CSOutputChannel oValue;

	public CSOneMinus(string identifier)
		: base(identifier)
	{
		iValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iValue.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Value");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			double value = 0.0;
			flattenArray.GetArrayItem(i).GetDouble(ref value);
			double number = 1.0 - value;
			cSDataArray.Append(new CSNumberData(number));
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			oValue.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			oValue.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oValue.SetData(cSDataArray);
		}
		return base.Update();
	}
}
