extern alias LegacyUndo;
using JewelDesign.Graph;
using Legacy = LegacyUndo::CS;
using LegacyObject = LegacyUndo::CS.CSObjects.CSObject;

namespace GraphContractChecks;

public static class UndoJournalAssertions
{
    public static string Run()
    {
        int steps = 0;
        var w = new World();
        w.Record(0); w.Undo(); w.Redo(); // no active transaction/buffers
        int first = w.Begin(); w.Record(0, " SETVALUEFROMUI "); w.Assign(0, 10);
        w.Record(0); w.Assign(0, 20); // one capture per object per transaction
        w.Record(1, "SetIdentifier"); w.Assign(1, 30);
        w.End(); w.Undo(first + 1); w.Undo(first); w.Redo();
        w.Undo(); w.Begin(); w.Record(0); w.Assign(0, 99); w.End(); w.Redo();
        steps += w.Steps;

        w = new World();
        w.Begin(); w.Begin(); w.Record(0); w.Assign(0, 1); w.End();
        w.Record(1); w.Assign(1, 2); w.End(); w.End(); w.Undo(); w.Redo();
        w.Begin(); w.End(); w.Begin(); w.Record(0); w.Assign(0, 5); w.Undo(); w.End();
        steps += w.Steps;

        w = new World();
        w.Begin(); w.Pause(true); w.Record(0); w.Assign(0, 7); w.End();
        w.Reset(); w.Begin(); // source Reset retains paused state
        w.Pause(false); w.Begin(); w.Record(0); w.Assign(0, 9); w.End();
        w.Lock(true, " Journal Test "); w.Begin(); w.Undo(); w.Redo();
        w.Pause(false); w.Record(0); w.Lock(false, "wrong");
        w.Lock(false, "journal test"); w.Undo(); w.Redo(); w.Reset();
        steps += w.Steps;

        w = new World();
        w.Begin(); w.ThrowCapture(true); w.Record(0); w.ThrowCapture(false);
        w.Record(0); w.Assign(0, 8); w.End();
        w.ThrowRestore(true); w.Undo(); w.ThrowRestore(false); w.Undo(); w.Redo();
        w.ReturnFalse(true); w.Undo(); w.Redo(); w.ReturnFalse(false);
        w.Begin(); w.NullRecord(); w.Record(0, null!); w.End();
        steps += w.Steps;

        w = new World();
        w.MissingSnapshot(true); w.Begin(); w.Record(0); w.Assign(0, 4); w.End();
        w.Undo(); w.Redo(); w.MissingSnapshot(false);
        w.Rename("Renamed"); w.Begin(); w.Record(0); w.Assign(0, 6); w.End();
        w.Undo(); w.Redo(); // same target reference after rename
        steps += w.Steps;

        w = new World();
        for (int i = 0; i < 1005; i++)
        {
            w.Begin(); w.Record(0); w.Assign(0, i + 1); w.End();
        }
        for (int i = 0; i < 1007; i++) w.Undo();
        for (int i = 0; i < 1007; i++) w.Redo();
        steps += w.Steps;

        return $"PASS undo journal: {steps} differential steps against source-linked CSUndoTable; reference identity, grouping, prune/pause/lock/branch and target failures; test targets only, no native object/PC claim.";
    }

    private sealed class Target : LegacyObject, IUndoTarget<int?>
    {
        public int Value;
        public string Name = "Duplicate";
        public Guid StoredGuid => Guid.Empty;
        public bool ThrowCapture, ThrowRestore, ReturnFalse, MissingSnapshot;
        private readonly List<string> trace;
        private readonly bool snapshot;
        public Target(List<string> trace, bool snapshot = false) { this.trace = trace; this.snapshot = snapshot; }
        public int? CaptureUndoState()
        {
            trace.Add($"capture:{Name}:{Value}");
            if (ThrowCapture) throw new InvalidOperationException("capture");
            return MissingSnapshot ? null : Value;
        }
        public bool RestoreUndoState(int? state)
        {
            trace.Add($"restore:{Name}:{state}");
            if (ThrowRestore) throw new InvalidOperationException("restore");
            if (!state.HasValue) return false;
            Value = state.Value;
            return !ReturnFalse;
        }
        public override LegacyObject CreateTempObject()
        {
            var state = CaptureUndoState();
            return state.HasValue ? new Target(trace, true) : null!;
        }
        public override bool SetUndo(LegacyObject source)
        {
            var state = (source as Target)?.Value;
            if (!snapshot) return RestoreUndoState(state);
            if (!state.HasValue) return false;
            Value = state.Value; return true;
        }
        // Deliberately hostile value equality: the journal must still use reference identity.
        public override bool Equals(object? obj) => obj is Target;
        public override int GetHashCode() => 0;
    }

    private sealed class World
    {
        private readonly Legacy.CSUndoTable original = new();
        private readonly UndoJournal<Target, int?> current = new();
        private readonly List<string> oldTrace = new(), newTrace = new();
        private readonly Target[] oldTargets, newTargets;
        private readonly DateTime started = DateTime.Now;
        public int Steps { get; private set; }
        public World()
        {
            oldTargets = new[] { new Target(oldTrace), new Target(oldTrace) };
            newTargets = new[] { new Target(newTrace), new Target(newTrace) };
        }
        private Target T(bool old, int index = 0) => (old ? oldTargets : newTargets)[index];
        private object? Step(Func<bool, object?> action)
        {
            (object? Value, Type? Error) Call(bool old)
            {
                try { return (action(old), null); }
                catch (Exception error) { return (null, error.GetType()); }
            }
            var expected = Call(true); var actual = Call(false);
            if (!Equals(expected, actual)) throw new Exception($"Undo outcome mismatch at step {Steps}: {expected} / {actual}");
            if (oldTargets.Where((t, i) => t.Value != newTargets[i].Value).Any()) throw new Exception("Undo target value mismatch");
            if (!oldTrace.SequenceEqual(newTrace)) throw new Exception("Undo capture/restore ordering mismatch");
            if (original.IsUndoBufferEmpty != current.IsUndoBufferEmpty || original.IsRedoBufferEmpty != current.IsRedoBufferEmpty
                || original.IsUIUpdateRequiredForLastAction != current.IsUIUpdateRequiredForLastAction) throw new Exception("Undo observable flags mismatch");
            foreach (var time in new[] { original.DateTimeOfFirstRedo, current.DateTimeOfFirstRedo })
                if (time.Kind != DateTimeKind.Local || time < started.AddSeconds(-1) || time > DateTime.Now.AddSeconds(1))
                    throw new Exception("Undo timestamp outside local execution window");
            Steps++; return actual.Value;
        }
        public int Begin() => (int)Step(old => old ? original.BeginUndoRecord("test") : current.BeginUndoRecord("test"))!;
        public void End() => Step(old => { if (old) original.EndUndoRecord("test"); else current.EndUndoRecord("test"); return null; });
        public void Record(int index, string source = "SetValue") => Step(old => old ? original.RecordObjectStateInUndoList(T(true, index), source) : current.RecordObjectStateInUndoList(T(false, index), source));
        public void NullRecord() => Step(old => old ? original.RecordObjectStateInUndoList(null!, "SetValue") : current.RecordObjectStateInUndoList(null!, "SetValue"));
        public void Assign(int index, int value) => Step(old => { T(old, index).Value = value; return null; });
        public void Undo(int number = -1) => Step(old => old ? original.UndoLast(number) : current.UndoLast(number));
        public void Redo() => Step(old => old ? original.RedoLast() : current.RedoLast());
        public void Pause(bool value) => Step(old => { if (old) original.Pause(value); else current.Pause(value); return null; });
        public void Reset() => Step(old => { if (old) original.Reset(); else current.Reset(); return null; });
        public void Lock(bool value, string text) => Step(old => old ? original.DisableAndLock(value, text) : current.DisableAndLock(value, text));
        public void ThrowCapture(bool value) => Step(old => { T(old).ThrowCapture = value; return null; });
        public void ThrowRestore(bool value) => Step(old => { T(old).ThrowRestore = value; return null; });
        public void ReturnFalse(bool value) => Step(old => { T(old).ReturnFalse = value; return null; });
        public void MissingSnapshot(bool value) => Step(old => { T(old).MissingSnapshot = value; return null; });
        public void Rename(string value) => Step(old => { T(old).Name = value; return null; });
    }
}
