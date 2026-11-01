# P08-T01-A05-03b joint viewport parallel transition

Execute the approved source camera integration plan without delegation. Goal: preserve complete published parallel/symmetry/target transitions over the verified frame and frustum math before applying original presets to the renderer. C#/.NET10 plus standalone C++17/macOS/iOS comparisons.

Requirements: P08-T01 / UI-039, with full UI/PC/physical parity still open. Inputs: camera-parallel-source.json, camera-frame-source.json and pinned openNURBS f2dc7fba67017475b8d530835a40fcfb5372134a. Recovery and native kernel baselines stay read-only. All local writes/caches D: via run-local; five-second retry on D: errors.

Design: CandidateViewportProjection owns a value snapshot joining camera frame, frustum validity/bounds, raw projection enum, raw symmetry byte and target. Preserve unknown symmetry bits, source setter returns, lock clearing, failed-operation mutations, target fallback/recenter tolerance and camera dolly. Delegate frame, lens and frustum arithmetic to the existing verified helpers, collecting their invalidations/diagnostics. SourceVectorLength becomes internal for shared exact point-distance arithmetic; no public frame API change. Generic renderer remains unchanged until joint transition and world-normalization/preset captures are proved.

- [x] Extract/hash exact transition, frame/frustum and point/vector dependency closure; compile observation-only native reference.
- [x] Generate bounded deterministic sequences for all validity/lock/symmetry combinations, raw projection enum, off-axis/behind/unset/huge targets, corrupt frusta, dolly rejection and target jitter boundaries. Compare all fields, return/query results and counters after every step.
- [x] Add managed assertions first and confirm missing projection API RED. Implement joint state and original transition ordering; require GREEN native differential and adjacent frame/frustum checks.
- [x] Wire the same assertions into macOS Clang/managed checks, Rendering.Tests, native contracts and actual iPad/iPhone viewport execution. Add fixture and tamper regressions.
- [x] Review no fallback geometry/state or unknown-bit loss, run integrity/adjacent checks, commit/push with task ID, inspect actual Actions outputs and record precise Apple/physical limits.
- [ ] Continue to double world-mesh composition/source preset application; original framing must remain explicit rather than invented.

Owned paths: native/Rendering/ViewportProjectionReference; prepare/generate viewport-projection scripts; Rendering.Candidate/CandidateViewportProjection.cs; internal source-vector-length seam in CandidateCameraFrame; shared assertions/fixture/runners, existing preflight/visible/repository workflows, focused Node regressions, evidence/ADR/current handoff.

Original native setter handoff: docs/evidence/audit/native-camera-set-locations.json locates the exact Rhino5 candidate wrapper/import/original1386-byte callee. Static analysis only; full callee semantics remain unverified. Perspective prerequisite audit: docs/evidence/audit/camera-perspective-source.json.
