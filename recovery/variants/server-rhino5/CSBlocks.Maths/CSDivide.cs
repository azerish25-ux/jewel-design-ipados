using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("Divide", "{0F9CCA61-4D31-4389-BB4A-ACF559543F2F}")]
public sealed class CSDivide : CSBlock
{
	public readonly CSInputChannel InStart;

	public readonly CSInputChannel InValue;

	public readonly CSOutputChannel OutValue;

	public readonly CSOutputChannel OutNegated;

	public readonly CSOutputChannel OutOneMinus;

	public CSDivide(string identifier)
		: base(identifier)
	{
		InStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		InValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InStart.Data;
		CSData data2 = InValue.Data;
		double value = 0.0;
		double value2 = 0.0;
		if (CSDataHelper.GetArrayCount(data) > 0 || CSDataHelper.GetArrayCount(data2) > 0)
		{
			CSDataArray cSDataArray = new CSDataArray();
			CSDataArray cSDataArray2 = new CSDataArray();
			CSDataArray cSDataArray3 = new CSDataArray();
			if (CSDataHelper.GetArrayCount(data) > 0 && CSDataHelper.GetArrayCount(data2) > 0)
			{
				for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
				{
					CSDataHelper.GetArrayItem(data, i).GetDouble(ref value);
					for (int j = 0; j < CSDataHelper.GetArrayCount(data2); j++)
					{
						CSDataHelper.GetArrayItem(data2, j).GetDouble(ref value2);
						if (value2 != 0.0)
						{
							double num = value / value2;
							cSDataArray.Append(new CSNumberData(num));
							cSDataArray2.Append(new CSNumberData(0.0 - num));
							cSDataArray3.Append(new CSNumberData(1.0 - num));
						}
					}
				}
			}
			else if (CSDataHelper.GetArrayCount(data) <= 0 && CSDataHelper.GetArrayCount(data2) > 0)
			{
				data.GetDouble(ref value);
				for (int k = 0; k < CSDataHelper.GetArrayCount(data2); k++)
				{
					CSDataHelper.GetArrayItem(data2, k).GetDouble(ref value2);
					if (value2 != 0.0)
					{
						double num = value / value2;
						cSDataArray.Append(new CSNumberData(num));
						cSDataArray2.Append(new CSNumberData(0.0 - num));
						cSDataArray3.Append(new CSNumberData(1.0 - num));
					}
				}
			}
			else if (CSDataHelper.GetArrayCount(data) > 0 && CSDataHelper.GetArrayCount(data2) <= 0)
			{
				data2.GetDouble(ref value2);
				if (value2 != 0.0)
				{
					for (int l = 0; l < CSDataHelper.GetArrayCount(data); l++)
					{
						CSDataHelper.GetArrayItem(data, l).GetDouble(ref value);
						double num = value / value2;
						cSDataArray.Append(new CSNumberData(num));
						cSDataArray2.Append(new CSNumberData(0.0 - num));
						cSDataArray3.Append(new CSNumberData(1.0 - num));
					}
				}
			}
			OutValue.SetData(cSDataArray);
			OutNegated.SetData(cSDataArray2);
			OutOneMinus.SetData(cSDataArray3);
		}
		else
		{
			data.GetDouble(ref value);
			data2.GetDouble(ref value2);
			double num = value / value2;
			OutValue.SetData(new CSNumberData(num));
			OutNegated.SetData(new CSNumberData(0.0 - num));
			OutOneMinus.SetData(new CSNumberData(1.0 - num));
		}
		return base.Update();
	}
}
