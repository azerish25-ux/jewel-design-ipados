# P04 — reproducible bounded file-proof work card

## Existing executable baseline

    node scripts/run-local.cjs node reference/file-proof/run-file-proof.cjs
    node scripts/run-local.cjs node reference/file-proof/run-independent-check.cjs

Input: reference/fixtures/library-item-4.csg, SHA256 73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Output stays under .local/file-proof. No target/native library is loaded.

## Required assertions

- Select fixture hash + graph index 8 + type GUID 56b84606-7f1d-41fb-b81f-f521d9a9577d. The display label Top Thickness is duplicated and cannot identify the node alone.
- Preserve record 1.1; change stored double 4.38 -> 4.48 in range 3–6, step 0.01.
- Retain 241 graph records, 74 decoded numbers, 240 unchanged other node records and 73 unchanged other numbers.
- Only bytes in the selected double and leaf CRC whitelist change; ten actual bytes differ.
- No-op and reverse edit exactly match original bytes/hash.
- Both readers reject corrupt CRC; the Node reader also rejects truncation and unsupported numeric record version.

## Next implementation

Create the Documents and Documents.Tests projects from the phase plan. Extract a bounded read model first, with explicit stream limits and opaque payload ownership. Preserve independent readers so the writer does not validate itself. Add the assertions above before moving this functionality into the real codec.

Then add portfolio XML, sidecars, histories, inherited serializer versions, unknown fields and fixtures from the 654-row ledger. The bounded writer deliberately keeps cached preview/volume bytes and does not regenerate geometry. Do not reuse its success as a PC/iPad/PC acceptance result.

Deliver: codec APIs defined in ARCHITECTURE.md, versioned fixture registry, explicit normalization policy, independent comparisons and eventual actual Windows reopen/regeneration evidence.
