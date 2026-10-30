using System.Collections.Generic;
using System.Linq;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSObjects.CSBlocks;

public abstract class CSBlockExpandableInput : CSBlock
{
	private int _inputChannelIndexFirstExpansion = -1;

	protected abstract ChannelArrayType ExpandableInputChannelArrayType { get; }

	protected abstract ChannelDataType ExpandableInputChannelDataType { get; }

	protected abstract string ExpandableInputChannelIdentifier { get; }

	protected List<CSInputChannel> NonExpandableInputChannels => base.InputChannels.GetRange(0, base.InputChannels.Count - 1);

	protected CSBlockExpandableInput(string identifier)
		: base(identifier)
	{
	}

	public bool AddLink(CSOutputChannel outputChannel)
	{
		int num = CreateExpansionInputChannel();
		if (num < 0 || num >= InputChannelCount)
		{
			return false;
		}
		base.InputChannels[num].SetLink(outputChannel);
		return true;
	}

	public void CleanupInputChannels()
	{
		for (int num = InputChannelCount - 1; num > _inputChannelIndexFirstExpansion; num--)
		{
			CSInputChannel cSInputChannel = base.InputChannels[num];
			CSInputChannel cSInputChannel2 = base.InputChannels[num - 1];
			if (!cSInputChannel.HasLink && !cSInputChannel2.HasLink)
			{
				base.InputChannels.RemoveAt(num);
			}
			else if (cSInputChannel2.HasLink)
			{
				break;
			}
		}
	}

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

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		if (base.ReadJson(jBlock, graph, major, minor))
		{
			CreateExpansionInputChannel();
			return true;
		}
		CSUtil.Print($"ERROR: Reading object indices for {base.Identifier}\n");
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		if (base.Read(ref archive, graph, major, minor))
		{
			CreateExpansionInputChannel();
			return true;
		}
		CSUtil.Print($"ERROR: Reading object indices for {base.Identifier}\n");
		return false;
	}
}
