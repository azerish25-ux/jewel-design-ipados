# P10 — Catalog pricing projects quotes and reports

> For agentic workers: read [AGENT_PLAYBOOK](../AGENT_PLAYBOOK.md) and root AGENTS.md. Execute one task and its verification at a time. Delegation requires user authorization.

**Goal:** Implement local data and commerce workflows while accounting explicitly for server-only capabilities.

**Architecture:** Reuse CSharp logic through real graph, document and geometry boundaries. Recovered snapshots are read-only references.

**Tech stack:** C#/.NET iOS/UIKit; a real native CAD backend and Metal where needed. Backend selection is gated by P05.

**Dependencies:** P03, P09.

**Owned/planned paths:** `src/Port/Features/Catalog/`, `src/Port/Features/Commerce/`, `tests/Port/Catalog.Tests/`. Paths not yet present are explicit future deliverables.

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

### P10-T01 — Map schemas, profiles, catalog/component relationships and asset references

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T01.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P03, P09. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **map schemas, profiles, catalog/component relationships and asset references**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T01: <concrete result>`. One task passing does not complete its phase.

### P10-T02 — Implement local ingestion, indexing, filters, categories and component availability

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T02.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P10-T01. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement local ingestion, indexing, filters, categories and component availability**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T02: <concrete result>`. One task passing does not complete its phase.

### P10-T03 — Implement project/customer administration, portfolios, quotes and reports

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T03.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P10-T02. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement project/customer administration, portfolios, quotes and reports**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T03: <concrete result>`. One task passing does not complete its phase.

### P10-T04 — Implement reproducible price snapshots, options, units, rounding and currency

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T04.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P10-T03. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **implement reproducible price snapshots, options, units, rounding and currency**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T04: <concrete result>`. One task passing does not complete its phase.

### P10-T05 — Record live supplier/account/service actions as explicit scope decisions

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T05.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P10-T04. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **record live supplier/account/service actions as explicit scope decisions**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T05: <concrete result>`. One task passing does not complete its phase.

### P10-T06 — Compare offline, stale/missing data, migration and report output to PC fixtures

**Files:** use the owned paths above. Create `docs/porting/evidence/P10-T06.json` and the focused test or analysis artifact named in that record. Never edit recovery/ to make a port test pass.

**Consumes:** P10-T05. **Produces:** a reviewed implementation or analysis artifact for this task with its exact verification command.

- [ ] Read matching source bodies and run `node scripts/porting.cjs backlog P10 10`. Record fields, defaults, error paths and dependencies under the selected requirement IDs.
- [ ] Define normal, boundary and failure cases from the mandatory acceptance contract below. Add the failing test first and run it. Acquisition/analysis tasks instead validate the currently missing or incorrect artifact fields; do not invent a meaningless unit test.
- [ ] Implement **compare offline, stale/missing data, migration and report output to pc fixtures**. Preserve reusable algorithms. Split into stable child task IDs before coding if multiple independently reviewable changes are needed. Missing prerequisites produce a blocker, never a fake pass.
- [ ] Run focused platform/codec/kernel tests, adjacent regressions and `node tools/repository/check.cjs`. Record fixture SHA256, exit codes, assertions, output paths and warnings. The repository checker alone never proves parity.
- [ ] Review source mutation, credential leakage, lost fields, swallowed errors and placeholder actions. Update matching backlog rows and commit with `P10-T06: <concrete result>`. One task passing does not complete its phase.

## Mandatory acceptance cases

- **P10-A1:** No complete account-specific catalog is assumed recovered
- **P10-A2:** Offline substitutions are not silently labeled identical live services
- **P10-A3:** No embedded credential, fake activation or entitlement bypass is used
- **P10-A4:** Quote outputs are checked against versioned expected financial results

## Completion gate

- [ ] All six task records link real committed outputs.
- [ ] Every acceptance case has a reproducible result. Required blockers keep the phase incomplete.
- [ ] Platform/physical device and PC reference comparisons exist wherever required.
- [ ] Feature/type rows include fixture IDs, commands, evidence and review.
- [ ] Update phases.json after review; run `node scripts/porting.cjs check`.
- [ ] Handoff exact paths, commands and remaining risks.

These work-card actions are checkpoints, not a claim that a phase takes minutes.
