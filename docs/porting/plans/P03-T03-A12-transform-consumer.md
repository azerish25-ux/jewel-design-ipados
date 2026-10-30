# P03-T03-A12 expandable transform consumer and curve metadata

Goal: regenerate original TransformObject115/122/127 from stored profiles and actual scale/rotation/translation/placement; preserve kind/name/sweep parameters/cumulative transform and owned geometry. Other original consumers require unported gem/one-rail producers and must keep explicit failures.

Source: CSBlocks.Transforms/CSTransformObject.cs; CSBlock.Read input loop; CSBlockExpandableInput CreateExpansionInputChannel/Aux and NonExpandableInputChannels; CSInputChannel.HasLink; CSProfileData duplicate/ApplyTransform/GetTransform; CSRailData/CSXformData; CSDataArray flatten; CSDataHelper.GetTransformNoArray/Transform. Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Exact1.0 GUID27f4537a-6fc3-404f-9abd-eb53b7b3d8a1,7 headers.141 known/100 opaque/100 scalar handles.

Design: replay saved expandable socket assignments including reuse of unlinked final slot, out-of-range/negative object normalization, negative channel HasLink test and trailing spare exclusion. Provider dependencies include all runtime sockets; consumer uses non-spare sockets. Prepend valid supported transform capabilities in order; duplicate object data then apply. Generated profile outputs own actual native curves plus immutable name/sorted sweep/source identity and cumulative matrix. Profile transform metadata prepends while rail/pure-transform metadata postmultiplies, per source. Unsupported arrays/data remain explicit gates; do not flatten them to numbers or curves. Preserve numeric/empty no-transform outcomes and missing-object errors. Existing candidate failure-cache policy remains separate from source scheduler.

Owned: Documents replay helper/header allowlist; candidate output metadata/consumer/curve capability logic; independent reader/source observation harnesses and document/native regressions; ADR/evidence/ledger/CI.

- [x] RED original consumer115 fails unknown producer.
- [x] Decode seven exact1.0 common headers independently; structural/future/rewire/reopen proof;141/100/100 counts.
- [x] Source-extracted expandable assignment/creation and comparison cases for gaps/invalid indices/spare/import offsets; immutable runtime vs stored separation.
- [x] Transform composition, native profile/rail copies, profile names/sweeps/matrix capabilities and source-specific multiply order; no geometry placeholder.
- [x] Original3 consumer pipelines, source coefficients/native points/metadata, nested transforms/selector snapshots, edits/save/reopen, native readers across replacements, failed/cyclic/disposal/zero-entry cases; retained earlier tests.
- [x] Source/runtime/provenance/reference/repository checks and review; no full PC/UI/material/array/invalid4x4 sign-off.
- [x] Commit/push32542f3; dispatch native host/iPad/iPhone41108929102 and unsigned device41108932509, both queued.
- [ ] Inspect actual source12285/native/profile metadata/family/AOT artifacts.

Scope remains full port. This card implements the currently resolvable known non-array data slice; complete arrays/other data, material/display flags, invalid4x4/Unset, segmentation APIs, all variants/one-rail/gem producers/full scheduler/render/export/UI/PC/physical parity remain required. Every local file/cache stays D:; retry transient D errors after5 seconds.
