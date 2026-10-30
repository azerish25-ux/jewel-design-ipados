# ADR0017: candidate native unit tangent query

Status: bounded Windows/iPad/iPhone assertions and unsigned device linking verified at5239e15; full Rhino/PC/physical parity remains open.

Recovered CSOutsideRail1.Update chooses final direction from TangentAt(Domain.Min) dot XAxis after relocating the seam. Add CandidateContext.TangentAt(shape,parameter) returning CandidateVector3, backed by jd_candidate_curve_tangent and the existing context gate and SafeHandles. This is a query with no new native ownership.

The pinned OCCT8.0.1 adaptor exposes EvalD1(parameter), not the old D1 output-argument API. Geom_Curve.ResD1 contains the native derivative. GeomAdaptor_TransformedCurve applies shape location to point and derivative. Evaluate that derivative, verify finite components, normalize with a finite nonzero hypot, and return three doubles. Solid, invalid/foreign/released handle, nonfinite/out-of-domain parameter and undefined derivative errors remain explicit. No finite differences, circle formula or guessed direction in production.

Parameter range is the current edge domain including endpoints. Extrapolation, one-sided derivative selection at arbitrary continuity breaks and Rhino stationary-curve fallback behavior are not established. The recovered caller only requests the start parameter; this operation alone does not port its intersections, seam selection, automatic join ordering, failure policy or complete graph lifecycle.

Tests compare50 actual directions to independent analytic sin/cos after native arc-length mapping; endpoints, quarter trim/reverse sign, affine normalization, translation, input preservation and typed invalid requests are covered. Native entry counts return to zero. Arbitrary imported stationary/continuity-break NURBS, original-PC, physical-device and process-memory leak gates remain open.

Sources inspected at kernel commit b8f597c677811d1f9f4d8a97f5ae2825c0353a42:
- Geom_Curve.hxx: ResD1 and EvalD1.
- GeomAdaptor_TransformedCurve.hxx/.cxx: inherited evaluator and derivative transformation.
- BRepAdaptor_Curve.hxx: inheritance and edge domain.

Hashes and commands are recorded in [P05-T02 evidence](evidence/P05-T02.json). Existing recovered source remains unchanged.

Bounded Apple execution/device proof for the ordered-join, unit-tangent and finite-intersection assertions is recorded together at checkpoint5239e15: [actual reports](../evidence/apple/native-curve-line-results.json). Later seam/bounds/outside/uniform-affine work is a separate checkpoint.
