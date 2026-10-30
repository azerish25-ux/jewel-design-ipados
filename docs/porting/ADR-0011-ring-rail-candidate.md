# ADR 0011: source-derived ring rail candidate subcomponent

Status: local actual-native verification passed; Apple verification pending.

Features.Candidate is an experimental feature layer above Geometry.Candidate. RingRailV1Candidate preserves the inspected registration GUID, default radius7, ZX plane and supplied Radius metadata, and owns an actual native four-span rational NURBS edge. Native failures propagate; wrapper-allocation failure releases the created shape. This code implements only the inspected geometry/input plan, not the entire recovered CSBlock/CSRingRailData lifecycle.

The test supplies the stored radius from the hash-locked library explicitly, using the independently verified rail4->source2/channel0 reference. RingSize1.2 is already radiusMM. It does not assume a stored scalar equals an evaluated channel output in every activation/override/CAM situation. Existing file-state handles edit radius +.5, export/reopen and feed a newly generated real curve; source curve and byte-identical undo are checked. This is genuine subgeometry generation, not full jewelry design regeneration or PC application save.

The original reader, UI defaults/error flags, channel conversion/fallback, curve user strings, copy/export/material behavior, complete graph/surface/solid generation, downstream parameterization and original-PC/physical comparisons remain open. SDK backend/distribution selection is unchanged.
