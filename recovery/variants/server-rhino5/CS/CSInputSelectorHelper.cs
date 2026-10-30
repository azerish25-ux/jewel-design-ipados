using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;

namespace CS;

public static class CSInputSelectorHelper
{
	public static bool SkipInputSelection { get; set; }

	public static List<CSInputChannel> GetAllPossibleInputs(bool showHiddenBlocks, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter)
	{
		List<CSInputChannel> list = new List<CSInputChannel>();
		foreach (CSObject graph in CSApplicationHelper.Application.Document.Graph.GraphList)
		{
			if (!(graph is CSBlock { Deleted: false } cSBlock) || (!showHiddenBlocks && !cSBlock.UIVisible))
			{
				continue;
			}
			int inputChannelCount = cSBlock.InputChannelCount;
			for (int i = 0; i < inputChannelCount; i++)
			{
				CSInputChannel inputChannel = cSBlock.GetInputChannel(i);
				if (inputChannel != null && CSChannelHelper.IsLinkValid(inputChannel, arrayTypeFilter, dataTypeFilter))
				{
					list.Add(cSBlock.InputChannels[i]);
				}
			}
		}
		return list;
	}

	public static List<CSOutputChannel> GetAllPossibleOutputs(bool showHiddenBlocks, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter)
	{
		List<CSOutputChannel> list = new List<CSOutputChannel>();
		foreach (CSObject graph in CSApplicationHelper.Application.Document.Graph.GraphList)
		{
			if (!(graph is CSBlock { Deleted: false } cSBlock) || (!showHiddenBlocks && !cSBlock.UIVisible))
			{
				continue;
			}
			int outputChannelCount = cSBlock.OutputChannelCount;
			for (int i = 0; i < outputChannelCount; i++)
			{
				CSOutputChannel outputChannel = cSBlock.GetOutputChannel(i);
				if (outputChannel != null && CSChannelHelper.IsLinkValid(arrayTypeFilter, dataTypeFilter, outputChannel))
				{
					list.Add(cSBlock.OutputChannels[i]);
				}
			}
		}
		return list;
	}

	public static CSBlock GetBlockFromPrompt(string prompt)
	{
		if (CSApplicationHelper.Application.InputChoosers == null)
		{
			return null;
		}
		return CSApplicationHelper.Application.InputChoosers.GetBlockFromPrompt(prompt);
	}

	public static string GetOutputDataTypeText(CSData data)
	{
		string text = "Nothing";
		if (CSDataHelper.GetDominantChannelDataType(data, out var channelDataType))
		{
			switch (channelDataType)
			{
			case ChannelDataType.Gem:
				text = "Gem";
				break;
			case ChannelDataType.Curve:
				text = "Curve";
				break;
			case ChannelDataType.Profile:
				text = "Profile";
				break;
			case ChannelDataType.Mesh:
				text = "Mesh";
				break;
			case ChannelDataType.Brep:
				text = "Brep";
				break;
			case ChannelDataType.Surface:
				text = "Surface";
				break;
			case ChannelDataType.RingRail:
				text = "Ring Rail";
				break;
			case ChannelDataType.RingRailPlanar:
				text = "Planar Ring Rail";
				break;
			case ChannelDataType.OutsideRingRail:
				text = "Outside Ring Rail";
				break;
			case ChannelDataType.OutsideRingRailPlanar:
				text = "Planar Outside Ring Rail";
				break;
			case ChannelDataType.Boolean:
			case ChannelDataType.Integer:
			case ChannelDataType.Double:
				text = "Value";
				break;
			case ChannelDataType.String:
				text = "String";
				break;
			case ChannelDataType.Point:
				text = "Point";
				break;
			case ChannelDataType.Vector:
				text = "Vector";
				break;
			case ChannelDataType.Transform:
				text = "Transformation Matrix";
				break;
			case ChannelDataType.BrepEdge:
				text = "Edge";
				break;
			default:
				text = "Miscellaneous";
				break;
			}
		}
		if (!(data is ICSArray csArray))
		{
			return text;
		}
		string arrayCountText = GetArrayCountText(csArray);
		if (!string.IsNullOrEmpty(arrayCountText))
		{
			return $"{text} Array{arrayCountText}";
		}
		return text;
	}

	public static bool GetOutputFromPrompt(string prompt, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter, out CSOutputChannel output, bool showAll = false, bool autoSelect = false, bool autoHiddenBlocks = true)
	{
		output = null;
		if (SkipInputSelection)
		{
			return true;
		}
		if (CSApplicationHelper.Application.InputChoosers == null)
		{
			return false;
		}
		CSOutputChannel outputFromPrompt = CSApplicationHelper.Application.InputChoosers.GetOutputFromPrompt(prompt, arrayTypeFilter, dataTypeFilter, showAll, autoSelect, autoHiddenBlocks);
		if (outputFromPrompt == null)
		{
			return false;
		}
		if (outputFromPrompt.Parent == null)
		{
			return false;
		}
		output = outputFromPrompt;
		return true;
	}

	public static bool GetOutputsFromPrompt(string prompt, CSInputChannel[] inputChannels, out CSOutputChannel output, bool showAll = false, bool autoSelect = false, bool autoHiddenBlocks = true)
	{
		CSChannelHelper.GetChannelFilters(out var channelArrayTypeFilter, out var channelDataTypeFilter, inputChannels);
		return GetOutputFromPrompt(prompt, channelArrayTypeFilter, channelDataTypeFilter, out output);
	}

	public static bool GetOutputsFromPrompt(string prompt, ChannelArrayType arrayTypeFilter, ChannelDataType dataTypeFilter, out List<CSOutputChannel> outputs)
	{
		outputs = null;
		if (SkipInputSelection)
		{
			outputs = new List<CSOutputChannel>();
			return true;
		}
		if (CSApplicationHelper.Application.InputChoosers == null)
		{
			return false;
		}
		Collection<CSOutputChannel> outputsFromPrompt = CSApplicationHelper.Application.InputChoosers.GetOutputsFromPrompt(prompt, arrayTypeFilter, dataTypeFilter);
		if (outputsFromPrompt == null)
		{
			return false;
		}
		outputs = (from initialOutput in outputsFromPrompt
			where initialOutput != null
			where initialOutput.Parent != null
			select initialOutput).ToList();
		return outputs.Count > 0;
	}

	public static bool GetOutputsFromPrompt(string prompt, CSInputChannel[] inputChannels, out List<CSOutputChannel> outputs)
	{
		CSChannelHelper.GetChannelFilters(out var channelArrayTypeFilter, out var channelDataTypeFilter, inputChannels);
		return GetOutputsFromPrompt(prompt, channelArrayTypeFilter, channelDataTypeFilter, out outputs);
	}

	public static CSObject GetStartFromPrompt(string prompt)
	{
		if (CSApplicationHelper.Application.InputChoosers == null)
		{
			return null;
		}
		return CSApplicationHelper.Application.InputChoosers.GetStartFromPrompt(prompt);
	}

	private static string GetArrayCountText(ICSArray csArray)
	{
		if (csArray == null)
		{
			return string.Empty;
		}
		double num = csArray.GetArrayCount();
		if (num <= 0.0)
		{
			return $"[{num}]";
		}
		string text = $"[{num}]";
		int num2 = 0;
		for (int i = 0; (double)i < num; i++)
		{
			int num3;
			if (!(csArray.GetArrayItem(i) is ICSArray iCSArray))
			{
				num3 = 0;
			}
			else
			{
				num3 = iCSArray.GetArrayCount();
				text = $"{text}({GetArrayCountText(iCSArray)})";
			}
			if (i == 0)
			{
				num2 = num3;
			}
			else if (num2 != num3)
			{
				return "Mixed";
			}
		}
		return text;
	}
}
