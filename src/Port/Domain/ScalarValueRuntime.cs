namespace JewelDesign.Domain;

/// <summary>Recovered scalar runtime fields; Update override is distinct from the UI setter/undo transition.</summary>
public readonly record struct ScalarValueRuntime(NumericValueState Numeric,bool AbsoluteCam=false,double CamAbsolute=0,double CamDelta=0)
{
    public double EffectiveValue(bool camMode)=>!camMode?Numeric.RealValue:AbsoluteCam?CamAbsolute:Numeric.RealValue+CamDelta;
    public ScalarValueRuntime ApplyOverride(bool hasNumber,double value)=>hasNumber?this with{Numeric=new(value,Numeric.PreviousValue)}:this;
    public ScalarValueRuntime SetValue(double value)=>this with{Numeric=Numeric.PlanSetValue(value).After};
    public ScalarNumberOutput ReadNumber(bool isOn,int outputIndex,bool onOff,bool camMode)
    {
        if(onOff&&outputIndex==4&&!isOn)return new(true,0);
        if(!isOn)return new(false,0);
        double value=EffectiveValue(camMode);
        return new(true,outputIndex==1?0.0-value:outputIndex==2?1.0-value:value);
    }
}
public readonly record struct ScalarNumberOutput(bool HasValue,double Value);
