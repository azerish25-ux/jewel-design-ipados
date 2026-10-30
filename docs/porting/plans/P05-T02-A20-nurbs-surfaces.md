# P05-T02-A20 rational tensor NURBS surfaces

Goal: native construction of recovered rational tensor surfaces, prerequisite for actual profile caps138/139 and legacy join140. Keep full1:1 parent acceptance; do not substitute planar caps for source domes.

Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Source CSProfileCap, CSUtilGeometry.CreateDomeRC/CreateDome and CSUtilRailRevolve.RailRevolve. Decoded headers:138 [115,136]/[0,0],139 [122,136]/[0,1],140 [134,138,139]/[0,0,0]. All exact1.0.

Critical version distinction: official McNeel explanation https://discourse.mcneel.com/t/nurbscurve-different-behaviour-between-rhino-net-and-rhinocommon/75376/2 says Rhino5 Point4d setters use Euclidean XYZ plus weight; Rhino6 changes to homogeneous XYZ/W. Do not infer old CreateDomeRC behavior from modern documentation. Surface factory accepts explicitly Euclidean poles plus weights, independent of this conversion decision.

- [x] RED missing immutable surface definition/native creation interface.
- [x] Bounded counted U-major tensor API: independent degrees/counts/full knots, optional rational weights, finite dimensions and native typed failures.
- [x] Actual native face, correct U/V orientation/domains, rational weights, clamped/nonclamped knots and degenerate-apex handling; no synthetic mesh.
- [x] Independent bilinear and rational/nonuniform tensor evaluation, nonclamped active domains, input isolation, error/zero-entry ownership checks.
- [ ] Recovered rail-revolve/dome math and version-specific conversion are next steps; average-plane/native curve export and cap/join graph integration remain required.
- [ ] Review/source/provenance/repository; commit/push; macOS iPad/iPhone and unsigned arm64 artifacts.

All local output/cache D:; retry transient D: errors after5 seconds. Kernel/recovery read-only. No full source cap or topology/PC/physical parity claims from surface import alone.

Local native/import checks pass1445 independent points including a collapsed apex. Source average-plane/cap/join behavior remains unfinished. [Decision](../ADR-0039-rational-tensor-surfaces.md), [evidence](../evidence/P05-T02-A20.json).
