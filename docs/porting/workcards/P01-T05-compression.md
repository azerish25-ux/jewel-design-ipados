# P01-T05 — compression differential work card

## Exact inputs

- Source: recovery/canonical/desktop/GV.CounterSketch.Studio.ZipIt/ZlibStream.cs, DeflateManager.cs, InflateBlocks.cs and the related ZipIt files.
- Existing probe: tests/Smoke/Checks.cs; original test path uses extern alias DesktopRecovered.
- Recorded Windows and iPad result: 100000 repeated x bytes produce output rejected by the BCL zlib decoder. Empty input emits zero bytes, not a complete zlib frame. A short string passes both directions.
- Dependency: a contained original PC binary comparison from P02. Without it, record the blocker; do not guess whether the defect is original or recovered.

## Bounded test matrix

For empty bytes, UTF-8 'CounterSketch portable compression', 100000 repeated x bytes, all 256 byte values repeated, and deterministic pseudo-random bytes, collect:

1. Original PC compressor -> original decoder and independent BCL/zlib decoder.
2. Recovered compressor -> original decoder and independent decoder.
3. Independent compressor -> original and recovered decoder.
4. Input SHA256, compressed SHA256/length, decoded SHA256/length, exact exception and build/runtime versions.

Keep each fixture private/local until its content is cleared; synthetic bytes are sufficient. Do not patch a third-party library merely to satisfy the existing 16 math goldens.

## Implementation and acceptance

Add a dedicated compatibility test project under tests/Port/Compression.Tests and a small original-PC probe in the D:-contained environment. The large-input test must fail against the current recovered implementation before a fix. Determine actual application call sites before changing the format: an original nonstandard stream might be a compatibility requirement rather than a defect to normalize.

If recovery is wrong, repair the port implementation outside recovery/ and preserve an exact failing regression. If original behavior is also nonstandard, implement an explicit compatibility path and separately label standards behavior. Test historical archive payloads before replacing it with a BCL stream. Do not turn the diagnostic into a fake passing assertion or silently remove it.

Reproduction:

    node scripts/run-local.cjs dotnet run --project tests/Console

Deliver: P01-T05 evidence, the matrix above, a reviewed implementation decision, exact code provenance, regressions, and independent decoder results. Full PC document compatibility remains a separate gate.
