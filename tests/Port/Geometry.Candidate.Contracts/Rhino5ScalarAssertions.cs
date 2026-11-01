using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ScalarAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    public static string Run()
    {
        const ulong negativeUnset=0xffe5f8bd9e331dd2UL;
        var cases=new (ulong Bits,bool Valid)[]{(0,true),(0x8000000000000000,true),(1,true),(0x8000000000000001,true),(0x3ff0000000000000,true),(0xbff0000000000000,true),(0x7fefffffffffffff,true),(0xffefffffffffffff,true),(0x7ff0000000000000,false),(0xfff0000000000000,false),(0x7ff8000000000001,false),(0xfff8000000000001,false),(0x7ff0000000000001,false),(0xfff0000000000001,false),(negativeUnset,false),(negativeUnset-1,true),(negativeUnset+1,true),(negativeUnset&0x7fffffffffffffff,true)};
        foreach(var item in cases)Check(Rhino5ScalarValidity.IsValid(BitConverter.UInt64BitsToDouble(item.Bits))==item.Valid,"Original Rhino5 scalar classification differs");
        int patterns=0;
        foreach(ulong low in new[]{0UL,1UL,0x0000ffffffffffffUL})for(ulong word=0;word<=65535;word++)
        {
            ulong bits=(word<<48)|low;double value=BitConverter.UInt64BitsToDouble(bits);
            bool expected=double.IsFinite(value)&&value!=BitConverter.UInt64BitsToDouble(negativeUnset);
            Check(Rhino5ScalarValidity.IsValid(value)==expected,"Original finite/sentinel branch differs at "+bits.ToString("x16"));patterns++;
        }
        Check(patterns==196608,"Scalar pattern coverage stale");
        return "PASS original Rhino5 scalar validity:"+cases.Length+" native-branch boundary cases/"+patterns+" IEEE high-word patterns; negative unset/nonfinite reject and positive unset accepts; static byte semantics, original runtime/viewport parity separate.";
    }
}
