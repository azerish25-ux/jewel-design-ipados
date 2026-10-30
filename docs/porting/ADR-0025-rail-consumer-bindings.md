# ADR0025: exact rail consumer headers and record-bound builders

Status: Windows document/native/reference assertions pass; Apple verification pending.

Recovered sealed CSRingRail1 and CSOutsideRail1 do not override CSBlock.Read/Write. Their exact1.0 records contain the common object/block header, user data and object/channel arrays only. LegacyCsg4SimpleBlock decodes these two registered GUIDs, preserves every serialized input slot and structurally writes the known body. It does not assume arbitrary derived block payloads have no tail. Unknown GUID/version stays opaque and malformed known counts/trailing data fail.

The shared decoded header projection now includes records4/13, yielding108 known headers,100 scalar edit handles and133 opaque records in the fixture. This is header/data availability, not108 executed builders. SimpleBlock has no invented scalar value. Existing input slots can be rewired through WithInputLink without extending/removing sockets; scalar-specific API stays restricted. Immutable lists and raw text/flag/userdata preservation remain part of the boundary.

RingRailV1Candidate.BuildFromNode and OutsideRailV1Candidate.BuildFromNode validate the record type/version/header, obtain source-ordered links, generate the supported upstream value/profile slice and call the actual native geometry algorithms. Missing serialized slots bind as disconnected according to constructor defaults; additional serialized slots remain stored but are not assigned new consumer semantics. Ring radius defaults7; OutsideRail inputs keep the recovered numeric/curve/default conversion rules.

The manual-binding overload remains available, but the fixture path no longer needs a hardcoded binding array. Tests use actual saved record4/13 links, rewired top source -> side value, save/reopen/rebuild/reversal, disconnected/default radius/top and nonnumeric top fallback. All resulting geometry is native; temporary profile/curve ownership returns to zero.

Document tests compare header fields/links against independent ring/outside readers, exact structural no-op, existing-slot rewire/reopen/reversal, future versions, malformed count and immutable/non-scalar guards. Earlier155-curve/4012-control,28160 source differential/value-graph and native generation tests remain passing.

This does not provide generic CSBlock decoding, full graph scheduling/dirty/output caching, geometry output producers in the scalar session, complete activation/error/UI/material/output-data lifecycle or original-PC/physical/application parity. Consumer output integration and dependent builders are next; no complete feature row closes here.

[Work card](plans/P04-T01-A04-rail-bindings.md), [P04 evidence](evidence/P04-T01.json), [ring source](../../recovery/canonical/cad/CSBlocks.Rails.Obsolete/CSRingRail1.cs), [outside source](../../recovery/canonical/cad/CSBlocks.Rails.Obsolete/CSOutsideRail1.cs).
