# Recorded public CRT100 width8 qsort reference

This owned collector loads only the hash-pinned, Microsoft-signed installed MSVCR100.dll copy on D:. Original CounterSketch/Rhino binaries never execute. Their ON_qsort import/thunk is inspected statically only.

    node scripts/run-local.cjs dotnet run --project native/Rendering/CrtSortReference -- .local/reference/windows-crt100/msvcr100.dll .local/evidence/native-cache-sort/reference-fresh.json
    node scripts/run-local.cjs node --test tests/crt-sort-reference.test.cjs
    node scripts/run-local.cjs node scripts/verify-crt-sort-reference.cjs --source .local/reference/windows-crt100/msvcr100.dll

Each record is8 bytes carrying a signed key and immutable input identity. Each array is sorted twice, with canary guards and complete permutation/key validation. Callback identity-pair/return-value events are hashed as three little-endian int32 values per call. Magnitudes1 and53 exercise sign handling. Exhaustive ternary arrays through length8 cover9841 cases; boundary, equal, ascending/descending, alternating/mod7, organ-pipe, extremes, almost-sorted and seeded random arrays extend to4097 elements.

Final result:10431 repeated cases/1972827 callback events per capture pass/10053 managed-reference order differences. Two complete acquisition files are byte-identical. The portable C# implementation must match every final identity and callback trace, not just sorted keys. Node independently reconstructs all9931 short-sort cases and checks every case's bounds/identity/order/guards; source bytes are checked locally with --source. Remote default source checking is manifest-only.

Scope: pinned installed runtime10.00.40219.473, normal deterministic comparisons and width8-equivalent value sorting. This does not establish every original runtime version, unstable concurrent comparisons, native image objects, GDI bsearch, entry string copying or full document/PC/physical parity.
