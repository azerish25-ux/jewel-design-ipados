# P04-T01-A01 structural legacy CSG-4 library codec

Goal: read and structurally rebuild the observed legacy library archive, edit decoded numeric records and reopen the output while preserving all opaque regions. This is a production file-format primitive, not PC application reopening or geometry regeneration.

Independent prerequisites: hash-locked library-item-4 fixture, Node/CSharp proof readers and completed portable project foundations. Full P04 still depends on P02/P03; portfolio/XML/current versions/all serializers and PC oracle remain outstanding. No native runtime/account use.

Source: CSDocument.Read/WriteCSFileInformation, CSObject/CSBlock/CSValueBase/CSValue/CSValueRange/CSValueRingSize readers; reference/file-proof/csg-v4.cjs and IndependentVerifier. Fixture SHA256 73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd. Observed archive 4 marker b237ee6c-ac7e-4725-86ee-54bccda34fea, graph/gem/category 1.0, info 1.0 (source also specifies 1.1); known numeric Range 1.1/RingSize 1.2.

Architecture: Documents owns immutable records, decoded numeric metadata, bounded readers, structural record/graph writer and CRC32. Unknown types/known unknown numeric versions remain opaque and non-editable. Prefix data is preserved; known numeric bodies are rebuilt field by field with original UTF16 code units/termination form, arrays and user-data order. Graph directory CRC covers directory only; record CRC covers full body; EOF length is rebuilt. No offset byte patching. Opaque preview/gem nested checksums remain unverified and explicitly reported. IEEE754 bits are preserved; PC acceptance of nonfinite values is unverified. No graph projection with guessed header flags or identities.

Owned: src/Port/Documents; tests/Port/Documents.Contracts and Documents.Tests; Smoke/CI wiring; evidence/classification/ADR. Shared fixture is embedded so the same codec reads/edits/writes/reopens on iPad/iPhone. Console exports bounded outputs under .local/file-proof/production for independent Node/CSharp checks.

- [x] Write failing fixture tests first: 241/74 counts, no-op/reversal byte identity, one numeric edit, other records unchanged, alias isolation, CRC/truncation/count/header/EOF rejection, opaque future type/version retention and edit rejection, nonfinite/signed-zero and raw UTF16 preservation.
- [x] Implement bounded codec/models and full known-record writer; expose validation coverage honestly.
- [x] Run local/shared/independent readers, source/security/integrity checks; review unsupported data and regeneration claims.
- [x] Commit/push; inspect macOS iPad/iPhone codec output and runtime family.
- [x] Commit/push results; parent phase/tasks and all full PC/geometry parity gates remain incomplete.
