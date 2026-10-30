using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueList", "{628241B6-6F65-41CB-AE7E-97AB6BA5B569}")]
public sealed class CSValueList : CSValueBase
{
	private int _listIndex;

	private bool _isTextOnly;

	private bool _hasCreatedUndoTransaction;

	public CSInputChannel InGem { get; }

	public CSOutputChannel OutListIndex { get; }

	public CSOutputChannel OutListValue { get; }

	public int ListIndex => _listIndex;

	public bool IsTextOnly => _isTextOnly;

	public List<ListItemIndexPair> List { get; } = new List<ListItemIndexPair>();

	public CSValueList(string identifier, bool isTextOnly)
		: base(identifier)
	{
		_isTextOnly = isTextOnly;
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutListIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public CSValueList(string identifier)
		: this(identifier, isTextOnly: false)
	{
	}

	public void AddToList(ValueListItem whichValueListItem)
	{
		AddToList(whichValueListItem, (int)whichValueListItem);
	}

	public void AddToList(ValueListItem whichValueListItem, int index)
	{
		List.Add(new ListItemIndexPair(index, whichValueListItem));
	}

	public void CloneList(CSValueList originalListObject)
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
			if (_listIndex < 0 || _listIndex >= List.Count)
			{
				return null;
			}
			OutListValue.SetData(new CSNumberData(List[_listIndex].Index));
			return OutListValue;
		}
		if (cSOutputChannel == OutListIndex)
		{
			OutListIndex.SetData(new CSNumberData(_listIndex));
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
				if (_listIndex < 0 || _listIndex >= List.Count)
				{
					return null;
				}
				return new CSNumberData(List[_listIndex].Index);
			}
			if (cSOutputChannel == OutListIndex)
			{
				return new CSNumberData(_listIndex);
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		List<ListItemIndexPair> value;
		bool result = base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<int>("listIndex", out _listIndex) & jBlock.ReadJsonValue<bool>("textOnly", out _isTextOnly) & jBlock.ReadJsonObject<List<ListItemIndexPair>>("ValueList", out value);
		List.Clear();
		List.AddRange(value);
		return result;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadInt(ref _listIndex);
		archive.ReadBool(ref _isTextOnly);
		List.Clear();
		int num = 0;
		if (!archive.ReadInt(ref num))
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			int index = 0;
			archive.ReadInt(ref index);
			int whichValueListItem = 0;
			archive.ReadInt(ref whichValueListItem);
			switch (major)
			{
			case 1:
			case 2:
				List.Add(new ListItemIndexPair(index, (ValueListItem)whichValueListItem));
				break;
			case 3:
			{
				bool isUIDisabled = false;
				archive.ReadBool(ref isUIDisabled);
				List.Add(new ListItemIndexPair(index, isUIDisabled, (ValueListItem)whichValueListItem));
				break;
			}
			}
		}
		return true;
	}

	public void RemoveFromUIList(int whichItem)
	{
		ListItemIndexPair listItemIndexPair = List.FirstOrDefault((ListItemIndexPair l) => l.Index == whichItem);
		if (listItemIndexPair == null)
		{
			return;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("RemoveFromUIList");
		listItemIndexPair.UIDisabled = true;
		int num = List.IndexOf(listItemIndexPair);
		int listIndex = -1;
		bool flag = false;
		int num2 = num;
		while (num2 > 0 && !flag)
		{
			num2--;
			if (!List[num2].UIDisabled)
			{
				listIndex = num2;
				flag = true;
			}
		}
		int num3 = num;
		while (num3 < List.Count - 1 && !flag)
		{
			num3++;
			if (!List[num3].UIDisabled)
			{
				listIndex = num3;
				flag = true;
			}
		}
		SetListIndex(listIndex);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("RemoveFromUIList");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void ResetUIList()
	{
		List.ForEach(delegate(ListItemIndexPair l)
		{
			l.UIDisabled = false;
		});
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetStyle(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (setObject is CSValueList cSValueList)
		{
			_listIndex = cSValueList._listIndex;
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
		if (setObject is CSValueList cSValueList)
		{
			_listIndex = cSValueList._listIndex;
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
		if (setObject is CSValueList cSValueList)
		{
			_listIndex = cSValueList._listIndex;
			_isTextOnly = cSValueList.IsTextOnly;
		}
		return true;
	}

	public bool SetListValue(ValueListItem whichValueListItem)
	{
		ListItemIndexPair listItemIndexPair = List.FirstOrDefault((ListItemIndexPair item) => item.Index == (int)whichValueListItem && item.ValueListItem == whichValueListItem);
		if (listItemIndexPair == null)
		{
			return false;
		}
		int listIndex = List.IndexOf(listItemIndexPair);
		return SetListIndex(listIndex);
	}

	public bool SetListIndex(int newListIndex)
	{
		if (newListIndex == _listIndex)
		{
			return true;
		}
		PreChange("SetListIndex");
		_listIndex = newListIndex;
		Change();
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("listIndex", _listIndex);
		archive.Add("textOnly", _isTextOnly);
		archive.Add("ValueList", JArray.FromObject(List));
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteInt(_listIndex);
		archive.WriteBool(_isTextOnly);
		archive.WriteInt(List.Count);
		foreach (ListItemIndexPair item in List)
		{
			archive.WriteInt(item.Index);
			archive.WriteInt((int)item.ValueListItem);
			archive.WriteBool(item.UIDisabled);
		}
		return true;
	}

	protected override void MajorMinorVersion(out int majorVersion, out int minorVersion)
	{
		majorVersion = 3;
		minorVersion = 0;
	}
}
