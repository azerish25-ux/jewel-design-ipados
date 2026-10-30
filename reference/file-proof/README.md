# Limited CSG file-format proof

This is stored-value preservation, not a CounterSketch application save, geometry regeneration or full PC compatibility proof.

    node scripts/run-local.cjs node reference/file-proof/run-file-proof.cjs
    node scripts/run-local.cjs node reference/file-proof/run-independent-check.cjs

Input: [library-item-4.csg](../fixtures/library-item-4.csg). Outputs: .local/file-proof. Node edits graph index 8, type 56b84606-7f1d-41fb-b81f-f521d9a9577d, Top Thickness version 1.1, from 4.38 to 4.48. Hash/type/index matter because two records have the display label.

Expected: 241 records, 74 decoded numbers, 240 other records and 73 other numbers unchanged; ten actual changed bytes restricted to the selected double/CRC; exact no-op/reversal; corrupt CRC rejected independently. Truncation and unsupported version are negative controls. Neither reader loads CounterSketch/Rhino. Cached preview/volume may be stale.

The JSON files retained here are historical reports using original-local path conventions. Regenerate current outputs under .local rather than treating old paths as runnable fixtures. See [the detailed work card](../../docs/porting/workcards/P04-bounded-file-proof.md).
