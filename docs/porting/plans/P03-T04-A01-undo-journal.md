# P03-T04-A01 portable undo journal

Goal: port the recovered transaction/history scheduler to a real portable journal with typed target snapshots. Preserve nesting, reference identity, recording deduplication, pause/disable/lock behavior, transaction numbers, redo invalidation, pruning, source-based UI-update flags and callback failure ordering.

Ready prerequisites: P03-T01 portable projects and P03-T02 classification are complete. This independent child needs object reference identity, not general units or persistent file GUID mapping. P03-T04 remains incomplete and retains its P03-T03 parent dependency for full parameter integration. Native object capture/restore and CAD/UI integration are separate work.

Source: recovery/canonical/cad/CS/CSUndoTable.cs SHA256 01fda0a8f0a522c090b1246220d374a5e3280788ca6c48a6e630a722ddd5729f and CUndoItem.cs. Original scheduling source is linked directly into a test-only project with a minimal abstract target contract; no Rhino/native runtime is loaded. Production adapts CSObject/CreateTempObject/SetUndo to IUndoTarget<TState>.CaptureUndoState/RestoreUndoState. Snapshot ownership remains a target adapter responsibility.

Owned files: src/Port/Graph/UndoJournal.cs, src/Port/Graph/IUndoTarget.cs; tests/Port/LegacyUndoHarness/ project; tests/Port/Graph.Contracts/UndoJournalAssertions.cs; phase/evidence/classification and shared graph test entry point.

Acceptance P03-A1/A2: no new platform references; differential action sequences compare outcomes, target values, callback order, buffers and UI-update flags with the original scheduler. Cover normal/nested/empty transactions, duplicate labels/GUIDs, repeated capture, branch editing, pause/reset/lock, explicit undo numbers, >1000 entries, null arguments and capture/restore exceptions. Preserve unexpected source behavior as evidence rather than silently redesigning it. Timestamps receive local-time/window checks; no deterministic exact timestamp claim.

- [x] Write source-linked oracle and failing Graph.Tests cases before production API.
- [x] Adapt the scheduler; implement genuine history storage and target restore calls. Omit only the source diagnostic methods whose entire output is an empty debug-only callback; document that scope.
- [x] Run differential/adjacent tests, integrity and source checks; review partial failures and target identity.
- [x] Commit/push; run macOS iPad/iPhone and inspect both artifacts.
- [x] Commit/push scoped results. Do not close full undo/native-object or PC parity rows.
