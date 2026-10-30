using JewelDesign.Domain;
using Oracle=DocumentContractChecks.Recovered.ValueChannels;
namespace DocumentContractChecks;

internal static class ValueChannelOracleAssertions
{
    private static void Bits(double a,double b,string field)
    {if(BitConverter.DoubleToInt64Bits(a)!=BitConverter.DoubleToInt64Bits(b))throw new Exception("Source channel bits mismatch: "+field);}
    public static int Run()
    {
        var nan=BitConverter.Int64BitsToDouble(unchecked((long)0x7ff8000000000042UL));
        double[] values={0,BitConverter.Int64BitsToDouble(long.MinValue),.25,.5,.5000001,-1,1,2,double.PositiveInfinity,double.NegativeInfinity,nan};
        double?[] onValues={null,0,1,nan},overrideValues={null,.1,BitConverter.Int64BitsToDouble(long.MinValue),nan,double.PositiveInfinity};
        int cases=0;
        foreach(double current in values)foreach(var on in onValues)foreach(var over in overrideValues)
        foreach(double delta in new[]{0.0,1.25})foreach(bool cam in new[]{false,true})foreach(bool absolute in new[]{false,true})foreach(bool onOff in new[]{false,true})
        foreach(int channel in new[]{-5,-1,0,1,2,3,4,99}){
            Oracle.CSData onData=on.HasValue?new Oracle.CSNumberData(on.Value):new Oracle.CSData();
            Oracle.CSData overData=over.HasValue?new Oracle.CSNumberData(over.Value):new Oracle.CSData();
            Oracle.CSValue oracle=onOff?new Oracle.CSValueOnOff(current,-9.25,cam,absolute,-123.5,delta,onData,overData):
                new Oracle.CSValue(current,-9.25,cam,absolute,-123.5,delta,onData,overData);
            var state=new ScalarValueRuntime(new(current,-9.25),absolute,-123.5,delta).ApplyOverride(over.HasValue,over.GetValueOrDefault());
            if(!oracle.Update())throw new Exception("Source update return");
            Bits(oracle.Real,state.Numeric.RealValue,"real");Bits(oracle.Previous,state.Numeric.PreviousValue,"previous");
            var actual=state.ReadNumber(!on.HasValue||on.Value>.5,channel,onOff,cam);
            var expected=oracle.GetOutputChannelData(channel);double expectedValue=17,actualValue=17;
            bool has=expected.GetDouble(ref expectedValue);
            if(has!=actual.HasValue)throw new Exception("Source empty/number channel mismatch");
            if(actual.HasValue)actualValue=actual.Value;Bits(expectedValue,actualValue,"output");
            bool boolean=true;bool converted=expected.GetBoolean(ref boolean);
            if(converted!=actual.HasValue||(converted&&boolean!=(actual.Value>.5)))throw new Exception("Source Boolean conversion");
            cases++;
        }
        return cases;
    }
}
