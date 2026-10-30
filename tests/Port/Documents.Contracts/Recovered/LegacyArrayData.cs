#nullable disable
// Exact source array/copy/arithmetic bodies with observation-only base values and channels.
using JewelDesign.Documents;
namespace DocumentContractChecks.Recovered.Arrays;
internal interface ICSArray{int GetArrayCount();CSData GetArrayItem(int index);CSDataArray GetFlattenArray();}
internal class CSData
{
    internal LegacyChannelValue Stored;
    internal CSData(){Stored=LegacyChannelValue.Empty;}
    internal CSData(LegacyChannelValue value){Stored=value;}
    protected CSData(CSData copy){Stored=copy.Stored;}
    public virtual bool GetDouble(ref double value)=>Stored.GetDouble(ref value);
    public virtual CSData Duplicate()=>new CSData(this);
}
internal sealed class CSNumberData:CSData
{
    private readonly double _numberData;internal CSNumberData(double value):base(LegacyChannelValue.FromNumber(value)){_numberData=value;}
    public override CSData Duplicate()=>new CSNumberData(_numberData);
public override bool GetDouble(ref double value)
	{
		value = _numberData;
		return true;
	}
}
internal sealed class CSDataArray:CSData,ICSArray{
internal List<CSData> Array{get;}=new();public CSDataArray(){}public int GetArrayCount()=>Array.Count;public void Append(CSData data){Array.Add(data);}
private CSDataArray(CSDataArray input)
		: base(input)
	{
		foreach (CSData item in input.Array)
		{
			Array.Add((item == null) ? new CSData() : item.Duplicate());
		}
	}
public bool IsIndexValid(int index)
	{
		if (index >= 0)
		{
			return index < Array.Count;
		}
		return false;
	}
public CSData GetArrayItem(int index)
	{
		if (!IsIndexValid(index))
		{
			return new CSData();
		}
		return Array[index];
	}
public CSDataArray GetFlattenArray()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSData item in Array.ToList())
		{
			if (item == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			if (!(item is ICSArray iCSArray))
			{
				cSDataArray.Append(item);
				continue;
			}
			CSDataArray flattenArray = iCSArray.GetFlattenArray();
			if (flattenArray == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			foreach (CSData item2 in flattenArray.Array.ToList())
			{
				cSDataArray.Append(item2);
			}
		}
		return cSDataArray;
	}
public override CSData Duplicate()
	{
		return new CSDataArray(this);
	}
}
internal static class CSDataHelper{
public static int GetArrayCount(CSData data)
	{
		if (data == null)
		{
			return 0;
		}
		return (data as ICSArray)?.GetArrayCount() ?? 0;
	}
public static CSData GetArrayItem(CSData data, int index)
	{
		if (data == null)
		{
			return new CSData();
		}
		if (!(data is ICSArray iCSArray))
		{
			return data;
		}
		if (index < 0 || index >= iCSArray.GetArrayCount())
		{
			return new CSData();
		}
		return iCSArray.GetArrayItem(index);
	}
}
internal sealed class Input{internal CSData Data;internal Input(CSData data){Data=data;}}
internal sealed class Output{internal CSData Data;internal void SetData(CSData data){Data=data;}}
internal class CSBlock{public virtual bool Update()=>true;}
internal sealed class CSAdd:CSBlock{
private readonly Input iStart,iValue;internal readonly Output oValue=new(),oNegated=new(),oOneMinus=new();
internal CSAdd(CSData first,CSData second){iStart=new(first);iValue=new(second);}
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
internal static class Values{
internal static CSData From(LegacyChannelValue value){if(value.IsMissing)return null;if(value.Array is{} items){var result=new CSDataArray();foreach(var child in items)result.Append(From(child));return result;}double n=0;return value.GetDouble(ref n)?new CSNumberData(n):new CSData(value);}
internal static LegacyChannelValue To(CSData value){if(value is null)return LegacyChannelValue.Missing;if(value is CSDataArray array)return LegacyChannelValue.FromArray(array.Array.Select(To));return value.Stored;}
}
