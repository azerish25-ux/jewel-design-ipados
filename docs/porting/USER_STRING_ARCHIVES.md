# Native user-string bodies and copied provenance

This is one native userdata body codec and its runtime binding. It is not a whole3dm reader, CounterSketch project writer or complete native class registry. [P02 codec evidence](evidence/P08-T01-A05-03f-02b-A02-P02.json) and [P03 lifecycle evidence](evidence/P08-T01-A05-03f-02b-A02-P03.json) distinguish native reference, managed, simulator and original-PC acceptance.

## Modules

| Module | Responsibility |
| --- | --- |
| [Documents codec](../../src/Port/Documents/LegacyUserStringListCodec.cs) | Bounded native1.0 framing, UTF16, CRC, partial values and original bytes |
| [Rendering concrete list](../../src/Port/Rendering.Candidate/Rhino5ConcreteUserData.cs) | Original-derived mutable pair/copy/assignment/owner lifecycle |
| [Views binding](../../src/Port/Views.Candidate/Rhino5UserStringArchiveBinding.cs) | Actual registered reader, immutable source provenance and output policy |
| [Read origin](../../src/Port/Views.Candidate/Rhino5UserStringReadOrigin.cs) | Immutable archive version/build and actual read observation |

Rendering does not reference Documents. One named friend assembly lets Views attach a private immutable state through an internal marker. This is managed preservation metadata, not an additional recovered native field.

## Reading and copying

Register Rhino5UserStringArchiveBinding once per runtime. Actual unknown conversion uses the recorded class UUID and archive version/build; do not guess those from payload bytes or execute original plugins. The native-derived conversion ignores reader false, so a partial conversion may contain a valid prefix. NativeReadSucceeded, CrcErrorCount, HasUnknownSchema, ConsumedBytes and CopyOriginal must remain distinct observations.

ReadInto appends actual decoded values. CopyReadOrigins returns chronological immutable observations in an independent array; TryGetReadResult returns the latest read observation. Both work after Duplicate/AssignFrom, including assignment across runtimes and disposal of the original. Values, owner links, IDs and native copy-count rules are handled by the concrete list's existing lifecycle. Changing values retains the baseline, allowing edit/revert to restore exact original-byte output. Purging/disposal releases that object's provenance; other copies keep their immutable state.

Each list retains at most1,024 origins and no more total original bytes than configured MaxPayloadBytes. These are explicit managed limits. Rejecting an over-budget or otherwise bounded read leaves prior values and provenance unchanged. Superseded accumulated pair baselines are not retained in the origin chain.

## Writing

| Situation | WriteForArchive |
| --- | --- |
| Fresh list with no origin | Real canonical1.0 writer |
| Single whole-body source, unchanged values and same archive version | Exact original bytes, including future/partial/CRC/trailing data |
| Single complete known source with edited values or changed archive version | Real canonical writer and fresh CRCs |
| Edited or context-converted future/partial/trailing source | Explicit error; source bytes remain available |
| Multiple reads or pre-existing values mixed with a read | Explicit error; no invented single original body |

WriteCanonical is an explicit1.0 rewrite API. Calling it does not prove that opaque/future data was preserved. A complete application save must make that decision visibly and deliberately at the document layer. Raw byte-preserving output also obeys the receiving binding's byte budget.

Supported body contexts are4, early5 and50+ multiples of10. The current native oracle covers4,5,50 with12 actual native writes and1,076 malformed/normal native reads. Valid Unicode, supplementary characters, combining marks and embedded nulls are covered. Windows raw UTF16 semantics are retained; malformed-surrogate equivalence with native wchar32 platforms is not claimed.

## Next integration

[Whole-view copy](plans/P08-T01-A05-03f-02b-A03-runtime-view-copy.md) must carry the actual viewport userdata owner, not merely copy serialized envelopes. Keep those envelopes for file preservation with explicit archive context. Constructor and assignment have different original byte/copy rules; original Initialize resets the opaque+0x1b byte to0. Full native class coverage, view history/document effects, saved PC projects and physical-device validation remain unfinished.
