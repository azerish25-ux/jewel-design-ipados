# ADR 0008: immutable portfolio member inventory

Status: accepted for P04-T03-A02.

LegacyPortfolioPackage is an in-memory folder-member inventory, not a new PC archive format or filesystem importer. It owns immutable member bytes and supplied timestamps, normalizes relative separators and rejects traversal/absolute/drive paths, case collisions and configured member/aggregate excess. It never follows CSP Windows paths or evaluates opaque files. Unknown files and nested members remain present.

The explicit root CSP manifest is parsed with the existing XML codec. Top-level paired CSG filenames define physical design candidates; XML basename matches and GUID checks provide diagnostics, not authoritative native reconciliation. Missing, ambiguous, invalid and contradictory metadata remain reported while CSP bytes are preserved. No metadata entry is silently dropped, no paths rewritten and no design generated. History candidates preserve inventory order and supplied timestamps; native filesystem date ordering remains unimplemented. Preview names locate supplied members only; bytes are opaque, not claimed valid images.

OpenDesign returns a scalar archive editor bound to this package snapshot. WithDesignArchiveData accepts only that owner and replaces only its selected member bytes; stale/foreign edits are rejected. WithProjectMetadata requires the edited XML to originate from the current manifest bytes, preventing accidental foreign/stale overwrite. All other member bytes and names remain unchanged. Replacement retains supplied timestamp metadata because this layer does not invent OS modification times. It is archive-data export, not updated previews/sidecars or a regenerated application save.

Recovered Project.PopulateProjectDesignsCollection performs additional relative-folder matching, path relocation, removal of missing entries and SaveProjectData; SaveAs/clone also changes customer and identity state. This inventory intentionally does not claim those application behaviors. Explicit import/reconciliation policies and real PC fixtures are still needed before parity.

Tests combine synthetic CSP/history inventory with copies of the real decoded library CSG and deliberately opaque sidecar bytes. They prove all-member retention, immutable input/output, candidate/history discovery, actual scalar CSG edit/reopen, metadata-only replacement and ownership/case/path/count/size/diagnostic failures. They do not prove native PC folder loading, authentic history ordering or preview validity.
