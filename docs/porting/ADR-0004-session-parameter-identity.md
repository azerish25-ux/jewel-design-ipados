# ADR 0004: registration identity and session parameter handles

Status: accepted for P03-T03-A05.

InstanceIdentity.ResolveStored adapts the CSObject.Read/ReadJson GUID rule: parse with Guid.TryParse; generate with Guid.NewGuid on failure. StoredText retains exactly the supplied decoded string including casing/whitespace, null, empty or malformed values. Valid Guid.Empty and duplicate GUIDs remain valid. The codec must supply only text actually decoded for the correct record version; this helper does not read an archive or infer whether a string field exists. Original Write/WriteJson emits the effective InstanceGuid, so raw-text preservation is not a decision about save-time migration or no-op byte identity.

GraphIdentityScope creates a distinct GraphNodeIdentity reference for each captured slot. Its InitialIdentity resolves once at registration and remains provenance; it is not the entire mutable CSObject state. GraphParameterHandle combines that reference and an exact ordinal field key. No label, table index, type GUID or instance GUID equality can collapse separate records.

GraphIdentityView binds an explicit host-supplied identity ordering to an immutable GraphSnapshot. It copies the mapping and rejects missing, duplicate, null or foreign identities. Reorder/rename views keep addresses stable through that mapping. Removal makes resolution fail; explicit restoration with the same identity makes the old address resolvable again. Registering a replacement creates a new reference even with identical metadata/GUIDs, so removed handles cannot retarget accidentally. Separate scopes never compare addresses equal.

The host is responsible for semantic correspondence between identities and node payloads. No heuristic matching or automatic type/GUID migration occurs. A view may contain changed GUID metadata while InitialIdentity remains registration provenance; future graph commands must own live mutable instance state. The mapping does not apply rename/reorder/delete commands, remap graph links, validate field existence/editability, execute undo or write bytes. Original record bytes remain opaque provenance, not automatically regenerated output.

This augments ADR 0001 single-capture GraphParameterAddress; it does not change that API or promise identity across file loads. P04 needs actual legacy/current codec fixtures and explicit raw/effective persistence decisions. General units, graph editing, native target adapters and PC save/reopen remain incomplete.

Shared SessionIdentityAssertions covers valid GUID formats, zero/duplicate/legacy identity, captured-object slot duplication, explicit rename/reorder/delete/restore/replacement/GUID-metadata mappings, immutable mapping isolation, exact field names and null/invalid/foreign ownership cases. Native calls, codecs and original PC execution are absent.

[ADR 0006](ADR-0006-legacy-scalar-edit-bridge.md) extends opaque metadata representation: InitialIdentity can be null for an undecoded identity field, which is distinct from a decoded absent legacy GUID.
