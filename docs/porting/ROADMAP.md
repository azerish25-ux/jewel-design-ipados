# Full offline iPadOS/iOS port roadmap

This gated plan organizes the work; it does not claim parity is easy or implemented. Read [CURRENT_STATE](CURRENT_STATE.md) and [AGENT_PLAYBOOK](AGENT_PLAYBOOK.md).

| Phase                                                                | Deliverable                                     | Dependencies                      |
| -------------------------------------------------------------------- | ----------------------------------------------- | --------------------------------- |
| [P00](phases/P00-recovery-integrity-and-agent-handoff.md)            | Recovery integrity and agent handoff            | —                                 |
| [P01](phases/P01-apple-runtime-and-differential-test-laboratory.md)  | Apple runtime and differential test laboratory  | P00                               |
| [P02](phases/P02-contained-windows-oracle-and-fixture-corpus.md)     | Contained Windows oracle and fixture corpus     | P00                               |
| [P03](phases/P03-portable-domain-and-graph-contracts.md)             | Portable domain and graph contracts             | P00                               |
| [P04](phases/P04-loss-preserving-pc-document-codecs.md)              | Loss-preserving PC document codecs              | P02, P03                          |
| [P05](phases/P05-offline-cad-backend-and-native-bridge.md)           | Offline CAD backend and native bridge           | P02, P03                          |
| [P06](phases/P06-one-complete-jewelry-vertical-slice.md)             | One complete jewelry vertical slice             | P04, P05                          |
| [P07](phases/P07-all-builders-and-legacy-graph-behavior.md)          | All builders and legacy graph behavior          | P06                               |
| [P08](phases/P08-viewport-materials-rendering-and-try-on.md)         | Viewport materials rendering and try-on         | P05, P06                          |
| [P09](phases/P09-complete-native-touch-interface.md)                 | Complete native touch interface                 | P06, P08                          |
| [P10](phases/P10-catalog-pricing-projects-quotes-and-reports.md)     | Catalog pricing projects quotes and reports     | P03, P09                          |
| [P11](phases/P11-stored-scripts-and-aot-safe-execution.md)           | Stored scripts and AOT-safe execution           | P04, P05                          |
| [P12](phases/P12-manufacturing-export-and-hardware-contracts.md)     | Manufacturing export and hardware contracts     | P05, P07                          |
| [P13](phases/P13-lifecycle-persistence-performance-and-packaging.md) | Lifecycle persistence performance and packaging | P09, P10, P11                     |
| [P14](phases/P14-full-interoperability-and-feature-acceptance.md)    | Full interoperability and feature acceptance    | P07, P08, P09, P10, P11, P12, P13 |
| [P15](phases/P15-physical-device-release-and-parity-sign-off.md)     | Physical-device release and parity sign-off     | P14                               |

Ledgers contain 1451 rows: 68 ui, 52 cad, 58 pc, 654 serializer, 619 builder. Serializer and builder catalogs overlap and are not independent feature counts.

P01/P02 can progress independently. P03 defines state; P04/P05 provide real documents and CAD. P06 proves one complete edit/save/reopen and measures adaptation. P07–P13 cover features and mobile integration. P14/P15 alone support a release claim. Early reference capture is allowed; closure still requires dependencies.

See [BLOCKERS](BLOCKERS.md). Live services cannot be identical offline; unknown future projects, arbitrary Windows scripts and native plugins are not silently promised. Required gaps remain visible.

P03 can start after P00 independently of the Windows/compression blocker. P01-T06 follows the completed runtime assertions and can proceed while P01-T05 awaits the original-PC comparison.
