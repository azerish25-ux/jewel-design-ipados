# ADR0032: pure array values before native collection ownership

Status: local pure-data/source verification; Apple proof pending. Parent P03-T03-A13 remains in progress.

Add immutable LegacyChannelValue.Array child lists; null input becomes explicit Missing, preserving source null vs empty data. Primitive number/Boolean conversions fail for arrays and preserve ref defaults. FromArray copies the child list; leaves remain immutable producer-scoped DTO references. It is not a native snapshot, retain or geometry clone. Existing graph does not emit Array/Unarray data until the ownership adapter is ready.

LegacyArrayData.Flatten preserves depth-first source order, turns null members into empty CSData and removes empty nested arrays. Duplicate preserves nesting and turns null members into empty data. Ordinary immutable leaf values/reference DTOs are preserved; native handles and source material/bitmap/disabled flags are not modeled by this helper. Explicit depth256 and traversal/item65536 limits reject rather than truncate. Factory per-array child count is bounded and traversal counts every descendant including nested array nodes.

LegacyArrayArithmetic.Add ports the complete source scalar/array arithmetic branch for represented DTO types. Positive counts produce Cartesian order or scalar-vs-array outputs. Failed conversions retain ref values across items/rows; do not reset defaults inside loops. Nested arrays do not become scalars. Empty-array+empty-array follows scalar fallback0. Direct null array members throw, matching source dereference; null scalar inputs default0. Results include sum/0-sum/1-sum, preserving operation order and floating bits. The older AddScalars API rejects arrays explicitly so no unadapted known caller silently produces0.

Generalize the known socket replay for constructors with zero or one fixed input. Earlier source12285 TransformObject cases stay unchanged; another12285 source cases cover Array/Unarray initial constructors. Decode only20 exact common-header1.0 records (13 Array/7 Unarray), preserving raw arrays/future schemas.161 known headers/80 opaque/100 scalar handles does not mean collections execute.

Nine exact recovered copy/flatten/item/helper/CSAdd methods are extracted into a test-only observation harness.289 input pairs include33 expected null failures;789 structured/random flatten/copy trees compare kind/bit/order/reference fidelity. Independent explicit carry-order/null/empty/mutation/resource-bound cases pass. Source operation/order/null-copy tampering is rejected. Existing native suites still run and explicitly reject unimplemented native collection roots61/116 without allocating shapes.

Next A13-A02 must deepen runtime output ownership for multiple native curve leaves per output, item-addressed curve references, recursive retained snapshots and rollback/disposal, then implement CSArray wildcard/output counts and CSUnarray1 one-level expansion plus its source trailing empty array. A13-A03 must adapt array transforms/aggregate bounds/first-item conversions and arithmetic provider outputs. Do not publish unowned curve-reference lists as completed arrays or silently reduce them to one curve.

Full material/segmentation/array-derived data/4x4/all constructors/producers/versions/full scheduler/PC/UI/physical parity remains required; no full parity row closes.
