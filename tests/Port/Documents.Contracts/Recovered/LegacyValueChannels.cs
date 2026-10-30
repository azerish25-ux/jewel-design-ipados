// Generated test-only source extraction; observation stubs are not native/graph substitutes.
namespace DocumentContractChecks.Recovered.ValueChannels;
internal class CSData
{
    public virtual bool GetDouble(ref double value)=>false;
    public virtual bool GetBoolean(ref bool value)=>false;
}
internal sealed class CSNumberData:CSData
{
    private readonly double _numberData;
    internal CSNumberData(double value){_numberData=value;}
public override bool GetDouble(ref double value)
	{
		value = _numberData;
		return true;
	}
public override bool GetBoolean(ref bool value)
	{
		value = _numberData > 0.5;
		return true;
	}}
internal sealed class Input
{
    private readonly CSData data;
    internal Input(CSData data){this.data=data;}
    internal bool GetDouble(ref double value)=>data.GetDouble(ref value);
    internal bool GetBoolean(ref bool value)=>data.GetBoolean(ref value);
}
internal sealed class CSOutputChannel{}
internal class CSBlock
{
    protected readonly List<CSOutputChannel> OutputChannels=new();
    public virtual bool Update()=>true;
    public virtual CSData GetOutputChannelData(int index)=>new CSData();
}
internal static class CSApplicationHelper
{
    internal sealed class App{public bool IsCamValueMode;}
    internal static readonly App Application=new();
}
internal class CSValue:CSBlock
{
    protected double _doubleValue,_previousValue;
    private readonly bool _absoluteCAM;
    private readonly double _doubleCAMAbsolute,_doubleCAMDelta;
    protected readonly Input InOn,InOverride;
    protected readonly CSOutputChannel OutValue=new(),OutNegated=new(),OutOneMinus=new();
    internal double Real=>_doubleValue;
    internal double Previous=>_previousValue;
    internal CSValue(double value,double previous,bool cam,bool absolute,double absoluteValue,double delta,CSData on,CSData over)
    {
        _doubleValue=value;_previousValue=previous;_absoluteCAM=absolute;_doubleCAMAbsolute=absoluteValue;_doubleCAMDelta=delta;
        CSApplicationHelper.Application.IsCamValueMode=cam;InOn=new(on);InOverride=new(over);
        OutputChannels.AddRange(new[]{OutValue,OutNegated,OutOneMinus});
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
protected bool IsOn()
	{
		bool value = true;
		return !InOn.GetBoolean(ref value) || value;
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
	}}
internal sealed class CSValueOnOff:CSValue
{
    private readonly CSOutputChannel OutOnOff=new(),OutGroupOnOff=new();
    internal CSValueOnOff(double value,double previous,bool cam,bool absolute,double absoluteValue,double delta,CSData on,CSData over)
        :base(value,previous,cam,absolute,absoluteValue,delta,on,over)
    {OutputChannels.AddRange(new[]{OutOnOff,OutGroupOnOff});}
public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutGroupOnOff)
			{
				if (!IsOn())
				{
					return new CSNumberData(0.0);
				}
				return new CSNumberData(base.Value);
			}
			if (cSOutputChannel == OutOnOff)
			{
				if (!IsOn())
				{
					return new CSData();
				}
				return new CSNumberData(base.Value);
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}
}
