# P03-T03-A11 rotation producers and affine products

Goal: execute six exact saved RotateObjectDegXform1.0 nodes, preserving XYZ input defaults, mirrorX/Z, degree conversion and source Rx*Ry*Rz product order. Prepare the real expandable transform consumer without pretending it is implemented.

Sources: recovered CSBlocks.Transforms/CSRotateObjectDegXform.cs and CSBlockUtil/CSUtilMath.cs; fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Official numeric comparison source mcneel/opennurbs6.x commitf2dc7fba67017475b8d530835a40fcfb5372134a: opennurbs_xform.cpp SHA2567b922be97b2d05c50cd5865e32826d87fc2f4926304f55f21bf9344478580258, defines SHA256773d75ecf3dab2d792de490da99a175be548c394a81f11cd9a04cec5c1fe7bd6, LICENSE SHA256de437cd429817f50999710543b9d3e743b17b35b4c96842393b9d39f7bb9b3bd. Published source is a pinned comparison, not the original Rhino5 binary/version proof.

Design: pure managed affine product and finite-angle rotation plans. Adopt documented comparison noise clipping and epsilon constants rather than leaving cardinal-angle noise. Compile unmodified published Rotation bodies in a test-only native observation shim; do not replace production CAD with it. Preserve notices/pin/source hashes under ignored D-local cache, record attribution/provenance. Product remains offline: CI acquisition only, no application fetch. Nonfinite angles reject explicitly because fixed affine DTO cannot represent arbitrary invalid projective rows; original error/Unset policy remains a gate.

Owned: affine DTO/product, rotation plan and known graph/header dispatch; independent reader/oracle extraction; native test reference and pinned preparation/CMake/host-test resolver/workflow integration; tests/ADR/evidence/inventory.

- [x] RED saved rotation114 header/execution missing.
- [x] Pin/acquire/verify source/defines/license and extract native Rotation reference bodies; test-only ABI, native source manifest and source-tamper controls.
- [x] Implement double affine multiply and source-ordered finite rotation/mirror plan; six exact1.0 headers;134 known/107 opaque/100 scalar handles.
- [x] Native reference differential, extracted recovered wrapper Update and ToRadians, cardinal/near-threshold/negative/full-turn/order/default/mirror/nonfinite and independent axis/basis cases.
- [x] Execute all six saved nodes, real native affine curve application/source preservation, edit/save/reopen, dependency/forwarding/failure/cycle/cleanup; retain previous suites.
- [x] Verify source/document/native/repository/provenance and review source/version/runtime distinctions.
- [x] Commit/push ba83e0e; dispatch native host/iPad/iPhone41104238238 and unsigned device41104242957, both queued.
- [x] Inspect actual source44217/published reference/native/family/AOT artifacts before bounded verification. Native iPad/iPhone and unsigned arm64/platform IOS52 warnings0 errors/source pin inspected in rotation-producer-results.json; not original Rhino5/full parity.

Still required: original Rhino5 matrix/runtime comparison, complete nonfinite/Unset/error behavior, expandable input reconstruction, profile metadata/sweep/transform cloning, full transform consumer/arrays/all other builders/UI/PC/physical parity. No full parity row closes. All local writes/cache in D:; wait5 seconds and retry transient D errors.
