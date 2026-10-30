# Recovery map

## Authoritative baselines

- [Desktop](canonical/desktop): installer-studio extraction; 1,229 CSharp files, WPF/WinForms, original framework 4.6.2.
- [CAD/server](canonical/cad): server-complete-refs; 1,554 CSharp files, original framework 4.5.2. Prefer this to reference-poor earlier passes.
- [Support](support): nine recovered assemblies, 333 CSharp files. Includes the 25-file GemvisionGeometry implementation.

## Variants

[desktop-xaml](variants/desktop-xaml) contains 113 editable XAML files plus its parallel CSharp extraction; markup is supplemental and not asserted identical to the canonical build. [studio-loose](variants/studio-loose) differs in authentication behavior and is comparison evidence, not the canonical entitlement implementation. [server-without-refs](variants/server-without-refs), [server-modern](variants/server-modern) and [server-rhino5](variants/server-rhino5) are earlier recovery passes. [CounterSketchCore](variants/CounterSketchCore) is a tiny partial extraction, not a usable CAD engine.

Variants remain separate because combining duplicate types into one project is incorrect. Every recovered source/text/project/markup file is exported with its hierarchy.

## Binary resources

[resource-files.json](manifests/resource-files.json) maps all 2,241 original resource paths to 744 unique objects. PNG/JPEG/BMP/ICO/PDF/BAML duplicates share one content-addressed object. Original semantic names are retained in the map. BAML is compiled markup; editable XAML is the UI inspection starting point.

    node scripts/recovery.cjs assets headstyle
    node scripts/recovery.cjs materialize installer-studio

Materialization reconstructs a sanitized original tree under .local/materialized. It does not install dependencies or run the Windows application.

## Integrity

[source-files.json](manifests/source-files.json) stores original/export paths and exported SHA256. [redactions.json](manifests/redactions.json) stores credential locations/categories without values. Redacted originals and their original hashes remain private on D:. [Security/provenance policy](../docs/porting/SECURITY_AND_PROVENANCE.md) explains what changed. Installers, native runtimes, SDK caches and signing/license material are excluded; source and every recovered resource are included.
