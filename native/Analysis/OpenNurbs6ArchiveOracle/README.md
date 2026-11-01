# Full published native archive oracle

This analysis executable links the full published openNURBS library at commit f2dc7fba67017475b8d530835a40fcfb5372134a from [McNeel's source repository](https://github.com/mcneel/opennurbs/tree/f2dc7fba67017475b8d530835a40fcfb5372134a). It never loads the original Rhino binary. It observes actual ON_UserStringList read/write behavior, rather than replacing archive implementations with stubs.

The generator compares12 canonical byte outputs and1,076 native reader outcomes against the tracked corpus. Each read checks bool, CRC diagnostics, cursor and retained ordered pairs, including existing entries. The managed codec and real registry binding execute these checks separately on Windows, macOS and iOS. This is one body codec; it does not implement whole3dm archives or establish PC-project compatibility.

## Reproduce

Acquire the exact commit into .local/reference/opennurbs6-archive-source. The macOS workflow [archive-oracle.yml](../../../.github/workflows/archive-oracle.yml) contains the complete acquisition/build/verification commands. Use its explicit commit equality check; do not follow a moving branch.

Windows uses the installed MSVC2022 compiler through the D-contained task profile:

    node scripts/run-archive-oracle-local.cjs cmake -S native/Analysis/OpenNurbs6ArchiveOracle -B .local/native/on6-archive-msvc -G "Visual Studio 17 2022" -A x64 "-DON6_SOURCE=D:/MAIN SOFTWARE/jewerly software/jewel-design-ipados/.local/reference/opennurbs6-archive-source" "-DCMAKE_CXX_FLAGS_RELEASE=/Od /DNDEBUG" "-DCMAKE_C_FLAGS_RELEASE=/Od /DNDEBUG"
    node scripts/run-archive-oracle-local.cjs cmake --build .local/native/on6-archive-msvc --config Release --parallel 2
    node scripts/run-archive-oracle-local.cjs node scripts/generate-userstring-body-cases.cjs .local/native/on6-archive-msvc/Release/archive_oracle.exe
    node scripts/run-local.cjs dotnet run --project tests/Port/Rendering.Tests
    node scripts/run-local.cjs node --test tests/userstring-oracle.test.cjs

Only deliberate reference refreshes use --write. Routine checks must compare against the existing corpus and fail on disagreement.

## Build adaptations and limits

CMake draws sources from the pinned makefile plus missing platform sources listed in its official projects. C++17 avoids obsolete atomic copy-initialization failures. MSVC/O2 repeatedly crashed compiling the legacy mesh code on the host; /Od builds successfully. MinGW is explicitly unsupported here.

AppleClang rejects five invalid nullptr returns in boolean helpers: three in ON_FixedSizePool::ElementIdIsIncreasing, one in ON_SubDLimitMesh::GetFaceCenterPointAndNormal and one through ON_SUBD_RETURN_ERROR in ON_SubD_FixedSizeHeap::ReturnPtrArray. The Apple-only build adaptation verifies the LF-normalized SHA256 and the exact expected matching returns in each function, then compiles a build-directory copy with those returns changed to false (including the error macro argument). The reference checkout remains unchanged; archive and userdata algorithms remain unmodified. Failed runs and the replacement outcome belong in the [work evidence](../../../docs/porting/evidence/P08-T01-A05-03f-02b-A02-P02.json).

The oracle converts observed wchar output to UTF16 for platform comparison. Valid Unicode, supplementary characters, combining marks and embedded nulls are covered. Malformed surrogate normalization across wchar16/wchar32 platforms is not claimed. Native5 comparison remains static evidence; published6 execution cannot substitute for an original PC round trip.
