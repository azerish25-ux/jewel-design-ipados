> HISTORICAL / SUPERSEDED: these allowances are not a measured new-code requirement or minimum. The 500k–1M figure was withdrawn as a requirement. See [current state](../../porting/CURRENT_STATE.md); re-estimate after the P06 vertical slice.

# CounterSketch offline iPad parity estimate

The code audit supports a scoped planning budget, not a minimum LOC guarantee. Literal full parity remains blocked by the decisions below; no LOC quantity makes live supplier operations work without communication or automatically ports arbitrary Windows scripts/hardware.

## Counted baseline

2783 C# files; 484,565 physical lines; 441,299 nonblank lines. These still contain generated proxies, lookup data, metadata, helpers and duplicate source. Earlier reference-poor CAD output was larger because of decompiler warnings/representations. None of these numbers is a direct iPad requirement.

## Conditional production budget

| Work package | Application LOC allowance | Code-backed scope evidence |
|---|---:|---|
| EST-01 Jewelry builders, graph evaluation and legacy algorithms | 140–260k | 619 CSBlocks GUID declarations across 60 namespaces; 209,138 physical lines in builder/helper path group; 272 legacy/obsolete graph declarations overall |
| EST-02 Geometry kernel adapters and mesh repair | 50–100k | 315 static kernel/wrapper call families; 24,002 physical lines in geometry/material/utility path group |
| EST-03 PC document, portfolio and legacy round-trip compatibility | 60–130k | 58 grouped PC requirements; 654 GUID-specific serialization/version acceptance rows; Four explicit envelope UUIDs and multiple file/bundle types |
| EST-04 Native iPad UI and interactions | 60–120k | 68 UI requirement groups including gaps; 317 public commands; 113 XAML; 384 command attributes; 771 hookup records |
| EST-05 Viewport, materials, try-on and local rendering | 50–110k | V-Ray/native material/render pipeline; UI/CAD viewport and try-on settings |
| EST-06 Catalog, project administration, pricing, quotes and reports | 45–90k | 38,820 physical lines in desktop domain/data/platform path group; Catalog, quote/report and portfolio workflow requirements |
| EST-07 Supported scripting compatibility | 35–100k | Stored C# source, CodeDom compilation, reflection and typed ref outputs |
| EST-08 iPad lifecycle, packaging and local platform integration | 25–45k | Windows registry/COM/BITS/printer assumptions; Offline local data and lifecycle requirements |

Arithmetic total: 465,000–955,000 lines; communicate as approximately **500,000–1,000,000 project-owned production lines**. Add a separate **150,000–350,000** allowance for verification/conversion/build tools and tests. These are engineering allowances, not measured implementation effort, a vendor quote, a confidence interval or a guaranteed minimum.

Counting basis: Normally formatted project-owned nonblank/non-comment production source, including generated compatibility glue; excludes third-party CAD/render/interpreter SDK source, assets/catalog bytes, tests and tools.

Each package includes a risk statement and assumptions in parity-estimate.json. Do not sum source counts and new-code budgets. SDK implementation source is excluded only if an existing appropriate SDK is actually used. A kernel or renderer implemented from scratch is outside this budget. Unrestricted PC scripts are not covered by the bounded scripting allowance.

## Decisions required for a literal parity claim

- D1: Offline versus live quotation/pricing, supplier/order actions and export accounting. Offline snapshots or exports are behavior changes, not literal service parity.
- D2: Stored C# scripts. Preserve all source/configuration, then define an executable mobile API/runtime subset. Arbitrary Windows/native dependencies cannot simply run on iPad.
- D3: Windows printer/mesh/native integrations. Agree on actual supported mobile hardware/output equivalence and obtain a viable SDK or replacement.
- D4: Renderer fidelity. Comparable gemstones/metals and matching material behavior need objective reference images and device benchmarks; byte-identical V-Ray images are not established.
- D5: PC version scope. Four container markers and 654 known GUIDs do not guarantee every past/future CounterSketch project. Versioned fixture coverage and unknown-data policy are mandatory.
- D6: UI meaning. Equivalent interactions/layout/state versus identical desktop pixels/mouse behavior must be distinguished on different hardware.
- D7: Catalog/profile state and a functioning PC reference environment remain missing in this workspace; source defaults are not evidence of the complete account-specific catalog.

## Re-estimation gates

- Working original/fixture corpus across real portfolios and supported old formats
- Real iPad kernel vertical slice with independent output comparisons
- Edited PC -> iPad -> PC round trip covering mixed legacy, embedded geometry and scripts
- Renderer/thermal benchmark
- Measured implementation and test throughput on representative builder families

Until those gates pass, use the budget for feasibility planning, not a fixed full-parity commitment. Count measured porting/test effort on representative builder families and codecs before tightening the range.
