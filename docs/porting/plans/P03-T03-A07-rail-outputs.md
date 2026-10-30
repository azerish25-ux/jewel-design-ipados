# P03-T03-A07 graph-managed rail outputs

Goal: let known value/profile/rail nodes consume actual upstream rail outputs through one explicit generation order, rather than rebuilding isolated consumers. Preserve source data conversions: rail outputs are curves (not numeric radius), OutsideRail curve-data Scale1 vs separate numeric channel1=0, invalid geometry output index -> empty.

Sources: CSBlock output getter, CSRailData/CSRingRailData/CSOutsideRingRailData constructors/conversions, recovered rail builders and scalar channel rules. Full material/export/scheduling/failure lifecycle remains separate.

- [x] RED CandidateRailGraphSession and curve-valued channel data missing.
- [x] Add pure-data external-node provider boundary to Documents value generation (no native dependency), curve-reference tokens and topological dependency integration; no unsupported defaulting.
- [x] Implement known rail provider/native cache session with explicit generation, source metadata, retained output handles, curve/profile resolution for downstream geometry and numeric0 output. Avoid recursive generations.
- [x] Test actual saved nodes, rewired rail->radius-curve and rail->outside-curve edges, default conversions, changed values/rebuild, previous output retained across replacement, cycle/failed-generation cache cleanup, invalid indices and disposal.
- [x] Preserve document/source differential/native/reference/provenance regressions and record ownership/source policy in ADR0026.
- [x] Push/dispatch Apple simulator/device tests; preserve existing runs. Runs41089011365/41089016432 atf3685f3 confirmed live/queued.
- [x] Inspect actual provider/native/family/ownership/AOT artifacts before bounded verification. Actual iPad/iPhone native reports and unsigned arm64/platform IOS52 warnings0 errors are recorded in rail-outputs-results.json; complete graph/PC/physical parity remains open.

Candidate cache generation commits only complete native output updates; scalar fields may already have updated if a later native operation fails. This is explicit candidate policy, not original CS graph failure equivalence. Full dirty/incremental/parallel scheduler, unsupported producers, generic data conversions, material/UI/export/PC/physical/performance/cancellation acceptance remain open.
