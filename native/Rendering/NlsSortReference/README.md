# Native Windows NLS1 comparison reference

This owned collector executes public Windows APIs only. It does not execute original Rhino/CounterSketch binaries. Inputs match the original static profile: locale1, CompareStringW flags0x20001 and C-terminated UTF16. LCMapStringW flags0x20401 generate diagnostic sort keys; they are not accepted as a substitute for direct comparison.

Use fresh output names because native key records use CreateNew. Local writes stay on D through the launcher:

    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsSortReference -- .local/evidence/nls-sort/new-capture.json
    node scripts/run-local.cjs node scripts/compact-nls-sort-reference.cjs .local/evidence/nls-sort/new-capture.json
    node scripts/run-local.cjs node scripts/acquire-nls-sort-sources.cjs
    node scripts/run-local.cjs node scripts/prepare-nls-sort-weights.cjs
    node scripts/run-local.cjs node scripts/verify-nls-sort-reference.cjs

prepare-nls-sort-weights uses the retained windows-05 native record stream for published/native alignment. New captures must be reviewed and bound explicitly before replacing that baseline.

The retained snapshot contains1114112 individual unit/scalar keys,15547 full-string keys and40663 direct comparisons, each repeated. The3400 targeted variant comparisons cover expansions, accent order, width/kana, Hangul/Jamo, supplementary casing and real filename suffixes. The72 long-input comparisons exercise combining accumulation and punctuation positions at15/16/17,127/128/129,255/256/257,32767/32768/65535 boundaries. Inputs retain UTF16 bytes including malformed surrogates and embedded NUL; Windows APIs receive count-1 and a terminating NUL. The original ON_wString empty/stored-length wrapper is a separate gate.

All native calls succeeded and all repeats agree. Binary comparison of native sort keys disagrees with direct CompareStringW on213 retained pairs. For example, comparing a+grave against grave+a returns LESS, while their keys order GREATER. The actual keys are0e02010f01010100 and0e02010d01010100. Do not normalize inputs or replace the direct API with sort-key ordering to hide this behavior. Per-UTF16-unit key-group concatenation also fails9407 retained full strings and is explicitly rejected.

reference/fixtures/windows-nls1-sort-keys-60502.bin.br retains the complete18810711-byte native stream. Each implicit ascending input0..0x10ffff record contains int32LE key byte length, int32LE error and complete native key bytes. Plane0 is individual UTF16 units; planes1..16 are valid supplementary pairs. The companion JSON fixture retains all full-string keys, native comparison results/repeats, stream/plane hashes and version metadata. An independent Node verifier recomputes key ordering, binds1475 individual/full-string cases, reconstructs the rejected model counts and validates all17 native plane digests.

The [official Microsoft weight download](https://www.microsoft.com/en-us/download/details.aspx?id=10921) has versioned Windows tables. The pinned Windows11 file reports NLSVERSION0x60502 and nominal default GUID matching the captured metadata. Parsed reference data retains60669 default weight rows,730 expansion rows and955 compatibility rows. All6029 eligible ordinary-symbol keys match native outputs. This does not verify special-key/Jamo algorithms or the direct comparer. The complete official source remains acquired under .local for those gates.

Published algorithm/data references: [table format](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-ucoderef/227922dc-ca6e-4ed0-9407-0a4410d65ba4), [GetWindowsSortKey](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-ucoderef/a51f180f-df59-4e36-9c00-6814a6288429), [sort-key comparison guidance](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-adts/dbb175fb-40f3-42a4-9257-4fc02a6fa943). Native direct behavior remains authoritative for this original API/profile.

The parsed weight binary starts with ASCII JDNLSW01 and uint32LE sorting version/default-count/expansion-count/compatibility-count. Defaults are sorted uint16LE input plus4 weight bytes. Expansions are sorted input/first/second uint16LE triples; compatibility rows are uint16LE input/target pairs. It is reference data, not an implemented comparison engine.

Full offline direct comparison, original empty/CRT/ANSI/state behavior, document/view cache ownership and original PC/physical acceptance remain open. No feature row closes from this reference acquisition.
