# ADR 0001: snapshot-scoped parameter addresses

Status: accepted for P03-T03-A02; cross-load identity remains open.

Recovered CSObject has separate display identifier, type GUID, runtime instance GUID and mutable TableIndex. CSObject.Read/ReadJson creates a runtime GUID when the stored value is absent or invalid. CSParaGraph.SetIndexesOfGraphItems assigns table indices for a selected/active scope. None of these alone establishes a permanent parameter identifier for all legacy records.

GraphSnapshot.AddressParameter therefore binds an exact, ordinal field key and node ordinal to one immutable snapshot object. GraphParameterAddress equality includes the snapshot object identity. A different capture cannot resolve the address, even if its nodes are identical. Duplicate labels, stored GUIDs, table indices or node object references cannot collapse separate slots. Keys are not trimmed or case folded. Blank keys and out-of-range slots are rejected.

The key is a serialized/schema field name such as CSValue.WriteJson's Value, never a display label. Creation validates address structure only: it does not establish field existence, editability or a schema mapping. ResolveNode resolves the node slot, not a decoded value. Unknown fields remain available for future codec interpretation.

This is new infrastructure derived from recovered semantics, not recovered algorithm reuse. It prevents accidental stale edits; addresses cannot be serialized, stored as project identity, or carried across graph replacement/reordering. P04 must reconcile raw/effective instance identities and unknown fields. Undo/document identity will need an explicit mapping when mutable graph editing is implemented. Do not guess that mapping from labels or overwrite stored GUIDs.

Verification: tests/Port/Graph.Contracts/ParameterAddressAssertions.cs is invoked by Graph.Tests, Console smoke, and the iPad/iPhone simulator harness. Cases include repeat lookup/hash-set equality, duplicate slots, ordinal keys, foreign captures, empty captures, invalid indices and null/blank arguments. No PC reopen, geometry regeneration or parameter-write claim is made.
