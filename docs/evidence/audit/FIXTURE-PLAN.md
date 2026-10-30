# CounterSketch acceptance fixture plan

No iPad or application round-trip acceptance has passed. The available library sample is not an edited PC portfolio.

## Levels

- L0: Source linked and original bytes retained.
- L1: Container/directory parsed; not editable compatibility.
- L2: All fields/links decoded and preserved with reference assertions.
- L3: iPad edit/recompute compared to reference.
- L4: iPad-saved project reopens and remains editable in target PC build.
- L5: Manufacturing/render/device acceptance passes.

## Required fixture families

### FIX-001 Portfolio bundles

- [ ] CSP XML, multiple base/variant CSGs, favorites, sidecars, Unicode metadata and relocated paths.

Current availability: No edited PC portfolio supplied.

### FIX-002 Binary containers

- [ ] All four envelope UUIDs; archive length widths, old/current record versions and provenance.

Current availability: One library-derived archive-4/B237 sample directory observed.

### FIX-003 GUID coverage

- [ ] All 654 declared GUIDs and supported version branches, including 272 legacy/obsolete types. Reserved null node is an expected-error case.

Current availability: 43 distinct GUIDs observed in one byte directory only.

### FIX-004 CAM and typed values

- [ ] Delta/absolute CAM, notes, defaults, numeric/boolean/string/choice/gem/font controls and overrides.

Current availability: Source indexed; semantics untested.

### FIX-005 Graph links/grouping

- [ ] Forward/static/expandable links, parents/import offsets, duplicate parts, old groups and selected subsets.

Current availability: No runtime comparison.

### FIX-006 Iterations/variants

- [ ] Typed options, alternative selections, history, GUID collisions, cleanup and value-only loading.

Current availability: No application round trip.

### FIX-007 Embedded geometry

- [ ] Native Rhino objects, trims, curves, meshes and binary fields lacking derived JSON serializers.

Current availability: Payload semantics unverified.

### FIX-008 Fonts/engraving

- [ ] Available/missing fonts, old enum IDs, text geometry and substitutions.

Current availability: Reference fixtures missing.

### FIX-009 Materials/display

- [ ] Old layer migration, groups/overrides, metals/gems, visibility and imported parts.

Current availability: Runtime renderer unverified.

### FIX-010 Jewelry extremes

- [ ] Heads, shanks, halos, pave, channels, clusters, sweeps, morphs, near-tolerance booleans and degenerate inputs.

Current availability: Representative bodies reviewed; kernel fixture corpus missing.

### FIX-011 Stored scripts

- [ ] Source/configuration/ref outputs, compiler errors, supported APIs and external Windows/filesystem dependencies.

Current availability: No representative project scripts supplied.

### FIX-012 Catalog packs

- [ ] CSGL compression, real ZBin/profile selection, gem exclusions, missing packs and old/new catalogs.

Current availability: One pack decoded; full selected catalog absent.

### FIX-013 JSON/GZip

- [ ] Root/derived fields, typed options, unknown GUIDs, large data, truncation and read-result propagation.

Current availability: No lossless neutral-format parity established.

### FIX-014 Backups/state

- [ ] Nested CSB ZIPs, local schemas, settings and versioned recovery/overwrite decisions.

Current availability: No representative PC backup supplied.

### FIX-015 Malformed/unsupported

- [ ] Bad UUID/type/version/count/CRC/strings, absent siblings and interrupted saves.

Current availability: Bounds checked in one directory probe only.

### FIX-016 Manufacturing

- [ ] STL/3DM component inclusion, dimensions, units, normals, manifoldness, repair and downstream acceptance.

Current availability: No PC/iPad output comparison.

### FIX-017 UI states/input

- [ ] Wired commands, dialogs, visibility/disabled states, mouse/key/Pencil equivalents and dynamic/third-party gaps.

Current availability: Static inventory only.

### FIX-018 Remote business

- [ ] Allowed/denied quote/export/order states, offline failures and stale pricing.

Current availability: Offline equivalents require explicit decisions.

### FIX-019 Render/printer

- [ ] Material/render and try-on references, native printer output and supported mobile hardware.

Current availability: Replacements and reference outputs unresolved.

### FIX-020 Reliability/device

- [ ] Complex scenes, low storage, crashes, undo/reopen, cancellation, memory and thermal pressure, offline cold start.

Current availability: Requires physical-device implementation.

## Round-trip protocol

1. Hash originals and work on copies.
2. Record PC build, plugins, catalog/gem/font versions, units and tolerances.
3. Capture node/instance IDs, links, values, options, categories, metadata and geometry.
4. PC -> iPad unchanged save -> PC reopen; compare semantic data, with intentional revision/provenance changes recorded.
5. Edit one known parameter on iPad, recompute/save, reopen and recompute on PC.
6. Compare geometry, volume, materials and exports independently; topology representation differences need an explicit contract.
7. Exercise undo/redo and historical branches.
8. Preserve unknown bytes or fail closed before destructive save. A successful preview is not an editable round trip.

Initialization sets 1-degree angle tolerance and Tolerance=0.001; capture units/later settings from actual reference. Do not assume a universal mm or volume threshold.
