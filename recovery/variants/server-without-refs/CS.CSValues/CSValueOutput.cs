using System;
using System.Globalization;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueOutput", "{14000DF7-F6DF-4682-9774-FCEDC158A61B}")]
public sealed class CSValueOutput : CSValueBase, ICSValue<string>
{
	private string _value;

	public CSInputChannel InValue { get; }

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

	public CSValueOutput(string identifier)
		: base(identifier)
	{
		InValue = new CSInputChannel(this, ChannelDataType.Boolean | ChannelDataType.Double | ChannelDataType.String, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		SetChannelIdentifiers();
	}

	public CSValueOutput(string identifier, string value)
		: base(identifier)
	{
		_value = value;
		InValue = new CSInputChannel(this, ChannelDataType.Boolean | ChannelDataType.Double | ChannelDataType.String, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		SetChannelIdentifiers();
	}

	public bool SetValue(string newValue)
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

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		return null;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		return new CSStringData(_value);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		double num = 0.0;
		archive.ReadDouble(ref num);
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
			_value = ((CSValueOutput)setObject)._value;
		}
		return true;
	}

	public void SetFromUI(string title, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (title != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				base.Identifier = title;
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
		_value = ((CSValueOutput)setObject)._value;
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
			_value = ((CSValueOutput)setObject)._value;
		}
		return true;
	}

	public override bool Update()
	{
		bool value = false;
		if (InValue.GetBoolean(ref value))
		{
			_value = value.ToString();
		}
		int value2 = 0;
		if (InValue.GetInteger(ref value2))
		{
			_value = value2.ToString(CultureInfo.InvariantCulture);
		}
		double value3 = 0.0;
		if (InValue.GetDouble(ref value3))
		{
			_value = value3.ToString(CultureInfo.InvariantCulture);
		}
		string output = string.Empty;
		if (InValue.GetString(ref output))
		{
			_value = output;
		}
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(0.0);
		return true;
	}
}
