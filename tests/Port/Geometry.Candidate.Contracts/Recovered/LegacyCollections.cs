#nullable disable
// Exact source collection Update bodies with observation-only channels and immutable value conversion.
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks.Recovered.Collections;
internal enum ChannelDataType{Any}
internal enum ChannelArrayType{Any,Array,Individual}
internal class CSData{internal LegacyChannelValue Value;internal CSData(){Value=LegacyChannelValue.Empty;}internal CSData(LegacyChannelValue value){Value=value;}}
internal sealed class CSDataArray:CSData{internal readonly List<CSData> Array=new();internal void Append(CSData value){Array.Add(value);}}
internal class CSBlock
{
    internal string Identifier=>"Observed";internal readonly List<CSOutputChannel> OutputChannels=new();
    internal int OutputChannelCount=>OutputChannels.Count;
    internal CSData GetOutputChannelData(int index)=>index>=0&&index<OutputChannels.Count?OutputChannels[index].Data:new CSData();
    public virtual bool Update()=>true;
}
internal sealed class CSOutputChannel
{
    internal CSData Data=new();internal CSOutputChannel(string id,CSBlock parent,ChannelDataType type,ChannelArrayType array){parent.OutputChannels.Add(this);}
    internal void SetData(CSData data){Data=data;}
}
internal sealed class CSInputChannel{internal CSData Data;internal CSBlock OutputChannelBlock;internal int OutputChannelIndex;internal CSInputChannel(CSData data,CSBlock block,int channel){Data=data;OutputChannelBlock=block;OutputChannelIndex=channel;}}
internal class CSBlockExpandableInput:CSBlock{protected readonly List<CSInputChannel> NonExpandableInputChannels;internal CSBlockExpandableInput(IEnumerable<CSInputChannel> inputs){NonExpandableInputChannels=inputs.ToList();}}
internal sealed class CSArray:CSBlockExpandableInput
{
    internal readonly CSOutputChannel OutArray;
    internal CSArray(IEnumerable<CSInputChannel> inputs):base(inputs){OutArray=new("Out",this,ChannelDataType.Any,ChannelArrayType.Array);}
public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			if (nonExpandableInputChannel == null)
			{
				continue;
			}
			if (nonExpandableInputChannel.OutputChannelIndex == -1)
			{
				if (nonExpandableInputChannel.OutputChannelBlock != null)
				{
					for (int i = 0; i < nonExpandableInputChannel.OutputChannelBlock.OutputChannelCount; i++)
					{
						cSDataArray.Append(nonExpandableInputChannel.OutputChannelBlock.GetOutputChannelData(i));
					}
				}
			}
			else
			{
				cSDataArray.Append(nonExpandableInputChannel.Data);
			}
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}
}
internal sealed class CSUnarray1:CSBlockExpandableInput{internal CSUnarray1(IEnumerable<CSInputChannel> inputs):base(inputs){}
public override bool Update()
	{
		base.OutputChannels.Clear();
		CSDataArray data = new CSDataArray();
		int num = 0;
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			CSData data2 = nonExpandableInputChannel.Data;
			if (data2.GetType() == typeof(CSDataArray))
			{
				foreach (CSData item in ((CSDataArray)data2).Array)
				{
					new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(item);
					num++;
				}
			}
			else
			{
				new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(data2);
				num++;
			}
		}
		new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(data);
		return base.Update();
	}
}
internal readonly record struct ObservationInput(LegacyChannelValue Value,IReadOnlyList<LegacyChannelValue> Outputs,int Channel);
internal static class Values{
private static CSData From(LegacyChannelValue value){if(value.IsMissing)return null;if(value.Array is{} items){var a=new CSDataArray();foreach(var item in items)a.Append(From(item));return a;}return new(value);}
private static LegacyChannelValue To(CSData value)=>value is null?LegacyChannelValue.Missing:value is CSDataArray a?LegacyChannelValue.FromArray(a.Array.Select(To)):value.Value;
internal static LegacyChannelValue[] Run(bool unarray,IEnumerable<ObservationInput> inputs){var channels=new List<CSInputChannel>();foreach(var input in inputs){CSBlock producer=null;if(input.Outputs is not null){producer=new();foreach(var item in input.Outputs)new CSOutputChannel("",producer,ChannelDataType.Any,ChannelArrayType.Any).SetData(From(item));}channels.Add(new(From(input.Value),producer,input.Channel));}
CSBlock result=unarray?new CSUnarray1(channels):new CSArray(channels);if(!result.Update())throw new Exception("Source collection Update failed");return result.OutputChannels.Select(x=>To(x.Data)).ToArray();}
}
