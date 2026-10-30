using System;
using System.Text;
using CS.CSObjects;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.CSValues;

[CSBlock("Value", "{38455005-5EF2-43db-8FB9-60E447D75D59}")]
public class CSValue : CSValueBase, ICSValue<double>
{
	private const short MajorCAMDeltaVersion = 4;

	private const short MajorCAMVersion = 5;

	private const short MajorNoCAMVersion = 3;

	protected double _doubleValue;

	protected double _previousValue;

	private bool _absoluteCAM;

	private double _doubleCAMAbsolute;

	private double _doubleCAMDelta;

	private string _noteCAM = string.Empty;

	public CSInputChannel InOn { get; }

	public CSInputChannel InOverride { get; }

	public CSOutputChannel OutNegated { get; }

	public CSOutputChannel OutOneMinus { get; }

	public CSOutputChannel OutValue { get; }

	public double PreviousValue => _previousValue;

	public double RealValue => _doubleValue;

	public double CAMAbsolute
	{
		get
		{
			return _doubleCAMAbsolute;
		}
		set
		{
			SetCAMAbsolute(value);
		}
	}

	public double CAMDelta
	{
		get
		{
			return _doubleCAMDelta;
		}
		set
		{
			SetCAMDelta(value);
		}
	}

	public bool CAMIsAbsolute
	{
		get
		{
			return _absoluteCAM;
		}
		set
		{
			_absoluteCAM = value;
			Change();
		}
	}

	public string CAMNote
	{
		get
		{
			return _noteCAM;
		}
		set
		{
			SetCAMNote(value);
		}
	}

	public double CAMValue
	{
		get
		{
			if (_absoluteCAM)
			{
				return _doubleCAMAbsolute;
			}
			return _doubleCAMDelta;
		}
		set
		{
			if (_absoluteCAM)
			{
				SetCAMAbsolute(value);
			}
			else
			{
				SetCAMDelta(value);
			}
		}
	}

	public double Value
	{
		get
		{
			if (!CSApplicationHelper.Application.IsCamValueMode)
			{
				return _doubleValue;
			}
			if (_absoluteCAM)
			{
				return _doubleCAMAbsolute;
			}
			return _doubleValue + _doubleCAMDelta;
		}
	}

	public CSValue(string identifier)
		: base(identifier)
	{
		InOn = new CSInputChannel(GetPropertyName(() => InOn), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutNegated = new CSOutputChannel(GetPropertyName(() => OutNegated), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutOneMinus = new CSOutputChannel(GetPropertyName(() => OutOneMinus), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public CSValue(string identifier, double value)
		: base(identifier)
	{
		_previousValue = value;
		_doubleValue = value;
		InOn = new CSInputChannel(GetPropertyName(() => InOn), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InOverride = new CSInputChannel(GetPropertyName(() => InOverride), this, ChannelDataType.Double, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutNegated = new CSOutputChannel(GetPropertyName(() => OutNegated), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutOneMinus = new CSOutputChannel(GetPropertyName(() => OutOneMinus), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public bool HasCAM()
	{
		if (!_absoluteCAM && !(Math.Abs(_doubleCAMDelta) > 0.0001))
		{
			return !string.IsNullOrEmpty(_noteCAM);
		}
		return true;
	}

	public bool SetCAMAbsolute(double newValue)
	{
		_absoluteCAM = true;
		if (newValue != _doubleCAMAbsolute)
		{
			_doubleCAMAbsolute = newValue;
			Change();
		}
		return true;
	}

	public bool SetCAMDelta(double newValue)
	{
		_absoluteCAM = false;
		if (newValue != _doubleCAMDelta)
		{
			_doubleCAMDelta = newValue;
			Change();
		}
		return true;
	}

	public bool SetCAMNote(string newValue)
	{
		if (newValue != _noteCAM)
		{
			_noteCAM = newValue;
			Change();
		}
		return true;
	}

	protected bool IsOn()
	{
		bool value = true;
		return !InOn.GetBoolean(ref value) || value;
	}

	public bool SetValue(double newValue)
	{
		if (Math.Abs(newValue - _doubleValue) < 1E-05)
		{
			return true;
		}
		_previousValue = _doubleValue;
		PreChange("SetValue");
		_doubleValue = newValue;
		Change();
		return true;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (!IsOn())
		{
			OutValue.SetData(new CSData());
			return OutValue;
		}
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutValue)
			{
				OutValue.SetData(new CSNumberData(Value));
				return OutValue;
			}
			if (cSOutputChannel == OutNegated)
			{
				OutNegated.SetData(new CSNumberData(0.0 - Value));
				return OutNegated;
			}
			if (cSOutputChannel == OutOneMinus)
			{
				OutOneMinus.SetData(new CSNumberData(1.0 - Value));
				return OutOneMinus;
			}
		}
		OutValue.SetData(new CSNumberData(Value));
		return OutValue;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (!IsOn())
		{
			return new CSData();
		}
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutValue)
			{
				return new CSNumberData(Value);
			}
			if (cSOutputChannel == OutNegated)
			{
				return new CSNumberData(0.0 - Value);
			}
			if (cSOutputChannel == OutOneMinus)
			{
				return new CSNumberData(1.0 - Value);
			}
		}
		return new CSNumberData(Value);
	}

	protected override void MajorMinorVersion(out int majorVersion, out int minorVersion)
	{
		majorVersion = (HasCAM() ? 5 : 3);
		minorVersion = 0;
	}

	public override bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(jObject, graph, major, minor);
		flag &= jObject.ReadJsonValue<double>("Value", out _doubleValue);
		_previousValue = _doubleValue;
		if (major >= 4)
		{
			flag &= jObject.ReadJsonValue<bool>("absoluteCAM", out _absoluteCAM);
			flag = ((!_absoluteCAM) ? (flag & jObject.ReadJsonValue<double>("CAMDelta", out _doubleCAMDelta)) : (flag & jObject.ReadJsonValue<double>("CAMAbsolute", out _doubleCAMAbsolute)));
			flag &= jObject.ReadJsonValue<string>("NotesCAM", out _noteCAM);
		}
		return flag;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _doubleValue);
		_previousValue = _doubleValue;
		if (major >= 4)
		{
			double d = 0.0;
			archive.ReadDouble(ref d);
			if (major >= 5)
			{
				archive.ReadBool(ref _absoluteCAM);
			}
			if (_absoluteCAM)
			{
				_doubleCAMAbsolute = d;
			}
			else
			{
				_doubleCAMDelta = d;
			}
			short s = 0;
			archive.ReadShort(ref s);
			if (s > 0)
			{
				byte[] bytes = new byte[s];
				archive.ReadBytes(ref bytes);
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				_noteCAM = aSCIIEncoding.GetString(bytes);
			}
		}
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValue)setObject).RealValue;
			_previousValue = RealValue;
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
			_doubleValue = ((CSValue)setObject).RealValue;
			_previousValue = RealValue;
		}
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
			SetValue(((CSValue)setObject).RealValue);
			CAMAbsolute = ((CSValue)setObject).CAMAbsolute;
			CAMDelta = ((CSValue)setObject).CAMDelta;
		}
		return true;
	}

	public override bool Update()
	{
		double value = 0.0;
		if (InOverride.GetDouble(ref value))
		{
			_doubleValue = value;
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _doubleValue);
		if (HasCAM())
		{
			if (_absoluteCAM)
			{
				archive.Add("CAMAbsolute", _doubleCAMAbsolute);
			}
			else
			{
				archive.Add("CAMDelta", _doubleCAMDelta);
			}
			archive.Add("absoluteCAM", _absoluteCAM);
			archive.Add("NotesCAM", _noteCAM);
		}
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_doubleValue);
		if (HasCAM())
		{
			if (_absoluteCAM)
			{
				archive.WriteDouble(_doubleCAMAbsolute);
			}
			else
			{
				archive.WriteDouble(_doubleCAMDelta);
			}
			archive.WriteBool(_absoluteCAM);
			byte[] bytes = new ASCIIEncoding().GetBytes(_noteCAM);
			archive.WriteShort((short)bytes.Length);
			if (bytes.Length != 0)
			{
				archive.WriteBytes(bytes);
			}
		}
		return true;
	}
}
