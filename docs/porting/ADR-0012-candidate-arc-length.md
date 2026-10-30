# ADR0012: candidate normalized arc-length parameters

Status: bounded Windows/iPad/iPhone native and CSharp assertions verified; unsigned device build passed. See [Apple evidence](../evidence/apple/native-arc-length-results.json).

OutsideRail_V1 samples50 normalized arc-length fractions before deformation. The candidate exposes ParameterAtNormalizedLength(shape,fraction,absoluteTolerance) through the existing context/shape ownership boundary. It integrates actual edge length and runs the native GCPnts abscissa solver, with IsDone and finite/domain guards. Fractions0/1 return exact edge-domain endpoints. Invalid fractions/tolerances and unsupported solid queries return typed failures; no linear parameter interpolation or synthetic point substitutes.

The supplied tolerance is a candidate model-space integration/solver tolerance. Equivalence with Rhino Curve.NormalizedLengthParameters and its cached application tolerance is unproven. Native mapping is a prerequisite, not an implementation of OutsideRail_V1 transforms/intersections/seam/trim/join or full PC parity.

Validation includes50 actual ZX-circle samples against analytic arc angles, monotone parameters, endpoints, a non-quarter fraction differing from naive parameter scaling, and invalid bounds. Both C ABI and managed ownership paths execute real native geometry locally. Mac iPad/iPhone reports and unsigned device build are recorded; physical execution and original Rhino differential proof remain required.
