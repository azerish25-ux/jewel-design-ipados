# Owned-process direct NLS dispatch and inputs

This probe calls public CompareStringW with locale1/flags0x20001 on controlled strings. It follows only public entry thunks and reads bounded NLS hash/context/table spans in its own process. It never launches or executes the original Rhino/CounterSketch binaries, calls a private comparison routine, modifies code/data, or examines another application. System images are read-only; every generated file stays in the D repository.

The exact kernelbase/kernel32 image hashes are mandatory before using symbol-derived offsets. Hash roots are128 inline pointer slots, not pointers to arrays. Initial pointer-layout candidates were rejected; the accepted trace identifies kernel32 SortCompareString at RVA23616, through context callback offset0xf0. Its live instruction bytes match the pinned image. The traced locale1 context has null casing maps130/138, traits140=0 and null compression150. This supports the fixed-profile specialization; it does not implement the comparison algorithm.

The generic GDI export audit rejected kernel32's legitimate FooStub aliases. The dedicated audit preserves exact GUID/image/DBI-age checks and validates890 direct plus581 explicit Stub export identities, including all12 conflicting internal/public names. Its live callback match and private algorithm symbols are separately checked.

Reproduce with the matching approved system/PDB snapshots on D:

    node scripts/run-local.cjs dotnet run --project native/Rendering/NlsDispatchReference -- .local/evidence/nls-engine/dispatch-07.json
    node scripts/run-local.cjs node scripts/inspect-nls-dispatch-symbols.cjs
    node scripts/run-local.cjs node scripts/prepare-nls-comparison-inputs.cjs
    node scripts/run-local.cjs dotnet run --project tests/Port/Rendering.Tests

The probe retains complete active native weights262144 bytes and ordered expansions2920 bytes beside its JSON. Native input hashes are compared against a reconstruction from the approved public Windows11 default/expansion/compatibility data. Preserve source expansion order: sorting by source character changes marker indices. Resolve compatibility aliases after their targets. The complete reconstructed arrays match active memory byte-for-byte.

The production input resource has ASCII JDNLSI01 then uint32LE locale/sorting-version/weight-count/expansion-count, followed by65536 packed uint32LE weights and730 pairs of UTF16LE units. Packed weight bytes are AW,SM,DW,CW in increasing byte significance. Its immutable managed provider exposes all units, including NUL/unpaired surrogates, without using device casing/collation APIs. It is an input provider only.

Static Ghidra views remain ignored under .local/evidence/nls-engine/decompile-02. The fast wrapper has9 chained unwind ranges; LongCompareString is a complete17960-byte body taking one state pointer, not6 public-API arguments. Four annotated views and all function bytes are hash-locked. The fast wrapper's cookie-check return model is not accepted as a literal implementation; raw instructions and call sites remain authoritative. Original decompiled system C is not shipped as production code.

The initial import completed after expensive external-library searching. For future imports use the pinned loader switches -loader-linkExistingProjectLibraries false -loader-loadLocalLibraries false -loader-loadSystemLibraries false; processing the existing project avoids a new import. An attempted stop found the process already terminal, and the successful original handle was retained. No live job was restarted because of an observation timeout.

Full direct comparison transitions, Jamo/special/overflow behavior, original ON_wString wrapper/CRT/ANSI/state/document integration and PC/physical/full application acceptance remain open. The213 native direct-versus-key disagreements remain mandatory future engine regressions.
