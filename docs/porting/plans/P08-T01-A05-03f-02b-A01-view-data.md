# P08-T01-A05-03f-02b-A01 owned current-view data

Implement the value model required by whole-view ownership: all mapped original5 viewport and metadata values, clipping infos, owned serialized userdata and unknown chunks. This captures current converted view data; it is NOT ON_Object.CopyUserData or native history cloning. Zero-copy-count and unsupported runtime class payloads remain retained in the current view. Native clone policy/history integration stays open.

RED tests cover every mapped metadata sector, raw IEEE values and stale target mirror, UUID/unknown enum/constraint values, clipping and byte-array alias isolation, input/output chunk order, inert Windows image/printer paths, and immutable updates preserving unrelated state. No native constructor/default, archive codec or plugin execution is synthesized.

GREEN model uses readonly values, immutable matrices/strings and private owned arrays. No native pointer blobs or normalizing on copy. Run focused/full native/adjacent/integrity checks, macOS and actual iPad/iPhone. All local files/caches stay D-only through run-local; retry transient D errors after five seconds. Commit/push exact results without closing full parity rows.
