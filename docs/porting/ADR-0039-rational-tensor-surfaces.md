# ADR0039: native rational tensor surface import

Status: local native/managed checks pass; Apple surface-import verification pending. This is a prerequisite for recovered caps, not a replacement cap algorithm.

CandidateNurbsSurfaceDefinition copies Euclidean XYZ controls, optional weights, full U/V knot vectors and independent degrees/counts. Layout is U-major: control index=u*CountV+v. The counted ABI validates total controls<=65536, array lengths, finite coordinates/knots, positive finite weights, active domains, supported degree/multiplicity and positive face tolerance before storing a genuine native BSplineSurface face.

The definition carries explicit Euclidean coordinates plus weights. There is no implicit Point4d convention. Native active domains preserve clamped and nonclamped vectors rather than taking the first/last stored knot. Surface queries, affine copies and native ownership use the existing BRep interface.

Independent homogeneous tensor de Boor evaluation passes1445 points across bilinear, rational quarter-cylinder, weights in both directions, nonuniform and nonclamped cases, plus a collapsed-apex cone patch. Tests mutate the original point/knot/weight arrays after definition creation, verify native face validity/affine copies, reject malformed data and confirm zero live entries. No triangulated substitute is used. [Evidence](evidence/P05-T02-A20.json).

Actual next records are138 ProfileCap [115,136]/[0,0],139 ProfileCap [122,136]/[0,1] and140 JoinBrep_V1 [134,138,139]/[0,0,0], all exact1.0 common headers. These are generated domes/rail-revolve caps, not planar-hole caps.

The recovered CreateDomeRC uses Point4d setters. [McNeel's version explanation](https://discourse.mcneel.com/t/nurbscurve-different-behaviour-between-rhino-net-and-rhinocommon/75376/2) states Rhino5 takes Euclidean XYZ plus weight while Rhino6 treats XYZ as homogeneous. The locally acquired RhinoCommon prerequisite reports assembly5.1.30000.16 and SDK package5.12.50810.13095. This is read-only metadata evidence, not proof of every installer runtime binding or a cap execution oracle. Modern setter semantics must not silently change recovered dome shape.

Next: expose copied native curve definitions as needed; preserve exact recovered CreateDome/RailRevolve tensor math and version-specific conversion; implement/source-check average plane orientation and cap placement; then caps138/139 and legacy join140 with actual boundary/solid/orientation tests. Periodic/discontinuous surface formats, wider native/PC/physical/UI acceptance remain open. Parity stays0/1451.
