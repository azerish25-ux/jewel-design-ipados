# P14 — Full interoperability and feature acceptance

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Close every required feature/type row with reproducible PC/iOS/PC evidence.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P07, P08, P09, P10, P11, P12, P13.

**Owned/planned paths:** `tests/Interop/`, `docs/porting/backlog/`, `docs/evidence/parity/`. Paths not yet present are explicit future deliverables.

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

### P14-T01 — Run the twenty-family corpus across supported PC versions and record branches

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P07, P08, P09, P10, P11, P12, P13. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **run the twenty-family corpus across supported pc versions and record branches**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T01: <concrete result>`. One task passing does not complete its phase.

### P14-T02 — Exercise no-op and edited interchanges with histories, scripts and sidecars

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P14-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **exercise no-op and edited interchanges with histories, scripts and sidecars**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T02: <concrete result>`. One task passing does not complete its phase.

### P14-T03 — Compare graph identity, parameters, geometry, units, materials, previews and exports separately

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P14-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **compare graph identity, parameters, geometry, units, materials, previews and exports separately**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T03: <concrete result>`. One task passing does not complete its phase.

### P14-T04 — Exercise corrupt data, unknown nodes, unsupported versions and mixed legacy graphs

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P14-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **exercise corrupt data, unknown nodes, unsupported versions and mixed legacy graphs**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T04: <concrete result>`. One task passing does not complete its phase.

### P14-T05 — Audit all UI/CAD/PC/serializer/builder rows for stale or missing evidence

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P14-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **audit all ui/cad/pc/serializer/builder rows for stale or missing evidence**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T05: <concrete result>`. One task passing does not complete its phase.

### P14-T06 — Publish the supported-version matrix and unresolved blockers

**Files:** use the owned paths above. Create `docs/porting/evidence/P14-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P14-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P14 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **publish the supported-version matrix and unresolved blockers**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P14-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P14-A1:** 654 serializer rows and 619 builder rows overlap; they are not independent feature counts
- **P14-A2:** No row is verified solely by a class declaration or build
- **P14-A3:** Offline adaptations and unavailable services remain visibly distinct from parity
- **P14-A4:** A reviewer can reproduce each accepted row from its fixture and command

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.
