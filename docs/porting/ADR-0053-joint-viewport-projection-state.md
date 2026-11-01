# ADR0053: joint viewport projection state and failure mutation

Status: Windows native reference/managed transitions pass; Apple verification pending.

CandidateViewportProjection owns an immutable snapshot joining the verified camera frame, frustum, target, raw uint projection and raw symmetry byte. It forwards setters/lens operations through the existing helpers, preserving counters and state changes even on false returns. Unknown symmetry bits survive flag changes. The internal source vector-length helper is shared with target distance; no public frame API changes.

Parallel conversion clears up/direction locks before its early return, retains the location lock, performs symmetric-frustum camera dolly, applies flags even after symmetry conversion failure, and rescales the target plane through the source frustum setter. The initial validity return flag is retained in source order rather than recomputed after mutations. Target fallback and recenter tolerance preserve published behavior. Unknown projection self-assignment returns true unchanged; a different invalid value falls back to parallel and returns false.

The standalone reference compiles55 unchanged published methods, including the exact projection enum and vector/point/frustum/frame dependency closure.961 sequences/6,675 steps compare return/query values, all27 numeric fields, flags, unknown bits, diagnostics and invalidations. Every source-unmodified group must retain exact floating-point bits; computed fields have explicit1e-13 tolerance and signed-zero/NaN/inf rules. Managed tests failed with missing state API first; the complete adjacent native candidate suite and34 regressions pass.

These pose-input sequences are not the original complete command wrapper. The recovered command first calls ChangeToPerspectiveProjection, then native SetCameraLocations, lens50, CameraUp and conditional parallel conversion. Those remaining perspective/native setter semantics, double world-mesh composition, source preset Metal frames and original-PC/private Rhino5/physical comparisons remain explicit prerequisites. Generic renderer behavior is unchanged. [Work card](evidence/P08-T01-A05-03b.json).
