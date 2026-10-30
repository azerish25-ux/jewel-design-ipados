# P05-T02-A11 copied NURBS trim/reverse

Goal: real independent native NURBS interval trimming and parameter-direction reversal needed by OutsideRail_V1. Candidate operations only; original Rhino extrapolation/seam/degenerate/ordering parity remains open.

Inspect pinned Geom_BSplineCurve::Segment/Reverse and located BRep_Tool::Curve. Copy located geometry, segment within current edge domain or reverse while retaining that domain, then create/validate a real native edge. Reject nonfinite/out-of-domain/decreasing/zero intervals and unsupported solids. No topology-only reversed flag standing in for reversed curve geometry.

- [ ] RED managed TrimCurve/ReverseCurve methods absent before implementation.
- [ ] Fixed-width C APIs and existing context/SafeHandle factory, deep native copy and typed failure guards.
- [ ] Actual tests quarter-circle length/degree/domain/endpoints, reverse parameter relationship, source preservation and ownership. Open-arc closest queries beyond either end must choose real endpoint; this extends the prior projection tests. Reject unsupported solids/intervals.
- [ ] Run cached D:-local C ABI/CSharp tests then native/Apple simulator/device verification. Keep full OutsideRail seam/join/intersection and PC/physical/cancellation/leak requirements incomplete.
