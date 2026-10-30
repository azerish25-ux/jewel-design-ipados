# P03-T03-A10 two-rail profile placement

Goal: execute untouched placement110/117/124 -> Add111/118/125 -> scale112/119/126 from the actual library. No stored height/preview/default replacement for geometry.

Sources: CSBlocks.Profiles.Obsolete/CSProfilePlacer2RailXform1.cs exact GUIDfc789225-3f3b-41f5-9eb0-6c25d4fc54b8 v1.0; CSUtilGeometry.OrientProfileToCurveNalp; CSUtilMath.Orient3Point vector overload; CSBlock.BlankOutBeforeUpdate/UpdateAllChanged. Hash these in task evidence. Fixture SHA25673e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd.110 inputs[4,13,13,94]/[0,0,1,0],117 last channel2,124 last object95/channel0.

Design: pure affine/result values plus actual native normalized length/point/tangent/intersections. Preserve source repeat length lookup, Y-scaled radial direction, right-handed frame, finite localY30 ray, first line-A event/paired outer parameter, min-height.01 and sequential seam checks using original normalized fraction against native domains (do not correct source quirk). Default outputs empty each successful generation per CSBlock blank-before-update; candidate cache retains prior generation on failure as already documented. Expose tolerance/event-order/orientation/degenerate policy limits explicitly.

Owned: exact header allowlist + independent reader/inventory; candidate placement/frame/result/graph output channels; source-extracted observation harness and native/document/source regressions; ADR/evidence/CI markers.

- [x] RED untouched original scale chains fail on missing placement110.
- [x] Decode/independently consume three exact common headers;128 known/113 opaque/100 scalar handles; structural/future/rewire/reopen fidelity.
- [x] Implement source frame and actual native placement; output transform/inner parameter/optional outer parameter/height; explicit unsupported degeneracies; no extra native shape retained.
- [x] Add graph multi-channel snapshots/curve resolution; preserve empty output reset and failure cache policy.
- [x] Compare extracted source frame/control/seam logic with declared observation stubs; independent analytical circle/ellipse/finite ray/no hit/height floor/paired reversed overlap checks.
- [x] Regenerate original Add/scale paths; native affine application, edits/save/reopen, frame/height/channel defaults, no-hit reset, failure/cycle/cleanup; preserve earlier suites.
- [x] Review, source hashes, document/native/reference/provenance/repository checks.
- [ ] Commit/push and dispatch Apple tests; inspect source-specific native/family/AOT artifacts.

Limits: candidate kernel tolerance, event sorting/multiplicity and vector/Unset behavior require original-PC comparison. Array input flattening, arbitrary data kinds, source scheduler failure effects, all other placement versions, full transform consumer/rotation/sweep/UI/PC/physical parity remain required. No full parity row closes. Every local write/cache stays D:; wait5 seconds/retry transient D: errors.
