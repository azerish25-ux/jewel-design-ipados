# Portable production domain helpers

The current Domain module adopts 12 recovered files unchanged: 579 nonblank material lines and 317 ring-size lines. These are portable helpers, not the complete application. [Provenance](../../src/Port/Domain/provenance.json) records source paths, byte hashes and line counts; run node scripts/verify-production.cjs from the repository root.

## Available APIs

Original namespaces remain intact. GV.CounterSketch.Shared.GemMaterial preserves serialized numeric IDs; GV.CounterSketch.Shared.Utilities.Helpers.GemCaratHelper.GetSpecificGravityForMaterial preserves the mapping and unknown-ID fallback.

CSRingSizes.RingSize contains RingSizeUSA, RingSizeUK, RingSizeFrance, RingSizeEur, RingSizeGerman, RingSizeJapan and RingSizeSwiss, with the original enum and UniversalRingSize result.

- GetSize takes diameter in millimeters; GetSizeRadius takes radius in millimeters.
- GetSizeIndex takes the table position. US size 7 uses position 28. Fractional indices truncate as in the recovery.
- MmDiameter returns the original table-rounded diameter. Result radiusMM is half that diameter.
- TxtSize and GetSizeTxt retain original text/culture behavior. Unknown text can return a partially populated struct; ringSizeType is not uniformly populated by every helper. Consult recovered behavior before treating metadata as authoritative.
- Do not write a displayed rounded radius over the raw project radius. The supplied fixture stores 8.667196228921199 for a size-7 ring; the translator returns radius 8.665. Display and saved geometry are separate concerns.

See [units and legacy-format evidence](UNITS_CONTRACT.md). Binary migration, native graph generation, validation/undo, document units and touch UI remain outside this module's current implementation.

## Verification

From the repository root, with the D:-local wrapper:

    node scripts/run-local.cjs dotnet run --project tests/Port/Domain.Tests
    node scripts/run-local.cjs dotnet run --project tests/Console

Domain.Contracts compares seven ring catalogs, exterior/fractional indices, invalid inputs and en-US/fr-FR text behavior against the hash-locked recovered-source baseline. Console and the simulator app run the same assertions. This is not an original Windows binary comparison. [Classification](classification.json) distinguishes unchanged helpers from new graph infrastructure.

## Numeric edit planning

JewelDesign.Domain.NumericValueState.PlanSetValue captures the recovered scalar setter decision as immutable before/prechange/after states; see [ADR 0002](ADR-0002-numeric-value-plans.md). The test-only source-verified method harness covers 909 bit-pattern cases. Live undo/graph integration is still required. This adapted file does not increase the unchanged recovered-line count.
