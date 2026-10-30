# ADR0041: owned native curve control export

Status: local native/shared suite passes; Apple execution pending. Prerequisite for BLD-0020 and SER-0055, no completed parity row.

CSProfileCap consumes a generated NurbsCurve, then changes its plane and centers it before RailRevolve. Stored input coefficients cannot represent the current transformed/generated curve. CandidateContext.ExportNurbs now obtains actual native coefficients through a two-pass counted ABI. It returns a detached immutable CandidateNurbsDefinition with Euclidean XYZ poles, optional rational weights and full nonperiodic knots.

The native query copies the actual edge basis through existing copiedEdgeSpline (including native affine/location behavior). If the basis domain differs from the actual edge interval, only the copy is segmented. Otherwise original nonclamped controls/knots remain unchanged. Periodic and non-NURBS shapes are explicitly unsupported. No native shape handle is created or retained. Queries serialize under context locks; managed buffers are allocated only after bounded metadata validation and metadata must match on both passes.

Null arrays with zero capacities query required dimensions. Short/missing data buffers report required dimensions and a typed error before any data write. Missing metadata outputs fail; metadata is reset before failed shape queries. Raw tests exercise short XYZ/weight/knot buffers, missing weights/metadata and BRep rejection, sentinel preservation and real circle coefficients. Caller pointer ranges remain the caller's responsibility, as for the rest of candidate ABI1.

Shared contracts cover14 curve cases/462 actual native export-reimport points: rational circle, affine, trim, reverse, moved seam, exact nonclamped rational definition, linear and joined curves, then actual library115/122 and their edited/saved/reopened generations. Original domains and source inspections remain unchanged; the no-edit serialized fixture is byte-identical. Definitions survive producer/context disposal; foreign/disposed/BRep requests fail and native entry count returns to zero.

This adds a prerequisite for cap plane placement. It does not establish Rhino TryGetPlane orientation, fast bounding-box policy, cap surfaces138/139, legacy join140, original-PC regeneration or physical-device parity. [Evidence](evidence/P05-T02-A22.json).
