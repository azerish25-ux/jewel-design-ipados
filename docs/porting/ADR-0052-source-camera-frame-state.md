# ADR0052: preserve source camera-frame state and normalization

Status: Windows and independent macOS Clang/managed sequence tests pass; actual iOS runtime results pending.

CandidateCameraFrame owns immutable value snapshots of raw camera location, requested direction/up, retained basis, validity and locks. Captured validity is kept explicit. Setters preserve lock equality returns and unset/invalid state changes; frame construction clears validity and projection cache before validation, retains the prior basis on failure, and preserves diagnostic versus ordinary parallel-vector failure paths. No fallback basis is manufactured.

The up vector takes precedence only when up is locked and direction is not; otherwise direction takes precedence. Length and Unitize preserve pinned published scaled arithmetic, including the smallest-normal/subnormal rescaling branch. Valid direction maximum-coordinate must be strictly above2.3283064365386962890625e-10 and below1e100; frame unit/perpendicular guards use1e-6. These are separate source policies.

The C++ reference compiles25 unchanged source methods with observation containers.730 sequences/2,978 steps compare return values, all18 state values, validity flags, diagnostic count and cache invalidations. Raw location/direction/up retain exact IEEE values (NaN classification and signed zero explicit); computed basis uses1e-13 tolerance. Four independent preset view/inverse matrix compositions establish the frame-to-matrix seam. Tests first failed with missing frame API before implementation.

This is published openNURBS6 numerical evidence, not private Rhino5 runtime parity. Lens/parallel transitions, world-mesh normalization, actual rendered source-preset frames, full controls and original-PC/physical acceptance remain open. See [work card](evidence/P08-T01-A05-03a.json) and [parent plan](plans/P08-T01-A05-camera-matrices.md).
