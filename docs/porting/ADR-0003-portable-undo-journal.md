# ADR 0003: portable undo scheduling with typed target snapshots

Status: accepted for P03-T04-A01.

UndoJournal<TTarget,TState> adapts canonical CSUndoTable scheduling to IUndoTarget<TState>. The journal stores real undo/redo entries, reference-identifies targets, groups nested transactions, captures state and invokes restore during undo/redo. Type/label/GUID equality does not deduplicate distinct targets. A complete isolated snapshot and faithful restore are obligations of the target adapter.

The scheduling body is adapted from CSUndoTable; CUndoItem native object construction and SetUndo are replaced by CaptureUndoState/RestoreUndoState. Recovered CreateTempObject uses reflection, graph registration/removal and undo pause side effects; CSObject/CSBlock/CSValue.SetUndo restore additional fields and invoke setters. None of those native object behaviors is implemented by this generic boundary. Tests use explicitly isolated integer-state targets. No application-native undo, graph regeneration, GUID migration or UI parity row is closed.

Preserved rules include reference deduplication within the current transaction, nested number reuse, clearing redo on begin, source-name-based UI-update flags, pruning only when BeginUndoRecord sees more than 1000 entries, resetting transaction level before undo/redo, strict latest-transaction matching and non-atomic failures. Exceptions propagate with the original scheduling order; false restore results are ignored as in the source. Reset does not clear the paused flag in the recovered code; that observed behavior is preserved and tested. The passphrase operation only locks this undo journal and is unrelated to account activation. UI-thread confinement and snapshot lifetime belong to the host.

Only two DEBUG methods are omitted: their recovered bodies enumerate diagnostic groupings into empty callbacks and produce no result or observable state change. No user action or undo operation is replaced with an empty stub.

LegacyUndoHarness compiles the canonical CSUndoTable.cs and CUndoItem.cs unchanged against a test-only abstract CSObject boundary. Recovery hash checks protect those inputs. Graph.Contracts runs identical action sequences through that table and the production journal, comparing return/exception types, restored values, callback trace, buffer flags and UI flags after every step. It covers 6130 steps, duplicate identities with deliberately hostile value equality, >1000 entries, missing snapshots, null inputs and failures. Timestamp checks cover local kind/execution window, not exact wall-clock equality. The oracle is recovered source with isolated targets, not original PC/native execution.

This independent child needs P03-T01/T02, not general units or persistent file IDs. Parent P03-T04 still depends on completion of P03-T03 and requires actual parameter/native adapters, validation and UI gesture transaction integration.

## Native snapshot follow-up

CSDocument owns the CSUndoTable. CUndoItem creates a temporary target, then calls SetUndo on it. CSObject.CreateTempObject changes global undo pause state and removes the temporary object from the graph; CSValue.SetUndo can invoke SetValue, which calls PreChange and records through the document undo table. This static call sequence requires native lifecycle/reentrancy observation before adapting full object snapshots. The counter targets in this suite do not model that lifecycle. Do not conclude that the original application has a recursion bug, or add a blanket suppression policy, solely from this isolated scheduler proof.
