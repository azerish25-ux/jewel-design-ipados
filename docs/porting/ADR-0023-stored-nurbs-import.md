# ADR0023: exact stored NURBS import and stored profile selection

Status: bounded Windows/iPad/iPhone155-curve import/all5 profile-choice assertions and unsigned device linking verified atd1caac5.

CandidateNurbsDefinition copies standard full knots, Euclidean3D poles and optional declared rational weights. CreateNurbs passes counted primitive arrays through the context/SafeHandle factory to a real Geom_BSplineCurve and validated BRep edge. The native boundary checks finite coordinates/knots/positive weights, counts/domain/multiplicity and explicit degree/resource/continuous-edge limits. Unsupported input never becomes a fitted polyline, mesh, cached preview or circle.

StoredMatrixProfileCandidate imports the decoded direct legacy representation: rational homogeneous XYZ values are divided by their weight; weights remain separate. Two omitted end knots follow the pinned ON_SuperfluousKnot convention, including non-clamped cases. Unique knot/multiplicity conversion occurs natively without merging distinct values. Native rational flag is retained even for equal weights. Stored2D controls map to XY/Z0; all fixture curves are3D. Source archive data remains untouched.

The returned profile owns native geometry, preserves its name as a label and copies/sorts sweep parameters to match CSProfileData constructor behavior. No serialized legacy path is opened. FromStoredChooser accepts exact decoded chooser1.1 and its stored InObjects source reference, truncating the stored selection as the recovered getter does. It explicitly does not evaluate IsOn, disabled/override/CAM inputs, nested choosers, full channel graphs or CSBlock/output-data lifecycle.

The fixture's155 curves use degrees1/2/3/5 and include three non-clamped profiles. Actual native evaluation at17 parameters each (2635 points) matches an independent homogeneous de Boor evaluator using raw stored data within1e-6 model units. The evaluator does not call native creation, abscissa or curve evaluation. Domain/degree/pole count/rationality, names, sweep ordering and native ownership also match. A rational quadratic quarter-circle independently verifies the C ABI constructor; malformed counts/pointers/weights/knots, output zeroing and managed definition isolation reject or preserve inputs correctly.

The stored chooser6 -> group7 path now feeds actual imported geometry into OutsideRail_V1. Tests select all five stored profiles, edit/save/reopen the chooser scalar, rebuild the real outside rail, verify closure/final top seam/positive-X direction/no fallback diagnostics and undo to byte-identical original archive. These are live geometry/data tests; they do not save regenerated output data into a complete PC portfolio or prove original-PC regeneration.

Candidate geometry uses a nonperiodic native representation over the valid stored domain. Periodicity metadata/extrapolation, negative or zero projective weights, degree/resource/continuity limits, arbitrary geometry and original Rhino numerical/tie/ordering behavior remain explicit gates. Original file metadata remains in the immutable document, not inferred from reconstructed native topology.

Next: bind decoded headers/links to real graph/chooser activation and override behavior, output rail-data properties and dependent builders; retain original-PC/physical-device/rendering/performance/cancellation/process-memory and full application parity requirements.

[Work card](plans/P05-T02-A18-stored-nurbs-import.md), [task evidence](evidence/P05-T02.json), [pinned end-knot convention](https://github.com/mcneel/opennurbs/blob/eb92af3ba1806b0a34a99aba0d3bda83e3d46083/opennurbs_knot.cpp).

[Actual Apple import reports](../evidence/apple/stored-nurbs-import-results.json) preserve selected/actual families, assertions and hashes; new channel evaluation is a separate checkpoint.
