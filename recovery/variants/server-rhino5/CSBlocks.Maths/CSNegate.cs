using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("Negate", "{4D5B81E5-0EF1-4BF4-9B2E-012A65429320}")]
public sealed class CSNegate : CSBlock
{
	public readonly CSInputChannel InValue;

	public readonly CSOutputChannel OutValue;

	public CSNegate(string identifier)
		: base(identifier)
	{
		InValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InValue.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Value");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			double value = 0.0;
			flattenArray.GetArrayItem(i).GetDouble(ref value);
			double number = value * -1.0;
			cSDataArray.Append(new CSNumberData(number));
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			OutValue.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			OutValue.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutValue.SetData(cSDataArray);
		}
		return base.Update();
	}
}
