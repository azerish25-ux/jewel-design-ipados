# ADR0038: typed owned BRep graph outputs

Status: local candidate/source/native checks pass; new Apple verification pending. Full source/PC/physical parity remains open.

LegacyChannelValue now has a distinct immutable BRep selection. One CandidateGeometryState owns native curves or BReps; typed access rejects the other kind. Capturing, nested array relay, transform copying, retained old generations and disposal share the existing implementation. Bare tokens still address the current producer cache; explicit snapshots retain their captured generation. BRep numeric/Boolean conversion fails without changing fallback values.

The exact1.0 sweep adapter now runs through graph.Generate(134). It reads already-generated inputs in the current transaction, avoiding reentrant generation, and publishes the actual candidate face as BRep data with source Identifier_0 naming. It currently returns one candidate result. Closed, single-profile and no-parameter paths fail explicitly. This does not establish original Rhino result splitting, face identities or complete block scheduling/material policy.

CSOnGeometryData.GetCurve/GetNurbsCurve cast only Curve; GetBrep checks HasBrepForm and duplicates BRep or converts Surface. GetTransform reads cumulative state. ApplyTransform changes physical geometry and postmultiplies stored metadata. Two noncommuting native transformations verify that metadata order separately from the physical point result.

CSScaleObjectToSize calls the old data.GetTightBoundingBox. For BRep, the recovered old overload explicitly uses doAccurate:false; New uses true. Native fast/accurate shape bounds are explicit policies with no triangulation dependence. Scale selects Fast for BRep and arrays union represented geometry bounds. Analytical box dimensions and rotated rational-circle boxes distinguish the modes. Native box numbers/margins are not claimed Rhino-equivalent. [Canonical contract](../evidence/audit/brep-data-source-contracts.json) includes exact method/caller hashes, guarded in CI with tamper controls.

A saved negative all-output link is not necessarily an operation input: exact load replay can treat it as an unlinked final spare. Tests preserve this quirk instead of silently fixing it. The wildcard body is separately executed from exact source using current real BRep/curve outputs, then captured with genuine native ownership.

Tests generate actual134, verify its type/name, reject curve access, edit radius, structurally save/reopen/regenerate, retain the old face and preserve the previous cache on unsupported closed generation. Nested arrays/unarray, trailing empty data, wildcard source execution, affine copy/metadata order, BRep scale, snapshot survival and zero-entry disposal pass. Adjacent native/document/source suites pass. [Evidence](evidence/P03-T03-A14.json) records scope and exact reports.

Remaining: full source data user strings/materials/display/mesh/export, raw4x4/Unset/collapsed transform behavior, other producers/consumers, closed/no-param/single-profile and native output topology/cardinality, complete scheduler and original-PC/physical acceptance. No full parity row closes. Earlier paired-surface Apple results are [inspected separately](../evidence/apple/paired-surface-results.json); they predate this graph integration.
