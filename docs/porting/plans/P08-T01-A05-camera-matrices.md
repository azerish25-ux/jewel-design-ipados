# P08-T01-A05 source camera matrices and Metal depth conversion

Goal: preserve pinned published WorldToCamera/CameraToWorld/CameraToClip calculations and explicitly bridge their homogeneous depth convention to Metal. This supports original camera application without inventing lens/framing behavior. Full private Rhino5/PC/physical/source camera movement acceptance remains open.

Design: a separate double4x4 camera transform module, not the affine-only12-coefficient document DTO. Preserve source coefficients, return values and unchanged matrix on CameraToClip equal-bound failure. Numeric source math can produce nonfinite output for raw invalid inputs; Metal float packing validates finite representable values explicitly. A native reference compiles exact published methods. Convert source clip z to Metal using z_m=(w_s-z_s)/2, preserving x/y/w, and transpose deliberately when constructing System.Numerics row-vector storage.

Source: openNURBS pin f2dc7fba67017475b8d530835a40fcfb5372134a; opennurbs_xform.cpp SHA 7b922be97b2d05c50cd5865e32826d87fc2f4926304f55f21bf9344478580258. WorldToCamera takes supplied basis vectors directly; no invented normalization. CameraToClip maps near camera z=-n to source clip z/w=+1 and far z=-f to-1. Numerical source comparison is distinct from original private viewport behavior.

Alternatives: a generic CreatePerspectiveFieldOfView shortcut loses asymmetric fields/source raw validation and depth convention; using affine DTO discards the fourth projective row. Use exact double source operations and explicit GPU adapter instead.

Children: P08-T01-A05-01 source extraction/reference matrix cases; P08-T01-A05-02 C# matrix port and Metal adapter/endpoint tests; P08-T01-A05-03 source preset/world normalization/parallel-conversion integration and actual Metal preset frames after evidence.

Owned files: standalone native/Rendering/CameraMatrixReference; prepare/generate scripts; Rendering.Candidate/CandidateCameraTransform.cs; shared matrix assertions/fixtures and Rendering.Tests; source audit/evidence/ADR; macOS reference/managed verification. Existing CandidateProbe/kernel/recovery stay read-only.

1. Hash source; compile exact methods with observation point/vector/matrix containers; deterministic perspective/parallel/symmetric/asymmetric/degenerate/raw nonfinite basis/bounds cases and retained prefilled matrix.
2. RED missing C# matrix API before adaptation; GREEN all16 coefficients and return/state semantics against independent compiled source; explicit finite/NaN/inf/signed-zero rules.
3. Verify near/far/corner/behind-camera coordinates independently, source-to-Metal0..1 endpoints and column/row packing. Reject nonfinite/float-overflow GPU matrices; no silent output replacement.
4. Run Windows/macOS native+managed cases and adjacent geometry/pixel/source/repository checks. Record source hashes, commands, actual outputs, warnings and remaining original/private/physical limits; commit/push. No rendering behavior change until matrix proof and integration tests.

All local writes/cache/temp/tools/auth/output D: via run-local; wait5 seconds and retry transient D:. Existing C: compiler may execute; no installation/config edit or original runtime execution. No subagents. Self-review: source matrix convention explicit, projection row preserved, renderer adapter rejects invalid data, no full parity row closure.
