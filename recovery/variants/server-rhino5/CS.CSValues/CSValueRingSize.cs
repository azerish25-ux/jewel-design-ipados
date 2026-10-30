using System;
using CS.CSObjects;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueRingSize", "{DD5C2342-D89A-440e-A76E-624A1FAF2B68}")]
public sealed class CSValueRingSize : CSValue
{
	private bool _hasCreatedUndoTransaction;

	private double _maxValue;

	private double _minValue;

	public double MaxValueIndex => CSApplicationHelper.Application.RingSizeTranslator.GetSizeRadius(_maxValue).index;

	public double MinValueIndex => CSApplicationHelper.Application.RingSizeTranslator.GetSizeRadius(_minValue).index;

	public double ValueIndex => CSApplicationHelper.Application.RingSizeTranslator.GetSizeRadius(base.RealValue).index;

	public double MaxValueRadiusMM
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

	public double MinValueRadiusMM
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

	public CSValueRingSize(string identifier)
		: base(identifier)
	{
		_doubleValue = ConvertFromUSToMMRadius(7.0);
		_previousValue = _doubleValue;
		_minValue = ConvertFromUSToMMRadius(3.0);
		_maxValue = ConvertFromUSToMMRadius(16.0);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public CSValueRingSize(string identifier, double usRingSize, double usMinSize, double usMaxSize)
		: base(identifier)
	{
		_doubleValue = ConvertFromUSToMMRadius(usRingSize);
		_previousValue = _doubleValue;
		_minValue = ConvertFromUSToMMRadius(usMinSize);
		_maxValue = ConvertFromUSToMMRadius(usMaxSize);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public static double ConvertFromUSToMMRadius(double ringSize)
	{
		return (ringSize * (0.102 / OnUtil.On_PI) + 1.43 / OnUtil.On_PI) * 25.4 / 2.0;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<double>("minValue", out _minValue) & archive.ReadJsonValue<double>("maxValue", out _maxValue);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _minValue);
		archive.ReadDouble(ref _maxValue);
		if (major == 1 && minor <= 1)
		{
			_doubleValue = (_previousValue = ConvertFromUSToMMRadius(base.RealValue));
			_minValue = ConvertFromUSToMMRadius(_minValue);
			_maxValue = ConvertFromUSToMMRadius(_maxValue);
		}
		return true;
	}

	public void SetFromUI(double value, double inputMinValue, double inputMaxValue, double inputStep, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		value = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(value).radiusMM;
		inputMinValue = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(inputMinValue).radiusMM;
		inputMaxValue = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(inputMaxValue).radiusMM;
		try
		{
			if (Math.Abs(value - base.RealValue) > 0.005 || Math.Abs(inputMaxValue - _maxValue) > 0.005 || Math.Abs(inputMinValue - _minValue) > 0.005 || identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_previousValue = value;
				_doubleValue = value;
				_minValue = inputMinValue;
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
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public void SetMaxFromUI(double inputMaxValue)
	{
		inputMaxValue = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(inputMaxValue).radiusMM;
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
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public void SetMinFromUI(double inputMinValue)
	{
		inputMinValue = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(inputMinValue).radiusMM;
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
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSValueRingSize cSValueRingSize)
		{
			_minValue = cSValueRingSize._minValue;
			_maxValue = cSValueRingSize._maxValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueRingSize cSValueRingSize)
		{
			_minValue = cSValueRingSize._minValue;
			_maxValue = cSValueRingSize._maxValue;
		}
		return true;
	}

	public void SetValueFromUI(double value, bool mouseDown)
	{
		value = CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(value).radiusMM;
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
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("minValue", _minValue);
		archive.Add("maxValue", _maxValue);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_minValue);
		archive.WriteDouble(_maxValue);
		return true;
	}
}
