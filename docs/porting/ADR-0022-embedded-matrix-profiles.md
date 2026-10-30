# ADR0022: embedded matrix-profile records and raw NURBS data

Status: Windows shared/document/native-adjacent tests pass; Apple validation for this new codec is pending.

CSGroupOfMatrixProfiles serializes base object/block header, direct curve array, sweep parameter arrays and (from group1.1) names. CSUtilIO writes each OnNurbsCurve directly, without an object-class wrapper. The pinned official openNURBS NurbsCurve1.0 layout corroborates the observed packed version byte, dimension/rational/order/control count, reserved integers, invalid bounding box, short knot array and homogeneous control values.

The fixture contains six known1.1 groups at records7/31/63/90/160/194:155 stored curves and4012 control vertices. Record7 alone contains five outside-rail profiles; chooser6/channel3 forwards a selected group output. Their Windows path names are serialized labels and are never resolved as file access by the codec.

LegacyCsg4MatrixProfiles and LegacyNurbsCurveData expose immutable raw data. Rational control coordinates remain homogeneous; no division by weights, knot expansion, sorting, geometric repair or native construction occurs while reading. Reserved integers, invalid bbox values, all double bit patterns, names/string termination and array ordering survive structural encoding. Nonfinite raw coordinates remain representable; a later geometry importer must reject unsupported/invalid geometry explicitly.

Known group1.0/1.1 and nested direct NURBS1.0 are decoded. Unknown outer or nested curve formats leave the complete record opaque; malformed known counts/layouts fail. Dimension/order/control/knot consistency, configured counts and bounded byte availability are checked before large arrays. Curve allocation grows only as parsed input is consumed. Group list sizes are preserved independently; no missing sweep/name entries are fabricated.

ILegacyCsg4Header unifies decoded scalar/group metadata for projection. The six group headers/links become known without creating editable scalar handles. The fixture has106 known headers,100 editable stored scalars (74 bounded numeric),6 decoded groups and135 other opaque records. Profile data has no public mutation API yet; scalar edit requests on groups reject.

Shared assertions compare every knot/control/bbox bit, reserved integer, name and sweep against an independent Node reader, covering all155 curves. They verify exact structural no-op, scalar edit/reopen preservation, synthetic group1.0 without names, unknown outer/nested versions, malformed/truncated/count cases, immutable views and raw NaN/negative-zero preservation. Original scalar/header/identity/archive and actual native outside-rail tests still pass.

This makes genuine profile data available; it does not instantiate native curves, evaluate the chooser, apply activation/override/CSBlock behavior, regenerate a PC project or implement group editing/UI. Next is native construction from the stored short-knot/homogeneous representation with independent point evaluation, then the actual chooser/profile-to-outside-rail path.

[Work card](plans/P04-T01-A03-matrix-profiles.md), [task evidence](evidence/P04-T01.json), [independent oracle](../evidence/audit/matrix-profile-reference.json), [pinned format source](https://github.com/mcneel/opennurbs/blob/eb92af3ba1806b0a34a99aba0d3bda83e3d46083/opennurbs_nurbscurve.cpp). Modern source corroboration is distinct from original CounterSketch/Rhino runtime proof. Full serializer, original-PC/physical-device/cancellation/process-memory and application parity remain open.
