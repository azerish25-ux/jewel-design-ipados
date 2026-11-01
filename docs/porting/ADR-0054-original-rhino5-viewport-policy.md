# ADR0054: original Rhino5 numerical viewport is a distinct policy

Status: original core static comparison and Windows/macOS C# checks pass; real iOS binding library builds with zero warnings/errors. Actual iPad/iPhone execution is in progress.

The original dependency candidate differs materially from the published6 source candidate. Original scalar validity accepts the positive unset counterpart. Camera location validation has no later world cutoff and does not repair a false camera-valid flag. Invalid/tiny direction/up requests are rejected before storage. Original frame closure has one camera-valid flag; frustum validation has no later world bounds. These are behavioral differences, not merely implementation details.

Rhino5ScalarValidity, Rhino5CameraFrame and Rhino5Viewport preserve that original-derived policy separately. Existing CandidateCameraFrame/CandidateViewportProjection tests keep their published-source meaning. Shared normalization is reused internally only after original static code/constants comparison for valid camera inputs. No public candidate policy was silently changed.

Original method regions and chained unwind ranges are pinned by whole-DLL/function hashes. Floating-point comparisons are checked against instructions where decompiler C loses unordered behavior.18 scalar boundaries/196,608 high-word patterns,32 camera-state checks/four analytical poses and26 viewport checks pass after missing-API RED tests. Perspective tests cover the exact0.125 early lens threshold, preserved width-plane camera dolly, location locks, near correction, target recenter and failure mutations.

These static-derived and analytical tests are not an independent original runtime oracle. Original DLL/application code was not executed. The complete native setter's roll/up recipe, SetVP cache/document/history effects, source-preset application, world-mesh composition and original-PC/physical acceptance remain separate open work. See [original core evidence](../evidence/audit/rhino5-core-perspective.json).
