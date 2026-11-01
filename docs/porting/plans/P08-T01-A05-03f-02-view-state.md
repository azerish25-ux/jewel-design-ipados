# P08-T01-A05-03f-02 owned view commit and history

Continue the parent concrete-commit requirements with [original history evidence](../../evidence/audit/rhino5-view-history-source.json) and [cache primitive evidence](../evidence/P08-T01-A05-03f-01.json). This card is not implemented.

1. Define an owned immutable whole-view snapshot, including core viewport, target mirror, screen/clip modifications, construction plane and other supported/preserved view metadata. Verify original ON_3dmView copy/assignment field coverage before claiming full history snapshots.
2. Model original target setter: core TrySetTarget return, then unconditional mirror sync to actual resulting core target. Model target getter mismatch diagnostic plus mirror repair. Test invalid retained target and raw NaN comparisons; no unrelated metadata field substitution.
3. Implement concrete SetVP guards and source-ordered mutation using parent plan. Preserve original incoming flag checks, ignored setters, old target-distance mode, lock/symmetry restoration, unsigned serial skip-zero, transform cache and screen reads. Use e/f-01 primitives; no fake cache callback.
4. Refine document ViewModified imports/global predicates, timer coalescing, view synchronization and controller effects before porting those branches. Do not call this complete with a revision-only or empty notification.
5. Implement history push: whole snapshot, append at end/empty; otherwise overwrite next and prune later entries. If count>255 remove oldest once. Update index to count-1; no duplicate suppression. Preserve document notification -> history push -> caller final target metadata ordering and actual snapshots.
6. Treat Pop as a separate follow-up until merge helper0x44d3d0, GetClientRect, SetView and SetScreenSize are reviewed. Its original return is false even after mutation; no blanket false-means-atomic assumption.
7. Run failing behavior tests first, local/full native/adjacent integrity, macOS independent numeric and actual iPad/iPhone. Require explicit immutable ownership, no unknown-data loss, no original target execution, all local output D-only and five-second transient drive retries. Commit/push measured evidence.

The full command/preset Metal integration follows concrete commit acceptance; original-PC and physical-device gates remain open. None of the static or numeric child checks closes a full parity row.
