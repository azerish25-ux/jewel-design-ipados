# P05-T02-A10 bounded curve closest-point projection

Goal: provide actual native curve closest-point data needed by OutsideRail_V1 seam/trim operations. Candidate query only; original Rhino solution/tie ordering and tolerances remain unverified.

Pinned BRep_Tool::Curve(edge,first,last) returns located3D curve; GeomAPI_ProjectPointOnCurve(point,curve,first,last) supplies native orthogonal candidates. Compare endpoints too because bounded closest points need not be interior stationary solutions. Reject failed native extrema computation; do not replace it with an invented point. Strict distance improvement chooses first among equal candidates (first endpoint,last endpoint,SDK candidate order), explicitly not Rhino parity.

- [ ] RED managed ClosestPoint contract missing before implementation.
- [ ] C ABI validates finite input and output pointers, returns parameter/point/distance, fixed widths and typed failures. Use actual edge geometry/location/domain and preserve source ownership.
- [ ] Test circle analytic nearest point for3D off-plane input, transformed copy, seam point, parameter evaluation consistency, unsupported solids/nonfinite points and no retained entries.
- [ ] Run actual local native/CSharp, C ABI host assertions and Apple simulator/device workflows. Future open-curve endpoint/tie/degenerate/general-curve cases and original-PC behavior remain required.
