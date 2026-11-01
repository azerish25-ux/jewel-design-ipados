# P08-T01-A05-03d original native camera setter analysis

Goal: establish the exact original Rhino5 dependency camera-setter behavior before building the recovered preset command on published candidate math. This is static analysis, with no original DLL/application execution.

Inputs are hash-locked in docs/evidence/audit/native-camera-set-locations.json: RhinoCommon5.12 wrapper; rhcommon_c.dll export CRhinoViewport_SetCameraLocations; Rhino.exe export CRhinoViewport::SetTargetAndCameraLocation, RVA0x44c730, main unwind region0x44c730..0x44cc9a. The wrapper supplies native int mode1. The runtime candidate is not a confirmed original licensed PC reference.

- [x] Acquire official portable Ghidra11.1.2 (JDK17-compatible) entirely under D:, verifying official SHA256219ec130b901645779948feeb7cc86f131dd2da6c36284cf538c3a7f3d44b588. Use the existing JDK17 read-only, with Java/Ghidra home, temp, cache, project, logs and scripts under repo .local. No global install, MCP/client config changes or target execution.
- [x] Import the target into a fresh D: project, hash/check target metadata and decompile only the exact native method with bounded analysis. Keep machine code/decompiler output/assumed ABI annotations distinct.
- [ ] Resolve direct callees/imports and branches controlling mode1, camera/target/up/frustum changes and UI/document effects. Prove each semantic claim from exact bytes and symbols, with unresolved behavior explicitly retained.
- [ ] Compare the original dependency semantics with the published numerical candidates. Do not assume openNURBS6 equals private Rhino5.
- [ ] Record reproducible commands, engine/source hashes, actual outputs and remaining runtime/PC/physical gaps. Validate artifacts and repository integrity; commit/push the bounded evidence. Analysis work validates actual binary contents rather than inventing unit tests.

All local outputs/cache/bloat stay in D:/MAIN SOFTWARE/jewerly software/jewel-design-ipados. Every cache-producing Windows command uses run-local. On transient D: errors wait five seconds and retry; never relocate cache to C:. No delegation. Recovery baselines stay read-only. No full parity row closure from static analysis alone.

Observed progress: full setter and chained commit views completed with formal-signature guards; exact branch/threshold/call-order requirements are in NATIVE_CAMERA_CONTRACT.md. Parent-view/instance guard names, unresolved callback and original5-versus-public6 primitive comparisons remain open. Five launcher regressions plus34 adjacent regressions pass locally; macOS CI is configured for the launcher contracts.
