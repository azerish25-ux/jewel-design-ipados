using JewelDesign.Domain;
using JewelDesign.Graph;

namespace JewelDesign.Documents;

/// <summary>Handle-based scalar archive-data editing. Native object snapshots, UI and CAD evaluation are separate.</summary>
public sealed class LegacyCsg4EditSession
{
    private readonly Dictionary<GraphNodeIdentity, ScalarTarget> targets = new(ReferenceEqualityComparer.Instance);
    private readonly UndoJournal<ScalarTarget, double> journal = new();
    public LegacyCsg4Document Document { get; private set; }
    public GraphIdentityView View { get; }
    public IReadOnlyList<GraphParameterHandle> Parameters { get; }
    public bool CanUndo => !journal.IsUndoBufferEmpty;
    public bool CanRedo => !journal.IsRedoBufferEmpty;

    public LegacyCsg4EditSession(LegacyCsg4Document document)
    {
        ArgumentNullException.ThrowIfNull(document);
        Document = document;
        View = new GraphIdentityScope(LegacyCsg4GraphProjection.Capture(document)).InitialView;
        var handles = new List<GraphParameterHandle>();
        for (int i = 0; i < document.Records.Count; i++)
        {
            if (document.Records[i].Scalar is not { } scalar) continue;
            var handle = View.AddressParameter(i, "Value");
            targets.Add(handle.NodeIdentity, new ScalarTarget(this, i, scalar.Value));
            handles.Add(handle);
        }
        Parameters = Array.AsReadOnly(handles.ToArray());
    }

    public GraphParameterHandle GetParameter(int ordinal)
    {
        var handle = View.AddressParameter(ordinal, "Value");
        Resolve(handle); return handle;
    }

    public NumericValueState ReadValue(GraphParameterHandle handle) => Resolve(handle).State;
    public NumericValueEditPlan SetValue(GraphParameterHandle handle, double value) => Resolve(handle).Apply(value);
    public int BeginEditTransaction() => journal.BeginUndoRecord("FileEdit");
    public void EndEditTransaction() => journal.EndUndoRecord("FileEdit");
    public bool Undo(int transactionNumber = -1) => journal.UndoLast(transactionNumber);
    public bool Redo() => journal.RedoLast();
    public byte[] ExportArchiveData() => LegacyCsg4Codec.Write(Document);

    private ScalarTarget Resolve(GraphParameterHandle handle)
    {
        View.ResolveNode(handle); // validates null, scope and membership before any mutation
        if (!StringComparer.Ordinal.Equals(handle.FieldKey, "Value") || !targets.TryGetValue(handle.NodeIdentity, out var target))
            throw new NotSupportedException("Only decoded legacy scalar Value fields are editable in this session.");
        return target;
    }

    private sealed class ScalarTarget : IUndoTarget<double>
    {
        private readonly LegacyCsg4EditSession owner;
        private readonly int ordinal;
        public NumericValueState State { get; private set; }
        public ScalarTarget(LegacyCsg4EditSession owner, int ordinal, double value)
        {
            this.owner = owner; this.ordinal = ordinal; State = new NumericValueState(value, value);
        }

        public NumericValueEditPlan Apply(double value)
        {
            var plan = State.PlanSetValue(value);
            if (!plan.RequiresChange) return plan;
            // Prepare immutable file data before publishing state or recording history.
            var nextDocument = owner.Document.WithScalarValue(ordinal, plan.After.RealValue);
            State = plan.AtPreChange ?? throw new InvalidOperationException("Changed plan lacks PreChange state");
            owner.journal.RecordObjectStateInUndoList(this, "SetValue");
            owner.Document = nextDocument;
            State = plan.After;
            return plan;
        }

        // Exact scalar file-state capture; not the native temporary-object/SetUndo lifecycle.
        public double CaptureUndoState() => State.RealValue;
        public bool RestoreUndoState(double value) { Apply(value); return true; }
    }
}
