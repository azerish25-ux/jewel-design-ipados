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
[CSBlock("ValueList1", "{30B2C6B5-E167-41da-BF4F-5689B4E99128}")]
public sealed class CSValueList1 : CSValue
{
	private List<double> _list = new List<double>();

	private double _maxValue = 10.0;

	private double _minValue;

	private bool _hasCreatedUndoTransaction;

	public CSOutputChannel OutListValue { get; }

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

	public CSValueList1(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(GetPropertyName(() => OutListValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueList1(string identifier, double inputValue)
		: base(identifier)
	{
		_previousValue = inputValue;
		_doubleValue = inputValue;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(GetPropertyName(() => OutListValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValueList1(string identifier, double inputValue, double inputMinValue, double inputMaxValue)
		: base(identifier)
	{
		_previousValue = inputValue;
		_doubleValue = inputValue;
		_minValue = inputMinValue;
		_maxValue = inputMaxValue;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutListValue = new CSOutputChannel(GetPropertyName(() => OutListValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public void AddToList(double newNumber)
	{
		_list.Add(newNumber);
	}

	public int GetCurrentListIndex()
	{
		int num = -1;
		for (int i = 0; i < _list.Count; i++)
		{
			if (_list[i] >= _minValue && _list[i] <= _maxValue)
			{
				num++;
			}
			if ((double)i == base.Value)
			{
				return num;
			}
		}
		return -1;
	}

	public int GetIndexFromValue(double value)
	{
		List<double> listOfCurrentValues = GetListOfCurrentValues();
		int i;
		for (i = 0; i < listOfCurrentValues.Count && !(listOfCurrentValues[i] > value); i++)
		{
		}
		i--;
		if (i < 0)
		{
			i = 0;
		}
		return i;
	}

	public int GetListLength()
	{
		int num = 0;
		for (int i = 0; i < _list.Count; i++)
		{
			if (_list[i] >= _minValue && _list[i] <= _maxValue)
			{
				num++;
			}
		}
		return num;
	}

	public List<double> GetListOfCurrentValues()
	{
		int i = 0;
		List<double> list = new List<double>();
		for (; i < _list.Count; i++)
		{
			if (_list[i] >= _minValue && _list[i] <= _maxValue)
			{
				list.Add(_list[i]);
			}
		}
		return list;
	}

	public double GetListValue(double index)
	{
		double num = -1.0;
		for (int i = 0; i < _list.Count; i++)
		{
			if (_list[i] >= _minValue && _list[i] <= _maxValue)
			{
				num += 1.0;
			}
			if (num == index)
			{
				return _list[i];
			}
		}
		return 0.0;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutListValue)
		{
			OutListValue.SetData(new CSNumberData(GetListValue(base.Value)));
			return OutListValue;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutListValue)
		{
			return new CSNumberData(GetListValue(base.Value));
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public double GetTotalListValue(double index)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			if ((double)i == index)
			{
				return _list[i];
			}
		}
		return 0.0;
	}

	public override bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jObject, graph, major, minor) & jObject.ReadJsonValue<double>("minValue", out _minValue) & jObject.ReadJsonValue<double>("maxValue", out _maxValue) & jObject.ReadJsonObject<List<double>>("ValueList", out _list);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _minValue);
		archive.ReadDouble(ref _maxValue);
		double[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			_list = new List<double>();
			return false;
		}
		_list = new List<double>(items);
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		return base.SetFileValue(setObject);
	}

	public void SetFromUI(double valueValue, double inputMinValue, double inputMaxValue, string identifier, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		double num = GetIndexFromValue(valueValue);
		try
		{
			if (Math.Abs(num - base.RealValue) > 0.005 || Math.Abs(inputMaxValue - _maxValue) > 0.005 || Math.Abs(inputMinValue - _minValue) > 0.005 || identifier != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				PreChange("SetFromUI");
				_previousValue = num;
				_doubleValue = num;
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
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public void SetList(double[] newList)
	{
		if (newList == null)
		{
			_list = new List<double>();
		}
		else
		{
			_list = new List<double>(newList);
		}
	}

	public void SetListIndexFromUI(double value, bool mouseDown)
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
				SetListValue(value);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetListValue(value);
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

	public int SetListValue(double index)
	{
		double num = -1.0;
		int i;
		for (i = 0; i < _list.Count; i++)
		{
			if (_list[i] >= _minValue && _list[i] <= _maxValue)
			{
				num += 1.0;
			}
			if (num == index)
			{
				SetValue(i);
				break;
			}
		}
		return i;
	}

	public void SetListValueFromUI(double value)
	{
		SetListIndexFromUI(GetIndexFromValue(value), mouseDown: false);
	}

	public void SetMaxFromUI(double inputMaxValue)
	{
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
		if (!base.NotCreatedYet && setObject is CSValueList1 cSValueList)
		{
			_minValue = cSValueList._minValue;
			_maxValue = cSValueList._maxValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && setObject is CSValueList1 cSValueList)
		{
			_minValue = cSValueList._minValue;
			_maxValue = cSValueList._maxValue;
		}
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("minValue", _minValue);
		archive.Add("maxValue", _maxValue);
		archive.Add("ValueList", JArray.FromObject(_list));
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_minValue);
		archive.WriteDouble(_maxValue);
		CSUtilIO.WriteArray(_list.ToArray(), ref archive);
		return true;
	}
}
