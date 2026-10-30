# P05-T02-A15 copied closed-curve seam relocation

Goal: provide genuine seam relocation needed by CSOutsideRail1 locked split/rejoin and final orientation. Preserve the documented Rhino5+ domain-start behavior, without claiming original binary/full builder equivalence.

Primary contract: https://developer.rhino3d.com/api/rhinocommon/rhino.geometry.curve/changeclosedcurveseam (Since5.0): new start/end is curve(t); resulting domain starts at t. Recovered source calls this before reverse/closest/trim and after final ray/closest selection. Pinned Geom_BSplineCurve.hxx/.cxx SetPeriodic/SetOrigin/Segment implementation inspected.

Interface: CandidateContext.RelocateClosedCurveSeam(shape,t) returns an independently owned copied NURBS. Restrict copy to current edge domain, require closed, convert to periodic native form, set origin to t and segment one full original period to clamped nonperiodic output. Native evaluators must preserve source points at corresponding wrapped parameters, length, orientation and endpoint closure. Only finite t within the current domain is supported; outside-period normalization/Rhino failure policy remain separate.

- [x] RED missing managed RelocateClosedCurveSeam.
- [x] Add C ABI/query adapter, native typed failures/zero output and ADR0019; no circle-specific geometry or polyline substitute.
- [x] Native/managed tests: knot and non-knot circle seam, new domain[t,t+period], full wrapped point samples, reverse/tangent/trim composition, translated/affine and closed piecewise-linear corner curves, source/output ownership, open/solid/nonfinite/outside rejection.
- [x] Run actual D: Windows native/CSharp suite plus repository/provenance checks. Record source hashes/commands/logs and review.
- [x] Dispatch Apple native host/iPad/iPhone and unsigned device builds; preserve existing jobs. Runs41059567171/41059570457 atf657de6 confirmed live/queued.
- [x] Inspect actual Apple reports before bounded verification. Full OutsideRail, original-PC, arbitrary near-closed/continuity cases, physical-device/cancellation/process-memory gates stay open.

Owned: ABI header/cpp/native contracts/probe, NativeMethods/CandidateContext.Curves, new shared seam contracts/runner, native simulator markers, plan/ADR/evidence. Accurate bounds and complete graph/caller composition remain required.

Affine composition initially failed because SDK NewCurve wraps splines in an exact trimmed curve. Unwrapping its basis while retaining edge domain fixed seam/trim/reverse; actual regression suite passes. Apple markers now require direct native and managed seam reports.

Bounded [Apple geometry/runtime/device evidence](../../evidence/apple/outside-rail-core-results.json) at07d0cae inspected. Wider original-PC/chooser/graph/physical/application gates remain open.
