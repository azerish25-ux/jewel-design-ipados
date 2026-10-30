# P05-T02-A02 native device build implementation plan

Goal: test the existing pinned native analytical candidate as an unsigned Release ios-arm64 app, beyond simulator-only linking. Execute directly in this authorized repository; no delegation. This extends the existing P05 candidate design, not backend selection.

Architecture: retain the same C++ probe and .NET/UIKit entry. Select a device-specific archive only for ios-arm64; preserve the existing simulator archive for iossimulator-arm64. A separate macOS workflow builds pinned OCCT with iphoneos rather than iphonesimulator, merges native archives, then builds .NET Release/Mono AOT without signing or IPA distribution. Inspect final app Mach-O architecture and LC_BUILD_VERSION via Apple tools to reject accidental simulator linkage.

Files: tests/iOS.Native/NativeGeometry.csproj; .github/workflows/native-device.yml; this plan; P05-T02 evidence and BUILDING.

Prerequisites: Pinned candidate sources and toolchain already configured; no physical-device account or signing material used. Missing evidence before this card: native OCCT device archive and combined device app have never been built in CI. Existing source reference points to simulator archive for every RID.

- [ ] Select correct native archive by explicit RuntimeIdentifier; reject unsupported RID before build.
- [ ] Add pinned macOS-26/Xcode26.6/SDK10.0.302/workload10.0.302.0 workflow with CMake iOS/iphoneos/arm64/min15.0, early API compile, actual library build and archive merge. Fail on compiler/link errors.
- [ ] Build tests/iOS.Native Release ios-arm64, signing false, BuildIpa false, MtouchLink SdkOnly. Assert output file, arm64 and platform IOS using vtool/lipo. No app/SDK binary upload, only text and hashes.
- [ ] Run local repository checks and inspect diff; commit/push. Dispatch/run macOS workflow and inspect terminal build logs, warnings and artifact hashes. If platform fails, diagnose actual failure before changing flags.
- [ ] Record result as build-only. Do not claim physical execution, full NativeAOT, full geometry, memory/cancellation, license/distribution or PC project parity. Keep parent phase incomplete.

Official build contract: [NativeReference](https://learn.microsoft.com/en-us/dotnet/ios/building-apps/build-items#nativereference) links native references into the executable; [EnableCodeSigning](https://learn.microsoft.com/en-us/dotnet/ios/building-apps/build-properties#enablecodesigning) may be disabled for this build check. Native build flags follow the pinned OCCT iOS script and existing passing simulator CMake.
