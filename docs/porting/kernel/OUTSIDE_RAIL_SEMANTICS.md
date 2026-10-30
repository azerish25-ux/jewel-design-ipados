# Legacy outside-rail source contract

[Source-hashed body audit](outside-rail-caller-contracts.json) records the next library dependency. OutsideRail_V1 is a sampled-distance/deformation pipeline, not a generic offset. It uses50 normalized arc-length samples, axis-intersection extents, and different ordered transforms for locked/unlocked branches. Locked mode also splits/joins curves and controls seam/tangent direction.

The helper tolerance is cached after its first callback; direct locked intersections use current application tolerance. Intersection ordering and first-hit access matter. oOutsideScale emits zero in the recovered body. None of these observations establishes replacement-kernel or original-PC parity.

Native prerequisites include arc-length parameter mapping, nonuniform transforms, closest points, curve intersections, seam/reverse/trim/join and bounds. Existing candidate circle/solid tests do not implement this node. Preserve the locked/unlocked and failure cases during the full port.

A17 now adapts both geometry branches through real native operations with analytical and stored-input tests; see [ADR0021](../ADR-0021-outside-rail-geometry-core.md). It does not complete chooser/channel/CSBlock/output-data/PC lifecycle, original native join0/batch/failure equivalence or full feature parity. The source-hashed audit remains the target contract.
