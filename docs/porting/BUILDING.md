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

The iOS workflow also watches tests/Port/** so contract-only changes run on macOS. Filter semantics: [official GitHub workflow syntax](https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-syntax#onpushpull_requestpull_request_targetpathspaths-ignore). Shared Domain.Contracts runs the recovered-source ring comparisons on Console and both simulator families.

## Native candidate device build

[Device build plan](plans/P05-T02-A02-native-device.md) extends the pinned analytical experiment to an unsigned Release ios-arm64 app. The native-device.yml workflow uses iphoneos CMake and a separate .local/native/ios-device-all.a, then verifies the app executable is arm64/platform IOS with Apple lipo/vtool. The initial native-only build passed in run 41023254221; the newer combined recovered/native build is pending. No physical-device execution or distribution permission is implied. Simulator and device archive selection is explicit in tests/iOS.Native/NativeGeometry.csproj.

    node scripts/run-local.cjs gh workflow run native-device.yml --ref main

All local downloads/logs remain under .local on D:. CI uploads text evidence only. See the official [NativeReference](https://learn.microsoft.com/en-us/dotnet/ios/building-apps/build-items#nativereference) and [EnableCodeSigning](https://learn.microsoft.com/en-us/dotnet/ios/building-apps/build-properties#enablecodesigning) contracts.

## D:-local Windows native iteration

An existing WinLibs GNU16.1.0/UCRT toolchain and Ninja were found without installing tools. The pinned OCCT candidate configures and its API objects compile locally; the first full native Windows build is pending at this checkpoint. The reproducible helper keeps source/build/temp/cache under the D: repository through run-local.cjs:

    node scripts/build-native-windows.cjs

It requires an existing g++/CMake/Ninja on PATH and the pinned .local/native/occt-source checkout. It installs nothing and reuses .local/native/windows-gnu for incremental builds. Do not launch another build/configure against that directory while its current Ninja process is live. This helper is Windows candidate validation, not Apple or original CounterSketch/Rhino execution. Mac CI remains required.
