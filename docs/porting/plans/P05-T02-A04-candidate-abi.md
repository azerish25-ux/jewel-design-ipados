# P05-T02-A04 candidate ownership ABI experiment

Goal: expose genuine candidate solids through a versioned extern-C boundary with uint64 IDs, explicit retain/release and typed status codes. Existing self-test entry is insufficient for adapted builders. This independent candidate experiment does not waive T01 full mapping, T03 selection/license/PC gates or mark T04 production ABI complete.

Architecture: candidate_abi.h is a C-compatible fixed-width protocol, candidate_abi.cpp owns OCCT objects and catches all native exceptions. A registry owns contexts; each context serializes operations and owns shape entries with explicit reference counts. IDs are globally unique, nonzero, never reused; foreign/stale handles must fail without dereferencing caller pointers. Context final release closes its shapes; future managed SafeHandle ownership must keep contexts alive while shapes exist. No managed pointer or Rhino layout crosses the ABI.

Scope: ABI version check, contexts and shape retain/release, box, deep copy, copy-transform translation, pairwise common/fuse/cut, volume/validity/solid count, live-entry diagnostics. Explicit fuzzy tolerance in caller units; no claim of Rhino tolerance equivalence or final boolean caller fallback. Booleans request non-destructive inputs. No geometry stubs. Cancellation, persistent archive/mesh export, concurrency throughput, process-memory leak metrics, complete operations and production adapter remain open.

Owned: native/GeometryBridge/CandidateProbe/candidate_abi.h/.cpp/candidate_abi_contracts.cpp; existing CMake/probe integration; P05-T02 evidence. Official pinned source headers BRepBuilderAPI_Copy/Transform and BRepAlgoAPI_BuilderAlgo were inspected for copyGeom/copyMesh and SetNonDestructive.

- [ ] RED: write independent analytic/lifetime/error-buffer contract cases; invoke them from native probe and compile early in CI. New ABI header/implementation absent before this work. Observe intended failure before implementation.
- [ ] GREEN: implement real OCCT-backed protocol; catch Standard_Failure/std::exception/unknown exceptions into status and bounded diagnostic; never let exceptions cross C. Zero output IDs on failed creation.
- [ ] Verify masses 8/4/12/0, source preservation, deep-copy survival after original release, retained/stale/foreign/context-close behavior, invalid/nonfinite inputs, unsupported version/operation, bounded error buffers and repeated lifetime entry counts.
- [ ] Run native host and both iOS simulator cases plus unsigned device build through pinned macOS CI. Inspect actual outputs, not only compile status. No physical-device or PC proof implied.
- [ ] Review lock order, allocation failures, counter overflow, output mutation, read/write bounds and ownership; preserve missing cancellation/leak/PC/SDK-selection gates. Commit/push checkpoints with exact evidence.

RED evidence: run 41025275608 host job 123961805120 failed early at missing candidate_abi.h. Implementation added only after inspecting that log. Green implementation remains on development branch pending actual native/runtime proof. Runs 41025671193 and 41025674158 test the implementation.
