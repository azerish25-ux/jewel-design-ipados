# ADR0034: source-specific array consumers

Status: local native/source verification; Apple source-specific validation pending. Full app acceptance remains open.

Complete the represented-data collection-consuming paths without one generic reduction rule. TransformObject builds a temporary input array, recursively flattens transformation data in source order, skips null/invalid/non-transformable inputs and prepends valid matrices. The object value is duplicated recursively while preserving array structure; null members become empty data. Each curve/profile leaf owns an actual transformed native copy and source metadata. Profile matrices prepend, rail/pure-transform matrices postmultiply; scalar/empty leaves remain unchanged. Failed traversal/native calls dispose partial outputs and preserve the earlier committed cache under existing candidate policy.

ScaleObjectToSize now aggregates native tight bounds across every represented curve/profile leaf, ignoring non-bound values. It fails when none has bounds; it does not use the first item or fabricate a box. Unpadded extrema/requested tolerance are used and maximum kernel margin is retained. Rhino exact bounds/tolerance/degenerate geometry remain comparison gates.

LegacyArrayData.FirstItem implements CSInputChannel.GetFirstData: repeatedly follow index0 through nested arrays, return missing for empty/null paths, and never search later items. Native curve resolution and the direct OutsideRail channel adapter use it; numeric/Boolean conversions still operate on root data and fail for arrays. This preserves numeric-radius fallback to first curve separately from aggregate object bounds.

Direct CSDataArray transform capability uses the first item only through GetTransformNoArray: nested first arrays fail even if they contain transformable data. Transform inputs instead flatten recursively before capability conversion. Source first-item and bounds/helper bodies are extracted with finite observation stubs; these are not original native geometry.

CSAdd provider now emits all three scalar/array result channels from the previously source-verified LegacyArrayArithmetic. Cartesian order, conversion carry, nested/empty behavior, negative/one-minus bits and null-item failure remain distinct. Nothing is silently converted to one number.

Native tests compare49 first-item trees to exact source, nested mixed profile/rail arrays to sequential per-leaf native transforms, null-copy/nesting/name/sweep/cumulative matrices, source bbox union over actual native leaf boxes and source first transform capability. They exercise nested rail and OutsideRail profile/radius array inputs, missing/empty/noncurve first entries with later valid curves, provider9 Cartesian results/all channels, empty-array scalar fallback/null failure, save/reopen, retained snapshots, managed/native failure cache/disposal and zero native entries. Existing original scene/native/source suites remain passing.

Parent A13 has all three known represented-data implementations locally; Apple verification, other source producers/types, source scene import/class/material/bitmap/disabled flags, profile segmentation, raw4x4/Unset/collapsed geometry, full scheduler/renderer/export/UI/PC-iPad-PC/physical-device acceptance remain required. No full parity row closes. No native pointer or output cache is serialized.
