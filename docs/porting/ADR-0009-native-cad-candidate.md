# ADR 0009: pinned native CAD candidate probe

Status: experiment only; no backend selection.

OCCT V8.0.1 at b8f597c677811d1f9f4d8a97f5ae2825c0353a42 is an independent public source candidate. Native code uses genuine BRep booleans, analytic mass properties, rational BSpline conversion/evaluation and triangulation. Analytical fixtures do not constitute CounterSketch project regeneration. OCCT source/binaries are not counted as recovered or authored port LOC. Local source and build files remain under .local/native.

The candidate is documented against the pinned official [iOS build script](https://github.com/Open-Cascade-SAS/OCCT/blob/b8f597c677811d1f9f4d8a97f5ae2825c0353a42/adm/scripts/ios_build.sh), [CMake configuration](https://github.com/Open-Cascade-SAS/OCCT/blob/b8f597c677811d1f9f4d8a97f5ae2825c0353a42/CMakeLists.txt) and API headers. Only TKPrim/TKBO/TKMesh plus their declared modeling dependencies are built, with visualization/data-exchange/application/draw modules and optional third parties disabled. Source SHA is checked during configure. Source remains separately pinned; no third-party files are added to recovery.

The native function is a test-only candidate entry, not the planned production operation ABI. It executes fixtures, validates real outputs and returns a bounded report or explicit nonzero native/managed failure. Host and iOS share the same C++ assertions. iOS uses static native linking through a separate .NET/UIKit test app and records actual iPad/iPhone runtime family. No success-returning fake operation, synthetic mesh or cached image is used. Test binaries/SDK archives are not uploaded, only bounded text evidence.

Recovered BooleanDifference uses .01 tolerance, an intersection flip and duplicate-input fallback for null/empty results. The probe fuzzy tolerance does not establish equivalence with Rhino tolerance or that caller policy. All 315 static call families plus uncounted instance/native behavior still require semantic mapping and original-PC fixtures. This experiment cannot close that gate.

License inputs are the upstream [LGPL 2.1 text](https://github.com/Open-Cascade-SAS/OCCT/blob/b8f597c677811d1f9f4d8a97f5ae2825c0353a42/LICENSE_LGPL_21.txt) and [OCCT exception](https://github.com/Open-Cascade-SAS/OCCT/blob/b8f597c677811d1f9f4d8a97f5ae2825c0353a42/OCCT_LGPL_EXCEPTION.txt). Recording/building an internal candidate is not a distribution decision; shipping/static linking/relink and platform distribution prerequisites remain to be reviewed in P05-T03. No approval or compliance is inferred.

The initial independent P05-T02-A01 child requires the source-backed subset and compatible CI toolchain. Full P05 retains P02/P03, all operation mapping, sweeps/trims/archive fixtures, a selected backend, real ownership/cancellation/failure/leak proof, physical devices and PC comparisons.
