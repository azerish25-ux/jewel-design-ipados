# ADR0020: native curve extrema bounds with explicit padding

Status: bounded Windows/iPad/iPhone native/CSharp assertions and unsigned device linking verified at07d0cae; full original-PC/application parity remains open.

Recovered CSOutsideRail1.Update falls back from numeric radius to a rail curve's accurate bounding box Max.Z. This is the absolute maximum-Z coordinate, not half the box height, distance from its center or a control-pole extent.

GetCurveBounds(shape,tolerance) uses pinned BndLib_Add3dCurve.AddOptimal on the actual finite BRep edge domain. BRepAdaptor.BSpline applies shape location; transformed/trimmed NURBS remain genuine native curves. The pinned implementation delegates to spline sampling plus numerical extrema refinement. It uses at least Precision::Confusion as its optimizer stopping tolerance and adds a box gap. It is not a formal arbitrary-spline extremum guarantee or established Rhino accurate-box equivalence.

CandidateCurveBounds reports unpadded Minimum/Maximum estimates, RequestedTolerance and actual KernelPadding. PaddedMinimum/PaddedMaximum reconstruct the native box margin. The C ABI retrieves GetGap, clears that gap on its temporary box and returns the underlying extrema separately. This avoids silently adding numerical padding to a physical ring radius. No sampled preview, pole hull, circle formula or managed geometry replaces native optimization.

The query requires positive finite tolerance and an edge; checks finite increasing domain, nonvoid/closed box, finite ordered raw/padded coordinates and nonnegative finite gap. Unsupported solids or invalid input fail through existing typed status handling. CSharp uses the context gate/SafeHandles and validates the returned finite ranges. Queries allocate no persistent native shape.

Tests compare actual circle,3D line, trimmed arc with interior maximum and sheared ellipse against independent analytical extrema at1e-6 model units, accounting for the native1e-7 optimization floor. The ellipse's tight maximum differs substantially from its pole hull. Translation, seam relocation, reversal, source preservation, requested/minimum gap, solid/invalid/missing output rejection and zero retained native entries are covered. This does not establish arbitrary oscillatory/imported or near-extrema curves at every requested tolerance.

Source hashes include recovered CSOutsideRail1, BndLib_Add3dCurve, GeomBndLib_Curve/BSplineCurve, spline helper and Bnd_Box at the pinned kernel commit. See [work card](plans/P05-T02-A16-curve-bounds.md) and [P05-T02 evidence](evidence/P05-T02.json).

Next is recovered outside-rail geometry composition with explicit defaults, sampled minimum-distance rule, ordered transforms, cached/helper vs current application tolerance, locked split/join/seam/direction branches and literal output scale0. Numeric/curve data selection and graph/CSBlock lifecycle still need integration. Original-PC, physical-device, cancellation/process-memory and full application parity remain open.

[Actual Apple reports](../evidence/apple/outside-rail-core-results.json) cover the named assertions at07d0cae. The later embedded matrix-profile codec is a distinct checkpoint.
