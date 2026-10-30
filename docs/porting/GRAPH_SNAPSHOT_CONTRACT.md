# Graph snapshot interpretation

The model captures data; it does not evaluate, validate an archive schema or regenerate serialized output.

- CSObject has a mutable runtime TableIndex, initially -1. CSParaGraph.SetIndexesOfGraphItems assigns it in the current selected/active indexing scope. Snapshot StoredTableIndex captures that mapping, not an independently encoded CSObject field, and need not match the snapshot list offset. Links refer to that indexing scope.
- CSBlock.Write/WriteJson serialize an unconnected input as object index -1 and channel index -5. CSInputChannel runtime initialization uses channel index -1. Preserve both raw pairs rather than normalizing them.
- Type GUID is distinct from instance identity. CSObject.Read/ReadJson can generate a runtime GUID when stored identity is absent/invalid. Snapshot StoredInstanceGuid preserves raw text; P04 must explicitly reconcile original bytes and effective runtime identity. The DTO does not choose migration policy.
- Unknown versions/GUIDs/flag bits and duplicate display labels are retained. That is preservation, not a claim those records can execute.
- Original record bytes are opaque provenance. A writer must reconcile known-field edits, unknown regions, version policy and CRC before saving; copying original bytes after an edit would produce stale data.
- Units, parameter-address semantics, graph scheduling and CAD evaluation remain incomplete work.
