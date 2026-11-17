# Public Microsoft CRT100 reference

The original core imports MSVCR100.dll _wcsicmp/_wcslwr. This owned collector loads only the hash-pinned, validly Microsoft-signed installed Windows system runtime10.00.40219.473 copied into ignored D: storage. It never executes Rhino/CounterSketch or loads their binaries. Runtime binaries are not committed.

    node scripts/run-local.cjs dotnet run --project native/Rendering/Crt100Reference -- .local/reference/windows-crt100/msvcr100.dll .local/evidence/crt100/reference-fresh.json
    node scripts/run-local.cjs node --test tests/crt100-reference.test.cjs

Output must be fresh and under the repository. Explicit locales prevent mutation of the process-global CRT locale. Three profiles each capture65536 UTF16 lower units and1156 comparison pairs, each repeated. The two successful complete captures are byte-identical. Initial Turkish_Turkey.1254 creation failed on this runtime/OS; the supported public name Turkish is recorded explicitly. No capture was emitted for the failed attempt.

C matches the previous public UCRT C controls exactly. English and Turkish differ from C on947/946 lower units and186 comparison pairs each. This confirms why current CRT locale cannot be silently assumed C. It does not identify the original loaded runtime build, live locale, ANSI conversion or correct runtime policy. _towlower_l and _wcsicmp_l are the measured public controls; whole _wcslwr buffer behavior is not measured here.
