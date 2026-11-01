# P08-T01-A05-03c original perspective conversion

Continue the approved camera integration without delegation. Goal: preserve perspective conversion with the original Rhino5 dependency semantics, then connect the complete preset command and normalized Metal matrices. This card does not close full UI/PC/physical parity.

- [x] Compare exact original core functions with the published-source candidate: perspective, helper, target recenter, lens, location/frame and scalar validity. Record actual byte hashes, chained regions, typed views and differences. No original DLL/application execution; static Ghidra projects and all cache/output D:.
- [x] Implement the exact observed scalar validity difference with tests first. Original5 rejects negative unset and nonfinite but accepts positive unset; do not silently change the published6 candidate or assume every primitive otherwise matches.
- [x] Add original-compatible validation/frame/frustum contracts as evidence requires; implement perspective transition with explicit source return/mutation/threshold semantics. Add missing-API RED tests and independent compiled-source/reference cases.
- [x] Verify exact0.125 lens threshold, near correction, target-plane width preservation, locks, invalid/huge/sentinel data and failure mutation. Re-run adjacent camera/native tests and integrity.
- [ ] Use macOS Actions and actual iPad/iPhone execution; inspect artifacts, preserve remaining original primitive/PC/physical gaps, commit/push with task ID.

Inputs: core DLL SHA15c1be75b8ca3b0a42d2dc0c9b2a3de81730ff09b06ca16fb55d3a1a7a766e7c and source/binary audits. Existing reverse-engineering baseline read-only. All generated files/caches remain inside this D: repository; five-second retry on transient D: errors. Native cache/history/document adapters remain explicit parts of the complete command.

Windows result:18 scalar boundaries/196,608 high-word patterns,32 camera state checks/four analytical poses,26 viewport checks,39 adjacent Node regressions and full existing managed/native CAD suite pass. Missing-API RED preceded each new API. Apple execution remains pending.
