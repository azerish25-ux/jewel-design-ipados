# P03-T03-A13-A03 collection-consuming behavior

Goal: use owned nested arrays in existing transform, scale, rail, placement and Add producer paths with source-specific semantics. Preserve full parent acceptance; no generic array-to-first-item shortcut.

Sources: CSTransformObject.Update; CSDataArray copy/ApplyTransform/GetTransform/GetTightBoundingBox/GetFlattenedList; CSInputChannel.GetFirstData; CSDataHelper/CSDatas.CSUtilMath.GetTransformNoArray; CSAdd.Update. Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd.

- [x] RED stored consumer52 rewired to owned profile array131 rejects array object.
- [x] Recursive owned object duplicate/application; preserve nesting, null child->empty, profile vs rail/pure metadata order, scalar/empty no-op, cleanup on partial native failure.
- [x] Recursively flatten transformation inputs only; skip null/invalid/nontransformable data; prepend source-order matrices and preserve invalid-product skip.
- [x] Aggregate native tight bounds across all represented curve leaves; no-bound arrays fail; keep requested/kernel tolerance metadata.
- [x] First-item array curve queries for rail/placement/OutsideRail, including nested/empty/null/noncurve first items; never search later items.
- [x] CSAdd provider uses source array arithmetic outputs; direct array transform capability uses first non-array item, per source.
- [x] Exact-source/analytical/native mixed tree cases, current scene/profile geometry, saved edits/reopen, old snapshots, rollback/cycles/zero entries; preserve earlier tests.
- [x] Review/source/provenance/repository checks.
- [x] Commit/push6f18df4; dispatch native host/iPad/iPhone41117797147 and unsigned device41117800707, both queued.
- [ ] Inspect actual source49/native array application/bounds/metadata/family/AOT reports.

Known represented data are numeric/empty/missing/transform/stored profile/generated curve/nested array. Arbitrary other source data, material/display/segmentation/raw4x4/Unset/degenerate native geometry/all builders/UI/PC/physical acceptance remain open. A13 and full port are not complete from compilation/limited tests. All local writes/cache D:, wait5 seconds/retry transient D errors.
