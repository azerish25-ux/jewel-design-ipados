# P04-T01-A02 additional version-1.1 scalar layouts

Goal: preserve and edit the stored numbers of CSValue, CSValueOnOff and CSValueChooser records in the available CSG library, without inventing a Boolean encoding, bounds, UI selection behavior or geometry. This bounded extension does not close P04 or native PC round-trip gates.

Recovered source chain inspected: CSObject.Read -> CSBlock.Read -> CSValueBase.Read -> CSValue.Read. Version1.1 has identifier, raw flags/material-layer integer, user-data triples, paired object/channel arrays, category array, legacy UI integer, then one stored double. CSValue.Read adds CAM fields only for major>=4. OnOff has no Read override. Chooser.Read only derives an override-image list from user data after base.Read, with no extra archive fields. The fixture has 15 OnOff, 7 Chooser and 4 plain Value records, all1.1. Only this exact profile is supported initially.

- [ ] Write independent reference-reader tests first, observe missing module failure, then inspect/strictly consume these 26 actual bodies. Confirm bounds, link-array mismatch, corrupt CRC, future version skip and raw IEEE754 representation. Preserve source hashes and a reference report.
- [ ] Add production codec RED tests for 100 decoded scalar payloads (74 existing bounded numeric +26 plain), preserving the existing Numeric API/count74.
- [ ] Extract immutable common scalar model from existing Numeric; retain Numeric bounds/step in a derived type, never fabricate bounds for plain values. Record.Scalar supports all100; Numeric remains the bounded subset. Refactor shared encoding structurally, preserving raw strings, flags, links and bits. Unknown versions stay byte-preserved/noneditable.
- [ ] Add explicit WithScalarValue while preserving WithNumericValue semantics. Wire decoded scalar metadata and optional scalar edit-session support as independently tested changes; do not claim OnOff output, chooser group mutation, live undo callbacks or CAD regeneration.
- [ ] Verify byte-identical no-op/reversal, every other record retained, finite and nonfinite bits, corruption/version/bounds guards. Reopen output with independent reference/CSharp readers; run Windows and Mac shared contracts. No PC application reopen is supplied.

Owned initial oracle: reference/file-proof/scalar-v1.cjs, tests/scalar-v1-reference.test.cjs, docs/evidence/audit/plain-scalar-reference.json. Production follow-up: LegacyCsg4 models/codec/projection/edit session and Documents.Contracts; keep source recovery read-only.

Reference RED/green complete: missing module failure observed first, then all4 actual-fixture/boundary tests passed. The source-hashed reference report confirms26 complete bodies. Production still74 decoded bounded numeric records; structural writing, graph integration and Mac checks remain future work.
