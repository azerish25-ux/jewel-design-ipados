# Units and ring-size contract: recovered evidence

P03-T03-A03 has one implemented subset: unchanged regional ring-size translators in Domain. General document units and persistent parameter mappings remain unfinished. This is not a declaration that every graph double is a length.

## Explicit units found

- CSValueRingSize.MaxValueRadiusMM/MinValueRadiusMM and ConvertFromUSToMMRadius establish millimeter radii for that value type. Its binary Read converts value/min/max for major 1 and minor <= 1 from US ring sizes. ReadJson does not apply that legacy conversion. P04 must test these different format/version paths rather than copying a migration indiscriminately.
- CSRingSizes.RingSize.GetSize takes a diameter; GetSizeRadius doubles a radius before lookup; GetSizeIndex truncates a numeric index to int; the returned UniversalRingSize carries separate diameterMM, radiusMM, index and Size. Index is a table position, not the printed ring size. US size 7 uses index 28, rounded diameter 17.33 mm and radius 8.665 mm.
- RingSizeUSA.MmDiameter rounds to two decimal places. CSValueRingSize.ConvertFromUSToMMRadius uses an unrounded formula and native OnUtil.On_PI. Do not substitute table-rounded values for legacy binary migration or claim bit-identical native conversion without comparison.
- CSApplication startup assigns geometric Tolerance=0.001 and AngleToleranceDegrees=1.0; AngleToleranceRadians comes from CSCoreMath.ToRadians. These are separate quantities, not evidence of the document's Rhino model unit setting.
- CSGemInChannelSetUp passes iGemRotation through ToRadians before a rotation. A generic CSValueRange/Double channel does not by itself distinguish length, angle, ratio or ring-size index.

## Adopted behavior and limitations

All ten CSRingSizes.RingSize files are copied byte-for-byte from the canonical CAD baseline. Seven translators, the enum, base helper and result struct are hash-locked in Domain/provenance.json. Original namespaces, enum IDs, rounding, culture-sensitive text parsing, out-of-range results and partial result metadata remain unchanged. For example, some base helpers leave ringSizeType at its default while GetSizeTxt explicitly sets it; callers must not assume every returned field was populated uniformly. This is source fidelity, not a corrected redesign.

The shared RingSizeAssertions suite compares every catalog index and three exterior indices per region in en-US/fr-FR, fractional index truncation, nonfinite/negative diameter outcomes, null/invalid/numeric text and fixed US/UK examples. Equal outcomes include exception type where relevant. The oracle is the hash-locked recovered C# baseline, not a running Windows binary. The suite executes through Domain.Tests and the existing Console/iPad/iPhone harness. No ring UI, graph regeneration, arbitrary model unit conversion or PC-file migration is implemented here.

## Next required evidence

1. Identify document/native settings and their serialized unit metadata with an actual PC fixture; preserve unknown values instead of assuming millimeters for every field.
2. Map quantities by serializer/type plus field/channel, retaining raw numbers and versions. Do not infer units from a display label.
3. Add codec fixtures for ring-size binary versions 1.0/1.1/current and JSON, including min/max and unknown records. Compare original PC outputs before closing compatibility rows.
4. Integrate graph edits, undo, regeneration and UI conversion without changing rounded/unrounded behavior.

## Existing library fixture

The [bounded fixture observation](../evidence/audit/ring-fixture-unit-facts.json) identifies node 2 as ValueRingSize version 1.2. Its saved value 8.667196228921199 agrees with the unrounded US-size-7 conversion; bounds agree with sizes 3 and 11 within 1e-12. The table translator displays size 7 with radius 8.665 after diameter rounding. This observed difference supports preserving the raw saved number while displaying a translated size. It does not prove a PC reopen or native conversion implementation.
