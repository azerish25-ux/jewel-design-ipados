using JewelDesign.Documents;
namespace DocumentContractChecks;

internal static class AdditionalScalarEditAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Equal(byte[] a,byte[] b,string message)=>Check(a.AsSpan().SequenceEqual(b),message);
    private static void Throws<T>(Action action) where T:Exception
    {try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    internal static string Run(LegacyCsg4Document document,byte[] original)
    {
        var editor=new LegacyCsg4EditSession(document);
        Check(editor.Parameters.Count==100,"Decoded scalar handles missing");
        int[] indices={6,11,95,8};double[] before={0,1,.5,4.38},after={1,0,.75,4.48};
        var handles=indices.Select(editor.GetParameter).ToArray();
        for(int i=0;i<indices.Length;i++)Check(editor.ReadValue(handles[i]).RealValue==before[i],"Mixed scalar fixture baseline");
        var initialView=editor.View;int transaction=editor.BeginEditTransaction();
        Check(!editor.SetValue(handles[1],1.000001).RequiresChange&&!editor.CanUndo,"Plain scalar tiny no-op");
        var expected=document;
        for(int i=0;i<indices.Length;i++){
            Check(editor.SetValue(handles[i],after[i]).RequiresChange,"Mixed scalar change missing");
            Check(editor.ReadValue(handles[i]).PreviousValue==before[i],"Plain scalar previous-state plan");
            expected=expected.WithScalarValue(indices[i],after[i]);
        }
        editor.EndEditTransaction();
        Check(ReferenceEquals(editor.View,initialView),"Value edit replaced initial address/provenance view");
        var bytes=editor.ExportArchiveData();Equal(LegacyCsg4Codec.Write(expected),bytes,"Mixed handle edit differs from verified structural codec");
        var reopened=LegacyCsg4Codec.Read(bytes);
        for(int i=0;i<indices.Length;i++)Check(reopened.Records[indices[i]].Scalar!.Value==after[i],"Mixed saved value lost");
        for(int i=0;i<document.Records.Count;i++)if(!indices.Contains(i))Equal(document.Records[i].CopyOriginalRecordBytes(),reopened.Records[i].CopyOriginalRecordBytes(),"Mixed edit changed another record");
        Check(editor.Undo(transaction),"Mixed transaction undo missing");Equal(original,editor.ExportArchiveData(),"Mixed scalar undo not byte-identical");
        Check(editor.Redo(),"Mixed scalar redo missing");Equal(bytes,editor.ExportArchiveData(),"Mixed scalar redo changed serialization");
        var foreign=new LegacyCsg4EditSession(document).GetParameter(11);
        Throws<ArgumentException>(()=>editor.SetValue(foreign,1));
        Throws<NotSupportedException>(()=>editor.GetParameter(0));
        Equal(bytes,editor.ExportArchiveData(),"Rejected mixed edit changed data");
        editor.Undo();editor.BeginEditTransaction();editor.SetValue(editor.GetParameter(11),.125);editor.EndEditTransaction();
        Check(editor.Document.Records[11].Scalar!.Value==.125&&!editor.CanRedo,"Generic stored scalar was coerced to Boolean or retained redo");
        Check(editor.Undo(),"Plain scalar branch undo missing");Equal(original,editor.ExportArchiveData(),"Plain scalar branch undo");
        return "PASS additional scalar handles:100 scoped fields, decoded header/links, mixed Range/Chooser/OnOff/Value transactions, save/reopen and byte-identical undo/redo; generic stored state only, no UI/graph/CAD/PC claim.";
    }
}
