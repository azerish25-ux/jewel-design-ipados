# P05-T02-A16 native curve extrema bounds

Goal: support recovered CSOutsideRail1's missing-numeric-radius fallback to accurate curve box Max.Z. Use actual native geometry over the current finite edge domain; control-pole boxes and sampled previews are not replacements.

Pinned BndLib_Add3dCurve.AddOptimal delegates to GeomBndLib_Curve and the spline helper's sampling plus numerical extrema refinement. Its stopping tolerance is at least Precision::Confusion and it adds an explicit Bnd_Box gap. BRepAdaptor.BSpline applies shape location. Record these limits; SDK output is not established original Rhino accurate-box equivalence or a formal arbitrary-spline extremum guarantee.

Interface: CandidateContext.GetCurveBounds(shape,tolerance)->CandidateCurveBounds with unpadded Minimum/Maximum estimates, requested tolerance and actual KernelPadding. PaddedMinimum/Maximum expose native margin separately. Native C ABI returns six extrema values and gap, rejects invalid/open/void/nonfinite responses, nonpositive/nonfinite tolerance and non-edge input. Query owns no new shape. Never silently add padding to a physical ring radius.

- [x] RED missing GetCurveBounds API/DTO.
- [x] Implement genuine native optimal bounds and gated/SafeHandle CSharp adapter; ADR0020 defines raw/padded meaning and numerical limits.
- [x] Native and managed analytical tests: circle, 3D line, actual trimmed arc with interior maximum, affine/sheared ellipse analytic amplitudes (distinct from pole hull), translated/seam/reversed curves, explicit minimum/requested padding, invalid tolerance/solid/null outputs, input preservation and no retained entries.
- [x] Run D: actual native/CSharp suites, repository/provenance checks and review; record exact source hashes and output.
- [x] Dispatch host/macOS iPad/iPhone and unsigned device builds, preserve existing runs. Runs41061366581/41061370035 atdf208e2 confirmed live/queued.
- [x] Inspect Apple reports before verification. Full radius-data/channel selection, OutsideRail/caller composition, arbitrary near-extrema/imported curves and original-PC/physical/cancellation/process-memory acceptance stay open.

Owned: ABI header/cpp/native contracts/probe, NativeMethods/CandidateContext.Curves/CandidateCurveTypes, new shared bounds tests/runner, simulator markers, plan/ADR/evidence. Next: full recovered outside-rail geometry algorithm using these primitives and explicit source policy.

Bounded [Apple geometry/runtime/device evidence](../../evidence/apple/outside-rail-core-results.json) at07d0cae inspected. Wider original-PC/chooser/graph/physical/application gates remain open.
