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
