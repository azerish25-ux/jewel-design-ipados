# Windows NLS1 lowercase reference

This collector calls public Windows APIs owned by this project. It never loads or executes original Rhino/CounterSketch binaries. Run through the D-only launcher and use a fresh output name (the raw record file uses CreateNew):

    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsLowerReference -- .local/evidence/nls-lower/new-capture.json
    node scripts/run-local.cjs node scripts/verify-nls-lower-reference.cjs .local/evidence/nls-lower/windows-01.json.native-records.bin

The source audit establishes original LCMapStringW locale1/flag0x100/stored-length arguments. The collector calls that API for every individual UTF16 unit (including unpaired surrogates) and every supplementary scalar pair. It also measures3329 explicit-length contexts, including embedded NUL, malformed surrogate sequences, Greek sigma, Turkish I, sharp S, full-width text and deterministic random strings. Native output uses UTF16LE bytes directly; replacement-based UTF8/Unicode encoders do not alter malformed inputs.

The retained capture has1114112 individual cases,1013 changed mappings,0 errors and0 context-composition mismatches. There are419 differences from the collector host's .NET invariant casing. NLSVERSIONINFOEX is captured before/after; its sorting version is metadata, not a claim that sorting version alone identifies every casing rule. OS, complete native output digests and the approved table SHA independently identify this capture.

The native record stream is13238272 bytes: ascending plane0 units then planes1..16 scalar pairs; each record contains int32LE output-unit-count, int32LE error and the exact output UTF16LE. The17 plane digests and whole stream SHA are retained in reference/fixtures/windows-nls1-lower-60502.json.br. Reconstructing unchanged entries plus all sparse changed entries reproduces the whole native stream digest; this is checked independently in Node and C#.

The8136-byte production resource begins with ASCII JDNLSL01, then uint32LE locale, flags, captured sorting version, effective sort ID, changed-record count and reserved0. Each following record is uint32LE input/output scalar, sorted by input. All captured changes preserve UTF16 width. The loader pins the resource hash and rejects other versions. No current-device casing or culture is consulted.

Microsoft documents that lowercase conversion is not context-sensitive, while mapping lengths and results can vary by NLS version: [sorting and mapping rules](https://learn.microsoft.com/en-us/windows/win32/intl/handling-sorting-in-your-applications), [LCMapStringW](https://learn.microsoft.com/en-us/windows/win32/api/winnls/nf-winnls-lcmapstringw). Version metadata follows [GetNLSVersion](https://learn.microsoft.com/en-us/windows/win32/api/winnls/nf-winnls-getnlsversion) and [NLSVERSIONINFOEX](https://learn.microsoft.com/en-us/windows/win32/api/winnls/ns-winnls-nlsversioninfoex).

This component performs lowercase mapping only. Complete CompareStringW collation, original CRT/ANSI fallback and state changes, other Windows mapping snapshots, document/view integration and original PC/physical acceptance remain open. Do not combine this mapping with an approximate comparer and label the result original filename parity.
