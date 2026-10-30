using JewelDesign.Documents;
namespace DocumentContractChecks;

internal static class ValueGraphAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static double Number(LegacyChannelValue data){double value=123;if(!data.GetDouble(ref value))throw new Exception("Expected numeric data");return value;}
    private static void Throws<T>(Action action)where T:Exception
    {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    public static string Run(LegacyCsg4Document document,byte[] original)
    {
        int sourceCases=ValueChannelOracleAssertions.Run();
        Check(sourceCases==28160,"Missing source differential cases");
        var baseline=new LegacyValueGraphSession(document);baseline.Generate(2,6,8,9,10,11);
        Check(Number(baseline.ReadOutput(8,0))==4.38,"Real stored channel baseline");
        var selection=baseline.ReadOutput(6,3).Profile??throw new Exception("Expected evaluated profile reference");
        Check(selection.GroupRecordIndex==7&&selection.ProfileIndex==0,"Chooser channel forwarding");
        var wired=document.WithScalarInputLink(8,0,11,0).WithScalarInputLink(8,1,9,1);
        var session=new LegacyValueGraphSession(wired);session.Generate(8);
        Check(Number(session.ReadOutput(8,0))==-1.84,"Override source output/ordering");
        Check(session.ReadState(8).Numeric.PreviousValue==4.38,"Override must not invoke setter/undo transition");
        session.SetValue(11,0);session.Generate(8);
        var inactive=session.ReadOutput(8,0);double sentinel=99;
        Check(!inactive.GetDouble(ref sentinel)&&sentinel==99,"Inactive value not empty/preserved ref");
        session.SetValue(9,2.5);session.Generate(8);Check(session.ReadState(8).Numeric.RealValue==-2.5,"Inactive override was skipped");
        var latched=session.ExportCurrentDocument().WithScalarInputLink(8,1,-1,-5);
        var next=new LegacyValueGraphSession(latched);next.SetValue(11,1);next.Generate(8);
        Check(Number(next.ReadOutput(8,0))==-2.5,"Unlinked override must retain last updated real value");
        var reopened=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(latched));
        Check(reopened.Records[8].Scalar!.ObjectIndices[0]==11&&reopened.Records[8].Scalar!.Value==-2.5,"Rewired override save/reopen");
        var cycle=document.WithScalarInputLink(8,1,9,0).WithScalarInputLink(9,1,8,0);
        Throws<InvalidOperationException>(()=>new LegacyValueGraphSession(cycle).Generate(8));
        Throws<NotSupportedException>(()=>new LegacyValueGraphSession(document.WithScalarInputLink(8,1,0,0)).Generate(8));
        var off=new LegacyValueGraphSession(document.WithScalarInputLink(11,0,95,0));off.SetValue(95,.5);off.Generate(11);
        Check(!off.ReadOutput(11,3).HasNumber&&Number(off.ReadOutput(11,4))==0,"Inactive OnOff group/ordinary distinction");
        var nonBoolean=new LegacyValueGraphSession(document.WithScalarInputLink(11,0,7,0));nonBoolean.Generate(11);
        Check(Number(nonBoolean.ReadOutput(11,0))==1,"Failed profile-to-Boolean activation must default on");
        Check(Number(session.ReadOutput(9,-5))==2.5,"Negative numeric output fallback");
        var cam=new LegacyValueGraphSession(document);cam.SetValue(8,BitConverter.Int64BitsToDouble(long.MinValue));
        Check(BitConverter.DoubleToInt64Bits(Number(cam.ReadOutput(8,0)))==long.MinValue,"Normal mode lost negative zero");
        cam.IsCamValueMode=true;Check(BitConverter.DoubleToInt64Bits(Number(cam.ReadOutput(8,0)))==0,"CAM delta0 source arithmetic");
        cam.SetCamState(8,true,12,0);Check(Number(cam.ReadOutput(8,0))==12,"CAM absolute getter");
        Throws<NotSupportedException>(()=>cam.ExportCurrentDocument());
        var restored=wired.WithScalarInputLink(8,0,-1,-5).WithScalarInputLink(8,1,-1,-5);
        Check(LegacyCsg4Codec.Write(restored).AsSpan().SequenceEqual(original),"Link reversal changed original archive");
        return "PASS value graph channels:28160 source differential cases; activation/override generations, source output transformations, persistent real/previous state, stored chooser profile and rewired save/reopen/cycle/unsupported guards; known subset only.";
    }
}
