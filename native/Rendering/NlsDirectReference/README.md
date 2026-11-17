# Direct NLS1 comparison implementation and references

The portable C# engine implements the successful fixed profile used by the original: locale1, IGNORECASE|IGNOREWIDTH, C-terminated UTF16 and the captured sorting snapshot. It reads immutable packed weights/expansions and357 public/native Jamo rows. It calls no Windows, ICU, current-device collation or normalization API.

The implementation uses two mutable logical cursors. Each retains its original position, expansion slots, cached packed weight and Jamo/extra weights. Direct primary decisions precede cached diacritic/extra/punctuation results. Nonspacing sums wrap at8 bits. Native word-punctuation priority, East Asian repeat/lookback and width/kana extras, extension/PUA primary rules, raw-prefix backtracking and supplementary variation selectors remain explicit. It is not a sort-key comparator or a lookup of recorded names.

The captured profile has null case maps and locale-contraction data, traits0 and the original fixed flags. Other locale/flag/state/CRT/ANSI behavior remains separate. The original ON_wString empty wrapper also remains separate: do not use this public-NLS engine as that wrapper without implementing its source branches.

Reproduce public Windows API acquisition on D using fresh raw output paths:

    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsDirectReference -- .local/evidence/nls-direct/new-exhaustive.json
    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsJamoReference -- .local/evidence/nls-direct/new-jamo.json
    node scripts/run-local.cjs node scripts/generate-nls-direct-stress.cjs
    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsStringStressReference -- .local/evidence/nls-direct/stress-inputs.json .local/evidence/nls-direct/new-stress.json

Use compact-nls-direct-reference.cjs with exhaustive/jamo/stress and the reviewed JSON input. It validates profile, counts, native errors/repeats and complete native record digests before generating immutable Brotli fixtures. verify-nls-direct-reference.cjs independently validates all retained artifacts. Production Jamo bytes reproduce from the approved public source via prepare-nls-jamo-inputs.cjs.

The exhaustive fixture evaluates every individual BMP UTF16 unit (including malformed surrogates) and supplementary scalar against empty, ASCII a, previous input and actual native-lower output:4456448 direct results. The Jamo fixture evaluates all1626875 listed leading/vowel/trailing triples against four mutations:6507500 results. Both complete native result streams were reacquired and remain byte-identical. Stress adds103280 repeated direct comparisons: all730 expansion/955 compatibility mappings in contexts, ignored insertions, adjacent-unit/expansion/repeat/punctuation/mark-order mutations and long runs. The older40663 direct comparisons preserve all213 native sort-key ordering disagreements.

Windows C# checks all11107891 native results, plus7 null/budget/terminator guards. These verify the recorded profile, not every Windows version or original application feature. Actual macOS/iOS and original wrapper/state/document/PC/physical gates remain independently recorded.

No original vendor executable or private system routine is executed by the collectors. Decompiled system C remains ignored reference material. The managed engine is an authored state-machine implementation, with byte-level data provenance and independent native outputs.
