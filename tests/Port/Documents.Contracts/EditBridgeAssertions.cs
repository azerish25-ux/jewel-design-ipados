using JewelDesign.Documents;
using JewelDesign.Graph;

namespace DocumentContractChecks;

internal static class EditBridgeAssertions
{
    public static string Run(LegacyCsg4Document document, byte[] original)
    {
        var editor = new LegacyCsg4EditSession(document);
        if (editor.Parameters.Count != 74 || editor.View.Snapshot.Nodes.Count != 241) throw new Exception("Editor lost record/parameter slots");
        var handle = editor.GetParameter(8);
        var duplicateOrdinal = document.Records.Select((r, i) => (r, i)).First(x => x.i != 8 && x.r.Numeric?.Identifier == "Top Thickness").i;
        var duplicate = editor.GetParameter(duplicateOrdinal);
        if (handle.Equals(duplicate)) throw new Exception("Duplicate labels collapsed in editor");
        double otherValue = editor.ReadValue(duplicate).RealValue;
        int transaction = editor.BeginEditTransaction();
        if (editor.SetValue(handle, 4.380001).RequiresChange || editor.CanUndo) throw new Exception("Tiny no-op acquired undo work");
        var plan = editor.SetValue(handle, 4.48);
        if (!plan.RequiresChange || editor.ReadValue(handle).RealValue != 4.48 || editor.ReadValue(handle).PreviousValue != 4.38) throw new Exception("Applied numeric state sequence");
        editor.EndEditTransaction();
        if (!editor.CanUndo || editor.CanRedo || editor.ReadValue(duplicate).RealValue != otherValue) throw new Exception("Edit grouping/duplicate target isolation");
        var edited = editor.ExportArchiveData();
        if (Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(edited)).ToLowerInvariant() != "26b6ef6e22f2a6d9ff131350f18b6951818824291de1b1a2ae8d000d85c07b1a") throw new Exception("Handle edit differs from independently verified file proof");
        if (LegacyCsg4Codec.Read(edited).Records[8].Numeric!.Value != 4.48) throw new Exception("Handle edit not serialized");
        if (editor.Undo(transaction + 1)) throw new Exception("Wrong numbered undo accepted");
        if (!editor.Undo(transaction) || editor.ReadValue(handle).PreviousValue != 4.48) throw new Exception("Scalar undo setter state");
        Equal(original, editor.ExportArchiveData(), "Undo did not restore archive");
        if (!editor.Redo()) throw new Exception("Redo missing");
        Equal(edited, editor.ExportArchiveData(), "Redo changed archive");
        if (editor.ReadValue(handle).PreviousValue != 4.38) throw new Exception("Redo scalar previous state");

        var foreign = new LegacyCsg4EditSession(document).GetParameter(8);
        Throws<ArgumentException>(() => editor.SetValue(foreign, 5));
        Throws<NotSupportedException>(() => editor.SetValue(handle.NodeIdentity.AddressParameter("minValue"), 5));
        Throws<NotSupportedException>(() => editor.SetValue(editor.View.AddressParameter(0, "Value"), 5));
        Throws<NotSupportedException>(() => editor.GetParameter(0));
        Throws<ArgumentOutOfRangeException>(() => editor.GetParameter(-1));
        Throws<ArgumentNullException>(() => editor.SetValue(null!, 5));
        Throws<ArgumentNullException>(() => new LegacyCsg4EditSession(null!));
        Throws<NotSupportedException>(() => ((IList<GraphParameterHandle>)editor.Parameters)[0] = duplicate);
        Equal(edited, editor.ExportArchiveData(), "Rejected edit mutated document");
        editor.Undo(); editor.BeginEditTransaction(); editor.SetValue(handle, 4.58); editor.EndEditTransaction();
        if (editor.CanRedo || editor.Redo()) throw new Exception("New branch retained redo");

        var grouped = new LegacyCsg4EditSession(document);
        grouped.BeginEditTransaction(); grouped.BeginEditTransaction();
        grouped.SetValue(grouped.GetParameter(8), 4.48); grouped.EndEditTransaction();
        grouped.SetValue(grouped.GetParameter(8), 4.58);
        grouped.SetValue(grouped.GetParameter(duplicateOrdinal), otherValue + .1); grouped.EndEditTransaction();
        if (!grouped.Undo()) throw new Exception("Grouped undo missing");
        Equal(original, grouped.ExportArchiveData(), "Nested multi-target undo");
        grouped.Redo();
        if (grouped.ReadValue(grouped.GetParameter(8)).RealValue != 4.58 || grouped.ReadValue(grouped.GetParameter(duplicateOrdinal)).RealValue != otherValue + .1) throw new Exception("Grouped redo lost fields");

        var direct = new LegacyCsg4EditSession(document);
        direct.SetValue(direct.GetParameter(8), 4.48);
        if (direct.CanUndo) throw new Exception("Setter silently began a transaction");
        var tiny = new LegacyCsg4EditSession(document.WithNumericValue(8, .000001));
        tiny.BeginEditTransaction(); tiny.SetValue(tiny.GetParameter(8), 4.48); tiny.EndEditTransaction(); tiny.Undo();
        if (tiny.ReadValue(tiny.GetParameter(8)).RealValue != .000001) throw new Exception("Exact file scalar capture lost tiny input");
        return "PASS library edit session: real handles, setter epsilon/previous state, grouped scalar undo/redo, branch/foreign/opaque guards and rebuilt archive reopening; no native snapshot or CAD claim.";
    }

    private static void Equal(byte[] expected, byte[] actual, string message) { if (!expected.AsSpan().SequenceEqual(actual)) throw new Exception(message); }
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; } throw new Exception("Expected " + typeof(T).Name);
    }
}
