using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Utilities.Helpers;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueHeadStyleList", "{A6250DF9-EDC8-42BE-A7D6-97A92C69780B}")]
public sealed class CSValueHeadStyleList : CSValueBase
{
	private sealed class HeadStyleSettings
	{
		private int _listIndex;

		public List<HeadStyleListItemIndexPair> List { get; } = new List<HeadStyleListItemIndexPair>();

		public int ListIndex
		{
			get
			{
				if (List.Count <= 0)
				{
					return -1;
				}
				return _listIndex;
			}
			set
			{
				if (value >= 0 && value < List.Count)
				{
					_listIndex = value;
				}
				else
				{
					_listIndex = -1;
				}
			}
		}

		public HeadStyleSettings(IEnumerable<HeadStyleListItemIndexPair> list, int selectedIndex = 0)
		{
			List.Clear();
			List.AddRange(list);
			if (List.Count <= 0)
			{
				_listIndex = -1;
			}
			else if (selectedIndex >= 0 && selectedIndex < List.Count)
			{
				_listIndex = selectedIndex;
			}
			else
			{
				_listIndex = 0;
			}
		}
	}

	private readonly Dictionary<string, HeadStyleSettings> _dictionary = new Dictionary<string, HeadStyleSettings>();

	private string _shape;

	private bool _hasCreatedUndoTransaction;

	public CSInputChannel InGem { get; }

	public CSOutputChannel OutListIndex { get; }

	public CSOutputChannel OutListValue { get; }

	public List<HeadStyleListItemIndexPair> List
	{
		get
		{
			HeadStyleSettings selectedHeadStyleSettings = SelectedHeadStyleSettings;
			if (selectedHeadStyleSettings != null)
			{
				return selectedHeadStyleSettings.List;
			}
			return new List<HeadStyleListItemIndexPair>();
		}
	}

	private HeadStyleSettings SelectedHeadStyleSettings
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_shape))
			{
				return null;
			}
			if (!_dictionary.ContainsKey(_shape))
			{
				return null;
			}
			return _dictionary[_shape];
		}
	}

	public int SelectedIndex
	{
		get
		{
			return SelectedHeadStyleSettings?.ListIndex ?? (-1);
		}
		private set
		{
			HeadStyleSettings selectedHeadStyleSettings = SelectedHeadStyleSettings;
			if (selectedHeadStyleSettings != null)
			{
				selectedHeadStyleSettings.ListIndex = value;
			}
		}
	}

	public CSValueHeadStyleList(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutListIndex = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public void CloneDictionary(CSValueHeadStyleList originalListObject)
	{
		_dictionary.Clear();
		foreach (string key in originalListObject._dictionary.Keys)
		{
			_dictionary.Add(key, originalListObject._dictionary[key]);
		}
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
			if (SelectedIndex < 0 || SelectedIndex >= List.Count)
			{
				return null;
			}
			OutListValue.SetData(new CSNumberData(List[SelectedIndex].Index));
			return OutListValue;
		}
		if (cSOutputChannel == OutListIndex)
		{
			OutListIndex.SetData(new CSNumberData(SelectedIndex));
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
				if (SelectedIndex < 0 || SelectedIndex >= List.Count)
				{
					return null;
				}
				return new CSNumberData(List[SelectedIndex].Index);
			}
			if (cSOutputChannel == OutListIndex)
			{
				return new CSNumberData(SelectedIndex);
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public void MoveUIListLeft(int whichItem)
	{
		MoveUIList(whichItem, doReverse: true);
	}

	public void MoveUIListRight(int whichItem)
	{
		MoveUIList(whichItem, doReverse: false);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = true;
		flag &= base.Read(ref archive, graph, major, minor);
		_dictionary.Clear();
		int i = 0;
		if (!archive.ReadInt(ref i))
		{
			return false;
		}
		for (int j = 0; j < i; j++)
		{
			string str = null;
			flag &= archive.ReadString(ref str);
			int i2 = 0;
			flag &= archive.ReadInt(ref i2);
			int i3 = 0;
			flag &= archive.ReadInt(ref i3);
			List<HeadStyleListItemIndexPair> list = new List<HeadStyleListItemIndexPair>();
			for (int k = 0; k < i3; k++)
			{
				int i4 = 0;
				flag &= archive.ReadInt(ref i4);
				bool b = false;
				flag &= archive.ReadBool(ref b);
				int i5 = 0;
				flag &= archive.ReadInt(ref i5);
				list.Add(new HeadStyleListItemIndexPair(i4, b, (HeadStyle)i5));
			}
			_dictionary.Add(str, new HeadStyleSettings(list, i2));
		}
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(jBlock, graph, major, minor);
		_dictionary.Clear();
		foreach (JObject item in (JArray)jBlock["HeadStyles"])
		{
			flag &= item.ReadJsonValue<string>("Shape", out var value);
			flag &= item.ReadJsonValue<int>("listIndex", out var value2);
			List<HeadStyleListItemIndexPair> list = new List<HeadStyleListItemIndexPair>();
			foreach (JObject item2 in (JArray)item["HeadStylesList"])
			{
				flag &= item2.ReadJsonValue<int>("listIndex", out var value3);
				flag &= item2.ReadJsonValue<bool>("UiDisabled", out var value4);
				flag &= item2.ReadJsonValue<int>("HeadStyleListItem", out var value5);
				list.Add(new HeadStyleListItemIndexPair(value3, value4, (HeadStyle)value5));
			}
			_dictionary.Add(value, new HeadStyleSettings(list, value2));
		}
		return flag;
	}

	public void RemoveFromUIList(int whichItem)
	{
		HeadStyleListItemIndexPair headStyleListItemIndexPair = List.FirstOrDefault((HeadStyleListItemIndexPair l) => l.Index == whichItem);
		if (headStyleListItemIndexPair != null)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("RemoveFromUIList");
			headStyleListItemIndexPair.UIDisabled = true;
			int initialSelectedIndex = List.IndexOf(headStyleListItemIndexPair);
			if (!GetAdjustedValidIndex(initialSelectedIndex, doReverse: true, out var newSelectedIndex) && !GetAdjustedValidIndex(initialSelectedIndex, doReverse: false, out newSelectedIndex))
			{
				newSelectedIndex = -1;
			}
			SetListIndex(newSelectedIndex);
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("RemoveFromUIList");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void ResetUIList()
	{
		if (_dictionary.ContainsKey(_shape))
		{
			_dictionary.Remove(_shape);
		}
		PopulateDictionaryFromShape();
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetStyle(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (setObject is CSValueHeadStyleList cSValueHeadStyleList)
		{
			SelectedIndex = cSValueHeadStyleList.SelectedIndex;
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

	public void SetListIndexFromUI(int value, bool mouseDown)
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
				SetListIndex(value);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetListIndex(value);
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
		if (setObject is CSValueHeadStyleList cSValueHeadStyleList)
		{
			SelectedIndex = cSValueHeadStyleList.SelectedIndex;
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
		if (setObject is CSValueHeadStyleList cSValueHeadStyleList)
		{
			SelectedIndex = cSValueHeadStyleList.SelectedIndex;
		}
		return true;
	}

	public override bool Update()
	{
		CSData data = InGem.Data;
		if (data == null)
		{
			return UpdateMissing("Gem");
		}
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSData arrayItem = flattenArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Gem");
		}
		if (!(arrayItem is CSGemData { GemShape: var gemShape }))
		{
			return UpdateMissing("Gem");
		}
		if (string.IsNullOrWhiteSpace(gemShape))
		{
			return UpdateError("Invalid Gem Shape");
		}
		if (!string.IsNullOrWhiteSpace(_shape) && _shape.Equals(gemShape, StringComparison.Ordinal))
		{
			return base.Update();
		}
		int selectedIndex = SelectedIndex;
		_shape = gemShape;
		PopulateDictionaryFromShape();
		if (selectedIndex < 0 || selectedIndex >= List.Count)
		{
			return base.Update();
		}
		if (List[selectedIndex].UIDisabled)
		{
			return base.Update();
		}
		SelectedIndex = selectedIndex;
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		bool flag = true;
		flag &= base.Write(ref archive);
		flag &= archive.WriteInt(_dictionary.Keys.Count);
		foreach (string key in _dictionary.Keys)
		{
			flag &= archive.WriteString(key);
			flag &= archive.WriteInt(_dictionary[key].ListIndex);
			flag &= archive.WriteInt(_dictionary[key].List.Count);
			foreach (HeadStyleListItemIndexPair item in _dictionary[key].List)
			{
				flag &= archive.WriteInt(item.Index);
				flag &= archive.WriteBool(item.UIDisabled);
				flag &= archive.WriteInt((int)item.HeadStyleListItem);
			}
		}
		return flag;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		foreach (string key in _dictionary.Keys)
		{
			JObject jObject = new JObject
			{
				{ "Shape", key },
				{
					"listIndex",
					_dictionary[key].ListIndex
				}
			};
			JArray jArray2 = new JArray();
			foreach (HeadStyleListItemIndexPair item2 in _dictionary[key].List)
			{
				JObject item = new JObject
				{
					{ "listIndex", item2.Index },
					{ "UiDisabled", item2.UIDisabled },
					{
						"HeadStyleListItem",
						(int)item2.HeadStyleListItem
					}
				};
				jArray2.Add(item);
			}
			jObject.Add("HeadStylesList", jArray2);
			jArray.Add(jObject);
		}
		archive.Add("HeadStyles", jArray);
	}

	private bool GetAdjustedValidIndex(int initialSelectedIndex, bool doReverse, out int newSelectedIndex)
	{
		newSelectedIndex = -1;
		int num = ((!doReverse) ? 1 : (-1));
		for (int i = initialSelectedIndex + num; i >= 0 && i < List.Count; i += num)
		{
			if (!List[i].UIDisabled)
			{
				newSelectedIndex = i;
				return true;
			}
		}
		return false;
	}

	private void MoveUIList(int whichItem, bool doReverse)
	{
		HeadStyleListItemIndexPair headStyleListItemIndexPair = List.FirstOrDefault((HeadStyleListItemIndexPair l) => l.Index == whichItem);
		if (headStyleListItemIndexPair == null)
		{
			return;
		}
		int num = List.IndexOf(headStyleListItemIndexPair);
		if (doReverse)
		{
			if (num == 0)
			{
				return;
			}
		}
		else if (num == List.Count - 1)
		{
			return;
		}
		if (GetAdjustedValidIndex(num, doReverse, out var newSelectedIndex))
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("MoveUIList");
			List.RemoveAt(num);
			List.Insert(newSelectedIndex, headStyleListItemIndexPair);
			SetListIndex(newSelectedIndex);
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("MoveUIList");
			CSApplicationHelper.Application.Document.Graph.Generate();
		}
	}

	private void PopulateDictionaryFromShape()
	{
		if (!string.IsNullOrWhiteSpace(_shape) && !_dictionary.ContainsKey(_shape))
		{
			List<HeadStyleListItemIndexPair> list = (from headStyle in HeadStyleHelper.GetHeadStyles(_shape)
				select new HeadStyleListItemIndexPair(headStyle)).ToList();
			_dictionary.Add(_shape, new HeadStyleSettings(list));
		}
	}

	private bool SetListIndex(int newListIndex)
	{
		if (SelectedIndex == newListIndex)
		{
			return true;
		}
		PreChange("SetListIndex");
		SelectedIndex = newListIndex;
		Change();
		return true;
	}
}
