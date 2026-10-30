# P05-T02-A18 real stored NURBS construction and profile selection

Goal: build actual native geometry from all155 decoded curves and feed stored chooser6/profile0 into the recovered OutsideRail core, replacing the explicit test carrier. Full active graph/CSBlock/UI/PC parity stays open.

Pinned OCCT Geom_BSplineCurve constructors require Euclidean poles/optional positive weights and unique knots/multiplicities. Official openNURBS SuperfluousKnot defines the two omitted end knots;3 fixture curves are non-clamped and degrees span1/2/3/5. Preserve source domain/coefficient meaning without polyline fitting or circle substitution.

- [x] RED generic CreateNurbs and stored-profile adapter APIs absent.
- [x] Add immutable generic full-knot definition and counted C ABI factory, native finite/count/weight/knot/multiplicity/domain guards and ownership; fixed model-space coordinates, explicit unsupported limits.
- [x] Adapt stored short knots/homogeneous coordinates using pinned superfluous-end convention; construct actual native profile with name/sorted sweep parameters/identity transform semantics. Serialized names remain labels. Stored chooser forwarding explicitly excludes activation/override/global graph evaluation.
- [x] Compare actual native points/domain/degree/rationality for all155 curves against independent homogeneous de Boor evaluation using raw data, including3 non-clamped profiles; test input isolation/lifetime and malformed native buffers.
- [x] Execute OutsideRail using actual stored selected curve, real radius/thickness/lock candidates, edit/save/reopen/rebuild/undo; distinguish native geometry rebuild from full PC project serialization.
- [x] Run Windows native/shared/document/reference/provenance gates, review and record source hashes.
- [x] Push and dispatch actual macOS simulator/device tests; preserve existing runs. Runs41079138260/41079142975 atd1caac5 confirmed running.
- [x] Inspect actual family/native/import/chooser and AOT platform reports before bounded verification.

Limits: negative/zero weights, native degree/resource/continuity limits or invalid data must reject explicitly, never fit a surrogate. Native periodic representation, extrapolation and original Rhino numeric/ordering policies remain comparison gates. General chooser nesting/IsOn/override/channels/output-data and PC/physical/device-performance acceptance remain in scope.

Bounded [Apple native-import execution/AOT reports](../../evidence/apple/stored-nurbs-import-results.json) atd1caac5 inspected; active graph/PC/physical parity remains open.
