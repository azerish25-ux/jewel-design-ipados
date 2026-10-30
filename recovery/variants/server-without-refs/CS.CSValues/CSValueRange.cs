using System;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("ValueRange", "{56B84606-7F1D-41fb-B81F-F521D9A9577D}")]
public sealed class CSValueRange : CSValue, ICSLoopUpdate
{
	private bool _hasCreatedUndoTransaction;

	private double _maxValue = 10.0;

	private double _minValue = -10.0;

	private double _step = 0.1;

	public CSInputChannel InAutoLink { get; }

	public CSInputChannel InMove { get; private set; }

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

	public CSValueRange(string identifier)
		: base(identifier)
	{
		InMove = new CSInputChannel(GetPropertyName(() => InMove), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InAutoLink = new CSInputChannel(GetPropertyName(() => InAutoLink), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public CSValueRange(string identifier, double inputValue)
		: base(identifier)
	{
		_previousValue = inputValue;
		_doubleValue = inputValue;
		InMove = new CSInputChannel(GetPropertyName(() => InMove), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InAutoLink = new CSInputChannel(GetPropertyName(() => InAutoLink), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public CSValueRange(string identifier, double inputValue, double inputMinValue, double inputMaxValue, double inputStep)
		: base(identifier)
	{
		_previousValue = inputValue;
		_doubleValue = inputValue;
		_minValue = inputMinValue;
		_maxValue = inputMaxValue;
		_step = inputStep;
		InMove = new CSInputChannel(GetPropertyName(() => InMove), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InAutoLink = new CSInputChannel(GetPropertyName(() => InAutoLink), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InDisplayUpdate = new CSInputChannel(GetPropertyName(() => InDisplayUpdate), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public void LoopUpdate()
	{
		if (InDisplayUpdate != null)
		{
			double value = 0.0;
			if (InDisplayUpdate.GetDouble(ref value) && Math.Abs(value - base.RealValue) > CSApplicationHelper.Application.Tolerance)
			{
				SetValue(value);
				UIChanged = true;
			}
		}
	}

	public bool CanAutoLink()
	{
		if (InAutoLink != null && InMove != null && InAutoLink.OutputChannelBlock != null)
		{
			if (InMove.OutputChannelBlock == InAutoLink.OutputChannelBlock)
			{
				return true;
			}
			if (InMove.OutputChannelBlock == null)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAutoLinkOn()
	{
		if (InAutoLink != null && InMove != null && InAutoLink.OutputChannelBlock != null && InMove.OutputChannelBlock == InAutoLink.OutputChannelBlock)
		{
			return true;
		}
		return false;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(archive, graph, major, minor) & archive.ReadJsonValue<double>("minValue", out _minValue) & archive.ReadJsonValue<double>("maxValue", out _maxValue) & archive.ReadJsonValue<double>("step", out _step);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _minValue);
		archive.ReadDouble(ref _maxValue);
		archive.ReadDouble(ref _step);
		return true;
	}

	public static void SetAutolink(CSValueRange valueRangeLhs, CSValueRange valueRangeRhs)
	{
		valueRangeLhs.InAutoLink.SetLink(valueRangeRhs.OutValue);
	}

	public void SetCAMTypeFromUI(bool value, bool mouseDown)
	{
		if (value != base.CAMIsAbsolute)
		{
			base.CAMIsAbsolute = value;
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetCAMValueFromUI(double value, bool mouseDown)
	{
		double num = 0.0;
		num = ((!base.CAMIsAbsolute) ? base.CAMDelta : base.CAMAbsolute);
		if (mouseDown && num == value)
		{
			return;
		}
		try
		{
			using (new CSUndoTransaction())
			{
				PreChange("SetCAMValueFromUI");
				UIChanged = true;
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				if (base.CAMIsAbsolute)
				{
					SetCAMAbsolute(value);
				}
				else
				{
					SetCAMDelta(value);
				}
			}
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

	public void SetFromUI(double value, double inputMinValue, double inputMaxValue, double inputStep, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			bool flag = Math.Abs(value - base.RealValue) > 0.005 || Math.Abs(inputMaxValue - _maxValue) > 0.005 || Math.Abs(inputMinValue - _minValue) > 0.005 || Math.Abs(inputStep - _step) > 0.005;
			bool flag2 = identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption;
			if (flag || flag2)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				if (flag)
				{
					_previousValue = value;
					_doubleValue = value;
					_minValue = inputMinValue;
					_step = inputStep;
					_maxValue = inputMaxValue;
				}
				if (flag2)
				{
					SetIdentifier(identifier);
					base.StudioVisible = studioVisible;
					base.StudioVisibleAdvanced = studioVisibleAdvanced;
					base.StudioVisibleDesignOption = studioVisibleDesignOption;
				}
				Change();
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
				if (flag)
				{
					CSApplicationHelper.Application.Document.Graph.Generate();
					CSUtil.Redraw();
				}
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
			if (inputMaxValue < _doubleValue)
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
			else
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
			if (inputMinValue > _doubleValue)
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
			else
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

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (!base.NotCreatedYet && setObject is CSValueRange cSValueRange)
		{
			_step = cSValueRange._step;
			_minValue = cSValueRange._minValue;
			_maxValue = cSValueRange._maxValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueRange cSValueRange)
		{
			_step = cSValueRange._step;
			_minValue = cSValueRange._minValue;
			_maxValue = cSValueRange._maxValue;
		}
		return true;
	}

	public void SetValueFromUI(double value, bool mouseDown)
	{
		if (mouseDown && Math.Abs(base.RealValue - value) < CSApplicationHelper.Application.Tolerance)
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
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	public void TurnOffAutoLink()
	{
		if (InAutoLink != null && InMove != null)
		{
			PreChange("TurnOffAutoLink");
			if (InMove.OutputChannelBlock == InAutoLink.OutputChannelBlock)
			{
				InMove.BreakLink();
			}
			Change();
		}
	}

	public void TurnOnAutoLink()
	{
		if (InAutoLink != null && InMove != null)
		{
			PreChange("TurnOnAutoLink");
			if (InMove.OutputChannelBlock == InAutoLink.OutputChannelBlock)
			{
				InMove = InAutoLink;
			}
			Change();
		}
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
