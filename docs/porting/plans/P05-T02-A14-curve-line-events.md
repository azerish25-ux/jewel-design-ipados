# P05-T02-A14 finite native curve/line intersection events

Goal: implement real finite3D intersection point/overlap events needed by recovered CSOutsideRail1 and CSUtilRingRail, without projecting into a plane or substituting bounding-box extents.

Sources: recovered CSOutsideRail1.cs and CSUtilRingRail.cs; pinned OCCT8.0.1 IntTools_EdgeEdge.hxx/.lxx/.cxx and CommonPrt.hxx. IntTools restores edge1/edge2 parameters after internal swaps; IsDone reports error status (empty success is valid); fuzzy value clamps to Precision::Confusion. Record requested/effective fuzzy tolerance separately. BRep edge tolerances additionally participate and are not Rhino equivalence.

Interfaces: CreateLine(start,end) creates an owned exact linear NURBS. IntersectCurveLine(shape,start,end,tolerance) returns immutable events with native curve ranges, normalized finite line ranges, and both endpoint pairs. Kinds Point/Overlap retained. Ranges independently ascending, so reversed overlaps do not imply paired endpoint correspondence. Events explicitly sort by curve start then line start; seam duplicates retained. Ordering/multiplicity and fuzzy behavior are candidate policy pending original-PC comparison.

C ABI: fixed-width136-byte event (two uint32 fields plus16 doubles), count/capacity two-pass query, required count on short buffer and no partial event writes. Native shape immutable under context lock; managed two-pass stays under same gate/SafeHandle. Reject bad/null/count buffers, nonfinite/degenerate lines, invalid tolerance/solid/context/shape; bounded65536 output event limit. Native exceptions never cross boundary.

- [x] RED missing managed CreateLine/IntersectCurveLine APIs.
- [x] Implement native point+overlap extraction and exact line creation, CSharp marshaling/immutable results and public ADR0018.
- [x] Run native and managed analytical tests: two circle diameter points and fractions, four finite axis rays, finite short/disjoint/skew lines, tangent and trim endpoints, reversed line, exact/reversed overlaps and true3D crossing, invalid inputs/buffers and zero retained shape entries.
- [x] Run Windows native/CSharp suite and repository/provenance gates, record inputs and logs; review layout/bounds/error handling.
- [ ] Dispatch actual macOS iPad/iPhone and unsigned device builds; keep earlier jobs. Do not mark Apple, Rhino, full OutsideRail/PC/physical/cancellation/process-memory parity verified.

Owned: candidate_abi.h/.cpp, candidate_curve_contracts.cpp, probe.cpp, NativeMethods.cs, new managed intersection files, new shared contracts and Assert runner, plan/ADR/evidence. Accurate bounds, seam relocation, general imported geometry, full recovered caller lifecycle and full portfolio/UI are still required.

Observed native overlap representation/direction/contained-range defects were corrected without weakening endpoint assertions; see ADR0018. Apple runner now requires both native/managed event markers. Local host proof passed; Apple remains pending.
