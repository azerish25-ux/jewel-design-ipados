# ADR0040: recovered Rhino5 dome profile semantics

Status: local source/native profile comparisons pass; Apple dome checks pending. Full cap placement/tensor/join behavior remains unfinished.

The acquired RhinoCommon5.1.30000.16 assembly's NurbsCurvePointList.SetPoint(int,Point4d) forwards the point to ON_NurbsCurve_SetCV2. The published wrapper at pinned commit3aebe3553b5e2c78ed4d5ae1c21f3e752a6b7f69 multiplies input XYZ by W before passing the homogeneous native point. This agrees with [McNeel's version explanation](https://discourse.mcneel.com/t/nurbscurve-different-behaviour-between-rhino-net-and-rhinocommon/75376/2): Rhino5 takes Euclidean XYZ plus weight; Rhino6 changes semantics. [Static artifact evidence](../evidence/audit/rhino5-control-point-setter.json) records hashes and the acquired/published distinction. No original SDK/native code was executed.

CreateDomeRhino5 adapts the recovered CreateDomeRC coefficients and literal0.70710678 weight. The source abs(amount)<0.005 branch produces a line. At the threshold, the original degree2/five-control rational profile is used. Signs, radius/2, radius/3 and amount are preserved; full knots expand the source short vector. The method name fixes the version convention explicitly.

Following the flat branch uncovered a parameter-domain shortcut: the acquired LineCurve constructor calls ON_LineCurve_New3, whose published wrapper calls native ON_LineCurve(point,point). Pinned openNURBS6 comparison sets domain0..line length with the documented zero-length tolerance/fallback. A regression failed the initial0..1 normalization and now passes. This published reference is not private Rhino5 runtime proof; the distinction remains in evidence.

The exact recovered CreateDomeRC body is extracted into a data observation harness with the verified5 setter and published line-domain policy. Twenty-one threshold/sign/radius cases pass693 native point comparisons, plus degree/control/domain/endpoint checks. Oracle body, threshold and weight tampering controls pass. Existing native graph/tensor suites stay passing. [Evidence](evidence/P05-T02-A21.json).

Next: recovered RailRevolve tensor construction, copied native curve definition export, average-plane/orientation/centering and inverse-transform metadata; then actual caps138/139 and legacy join140. No planar cap, mesh, geometry patch or modern-API substitution closes those gates. Full source/native topology/PC/physical parity remains open.
