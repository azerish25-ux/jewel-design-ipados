using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CSDatas;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Utilities.Helpers;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.DocObjects;

namespace CS.CSValues;

[CSBlock("CSValueFontList", "{44CCF831-902E-47FA-B7E3-C487F3DA05CA}")]
public sealed class CSValueFontList : CSValueBase, IFontSource
{
	private readonly List<GV.CounterSketch.Shared.Font> _unavailableFonts = new List<GV.CounterSketch.Shared.Font>();

	private readonly List<int> _unrecognizedFontValues = new List<int>();

	private bool _hasCreatedUndoTransaction;

	public CSOutputChannel OutListIndex { get; }

	public CSOutputChannel OutListValue { get; }

	public List<GV.CounterSketch.Shared.Font> List { get; } = new List<GV.CounterSketch.Shared.Font>();

	public int ListIndex { get; private set; } = -1;

	public GV.CounterSketch.Shared.Font? MissingRestoreFont { get; private set; }

	public IEnumerable<GV.CounterSketch.Shared.Font> UnavailableFonts => _unavailableFonts;

	public IEnumerable<int> UnrecognizedFontValues => _unrecognizedFontValues;

	public CSValueFontList(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutListIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public void AddToList(GV.CounterSketch.Shared.Font whichFont)
	{
		if (IsFontAvailable(whichFont))
		{
			List.Add(whichFont);
		}
	}

	public void CloneList(CSValueFontList originalListObject)
	{
		List.Clear();
		List.AddRange(originalListObject.List);
	}

	public int GetListLength()
	{
		return List.Count;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= base.OutputChannels.Count)
		{
			return base.GetOutputChannel(outputChannelIndex);
		}
		CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
		if (cSOutputChannel == OutListValue)
		{
			if (ListIndex < 0 || ListIndex >= List.Count)
			{
				return null;
			}
			OutListValue.SetData(new CSNumberData((double)List[ListIndex]));
			return OutListValue;
		}
		if (cSOutputChannel == OutListIndex)
		{
			OutListIndex.SetData(new CSNumberData(ListIndex));
			return OutListIndex;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutListValue)
			{
				if (ListIndex < 0 || ListIndex >= List.Count)
				{
					return null;
				}
				return new CSNumberData((double)List[ListIndex]);
			}
			if (cSOutputChannel == OutListIndex)
			{
				return new CSNumberData(ListIndex);
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(archive, graph, major, minor);
		_unavailableFonts.Clear();
		_unrecognizedFontValues.Clear();
		flag &= archive.ReadJsonValue<int>("Selected", out var value);
		if (archive.TryGetValue("Items", out var value2) && value2 is JArray)
		{
			foreach (JToken item in value2 as JArray)
			{
				int num = item.Value<int>();
				if (!Enum.IsDefined(typeof(GV.CounterSketch.Shared.Font), num))
				{
					_unrecognizedFontValues.Add(num);
					continue;
				}
				GV.CounterSketch.Shared.Font font = (GV.CounterSketch.Shared.Font)num;
				if (!IsFontAvailable(font))
				{
					_unavailableFonts.Add(font);
				}
				else
				{
					List.Add(font);
				}
			}
			GV.CounterSketch.Shared.Font font2 = (Enum.IsDefined(typeof(GV.CounterSketch.Shared.Font), value) ? ((GV.CounterSketch.Shared.Font)value) : GV.CounterSketch.Shared.Font.Unknown);
			if (List.Contains(font2))
			{
				ListIndex = List.IndexOf(font2);
				MissingRestoreFont = null;
			}
			else
			{
				ListIndex = ((List.Count <= 0) ? (-1) : 0);
				MissingRestoreFont = font2;
			}
			return flag;
		}
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_unavailableFonts.Clear();
		_unrecognizedFontValues.Clear();
		int index = 0;
		archive.ReadInt(ref index);
		int num = 0;
		if (!archive.ReadInt(ref num))
		{
			return false;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			int item = 0;
			archive.ReadInt(ref item);
			list.Add(item);
		}
		foreach (int item2 in list)
		{
			if (!Enum.IsDefined(typeof(GV.CounterSketch.Shared.Font), item2))
			{
				_unrecognizedFontValues.Add(item2);
				continue;
			}
			GV.CounterSketch.Shared.Font font = (GV.CounterSketch.Shared.Font)item2;
			if (!IsFontAvailable(font))
			{
				_unavailableFonts.Add(font);
			}
			else
			{
				List.Add(font);
			}
		}
		int num2 = list[index];
		GV.CounterSketch.Shared.Font font2 = (Enum.IsDefined(typeof(GV.CounterSketch.Shared.Font), num2) ? ((GV.CounterSketch.Shared.Font)num2) : GV.CounterSketch.Shared.Font.Unknown);
		if (List.Contains(font2))
		{
			ListIndex = List.IndexOf(font2);
			MissingRestoreFont = null;
		}
		else
		{
			ListIndex = ((List.Count <= 0) ? (-1) : 0);
			MissingRestoreFont = font2;
		}
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetStyle(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (setObject is CSValueFontList cSValueFontList)
		{
			ListIndex = cSValueFontList.ListIndex;
		}
		return true;
	}

	public void SetFromUI(string identifierName, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (identifierName != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				base.Identifier = identifierName;
				base.StudioVisible = studioVisible;
				base.StudioVisibleAdvanced = studioVisibleAdvanced;
				base.StudioVisibleDesignOption = studioVisibleDesignOption;
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public void SetListIndexFromUI(int newListIndex, bool mouseDown)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			if (!_hasCreatedUndoTransaction)
			{
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetListIndexFromUI");
				PreChange("SetListIndexFromUI");
				_hasCreatedUndoTransaction = true;
			}
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				SetListIndex(newListIndex);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetListIndex(newListIndex);
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetListIndexFromUI");
				_hasCreatedUndoTransaction = false;
			}
			CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
			CSUtil.Redraw();
			if (!mouseDown)
			{
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (setObject is CSValueFontList cSValueFontList)
		{
			ListIndex = cSValueFontList.ListIndex;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (setObject is CSValueFontList cSValueFontList)
		{
			ListIndex = cSValueFontList.ListIndex;
		}
		return true;
	}

	public bool SetValue(GV.CounterSketch.Shared.Font whichFont)
	{
		if (!List.Contains(whichFont))
		{
			return false;
		}
		int listIndex = List.IndexOf(whichFont);
		return SetListIndex(listIndex);
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		int num = (int)List[ListIndex];
		archive.Add("Selected", num);
		archive.Add("Items", JArray.FromObject(List));
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteInt(ListIndex);
		archive.WriteInt(List.Count);
		foreach (GV.CounterSketch.Shared.Font item in List)
		{
			archive.WriteInt((int)item);
		}
		return true;
	}

	protected override void MajorMinorVersion(out int majorVersion, out int minorVersion)
	{
		majorVersion = 3;
		minorVersion = 0;
	}

	private static bool IsFontAvailable(GV.CounterSketch.Shared.Font whichFont)
	{
		string enumDescriptionFromValue = EnumHelper.GetEnumDescriptionFromValue(whichFont);
		if (string.IsNullOrWhiteSpace(enumDescriptionFromValue))
		{
			return false;
		}
		return Rhino.DocObjects.Font.AvailableFontFaceNames().Contains(enumDescriptionFromValue);
	}

	private bool SetListIndex(int newListIndex)
	{
		if (newListIndex == ListIndex)
		{
			return true;
		}
		PreChange("SetListIndex");
		ListIndex = newListIndex;
		Change();
		return true;
	}
}
