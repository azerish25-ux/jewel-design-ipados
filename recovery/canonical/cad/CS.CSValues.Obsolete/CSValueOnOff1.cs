using System;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues.Obsolete;

[Obsolete]
[CSBlock("ValueOnOff1", "{52A6C355-9677-4404-8B1E-FBC104ED89BE}")]
public sealed class CSValueOnOff1 : CSValueBase, ICSValue<double>
{
	private bool _value;

	private bool _hasCreatedUndoTransaction;

	public bool Value => _value;

	public CSInputChannel InOverride { get; }

	public CSOutputChannel OutValue { get; }

	double ICSValue<double>.Value
	{
		get
		{
			if (!_value)
			{
				return 0.0;
			}
			return 1.0;
		}
	}

	public CSValueOnOff1(string identifier)
		: base(identifier)
	{
		_value = true;
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public CSValueOnOff1(string identifier, bool onOffvalue)
		: base(identifier)
	{
		_value = onOffvalue;
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public bool SetValue(double newValue)
	{
		return SetValue(newValue > 0.5);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<bool>("Value", out _value);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadBool(ref _value);
		return true;
	}

	public bool SetValue(bool newValue)
	{
		if (newValue == _value)
		{
			return true;
		}
		PreChange("SetValue");
		_value = newValue;
		Change();
		return true;
	}

	public void SetValueFromUI(bool value, bool mouseDown)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			if (!_hasCreatedUndoTransaction)
			{
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
				PreChange("SetValueFromUI");
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
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
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

	public void SetValueFromUI(bool value, string text, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			PreChange("SetValueFromUI");
			SetValue(value);
			SetIdentifier(text);
			Change();
			base.StudioVisible = studioVisible;
			base.StudioVisibleAdvanced = studioVisibleAdvanced;
			base.StudioVisibleDesignOption = studioVisibleDesignOption;
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public override bool Update()
	{
		if (InOverride != null)
		{
			InOverride.GetBoolean(ref _value);
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
		archive.WriteBool(_value);
		return true;
	}
}
