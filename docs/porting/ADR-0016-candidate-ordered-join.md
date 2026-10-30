# ADR0016: candidate ordered NURBS pair join

Status: bounded Windows/iPad/iPhone assertions and unsigned device linking verified at5239e15; full Rhino/PC/physical parity remains open.

Two located curve geometries are deep-copied/restricted to their edge intervals. The first end must lie within explicit finite positive tolerance of the second start. Native GeomConvert concatenation appends the second curve using its uniform-ratio policy and validates the owned output edge. Failed continuity/algorithm requests return typed errors rather than an unchanged input.

The candidate operation intentionally does not reproduce Rhino automatic orientation/reordering, multiple-input/multiple-output handling or exact joined parameterization. Those original caller policies remain open. Native/CSharp tests verify actual half-circle length/endpoints, source preservation, output lifetime after input disposal, rejected order/tolerance and zero remaining entries. This is one OutsideRail_V1 prerequisite; seam/intersection and whole algorithm/PC/physical/cancellation/process-memory scope remain incomplete.

Bounded Apple execution/device proof for the ordered-join, unit-tangent and finite-intersection assertions is recorded together at checkpoint5239e15: [actual reports](../evidence/apple/native-curve-line-results.json). Later seam/bounds/outside/uniform-affine work is a separate checkpoint.
