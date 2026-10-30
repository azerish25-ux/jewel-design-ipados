# P05-T02-A24 automatic cap plane candidate

Prerequisite for BLD-0020/SER-0055. Inputs: current native NURBS exports and recovered CSUtilMath.AveragePlaneOfCurve; SDK5 TryGetPlane default1e-12. Published openNURBS6 pin f2dc7fba67017475b8d530835a40fcfb5372134a is a comparison reference, not original Rhino5 execution.

- RED missing CurvePlaneCandidate.TryPlanar/TryAveragePlane.
- Preserve published clamped IsLinear/control-order checks, line InPlane axis priority, largest control-triangle stepping, minimum-error unit normal selection, tangent alignment/snapping and1e-12 control-plane test.
- Recover fallback normalized samples .25/.33/.5/.66/.75, dot guard .01..3.14, unit-cross averaging and perpendicular-axis rule. Degenerate invalid planes remain explicitly unsupported.
- Test analytical oriented circles/reversals/lines, actual generated115/122 cap construction and edit/save/reopen, nonplanar fallback and native ownership. Independent source-body comparison still needed before claiming verified orientation equivalence.
- Full graph138/139 serialization/136 sign and join140 remain open. Run tests/repository checks; commit/push; use macOS Actions.

All local output D:;5-second retry on transient I/O. Recovery/kernel read-only.

Local result:20 plane cases/198 generated native cap boundary points and4 exact-source fallback comparisons pass; no native entries retained. Independent published-planar-body differential and new Apple artifacts are pending, not treated as completed parity.
