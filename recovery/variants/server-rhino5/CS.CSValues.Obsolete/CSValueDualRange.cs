using System;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues.Obsolete;

[Obsolete]
[CSBlock("ValueDualRange", "{2EF2B266-78A0-4245-91F8-BCE367E508C2}")]
public sealed class CSValueDualRange : CSValue
{
	private double _maxValue1 = 10.0;

	private double _maxValue2 = 10.0;

	private double _minValue1;

	private double _minValue2;

	private double _step = 0.1;

	private double _value2;

	private bool _hasCreatedUndoTransaction;

	public CSOutputChannel OutValue2 { get; }

	public double MaxValue1
	{
		get
		{
			return _maxValue1;
		}
		set
		{
			_maxValue1 = value;
		}
	}

	public double MaxValue2
	{
		get
		{
			return _maxValue2;
		}
		set
		{
			_maxValue2 = value;
		}
	}

	public double MinValue1
	{
		get
		{
			return _minValue1;
		}
		set
		{
			_minValue1 = value;
		}
	}

	public double MinValue2
	{
		get
		{
			return _minValue2;
		}
		set
		{
			_minValue2 = value;
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

	public double Value2
	{
		get
		{
			return _value2;
		}
		set
		{
			_value2 = value;
		}
	}

	public void SetFromUI(double value, double inputMinValue, double inputMaxValue, double inputStep, double inputValue2, double inputMinValue2, double inputMaxValue2, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (Math.Abs(value - base.RealValue) > 0.005 || Math.Abs(inputMaxValue - _maxValue1) > 0.005 || Math.Abs(inputMinValue - _minValue1) > 0.005 || Math.Abs(inputMaxValue2 - _maxValue2) > 0.005 || Math.Abs(inputMinValue2 - _minValue2) > 0.005 || Math.Abs(inputValue2 - _value2) > 0.005 || Math.Abs(inputStep - _step) > 0.005 || identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_previousValue = value;
				_doubleValue = value;
				_minValue1 = inputMinValue;
				_step = inputStep;
				_maxValue1 = inputMaxValue;
				SetIdentifier(identifier);
				_value2 = inputValue2;
				_minValue2 = inputMinValue2;
				_maxValue2 = inputMaxValue2;
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

	public void SetMaxFromUI(double inputMaxValue, double inputMaxValue2)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMaxFromUI");
			PreChange("SetMaxFromUI");
			_maxValue1 = inputMaxValue;
			_maxValue2 = inputMaxValue2;
			Change();
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

	public void SetMinFromUI(double inputMinValue, double inputMinValue2)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMinFromUI");
			PreChange("SetMinFromUI");
			_minValue1 = inputMinValue;
			_minValue2 = inputMinValue2;
			Change();
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

	public bool SetValue2(double newValue)
	{
		if (newValue != _value2)
		{
			PreChange("SetValue2");
			_value2 = newValue;
			Change();
		}
		return true;
	}

	public void SetValueFromUI(double value, double value2, bool mouseDown)
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
				SetValue2(value2);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetValue(value);
				SetValue2(value2);
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

	public CSValueDualRange(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue2 = new CSOutputChannel(GetPropertyName(() => OutValue2), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueDualRange(string identifier, double inputValue1, double inputValue2)
		: base(identifier)
	{
		_previousValue = inputValue1;
		_doubleValue = inputValue1;
		_value2 = inputValue2;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue2 = new CSOutputChannel(GetPropertyName(() => OutValue2), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueDualRange(string identifier, double inputValue1, double inputValue2, double inputMinValue, double inputMaxValue, double inputStep)
		: base(identifier)
	{
		_previousValue = inputValue1;
		_doubleValue = inputValue1;
		_value2 = inputValue2;
		_minValue1 = inputMinValue;
		_maxValue1 = inputMaxValue;
		_minValue2 = inputMinValue;
		_maxValue2 = inputMaxValue;
		_step = inputStep;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue2 = new CSOutputChannel(GetPropertyName(() => OutValue2), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueDualRange(string identifier, double inputValue1, double inputMinValue1, double inputMaxValue1, double inputValue2, double inputMinValue2, double inputMaxValue2, double inputStep)
		: base(identifier)
	{
		_previousValue = inputValue1;
		_doubleValue = inputValue1;
		_minValue1 = inputMinValue1;
		_maxValue2 = inputMaxValue1;
		_value2 = inputValue2;
		_minValue2 = inputMinValue2;
		_maxValue2 = inputMaxValue2;
		_step = inputStep;
		OutValue2 = new CSOutputChannel(GetPropertyName(() => OutValue2), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (!IsOn())
		{
			return null;
		}
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutValue2)
		{
			OutValue2.SetData(new CSNumberData(_value2));
			return OutValue2;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (!IsOn())
		{
			return new CSData();
		}
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutValue2)
		{
			return new CSNumberData(_value2);
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public override bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jObject, graph, major, minor) & jObject.ReadJsonValue<double>("minValue1", out _minValue1) & jObject.ReadJsonValue<double>("maxValue1", out _maxValue1) & jObject.ReadJsonValue<double>("step", out _step) & jObject.ReadJsonValue<double>("Value2", out _value2) & jObject.ReadJsonValue<double>("minValue2", out _minValue2) & jObject.ReadJsonValue<double>("maxValue2", out _maxValue2);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _minValue1);
		archive.ReadDouble(ref _maxValue1);
		archive.ReadDouble(ref _step);
		archive.ReadDouble(ref _value2);
		archive.ReadDouble(ref _minValue2);
		archive.ReadDouble(ref _maxValue2);
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && setObject is CSValueDualRange cSValueDualRange)
		{
			_value2 = cSValueDualRange._value2;
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSValueDualRange cSValueDualRange)
		{
			_step = cSValueDualRange._step;
			_minValue1 = cSValueDualRange._minValue1;
			_maxValue1 = cSValueDualRange._maxValue1;
			_minValue2 = cSValueDualRange._minValue2;
			_maxValue2 = cSValueDualRange._maxValue2;
			_value2 = cSValueDualRange._value2;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueDualRange cSValueDualRange)
		{
			_step = cSValueDualRange._step;
			_minValue1 = cSValueDualRange._minValue1;
			_maxValue1 = cSValueDualRange._maxValue1;
			_minValue2 = cSValueDualRange._minValue2;
			MaxValue2 = cSValueDualRange._maxValue2;
			_value2 = cSValueDualRange._value2;
		}
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("minValue1", _minValue1);
		archive.Add("maxValue1", _maxValue1);
		archive.Add("step", _step);
		archive.Add("Value2", _value2);
		archive.Add("minValue2", _minValue2);
		archive.Add("maxValue2", _maxValue2);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_minValue1);
		archive.WriteDouble(_maxValue1);
		archive.WriteDouble(_step);
		archive.WriteDouble(_value2);
		archive.WriteDouble(_minValue2);
		archive.WriteDouble(_maxValue2);
		return true;
	}
}
