# P05-T02-A17 recovered OutsideRail_V1 geometry composition

Goal: adapt both recovered Update geometry branches to actual native operations, preserving defaults3/2/1/false,50 normalized-length samples (exclude endpoint), sampled minimum origin distance, numeric-or-curve-Max.Z radius selection, finite axis rays, ordered pivot transforms, locked split/top-bottom/join/seam/tangent and literal output scale0. This is a source-derived geometry core, not full CSBlock/chooser/graph/UI/PC parity.

Primary source bodies: CSOutsideRail1.cs, CSUtilGeometry.GetMinMax, CSUtilRingRail extent helpers, CSUtil.Tolerance and CSNumberData.GetBoolean (>0.5). Official RhinoCommon wrapper corroborates join default0 (native policy unresolved) and absolute tolerance plus fractional1e-8 for length arrays. Candidate settings take explicit helper/application/effective join tolerances; native operation equivalence remains gated.

Interface: Build(context, outsideCurve, numericRadius?, settings, parameters?, radiusCurve?) -> owned OutsideRailCandidate with Curve, ResolvedRadius, MinimumSampledDistance, OutsideScale0, JoinDisposition and explicit native fallback diagnostics. Preserve caller input ownership. Missing/zero inputs use recovered messages; native unsupported errors stay visible. Two-curve merge explicitly orients copies, returns unmerged real inputs if disconnected, and records native KernelFailure if source empty-join fallback is used. Seam Boolean no-op/failure policy is surfaced, not hidden. Add native current-edge closure query for source ignored-false seam behavior.

Analytical unlocked circle output has Z center(top-bottom)/2 and Z semiaxis radius+(top+bottom)/2. X semiaxis is (radius+side)/sqrt(1-centerZ^2/semiZ^2), because the source recomputes X-axis intersections after Z stretching. Locked output is two origin-centered half ellipses, X semiaxis radius+side, separate Z semiaxes radius+top/radius+bottom and final positive-Z seam/+X direction. These are not generic offsets.

- [x] RED shared feature/closure APIs absent.
- [x] Implement CSharp geometry core with real native operations, exact source ordering/defaults, exception-safe temporary ownership and explicit fallback diagnostics; ADR0021 records policy limits.
- [x] Add strict read-only independent record13 header/input oracle (six links, fully consumed, exact GUID/version) and tests. Radius/thickness/lock stored candidates remain distinct from evaluated channel/chooser geometry.
- [x] Actual tests: analytical locked/unlocked/default/asymmetric cases, input reversal/seam, radius fallback/priority, sampled vs closest ellipse minimum, missing/zero/failure cleanup, current/cached tolerance distinction, real stored parameter edit/save/reopen/rebuild/undo with explicit input profile.
- [x] Run D: native/CSharp, reference regressions, repository/source provenance checks and review; preserve all earlier runtime cases.
- [x] Dispatch actual Apple host/iPad/iPhone and unsigned device builds, preserving existing runs. Runs41069909325/41069913934 at07d0cae confirmed running.
- [ ] Inspect actual family/native/managed reports and device platform/warnings before bounded platform verification.

Remaining gates: original Rhino join default0/native Boolean failures/order/parameterization, arbitrary degenerate/unjoined/fallback cases, cached application provider integration, complete channels/IsOn/override/CSBlock/output-rail data/export, actual chooser profile and full original-PC/physical/cancellation/process-memory parity. No full feature row closes from this card alone.

Both normal branches and representative source tolerances pass. Composition exposed and fixed uniform SetForm scale collapse; diagnostics/fallback/source-wrapper/batch-solver limits are detailed in ADR0021. Actual chooser record7 generation and full graph/PC output remain open.
