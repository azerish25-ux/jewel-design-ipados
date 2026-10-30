#nullable disable
// Exact source curve-query overloads, with observed curve markers and channels; no geometry emulation.
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks.Recovered.CurveRouting;
internal sealed class NurbsCurve{internal LegacyChannelValue Value;internal NurbsCurve(LegacyChannelValue value){Value=value;}}
internal sealed class OnNurbsCurve{internal NurbsCurve Value;internal OnNurbsCurve(NurbsCurve value){Value=value;}}
internal static class ConvertCurve{internal static OnNurbsCurve Convert(this NurbsCurve value)=>value is null?null:new(value);}
internal interface ICSCurve{NurbsCurve GetNurbsCurve();}
internal interface ICSArray{int GetArrayCount();CSData GetArrayItem(int index);CSDataArray GetFlattenArray();}
internal class CSData{public virtual NurbsCurve GetNurbsCurve()=>null;}
internal sealed class Leaf:CSData,ICSCurve{private readonly LegacyChannelValue value;internal Leaf(LegacyChannelValue value){this.value=value;}public override NurbsCurve GetNurbsCurve()=>value.Curve.HasValue||value.Profile.HasValue?new(value):null;}
internal sealed class CSDataArray:CSData,ICSArray{internal readonly List<CSData> Array=new();internal CSDataArray(){}internal CSDataArray(CSData data){Array.Add(data);}internal void Append(CSData data){Array.Add(data);}public int GetArrayCount()=>Array.Count;public CSData GetArrayItem(int index)=>index>=0&&index<Array.Count?Array[index]:new CSData();
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
}
internal static class CSDataHelper{
public static CSDataArray GetFlattenArray(CSData data)
	{
		if (data == null)
		{
			return new CSDataArray();
		}
		if (data is ICSArray iCSArray)
		{
			return iCSArray.GetFlattenArray();
		}
		return new CSDataArray(data);
	}
public static bool GetNurbsCurve(CSData data, out NurbsCurve value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		NurbsCurve nurbsCurve = (flattenArray.GetArrayItem(0) as ICSCurve)?.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return false;
		}
		value = nurbsCurve;
		return true;
	}
}
internal sealed class CSOutputChannel{internal CSData Data;internal CSOutputChannel(CSData data){Data=data;}}
internal sealed class Block{private readonly CSData value;internal Block(CSData value){this.value=value;}internal CSOutputChannel GetOutputChannel(int index)=>index==0?new(value):null;internal CSData GetOutputChannelData(int index)=>index==0?value:new CSData();}
internal sealed class CSInputChannel{private readonly object _defaultValue=null;internal Block OutputChannelBlock;internal int OutputChannelIndex=0;internal CSInputChannel(CSData data){OutputChannelBlock=new(data);}
private CSData GetFirstData
	{
		get
		{
			if (OutputChannelBlock == null)
			{
				return null;
			}
			CSOutputChannel outputChannel = OutputChannelBlock.GetOutputChannel(OutputChannelIndex);
			if (outputChannel == null)
			{
				return null;
			}
			CSData cSData = outputChannel.Data;
			while (cSData is ICSArray)
			{
				ICSArray iCSArray = cSData as ICSArray;
				cSData = ((iCSArray.GetArrayCount() == 0) ? null : iCSArray.GetArrayItem(0));
			}
			return cSData;
		}
	}
public NurbsCurve NurbsCurve
	{
		get
		{
			NurbsCurve result = _defaultValue as NurbsCurve;
			CSData getFirstData = GetFirstData;
			if (getFirstData == null)
			{
				return result;
			}
			if (getFirstData is ICSCurve iCSCurve)
			{
				return iCSCurve.GetNurbsCurve();
			}
			return result;
		}
	}
public OnNurbsCurve GetNurbsCurve()
	{
		return (OutputChannelBlock?.GetOutputChannelData(OutputChannelIndex))?.GetNurbsCurve().Convert();
	}
}
internal static class Values{internal static CSData From(LegacyChannelValue value){if(value.IsMissing)return null;if(value.Array is{} items){var a=new CSDataArray();foreach(var child in items)a.Append(From(child));return a;}return new Leaf(value);}
internal static (LegacyChannelValue Property,LegacyChannelValue Method,LegacyChannelValue Helper) Observe(LegacyChannelValue value){var data=From(value);var input=new CSInputChannel(data);CSDataHelper.GetNurbsCurve(data,out var helper);return(input.NurbsCurve?.Value??LegacyChannelValue.Missing,input.GetNurbsCurve()?.Value.Value??LegacyChannelValue.Missing,helper?.Value??LegacyChannelValue.Missing);}
}
