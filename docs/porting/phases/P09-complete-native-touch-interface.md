# P09 — Complete native touch interface

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Recreate every recovered UI workflow and state transition for iPad and iPhone.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P06, P08.

**Owned/planned paths:** `src/Port/Platform.iOS/`, `tests/UITests/`, `docs/evidence/ui/`. Paths not yet present are explicit future deliverables.

## Global constraints

- All CAD/rendering works offline on iPad/iPhone; no Windows server.
- Windows tools write only inside the D: repository, including auth, temp and caches.
- Preserve GUIDs, versions, unknown payloads and input files. Edit copies.
- Distinguish static observation, compilation, simulator, physical device and PC round trip.
- No embedded credential use, activation bypass, fake geometry or silent feature omission.

## Source entry points

- [BreadCrumbViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs)
- [MainViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs)
- [TileSearchMainViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs)
- [TileSearchResultViewModelBase.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs)
- [TileViewViewModelBase.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs)
- [DesignStackViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs)
- [CustomizeViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizeViewModel.cs)
- [CustomizePartsListViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs)
- [FreehandStackViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs)
- [TileViewPartsViewModel.cs](../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs)

## Interfaces and handoffs

Consume [architecture contracts](../ARCHITECTURE.md). Public boundary changes require an ADR and regression case in the same task. Use [the evidence template](../templates/task-evidence.json) to record exact changed paths, fixture hashes, commands, expected/actual results and downstream contracts. A chat summary is not an artifact.

### P09-T01 — Map 68 UI groups, 317 public commands, 113 XAML files and 771 hookups

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P06, P08. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **map 68 ui groups, 317 public commands, 113 xaml files and 771 hookups**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T01: <concrete result>`. One task passing does not complete its phase.

### P09-T02 — Implement native navigation, selection, property editing and catalog surfaces in CSharp/UIKit

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P09-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement native navigation, selection, property editing and catalog surfaces in csharp/uikit**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T02: <concrete result>`. One task passing does not complete its phase.

### P09-T03 — Implement customize/freehand, dialogs, validation, orders and saved designs

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P09-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement customize/freehand, dialogs, validation, orders and saved designs**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T03: <concrete result>`. One task passing does not complete its phase.

### P09-T04 — Resolve dormant commands, empty handlers and ambiguous bindings with reference evidence

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P09-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **resolve dormant commands, empty handlers and ambiguous bindings with reference evidence**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T04: <concrete result>`. One task passing does not complete its phase.

### P09-T05 — Implement undo/redo, gestures, appropriate Pencil/keyboard support, accessibility and localization

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P09-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement undo/redo, gestures, appropriate pencil/keyboard support, accessibility and localization**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T05: <concrete result>`. One task passing does not complete its phase.

### P09-T06 — Run interaction and screenshot cases across device sizes and orientations

**Files:** use the owned paths above. Create `docs/porting/evidence/P09-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P09-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P09 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **run interaction and screenshot cases across device sizes and orientations**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P09-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P09-A1:** Every command/event maps to a test or an explicit original-source gap
- **P09-A2:** No static placeholder panel or empty action satisfies a workflow
- **P09-A3:** Core flows work offline with state restoration and actionable errors
- **P09-A4:** Functionality survives compact layouts; literal desktop pixels/mouse behavior are not presumed

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.
