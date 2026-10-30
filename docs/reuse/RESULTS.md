# Reuse evidence

Main whole-file BCL-only compilation: desktop 753 files / 50,586 nonblank lines; CAD 638 / 30,353. Combined 80,939 / 441,299 main-source lines = 18.34%. This is a compilation candidate count, not feature parity or a proven iOS portability percentage. AssemblyInfo files excluded.

Separate GemvisionGeometry support assembly: 25 implementation files / 3,100 nonblank lines, all compile unchanged. Do not add these lines to the main numerator without adding to its denominator.

Two StullerFileService copies containing an embedded vendor credential are excluded from export and CI. Exported total includes 1,414 recovered source files. No embedded credential was used. Source hashes verify unchanged content; native binaries and installer configurations stay local.

## Limits

Twelve retained files contain Windows native imports (runtime-risks.json). These declarations compile but cannot be used on iPad without replacements. Resources.cs can compile without resource containers; resource access remains untested. Generated service data/callbacks are not offline service implementations. One lookup source alone contributes 23,714 nonblank lines. Legacy CAD head-style definitions add thousands. Shared duplicates and annotations also inflate counts.

Whole-file elimination is conservative: one missing dependency or unused namespace can remove otherwise reusable methods. Rejected files are not proof of mandatory rewrites. No Rhino/WPF reference binaries or fake API stubs were used to achieve the compilation result.

## Behavioral evidence

Sixteen Bernstein cases compare against output from the original Windows GemvisionGeometry binary, tolerance 1e-14. Additional smoke checks exercise vector length/cross product and diamond specific gravity. CI tests an iPad simulator app and attempts unsigned Release ios-arm64 AOT compilation. A successful build does not mean every retained method is exercised; unused code may be trimmed.

A separate non-gating compression probe records incompatibilities explicitly: on Windows .NET 10, recovered zlib output for 100,000 repeated x bytes fails BCL decoding with InvalidDataException; empty input yields zero bytes rather than a complete zlib stream; a short string passes both directions. This has not been compared to the original desktop binary and might be an original bug or recovery defect. Compressed project compatibility is not proven.

## Required next work

Extract pure domain logic from Rhino/WPF dependencies; replace Windows calls with real adapters. Supply a genuine on-device CAD kernel and viewport. Replace CodeDom runtime compilation with AOT-compatible execution, preserving serialized block identities and reflection factories. Restore offline assets/catalogs and resources. Verify PC -> iPad edit/save -> PC reopen/regenerate across real project fixtures, legacy blocks, unknown fields and user data. Complete touch UI and full feature acceptance. Physical iPad signing/performance/memory/rendering tests are still required.

## Reproduction

Use scripts/run-local.cjs for local dotnet/gh commands to route configuration, caches, SDK and temp files into ignored .local on D:. ReuseAudit takes original source directory, local output directory and .NET reference assembly directory. It removes complete files with compiler errors until a closed error-free assembly emits. Full diagnostics remain local because they can contain original literals. Manifests store paths, counts and SHA256 of UTF-8 source decoded without BOM.

## Apple build evidence (2026-10-29)

[Device job](https://github.com/azerish25-ux/jewel-design-ipados/actions/runs/40953692111/job/123728156674): unsigned Release ios-arm64 Mono AOT build succeeded, 47 warnings, zero errors, elapsed 9m17s. Toolchain: .NET SDK 10.0.302, workload set 10.0.302.0, iOS SDK 26.5.10301, Xcode 26.6, macOS ARM64. Framework trimming uses SdkOnly; the recovered application libraries are retained. Windows uses SDK 10.0.403 for the static audit. This is compilation, not physical-device execution or signing.

Exported compilation set: 1,414 unchanged source files / 83,929 nonblank lines (80,829 main-source lines plus 3,100 geometry-helper lines). Two credential-containing copies totaling 110 lines are intentionally omitted. The initial local BCL count includes those copies; do not confuse it with the exported count.

Warnings include COM interop, reflection metadata, XML serialization/DataSet/DataTable trimming and resource-related risks. Build success does not validate those paths or make Windows native calls available. Test assemblies use experimental names desktop/cad/geometry; original assembly identity and project deserialization compatibility have not been reconstructed.

Adding portable Newtonsoft.Json 13.0.4 locally increases the main whole-file compilation set only to 1,397 files / 81,125 nonblank lines (+6 files / +186 lines). This extra profile has not been tested on iOS or uploaded as source. Exact-content deduplication of the BCL main compilation set yields 1,334 unique files / 77,625 nonblank lines.

Signed standalone iPad simulator run 40969820121 now passes the selected math/vector/density assertions. The native pre-Main failure was CODESIGNING/Invalid Page; valid ad-hoc signing fixed it. This establishes only the executed paths, not every retained method or full feature parity. The macOS golden/math/domain checks pass, with compression incompatibility reported as a non-gating probe.

All 2,783 original main recovered-source hashes were rechecked: zero changed. Local SDK, packages, GitHub configuration, temporary files and downloaded CI evidence reside under D:. No global SDK or workload installation was used.
