# P05 — Offline CAD backend and native bridge

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Prove a real on-device geometry backend against the recovered operation requirements.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P02, P03.

**Owned/planned paths:** `native/GeometryBridge/`, `src/Port/Geometry/`, `tests/Port/Geometry.Tests/`, `docs/decisions/`. Paths not yet present are explicit future deliverables.

## Global constraints

- All CAD/rendering works offline on iPad/iPhone; no Windows server.
- Windows tools write only inside the D: repository, including auth, temp and caches.
- Preserve GUIDs, versions, unknown payloads and input files. Edit copies.
- Distinguish static observation, compilation, simulator, physical device and PC round trip.
- No embedded credential use, activation bypass, fake geometry or silent feature omission.

## Source entry points

- [Recovery map](../../../recovery/README.md)
- [Current evidence](../CURRENT_STATE.md)

## Interfaces and handoffs

Consume [architecture contracts](../ARCHITECTURE.md). Public boundary changes require an ADR and regression case in the same task. Use [the evidence template](../templates/task-evidence.json) to record exact changed paths, fixture hashes, commands, expected/actual results and downstream contracts. A chat summary is not an artifact.

### P05-T01 — Map all 315 kernel/wrapper call families to required semantics

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02, P03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **map all 315 kernel/wrapper call families to required semantics**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T01: <concrete result>`. One task passing does not complete its phase.

### P05-T02 — Evaluate iOS-capable kernels using actual sweeps, trims, booleans, meshing and archive fixtures

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P05-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **evaluate ios-capable kernels using actual sweeps, trims, booleans, meshing and archive fixtures**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T02: <concrete result>`. One task passing does not complete its phase.

### P05-T03 — Record the backend decision with distribution/license prerequisites and gaps

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P05-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **record the backend decision with distribution/license prerequisites and gaps**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T03: <concrete result>`. One task passing does not complete its phase.

### P05-T04 — Implement a versioned C ABI with opaque handles, ownership and typed errors

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P05-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement a versioned c abi with opaque handles, ownership and typed errors**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T04: <concrete result>`. One task passing does not complete its phase.

### P05-T05 — Implement CSharp adapters for representative curve/surface/solid/mesh operations

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P05-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement csharp adapters for representative curve/surface/solid/mesh operations**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T05: <concrete result>`. One task passing does not complete its phase.

### P05-T06 — Compare PC/iOS topology, units, orientation, tolerances, cancellation and memory

**Files:** use the owned paths above. Create `docs/porting/evidence/P05-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P05-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P05 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **compare pc/ios topology, units, orientation, tolerances, cancellation and memory**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P05-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P05-A1:** Missing operations remain blocked; dummy meshes and success-returning stubs cannot pass
- **P05-A2:** No network geometry compute is permitted
- **P05-A3:** Independent CAD SDK source is not claimed as recovered code
- **P05-A4:** Repeated failure, cancellation and handle disposal do not leak memory

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.

## Independent P05-T02-A01 candidate experiment

[Native experiment plan](../plans/P05-T02-A01-native-candidate.md) consumes a source-backed operation subset and pinned public candidate. It does not waive the full 315-family/PC oracle or backend-selection gates. [ADR 0009](../ADR-0009-native-cad-candidate.md) records genuine analytical fixtures and unverified distribution/native lifecycle requirements.
