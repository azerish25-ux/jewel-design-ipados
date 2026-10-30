# ADR0024: explicit value-channel generations

Status: Windows document/native and28160 source-extracted differential cases pass; Apple proof pending. This is a known scalar/profile dependency slice, not the complete CS graph scheduler.

Recovered CSValue.IsOn reads InOn and defaults on when Boolean conversion fails; flags/visibility are not substituted for activation. CSNumberData uses >0.5 and preserves ref defaults on failed conversion. Base scalar outputs are Value,0-Value,1-Value; invalid/negative output indices fall back to Value. OnOff's inactive group output is numeric0 while ordinary outputs are empty. Chooser OutObject forwards the selected producer output only when active.

ScalarValueRuntime retains Real/Previous and explicit CAM fields. Effective values follow normal/absolute/delta CAM branches including signed-zero arithmetic. ApplyOverride directly changes Real when a numeric override is available, including inactive nodes, without setter epsilon/previous/undo transitions. SetValue separately reuses NumericValueState. Unlinking or disabling an override leaves the prior assigned real value; it does not restore an earlier stored value.

LegacyValueGraphSession explicitly generates known scalar/profile roots and then reads outputs. An iterative dependency preflight rejects cycles, unsupported producers and unresolved positive references before applying overrides. Reads retain explicit cycle/depth guards. Negative object references mean no provider; negative channel indices on a linked numeric provider retain source fallback semantics. Known static profile data are not treated as numeric/Boolean. Unknown producers never become empty/default data.

Existing scalar input slots can be structurally rewired through WithScalarInputLink. Numeric subclass bounds/steps, headers, raw strings and values remain intact; link counts are not silently extended. ExportCurrentDocument writes actual current real values and rejects unrepresentable nondefault runtime CAM fields instead of dropping them. Legacy1.x CAM fields initialize to the recovered constructor defaults because those fields are absent on disk.

BuildFromChannels snapshots six explicit source-ordered links, generates/reads the supported input slice and constructs native profiles/OutsideRail from evaluated data, honoring radius and thickness/default/Boolean conversion paths. Consumer block headers are not yet production-decoded, so the input binding is explicit. Temporary native profiles are disposed while resulting curves retain independent ownership.

The test-only oracle compiles exact extracted Value/IsOn/Update/output and number-conversion bodies with isolated observation types. A verifier compares generated code and seven canonical method hashes; tamper tests catch arithmetic drift.28160 cases cover normal/CAM absolute/delta/zero-delta, NaN/negative-zero/nonfinite values, activation/override and OnOff/base output choices. These prove the extracted field/data behavior, not original native/GUI callbacks.

Graph tests verify fixture baselines, separate update/read behavior, inactive overrides, latching, previous-state preservation, failed-conversion activation, special/negative outputs, CAM export rejection, structural rewire/save/reopen/reversal and cycle/unknown producer rejection. Native tests verify generated overrides change the actual selected-profile rail, saved state reopens/rebuilds consistently, inactive chooser produces the recovered missing-curve error and all temporary entries release.

Remaining work includes complete graph scheduling/dirty/cache/loop behavior, generic producer types, UI-disabled state, chooser disabled cycling, CAM formats/migration, source failure/concurrency policy, typed consumer binding, output-data/dependent builders, original-PC/physical/performance and full application parity. No complete feature row closes here.

[Work card](plans/P03-T03-A06-value-channels.md), [P03 evidence](evidence/P03-T03.json), [oracle verifier](../../scripts/verify-value-channel-oracle.cjs).
