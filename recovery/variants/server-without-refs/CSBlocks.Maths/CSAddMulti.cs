using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("AddMulti", "{0AFDE497-DB62-4ca5-8DE9-26B721A5F9E7}")]
public sealed class CSAddMulti : CSBlockExpandableInput
{
	public readonly CSInputChannel iStart;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	protected override string ExpandableInputChannelIdentifier => "InValue";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Double;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSAddMulti(string identifier)
		: base(identifier)
	{
		iStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		CreateExpansionInputChannel();
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iStart.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		for (int i = 1; i < base.NonExpandableInputChannels.Count; i++)
		{
			if (base.NonExpandableInputChannels[i] != null)
			{
				CSData data = base.NonExpandableInputChannels[i].Data;
				if (data != null)
				{
					cSDataArray4.Append(data);
				}
			}
		}
		CSDataArray flattenArray2 = cSDataArray4.GetFlattenArray();
		for (int j = 0; j < flattenArray.GetArrayCount(); j++)
		{
			double value = 0.0;
			flattenArray.GetArrayItem(j).GetDouble(ref value);
			double num = value;
			for (int k = 0; k < flattenArray2.GetArrayCount(); k++)
			{
				double value2 = 0.0;
				flattenArray2.GetArrayItem(k).GetDouble(ref value2);
				num += value2;
			}
			cSDataArray.Append(new CSNumberData(num));
			cSDataArray2.Append(new CSNumberData(0.0 - num));
			cSDataArray3.Append(new CSNumberData(1.0 - num));
		}
		oValue.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		oNegated.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		oOneMinus.SetData((cSDataArray3.GetArrayCount() == 1) ? cSDataArray3.GetArrayItem(0) : cSDataArray3);
		return base.Update();
	}
}
