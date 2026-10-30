using System;
using System.Collections.Generic;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues.Obsolete;

[Obsolete]
[CSBlock("ValueDropDown", "{0AC20EF7-4AE1-45bb-BFDA-343C7FD698B4}")]
public sealed class CSValueDropDown : CSValue
{
	private List<string> _listNames = new List<string>();

	private List<string> _listOutputStrings = new List<string>();

	private bool _hasCreatedUndoTransaction;

	public CSOutputChannel OutSelectedName { get; }

	public CSOutputChannel OutSelectedOutput { get; }

	public void AddToList(string name, string output)
	{
		if (name != null)
		{
			_listNames.Add(name);
			if (output == null)
			{
				output = string.Empty;
			}
			_listOutputStrings.Add(output);
		}
	}

	public string GetDropName(double value)
	{
		if (value >= 0.0 && value < (double)_listNames.Count)
		{
			return _listNames[(int)value];
		}
		return null;
	}

	public List<string> GetDropNamesList()
	{
		int i = 0;
		List<string> list = new List<string>();
		for (; i < _listNames.Count; i++)
		{
			list.Add(_listNames[i]);
		}
		return list;
	}

	public string GetDropOutput(double value)
	{
		if (value >= 0.0 && value < (double)_listOutputStrings.Count)
		{
			return _listOutputStrings[(int)value];
		}
		return null;
	}

	public List<string> GetDropOutputList()
	{
		int i = 0;
		List<string> list = new List<string>();
		for (; i < _listOutputStrings.Count; i++)
		{
			list.Add(_listNames[i]);
		}
		return list;
	}

	public void SetFromUI(double value, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (Math.Abs(value - base.Value) > 0.005 || identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_previousValue = value;
				_doubleValue = value;
				SetIdentifier(identifier);
				Change();
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
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public void SetList(string[] newNames, string[] newOutput)
	{
		_listNames = ((newNames == null) ? new List<string>() : new List<string>(newNames));
		_listOutputStrings = ((newOutput == null) ? new List<string>() : new List<string>(newOutput));
		int num = _listNames.Count - _listOutputStrings.Count;
		for (int i = 0; i < num; i++)
		{
			_listOutputStrings.Add("");
		}
	}

	public void SetListIndexFromUI(double value, bool mouseDown)
	{
		if (mouseDown && base.Value == value)
		{
			return;
		}
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
				SetValue(value);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetValue(value);
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
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public CSValueDropDown(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutSelectedName = new CSOutputChannel(GetPropertyName(() => OutSelectedName), this, ChannelDataType.String, ChannelArrayType.Individual);
		OutSelectedOutput = new CSOutputChannel(GetPropertyName(() => OutSelectedOutput), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public CSValueDropDown(string identifier, double inputValue)
		: base(identifier)
	{
		_previousValue = inputValue;
		_doubleValue = inputValue;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutSelectedName = new CSOutputChannel(GetPropertyName(() => OutSelectedName), this, ChannelDataType.String, ChannelArrayType.Individual);
		OutSelectedOutput = new CSOutputChannel(GetPropertyName(() => OutSelectedOutput), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutSelectedName)
			{
				OutSelectedName.SetData(new CSStringData(GetDropName(base.Value)));
				return OutSelectedName;
			}
			if (cSOutputChannel == OutSelectedOutput)
			{
				OutSelectedOutput.SetData(new CSStringData(GetDropOutput(base.Value)));
				return OutSelectedOutput;
			}
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutSelectedName)
			{
				return new CSStringData(GetDropName(base.Value));
			}
			if (cSOutputChannel == OutSelectedOutput)
			{
				return new CSStringData(GetDropOutput(base.Value));
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<List<string>>("Name", out _listNames) & jBlock.ReadJsonValue<List<string>>("string", out _listOutputStrings);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		string[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			_listNames = new List<string>();
		}
		_listNames = new List<string>(items);
		items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			_listOutputStrings = new List<string>();
		}
		_listOutputStrings = new List<string>(items);
		int num = _listNames.Count - _listOutputStrings.Count;
		for (int i = 0; i < num; i++)
		{
			_listOutputStrings.Add("");
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		_ = base.NotCreatedYet;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		_ = base.NotCreatedYet;
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Name", JArray.FromObject(_listNames));
		archive.Add("string", JArray.FromObject(_listOutputStrings));
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		CSUtilIO.WriteArray(_listNames.ToArray(), ref archive);
		CSUtilIO.WriteArray(_listOutputStrings.ToArray(), ref archive);
		return true;
	}
}
