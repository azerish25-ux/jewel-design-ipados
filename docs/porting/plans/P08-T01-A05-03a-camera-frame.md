# P08-T01-A05-03a camera frame state and numerical reference

For the active approved camera integration plan, execute this dependency without delegation. Goal: preserve published camera frame/setter state before connecting presets to the renderer. Tech stack: C#/.NET10 plus standalone C++17 numerical reference and macOS/iOS Actions.

Requirements: UI-039 camera presets and P08-T01; this child proves numerical frame behavior only. Inputs are pinned in docs/evidence/audit/camera-frame-source.json. Recovery/native kernel baselines remain read-only. All Windows outputs/caches stay on D: through run-local; wait five seconds and retry transient D: errors.

Design: CandidateCameraFrame owns an immutable raw state snapshot (location, requested direction/up, basis, validity and locks). Explicit setters retain original lock equality, unset sentinel, validity/cache invalidation and failure behavior. No fallback basis. Published Length/Unitize arithmetic is preserved separately from System.Numerics/GPU packing. A standalone reference compiles unchanged frame, setter, vector arithmetic and validation methods with observation-only state/cache/diagnostic containers. Original private Rhino5 runtime equivalence remains unverified.

- [x] Prepare hash-locked source bodies/macros; compile reference without fast math or fused contraction.
- [x] Generate deterministic sequences for all locks/raw validity flags, four presets, setter order, parallel/nearly parallel, threshold/huge/nonfinite/sentinel components, and retained failed state. Validate every step's return,18 state values, validity, diagnostics and cache invalidations.
- [x] Add managed assertions first; confirm RED missing frame API. Implement source state and normalization; require GREEN reference comparisons plus frame/matrix composition checks.
- [x] Wire the same assertions into Rendering.Tests, shared native contracts and actual iPad/iPhone viewport execution. Add independent Clang reference checks to existing macOS preflight and cheap fixture/tamper regressions to repository CI.
- [x] Review failure retention, source hashes/licensing, ownership and raw invalid-state handling; run adjacent regressions/integrity. Commit and push code with task ID; inspect actual Actions outputs before recording Apple success.
- [x] Record remaining lens/parallel transitions, normalized world-mesh projection, actual preset frames and original-PC/private/physical acceptance. No full parity row closure.

Owned paths: native/Rendering/CameraFrameReference; scripts/prepare-camera-frame-reference.cjs and generate-camera-frame-cases.cjs; Rendering.Candidate/CandidateCameraFrame.cs; shared CameraFrameAssertions/fixture and existing runners/workflows; focused Node tests; evidence/ADR/current-state notes.

Additional device-target check: run41286695881 builds current source as unsigned ios-arm64 Release/AOT using validated native archive producer41181435472. It remains a build-only check and was still in progress when simulator evidence was inspected.
