# P05-T02-A21 recovered Rhino5 dome and rail-revolve math

Goal: preserve recovered cap control-grid math before actual caps138/139 and join140. A20 supplies native tensor face import; average-plane/placement and native curve export remain separate required work.

Source CSUtilGeometry.CreateDomeRC and CSUtilRailRevolve.RailRevolve(NurbsCurve,...). SDK5 static SetPoint->ON_NurbsCurve_SetCV2 and pinned published wrapper confirm Euclidean XYZ plus weight; see docs/evidence/audit/rhino5-control-point-setter.json. Do not silently use Rhino6 homogeneous setter convention.

- [x] RED missing explicit Rhino5 dome definition.
- [x] Preserve threshold abs(dome)<.005, five controls/weight.70710678/full-knot expansion; immutable data; no smoothing/weight correction.
- [x] Compare exact recovered CreateDomeRC body with finite observation types honoring verified Rhino5 setter semantics; boundary/sign/radius/native samples.
- [ ] Adapt recovered rail-revolve tensor math with exact direction/zAdjust/weight product; native tensor verification.
- [ ] Average plane, centering/inverse metadata and actual cap/join graph integration still required; no planar substitute.
- [ ] Review/source/repository/native checks, commit/push and new Apple tests; full PC/physical/native topology parity remains open.

All local output/cache D:; retry transient D errors after5 seconds; recovery/kernel read-only.

Local recovered dome profile checks pass21 cases/693 points. Flat-line parameter domain preserves published length/fallback rule; do not normalize it silently. Rail-revolve/plane/caps/join remain required. [Decision](../ADR-0040-recovered-rhino5-dome.md), [evidence](../evidence/P05-T02-A21.json).
