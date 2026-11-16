# CounterSketch full iPad parity audit

This is a source-backed feature/compatibility checklist for recovered CounterSketch 8.1.19323.1002. It covers the enumerated static surfaces and provides acceptance criteria for implementation; it is **not a completed behavioral specification or proof of runtime/PC round-trip parity**. All newly created artifacts and tool caches are inside this D: workspace. No target application, script, native plugin or vendor-account request was executed during this audit.

## Start here

- [UI and workflow checklist](<UI-CHECKLIST.md>) — 68 grouped requirements.
- [CAD capability checklist](<CAD-CHECKLIST.md>) — 52 capability groups plus a separate 619-builder catalog.
- [Editable PC project checklist](<PC-COMPATIBILITY-CHECKLIST.md>) — 58 grouped requirements.
- [All GUIDs and serializer matrix](<TYPE-COMPATIBILITY-MATRIX.md>) — 654 types.
- [Conditional bottom-up estimate](<ESTIMATE.md>) — scope, assumptions, blockers and budget.

## Viewport image dependencies

- [Wallpaper raster source and exact grayscale constants](rhino5-wallpaper-raster-source.json): portable arithmetic and all-color checksum; full decode/render remains open.
- [Bitmap lookup, decoder and storage ownership](rhino5-bitmap-loading-source.json): seven original bodies, ordinal codec imports, and failure/lifetime rules. [Next image implementation card](../../porting/plans/P08-T01-A05-03f-02b-A05-P03-A02-image-decode.md).

- [Original codec module inventory](rhino5-image-codec-module-inventory.json): installed filter/library hashes; current fixtures do not establish full format support.
- [Original image drawing and trace grayscale](rhino5-image-drawing-source.json): eight source bodies, world/screen paths and trace rounding/alpha differences; raw GL/GDI refinement remains required.

- [Refined image drawing ABIs](rhino5-image-drawing-abi-source.json):23 actual imported graphics prototypes,9 original bodies, UV/filter/blend/centering and verified0.1 raster offset; original cache/visible integration remains open.

- [Native Apple provider observations](../apple/native-image-provider-results.json): measured raw low-alpha/transparent RGB preservation on macOS; iOS adapter and wider conversion parity still required.

- [Original image texture cache/upload](rhino5-image-texture-cache-source.json):7 static bodies, context/kind/name matching, file/embedded fallback and resize/alpha repair obligations.
- [PNG sample native reference](../apple/png-straight-native-reference-results.json):22 controlled rasters match actual macOS provider components; wider source codec/color behavior remains unverified.

- [Original texture preparation](rhino5-image-preparation-source.json): corrected float ABI, device-size selection, cross-axis/budget order and alpha OR/stored-row semantics; no original runtime execution.

- [Original texture final upload](rhino5-image-upload-source.json): seven original helpers,664 static enum results, BGR/BGRA versus internal DXT1/DXT5 storage, allocation/subimage/mipmap and truthful native success limitations.

- [Original texture anisotropy](rhino5-texture-anisotropy-source.json): mode normalization, extension/no-query distinction, float divisors and explicit Metal representability; original-driver equality remains unverified.

## Most consequential findings

1. A PC portfolio is .csp XML plus GUID-pair .csg designs and sibling preview/history files. An STL/3DM geometry export is a different contract. [Project.cs:101](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L101>) — public string ProjectInfoFilePath.
2. Binary CSG uses custom UUID/table/graph records within an OpenNURBS-style archive. Four envelope UUIDs and node-specific record versions are explicit.
3. The matrix contains 654 unique declared GUIDs, including 272 legacy/obsolete types. Supporting only the newest builders loses old graph behavior.
4. CSScript stores C# source/configuration, compiles through CodeDom and invokes methods with typed outputs. Scripting compatibility is a substantial separate item.
5. CSRhinoObjectsFromBoundingBox has binary embedded-object data but no own JSON serializer overrides. A general JSON migration cannot be presumed lossless.
6. Quotes, quick estimates and export accounting have remote-service paths. Complete offline behavior needs explicit accepted substitutions, not a hidden claim of identical live services.
7. Source declarations are not proof of working features: 44 command declarations lack resolved initialization; six supplemental application bindings remain unresolved; PrintQuote, SaveSortedImportGroups and a four-view worker have empty bodies.

## Coverage

| Surface | Enumerated coverage | What it does not prove |
|---|---:|---|
| Baseline C# files parsed | 2783; zero syntax diagnostics | Semantic equivalence or working target runtime |
| Type declarations / members | 2,644 / 37,008 | Independent feature count |
| Public RelayCommands | 317 plus 1 private backing field | Every declared command is initialized/reachable |
| Recovered XAML files | 113 | Pixel/state behavior on a running Windows/iPad UI |
| Actual command attributes | 384, each mapped to feature or gap | Dynamic DataContext dispatch |
| Direct XAML events | 169; all exact AST handler matches | Runtime event ordering |
| Event/input/source hookup records | 771; mapped to requirements or explicit gaps | Every dynamic/third-party event path |
| Declared CSBlock GUIDs | 654; zero duplicates | Successful registration/construction at runtime |
| Declared builder GUIDs | 619 in 60 namespaces | 619 independent UI features |
| Static kernel/wrapper call families | 315 | Availability/equivalence of an iPad kernel |

CAD body-review examples cover 93 files and 75 builder declarations, including representatives in all 25 obsolete builder namespaces. The rest of the per-type inventory is syntactic/declaration-backed, not an exhaustive manual semantic review. The UI inventory retains 333 handler bodies and 504 including containing members; external/dynamic framework behavior remains unverified. Raw coverage details and unresolved cases are preserved rather than marked complete.

## Actual file probe

The public library-derived sample item-4.csg has archive header 4, the B237 envelope marker, file-information version 1.0, embedded build date 2012-07-23, two gem records, four category records and 241 graph records. Those records map to 43 distinct known GUIDs, with 33 legacy nodes and record versions 1.0/1.1/1.2. Four tail bytes remain outside the decoded graph-directory portion; CRC and node payload semantics were not verified. This is one library design, not an edited PC portfolio. No PC -> iPad -> PC fixture has passed; **43 GUIDs observed in a byte probe is not 43 functionally verified types**.

## Evidence and reproducibility

- source-files.json: SHA-256 of every parsed baseline file, line counts and syntax diagnostics.
- source-types.json/source-members.json: full syntax index, calls and serialization operations.
- serialization-matrix.json: effective inherited readers/writers/version methods for every declared GUID.
- ui-surfaces.json/ui-event-inventory.json: raw command, binding, event/input maps and unresolved cases.
- cad-type-inventory.json/cad-features.json: builders, legacy observations and kernel-call families.
- pc-project-checklist.json: scoreable PC acceptance requirements.
- project-envelope-probes.json/sample-csg-directory.json: bounded byte observations and parsed node directory.
- source-cost-metrics.json: disclosed path-based counting heuristic; not a mobile source-line forecast.
- parity-estimate.json: conditional work packages and assumptions.
- SourceAudit: read-only Roslyn source indexer; use analysis/scripts/run-local.cjs for local cache/temp settings.

All acceptance scores currently concern future iPad functionality. Existing Windows compiler/reflection checks and sample byte parsing are kept separate from those scores.

## Acceptance fixture plan

[Twenty-family fixture plan](<FIXTURE-PLAN.md>) defines the corpus, verification levels and edited PC/iPad/PC protocol.
