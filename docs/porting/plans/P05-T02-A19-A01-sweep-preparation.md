# P05-T02-A19-A01 explicit two-rail sweep preparation

Parent A19 goal: build original sweep134 with genuine native topology and source caller/geometry semantics. A01 is the necessary explicit-parameter preparation, not surface generation.

Source correction: CSSimpleSweep2 explicit-parameter branch uses whole profile curves, not CSProfileData segment buffers. Do not make segmentation an invented prerequisite for134. Its stored1.0 header has seven links[4,13,131,132,133,129,130]/channels0; constructor eighth seam option defaults false. GUIDf19ea1ff-57e7-4922-9ff7-87535b49c1c6. Fixture73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd.

CSUtilSweep.Sweep2 explicit overload validates profiles/parameter lengths and rails, duplicates both rails, moves seams only when both closed, maps subsequent original parameter points to copied rails using closest points, otherwise clamps both parameter lists to native domains; stable bubble order by first parameters only; duplicates profiles in that order. Effective closed is both original rails closed && requested flag. Maintain-height is carried to later native construction. No-param branch and original tolerances/order/failure fallback remain separate.

- [x] RED real sweep preparation134 interface/header missing.
- [x] Independently decode exact1.0 common header/bindings;162 known/79 opaque/100 scalar handles, future/structural/no-op/rewire/reopen proof.
- [x] Owned prepared native rails/profiles, source stable order/parameter mapping/clamp/closed flags; explicit clear distinction from BRep generation.
- [x] Real node adapter generates existing upstream closure, reads flatten vs top-level parameter rules and defaults, copies metadata, no preview/substitute geometry.
- [x] Extract source explicit branch/order/clamp observation contracts; analytical open/closed/mixed/tie/non-default/native seam/point/ownership cases.
- [x] Real134 preparation from actual3 profiles and132/133 parameters, edits/save/reopen, snapshots after graph disposal, failures/zero entries; keep prior tests.
- [ ] Review/source/provenance/repository checks; commit/push and Apple runtime/AOT; inspect artifacts.
- [ ] A19-A02 genuine native two-rail sweep/topology/constraint comparison (required).
- [ ] Full graph BRep output/caps/PC/runtime acceptance (required).

Original Rhino sweep tolerances, multi-output splitting, maintain-height/closed/seam/native parity, no-param behavior, other data/builders/UI/PC/physical remain required. Do not register134 as successfully generating BReps until A02 is real. All local files/cache D:; retry transient D errors after5 seconds.

Local review/commands and exact source hashes: [evidence](../evidence/P05-T02-A19-A01.json). Native direct method/helper and initial seam tolerance defects were reproduced and fixed; full local suites pass. Apple results remain pending.
