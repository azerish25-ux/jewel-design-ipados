#nullable disable
// Exact source socket creation/assignment bodies with observation-only graph/channel constructors.
using JewelDesign.Graph;
namespace CandidateGeometryContractChecks.Recovered.Consumer;
internal enum ChannelDataType{Any,Transform}
internal enum ChannelArrayType{Any}
internal enum ChannelFlag{NotRequired}
internal class CSObject{internal string Identifier=>"Observed";internal int Index;}
internal static class CSApplicationHelper{internal sealed class App{internal bool InImport;internal int ImportOffset;}internal static readonly App Application=new();}
internal sealed class CSParaGraph{internal readonly List<CSObject> GraphList=new();internal CSParaGraph(int count){for(int i=0;i<count;i++)GraphList.Add(new CSBlock(i));}}
internal static class CSUtil{internal static void Print(string text)=>throw new Exception(text);}
internal class CSBlock:CSObject
{
    internal readonly List<CSInputChannel> InputChannels=new();
    internal int InputChannelCount=>InputChannels.Count;
    internal CSBlock(int index=-1){Index=index;}
    internal void Replay(int[] items,int[] items2,CSParaGraph graph)
    {
for (int i = 0; i < items.Length; i++)
		{
			CSBlock outputChannelBlock = null;
			int outputChannelIndex = -1;
			int num = items[i];
			if (num >= 0)
			{
				if (CSApplicationHelper.Application.InImport)
				{
					num += CSApplicationHelper.Application.ImportOffset;
				}
				if (num >= 0 && num < graph.GraphList.Count)
				{
					outputChannelBlock = (CSBlock)graph.GraphList[num];
					outputChannelIndex = items2[i];
				}
			}
			if (i < InputChannelCount)
			{
				InputChannels[i].OutputChannelBlock = outputChannelBlock;
				InputChannels[i].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSBlockExpandableInput cSBlockExpandableInput)
			{
				int index = cSBlockExpandableInput.CreateExpansionInputChannel();
				InputChannels[index].OutputChannelBlock = outputChannelBlock;
				InputChannels[index].OutputChannelIndex = outputChannelIndex;
			}
			else if (this is CSGroupStartObsolete || this is CSGroupEndObsolete)
			{
				CSInputChannel cSInputChannel = new CSInputChannel(outputChannelBlock, outputChannelIndex);
				cSInputChannel.SetParent(this);
				InputChannels.Add(cSInputChannel);
			}
			else
			{
				CSUtil.Print($"ERROR: Too many input channels for {base.Identifier}\n");
			}
		}
}
}
internal class CSGroupStartObsolete:CSBlock{}
internal class CSGroupEndObsolete:CSBlock{}
internal sealed class CSInputChannel
{
    internal CSBlock OutputChannelBlock;internal int OutputChannelIndex=-1;
    internal CSInputChannel(string identifier,CSBlock parent,ChannelDataType data,ChannelArrayType array,ChannelFlag flag){parent.InputChannels.Add(this);}
    internal CSInputChannel(CSBlock parent){parent.InputChannels.Add(this);}
    internal CSInputChannel(CSBlock block,int index){OutputChannelBlock=block;OutputChannelIndex=index;}
    internal void SetParent(CSBlock parent){parent.InputChannels.Add(this);}
public bool HasLink
	{
		get
		{
			if (OutputChannelBlock != null)
			{
				return OutputChannelIndex >= 0;
			}
			return false;
		}
	}
}
internal sealed class CSBlockExpandableInput:CSBlock
{
private int _inputChannelIndexFirstExpansion=-1;
private string ExpandableInputChannelIdentifier=>"InTransformation";
private ChannelDataType ExpandableInputChannelDataType=>ChannelDataType.Transform;
private ChannelArrayType ExpandableInputChannelArrayType=>ChannelArrayType.Any;
internal CSBlockExpandableInput(int fixedInputCount=1){for(int i=0;i<fixedInputCount;i++)new CSInputChannel(this);CreateExpansionInputChannel();}
public int CreateExpansionInputChannel()
	{
		if (_inputChannelIndexFirstExpansion < 0)
		{
			return CreateExpansionInputChannelAux();
		}
		int num = InputChannelCount - 1;
		if (num < _inputChannelIndexFirstExpansion)
		{
			int num2 = _inputChannelIndexFirstExpansion - num;
			for (int i = 0; i < num2; i++)
			{
				new CSInputChannel(string.Format("{0}_{1}", "InRecovered", InputChannelCount), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
			}
		}
		if (base.InputChannels.Last().HasLink)
		{
			return CreateExpansionInputChannelAux();
		}
		return InputChannelCount - 1;
	}
private int CreateExpansionInputChannelAux()
	{
		if (_inputChannelIndexFirstExpansion < 0)
		{
			_inputChannelIndexFirstExpansion = InputChannelCount;
		}
		int num = InputChannelCount - _inputChannelIndexFirstExpansion;
		new CSInputChannel($"{ExpandableInputChannelIdentifier}_{num}", this, ExpandableInputChannelDataType, ExpandableInputChannelArrayType, ChannelFlag.NotRequired);
		return InputChannelCount - 1;
	}
internal static SerializedChannelLink[] Observe(IReadOnlyList<SerializedChannelLink> links,int count,int offset,int fixedInputCount=1){
CSApplicationHelper.Application.InImport=offset!=0;CSApplicationHelper.Application.ImportOffset=offset;
var graph=new CSParaGraph(count);var block=new CSBlockExpandableInput(fixedInputCount);block.Replay(links.Select(x=>x.ObjectIndex).ToArray(),links.Select(x=>x.ChannelIndex).ToArray(),graph);block.CreateExpansionInputChannel();
return block.InputChannels.Select(x=>new SerializedChannelLink(x.OutputChannelBlock?.Index??-1,x.OutputChannelIndex)).ToArray();
}
}
