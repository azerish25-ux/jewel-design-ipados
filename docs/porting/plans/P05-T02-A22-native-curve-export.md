# P05-T02-A22 native curve control export

Prerequisite for BLD-0020 / SER-0055 (CSProfileCap), not full acceptance of either row. Actual library records138/139 consume generated/transformed native curves; stored input definitions alone cannot supply their current controls. Input planning fixture library-item-4.csg SHA25673e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd remains unchanged.

Owned files: candidate_abi.cpp/.h; CandidateContext.NurbsExport.cs; CurveExportAssertions.cs; shared assertion runner and simulator gate; this plan/evidence/ADR.

- RED: compile shared native contract suite with missing ExportNurbs API.
- Implement bounded two-pass copied native NURBS export, Euclidean poles/optional weights/full knots, actual edge domain; explicitly reject unsupported periodic/non-NURBS shapes.
- Preserve untrimmed nonclamped definitions; trim copied basis only when edge interval differs. No source handle mutations or native retained entries.
- Compare rational/nonrational/nonclamped, circle, affine, trim, reverse, seam and join outputs with rebuilt native samples; detached owned definitions, foreign/disposed/BRep guards.
- Test raw ABI missing/short/null buffers, counts and no partial writes. Review and run native/shared/repository tests; commit/push; macOS simulator/device tests.

Average-plane/native orientation, fast bounds centering, cap metadata,138/139 registration and140 join remain separate. Recovery/kernel read-only; all local writes D:.
