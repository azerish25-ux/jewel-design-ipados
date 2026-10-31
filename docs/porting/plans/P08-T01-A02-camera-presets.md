# P08-T01-A02 portable original camera preset decisions

Goal: preserve the recovered CSUtil.DocumentSetCameraView preset decisions in C#, serving UI-039 without asserting completed viewport parity. Execute within the approved continuous port roadmap; no new product design or external dependency choice.

Architecture: a portable LegacyCameraPreset.Resolve(string) returns normalized name, double camera/target/up, construction plane, enum state, 50 mm lens and parallel/perspective decision. It does not claim to execute viewport selection, redraw, native lens/frustum or zoom. Renderer integration remains separately gated by real Metal proof. This keeps original source rules available without fabricating a Rhino projection implementation.

Alternatives: routing through Windows/Rhino cannot work offline on iPad; assigning generic orbit defaults loses original behavior. Preserve explicit source decisions now and integrate a real projection backend after its behavior is measured.

Sources: docs/evidence/audit/viewport-camera-source.json, CSUtil.cs229-281 (hash locked). Original null throws at Trim; unknown/empty names use Perspective pose/state but parallel projection. Trim/invariant lower casing and exact original constants retained.

Owned paths: src/Port/Rendering.Candidate/LegacyCameraPreset.cs; shared Geometry.Candidate.Contracts/LegacyCameraPresetAssertions.cs and Recovered/LegacyCameraPresets.cs; scripts/verify-camera-preset-oracle.cjs; lightweight tests/Port/Rendering.Tests console entry; repository CI verification step and evidenceP08-T01-A02. Shared assertions will execute on iPad/iPhone native harness on the next appropriate full run.

1. Build a hash-checked unchanged extraction of source method using test-only observation types; stubs log wrapper calls only, never implement native projection.
2. Write normal/boundary/null/culture tests and run RED before implementation. Compare four presets and mixed-case/Unicode whitespace/unknown strings across invariant/en-US/tr-TR cultures; verify null exception and exact double data/parallel flag. Also validate original wrapper sequence and viewport preference through observed calls separately.
3. Implement the portable immutable value plan. Run lightweight source-differential tests GREEN; append shared assertion to actual iOS test suite.
4. Verify extracted source fidelity, focused tests, adjacent native contract suite and repository integrity. Record source/body hashes, commands, case counts and limits. No full parity row closure. Commit/push task ID.

All local writes/caches D: through run-local; wait5 seconds and retry transient D: failures. Recovery remains read-only. Self-review: no fake frustum/native execution/UI, no hidden credentials/accounts/network and source null/unknown behavior explicit.
