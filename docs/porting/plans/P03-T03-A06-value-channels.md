# P03-T03-A06 scalar channels and explicit value-graph generations

Goal: evaluate known legacy scalar/profile channel dependencies rather than always using stored scalar fields. Preserve source activation, number conversions, OnOff special outputs, CAM getter branches, Update override latching and previous-value semantics. Complete scheduler/CSBlock/UI/native producer parity is not implied.

Sources: CSValue.Value/IsOn/Update/GetOutputChannelData, CSValueOnOff/Chooser getters, CSInputChannel conversions, CSNumberData and CSData. Source InOn defaults to true on failed conversion; numeric Boolean uses >0.5; inactive OnOff group output is zero; base negative/unknown numeric outputs fall back to Value. Override updates Real directly without setter epsilon or previous/undo change, including when inactive. Known legacy1.x records have no serialized CAM fields.

- [x] RED value-channel rules/session APIs absent.
- [x] Compile test-only exact extracted source getter/activation/update/output bodies with observation stubs; hash/body verification and bit-pattern differential tests.
- [x] Add portable value state/output and a Documents session with explicit Generate/read phases, known scalar/profile data, recursive activation/chooser reads, dependency/cycle/unsupported producer checks and persistent override state. No unsupported link becomes empty/default data.
- [x] Add structural existing-scalar input-link rewiring preserving bounds/header/raw fields and exact save/reopen/reversal.
- [x] Test real fixture values, inactive/empty/zero/missing/conversion, override latching/previous state, negated/one-minus/special channels/CAM/-0/NaN, cycles/unknowns, rewires/save/reopen and generated chooser-to-native profile flow.
- [x] Run local source oracle, document/native/reference/provenance checks; document known subset vs complete graph semantics in ADR0024.
- [x] Push/dispatch actual Apple simulator/AOT jobs, preserving earlier runs. Runs41084116370/41084119917 at8a764f7 confirmed live/queued.
- [x] Inspect actual channel/source-case/native-geometry/family/AOT artifacts before bounded verification. Actual iPad/iPhone reports and unsigned arm64/platform IOS device build52 warnings0 errors recorded in value-channel-results.json; full parity remains open.

Limits: no full graph scheduling/dirty propagation/loop/UI semantics, unsupported producer implementations, IsUIDisabled/chooser disabled cycling, CAM serialization/migration, arbitrary input kinds or original-PC/physical/app parity. Legacy known input1.x mode initializes absent CAM fields to constructor defaults; runtime CAM state is explicit.
