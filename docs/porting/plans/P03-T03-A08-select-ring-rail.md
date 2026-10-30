# P03-T03-A08 SelectRingRail forwarding

Goal: execute actual saved node16 (GUID e10a6966-719f-4ba9-9efe-b7ad0378b5f3 v1.0) over existing graph-owned native rails. Source CSSelectRingRail.Update reads Boolean defaultfalse, both raw data inputs, null fallback then selected data; SetData forwards without filtering/cloning. Missing and empty data must remain distinct. Existing candidate cache ownership retains selected native curve for a generation independently of later upstream replacement.

Source: recovery/canonical/cad/CSBlocks.Rails/CSSelectRingRail.cs SHA256 9322a3c04187a9f4b1cb8aeb2836982787429b4e95fd1e8b2db04a75a5642256. Inspect CSBlock common serializer/output getter, CSInputChannel.Data/GetBoolean and CSOutputChannel.SetData too. Fixture SHA256 73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd; actual links [4,13,15]/[0,0,0]. Covers P03-T03/P04-T01 known subset, no full parity row closed.

Owned files: known GUID allowlist/common header, missing channel flag, CandidateRailGraphSession forwarding/ownership; independent reference reader/source extraction, document/native tests; footprint/ADR/evidence. No new kernel API.

- [x] RED: known selector header/execution missing; assert actual node16 Generate fails before implementation.
- [x] Independent full-body header reader; decode only exact1.0, preserve fields and stored extra/missing slots; no scalar handle inflation.
- [x] Extract exact source Update into test-only observation harness; exercise null/empty/number/curve/profile and Boolean conversion threshold/NaN/default cases.
- [x] Distinguish Missing from Empty in pure channel data; preserve earlier conversions; graph-owned selector cache forwards any supported data and retains curves/metadata without regenerating them.
- [x] Test saved library selection/invalid output, absent inputs, empty vs missing, noncurve pass-through, selected alias across upstream-only regeneration, downstream consumer, save/reopen, cycles and zero retained native entries.
- [x] Local source/document/native/provenance/footprint checks, review, ADR and evidence; preserve known compression diagnostic.
- [ ] Commit/push; dispatch macOS native simulator/device tests; inspect artifacts before bounded platform verification.

Limits: explicit known dependency closure still generates both rails. Source scheduling/dirty/parallel/side-effects/material/render/export/physical/PC parity remains open. Candidate output lifetime is an adapter policy, not proof of original object alias/disposal behavior.
