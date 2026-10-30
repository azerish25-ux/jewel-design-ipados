# P04-T03-A02 immutable portfolio member inventory

Goal: assemble CSP metadata, paired CSG files, previews/history candidates and opaque members without losing bytes or following stored Windows paths. The package is an in-memory folder inventory, not a new CounterSketch archive format.

Source: Project.PopulateProjectDesignsCollection enumerates top-level CSGs, derives raw paired filenames, then reconciles/drops entries and saves metadata; Design derives preview/history references; ProjectManagementHelper.SaveAs copies files and rewrites paths/customer fields. This primitive must not silently apply those native load/save mutations without a PC oracle.

Prerequisites: CSP/source-shape codec, real library CSG codec and scalar editor proofs. Tests combine synthetic CSP inventory with the existing real library bytes; no real saved PC portfolio/history is asserted.

Architecture: immutable member byte ownership plus normalized relative names/Windows case-collision rejection and configurable aggregate limits. Chosen manifest parsed explicitly; all other members retained. Bindings are filename candidates, with metadata GUID checks and missing/ambiguous diagnostics. History candidates retain supplied timestamps, not invented filesystem ordering. Opaque files/sidecars are never decoded as previews or regenerated.

Owned: src/Port/Documents/LegacyPortfolioPackage.cs; shared package assertions; evidence/ADR/classification/CI.

- [ ] RED: open inventory, clone isolation, 2 paired files/history candidate, unchanged sidecars/unknown members, handle-edit commit/reopen, metadata-only replacement, foreign/stale edit guards, case/path/aggregate bounds and missing/ambiguous/GUID diagnostics.
- [ ] GREEN immutable inventory and owner-bound archive edit handle; replacement changes only explicitly selected bytes. No directory IO, automatic CSP path migration, model regeneration, preview update or file deletion.
- [ ] Run shared/local/independent/source checks and inspect latest pending XML Mac run; commit/push.
- [ ] Inspect Mac iPad/iPhone package assertions, then commit/push scoped evidence. Full P04-T03 remains incomplete for real corpus, native reconciliation, history ordering, sidecar semantics and PC reopening.
