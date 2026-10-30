# P01-T06-A01 fresh simulator launch evidence

Requirement: P01-T06 actual iPad/iPhone family assertions must come from the current invocation. This is a test-laboratory repair, not a new application feature.

Observed run [41046672575](https://github.com/azerish25-ux/jewel-design-ipados/actions/runs/41046672575) failed with "Simulator result missing". Downloaded iPad app-console contains Managed Main and the complete passing recovered report. Boot finished at11:19:56Z; managed Main arrived at11:21:50Z. The background console launch returned before startup; the60 one-second poll budget could expire before execution. The old harness also retained earlier result files.

Design: use the direct launch pattern already exercised by native tests. Obtain container after installation, remove previous result/family/started/pending files, propagate launch exit status, wait at most300 polls and require a freshly published report plus matching family. Publish both success and failure via pending file and atomic rename. Keep all original checks and diagnostics. Extend the workflow step to20min to accommodate first-boot migration and bounded polling. No retries may turn assertion failures into passes.

- [x] RED: delayed fresh scheduling and stale-success regressions fail against original launcher.
- [x] GREEN: five shell-executed scheduling/publication controls plus four device-selection tests pass on Windows Git Bash; all fixtures/temp remain under repository .local.
- [x] Wire the same regressions into Ubuntu repository CI.
- [x] Inspect launcher/publisher diff for correctness, ownership of output, bounded waits and unchanged recovered source.
- [ ] Confirm actual macOS iPad/iPhone execution after fix; mocked simctl tests prove harness control flow only.

Owned files: scripts/run-simulator.sh, tests/iOS/Main.cs, tests/simulator-launch.test.cjs, two workflow files and this evidence. Original source and geometry assertions unchanged. See [record](../../evidence/apple/simulator-launch-repair.json).

Full offline application, physical device, compression defect and PC compatibility remain separate open work.
