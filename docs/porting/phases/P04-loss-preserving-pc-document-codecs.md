# P04 — Loss-preserving PC document codecs

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Read and write portfolios, CSG graphs and legacy records while retaining unknown data and editable identities.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P02, P03.

**Owned/planned paths:** `src/Port/Documents/`, `tests/Port/Documents.Tests/`, `reference/file-proof/`. Paths not yet present are explicit future deliverables.

## Global constraints

- All CAD/rendering works offline on iPad/iPhone; no Windows server.
- Windows tools write only inside the D: repository, including auth, temp and caches.
- Preserve GUIDs, versions, unknown payloads and input files. Edit copies.
- Distinguish static observation, compilation, simulator, physical device and PC round trip.
- No embedded credential use, activation bypass, fake geometry or silent feature omission.

## Source entry points

- [AssemblyInfo.cs](../../../recovery/canonical/desktop/properties/AssemblyInfo.cs)
- [CSDocument.cs](../../../recovery/canonical/cad/CS/CSDocument.cs)
- [Project.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs)
- [Design.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs)
- [ProjectManagementHelper.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/ProjectManagementHelper.cs)
- [BackupHelper.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/BackupHelper.cs)
- [CSUtilIO.cs](../../../recovery/canonical/cad/CS/CSUtilIO.cs)
- [CSObject.cs](../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs)
- [JSonDocumentWriter.cs](../../../recovery/canonical/cad/CS.JSonDoc/JSonDocumentWriter.cs)
- [JSonDocumentReader.cs](../../../recovery/canonical/cad/CS.JSonDoc/JSonDocumentReader.cs)

## Interfaces and handoffs

Consume [architecture contracts](../ARCHITECTURE.md). Public boundary changes require an ADR and regression case in the same task. Use [the evidence template](../templates/task-evidence.json) to record exact changed paths, fixture hashes, commands, expected/actual results and downstream contracts. A chat summary is not an artifact.

### P04-T01 — Create Documents and Documents.Tests with an independent fixture reader

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02, P03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **create documents and documents.tests with an independent fixture reader**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T01: <concrete result>`. One task passing does not complete its phase.

### P04-T02 — Implement bounded envelopes, chunk tables, CRC and record version handling

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P04-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement bounded envelopes, chunk tables, crc and record version handling**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T02: <concrete result>`. One task passing does not complete its phase.

### P04-T03 — Implement portfolio XML, paired GUID filenames, sidecars, previews and history

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P04-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement portfolio xml, paired guid filenames, sidecars, previews and history**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T03: <concrete result>`. One task passing does not complete its phase.

### P04-T04 — Map all 654 serializer identities and inherited reader/writer/version behavior

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P04-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **map all 654 serializer identities and inherited reader/writer/version behavior**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T04: <concrete result>`. One task passing does not complete its phase.

### P04-T05 — Implement no-op save, one-parameter edits and explicit migration policy

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P04-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement no-op save, one-parameter edits and explicit migration policy**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T05: <concrete result>`. One task passing does not complete its phase.

### P04-T06 — Verify edited PC-to-iOS-to-PC reopening and fail-closed future formats

**Files:** use the owned paths above. Create `docs/porting/evidence/P04-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P04-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P04 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **verify edited pc-to-ios-to-pc reopening and fail-closed future formats**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P04-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P04-A1:** Item-4 edit 4.38 to 4.48 preserves 240 other records and 73 other numbers
- **P04-A2:** No-op preservation, unknown nodes, corrupt CRC, truncation and duplicate labels are tested
- **P04-A3:** Cached volume/preview retention is not geometry regeneration
- **P04-A4:** Unsupported future records never cause silent data removal or overwrite

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.

## Independent P04-T01-A01

[Legacy library codec plan](../plans/P04-T01-A01-legacy-csg4-codec.md) consumes the existing hash-locked library fixture and portable foundations. It can progress without a Windows oracle, while full P04 retains P02/P03 and PC/native parity gates. [ADR 0005](../ADR-0005-legacy-csg4-codec.md) specifies the supported profiles, opaque coverage and absence of regeneration.

## Independent P04-T05-A01 scalar edit bridge

[Bridge plan](../plans/P04-T05-A01-legacy-edit-bridge.md) consumes the proven legacy codec/identity/numeric/journal child outputs. It establishes archive-data edits, not full native capture, all-serializer migration or application save. [ADR 0006](../ADR-0006-legacy-scalar-edit-bridge.md) defines unknown metadata and the explicit scalar adapter boundary. Parent dependencies remain in force for full task completion.
