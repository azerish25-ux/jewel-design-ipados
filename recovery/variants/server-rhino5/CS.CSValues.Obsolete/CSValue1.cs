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
[CSBlock("Value1", "{23281C9C-D70C-4F5B-B50C-AB2C7D9EAFD3}")]
public sealed class CSValue1 : CSValueBase, ICSValue<double>
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

	public CSValue1(string identifier)
		: base(identifier)
	{
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Any, ChannelArrayType.Individual);
	}

	public CSValue1(string identifier, double value)
		: base(identifier)
	{
		_doubleValue = value;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Any, ChannelArrayType.Individual);
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
		if (base.ReadJson(jBlock, graph, major, minor))
		{
			return jBlock.ReadJsonValue<double>("Value", out _doubleValue);
		}
		return false;
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
			_doubleValue = ((CSValue1)setObject)._doubleValue;
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
			_doubleValue = ((CSValue1)setObject)._doubleValue;
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValue1)setObject)._doubleValue;
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
