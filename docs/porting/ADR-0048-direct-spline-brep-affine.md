# ADR0048: direct affine modification of explicit spline BReps

Status: local native/shared regression suite passes; new Apple affine execution pending. Original-PC/full affine-family parity remains unverified.

A27 reconstructs the earlier failing local cap face from actual115/122 plus the inverse transform carried by138/139. The generic native affine path reproduced invalid BRep: face UnorientableShape27 and wire NotClosed28. Native validation now reports bounded subshape/status diagnostics instead of only an undifferentiated invalid-BRep message.

Pinned BRepBuilderAPI_GTransform always runs NurbsConvert before modification, including shapes that already contain explicit spline geometry. For the reproduced collapsed-apex face, bypassing that redundant conversion and applying the existing BRepTools_GTrsfModification directly preserves valid topology. The candidate preflights every surface and edge: only direct BSpline/Bezier (or degenerate null edge-curve) geometry takes the direct path. Analytic/wrapped representations keep the existing conversion path. BRepTools_Modifier is nonmutable on input; surface/curve modifications copy geometry. Recovery and pinned kernel remain unchanged. [Source observations](../evidence/audit/direct-spline-affine-kernel.json).

The full supplied affine coefficients remain in gp_Other. There is no gp_Trsf orthogonalization, SetForm scale reclassification, matrix snapping, tolerance inflation, forced cap reconstruction or dropping of shear/nonuniform transforms. This distinction matters because a rigid-only workaround would leave real scaling/mirroring cases broken.

Twenty native cases/350 direct coefficient-mapped surface points pass at1e-8: original local cap placements, existing world caps, identity/rigid/uniform/nonuniform/shear/reflection, and joined140 volume scaling by determinant. Surface parameter domains and original geometry/tolerance bounds remain unchanged; final native entry count is zero. Earlier curves/arrays/graph/sewing and native ABI analytical cases remain passing.

The observed explicit-spline failure is resolved. This does not prove all analytic/wrapped pole geometries through conversion, every degenerate family, cached triangulation/reflection behavior, original Rhino5 numerical equivalence or physical-device behavior. Those remain wider acceptance gates. [Evidence](evidence/P05-T02-A27.json).
