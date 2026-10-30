# Portable graph snapshot

Immutable data capture preserves stored table indices separately from collection order, type/instance identity text, record versions, flags, duplicate labels, raw channel pairs and original record bytes. CSBlock serialization uses (-1,-5) for disconnected links, unlike the runtime channel default -1. No link resolution, codec, graph evaluator or CAD result is implemented. Original bytes are provenance; never reuse them as regenerated save output after editing structured fields. P04 owns reconciliation, CRC and version validation.
