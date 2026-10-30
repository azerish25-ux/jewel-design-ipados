# P03-T03-A13 owned arrays and legacy collection execution

Parent goal: execute Array/Unarray and nested collections without losing source null/empty distinctions, output count/wildcard/flatten behavior, native snapshot ownership, profile metadata or arithmetic semantics.

Split into independently reviewable children; parent acceptance remains full:
- A13-A01: immutable pure array values, exact flatten/duplicate and CSAdd array algebra; generic expandable constructor replay and20 exact common headers. This does not claim native arrays are generated.
- A13-A02: native curve-token item addressing, owned nested snapshots, source Array/Unarray dynamic outputs/all-output wildcard; selector forwarding and disposal/rebuild proof.
- A13-A03: transform-array flatten/application, aggregate bounds/first-item conversions and remaining known consumer integration; no unsupported data collapse.

A01 sources: CSDatas/CSDataArray.cs GetFlattenArray/copy constructor/GetArrayItem; CSBlocks.Maths/CSAdd.cs Update; CSDataHelper.GetArrayCount/GetArrayItem; CSArray/CSUnarray1 constructor/Update/Read inheritance. Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Exact GUIDs8d8b262f-f2bb-43c5-9486-fbd563c4759b/e147e20f-a225-4005-ab5e-2f37d149e81c v1.0.13 Array+7 Unarray headers =>161 known/80 opaque/100 scalar handles.

A01 owns Documents array value/algebra/replay/header allowlist, independent reader, exact extracted source observation harness/tests, inventory/ADR/evidence. Existing graph does not admit array producers until A02 ownership is implemented; array tokens are references, not owned geometry. Existing APIs must reject arrays when their source-specific behavior is not yet adapted.

- [x] RED array value/algebra APIs missing.
- [x] Immutable child list including explicit Missing(null), no scalar/Boolean conversion; source flatten null->empty and duplicate nested null->empty, preserve ordinary leaf bits/reference values. Explicit depth/item bounds fail without truncation.
- [x] Source CSAdd scalar/array Cartesian order and persistent ref defaults across failed conversions, nested/empty arrays and null-item failure. Non-array helper stays explicit.
- [x] Generalize socket replay fixed constructor count0/1; preserve earlier source12285 and new collection constructor cases.
- [x] Independently decode/read/write/rewire/future20 exact headers;161/80/100 counts; no runtime execution claim.
- [x] Source-body/tamper/differential/document/native-adjacent/provenance/repository checks, review.
- [ ] Commit/push and Apple managed runtime/AOT validation; inspect artifacts.
- [ ] A02 owned native collection execution (next).
- [ ] A03 complete known collection-consuming behavior (required).

Retain original CSUnarray1 odd trailing empty array and one-level expansion; null input throws in source. CSArray outputIndex-1 wildcard and expandable HasLink=false load quirks must not be corrected silently. Full arrays/material/data/4x4/segments/all builders/UI/PC/physical parity remain open;0/1451 verified. All local files/cache D:, wait5 seconds/retry transient D errors.
