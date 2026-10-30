# P05-T02-A08 normalized curve-length mapping

Goal: provide real native normalized arc-length-to-parameter mapping needed by OutsideRail_V1 sampling. This is a kernel candidate operation, not the outside-rail algorithm or Rhino tolerance equivalence.

Pinned GCPnts_AbscissaPoint.hxx inspected: Length(adaptor,tolerance) and constructor(tolerance,adaptor,distance,startParameter); IsDone guards Parameter(). Native edge geometry is adapted with BRepAdaptor_Curve. Input fraction must be finite[0,1], tolerance finite positive. Exact0/1 return actual first/last parameters; interior uses native integrated length and abscissa solver. No linear parameter interpolation, managed synthetic point or swallowed nonconvergence.

- [ ] RED managed contract calls missing ParameterAtNormalizedLength; observe compile failure.
- [ ] Implement fixed-width C API, native exceptions/typed errors and output zeroing. Verify source API overload and use caller-supplied tolerance without claiming equivalence with Rhino normalized-length semantics.
- [ ] Add CSharp wrapper through existing gate/SafeHandle; validate returned finite parameter.
- [ ] Test50 fractions j/50 on actual ZX radius7 NURBS: evaluated points match analytic sin/cos at arc angles, monotone params and exact endpoints; a non-quarter fraction differs from naive parameter scaling. Test solid/invalid fraction/tolerance rejection and native counts.
- [ ] Run actual Windows/CSharp/native contracts using cached D: SDK, host C++ assertions and macOS simulator/device workflows. Keep outside-rail deformation/intersection/closest-point/seam/join and PC/physical tests open.
