# Original native camera analysis

This is static analysis of the hash-locked Rhino5 dependency candidate, never original application/DLL execution. Decompiled vendor C, projects, tool binaries and caches remain ignored under .local. [Tool provenance](../../../docs/evidence/tooling/ghidra-11.1.2.json) and [original method evidence](../../../docs/evidence/audit/native-camera-setter-ghidra.json) identify exact inputs and limitations.

Acquire/extract the official Ghidra11.1.2 release into .local/tools/ghidra/ghidra_11.1.2_PUBLIC after matching the published SHA256. The existing JDK17 is read-only. scripts/run-ghidra-local.cjs launches Java directly with D-only settings/cache/temp/home, streamed bounded log checking and rejection of zero-exit script errors. Do not change HOME, CODEX_HOME, global Java/MCP settings or use C: for generated files.

Use a fresh project for the first pass. Import the original Rhino.exe with -noanalysis and run InspectNativeCamera.java. Its six arguments are target SHA, setter RVA, setter byte length, output folder, repository root and setter-region SHA. The exact values are in the [tracked manifest](../fixtures/rhino5-camera-regions.json). This preserves a pre-annotation view and verifies301 instructions over the original1386-byte setter.

Then run RefineNativeCamera.java with the manifest path, a fresh output folder and repository root. Use -process Rhino.exe to reopen the existing project rather than reimporting it. The script converts only called PE import slots from labels to functions, applies MSVC signatures, supplies explicit24-byte point/vector and128-byte transform data shapes, preserves formal parameters/returns, validates the acos thunk and covers all chained commit regions. Native code bytes are never patched.

Example follow-up command from this D: repository:

    node scripts/run-local.cjs node scripts/run-ghidra-local.cjs .local/ghidra/projects Rhino5CameraFormal -process Rhino.exe -noanalysis -max-cpu 2 -scriptPath native/Analysis/Ghidra -postScript RefineNativeCamera.java native/Analysis/fixtures/rhino5-camera-regions.json .local/evidence/native-camera/new-review . -log .local/evidence/native-camera/new-review/headless.log -scriptlog .local/evidence/native-camera/new-review/scripts.log

Always use fresh output/log names for a new review and inspect analysis.json, actual script error logs and function hashes. Process success alone was insufficient in the original tool: a post-script exception returned0. The launcher now rejects logged errors, and an intentional runtime-argument failure verified Java0 becomes wrapper1. D: I/O failures require a five-second retry, not relocation, lock deletion or target execution.

InspectNativeImportModel.java provides read-only evidence for import-label and listing coverage failures. Intermediate views under set-locations-typed and set-locations-typed-02 have incomplete/wrong prototype handling and are rejected; use the final full-region view and recorded hashes. Static decompiler output remains subject to instruction/call review, original primitive comparisons and PC/physical runtime evidence. [Remaining port requirements](../../../docs/porting/NATIVE_CAMERA_CONTRACT.md).
