# ADR0018: finite native curve/line events

Status: bounded Windows/iPad/iPhone assertions and unsigned device linking verified at5239e15; full Rhino/PC/physical parity remains open.

Recovered CSUtilRingRail intersects a rail with four finite100-unit axis rays, using the first event. CSOutsideRail1 also intersects a finite horizontal segment, requires two events in its locked branch, splits/rejoins curves, and finally intersects a positive-Z ray. Bounding boxes or plane projections cannot replace these queries.

CreateLine(start,end) builds an exact degree1/two-pole native NURBS with independent shape ownership. IntersectCurveLine(shape,start,end,tolerance) runs pinned OCCT8.0.1 IntTools_EdgeEdge on finite native edges. It returns immutable Point/Overlap events with both ranges and both endpoint pairs. Curve parameters remain the input edge's parameters; line parameters are normalized fractions. Ranges independently ascend, so reversed overlaps may have opposite endpoint correspondence. Sorting by curve start then line start and retaining native seam duplicates is candidate policy, not established Rhino event order/multiplicity.

The fixed-width C record is136 bytes: two uint32 fields, four doubles and four3-double points. Native static assertions and managed layout guards check the contract. A zero-capacity/null-buffer query returns count; short buffers return required count with InvalidArgument and write no events. Two-pass managed queries use existing context gate and SafeHandles. Native output is capped at65536 events with explicit UnsupportedOperation, never truncation. No native query result or temporary segment is stored as a persistent shape.

Native exceptions and invalid/nonfinite/degenerate input remain typed failures. Effective fuzzy tolerance is returned because SetFuzzyValue clamps to Precision::Confusion; BRep edge tolerances additionally participate. Quick sampled coincidence is explicitly disabled. This is not a claim that candidate tolerance equals Rhino's intersection/overlap tolerances.

Actual tests revealed two pinned-kernel representation/clipping issues:
1. A partially coincident two-pole linear NURBS was reduced to a Point by the general NURBS path. Exact non-rational two-pole degree1 inputs now use a temporary native analytic line and map parameters back. No curved input is approximated or replaced.
2. Native line/line clipping returned inconsistent source ranges for reversed directions and extended a common interval beyond a contained finite query segment. Solver lines are direction-aligned; after a native Overlap classification, exact line domains are intersected using native ElCLib coordinates, then mapped back to original ranges. This does not invent an intersection when the native query returned none.

At most16 floating-point epsilons of domain-scaled endpoint roundoff may clamp a parameter to its exact endpoint. Larger out-of-domain results fail. Original source curves are never mutated.

Analytical tests cover diameter points, four axis rays, finite short/skew segments, tangent/trim endpoints, true3D crossing, exact/reversed/scaled/contained/3D overlaps, original parameter preservation, requested/effective tolerance, invalid inputs, count/capacity and no partial writes, plus zero retained native entries. Source hashes include IntTools headers/implementation and ElCLib at the pinned commit.

Full OutsideRail remains incomplete: accurate bounds, closed seam handling, general imported-curve fixtures, recovered graph/caller failure policy and automatic join orientation must still be composed and compared. Arbitrary curved overlaps, continuity-break/near-coincident cases, original-PC ordering, physical devices, cancellation and process-memory acceptance stay open. No full application parity row is closed.

See [work card](plans/P05-T02-A14-curve-line-events.md) and [task evidence](evidence/P05-T02.json).

Bounded Apple execution/device proof for the ordered-join, unit-tangent and finite-intersection assertions is recorded together at checkpoint5239e15: [actual reports](../evidence/apple/native-curve-line-results.json). Later seam/bounds/outside/uniform-affine work is a separate checkpoint.
