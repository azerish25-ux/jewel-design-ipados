# P03-T03-A14 owned BRep graph data

Parent scope: full source geometry-channel behavior and offline editable design graph. No parity acceptance is reduced. This child builds on A19-A02-A01's actual paired surface, not cached previews.

Requirements: BLD-0516, SER-0551, CAD-031 plus represented transform/collection data paths. Fixture SHA25673e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Source: CSOnGeometryData.GetBrep/GetCurve/GetNurbsCurve/GetTransform/ApplyTransform/ConstructorAux/GetTightBoundingBox/TryGetBoundingBox; CSSimpleSweep2.Update; CSScaleObjectToSize.Update and existing array helper contracts.

Source observations: BRep converts to copied BRep, not curve; numeric queries fail. Initial transform identity; geometry transformed physically while cumulative metadata postmultiplies. Old tight-box method deliberately requests inaccurate/fast for BRep; New requests accurate. Sweep publishes actual nonnull native results with dynamic names Identifier_index. Closed/no-param/single-profile/other unsupported candidate modes must fail explicitly. Native backend/topology/PC equivalence remain separate.

Design: one owned native geometry state for curve and BRep, distinct immutable reference types. Shared retention/snapshots/array relay/affine transforms; typed curve queries never return faces. BRep bounds explicitly select native fast/accurate policy with no mesh dependence. Both are candidate policies, not claimed Rhino numeric equivalents. Exact1.0 sweep adapter uses prepared dependency outputs without reentrant generation, constructs actual native BRep, and publishes result channels. Source/renderer/user-string/material/full scheduler acceptance remains open.

- [x] A01 RED missing BRep reference, typed retention and graph134 output. Add ownership/kind/failure/lifetime assertions with actual geometry.
- [x] Generalize current curve owner implementation without separate BRep traversal paths; preserve all earlier source/native regressions.
- [x] A02 actual sweep graph adapter, input/output count/default/name behavior, explicit unsupported modes and transactional cache preservation.
- [x] A03 native fast/accurate shape bounds, source box policy, mixed BRep/curve collection/transform/scale/selector semantics.
- [x] Source-hashed conversion/transform/bounds-policy evidence; normal/boundary/failure tests.
- [x] Real134 -> nested arrays/unarray/affine/bounds, radius/parameter edits, structural save/reopen/rebuild, retained readers after regeneration/disposal and zero native entries.
- [ ] Review/provenance/repository checks; commit/push; macOS iPad/iPhone and unsigned arm64 tests, inspect reports.

All local files/temp/cache/build/auth under D:; retry transient D: errors after5 seconds. Recovery/kernel read-only. No fake BRep/mesh/type coercion or hidden feature removal. Source block failure scheduling/native result cardinality, other geometry, material/user strings/rendering/export/closed/no-param/PC/physical gates remain required.

Local regressions pass. Saved wildcard/spare behavior is preserved; native wildcard capture is tested separately against source execution. [Decision](../ADR-0038-owned-brep-graph.md), [evidence](../evidence/P03-T03-A14.json). Full source/PC/physical sign-off remains open.
