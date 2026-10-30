# P04-T01-A03 embedded matrix-profile records

Goal: decode real CSGroupOfMatrixProfiles record7 rather than substitute an invented profile, and structurally re-encode its stored data without byte loss. Native curve import/chooser execution is the next separate step.

Sources: CSGroupOfMatrixProfiles.Read/Write/Update, CS.CSUtilIO curve/double/string arrays, CSBlock/CSObject headers, CSProfileData and chooser forwarding. Pinned official openNURBS NurbsCurve.Read/Write corroborates legacy direct1.0 curve fields (packed version byte, dimensions/rational/order/CV count, reserved integers, invalid bbox, short knots, homogeneous CV array). Fixture has5 cubic curves/156 CVs, domains0..1, two rational and three nonrational, five sweep arrays and serialized legacy path labels.

- [x] RED typed MatrixProfiles/header APIs absent before production changes.
- [x] Strict independent Node reader and raw-bit oracle; fully consume profile record, verify curve/count/array layout, preserve unknown outer/nested versions without guessing.
- [x] Add immutable CSharp profile/NURBS data and exact structural encoder for known group1.0(no names)/1.1(names), legacy NURBS1.0 only. Preserve reserved/bbox/string termination/raw double bits; configured bounds and malformed count guards. Unsupported nested schema remains opaque.
- [x] Share decoded header contract with scalars and project known group header/links without creating scalar edit handles. Scalar74/100 counts unchanged; group7 read-only geometry.
- [x] Shared tests compare every coefficient/knot/bbox bit/name/sweep against independent oracle; exact no-op, unrelated scalar edit/reopen, synthetic1.0, unknown outer/nested, malformed/truncated/count and NaN/-0 preservation, immutable views/header projection.
- [x] Run local document/shared/native regressions, repository/provenance/reference checks; review ownership/bounds/no legacy path resolution.
- [x] Push and dispatch actual Apple simulator/AOT tests. Runtime41075741858, device41075740585 and repository41075741873 at49cf8b3 confirmed running.
- [ ] Inspect actual family/profile raw-bit/version reports and device platform/warnings before bounded platform verification.

Limits: no file access through serialized C: names, no BinaryFormatter or original native execution. No claim of arbitrary curve versions, profile UI/chooser/graph/transform/sweep behavior, native knot/weight import, original-PC regeneration or physical-device parity. Known data becomes available for subsequent native construction.

Fixture-wide discovery expanded the work from record7 to all6 groups/155 curves/4012 controls. Actual document/native-adjacent and independent raw-bit/reference tests pass. Native geometry creation/chooser/PC execution remain separate; Apple proof pending.
