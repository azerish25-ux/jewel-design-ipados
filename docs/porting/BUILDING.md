# Building and verification

Run from repository root. CI uses Node 24 and .NET 10. On Windows all SDK/cache/auth/temp writes stay under .local on D:. Global Windows SDK fallback is disabled.

    node scripts/bootstrap-dotnet.cjs
    node tools/repository/check.cjs
    node scripts/run-local.cjs dotnet run --project tests/Console
    node scripts/run-local.cjs node reference/file-proof/run-file-proof.cjs
    node scripts/run-local.cjs node reference/file-proof/run-independent-check.cjs

The independent proof writes cases, snapshots, verifier output and logs under .local/file-proof; it loads no CounterSketch/Rhino assembly and regenerates no geometry. Repository checks validate hashes, secrets, links and ledgers, not CAD correctness.

## Apple

Recorded compatible set: SDK 10.0.302, workload 10.0.302.0, iOS SDK 26.5.10301, Xcode 26.6, macOS 26 ARM64. Windows audits use 10.0.403 from global.json. The workflow selects the compatible Apple SDK explicitly.

Relevant changes run the signed iPad smoke. Manual dispatch additionally runs unsigned ios-arm64 Release/Mono AOT with framework-only trimming:

    node scripts/run-local.cjs gh workflow run ios-reuse.yml --ref main

On a suitable Mac select the compatible SDK first (CI rewrites global.json in its ephemeral checkout), then:

    dotnet workload install ios --version 10.0.302.0
    dotnet build tests/iOS -c Debug -r iossimulator-arm64 -p:EnableCodeSigning=true -p:CodesignKey=- -p:CodesignRequireProvisioningProfile=false -p:MtouchDebug=false -p:EnableDiagnostics=false
    mkdir -p artifacts
    bash scripts/run-simulator.sh
    dotnet build tests/iOS -c Release -r ios-arm64 -p:EnableCodeSigning=false -p:BuildIpa=false -p:MtouchLink=SdkOnly

Invalid ARM64 signatures previously killed startup before managed Main. Simulator build/execution and physical-device evidence remain separate.

Do not build every archived csproj or install CounterSketch/Rhino on the host. Windows projects need external native libraries, reconstructed resources and recovery fixes. P02 owns contained reproduction; materialization only reconstructs files.
