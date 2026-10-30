# P05-T02-A03 combined recovered and native runtime proof

Goal: execute the existing recovered/production CSharp contract suite and genuine native CAD analytical cases in the same iOS app, then build that combination for unsigned ios-arm64. Separate successful apps do not establish coexistence in one link/runtime. This remains a test app, not builder regeneration or the application UI.

Architecture: reference existing tests/Smoke from tests/iOS.Native; do not copy recovered source or change any native fixture. After native status/report checks, execute ReuseSmoke.Checks.Run; write the final result only after both succeed. Runner must require both PASS prefixes and actual runtime family. Existing compression defect remains explicitly non-gating as in the suite; do not hide it or reinterpret all features as passing.

- [ ] Before change: native app references no recovered/production smoke project, and reports only native analytical results. Record this missing combined proof.
- [ ] Add Smoke project reference; run shared checks after native call; catch/write FAIL for either exception. Preserve native entry and archives.
- [ ] Require both native and managed PASS lines in simulator runner, keeping both families and current time budget.
- [ ] Watch tests/Smoke changes in candidate workflows; run repository checks plus native simulator/device macOS jobs, inspect actual artifacts and failures.
- [ ] Commit/push source, record exact commits/runs/hashes and bounded results. No PC project/geometry/UI/physical-device feature row is closed by coexistence.

Review correction: remove prior result/family/started files from the installed app data container before launching, and publish successful combined output by same-directory pending-file rename. This prevents stale results and partial success reads during manual reruns.
