using System;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues.Obsolete;

[Obsolete]
[CSBlock("ValueRange1", "{62B4DDEC-4FCE-4AB7-9FB7-E41AD9C87566}")]
public sealed class CSValueRange1 : CSValueBase, ICSValue<double>, ICSLoopUpdate
{
	private double _doubleValue;

	private double _maxValue = 10.0;

	private double _minValue = -10.0;

	private double _step = 0.1;

	private bool _hasCreatedUndoTransaction;

	public CSInputChannel InOverride { get; }

	public CSOutputChannel OutValue { get; }

	public double MaxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value;
		}
	}

	public double MinValue
	{
		get
		{
			return _minValue;
		}
		set
		{
			_minValue = value;
		}
	}

	public double Step
	{
		get
		{
			return _step;
		}
		set
		{
			_step = value;
		}
	}

	public CSInputChannel InDisplayUpdate { get; set; }

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

	public CSValueRange1(string identifier)
		: base(identifier)
	{
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueRange1(string identifier, double inputValue)
		: base(identifier)
	{
		_doubleValue = inputValue;
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueRange1(string identifier, double inputValue, double inputMinValue, double inputMaxValue, double inputStep)
		: base(identifier)
	{
		_doubleValue = inputValue;
		_minValue = inputMinValue;
		_maxValue = inputMaxValue;
		_step = inputStep;
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public void LoopUpdate()
	{
		double value = 0.0;
		if (InDisplayUpdate.GetDouble(ref value) && value != _doubleValue)
		{
			_doubleValue = value;
			UIChanged = true;
		}
	}

	public bool SetValue(double newValue)
	{
		if (newValue != _doubleValue)
		{
			PreChange("SetValue");
			_doubleValue = newValue;
			Change();
		}
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jBlock, graph, major, minor) & jBlock.ReadJsonValue<double>("minValue", out _minValue) & jBlock.ReadJsonValue<double>("maxValue", out _maxValue) & jBlock.ReadJsonValue<double>("step", out _step);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _minValue);
		archive.ReadDouble(ref _maxValue);
		archive.ReadDouble(ref _step);
		return true;
	}

	public void SetFromUI(double value, double inputMinValue, double inputMaxValue, double inputStep, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (Math.Abs(value - _doubleValue) > 0.005 || Math.Abs(inputMaxValue - _maxValue) > 0.005 || Math.Abs(inputMinValue - _minValue) > 0.005 || Math.Abs(inputStep - _step) > 0.005 || identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_doubleValue = value;
				_minValue = inputMinValue;
				_step = inputStep;
				_maxValue = inputMaxValue;
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

	public void SetMaxFromUI(double inputMaxValue)
	{
		if (_maxValue == inputMaxValue)
		{
			return;
		}
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMaxFromUI");
			if (inputMaxValue != _maxValue)
			{
				PreChange("SetMaxFromUI");
				_maxValue = inputMaxValue;
				Change();
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetMaxFromUI");
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

	public void SetMinFromUI(double inputMinValue)
	{
		if (_minValue == inputMinValue)
		{
			return;
		}
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMinFromUI");
			if (inputMinValue != _minValue)
			{
				PreChange("SetMinFromUI");
				_minValue = inputMinValue;
				Change();
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetMinFromUI");
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

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSValueRange1 cSValueRange)
		{
			_doubleValue = cSValueRange._doubleValue;
			_step = cSValueRange._step;
			_minValue = cSValueRange._minValue;
			_maxValue = cSValueRange._maxValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueRange1 cSValueRange)
		{
			_doubleValue = cSValueRange._doubleValue;
			_step = cSValueRange._step;
			_minValue = cSValueRange._minValue;
			_maxValue = cSValueRange._maxValue;
		}
		return true;
	}

	public void SetValueFromUI(double value, bool mouseDown)
	{
		if (mouseDown && Value == value)
		{
			return;
		}
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

	public override bool Update()
	{
		if (InOverride != null)
		{
			InOverride.GetDouble(ref _doubleValue);
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("minValue", _minValue);
		archive.Add("maxValue", _maxValue);
		archive.Add("step", _step);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_minValue);
		archive.WriteDouble(_maxValue);
		archive.WriteDouble(_step);
		return true;
	}
}
