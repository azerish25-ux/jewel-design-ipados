# Current verified state

The recovery is present; the application port is unfinished. Feature/type parity rows remain unverified.

| Result                    | Evidence / limit                                                                                                                                                                                    |
| ------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Recovery                  | 10,405 text/source/project/markup files, 10,238 CSharp files including duplicates/variants                                                                                                          |
| Resources                 | 2,241 semantic paths, 744 objects, zero exclusions                                                                                                                                                  |
| Redactions                | Remote credential literals removed in 31 files; local originals unchanged                                                                                                                           |
| Main baseline             | 2,783 files / 441,299 nonblank lines; zero changed original hashes                                                                                                                                  |
| BCL compilation           | 1,391 main files / 80,939 lines before excluding two credential-containing copies                                                                                                                   |
| Exported reuse experiment | 1,414 files / 83,929 lines including 3,100 separate geometry-helper lines                                                                                                                           |
| Device build              | [Unsigned ios-arm64 Release/Mono AOT](https://github.com/azerish25-ux/jewel-design-ipados/actions/runs/40953692111/job/123728156674): 47 warnings, zero errors, 9m17s; no physical execution        |
| iPad execution            | [Signed standalone simulator run](https://github.com/azerish25-ux/jewel-design-ipados/actions/runs/40969820121): 16 original-binary Bernstein goldens, vector length/cross and diamond density pass |
| Compression               | Empty output is not a complete zlib frame; 100000 repetitive bytes fail BCL decoding on Windows/iOS. Non-gating diagnostic, unresolved compatibility defect                                         |
| Limited CSG proof         | Node editor + independent CSharp reader: 4.38 to 4.48; 240 other records and 73 other numbers preserved; byte-identical no-op/reversal; corrupt CRC rejected                                        |
| PC regeneration           | Not run; no contained Windows reference or representative PC portfolio corpus supplied                                                                                                              |

The 18.3% compilation figure is not feature progress. It includes data, generated classes, annotations, duplicates and Windows imports. More code may be adapted; the remainder is not automatically rewritten. No reliable new-code LOC minimum exists. Historical 500k–1M allowances were withdrawn as requirements. P06 measures actual adaptation before re-estimation.

Still required: genuine local CAD, builders, editable PC/legacy codecs, touch UI, rendering, complete offline data, supported scripting and physical-device acceptance. Repository checks and smoke tests do not prove full parity.

P01-T06 simulator family coverage is verified for iPad/iPhone; the unsigned device build still has no physical execution. Next: P01-T05 needs the original compression oracle. P03-T03 is implementing graph/parameter/unit contracts; see [units evidence](UNITS_CONTRACT.md) and [snapshot identity decision](ADR-0001-snapshot-parameter-addresses.md). P02 obtains the Windows oracle and twenty-family fixtures. The startup diagnosis is proven: SIGKILL (Code Signature Invalid), CODESIGNING/Invalid Page before managed Main; valid ad-hoc signing fixed it.

Production Domain now adopts 12 unchanged recovered files (materials plus seven ring-size region translators). The material, graph/address and new ring checks pass on both iPad and iPhone simulators; see [ring results](../evidence/apple/production-ring-results.json) and [graph results](../evidence/apple/graph-contract-results.json). These helpers do not implement ring UI, native CAD or full document-unit migration.

NumericValueState now plans CSValue.SetValue scalar state transitions without native callbacks. The source-verified method harness passes 909 bit-pattern cases locally; both iPad/iPhone simulators passed the same cases through macOS CI; see [results](../evidence/apple/numeric-value-results.json). See [the contract decision](ADR-0002-numeric-value-plans.md). This does not implement live undo, CAM, override channels or graph regeneration.

UndoJournal now adapts recovered undo/redo scheduling through typed target snapshots. The source-linked comparison passes 6130 steps locally and on both iPad/iPhone simulators; see [results](../evidence/apple/undo-journal-results.json). This is a scheduler with isolated test targets, not complete CounterSketch object snapshots or native/UI undo integration. See [the boundary decision](ADR-0003-portable-undo-journal.md).

GraphIdentityScope now supplies per-record reference identities and parameter handles that survive explicit mappings between immutable views; InstanceIdentity preserves decoded raw text and the recovered GUID fallback rule. Identity/ownership assertions pass locally and on iPad/iPhone simulators; see [results](../evidence/apple/session-identity-results.json). [ADR 0004](ADR-0004-session-parameter-identity.md) separates registration provenance from live mutable state and save-time migration.
