# P08-T01-A05-03e camera-location dependencies

Continue the approved original-camera integration for UI-039. Original core input SHA256 15c1be75b8ca3b0a42d2dc0c9b2a3de81730ff09b06ca16fb55d3a1a7a766e7c. No original target execution.

This reviewable child, P08-T01-A05-03e-01, implements immutable arbitrary-axis vector rotation, not the full camera setter. Preserve cardinal cleanup, pair normalization/diagnostics, axis normalization, zero-axis scaling and vector-only application. Angle calls resolve to MSVCR100 sin/cos via exact IAT slots. Original 2D normalization instructions agree with the pinned published arithmetic. Published-source comparisons remain distinct from original runtime parity.

1. Add missing-API RED tests: cardinal/general/nonunit/zero axes, noisy/scaled/zero pairs and independent geometric invariants.
2. Build an independent standalone published reference using unchanged hash-pinned methods and robust normalization; preserve existing native archive inputs.
3. Implement Rhino5VectorRotation under Rendering.Candidate, then compare generated fixtures locally and on macOS and both simulator families.
4. Run adjacent regressions/integrity, review scope/provenance/immutability, record exact results, commit/push. All local generated outputs use D:/run-local; transient D: failures wait five seconds and retry.

Next children: temporary-viewport up/roll recipe; native commit guards/target ordering/document/history/cache adapters; full source preset application to Metal. Original-PC and physical-device acceptance remain open. No parity row closes from this numeric work.
