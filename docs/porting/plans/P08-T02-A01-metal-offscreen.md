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

Portable RED missing Rendering namespace recorded;144 real mesh clip points and1e9-offset precision/stride/face-identity/owned-buffer guards pass locally. First real app builds at74efefb failed missing System/System.IO imports that implicit-usings preflight hid; strict preflight41181096985 reproduced it and41181190164 passed after fix7d64ed4. Producer runs41181433871/41181435472 atcf48a04 retained genuine native archives. Both unsigned device producer and current1b62e58 archive-consumer41182342075 passed52 warnings0 errors and executable iOS/arm64 inspection. Producer and current consumer41182340484 iPad runtime steps passed; iPhone runs/image artifact review still pending. Dedicated macOS camera/Metal checkpoint41181809482 passed120 source comparisons and0-warning/0-error library compile. Actual simulator artifacts must be inspected before closing this proof card; full UI/render/physical/original-PC parity remains separate.
