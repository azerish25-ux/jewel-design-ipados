# ADR0026: graph-managed known rail outputs

Status: Windows native/document/source-oracle regressions pass; Apple validation pending. This is a known value/profile/rail dependency slice, not the complete original graph scheduler.

ILegacyValueNodeProvider extends explicit value generations without a native dependency in Documents. Supported external producers declare input references, update in dependency order and return pure channel data. LegacyCurveSelection identifies a producer output; no native pointer/handle enters the document model. Unknown producers remain unsupported, and cycle/depth/preflight guards remain active.

CandidateRailGraphSession owns native RingRail_V1/OutsideRail_V1 outputs. It generates upstream scalar/profile/rail dependencies once without recursive generation, uses exact node bindings and resolves profile or already-generated curve references for downstream inputs. Both a ring->radius-curve fallback and ring->outside-curve edge execute actual native operations. The ring curve is not implicitly numeric: recovered CSRailData implements curve/transform data, not ICSDouble. Outside curve-data Scale is1, distinct from numeric output channel1=0. Invalid rail output indices return empty data; valid outputs require explicit generation.

Native outputs are held by the session; RetainCurve returns an independent owned reference. A regeneration builds temporary outputs and commits a new native cache only when the supported generation completes. Unrelated cached outputs remain; this is not dirty propagation. Old outputs are released after commit, retained readers remain usable, and failed/cyclic generation releases pending entries while preserving prior complete native cache. Scalar states can already have changed before a later native failure. This explicit candidate failure/commit policy is not an original CS graph equivalence claim.

Source data metadata and OutsideRail diagnostics remain accessible without fabricating new scalar conversions. Native temporary input references are retained/disposed safely. Disposal releases all cached outputs; externally retained curves survive until their owners release them.

Actual tests cover original saved project nodes, rewired rail->curve/radius edges, generated values, scalar export/save/reopen/rebuild, curve vs numeric/data-scale outputs, invalid channels before/after generation, replacing outputs with retained old readers, failed ring generation and cycle cleanup. Native entry count reaches zero after final release. Existing28160 source field/output cases,155 curve imports, project codecs and previous native assertions remain passing.

Still required: complete scheduler/dirty/cache/parallel/error policy, generic producer/data kinds, dependent builders and output-rail property/material/export lifecycle, original-PC/physical/rendering/performance/cancellation/process-memory acceptance. No complete feature row closes here.

[Work card](plans/P03-T03-A07-rail-outputs.md), [P03 evidence](evidence/P03-T03.json), [source rail data](../../recovery/canonical/cad/CSDatas/CSRailData.cs), [ring metadata](../../recovery/canonical/cad/CSDatas/CSRingRailData.cs), [outside metadata](../../recovery/canonical/cad/CSDatas/CSOutsideRingRailData.cs).
