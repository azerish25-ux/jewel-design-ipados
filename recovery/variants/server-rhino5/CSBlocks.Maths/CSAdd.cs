using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("Add", "{8793B48F-30C7-4841-9314-157A5C7BB629}")]
public sealed class CSAdd : CSBlock
{
	public readonly CSInputChannel iStart;

	public readonly CSInputChannel iValue;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oNegated;

	public readonly CSOutputChannel oOneMinus;

	public CSAdd(string identifier)
		: base(identifier)
	{
		iStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oNegated = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOneMinus = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iStart.Data;
		CSData data2 = iValue.Data;
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
						double num = value + value2;
						cSDataArray.Append(new CSNumberData(num));
						cSDataArray2.Append(new CSNumberData(0.0 - num));
						cSDataArray3.Append(new CSNumberData(1.0 - num));
					}
				}
			}
			else if (CSDataHelper.GetArrayCount(data) <= 0 && CSDataHelper.GetArrayCount(data2) > 0)
			{
				data?.GetDouble(ref value);
				for (int k = 0; k < CSDataHelper.GetArrayCount(data2); k++)
				{
					CSDataHelper.GetArrayItem(data2, k).GetDouble(ref value2);
					double num = value + value2;
					cSDataArray.Append(new CSNumberData(num));
					cSDataArray2.Append(new CSNumberData(0.0 - num));
					cSDataArray3.Append(new CSNumberData(1.0 - num));
				}
			}
			else if (CSDataHelper.GetArrayCount(data) > 0 && CSDataHelper.GetArrayCount(data2) <= 0)
			{
				data2?.GetDouble(ref value2);
				for (int l = 0; l < CSDataHelper.GetArrayCount(data); l++)
				{
					CSDataHelper.GetArrayItem(data, l).GetDouble(ref value);
					double num = value + value2;
					cSDataArray.Append(new CSNumberData(num));
					cSDataArray2.Append(new CSNumberData(0.0 - num));
					cSDataArray3.Append(new CSNumberData(1.0 - num));
				}
			}
			oValue.SetData(cSDataArray);
			oNegated.SetData(cSDataArray2);
			oOneMinus.SetData(cSDataArray3);
		}
		else
		{
			data?.GetDouble(ref value);
			data2?.GetDouble(ref value2);
			double num = value + value2;
			oValue.SetData(new CSNumberData(num));
			oNegated.SetData(new CSNumberData(0.0 - num));
			oOneMinus.SetData(new CSNumberData(1.0 - num));
		}
		return base.Update();
	}
}
