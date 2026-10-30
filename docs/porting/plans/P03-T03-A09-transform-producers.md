# P03-T03-A09 translation and scale-to-size producers

Goal: execute six saved TranslateObjectXform1.0 and seven ScaleObjectToSizeXform1.0 dependencies in the real profile transform chains. No replacement geometry or skipped inputs.

Source SHA256: CSTranslateObjectXform.cs 863bb6cf6dc9227db1374f2a38afa127dc1002bd177f5e63a2b291926d6c47a0; CSScaleObjectToSizeXform.cs 0e8bce0c130d7e4aee9e689440c97c1b1da517250ab15ab352bf9fc4307c413a. Inspect CSXformData/ProfileData/common serializer too. Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Translation50 links40/41/42/missing, mirror56 input46; scale49 chooser33->group31 and sizes35/36/missing.

Design: immutable12-double affine DTO in Documents. Candidate graph stores pure transform outputs. Translation defaultsXYZ/mirror0, mirror>.5 negatesX/Z only. Scale imports real profile or resolves curve, obtains native tight bounds, floors each extent<.01 to1, defaults target sizes to adjusted extents, scales about WorldXY origin. Missing/unsupported geometry rejects explicitly, with no fabricated bounds. Existing native affine API proves application.

Owned files: exact GUID header codec, affine/channel data, candidate graph dispatcher/transform partial, independent reader, source-extracted Update observation harness, document/native/source tests, inventory/ADR/evidence/CI.

- [x] RED saved translation header/execution missing.
- [x] Independent fully consumed16 common headers and exact1.0 codec; preserve future/opaque/common bits, rewind/save/reopen;125 known headers/116 opaque/100 scalar handles.
- [x] Immutable affine data and source-ordered translation/scale producers using actual native geometry bounds; no new kernel API.
- [x] Extract exact source Update with narrow observation stubs; test default/conversion/threshold/NaN/-0/size floor/origin and reject source tampering.
- [x] Six original translations/four original scales execute; three original scale chains explicitly reject their unported placement dependency. All seven scale plans execute with explicit resolved Add inputs. Real native application, edit/save/reopen, selector forwarding, failure/cycle and cleanup pass.
- [x] Focused/adjacent native/document/reference/provenance/repository checks, review and ADR/evidence.
- [ ] Commit/push and dispatch macOS native simulator/device; inspect actual reports before bounded verification.

Limits: native tight bounds vs original Rhino tolerance/bits remain a PC gate. Array/solid/other data bounds, complete Unset validity, rotation/placement/expandable transform consumer, scheduler/UI/render/export/PC/physical/full-design parity remain required. No full parity row closes. All local writes/cache stay D:.

Integration RED exposed saved scale dependencies on CSAdd111/118/125. Include those exact1.0 common headers and known non-array scalar addition (defaults0, outputs sum/0-sum/1-sum). No array value producer is admitted or collapsed. Full CSAdd array Cartesian behavior remains open.

Integration results:16093 extracted-source comparisons pass. Original scale112/119/126 -> Add111/118/125 -> placement110/117/124 remains unsupported and tested as an explicit failure. Next port placement with ring4/outside13/scalars94/95; never replace its height with a stored/default value. No-argument Generate keeps prior known-rail roots; new transform/math roots must be requested explicitly.
