# P08-T02-A01-F02 validated native archive reuse for app retests

Problem: a C# harness import failure appeared only after a full native rebuild. Early preflight now reproduces it; future runtime/Metal iterations must reuse unchanged real native build output with explicit provenance.

Implementation: retain the combined simulator/device archives as separate named Actions artifacts before app compilation. Manifest records producer run/source commit, platform, OCCT pin, exact native-input fingerprint, archive SHA256 and Xcode identity. Existing workflows accept an optional producer run ID; download only this repository's named artifact and reject wrong run/platform/toolchain/fingerprint/digest before copying to the fixed matching native archive path. Actual .NET build/runtime and final device executable inspection still run; archive reuse never counts as a passed app test. No arbitrary artifact filenames or restore paths.

Files: scripts/native-archive.cjs; tests/native-archive.test.cjs; .github/workflows/native-geometry.yml and native-device.yml; repository regression step; evidenceP08-T02-A01-F02.

Tests first: RED missing module. Unit fixtures model artifact integrity only, never native geometry: accept matching archive; reject changed native input, altered/truncated archive, wrong producer/platform/Xcode/OCCT/schema/source commit and invalid run ID; assert fixed output path. Add workflow wiring controls for optional download and immediate pre-app upload. GREEN unit tests, full repository checks, and actual Apple producer/retest runs required independently.

Use existing pinned upload action and GitHub CLI on macOS, contents/actions read permissions only. Official docs reviewed: https://docs.github.com/en/actions/tutorials/store-and-share-data and https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-syntax. Local artifacts/tools/cache D:;5-second retry if D: transient; recovery read-only.
