# P04-T05-A01 handle-based legacy scalar file editing

Goal: connect the real library codec, session identities, recovered numeric setter decisions and portable undo journal for explicit scalar archive-data edits. Export rebuilt bytes; do not claim native CAD regeneration/application save.

Child A01.1: represent undecoded graph-header flags as nullable and identity resolution as explicitly undecoded. Opaque records retain type/version/raw bytes without generated replacement GUIDs. Known legacy numeric records have decoded flags and absent stored GUIDs. Project all slots without shifting indices.

Child A01.2: LegacyCsg4EditSession registers one numeric target per decoded record; exact Value handles select targets. Apply NumericValueState.PlanSetValue, record the scalar value at PreChange and update immutable archive state. Transactions use UndoJournal. Scalar capture is exact raw value; restore invokes the same numeric setter plan with recording naturally inactive during undo/redo. This does not emulate native CSObject.CreateTempObject, full metadata/CAM/flag restoration, UI gesture policy or kernel callbacks. Native capture behavior remains an original-PC-oracle gate.

Prerequisites: production codec/identity/numeric/journal child proofs passed on iPad/iPhone. P04/P03 parent gates remain. No native runtime or credential use.

Owned: GraphSnapshot/GraphIdentityScope metadata contracts; Documents graph projection and edit session; shared Documents/Graph contract tests, provenance/evidence/ADR and CI export/checker. Documents depends on the portable Graph project, which does not depend on Documents.

- [ ] RED projection tests: preserve 241 slots, decoded vs unknown metadata, no invented opaque GUID, raw bytes/links retained.
- [ ] GREEN explicit unknown-header model and record projection; run graph regressions.
- [ ] RED edit bridge: handle selection among duplicate labels, epsilon no-op, state sequence, undo/redo/grouping/branching, foreign/unknown field/opaque rejection before mutation, rebuilt edit/reopen/reversal, exports accepted by independent readers.
- [ ] GREEN typed scalar targets and coordinator; retain unsupported/native boundaries.
- [ ] Run combined/local/independent checks; review unknown-data loss and undo claims; commit/push.
- [ ] Inspect macOS iPad/iPhone results and record any CI startup failures honestly; commit/push evidence.
