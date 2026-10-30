# P04-T01-A04 typed legacy rail consumer bindings

Goal: decode RingRail_V1 and OutsideRail_V1 exact1.0 common block records, structurally round-trip/rewire saved links, and drive their native builders from record bindings rather than a fixture-specific array.

Sources: sealed legacy rail classes have no Read/Write overrides; CSBlock/CSObject encode header/user data/object/channel arrays. Preserve all serialized slots and negative indices. Source runtime constructors define1/6 consumer inputs; missing serialized slots are disconnected at binding, extra serialized slots remain stored but outside the supported consumer template.

- [x] RED SimpleBlock/BuildFromNode APIs missing.
- [x] Add immutable exact known block header codec and writer, common header projection, public existing-slot rewiring without scalar field invention. Unknown GUID/version remains opaque.
- [x] Compare fixture record4/13 header/links to independent readers; exact no-op, rewire/reopen/reversal, unknown/version/count/immutability guards; no new scalar handles.
- [x] Native RingRail/OutsideRail BuildFromNode reads header links, generates the known upstream slice and honors disconnected/defaults. Test real project links, alternate radius/thickness wiring, save/reopen/rebuild and ownership.
- [x] Run document/native/source/reference/provenance checks, update decoded inventory and ADR0025 with scope limits.
- [x] Push/dispatch Apple simulator/AOT tests, preserving existing runs. Runs41086518672/41086522718 ate90440a confirmed live/queued.
- [x] Inspect actual binding/native/family/AOT artifacts before bounded verification. Native iPad/iPhone and arm64/platform IOS unsigned Release52 warnings0 errors inspected in rail-bindings-results.json; no PC/physical parity.

Limits: not a generic decoder of arbitrary CSBlock derived payloads; no full graph scheduling/geometry output caching/dirty propagation, activation/error/UI/material/output-data lifecycle or original-PC/physical parity. Existing scalar/profile engine rejects unsupported producers. Existing link API edits stored slots only; adding/removing sockets needs source-specific behavior later.
