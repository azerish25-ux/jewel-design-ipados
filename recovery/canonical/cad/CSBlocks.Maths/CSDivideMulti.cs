using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("DivideMulti", "{62FCC32E-B7EB-4b4e-8164-BC2528CFF2DE}")]
public sealed class CSDivideMulti : CSBlockExpandableInput
{
	public CSInputChannel InStart { get; }

	public CSOutputChannel OutValue { get; }

	public CSOutputChannel OutNegated { get; }

	public CSOutputChannel OutOneMinus { get; }

	protected override string ExpandableInputChannelIdentifier => "InValue";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Double;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSDivideMulti(string identifier)
		: base(identifier)
	{
		InStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		CreateExpansionInputChannel();
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InStart.GetFlattenArray();
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
				if (value2 != 0.0)
				{
					num /= value2;
				}
			}
			cSDataArray.Append(new CSNumberData(num));
			cSDataArray2.Append(new CSNumberData(0.0 - num));
			cSDataArray3.Append(new CSNumberData(1.0 - num));
		}
		OutValue.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		OutNegated.SetData((cSDataArray2.GetArrayCount() == 1) ? cSDataArray2.GetArrayItem(0) : cSDataArray2);
		OutOneMinus.SetData((cSDataArray3.GetArrayCount() == 1) ? cSDataArray3.GetArrayItem(0) : cSDataArray3);
		return base.Update();
	}
}
