# P08-T01-A05-03f concrete viewport commit

Next ready integration under UI-039; this is a plan, not an implemented adapter. Consume e-01/e-02 numeric/recipe evidence. Keep source-preset rendering and original-PC acceptance distinct.

## Source boundary

Original Rhino.exe SHA256 951ff639f2c9b61bbf7c32a67169d0e23bdbd026f74ada80ace9b1f02b1b48eb. SetVP RVA0x44c0c0, full1266 bytes/five chained unwind regions, SHA256 b05d274c3ab09410362b29356b916a356fda7385f13da6aa10363db95faf05b1. Use accepted verified-replay/final output and tracked native-camera-setter-ghidra audit, not rejected partial/incorrect-ABI views.

## Prerequisite checks

1. Statically compare original GetXform/world-to-clip arithmetic and cache dependencies before adopting published6 matrices as legacy5. Known exports in opennurbsx64.dll: GetXform0x2d4160, GetScreenPort0x2d32f0, GetFrustum0x2d2ea0, IsValidCamera0x2cdec0, IsValidFrustum0x2cded0, Xform.IsIdentity0x2e6500. These are verified export entry addresses only; complete byte regions/semantic equivalence have not been established here.
2. Inspect ON_3dmView.SetTargetPoint to distinguish embedded viewport target and view metadata effects. Do not collapse those fields or assume independent storage without evidence.
3. Resolve or explicitly model effective original view restrictions: raw byte at viewport+0x260 rejects; parent+0x600==1 and parent+0x108==this restricts incoming projection to parallel/world XYZ basis. Whole-view/named-getter evidence now resolves these as projection locking and IsPageViewMainViewport restrictions; concrete parent-context binding remains required.
4. Define a concrete owned view state with transform/screen cache serial, extra transform, target metadata, notification and view-history contracts. Reuse existing immutable numeric values; do not introduce empty production callbacks or always-success commits.

## Required tests before implementation

- Guard rejection and invalid incoming camera/frustum/GetFrustum make no state/cache/history writes. Test restricted world-aligned parallel acceptance and every rejection axis/projection.
- Clear old locks and symmetry before sequential projection/location/direction/up/frustum application; restore incoming known flags afterward. Preserve original ignored setter returns and any retained state. Incoming viewport target is not blindly copied by SetVP.
- Symmetrize incoming bounds in source order before SetFrustum. Test unequal bounds, overflow and downstream false returns without inventing rollback.
- Nonzero commit mode preserves old positive target distance using new camera Z/location; mode0 skips that branch. Verify invalid/zero/negative old distance.
- Increment unsigned32 serial and skip zero on wrap. Rebuild matrix only when cache serial differs; failed transform resets the output cache matrix to identity and retains its stale serial. Screen width/height reads still occur in the enclosing serial-difference block; failed screen read zeroes both. Extra transform multiplication order must match source.
- Notify parent document ViewModified, then push view-projection history, then caller e-02 writes final target metadata, even on false commit. Observe snapshots/order; do not equate view history with parameter UndoJournal.

## Verification and integration

Use one independently reviewable child for unresolved static dependencies and another for the concrete state/side-effect adapter if needed. Run missing-API/behavior RED, local numeric and full native suite, adjacent regressions, integrity, macOS and actual iPad/iPhone. Keep all local cache/build/tool/report writes on D: through run-local; wait five seconds and retry transient D: errors. Record hashes, warnings and exact source commits. No parity row closes from static/numeric/protocol work.

After the concrete adapter passes, compose the recovered full preset order (perspective, plane/pose, locations, lens50, prescribed up, optional parallel, redraw) and world-normalized Metal matrix packing. Require source-preset screenshots and independent clip checks before claiming rendered camera parity. Original-PC, wider project corpus and physical-device gates remain open.
