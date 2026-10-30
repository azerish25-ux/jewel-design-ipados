# P05-T02-A19-A02 native two-rail surface implementation plan

For agentic workers: execute sequentially using the executing-plans skill and the repository AGENTS contract. No delegation is authorized. This is a proposed implementation plan, not working sweep code.

Goal: construct genuine editable native swept surface/BRep output from PreparedSweep2Candidate, preserving both explicit rail-parameter lists and source flags, then integrate original node134.

Architecture: retain A01 as the independently tested ownership/preparation boundary. Add a native typed sweep operation only after a constrained analytical experiment demonstrates its parameter/contact behavior. Unsupported modes fail explicitly. Renderer meshes, loft-only approximations and cached previews cannot stand in for a sweep.

Tech stack: recovered CSharp caller semantics, .NET10 managed SafeHandles, C++17 C ABI, pinned OCCT8.0.1 candidate, macOS26/Xcode26.6/.NET10.0.302/iOS26.5 Actions.

Constraints: offline execution; all local files/cache/logs/builds/auth under D: workspace; retry transient D: errors after5 seconds; recovery read-only; preserve genuine native outputs and source metadata; no original scripts/account/credential use. No full parity rows close on analytical or simulator evidence alone. Keep live CI and identify physical/PC gates separately.

Requirements: BLD-0516, SER-0551 and CAD-031. Fixture SHA25673e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd; record134 whole curves/explicit parameter branch, three prepared profiles. A01 source/evidence and exact hashes are in ../evidence/P05-T02-A19-A01.json.

## Source-backed design gaps

CSUtilSweep explicit overload lines1360-1402 configures two MRhinoPolyEdge rails; angle tolerance; closed only if both rails closed and requested; same-height flag; rebuild_count=-1; refit/sweep tolerance; simplify=0; sorted duplicated profiles and separate rail parameters. CSUtil defaults tolerance and angle to0.001 only when callbacks are absent; configured/cached document values must remain explicit, not assumed universal constants. RhinoSweep2 implementation is not recovered in this body.

Pinned BRepOffsetAPI_MakePipeShell auxiliary-spine mode exposes length-ratio correspondence or tangent-plane intersection, plus NoContact/Contact/ContactOnBorder. Add accepts profile and optional primary-spine location. This public interface does not expose both arbitrary per-profile rail-parameter lists. Contact preserves width and does not necessarily make the auxiliary rail a boundary; ContactOnBorder makes it a boundary with variable width. Header warns auxiliary contact produces only C0 surfaces and auxiliary mode is incompatible with homothetic laws. These are candidate constraints to test, not assumed Rhino equivalents.

TKOffset is not built in the current minimal TKPrim/TKBO/TKMesh configuration. Build inclusion, complete static link closure and iOS archive merge must be verified before testing an operator. Original Ring/OutsideRail source tolerances and native seam failure policies also need original-PC comparison.

## A02-A01: analytical operator and parameter/contact decision

Files: create native/GeometryBridge/CandidateProbe/candidate_sweep_contracts.cpp; modify CMakeLists.txt and probe contract invocation only after selecting a testable native operator. Keep source hash/decision evidence under docs/porting/evidence/P05-T02-A19-A02.json.

Consumes: PreparedSweep2Candidate rails/profiles/order/Parameters1/Parameters2/EffectiveClosed/MaintainHeight from A01. Native experiment uses independent analytic inputs first; it does not register134.

- [ ] Pin MakePipeShell.hxx/cxx and BRepFill_PipeShell.hxx hashes with kernel revision; record exact methods and missing correspondence controls.
- [ ] Define failing analytic two-straight-rail/three-section case with known surface points, boundaries and section locations. Include unequal native domains, nonuniform stations and independent second-rail correspondence.
- [ ] Build minimal required toolkit closure on Windows D: and macOS; fail on API/link errors before expensive full builds.
- [ ] Evaluate genuine native construction. Verify both rail residuals and all section residuals against independent points, valid topology, face count/orientation and input isolation. A passing Build/mesh alone fails acceptance.
- [ ] Compare asymmetric/variable-width/height cases with height flag false/true; closed primary/secondary seams; nonplanar rail; profile direction mismatch; degenerate/self-intersecting/impossible contact cases.
- [ ] If proposed API cannot respect both explicit lists or height behavior, record the exact failing fixture and design a deeper native constrained operator. Do not silently discard the second parameter list or choose a visually similar loft.
- [ ] Commit a bounded operator decision plus reproducible analytic result; no graph feature claim.

## A02-A02: owned native operation

Files: candidate_abi.h/cpp, candidate_sweep_contracts.cpp; managed CandidateContext.Sweep.cs (new focused partial) and contracts. Reuse CandidateShape ownership/error conventions. Concrete ABI signature is chosen after A02-A01; include counted shape/parameter arrays, explicit tolerances/flags and counted BRep outputs with transactional cleanup.

- [ ] RED: missing typed native sweep entry; then invalid count/null/wrong-context/wrong-kind/nonfinite/tolerance/closed-mode cases, zero result IDs and unchanged inputs on failure.
- [ ] Implement only the demonstrated constrained mode with actual native BRep generation and validation. Dispose every partial native result; typed unsupported errors for all other modes.
- [ ] Test retained output lifetime, repeated success/failure cycles, cancelled work if supported, valid topology and exact analytic section/rail/volume or area evidence where mathematically applicable. Native live entry counts do not prove process-memory freedom.
- [ ] Execute C++ and managed host tests; commit reviewed ABI slice.

## A02-A03: original134 integration and Apple/PC gates

Files: src/Port/Features.Candidate/CandidateRailGraphSession.Sweep.cs (new), dispatch/output owner files, tests/Port/Geometry.Candidate.Contracts/SweepGeometryAssertions.cs (new), scripts/run-native-simulator.sh, original134 work/evidence ledgers.

- [ ] RED original134 unsupported BRep generation. Dispatch only once the operator supports this actual input configuration; keep no-param path explicitly unsupported.
- [ ] Add owned BRep output(s) and source dynamic channel count/null filtering/name behavior. Do not use a curve token or cached mesh to carry topology.
- [ ] Generate real134, non-default radius/height/chooser/profile edits and serialized save/reopen/rebuild; verify rail/section contact, topology, editable native ownership, retained prior generations and cleanup. Never alter opaque records.
- [ ] Run adjacent native/document/source/provenance/repository tests. Require native/managed sweep markers in the simulator launcher.
- [ ] Push and dispatch native host/iPad/iPhone plus unsigned arm64 build; inspect actual family/result/hash/platform/warning artifacts before changing bounded status. Unsigned AOT is not physical execution.
- [ ] Compare Windows Rhino output/tolerances/topology and PC/iPad/PC editable reopening when a contained licensed Windows reference and real portfolio corpus exist. These full parity gates remain open independently of candidate experiments.

Next beyond this plan: no-param branch, other sweep builders and versions, caps/join/solid semantics where callers request them, segmentation for paths that actually require it, scheduling/undo/BRep serialization, export/render/touch UI, catalog and physical-device acceptance.
