# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-N04 public Windows GDI RGB reference

UI-039; prerequisite N03 original DIB setup audit. Independent public API measurement while original S05 iOS run43879034941 continues. No original vendor binary/DLL/script execution. Own net10 console executable, all local project/build/cache/results under D:.

Match observed positive-height40byte BI_RGB24/32 DIBs, DWORD storage, CreateDIBSection(NULL,...DIB_RGB_COLORS,NULL,0), CreateCompatibleDC(NULL), selected bitmap and source integer-ratio HALFTONE boundary. Record actual DC stretch/brush/map state, untouched destination bytes, original setup assumptions and OS version. Public-GDI result is not original vendor/MFC/OS/driver parity.

Artifact acquisition acceptance: own-code identity/constant/padded-row/asymmetric/resize ratio boundary cases, repeatability, explicit destination seed dependence and raw stored bytes. Compare ordinary mode3 versus4 controls without assuming a portable filter. Flush before direct memory read and verify native resource cleanup. Validate all output hashes and setup fields; preserve source alpha separately for later original OR repair. No synthetic geometry or preview substitute. Portable RGB implementation and palette/profile/full source/PC/physical proof remain open.

Measured:288 cases/432 controls,42 unique source rasters, zero owned native handles. Fresh host DC mode1/brush0,0/map1/ICM1; destination memory observed zero before explicit seed. Independent verifier preserves original stored bytes, source/destination row/channel layout, mode thresholds and measured differences. Four corpus corruption regressions plus eight adjacent launcher/workflow tests pass.

Reproduce on Windows (existing D: SDK):

    node scripts/run-local.cjs dotnet run --project native/Rendering/GdiRgbReference -- .local/evidence/gdi-rgb-reference.json
    node scripts/run-local.cjs node scripts/verify-gdi-rgb-reference.cjs .local/evidence/gdi-rgb-reference.json
    node scripts/run-local.cjs node --test tests/gdi-rgb-reference.test.cjs

[Retained corpus](../../../reference/fixtures/gdi-rgb-windows-26300.json) supports cross-platform validation; macOS CI validates this Windows evidence without claiming to execute Win32 APIs. [Evidence](../evidence/P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-N04.json) records exact producer/corpus hashes.
