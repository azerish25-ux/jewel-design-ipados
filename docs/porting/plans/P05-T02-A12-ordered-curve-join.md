# P05-T02-A12 ordered native NURBS pair join

Goal: real endpoint-connected native NURBS concatenation needed before porting OutsideRail_V1 split/join. Candidate operation only; Rhino automatic ordering/reversal/multiple-output and parameterization parity remain unverified.

Pinned GeomConvert_CompCurveToBSplineCurve(BasisCurve) and Add(NewCurve,tolerance,After=true,WithRatio=true,MinM=0) inspected. Copy located NURBS curves/restrict to edge intervals; require ordered end-to-start distance<=finite positive tolerance, then native concatenation and validated owned BRep edge. Failure must be typed, not return an unchanged input or synthetic polyline.

- [ ] RED managed JoinCurves API absent before implementation.
- [ ] C ABI receives2 handles and tolerance; validates same context/native ownership and native continuity result. CSharp uses existing gated factory/SafeHandles.
- [ ] Actual tests join two quarter arcs into half circle with analytic length and endpoints; reversed/disconnected ordering and invalid tolerance reject; source curves remain unchanged; joined output survives source disposal; endpoint projections remain valid.
- [ ] Native host direct assertions and Windows/CSharp then Apple simulator/device builds. Original Rhino curve ordering/ties/multi-output, seam/periodicity and full outside-rail/PC/physical/cancellation/leak gates stay open.
