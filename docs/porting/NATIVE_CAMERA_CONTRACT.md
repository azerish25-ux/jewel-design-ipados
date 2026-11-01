# Original camera setter requirements for the iPad port

The original Rhino5 dependency candidate is now statically traced, not executed. See [byte-locked native evidence](../evidence/audit/native-camera-setter-ghidra.json). Existing published-source numerical tests remain valuable but are not proof that their complete behavior equals the original private runtime.

| Required behavior | Original evidence | Port state |
| --- | --- | --- |
| Reject invalid/tiny target-camera direction before changing the viewport | Original setter initial validity checks; exact threshold2^-32 | Native setter adapter pending |
| Calculate through a temporary viewport copy | Original ON_Viewport copy constructor and final conditional commit | Direct setter sequence is insufficient |
| Preserve up-lock policy, prior roll and degenerate up fallback | Original52 typed called-import boundary signatures, acos/rotation paths; roll threshold1e-6 | Portable recipe and original primitive comparisons pending |
| Preserve failure ordering | Up failure avoids viewport commit; after accepted up, SetVP result is ignored and original target metadata setter follows | Must not replace this with an all-fields atomic success/failure policy |
| Preserve original view restrictions | SetVP byte/parent-view/projection/axis guards | Raw guard conditions observed; semantic field names still unresolved |
| Apply state with original lock/symmetry handling | SetVP temporarily clears locks/flags, applies frame/frustum, restores incoming flags | Existing primitive helpers available; commit adapter pending |
| Preserve target and view metadata | Temporary target, optional commit target-distance branch, original ON_3dmView target write | Metadata and source command integration pending |
| Invalidate derived caches and nonzero change serial | Original transform/screen cache and counter updates | Portable view-state/cache contract pending |
| Preserve view-projection history and update notifications | Parent document ViewModified bridge and PushViewProjection export | ViewModified precedes history push, which precedes the caller final target metadata write; adapters pending and not covered by parameter UndoJournal |
| Reproduce the full recovered preset command | Perspective conversion, construction plane/pose, native locations, lens50, up, conditional parallel, redraw | Partial numerical dependencies verified; complete command/preset pixels still open |

Only the original dependency candidate hashes/version are claimed. Original-PC and physical-device observations remain separate gates. No full UI-039 or other parity row is closed by this static work.

The original core comparison now has a distinct [original5-derived C# numerical implementation](evidence/P08-T01-A05-03c.json), with Windows checks passed and Apple tests pending. Published6 validation/flags are not silently reused as original5. This supplies the core primitives for the native recipe and commit adapters; it does not implement those remaining UI effects.
