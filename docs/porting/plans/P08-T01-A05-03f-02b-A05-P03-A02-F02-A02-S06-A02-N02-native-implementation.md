# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-A02-N02 native HALFTONE implementation audit

Required precision/source prerequisite for full RGB S06-A02. Input53d2aa371107c2f104fbe3e3e81e7feb8c38c588. Existing2304-case native response rejects the approximate1D model at52 cases. Inspect the current host public-Windows-GDI dependency statically to locate actual resampling/coefficient/rounding logic, separate from original vendor/MFC/OS/driver parity.

Read existing OS dependency binaries only; copy/hash all analysis inputs under D: .local/reference/windows-gdi. No changes to C:, no kernel debugging/injection or original vendor binary execution. Use bounded PE/export/debug metadata and official Microsoft public symbols where available, D:-contained Ghidra/JDK/cache only. Never publish Windows binaries or symbol packages in the repository; retain hashes/RVAs/bounded analysis and findings.

Acquisition acceptance: copied bytes match source SHA; symbol identity matches PE CodeView GUID/age; exact bounded source regions and ABI/control-flow cross-check before changing a portable algorithm. Analysis acquisition validates artifacts rather than invented unit tests. Existing mode4 rejection stays until full HALFTONE/2D/mixed-axis/source integration is proved. No feature parity closure.

Acquired on-disk win32kfull.sys SHA94f00aabe76a4af3794e2134b806bf89e556b66814859c285de62173350a5173 and official public PDB SHA4d7ea254adda641a8074f913b3989435c592986a070a8e45348040ebdafe7799. Exact GUID E44AE6E5F6284389F7A58878592D07B8, image age1/DBI age1/info age3; [Microsoft loader source](https://github.com/Microsoft/microsoft-pdb/blob/master/PDB/dbi/pdb.cpp) supports the observed newer PDB info age when DBI matches image.2397 independent export matches include one explicitly verified E9 memset thunk. Non-image metadata symbols are reported separately, never used as code addresses.

[Native audit](../../evidence/audit/windows-gdi-halftone-source.json) records13 exact code/unwind ranges and decompiler hashes. Only SharpenInput ABI is explicitly reviewed; other signatures/layouts remain inferred. Strict final Ghidra import disables ordinal lookup and all library linking/loading, so it neither searches outside the D: copied input nor reads libraries to populate export caches. All Ghidra cache/log writes stayD. Earlier discovery import read existing system exports read-only; final accepted views are the strict D-only run. No kernel debugging/injection/direct private-function invocation. Windows binaries/PDB packages remain ignored local cache.

Reproduce using the existing D: JDK/Ghidra, GHIDRA_JAVA set to D: java.exe:

    node scripts/run-local.cjs node scripts/windows-gdi-symbols.cjs .local/reference/windows-gdi/win32kfull.sys .local/reference/windows-gdi/win32kfull.pdb .local/evidence/native-halftone/symbol-audit.json
    node scripts/run-local.cjs node scripts/run-ghidra-local.cjs .local/ghidra/projects WindowsGdiHalftoneStrict -process win32kfull.sys -noanalysis -max-cpu 2 -scriptPath native/Analysis/Ghidra -postScript InspectWindowsGdi.java native/Analysis/fixtures/windows-gdi-halftone-regions.json .local/evidence/native-halftone/decompiled-03 . -log .local/evidence/native-halftone/ghidra-03.log -scriptlog .local/evidence/native-halftone/script-03.log

For a fresh import, replace process with import of the cached sys file and add loader PeLoader plus loader-ordinalLookup/loadLocalLibraries/loadSystemLibraries/linkExistingProjectLibraries false (all four). Do not update target hashes if the host OS changes without new provenance and measurements. [Official symbol server](https://learn.microsoft.com/en-us/windows-hardware/drivers/debugger/microsoft-public-symbols) supplies the exact GUID/age-keyed PDB; URL is recorded by the audit tool.

Native-derived reference now matches all2304 stored1D response cases, including the52 prior floating discrepancies. Separate horizontal +4096/shift13 then vertical/four-neighbor shift16 matches108 represented raster cases. All24 mixed-axis raster cases stay unsupported and reported; full filter/generalization is not inferred. Source expansion uses decimal kernels and RaisePower/DivFD6 helpers requiring further review. No production mode4 implementation or parity closure.

    node scripts/run-local.cjs node --test tests/native-halftone-reference.test.cjs tests/halftone-response-reference.test.cjs
    node scripts/run-local.cjs node scripts/halftone-fixed-reference.cjs response reference/fixtures/gdi-halftone-response-windows-26300.json
    node scripts/run-local.cjs node scripts/halftone-fixed-reference.cjs raster reference/fixtures/gdi-rgb-windows-26300.json

Windows21 adjacent regressions and repository integrity pass; macOS artifact validation is required next.

Fresh strict import command (existing D: tools and cached target, set GHIDRA_JAVA first):

    node scripts/run-local.cjs node scripts/run-ghidra-local.cjs .local/ghidra/projects WindowsGdiHalftoneStrict -import .local/reference/windows-gdi/win32kfull.sys -loader PeLoader -loader-ordinalLookup false -loader-loadLocalLibraries false -loader-loadSystemLibraries false -loader-linkExistingProjectLibraries false -noanalysis -max-cpu 2 -scriptPath native/Analysis/Ghidra -postScript InspectWindowsGdi.java native/Analysis/fixtures/windows-gdi-halftone-regions.json .local/evidence/native-halftone/decompiled-03 . -log .local/evidence/native-halftone/ghidra-03.log -scriptlog .local/evidence/native-halftone/script-03.log

Checkpointfa0ab85: repository43901694739 passed. Original [macOS reference validation43901694813](https://github.com/azerish25-ux/jewel-design-ipados/actions/runs/43901694813) remains queued; preserve this run. Expected artifacts: native-halftone-reference-checks.txt (4 passing checks), native-halftone-response.json (2304 exact/no mismatches), native-halftone-raster.json (108 exact/no represented mismatches,24 explicitly unsupported, completefalse). This is reference-domain proof; full mixed/clip/flags/profile/generalization/product/PC/physical acceptance stays open.
