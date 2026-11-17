# Public NLS1 ANSI reference

Owned public Windows API collector; it never executes Rhino/CounterSketch or loads their DLLs. All output paths must be fresh and within the D: repository.

    node scripts/run-local.cjs dotnet run --project native/Rendering/AnsiNlsReference -- .local/evidence/ansi-source/native-ansi-fresh.json
    node scripts/run-local.cjs node --test tests/ansi-nls-reference.test.cjs tests/ansi-source-contract.test.cjs
    node scripts/run-local.cjs node scripts/verify-ansi-source-contract.cjs --source ../analysis/runtime-prerequisites/rhino5/System/opennurbsx64.dll

Locale1/flags0x100 lower and0x20001 comparison use CP1256 on the recorded Windows NLS0x60502 profile. The complete256-byte decode/lower maps,65536 repeated A/W comparison controls,65536 two-byte lower buffers and65536 two-byte decode buffers are retained. All1112064 valid Unicode scalars are encoded to UTF8 and actually lowered through LCMapStringA with explicit input counts and ample output capacity. Complete per-plane output digests and changed/invalid counts are retained; two final captures are byte-identical. The independent verifier reconstructs every native digest from byte mappings using Node's separate UTF8 encoder.

System ACP1252 decoding changes10460 comparison results. Bytewise lower changes50349 scalar encodings;4096 outputs are invalid UTF8 (original inputs U+10C000..U+10CFFF). These are measured byte-API outcomes, not an original application round trip. Source review shows that original query/write/decoder error policies, ASCII bypass, stored/C-length behavior and current-CRT fallback must still be implemented. Zero source count yields0/error87 and is a deliberate failure control.

The default source verifier checks manifest integrity only; --source additionally validates3751 original PE bytes across9 whole functions/46 chained regions. macOS CI has no original PE and cannot claim that local byte check. Vendor decompiled C and original binaries stay ignored on D:.
