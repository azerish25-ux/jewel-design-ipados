# ADR0019: copied native closed-curve seam relocation

Status: bounded Windows/iPad/iPhone native/CSharp assertions and unsigned device linking verified at07d0cae; full original-PC/application parity remains open.

Recovered CSOutsideRail1.Update relocates closed seams before reverse/closest/trim and after its final ray/closest selection. The [official RhinoCommon contract](https://developer.rhino3d.com/api/rhinocommon/rhino.geometry.curve/changeclosedcurveseam), available since5.0, places the start/end at the requested parameter and starts the resulting domain there. This is a documented target contract, not an original PC binary comparison.

RelocateClosedCurveSeam(shape,t) returns a separate owned native NURBS through the existing context gate/SafeHandle factory. It deep-copies the located source basis, restricts to the current edge interval, requires native closure, converts to periodic representation, calls SetOrigin(t,0), and segments one original period into nonperiodic output. Requested t must be finite and inside the current edge domain. Output domain is checked against[t,t+period] within16 domain-scaled machine epsilons; overflow, open curves, solids and invalid requests fail explicitly. No analytic-circle generation, polyline approximation or source mutation substitutes for the operation.

Composition testing exposed an existing helper defect: BRepTools_GTrsfModification.NewCurve wraps transformed splines in Geom_TrimmedCurve. The old copiedEdgeSpline direct downcast rejected those exact transformed NURBS. The helper now unwraps exact trimmed bases while preserving the located edge interval, then deep-copies the spline. Seam, trim, reverse and join callers share this correction. No arbitrary non-NURBS conversion or approximation was added.

Actual tests verify knot/non-knot/domain-end and repeated circle seams,65 wrapped parameter samples per case, length/closure/direction, affine-transformed input, a closed degree1 rectangle with corners, reverse/tangent/closest/trim composition, affine trim/reverse, original input preservation, independent output after source disposal, invalid/open/solid/missing outputs and zero retained native entries. Direct C ABI and managed paths both execute the actual cached native kernel.

Candidate restrictions remain explicit: in-domain parameters only; native closure and knot tolerances; typed exceptions instead of Rhino's Boolean failure; general near-closed, discontinuity/near-knot/tie and topology-import policies need wider fixtures/original PC comparison. Complete outside-rail caller and graph lifecycle, accurate bounds, automatic join orientation, physical-device/cancellation/process-memory and full app/PC parity remain open.

Source hashes include recovered CSOutsideRail1.cs, pinned Geom_BSplineCurve.hxx/.cxx and BRepTools_GTrsfModification.cxx. See [work card](plans/P05-T02-A15-closed-seam.md) and [P05-T02 evidence](evidence/P05-T02.json).

[Actual Apple reports](../evidence/apple/outside-rail-core-results.json) cover the named assertions at07d0cae. The later embedded matrix-profile codec is a distinct checkpoint.
