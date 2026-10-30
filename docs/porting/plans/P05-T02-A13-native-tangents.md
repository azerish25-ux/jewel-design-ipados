# P05-T02-A13 native unit tangents

Goal: provide the genuine derivative direction used by recovered CSOutsideRail1.Update to reverse a rail when TangentAt(Domain.Min).X<0. Candidate operation; full builder and original Rhino differential parity stay open.

Source: recovery/canonical/cad/CSBlocks.Rails.Obsolete/CSOutsideRail1.cs, lines170-172, input hash recorded in P05-T02. Pinned OCCT8.0.1 BRepAdaptor_Curve inherits GeomAdaptor_TransformedCurve.EvalD1; Geom_Curve.ResD1 contains Point and D1. Inspected actual header/implementation, not an older D1 API. The adaptor applies location transforms before returning derivatives.

Interface: CandidateContext.TangentAt(CandidateShape,double)->CandidateVector3. Fixed-width C ABI returns three doubles through existing locked context/SafeHandle ownership. Evaluate actual native derivative; normalize finite nonzero vector. Parameters must be finite and inside the current edge domain. Reject solid/invalid/nonconvergent/stationary output with typed errors. No finite-difference or analytic-circle substitute. Query creates no shape.

- [x] RED: shared managed contracts fail to compile because TangentAt is absent.
- [x] Add C ABI implementation/declaration and CSharp import/adapter; document bounded domain/unit normalization and error policy in ADR0017.
- [x] Actual C ABI and managed tests:50 normalized-length circle positions against independent unit sin/cos; endpoints, trim/reverse orientation, translated copy and affine scale/shear direction; source preservation; invalid/nonfinite/outside parameters, solid/null outputs and ownership counts.
- [x] Run D:-local Windows native and CSharp suite plus repository/provenance checks; record exact source hashes and output.
- [x] Dispatch/preserve macOS iPad/iPhone runtime and unsigned arm64 device build; mark verified only after inspecting reports.

Owned files: candidate_abi.h/.cpp, candidate_curve_contracts.cpp, probe.cpp, NativeMethods.cs, CandidateContext.Curves.cs, CurveAssertions.cs and plan/ADR/evidence. No changes to recovery, no full OutsideRail/PC/physical-device/process-memory claim. Arbitrary stationary or continuity-break NURBS fixtures remain a broader input-coverage gate.

Bounded [Apple execution/device reports](../../evidence/apple/native-curve-line-results.json) at5239e15 include these exact assertions. Original Rhino/PC/physical and broader cases remain open.
