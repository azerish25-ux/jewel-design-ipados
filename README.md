# CounterSketch recovered C# reuse experiment

This is a measured portability experiment, not a working CounterSketch iPad port.

- Main-code BCL-only whole-file compilation: desktop 753 files / 50,586 nonblank lines; CAD 638 / 30,353.
- Separate GemvisionGeometry: all 25 implementation files / 3,100 nonblank lines compile unchanged.
- Main denominator: 441,299 nonblank lines including AssemblyInfo. 80,939 retained lines = 18.34%; generated DTOs/events and large lookup tables are included, so this is not a feature percentage.
- Source copies are unchanged. AssemblyInfo is excluded in each component to avoid original framework/assembly identity attributes. No Rhino/WPF DLLs or placeholder APIs are used.
- Compilation rejects cascade through missing types, so rejected files are not proof of mandatory rewrites. Adapters and extraction may recover more logic.
- CI builds an iPad simulator application, runs 16 golden math cases recorded from the original Windows DLL, and attempts an unsigned Release device AOT build. CI results, not this description, determine what passed.
- This does not test CAD regeneration, project compatibility, WPF UI, offline catalogs, rendering, or physical iPad performance.

## Local Windows commands

Use scripts/run-local.cjs for every build, gh command, or download. It directs SDK, NuGet, CLI configuration and temporary files into ignored .local on D:. Existing SDK must be extracted into .local/dotnet. Never commit .local.

Run: node scripts/run-local.cjs dotnet run --project tests/Console

Audit: tools/ReuseAudit takes original source root, output directory, and .NET reference-assembly directory. It removes whole files with compiler errors until a closed error-free set remains. Full diagnostics stay local because they may include original source literals.

Source counts and per-file content hashes: docs/reuse. CI verifies hashes before compilation. Selected sources were scanned for embedded credential literals; installer configurations and binaries are not uploaded.

Two compile-pass file copies containing an embedded vendor developer credential are intentionally excluded from export and CI. Local compilation counts above include them; exported counts are lower. No credential was used.
