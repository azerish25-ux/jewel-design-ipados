# Original camera setter requirements for the iPad port

The original Rhino5 dependency candidate is now statically traced, not executed. See [byte-locked native evidence](../evidence/audit/native-camera-setter-ghidra.json). Existing published-source numerical tests remain valuable but are not proof that their complete behavior equals the original private runtime.

| Required behavior | Original evidence | Port state |
| --- | --- | --- |
| Reject invalid/tiny target-camera direction before changing the viewport | Original setter initial validity checks; exact threshold2^-32 | Portable preparation and effective numeric guard core implemented; native parent/guard context binding pending |
| Calculate through a temporary viewport copy | Original ON_Viewport copy constructor and final conditional commit | Copied-state preparation implemented |
| Preserve up-lock policy, prior roll and degenerate up fallback | Original52 typed called-import boundary signatures, acos/rotation paths; roll threshold1e-6 | Portable recipe implemented; compiled published rotation checks pass, original runtime comparison open |
| Preserve failure ordering | Up failure avoids viewport commit; after accepted up, SetVP result is ignored and original target metadata setter follows | Typed protocol preserves commit-false then target write; concrete commit adapter pending |
| Preserve original view restrictions | SetVP byte/parent-view/projection/axis guards | Raw guard conditions observed; semantic field names still unresolved |
| Apply state with original lock/symmetry handling | SetVP temporarily clears locks/flags, applies frame/frustum, restores incoming flags | Existing primitive helpers available; commit adapter pending |
| Preserve target and view metadata | Temporary target, optional commit target-distance branch; view setter always synchronizes mirror from retained/resulting core target and getter diagnoses/repairs mismatch | Original core/mirror setter/getter repair implemented in numeric commit core; whole-view adapter pending |
| Invalidate derived caches and nonzero change serial | Original transform/screen cache and counter updates | Numeric core implements serial wrap/collision, identity-on-failure cache mutation and source-oriented unchecked screen differences; whole-view/document integration pending |
| Preserve view-projection history and update notifications | [History/notification audit](../evidence/audit/rhino5-view-history-source.json): whole-view snapshots, overwrite-next/prune on branch,255-entry bound; conditional view synchronization and UI work | Document notification -> history push -> final target metadata; full adapters pending, distinct from parameter UndoJournal. Pop returns false even after mutation. |
| Reproduce the full recovered preset command | Perspective conversion, construction plane/pose, native locations, lens50, up, conditional parallel, redraw | Partial numerical dependencies verified; complete command/preset pixels still open |

Only the original dependency candidate hashes/version are claimed. Original-PC and physical-device observations remain separate gates. No full UI-039 or other parity row is closed by this static work.

The original core comparison now has a distinct [original5-derived C# numerical implementation](evidence/P08-T01-A05-03c.json), with Windows, macOS and both simulator-family numerical checks passed. Published6 validation/flags are not silently reused as original5. The [temporary-viewport recipe](evidence/P08-T01-A05-03e-02.json) now consumes these core primitives. Its typed commit boundary preserves ordering but does not implement the remaining native UI effects.
