# P03-T03-A05 runtime and session parameter identity

Goal: resolve stored instance GUIDs using recovered CSObject read rules while preserving raw text, and keep distinct graph-node/parameter handles stable across explicitly mapped immutable views in one editing session.

Architecture: Domain InstanceIdentity stores raw/effective identity and generated status. GraphIdentityScope creates one reference identity per captured node slot. GraphIdentityView binds explicit identity order to a snapshot, validating ownership/count/uniqueness. GraphParameterHandle binds node reference identity plus ordinal field key, independent of labels/table indices/GUID equality. No automatic cross-file matching, codec write or graph/link regeneration.

Source: CSObject.Read/ReadJson use Guid.TryParse and Guid.NewGuid on failure; valid zero and duplicate GUIDs are not rejected. CSObject.Write/WriteJson writes effective InstanceGuid. Session handles are new infrastructure, not an original serialized identifier. Source graph links remain in their captured index space.

Prerequisites: P03-T01/T02 and P03-T03-A01/A02/A04 are verified; undo scheduler now uses reference targets. No native runtime required. General units, persistent cross-load/codec policy and live graph commands remain unfinished.

Owned files: src/Port/Domain/InstanceIdentity.cs; src/Port/Graph/GraphIdentityScope.cs; tests/Port/Graph.Contracts/SessionIdentityAssertions.cs; shared Graph.Contracts runner and phase/evidence/classification/ADR.

- [ ] Write failing normal/boundary/failure assertions first: valid GUID formats, null/invalid/empty strings, valid zero, duplicates, two equal-looking slots, mapped rename/reorder, field-key case, deletion, foreign session, duplicate mapping, count mismatch and null inputs.
- [ ] Implement immutable runtime identity plus scope/view/handle boundaries. IDs resolve once per registered record; unknown metadata/bytes stay in snapshots. Explicit mapping never repairs or remaps serialized links.
- [ ] Run Graph.Tests, Domain.Tests, combined Console and integrity/provenance checks; review ownership, payload isolation and no heuristic matching.
- [ ] Commit/push; use GitHub Actions for macOS iPad/iPhone execution and inspect both artifact results.
- [ ] Commit/push evidence, retaining parent blockers and no PC save/reopen claim.
