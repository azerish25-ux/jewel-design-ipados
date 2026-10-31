# P08-T02-A01 Metal offscreen CAD viewport proof

Existing approved direction: C#/.NET iOS/UIKit with native CAD and Metal. Candidate proof only; P08-T01 source camera/material mapping and full viewport/UI parity are not declared complete.

Design decision: use Metal directly with the genuine P05 mesh snapshot. Scene-graph frameworks would add a second rendering abstraction; a CPU picture would not prove device GPU geometry upload/depth. Neutral lighting is a test material, not an asserted original CounterSketch material. No product control layout is invented.

Boundaries/files:
- src/Port/Rendering.Candidate: portable double-center/unit-radius mesh packing, float6 position/normal vertex layout, UInt32 indices and source face ordinals, fitted right-handed0..1-depth camera matrix. Empty mesh clears only. Validate finite values and image dimensions.
- tests/Port/Geometry.Candidate.Contracts/ViewportPackingAssertions.cs: RED missing portable packing; independent fit/clip/precision/normal/layout/empty/error checks using real native mesh through shared contract runner where relevant.
- src/Port/Rendering.Candidate.iOS: Metal-only library; compile shader, upload real buffers, depth-test indexed triangles, render shared BGRA target, wait/check GPU completion, read back pixels. No CPU fallback or cached image.
- tests/iOS.Native: render original/edited/orbit/empty frames from actual native joined140; write PNG/metrics and PASS marker only after foreground/change/depth checks. Existing result protocol gates platform-family identity.
- scripts/run-native-simulator.sh and native workflow: compile Metal binding library before expensive kernel build; copy/verify fresh PNG/JSON artifacts, retain logs. A library build is compile proof, not GPU execution.

1. Commit this design/plan; self-review: all data comes from CAD, CPU precision conversion is centered/scaled, GPU absence fails, resources disposed, image proof fresh; camera/material/original UI/picking remain open.
2. TDD portable packing/camera with real meshes; add small project and measured checks.
3. Implement Metal renderer from pinned official dotnet/macios bindings; verify API compile on macOS Actions and fix actual errors. Use native simulator app for actual GPU test, not a stub native archive.
4. Verify images/metrics on iPad/iPhone and unsigned device compile/link separately. Update source/provenance/repository checks, evidence and current state; commit/push progress.

Success for this card: actual CAD triangles visibly rendered by Metal, edit/orbit change pixels, empty scene remains background, image artifacts inspected. This does not complete full1:1 UI/material/render/picking/physical/PC parity. All local tools/cache/output D:;5-second retry for transient D: errors. No recovery/kernel edits.

Checkpoint verified: implementation/source1b62e58 passed actual iPad/iPhone Metal rendering and camera decisions in41182340484, reusing verified native source from41181433871. Producer and current unsigned device builds passed52 warnings0 errors. All actual PNGs/metrics were inspected and independently decoded; native GPU raw BGRA hash and foreground/edit/orbit counts reproduced exactly. Earlier import/preflight failures and fixes remain recorded in the evidence. See [actual pixel proof](../../evidence/apple/metal-viewport-results.json) and [retained frames](../../evidence/apple/metal-viewport/README.md). This closes only the bounded offscreen proof card; full interactive/source-camera/material/original-PC/physical/UI parity remains open.
