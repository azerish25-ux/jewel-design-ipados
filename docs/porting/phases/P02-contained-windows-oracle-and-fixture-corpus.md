# P02 — Contained Windows oracle and fixture corpus

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Establish authoritative Windows behavior in a D:-contained environment and obtain representative PC projects.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P00.

**Owned/planned paths:** `reference/fixtures/`, `tests/Fixtures/`, `docs/evidence/reference/`. Paths not yet present are explicit future deliverables.

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

### P02-T01 — Inventory missing runtime, licensing, catalog state and user-created projects

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P00. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **inventory missing runtime, licensing, catalog state and user-created projects**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T01: <concrete result>`. One task passing does not complete its phase.

### P02-T02 — Prepare a contained Windows reference VM with its disk, profile and tools on D:

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **prepare a contained windows reference vm with its disk, profile and tools on d:**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T02: <concrete result>`. One task passing does not complete its phase.

### P02-T03 — Hash the original installers and distinguish altered loose-executable behavior

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **hash the original installers and distinguish altered loose-executable behavior**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T03: <concrete result>`. One task passing does not complete its phase.

### P02-T04 — Acquire the twenty planned fixture families with provenance and version labels

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **acquire the twenty planned fixture families with provenance and version labels**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T04: <concrete result>`. One task passing does not complete its phase.

### P02-T05 — Capture Windows open-edit-regenerate-save-reopen results and UI images

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **capture windows open-edit-regenerate-save-reopen results and ui images**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T05: <concrete result>`. One task passing does not complete its phase.

### P02-T06 — Publish oracle fields, geometry outputs, tolerances and failure cases

**Files:** use the owned paths above. Create `docs/porting/evidence/P02-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P02-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P02 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **publish oracle fields, geometry outputs, tolerances and failure cases**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P02-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P02-A1:** No VM/image/license/account is assumed available
- **P02-A2:** The public item-4 library fixture is not a PC portfolio oracle
- **P02-A3:** Do not use embedded credentials or execute vendor-account requests
- **P02-A4:** Each oracle records PC build, input hash, edit steps and expected outputs

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.
