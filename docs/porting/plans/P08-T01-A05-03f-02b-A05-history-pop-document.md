# P08-T01-A05-03f-02b-A05 history restoration and document effects

After A04 owned push, inspect original Pop merge helper0x44d3d0, SetView and client-screen restoration before implementing actual history restoration. Saved/current/default-temporary field selection is source-defined. Pop removes the last entry and updates cursor but returnsfalse after mutation; do not turn that into success=true or assume false means no change. Native pointer/global-window failure/undefined behavior is not an iOS contract.

Actual document ViewModified requires active-view/window timer coalescing, non-page synchronization and early return, optional document operation and controller notification/flag. Do not replace these with a fake revision counter or success callback. Bind actual iOS view/document/synchronization lifetimes before completing the camera commit adapter; preserve notification -> history -> final caller target update ordering.

Read original bodies statically only, hash all regions and resolve typed/called boundaries. Implement RED normal/boundary/failure cases only after the concrete behavior and collaborators are defined. Verify Windows/native/adjacent, macOS and actual iPad/iPhone. Keep local files/tools/cache/reports D-only and publish through the connected owner GitHub plugin, without shared CLI access. Full UI/PC/physical closure remains open.

[Merge audit](../../evidence/audit/rhino5-pop-view-merge-source.json) now establishes conditional double current-view assignment before saved-viewport replacement, target getter/setter effects, alias guards, restricted clean-name adoption and zero-screen-port call. Resolve native trim/compare, SetScreenPort and SetView before coding this as an ordinary saved-view replacement.
