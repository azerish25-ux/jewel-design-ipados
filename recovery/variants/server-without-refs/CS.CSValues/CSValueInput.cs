using System;
using System.Globalization;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueInput", "{02DDB8FC-4D4D-4605-96DE-162245D788D7}")]
public sealed class CSValueInput : CSValueBase, ICSValue<string>
{
	private string _value;

	public CSInputChannel InOverride { get; }

	public CSOutputChannel OutValue { get; }

	public override bool UIChanged
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public string Value => _value;

	public CSValueInput(string identifier)
		: base(identifier)
	{
		InOverride = new CSInputChannel(this, ChannelDataType.Boolean | ChannelDataType.Integer | ChannelDataType.Double | ChannelDataType.String, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public CSValueInput(string identifier, string value)
		: base(identifier)
	{
		_value = value;
		InOverride = new CSInputChannel(this, ChannelDataType.Boolean | ChannelDataType.Integer | ChannelDataType.Double | ChannelDataType.String, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public void SetValueFromUI(string newValue, bool mouseDown)
	{
		if (mouseDown && newValue.Equals(_value, StringComparison.Ordinal))
		{
			return;
		}
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			PreChange("SetValueFromUI");
			SetValue(newValue);
			Change();
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public bool SetValue(string newValue)
	{
		if (newValue.Equals(_value, StringComparison.Ordinal))
		{
			return true;
		}
		PreChange("SetValue");
		_value = newValue;
		Change();
		return true;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= base.OutputChannels.Count)
		{
			return base.GetOutputChannel(outputChannelIndex);
		}
		if (base.OutputChannels[outputChannelIndex] != OutValue)
		{
			return base.GetOutputChannel(outputChannelIndex);
		}
		OutValue.SetData(new CSStringData(_value));
		return OutValue;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex < 0 || outputChannelIndex >= base.OutputChannels.Count)
		{
			return base.GetOutputChannelData(outputChannelIndex);
		}
		if (base.OutputChannels[outputChannelIndex] != OutValue)
		{
			return base.GetOutputChannelData(outputChannelIndex);
		}
		return new CSStringData(_value);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<string>("Value", out _value);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadString(ref _value);
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueInput)setObject)._value;
		}
		return true;
	}

	public void SetFromUI(string title, string value, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (title != base.Identifier || value != _value || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				SetValue(value);
				base.Identifier = title;
				Change();
				base.StudioVisible = studioVisible;
				base.StudioVisibleAdvanced = studioVisibleAdvanced;
				base.StudioVisibleDesignOption = studioVisibleDesignOption;
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate();
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
		if (!GetType().IsInstanceOfType(setObject))
		{
			return true;
		}
		string identifier = setObject.Identifier;
		if (identifier != null && identifier.Length > 0)
		{
			SetIdentifier(setObject.Identifier);
		}
		_value = ((CSValueInput)setObject)._value;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueInput)setObject)._value;
		}
		return true;
	}

	public override bool Update()
	{
		if (!InOverride.HasLink)
		{
			return base.Update();
		}
		bool value = false;
		if (InOverride.GetBoolean(ref value))
		{
			_value = value.ToString();
		}
		int value2 = 0;
		if (InOverride.GetInteger(ref value2))
		{
			_value = value2.ToString(CultureInfo.InvariantCulture);
		}
		double value3 = 0.0;
		if (InOverride.GetDouble(ref value3))
		{
			_value = value3.ToString(CultureInfo.InvariantCulture);
		}
		string output = string.Empty;
		if (InOverride.GetString(ref output))
		{
			_value = output;
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _value);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteString(_value);
		return true;
	}
}
