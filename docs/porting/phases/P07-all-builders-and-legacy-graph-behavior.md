# P07 — All builders and legacy graph behavior

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Port every declared builder identity and legacy behavior in bounded auditable batches.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P06.

**Owned/planned paths:** `src/Port/Features/Builders/`, `src/Port/Graph/`, `tests/Port/Builders.Tests/`. Paths not yet present are explicit future deliverables.

## Global constraints

- All CAD/rendering works offline on iPad/iPhone; no Windows server.
- Windows tools write only inside the D: repository, including auth, temp and caches.
- Preserve GUIDs, versions, unknown payloads and input files. Edit copies.
- Distinguish static observation, compilation, simulator, physical device and PC round trip.
- No embedded credential use, activation bypass, fake geometry or silent feature omission.

## Source entry points

- [CSApplication.cs](../../../recovery/canonical/cad/CS/CSApplication.cs)
- [CSBlockDescription.cs](../../../recovery/canonical/cad/CS.Parametric/CSBlockDescription.cs)
- [CSParaGraph.cs](../../../recovery/canonical/cad/CS/CSParaGraph.cs)
- [CSNullBlock.cs](../../../recovery/canonical/cad/CSBlocks/CSNullBlock.cs)
- [CSValue.cs](../../../recovery/canonical/cad/CS.CSValues/CSValue.cs)
- [CSScript.cs](../../../recovery/canonical/cad/CS.CSValues/CSScript.cs)
- [CSValueXFormObject.cs](../../../recovery/canonical/cad/CS.CSValues.CSValueHandle/CSValueXFormObject.cs)
- [CSValueXFormObject1.cs](../../../recovery/canonical/cad/CS.CSValues.CSValueHandle.Obsolete/CSValueXFormObject1.cs)
- [CSRingRail.cs](../../../recovery/canonical/cad/CSBlocks.Rails/CSRingRail.cs)
- [CSRingRailCustom.cs](../../../recovery/canonical/cad/CSBlocks.Rails/CSRingRailCustom.cs)

## Interfaces and handoffs

Consume [architecture contracts](../ARCHITECTURE.md). Public boundary changes require an ADR and regression case in the same task. Use [the evidence template](../templates/task-evidence.json) to record exact changed paths, fixture hashes, commands, expected/actual results and downstream contracts. A chat summary is not an artifact.

### P07-T01 — Partition all 619 builder rows by namespace, dependency family and legacy status

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P06. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **partition all 619 builder rows by namespace, dependency family and legacy status**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T01: <concrete result>`. One task passing does not complete its phase.

### P07-T02 — Map a bounded batch of input/output channels and original algorithms

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **map a bounded batch of input/output channels and original algorithms**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T02: <concrete result>`. One task passing does not complete its phase.

### P07-T03 — Port current and legacy implementations with stable GUID/version dispatch

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **port current and legacy implementations with stable guid/version dispatch**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T03: <concrete result>`. One task passing does not complete its phase.

### P07-T04 — Test limits, non-defaults, invalid geometry and parameter combinations

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **test limits, non-defaults, invalid geometry and parameter combinations**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T04: <concrete result>`. One task passing does not complete its phase.

### P07-T05 — Test mixed legacy/current graphs, ordering, invalidation and cancellation

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **test mixed legacy/current graphs, ordering, invalidation and cancellation**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T05: <concrete result>`. One task passing does not complete its phase.

### P07-T06 — Attach fixtures, Windows comparisons, iOS results and review to each builder row

**Files:** use the owned paths above. Create `docs/porting/evidence/P07-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P07 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **attach fixtures, windows comparisons, ios results and review to each builder row**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P07-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P07-A1:** Every builder is verified or explicitly blocked
- **P07-A2:** Do not silently replace old algorithms with their newest variants
- **P07-A3:** A class declaration or constructor registration is insufficient
- **P07-A4:** Reserved/expired nodes preserve expected error behavior

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.
