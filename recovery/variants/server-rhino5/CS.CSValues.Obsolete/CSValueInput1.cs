using System;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues.Obsolete;

[Obsolete]
[CSBlock("ValueInput1", "{CA78EBC2-CAB2-4F02-8895-4651F656C3FF}")]
public sealed class CSValueInput1 : CSValueBase, ICSValue<double>
{
	private double _doubleValue;

	public CSOutputChannel OutValue { get; }

	public double Value
	{
		get
		{
			return _doubleValue;
		}
		set
		{
			SetValue(value);
		}
	}

	public CSValueInput1(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueInput1(string identifier, double value)
		: base(identifier)
	{
		_doubleValue = value;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public void SetFromUI(string title, double value, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (Math.Abs(_doubleValue - value) > 0.005 || title != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_doubleValue = value;
				SetIdentifier(title);
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
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public void SetValueFromUI(double newValue, bool mouseDown)
	{
		if (mouseDown && Math.Abs(newValue - _doubleValue) < CSApplicationHelper.Application.Tolerance)
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

	public bool SetValue(double newValue)
	{
		if (Math.Abs(newValue - _doubleValue) < CSApplicationHelper.Application.Tolerance)
		{
			return true;
		}
		PreChange("SetValue");
		_doubleValue = newValue;
		Change();
		return true;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		OutValue.SetData(new CSNumberData(_doubleValue));
		return OutValue;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		return new CSNumberData(_doubleValue);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(jBlock, graph, major, minor);
		return jBlock.ReadJsonValue<double>("Value", out _doubleValue) && flag;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _doubleValue);
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValueInput1)setObject)._doubleValue;
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			if (setObject.Identifier != null && setObject.Identifier.Length > 0)
			{
				SetIdentifier(setObject.Identifier);
			}
			_doubleValue = ((CSValueInput1)setObject)._doubleValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValueInput1)setObject)._doubleValue;
		}
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _doubleValue);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_doubleValue);
		return true;
	}
}
