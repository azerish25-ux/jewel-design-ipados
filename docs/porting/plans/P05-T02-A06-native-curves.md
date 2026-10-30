# P05-T02-A06 candidate curve/ring-rail experiment

Goal: add real native NURBS curve creation/inspection/evaluation to the ownership adapter, moving beyond solid-box infrastructure toward the library RingRail_V1 source. This remains a candidate experiment; P05 backend selection/full semantic and P06 PC gates remain incomplete.

Read [ring-rail contracts](../kernel/ring-rail-caller-contracts.json) and actual pinned OCCT headers before coding. Important: the default circle converter uses3 spans; source-correlated quarter-circle NURBS use4. Force/test an explicit four-span rational representation, and do not confuse NURBS parameter with radians or normalized arc length. Modern OpenNURBS source is corroboration, not a substitute for original Rhino5 output.

Proposed boundary: generic circle in a caller-supplied plane (origin,normal,Xdirection), native BRep edge containing a rational NURBS; inspect domain/length/degree/rational/pole count; evaluate actual curve point at a native parameter. Use existing context/shape IDs, SafeHandle ownership and typed errors. A curve query on a solid must reject rather than invent a point. No managed synthetic curve is accepted as native output.

- [ ] Write managed contracts first and observe missing curve API. Cover ZX frame origin0/localX=worldZ/normalY, another frame, actual length/domain/degree/rational/poles, four-knot seams and rational Bezier quarter-span samples.
- [ ] Implement native curve creation/query in candidate_abi.h/.cpp using pinned SDK. Keep all existing ownership/error/boolean tests. Validate finite data, reject invalid frames/radii, catch every native exception and honor buffer/output contracts.
- [ ] Add managed point/vector/plane/curve-info DTOs and CandidateContext methods through existing gated factory/SafeHandles. Keep fixed-width flattened C ABI parameters; do not introduce ambiguous struct packing or expose pointers.
- [ ] Add source-derived default-radius/ZX rail prototype only after generic native curve tests pass. Do not claim original Update/CAD graph/UI/export/callback parity.
- [ ] Run actual host/iPad/iPhone native plus managed assertions and unsigned device build in pinned macOS CI. Inspect reports/errors/trim warnings. Preserve SourceSDK/PC/control-point/domain/closed-periodic/distribution/cancellation/leak gates.

Owned: candidate native header/implementation/contract tests/probe; Geometry.Candidate DTOs/methods/native imports; Geometry.Candidate.Contracts; P05-T02 evidence. Consider an explicit feature-minor/capability handshake if added symbols are consumed across mismatched binaries; static test app builds source and native module together. No final stable production ABI is claimed.

Managed RED observed at missing CandidatePoint3 before implementation. Managed GREEN compiles with0 warnings/errors. Native creation uses explicit Convert_TgtThetaOver2_4; pinned converter source makes this path nonperiodic (default path is periodic). Native and CSharp contracts test9-pole rational degree2 representation,28 rational parameter samples and actual frame/transform/error/ownership behavior. Mac/native execution remains pending.

Observed host RED at native runtime: Unexpected four-span circle representation in run41041204408/job124014178580. Pinned GeomConvert.cxx lines~386-406 force SetPeriodic for untrimmed circles after conversion; the trimmed branch at~208 uses the explicit interval converter without that step. Fix supplies Geom_TrimmedCurve over0..2pi with explicit_4, preserving strict assertions. Do not weaken pole-count/periodic/parameter checks or treat this source-backed fix as verified until rerun.
