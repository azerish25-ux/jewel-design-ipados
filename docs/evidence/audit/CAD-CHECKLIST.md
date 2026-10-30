# CounterSketch CAD capability checklist

52 grouped capability requirements. The 619 builder declarations are a separate type catalog and acceptance crosswalk, not 619 independently observed user features.

Every checkbox is an acceptance requirement, not a passed implementation test. Static evidence is distinct from runtime behavior.

## CAD-001 Declared block catalog and old algorithm dispatch

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RegisterCSBlocks scans nonabstract CSObject classes, reads noninherited CSBlockAttribute, rejects duplicate GUIDs within that scan and adds CSBlockDescription. GetNew invokes a string constructor. The 619 CSBlocks attributes are declared node identities, not user features or proof of successful runtime registration. Obsolete namespaces remain deliberately discoverable; warnings do not remove them.

**Offline disposition:** port-managed-registry

**Acceptance:**

- Build registry for every declared GUID, assert a constructible identity and unique GUID; compare against PC registry dump.
- Load a fixture per legacy GUID and preserve its versioned algorithm rather than silently substituting the latest class.

**Dependencies:** .NET reflection; CSObject hierarchy; GUID registry

**Evidence:**

- [CSApplication.cs:415](<../../../recovery/canonical/cad/CS/CSApplication.cs#L415>) — CSApplication.RegisterCSBlocks
- [CSBlockDescription.cs:25](<../../../recovery/canonical/cad/CS.Parametric/CSBlockDescription.cs#L25>) — CSBlockDescription.GetNew

**Unresolved:** No application executed; externally loaded assemblies and constructor failures remain unverified.

## CAD-002 Unsupported-node fallback is an error node

- [ ] Implement and verify the acceptance criteria below.

**Observed:** When graph lookup cannot find a GUID it selects the all-zero GUID. CSNullBlock is registered at that GUID and Update returns Block No Longer Supported. This supplies a graph placeholder with an explicit error, not a working geometry substitute.

**Offline disposition:** must-preserve-or-reject-explicitly

**Acceptance:**

- Load a PC file containing an unknown GUID; show a blocking compatibility error while retaining original opaque node payload.
- Verify that a supported legacy GUID never enters this fallback.

**Dependencies:** Graph archive GUID dispatch; CSNullBlock

**Evidence:**

- [CSParaGraph.cs:521](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L521>) — CSParaGraph.ReadGraphItems
- [CSNullBlock.cs:23](<../../../recovery/canonical/cad/CSBlocks/CSNullBlock.cs#L23>) — CSNullBlock.Update

**Unresolved:** Unknown archive payload retention is covered by the primary serialization audit.

## CAD-003 Numeric values, overrides and manufacturing CAM compensation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSValue Update accepts a wired override. Binary versions use major 5 for CAM and 3 otherwise; the writer stores absolute-or-delta CAM value, mode and ASCII note. This means output geometry depends on manufacturing mode and stored compensation, not just the visible number.

**Offline disposition:** port-managed-core

**Acceptance:**

- Open fixtures with absolute CAM, delta CAM and no CAM; compare displayed and generated dimensions in design and CAM modes.
- Change wired override and undo; verify prior value, change propagation and CAM note persist.

**Dependencies:** CSValueBase; CAM mode; channel override semantics

**Evidence:**

- [CSValue.cs:277](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L277>) — CSValue.MajorMinorVersion
- [CSValue.cs:371](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L371>) — CSValue.Update
- [CSValue.cs:400](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L400>) — CSValue.Write

**Unresolved:** Archive version interpretation is delegated to the primary serialization audit.

## CAD-004 Stored C# scripts with typed graph inputs and ref outputs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSScript reads and writes source text and typed input/output descriptors in binary and JSON. Update compiles a generated ScriptClass.Main using CSharpCodeProvider CompilerVersion v4.0, GenerateInMemory=true, IncludeDebugInformation=true and System.Core.dll. Generated source imports System.IO and other .NET namespaces; Main receives bool camMode and ref outputs. Inputs are rounded/coerced, reflection Invoke executes Main, outputs are converted to CSData; compile/runtime errors become graph errors. No fixed .cs filename is present in this class.

**Offline disposition:** major-platform-compatibility-blocker

**Acceptance:**

- Load one PC fixture for all 10 ScriptVarType variants, multiple ref outputs, camMode true/false, exceptions and compiler diagnostics; compare rounding and output data.
- Execute the same stored script corpus on the intended offline iPad runtime or a defined equivalent interpreter; require explicit incompatibility diagnostics for unavailable APIs.
- Round-trip source text and I/O descriptors without recompiling or losing code; include scripts using System.IO and object/list values in the compatibility corpus.

**Dependencies:** Microsoft.CSharp / CodeDom .NET 4 compiler; dynamic assembly creation; reflection; System.Core.dll and .NET standard library; ScriptVarType primitive/object/list conversions

**Evidence:**

- [CSScript.cs:90](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L90>) — CSScript.Read
- [CSScript.cs:158](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L158>) — CSScript.ReadJson
- [CSScript.cs:652](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L652>) — CSScript.Write
- [CSScript.cs:698](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L698>) — CSScript.WriteJson
- [CSScript.cs:749](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L749>) — CSScript.BuildAndCompileScript
- [CSScript.cs:756](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L756>) — CSScript.BuildAndCompileScript
- [CSScript.cs:795](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L795>) — CSScript.BuildAndCompileScript
- [CSScript.cs:837](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L837>) — CSScript.BuildAndCompileScript
- [CSScript.cs:858](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L858>) — CSScript.BuildAndCompileScript
- [CSScript.cs:870](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L870>) — CSScript.BuildAndCompileScript
- [CSScript.cs:500](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L500>) — CSScript.Update
- [CSScript.cs:523](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L523>) — CSScript.Update

**Unresolved:** No iOS compiler/interpreter is present or validated. Supporting only numeric expressions cannot guarantee all existing scripts. No target script was executed.

## CAD-005 Freehand object manipulation and common-center mode

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSValueXFormObject applies a shared xTrans by premultiplication when common-center mode is enabled; otherwise switch branches rotate around BasePoint, translate in signed object axes or scale matrix basis columns. Thus group/common-center and per-object transforms have distinct semantics.

**Offline disposition:** replace-touch-ui-preserve-transform-core

**Acceptance:**

- Translate/rotate/nonuniformly scale the same asymmetric PC object in local and common-center modes; compare all 16 transform coefficients and resulting bounds.
- Select two objects, manipulate around common center, save/reopen and undo/redo; verify independent origins and orientation.

**Dependencies:** Rhino viewport picking/drawing; mouse/handle interaction; transform math; CSValueXFormBase

**Evidence:**

- [CSValueXFormObject.cs:3003](<../../../recovery/canonical/cad/CS.CSValues.CSValueHandle/CSValueXFormObject.cs#L3003>) — CSValueXFormObject transform application
- [CSValueXFormObject.cs:2486](<../../../recovery/canonical/cad/CS.CSValues.CSValueHandle/CSValueXFormObject.cs#L2486>) — CSValueXFormObject transform application
- [CSValueXFormObject1.cs:648](<../../../recovery/canonical/cad/CS.CSValues.CSValueHandle.Obsolete/CSValueXFormObject1.cs#L648>) — CS.CSValues.CSValueHandle.Obsolete.CSValueXFormObject1.Update

**Unresolved:** Touch hit areas, multitouch/pencil gestures and viewport adaptation have not been implemented. Old CSValueXFormObject1/2 remain separate identities.

## CAD-006 Ring rail and ring size geometry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSRingRail obtains a radius, creates a ring rail and emits CSRingRailData carrying that radius. CSRingRailCustom requires radius, curve, span and blend; caps span at 340 degrees, enforces blend >=10 then total <=350, and falls back to a circle if span or width is zero.

**Offline disposition:** local-kernel-and-tables-required

**Acceptance:**

- Generate standard rail at radius 8.667196228921199 and compare circumference, plane and stored radius to PC.
- Use custom rail with span 400, blend 5, width zero and nonzero; compare clamps, circle fallback and seam orientation.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; ring-size conversion tables

**Evidence:**

- [CSRingRail.cs:25](<../../../recovery/canonical/cad/CSBlocks.Rails/CSRingRail.cs#L25>) — CSRingRail.Update
- [CSRingRailCustom.cs:69](<../../../recovery/canonical/cad/CSBlocks.Rails/CSRingRailCustom.cs#L69>) — CSRingRailCustom.Update
- [CSOutsideRail1.cs:81](<../../../recovery/canonical/cad/CSBlocks.Rails.Obsolete/CSOutsideRail1.cs#L81>) — CSBlocks.Rails.Obsolete.CSOutsideRail1.Update

**Unresolved:** Bend/bypass/flex/open/tube/outside/secondary rail variants are inventoried separately; each needs its own fixture.

## CAD-007 Gem size selection and unconstrained resizing

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSResizeGem switches between unconstrained, full gem-table and iteration-table scaling, supports largest-possible choice, duplicates each gem before transforming and emits scalar for one result or array for many. Invalid mode returns an error.

**Offline disposition:** local-kernel-and-tables-required

**Acceptance:**

- Resize round and elongated gems to a noncatalog target using all three modes and largest-possible on/off; compare selected dimensions and transforms.
- Resize an array of gems and confirm original inputs unchanged and scalar/array output shape agrees with PC.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; gem shape and size tables; iteration exclusions

**Evidence:**

- [CSResizeGem.cs:55](<../../../recovery/canonical/cad/CSBlocks.Gems/CSResizeGem.cs#L55>) — CSResizeGem.Update

**Unresolved:**

## CAD-008 Gem rows on curves with count, taper, placement and orientation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGemOnCrvCount reads start/middle/end gem definitions and placement, count, roll/tilt, orientation and flip/mirror/reverse switches. It derives intermediate sizes when toggles are disabled, falls back to Y-axis orientation when no surface exists, preprocesses curve data and computes rotated test-gem radii.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Place 7 tapered gems on a noncircular curve with end/middle toggles, roll and tilt; compare gem order, count, sizes and transforms.
- Remove the surface input while surface orientation is selected and verify the PC fallback; test reverse plus mirror separately.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSUtilGemOnCrv; size-table/custom-size state

**Evidence:**

- [CSGemOnCrvCount.cs:110](<../../../recovery/canonical/cad/CSBlocks.Gems/CSGemOnCrvCount.cs#L110>) — CSGemOnCrvCount.Update
- [CSGemOnCrvCount.cs:174](<../../../recovery/canonical/cad/CSBlocks.Gems/CSGemOnCrvCount.cs#L174>) — CSGemOnCrvCount.Update

**Unresolved:** Full placement-search loops and every obsolete row algorithm require fixture validation.

## CAD-009 Custom tapered baguette and emerald gem definitions

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCustomBaguette constructs a CSGemInstanceDefinitionCustomBaguetteTapered directly from length, bottom/top width, thickness and girdle thickness and wraps it in CSGemData. Custom gem shape instances are editable geometry, not only catalog lookup.

**Offline disposition:** local-procedural-gem-definitions-required

**Acceptance:**

- Create a baguette with unequal top/bottom widths and verify girdle outline, depth, facet mesh and export geometry.
- Save/reopen a PC custom baguette fixture and compare dimensions, weight/size metadata and gem shape identity.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSGemInstanceDefinition custom shape code

**Evidence:**

- [CSCustomBaguette.cs:43](<../../../recovery/canonical/cad/CSBlocks.Gems/CSCustomBaguette.cs#L43>) — CSCustomBaguette.Update
- [CSCustomBaguette1.cs:45](<../../../recovery/canonical/cad/CSBlocks.Gems.Obsolete/CSCustomBaguette1.cs#L45>) — CSBlocks.Gems.Obsolete.CSCustomBaguette1.Update

**Unresolved:** CustomEmerald is inventoried but its complete body was not separately audited here.

## CAD-010 Gem duplicate filtering and collision interpretation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGemRemoveIntersecting compares center distances against InTolerance for each previous gem. It does not test facet or solid intersection; matching the name with a geometric boolean would change behavior. A flagged gem is omitted from output; null inputs may be appended.

**Offline disposition:** port-managed-core

**Acceptance:**

- Use two large overlapping gems with center separation above tolerance and verify both remain.
- Use two tiny nonoverlapping gems with center separation below tolerance and verify the later item is filtered; test equality at threshold.

**Dependencies:** CSGemData center positions; array order

**Evidence:**

- [CSGemRemoveIntersecting.cs:51](<../../../recovery/canonical/cad/CSBlocks.Gems/CSGemRemoveIntersecting.cs#L51>) — CSGemRemoveIntersecting.Update

**Unresolved:** Other CSRemoveIntersectionGems implementations are distinct and require their own tests.

## CAD-011 Cluster and halo distribution around center geometry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCluster builds CSClusterParam, accepts gem or general center objects, chooses a discrete gem size matching available channel width unless explicit size override is enabled, and supports minimum-culet grow-up or grow-smaller modes. It builds local placement transforms from rotation, angle, spacing and Z placement.

**Offline disposition:** local-kernel-and-tables-required

**Acceptance:**

- Build round, cushion, emerald, heart, marquise and pear halos with equal/unequal spacing and explicit gem size override; compare gem count/order and channel rails.
- Use a gem deeper than available profile height; test grow-up and grow-smaller and compare out min-culet offsets/flags.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSClusterGemShapes; gem tables; min-culet constraints

**Evidence:**

- [CSCluster.cs:167](<../../../recovery/canonical/cad/CSBlocks.GemClusters/CSCluster.cs#L167>) — CSCluster.Update
- [CSCluster.cs:200](<../../../recovery/canonical/cad/CSBlocks.GemClusters/CSCluster.cs#L200>) — CSCluster.Update
- [CSCluster.cs:239](<../../../recovery/canonical/cad/CSBlocks.GemClusters/CSCluster.cs#L239>) — CSCluster.Update

**Unresolved:** Per-shape helper bodies and full CSCluster/CSHaloBuilder search loops remain unexecuted.

## CAD-012 Legacy cluster algorithms remain distinct project dependencies

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSClusterRound1 constructs profile sections from center-gem bounds, inside/outside metal amounts, channel depth and tilt, offsets round gem curves and translates sections along gem up direction. It is an independent old implementation, not a GUID alias to CSCluster.

**Offline disposition:** retain-versioned-local-algorithms

**Acceptance:**

- Open round-cluster V1 and current cluster fixtures with corresponding inputs; compare each against its own PC geometry rather than against one another.
- Sweep old inside/outside channel amounts and tilt; compare section curves, winding and metal thickness.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; GemOffsetCurveV1; legacy placement conventions

**Evidence:**

- [CSClusterRound1.cs:129](<../../../recovery/canonical/cad/CSBlocks.GemClusters.Obsolete/CSClusterRound1.cs#L129>) — CSClusterRound1.Update
- [CSClusterRound1.cs:143](<../../../recovery/canonical/cad/CSBlocks.GemClusters.Obsolete/CSClusterRound1.cs#L143>) — CSClusterRound1.Update

**Unresolved:** 257 legacy builder registrations are indexed; representative bodies do not prove every old implementation correct.

## CAD-013 Head builder V2: prongs, V-prongs, bezel, domes, gallery rails and between-prong surfaces

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSHeadBuilder duplicates the gem, applies optional construction transform, derives head girdle/top/base/side/prong rails via CSHeadShape, and branches between bezel profile/revolve and prong construction. Separate helpers implement gallery rails, V-prongs, domes and surfaces. PullToBase derives either a circular rail morph or a target BRep morph, uses tolerance .05 and PreserveStructure=true, and morphs multiple output families.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Generate round and pointed heads with straight/V-prongs, independent base/top profiles, domes, gallery rails and between-prong surface toggle; compare each named output family.
- Build bezel with seat angle/length/height; pull to circle, noncircular curve, surface and BRep and compare geometry bounds and rail continuity.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; ProngParam/RailParam; custom dome/revolve/sweep helpers; CSCompatibility morphs

**Evidence:**

- [CSHeadBuilder.cs:313](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV2/CSHeadBuilder.cs#L313>) — CSHeadBuilder.Update
- [CSHeadBuilder.cs:344](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV2/CSHeadBuilder.cs#L344>) — CSHeadBuilder.Update bezel branch
- [CSHeadBuilder.cs:592](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV2/CSHeadBuilder.cs#L592>) — CSHeadBuilder.PullToBase
- [CSHeadBuilder.cs:679](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV2/CSHeadBuilder.cs#L679>) — CSHeadBuilder.PullToBase

**Unresolved:** Helpers are inventoried; no kernel or licensing availability on iPad has been demonstrated.

## CAD-014 Head builder V1 and older head generation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Current V1 CSHeadProngs validates gems/profiles, supports one fast-multi seed and optional distinct base profile, V-prong options and domes. CSHeadProngs1 separately uses legacy head style positions, profile bounds normalization and rail-revolved dome geometry. Retention requires both algorithm generations plus HeadSetup/Shape/Styles/Rail/Ultimate identities.

**Offline disposition:** retain-versioned-local-algorithms

**Acceptance:**

- Load a fixture for every V1/obsolete registered head GUID; adjust gem shape and layout, preserve old prong locations and body outputs.
- Toggle fast multi and independent base profiles in current V1 and verify duplicated instance transforms and dome sizes.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; HeadStyle tables; versioned profile/style conventions

**Evidence:**

- [CSHeadProngs.cs:109](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV1/CSHeadProngs.cs#L109>) — CSHeadProngs.Update
- [CSHeadProngs1.cs:132](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete/CSHeadProngs1.cs#L132>) — CSHeadProngs1.Update
- [CSHeadProngs1.cs:152](<../../../recovery/canonical/cad/CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete/CSHeadProngs1.cs#L152>) — CSHeadProngs1.Update
- [CSHeadProngPoints1.cs:82](<../../../recovery/canonical/cad/CSBlocks.Tools.Obsolete/CSHeadProngPoints1.cs#L82>) — CSBlocks.Tools.Obsolete.CSHeadProngPoints1.Update

**Unresolved:** Only representative V1 and obsolete prong bodies were inspected in depth, not all 35 obsolete registered head nodes.

## CAD-015 Per-gem bezels and synchronous/parallel generation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSBezel chooses sync or concurrent implementation from application parallel-processing state. Concurrent path supplies a default line profile, extracts seat/fillet/corner/rail parameters and calls MakeBezel per indexed gem, preserving result order. In the recovered concurrent body bottomThickness reads InTopThickness, which must be captured in baseline tests rather than silently corrected.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Run identical gem arrays in sync and concurrent mode and compare order, valid results, cancellation and error outcomes.
- Set distinct top/bottom thickness values in a PC baseline and record actual output for each execution mode; verify seat angle/length/height and rail option.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; parallel cancellation/order semantics

**Evidence:**

- [CSBezel.cs:75](<../../../recovery/canonical/cad/CSBlocks.Settings.Bezels/CSBezel.cs#L75>) — CSBezel.Update
- [CSBezel.cs:109](<../../../recovery/canonical/cad/CSBlocks.Settings.Bezels/CSBezel.cs#L109>) — CSBezel.UpdateConcurrent
- [CSBezel.cs:124](<../../../recovery/canonical/cad/CSBlocks.Settings.Bezels/CSBezel.cs#L124>) — CSBezel.UpdateConcurrent
- [CSBezel1.cs:118](<../../../recovery/canonical/cad/CSBlocks.Settings.Obsolete.Bezels/CSBezel1.cs#L118>) — CSBlocks.Settings.Obsolete.Bezels.CSBezel1.Update

**Unresolved:** Potential recovered-source defect observed; physical output not verified.

## CAD-016 Bar settings and normalized prong profiles

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSBarSet requires sorted gem data, defaults a missing profile to a circle, normalizes its bounding box to unit width/height, reads large/small width/top/bottom adjustments, and derives gem size extremes when small-gem variation is active.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Build alternating small/large gems with custom and default bar profiles; compare bar dimensions, centering, nudge and dome amount.
- Verify output changes when small-gem adjustments become nonzero and confirm handling of empty sorted gems.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSChannelGemData/rail generation

**Evidence:**

- [CSBarSet.cs:74](<../../../recovery/canonical/cad/CSBlocks.Settings/CSBarSet.cs#L74>) — CSBarSet.Update
- [CSBarSet.cs:126](<../../../recovery/canonical/cad/CSBlocks.Settings/CSBarSet.cs#L126>) — CSBarSet.Update
- [CSBarSet1.cs:147](<../../../recovery/canonical/cad/CSBlocks.Settings.Obsolete/CSBarSet1.cs#L147>) — CSBlocks.Settings.Obsolete.CSBarSet1.Update

**Unresolved:** Other standalone/prong-placement/by-pass setting nodes require their per-GUID fixtures.

## CAD-017 Shank builder with gem-dependent channels, side construction and closed loops

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSShankNew normalizes a supplied profile, reads typed UltimateGemData arrays, supports forced/no channel, automatic depth, side/front construction, closed shank/gem loops, per-position wall drops/bottom offsets and endpoint tangencies. BuildChannel emits base/left/right/center/top surfaces and multiple rail/profile outputs using conditional two-rail sweeps.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Build open and closed shanks around tapered gem rows, toggle channel/no-channel/forced-channel and compare all named BRep and profile/rail outputs.
- Vary start/mid/end wall drop and bottom offsets independently, with side/front mode and endpoint tangency; compare section dimensions and continuity.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; UltimateGemData field layout; CSUtilSweepRC; per-gem profile generation

**Evidence:**

- [CSShankNew.cs:246](<../../../recovery/canonical/cad/CSBlocks.Shank/CSShankNew.cs#L246>) — CSShankNew.Update
- [CSShankNew.cs:432](<../../../recovery/canonical/cad/CSBlocks.Shank/CSShankNew.cs#L432>) — CSShankNew.Update
- [CSShankNew.cs:451](<../../../recovery/canonical/cad/CSBlocks.Shank/CSShankNew.cs#L451>) — CSShankNew.BuildChannel
- [CSShankNew.cs:613](<../../../recovery/canonical/cad/CSBlocks.Shank/CSShankNew.cs#L613>) — CSShankNew.BuildChannel

**Unresolved:** CSShankGemNew and old/ShankV1 variants are inventoried; all combinations remain unexecuted.

## CAD-018 Matching bands and versioned ultimate shanks

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSMatchingShank independently normalizes profiles and applies start/middle/end wall-drop overrides before generating matching-band geometry. Old CSUltimateShank1 reads positional nested arrays for gem size, parameters, transform and profile, then feeds BuildChannel. Their data contracts differ from simple finished mesh placement.

**Offline disposition:** retain-versioned-local-algorithms

**Acceptance:**

- Open matching-band fixtures with independent start/mid/end drop; compare distance to center ring and profile outputs.
- Load each UltimateShank and UltimateMatchingShank GUID fixture, edit gem row and compare old generated surfaces and nested data shape.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; matching gem/shank channel layouts; legacy nested array contracts

**Evidence:**

- [CSMatchingShank.cs:319](<../../../recovery/canonical/cad/CSBlocks.Shank.Matching/CSMatchingShank.cs#L319>) — CSMatchingShank.Update
- [CSUltimateShank1.cs:188](<../../../recovery/canonical/cad/CSBlocks.Shank.Obsolete/CSUltimateShank1.cs#L188>) — CSUltimateShank1.Update
- [CSUltimateShank1.cs:213](<../../../recovery/canonical/cad/CSBlocks.Shank.Obsolete/CSUltimateShank1.cs#L213>) — CSUltimateShank1.Update

**Unresolved:** Body audit covers representative old/current implementations; exact old channel layouts must remain stable.

## CAD-019 French shank mesh/BRep part deformation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSFrenchShank exits early for production display, otherwise separates BRep, mesh and curve parts, constructs tagged start/center/end CSFrenchGeometry pieces according to open/closed gem loop, validates top/base/side parts and consumes UltimateGemData. Later it forms edge/Z/gem curves and emits channel parts and edge curves.

**Offline disposition:** local-kernel-and-part-assets-required

**Acceptance:**

- Load BRep and mesh French-part fixtures with start/center/end tags; vary edge thickness at all positions and compare transitions.
- Test closed gem loop, missing top/base/side assets and production-show toggle; compare outputs and errors.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; named/tagged French geometry assets; CSFourRailSweep; CAM mode

**Evidence:**

- [CSFrenchShank.cs:91](<../../../recovery/canonical/cad/CSBlocks.Shank.French/CSFrenchShank.cs#L91>) — CSFrenchShank.Update
- [CSFrenchShank.cs:273](<../../../recovery/canonical/cad/CSBlocks.Shank.French/CSFrenchShank.cs#L273>) — CSFrenchShank.Update
- [CSFrenchShank.cs:339](<../../../recovery/canonical/cad/CSBlocks.Shank.French/CSFrenchShank.cs#L339>) — CSFrenchShank.Update
- [CSFrenchShank1.cs:104](<../../../recovery/canonical/cad/CSBlocks.Shank.French.Obsolete/CSFrenchShank1.cs#L104>) — CSBlocks.Shank.French.Obsolete.CSFrenchShank1.Update

**Unresolved:**

## CAD-020 French halo display and production tooling

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSFrenchHaloSet skips when production is shown, builds gem rails from sorted gems, accepts mesh/BRep French parts and computes upper/lower rails with independent corner blends/scales. CSFrenchHaloProduction has its own production/underlying-surface gate and emits channel/shank profiles, base/loft, scallop, graver, gem and pilot-hole cutters.

**Offline disposition:** local-kernel-and-part-assets-required

**Acceptance:**

- Produce round and pointed French halos in display and production modes; compare rail corner continuity, scallops, pilot holes and cutter positions.
- Toggle boolean/divot/underlying surfaces and vary wall thickness, profile size and cluster angle; compare each named output family.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; French named part assets; CSFrenchHaloHelper/morphs; production cutter templates; CAM mode

**Evidence:**

- [CSFrenchHaloSet.cs:77](<../../../recovery/canonical/cad/CSBlocks.Settings.FrenchHalo/CSFrenchHaloSet.cs#L77>) — CSFrenchHaloSet.Update
- [CSFrenchHaloSet.cs:193](<../../../recovery/canonical/cad/CSBlocks.Settings.FrenchHalo/CSFrenchHaloSet.cs#L193>) — CSFrenchHaloSet.Update
- [CSFrenchHaloProduction.cs:128](<../../../recovery/canonical/cad/CSBlocks.Settings.FrenchHalo/CSFrenchHaloProduction.cs#L128>) — CSFrenchHaloProduction.Update
- [CSFrenchHaloSet1.cs:104](<../../../recovery/canonical/cad/CSBlocks.Settings.FrenchHalo.Obsolete/CSFrenchHaloSet1.cs#L104>) — CSBlocks.Settings.FrenchHalo.Obsolete.CSFrenchHaloSet1.Update

**Unresolved:** Full production boolean loops and obsolete French variants remain fixture-validation gaps.

## CAD-021 Linked gem/profile/collar sequences

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSLinkedGemRowOnRingRail orients at normalized rail start, uses prior right point/vector when present to derive continuity transform, computes spacing from rotated gem bounds, duplicates a requested count and emits right-point/right-vector for the next linked section.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Chain two linked gem rows of unequal sizes/rotation/spacing; verify endpoint/vector handoff and no discontinuity after changing first row.
- Remove predecessor point/vector and verify identity fallback; include old linked GUID fixture corpus.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; linked predecessor outputs; normalized arc-length framing

**Evidence:**

- [CSLinkedGemRowOnRingRail.cs:59](<../../../recovery/canonical/cad/CSBlocks.Linked/CSLinkedGemRowOnRingRail.cs#L59>) — CSLinkedGemRowOnRingRail.Update
- [CSLinkedGemRowOnRingRail.cs:136](<../../../recovery/canonical/cad/CSBlocks.Linked/CSLinkedGemRowOnRingRail.cs#L136>) — CSLinkedGemRowOnRingRail.Update
- [CSLinkedGemOnRingRail1.cs:108](<../../../recovery/canonical/cad/CSBlocks.Linked.Obsolete/CSLinkedGemOnRingRail1.cs#L108>) — CSBlocks.Linked.Obsolete.CSLinkedGemOnRingRail1.Update

**Unresolved:** Other linked collar/profile/space/start nodes are indexed, not separately body-validated.

## CAD-022 Gem/prong channel builder mode and width loopback

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGemInChannelSetUp switches prong versus channel spacing/wall-drop inputs, requires profile and gem definition, rotates the gem and determines whether profile width/wall spacing changes require gem size recalculation. It emits profile/gem sizes and loopback values used by downstream builders.

**Offline disposition:** local-kernel-and-graph-semantics-required

**Acceptance:**

- Toggle channel/prong mode while holding geometry fixed; compare effective gem spacing, wall drop and loopback profile width/gem size.
- Change profile width then gem size separately and compare dependency-update order with PC.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; gem table scaling; graph update markers/loopback

**Evidence:**

- [CSGemInChannelSetUp.cs:107](<../../../recovery/canonical/cad/CSBlocks.Builders/CSGemInChannelSetUp.cs#L107>) — CSGemInChannelSetUp.Update
- [CSGemInChannelSetUp.cs:180](<../../../recovery/canonical/cad/CSBlocks.Builders/CSGemInChannelSetUp.cs#L180>) — CSGemInChannelSetUp.Update
- [CSGemInChannelSetUp1.cs:72](<../../../recovery/canonical/cad/CSBlocks.Builders.Obsolete/CSGemInChannelSetUp1.cs#L72>) — CSBlocks.Builders.Obsolete.CSGemInChannelSetUp1.Update

**Unresolved:**

## CAD-023 Gem seat/cutter solids

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGemCutter converts each gem to local unscaled coordinates, produces girdle/top/seat/bottom outlines with Z offsets, clamps scale to .01 and lofts four sections straight with caps, then transforms cutter back to world.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Generate seat cutters for round and elongated gems with positive/negative spacing and distinct top/seat/bottom offsets; compare capped solid volume and placement.
- Use offsets that would collapse the cutter; confirm .01 clamp and behavior of invalid gems.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; gem outline and unscaled transforms

**Evidence:**

- [CSGemCutter.cs:50](<../../../recovery/canonical/cad/CSBlocks.Cutters/CSGemCutter.cs#L50>) — CSGemCutter.Update
- [CSGemCutter.cs:107](<../../../recovery/canonical/cad/CSBlocks.Cutters/CSGemCutter.cs#L107>) — CSGemCutter.Update
- [CSBezelCutter1.cs:75](<../../../recovery/canonical/cad/CSBlocks.Cutters.Obsolete/CSBezelCutter1.cs#L75>) — CSBlocks.Cutters.Obsolete.CSBezelCutter1.Update

**Unresolved:** Azure/box/bezel/channel/halo/micropave/pattern/scallop/shank cutters have separate indexed identities needing fixtures.

## CAD-024 Micro-pave metal surface and gem-radius interpolation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSMicroPave requires gems, rail curve, base profile and ring radius. It maps gem centers to closest curve parameters and sizes, recalculates curves on dependency changes, creates a micro-pave surface and start/end profiles. CalculateCurves maps arc lengths to a straight line and interpolates local gem radii.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Build a micro-pave row with varying gem sizes on a curved shank; compare metal surface and endpoint profiles.
- Modify gem size and curve independently and verify interpolated envelope refresh, including missing-radius/profile failure.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; MicroPaveProng helpers; dependency change markers

**Evidence:**

- [CSMicroPave.cs:59](<../../../recovery/canonical/cad/CSBlocks.Surfaces/CSMicroPave.cs#L59>) — CSMicroPave.Update
- [CSMicroPave.cs:113](<../../../recovery/canonical/cad/CSBlocks.Surfaces/CSMicroPave.cs#L113>) — CSMicroPave.CalculateCurves

**Unresolved:**

## CAD-025 Milgrain point spacing

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSMilgrainPoints computes point count as curve length/(2*size+spacing), samples normalized arc length and nests outputs only for multiple input curves. Invalid curve or pitch below .001 is skipped.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Sample a 10mm line with size .1 and spacing .05; compare count and exact point positions including end behavior.
- Supply two curves and verify nested array shape, then pitch .0009 and verify skipped output.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSMilgrainPoints.cs:70](<../../../recovery/canonical/cad/CSBlocks.Transforms/CSMilgrainPoints.cs#L70>) — CSMilgrainPoints.GeneratePointsFromCurve

**Unresolved:**

## CAD-026 Profile placement on one/two ring rails

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSProfilePlacer2RailXform rejects more than one inner/outside rail, adjusts relative curve direction and length position, wraps closed-rail normalized values once and clamps open rails. It uses normalized arc-length parameters and outputs transform/profile metadata consumed by sweeps.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Place asymmetric profile at positions -.1, 0, .5, 1 and 1.1 on open/closed two-rail inputs; compare transforms and parameter outputs.
- Reverse either rail or supply reference curve and verify frame orientation and side selection.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; profile dictionaries; rail direction/seam conventions

**Evidence:**

- [CSProfilePlacer2RailXform.cs:48](<../../../recovery/canonical/cad/CSBlocks.Profiles/CSProfilePlacer2RailXform.cs#L48>) — CSProfilePlacer2RailXform.Update
- [CSProfilePlacer2RailXform.cs:108](<../../../recovery/canonical/cad/CSBlocks.Profiles/CSProfilePlacer2RailXform.cs#L108>) — CSProfilePlacer2RailXform.Update
- [CSProfilePlacer1RailXform1.cs:57](<../../../recovery/canonical/cad/CSBlocks.Profiles.Obsolete/CSProfilePlacer1RailXform1.cs#L57>) — CSBlocks.Profiles.Obsolete.CSProfilePlacer1RailXform1.Update

**Unresolved:** All 13 current/7 old profile node identities remain separate test entries.

## CAD-027 Dome end caps and collar trimming

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSProfileCapRing derives an average profile plane, transforms and centers the profile, picks dome sign from location/orientation, rail-revolves a dome and transforms it back. CSSweepCollarRail locally unscales a rail, intersects a horizontal line at height, trims between extreme hits and extends tangents toward ZX plane.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Cap an asymmetric ring profile on opposite ring sides and compare dome direction and thickness.
- Trim collar with 2 and >2 intersections, tangent side extensions and height outside bounds; compare rail output and failure modes.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSProfileCapRing.cs:64](<../../../recovery/canonical/cad/CSBlocks.Caps/CSProfileCapRing.cs#L64>) — CSProfileCapRing.Update
- [CSSweepCollarRail.cs:76](<../../../recovery/canonical/cad/CSBlocks.Collars/CSSweepCollarRail.cs#L76>) — CSSweepCollarRail.Update
- [CSSweepCollarRail.cs:99](<../../../recovery/canonical/cad/CSBlocks.Collars/CSSweepCollarRail.cs#L99>) — CSSweepCollarRail.Update
- [CSSweepCollarRail1.cs:107](<../../../recovery/canonical/cad/CSBlocks.Collars.Obsolete/CSSweepCollarRail1.cs#L107>) — CSBlocks.Collars.Obsolete.CSSweepCollarRail1.Update

**Unresolved:**

## CAD-028 Bail rails and opening measurement

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSBailRail reads length/opening/pinch/angle/closed and three blend values, removes short segments at .2, repairs bad knots and calls RhinoRepairCurve before building its modified rail. Dedicated outputs provide three parameters and inside height/depth.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Build open/closed bail with pinch and three blend controls; compare opening inside height/depth and sweep placement parameters.
- Use an input rail with sub-.2 segments/bad knots; compare repaired rail topology to PC.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; Bail morph/profile helpers

**Evidence:**

- [CSBailRail.cs:110](<../../../recovery/canonical/cad/CSBlocks.Bails/CSBailRail.cs#L110>) — CSBailRail.Update
- [CSBailRail.cs:136](<../../../recovery/canonical/cad/CSBlocks.Bails/CSBailRail.cs#L136>) — CSBailRail.Update
- [CSBailRail1.cs:100](<../../../recovery/canonical/cad/CSBlocks.Bails.Obsolete/CSBailRail1.cs#L100>) — CSBlocks.Bails.Obsolete.CSBailRail1.Update

**Unresolved:** Bail placer, two-rail side profile, half profile and obsolete variants are in inventory but not individually body-audited.

## CAD-029 Curve offsets with local plane conventions

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCurveOffset obtains plane/bounds for planar curves, reverses the requested offset sign for that branch, uses OffsetOld closed=true and optionally applies a Z translation carrying transform metadata. This legacy kernel path and sign convention must be retained.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Offset clockwise/counterclockwise planar loops in XY and tilted planes by +/- .2; compare side, curve count and closure.
- Offset a curve array with optional Z shift and confirm transform metadata and failure behavior for bad curve.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSUtilGeometry.OffsetOld

**Evidence:**

- [CSCurveOffset.cs:71](<../../../recovery/canonical/cad/CSBlocks.Curves/CSCurveOffset.cs#L71>) — CSCurveOffset.Update
- [CSCurveOffset.cs:73](<../../../recovery/canonical/cad/CSBlocks.Curves/CSCurveOffset.cs#L73>) — CSCurveOffset.Update
- [CSBlendCurves1.cs:103](<../../../recovery/canonical/cad/CSBlocks.Curves.Obsolete/CSBlendCurves1.cs#L103>) — CSBlocks.Curves.Obsolete.CSBlendCurves1.Update

**Unresolved:**

## CAD-030 Planar curve boolean with first-result selection

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCurveBoolean repeatedly subtracts each negative curve through RhinoCurveBooleanDifference. When multiple loops are returned it uses array[0], converts back to NURBS and preserves input transform metadata. Empty/failed difference leaves the previous curve.

**Offline disposition:** retain-kernel-result-selection

**Acceptance:**

- Subtract a stripe splitting a positive loop into two islands and compare which first island remains with PC.
- Run a nonintersecting and invalid subtraction and verify unchanged input behavior and transform metadata.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSCurveBoolean.cs:56](<../../../recovery/canonical/cad/CSBlocks.Curves/CSCurveBoolean.cs#L56>) — CSCurveBoolean.Update
- [CSCurveBoolean.cs:62](<../../../recovery/canonical/cad/CSBlocks.Curves/CSCurveBoolean.cs#L62>) — CSCurveBoolean.Update

**Unresolved:**

## CAD-031 One-, two- and four-rail sweeps with parameters and seam handling

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSSimpleSweep2 gathers rails/profile arrays and optional parameter arrays; with no parameters it calls Sweep2NoParam with seam/maintain-height/closed options, otherwise Sweep2. It rebuilds dynamic output channels for every returned BRep. CSUtilSweepRC Sweep4Profile stores four curve parameters, rematches direction and seam and splits profile into rail-matched pieces.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Sweep asymmetric profiles on open/closed rails with/without explicit parameters and reversed seam; compare surface count, order, normals and channel IDs.
- Build four-rail sweep with all four contact points, reverse a section and verify continuity and seam consistency.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSUtilSweep/CSUtilSweepRC; dynamic output channel identities

**Evidence:**

- [CSSimpleSweep2.cs:124](<../../../recovery/canonical/cad/CSBlocks.Sweeps/CSSimpleSweep2.cs#L124>) — CSSimpleSweep2.Update
- [CSSimpleSweep2.cs:135](<../../../recovery/canonical/cad/CSBlocks.Sweeps/CSSimpleSweep2.cs#L135>) — CSSimpleSweep2.Update
- [CSUtilSweepRC.cs:130](<../../../recovery/canonical/cad/CSBlockUtil/CSUtilSweepRC.cs#L130>) — CSUtilSweepRC.Sweep4Profile.SetupFromRails
- [CSUtilSweepRC.cs:171](<../../../recovery/canonical/cad/CSBlockUtil/CSUtilSweepRC.cs#L171>) — CSUtilSweepRC.Sweep4Profile.Split
- [CSMeshPipe1.cs:59](<../../../recovery/canonical/cad/CSBlocks.Sweeps.Obsolete/CSMeshPipe1.cs#L59>) — CSBlocks.Sweeps.Obsolete.CSMeshPipe1.Update

**Unresolved:** All 12 current and 2 obsolete sweep registrations require separate fixtures including pipe/revolve and no-profile branches.

## CAD-032 Loft and matched profile direction

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSLoft takes only the first curve from each input array and calls LoftMatchCurves with straight loft and capped=false, emitting the converted BRep or explicit failure.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Loft two differently oriented asymmetric closed profiles and compare seam matching and uncapped surface.
- Pass multiple curves per channel and verify PC first-item behavior and missing/invalid failure.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSLoft.cs:43](<../../../recovery/canonical/cad/CSBlocks.Surfaces/CSLoft.cs#L43>) — CSLoft.Update

**Unresolved:**

## CAD-033 Advanced network surfaces with independent edge continuity

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSNetworkSurfaceAdvanced collects U/V curves, translates four continuity-mode selectors, calls NurbsSurface.CreateNetworkSurface with .001 edge tolerance, .01 interior tolerance and application angle tolerance, converts through BRep/OnBrep and reports mapped error codes.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Create a U/V network with independent start/end continuity and compare positional/tangent/curvature seams against PC.
- Use incompatible networks to compare error strings and invalid-mode behavior; repeat at small jewelry dimensions.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; NetworkSurfaceHelper continuity/error mapping

**Evidence:**

- [CSNetworkSurfaceAdvanced.cs:92](<../../../recovery/canonical/cad/CSBlocks.Surfaces/CSNetworkSurfaceAdvanced.cs#L92>) — CSNetworkSurfaceAdvanced.Update

**Unresolved:**

## CAD-034 Surface offset and standard extrusion/sphere/planar surfaces

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSSurfaceOffset flattens input, offsets each ICSSurface at requested distance using application tolerance, wraps results and emits scalar for one or array otherwise. Other surface/extrusion/sphere identities are catalogued separately.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Offset a trimmed/curved surface inward and outward and compare boundaries and normals.
- Offset an array of mixed surface/non-surface data; compare output filtering and scalar/array convention.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSSurfaceOffset.cs:42](<../../../recovery/canonical/cad/CSBlocks.Surfaces/CSSurfaceOffset.cs#L42>) — CSSurfaceOffset.Update
- [CSExtrudePlanarCurve1.cs:42](<../../../recovery/canonical/cad/CSBlocks.Surfaces.Obsolete/CSExtrudePlanarCurve1.cs#L42>) — CSBlocks.Surfaces.Obsolete.CSExtrudePlanarCurve1.Update

**Unresolved:** Only offset body is audited in this grouping; extrusion/sphere/planar variants need fixture validation.

## CAD-035 BRep booleans, join, split and capping semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSBooleanDifference supports a flip input that actually selects intersection, calls kernel booleans at .01 tolerance and returns duplicated left input if result is null or empty. CSBooleanUnion uses application tolerance and also falls back to duplicated input. Failed operations can therefore look like successful unchanged geometry.

**Offline disposition:** local-kernel-required

**Acceptance:**

- Boolean overlapping, tangent and disjoint jewelry solids; compare output count and unchanged-input fallback at .01 versus application tolerance.
- Set difference flip=true and verify intersection result; test split/join/cap registered node fixtures separately.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSUtilDataHelper clean BRep filtering

**Evidence:**

- [CSBooleanDifference.cs:40](<../../../recovery/canonical/cad/CSBlocks.Solids/CSBooleanDifference.cs#L40>) — CSBooleanDifference.Update
- [CSBooleanUnion.cs:28](<../../../recovery/canonical/cad/CSBlocks.Solids/CSBooleanUnion.cs#L28>) — CSBooleanUnion.Update
- [CSBooleanDifferenceFast1.cs:99](<../../../recovery/canonical/cad/CSBlocks.Solids.Obsolete/CSBooleanDifferenceFast1.cs#L99>) — CSBlocks.Solids.Obsolete.CSBooleanDifferenceFast1.Update

**Unresolved:** Mesh boolean, split/join/cap bodies are indexed but not individually behavior-verified.

## CAD-036 Rigid transforms, object-local transforms, mirrors and polar arrays

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSMirror resolves optional coordinate transform/inverse, scales selected axes negatively, duplicates combinations when requested and flips geometry orientation after handedness changes. CSPolarArray duplicates objects in nested primary/sub loops with selected axes, offsets and optional visible-count clipping.

**Offline disposition:** local-transform-core-required

**Acceptance:**

- Mirror asymmetric solid on X/Y/Z with duplicate on/off and custom frame; compare combinations, normals and source preservation.
- Build primary/sub polar array with nondefault axes/count visibility and compare transform order, object count and ordering.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSData transform/flip preservation; world versus object frame

**Evidence:**

- [CSMirror.cs:40](<../../../recovery/canonical/cad/CSBlocks.Transforms/CSMirror.cs#L40>) — CSMirror.Update
- [CSMirror.cs:67](<../../../recovery/canonical/cad/CSBlocks.Transforms/CSMirror.cs#L67>) — CSMirror.Update
- [CSPolarArray.cs:92](<../../../recovery/canonical/cad/CSBlocks.Transforms/CSPolarArray.cs#L92>) — CSPolarArray.Update
- [CSCenterObjectOnObjectTransform1.cs:81](<../../../recovery/canonical/cad/CSBlocks.Transforms.Obsolete/CSCenterObjectOnObjectTransform1.cs#L81>) — CSBlocks.Transforms.Obsolete.CSCenterObjectOnObjectTransform1.Update

**Unresolved:** 38 current and 12 obsolete transform registrations must retain distinct numeric/frame semantics.

## CAD-037 Flow along surface morph

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSFlowAlongSurface accepts surface or first BRep face, optionally transposes/reverses source UV, configures special planar source frame/lines, duplicates object data, morphs with tolerance .05 and flips output if FlipZ selected.

**Offline disposition:** local-nonlinear-morph-required

**Acceptance:**

- Flow asymmetric planar motif to doubly curved target with every UV swap/reversal and FlipZ; compare orientation and surface fidelity.
- Use trimmed BRep sources/destinations and verify first-face selection and source data immutability.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSFlowAlongSurfaceMorph; UV parameterization

**Evidence:**

- [CSFlowAlongSurface.cs:175](<../../../recovery/canonical/cad/CSBlocks.UDT/CSFlowAlongSurface.cs#L175>) — CSFlowAlongSurface.Update
- [CSFlowAlongSurface.cs:250](<../../../recovery/canonical/cad/CSBlocks.UDT/CSFlowAlongSurface.cs#L250>) — CSFlowAlongSurface.Update

**Unresolved:**

## CAD-038 Curved XY taper, ring pull and grouped deformation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCurvedTaperXY computes a bounding box and builds a three-position independent X/Y morph, clamping additional Z bounds to nonnegative, duplicates input and morphs at .05. CSPullToRail derives local unscaled frame and top-center angle, gets oriented bounds and morphs duplicated data, emitting only successful results.

**Offline disposition:** local-nonlinear-morph-required

**Acceptance:**

- Apply unequal X/Y taper at three Z stations to mesh, BRep and gem group; compare section bounds and unchanged source data.
- Pull to ring with rotated/scaled objects, vary multiplier and center-control/group modes using per-GUID fixtures.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; SpaceMorph/OnSpaceMorph; PullToRailMorph; group-center variants

**Evidence:**

- [CSCurvedTaperXY.cs:112](<../../../recovery/canonical/cad/CSBlocks.UDT/CSCurvedTaperXY.cs#L112>) — CSCurvedTaperXY.Update
- [CSPullToRail.cs:84](<../../../recovery/canonical/cad/CSBlocks.UDT/CSPullToRail.cs#L84>) — CSPullToRail.Update

**Unresolved:** PullToCurve/PlopOnRingRail/RingTaper/group variants are indexed; individual bodies not all inspected.

## CAD-039 Dimensional measurement: curve length and directional surface distance

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSCurveLength emits per-curve kernel length and scalar/array shape. CSDistanceFromSurface extracts object transform origin/Z direction, intersects a +/-100 line against BRep at application tolerance, then selects closest overlap/intersection point. It is directional distance, not arbitrary Euclidean closest distance.

**Offline disposition:** local-kernel-measurement-required

**Acceptance:**

- Measure circle/arc/spline lengths and compare to PC under model units/tolerance.
- Use a surface whose nearest point is off object Z axis and verify directional intersection result; include overlap and no-hit cases.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSCurveLength.cs:37](<../../../recovery/canonical/cad/CSBlocks.Curves/CSCurveLength.cs#L37>) — CSCurveLength.Update
- [CSDistanceFromSurface.cs:69](<../../../recovery/canonical/cad/CSBlocks.Core/CSDistanceFromSurface.cs#L69>) — CSDistanceFromSurface.Update

**Unresolved:** Other point/curve/rail/planar distance nodes are inventoried with separate acceptance fixtures.

## CAD-040 Volume centroid and approximate union volume

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSVolumeCentroid collects solid BReps, caps open BReps when possible and accepts closed meshes, computes VolumeMassProperties and emits a centroid. The recovered code indexes list3[0] without a guard and then sums every entry including that first item again. MeshRepairHelper converts meshes to GMesh and OctTree.GetVolume computes voxel union at depth 11 with pixel-count correction and scale correction.

**Offline disposition:** local-kernel-and-voxel-volume-required

**Acceptance:**

- Measure two asymmetric solids of known differing volumes and centers; capture PC centroid including first-entry weighting behavior.
- Run only-open/unclosable input and document error; compute two overlapping spheres with octree volume and compare PC within calibrated voxel tolerance.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; OctTree/GMesh/OctShell; closed-shell policies

**Evidence:**

- [CSVolumeCentroid.cs:77](<../../../recovery/canonical/cad/CSBlocks.Bails/CSVolumeCentroid.cs#L77>) — CSVolumeCentroid.Update
- [MeshRepairHelper.cs:737](<../../../recovery/canonical/cad/CS.Utilities.Helpers/MeshRepairHelper.cs#L737>) — MeshRepairHelper.GetVolumeViaOctTree
- [OctTree.cs:222](<../../../recovery/canonical/cad/OctTree/OctTree.cs#L222>) — OctTree.GetVolume

**Unresolved:** Observed centroid edge cases may be recovered-source defects. Approximate octree volume must not be replaced by naive sum of overlapping mesh volumes.

## CAD-041 Text, font outlines and solid lettering

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSTextObjects resolves font enum description in Rhino font table, explodes TextEntity to outlines, makes planar BReps, offsets first faces by half depth on both sides into solids, fixes orientation and optionally translates union bounding-box center to origin.

**Offline disposition:** local-font-outline-equivalence-required

**Acceptance:**

- Generate bold/italic text containing holes and non-ASCII characters; compare outline nesting, depth and solid closure with identical PC font.
- Center and uncenter same text and compare world bounds, glyph spacing and output count.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; font enum/name mapping; installed font faces and metrics

**Evidence:**

- [CSTextObjects.cs:67](<../../../recovery/canonical/cad/CSBlocks.Tools/CSTextObjects.cs#L67>) — CSTextObjects.Update
- [CSTextObjects.cs:100](<../../../recovery/canonical/cad/CSBlocks.Tools/CSTextObjects.cs#L100>) — CSTextObjects.Update

**Unresolved:** iPad font availability and glyph metric equivalence unverified; user font files may be external dependencies.

## CAD-042 Captured Rhino geometry and file-backed part groups

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSRhinoObjectsFromBoundingBox binary Read loads embedded 3dm objects and sets _useObjects; otherwise Update captures selected Rhino objects inside an expanded XY box spanning +/-1000 Z and stores name/layer user strings. Write embeds 3dm geometry. CSGroupOfRhinoObjectsFilePath chooses allowed gem orientations, normalizes geometry to gem dimensions and attaches CSMounting tags.

**Offline disposition:** local-geometry-and-path-remapping-required

**Acceptance:**

- Load PC fixture with captured geometry but no original source file; ensure geometry survives and can regenerate.
- Import part group with allowed gem-shape list and CSMounting tags; compare normalization, disabled choices, previews and material assignment.
- Round-trip this node through proposed JSON interchange and verify _objects are retained; own JSON overrides are absent in recovered class.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; embedded 3dm object payloads; external file paths/parts/previews; CSName/CSLayer/CSMounting tags

**Evidence:**

- [CSRhinoObjectsFromBoundingBox.cs:88](<../../../recovery/canonical/cad/CSBlocks.Core/CSRhinoObjectsFromBoundingBox.cs#L88>) — CSRhinoObjectsFromBoundingBox.Read
- [CSRhinoObjectsFromBoundingBox.cs:144](<../../../recovery/canonical/cad/CSBlocks.Core/CSRhinoObjectsFromBoundingBox.cs#L144>) — CSRhinoObjectsFromBoundingBox.Update
- [CSRhinoObjectsFromBoundingBox.cs:177](<../../../recovery/canonical/cad/CSBlocks.Core/CSRhinoObjectsFromBoundingBox.cs#L177>) — CSRhinoObjectsFromBoundingBox.Write
- [CSGroupOfRhinoObjectsFilePath.cs:194](<../../../recovery/canonical/cad/CSBlocks.Core/CSGroupOfRhinoObjectsFilePath.cs#L194>) — CSGroupOfRhinoObjectsFilePath.Update
- [CSBoundingBox2.cs:118](<../../../recovery/canonical/cad/CSBlocks.Core.Obsolete/CSBoundingBox2.cs#L118>) — CSBlocks.Core.Obsolete.CSBoundingBox2.Update

**Unresolved:** Concrete JSON/binary asymmetry: inherited JSON alone may omit custom captured geometry. Full read/write field analysis belongs to primary audit.

## CAD-043 SMI manufacturing profile tools and textual output

- [ ] Implement and verify the acceptance criteria below.

**Observed:** SMI cutter nodes operate on profile curves and transform metadata; CSSMICutterComfortFit passes profile, transform and edge height into CutterComfortFit. CSSMIOutput walks numeric/profile inputs and builds text sections using the block identifier and profile names. Shell, interval and cutter variants form a manufacturing-specific family beyond visual geometry.

**Offline disposition:** local-kernel-and-manufacturing-output-required

**Acceptance:**

- Apply comfort-fit cutter to a transformed profile at two edge heights; compare profile section and transform metadata.
- Generate SMI output for numeric arrays and named profiles under PC locale and compare exact text lines; run all SMI cutter fixture IDs.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSUtilSMI manufacturing curves; locale numeric formatting/profile names

**Evidence:**

- [CSSMICutterComfortFit.cs:50](<../../../recovery/canonical/cad/CSBlocks.SMI/CSSMICutterComfortFit.cs#L50>) — CSSMICutterComfortFit.Update
- [CSSMIOutput.cs:27](<../../../recovery/canonical/cad/CSBlocks.SMI/CSSMIOutput.cs#L27>) — CSSMIOutput.Update
- [CSSMICutterBand1.cs:86](<../../../recovery/canonical/cad/CSBlocks.SMI.Obsolete/CSSMICutterBand1.cs#L86>) — CSBlocks.SMI.Obsolete.CSSMICutterBand1.Update

**Unresolved:** 23 current/3 obsolete SMI registrations are inventoried; manufacturing controller/file contract not independently recovered in this scope.

## CAD-044 Graph arithmetic, nonlinear controls and branching/array shape

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSNonlinear clamps numeric input, scales/translates a response curve into input/output ranges and intersects it at requested X. CSIfElse passes the selected branch data directly. This family also contains arithmetic/comparison/boolean reducers and array flatten/unarray/reorder utilities that influence builder topology and data shape.

**Offline disposition:** port-managed-core-plus-response-curves

**Acceptance:**

- Evaluate nonlinear control below/inside/above range with increasing/decreasing response curves and compare output/branch geometry.
- Toggle IfElse between scalar and nested arrays and compare aliasing/change propagation; validate arithmetic divide/NaN and boolean reducer edge cases per fixture.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; graph data aliasing; channel flatten/scalar/array conventions

**Evidence:**

- [CSNonlinear.cs:43](<../../../recovery/canonical/cad/CSBlocks.Maths/CSNonlinear.cs#L43>) — CSNonlinear.Update
- [CSIfElse.cs:37](<../../../recovery/canonical/cad/CSBlocks.Utilities/CSIfElse.cs#L37>) — CSIfElse.Update
- [CSUnarray1.cs:47](<../../../recovery/canonical/cad/CSBlocks.Utilities.Obsolete/CSUnarray1.cs#L47>) — CSBlocks.Utilities.Obsolete.CSUnarray1.Update

**Unresolved:** Individual arithmetic/array implementations are declaration-inventoried; generic names alone do not establish exact edge semantics.

## CAD-045 Display mesh preprocessing is distinct from manufacturing mesh

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGenerateMesh duplicates flattened data and calls PreProcessDisplayInfo on CSOnGeometryData, then selects null/scalar/array output. It does not directly export an STL or invoke repair; display tessellation caches and manufacturing meshes follow separate paths.

**Offline disposition:** local-tessellation-and-cache-required

**Acceptance:**

- Regenerate display mesh for trimmed curved BRep, then change a parameter and verify mesh invalidation and unchanged source.
- Compare view quality and exported STL quality independently, including scalar/empty/multiple inputs.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels; CSOnGeometryData display meshing/cache

**Evidence:**

- [CSGenerateMesh.cs:34](<../../../recovery/canonical/cad/CSBlocks.Core/CSGenerateMesh.cs#L34>) — CSGenerateMesh.Update

**Unresolved:**

## CAD-046 Manufacturing mesh repair and native union

- [ ] Implement and verify the acceptance criteria below.

**Observed:** MeshRepairHelper caps and joins BReps at .001, converts geometry to triangular meshes, flips inward solid orientation, optionally prerepairs open shells, appends meshes and calls Netfabb repair. NetfabbRepairMesh scales input by 3, requires valid repaired result, scales back and unwelds at 22.5 degrees. NetfabbHelper dynamically loads Windows helper/managed DLLs and marshals pinned vertex/index buffers to native build-mesh.

**Offline disposition:** major-native-platform-replacement-required

**Acceptance:**

- Repair intersecting/open/reversed/disconnected jewelry meshes and compare watertightness, manifold edges, shell count and volume with PC normal/extended/prerepair options.
- Verify thin prongs and milgrain survive tessellation and repair; measure dimensional drift after 3x scale round trip.

**Dependencies:** Rhino meshing and BRep topology; gemvision_helper64.dll; gemvision_netfabbmanaged.dll; native Netfabb engine/context; CSMeshRepairPath assets

**Evidence:**

- [MeshRepairHelper.cs:210](<../../../recovery/canonical/cad/CS.Utilities.Helpers/MeshRepairHelper.cs#L210>) — MeshRepairHelper.RepairMesh
- [MeshRepairHelper.cs:631](<../../../recovery/canonical/cad/CS.Utilities.Helpers/MeshRepairHelper.cs#L631>) — MeshRepairHelper.NetfabbRepairMesh
- [NetfabbHelper.cs:34](<../../../recovery/canonical/cad/CS.Utilities.Helpers/NetfabbHelper.cs#L34>) — NetfabbHelper static initializer
- [NetfabbHelper.cs:98](<../../../recovery/canonical/cad/CS.Utilities.Helpers/NetfabbHelper.cs#L98>) — NetfabbHelper.NetfabbBuildMesh

**Unresolved:** Native implementation and distributable iPad equivalent not established. RepairHelper and older Travis MeshRepair are both present and must be traced by actual call path.

## CAD-047 Binary STL writer and optional repair export

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RhinoServices.ExportDesignAsSTL sets center/side/accent gem flags and export mode, adds model to Rhino, calls Travis MeshRepair.CreateSTLFromRhinoObjects and cleans temporary document geometry. Binary writer triangulates, writes an 80-byte header, declared face count, normal/three vertices and zero attribute bytes. NaN vertex triangles are skipped after count is written, so malformed-count cases require tests.

**Offline disposition:** offline-local-export-after-kernel-replacement

**Acceptance:**

- Export with all 8 center/side/accent inclusion combinations and repair on/off; verify triangle count, shell content, units and topology.
- Validate each binary STL length equals 84+50*declaredFaces; include NaN/invalid geometry and assert explicit failure rather than silent corrupt output.

**Dependencies:** Rhino temporary document/layers; CSOptionsExport gem-print flags; Travis MeshRepair; filesystem binary output

**Evidence:**

- [RhinoServices.cs:1954](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L1954>) — RhinoServices.ExportDesignAsSTL
- [MeshRepair.cs:173](<../../../recovery/canonical/cad/CS.Utilities.Travis/MeshRepair.cs#L173>) — MeshRepair.CreateSTLFromRhinoObjects
- [MeshRepairHelper.cs:278](<../../../recovery/canonical/cad/CS.Utilities.Helpers/MeshRepairHelper.cs#L278>) — MeshRepairHelper.SaveStlBinary

**Unresolved:** Service invokes Travis path; writer example in newer helper demonstrates format/edge behavior but caller path must be verified separately.

## CAD-048 3DM export retaining user data and original BRep geometry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ExportDesignAs3DM disables mesh-object meshing, forces those blocks to regenerate, honors gem printing flags, adds model to Rhino and writes a temporary file with user data enabled, render meshes/bitmap table disabled and all objects selected. It copies to requested destination and deletes temporary model. It is geometry export, not full parametric project serialization.

**Offline disposition:** local-3dm-writer-equivalence-required

**Acceptance:**

- Export editable BReps/curves/gems/mesh objects and reopen in PC Rhino; compare layer/material names, user strings and shape without render meshes.
- Include/exclude gem categories and verify mesh-object regeneration yields intended analytic geometry and no temporary document objects remain.

**Dependencies:** Rhino 3dm writer/native geometry; FileWriteOptions; material/layer/user-string mapping; local filesystem

**Evidence:**

- [RhinoServices.cs:1987](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L1987>) — RhinoServices.ExportDesignAs3DM
- [RhinoServices.cs:2014](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L2014>) — RhinoServices.ExportDesignAs3DM
- [RhinoServices.cs:2016](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L2016>) — RhinoServices.ExportDesignAs3DM

**Unresolved:** Requires portable 3dm writer plus equivalent geometry/user-data representation; does not preserve graph by itself.

## CAD-049 Photoreal rendering with material, environment and prop libraries

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RenderHelper.GenerateCustomRender explicitly selects V-Ray, loads fullenv.visopt and selected environment .visopt, registers resource paths, controls batch/output/progress, inserts render objects and .3dm props/ground plane, loads .vismat materials and manages alpha contribution. It depends on renderer-specific scene/material APIs and local asset paths.

**Offline disposition:** major-renderer-and-asset-port-required

**Acceptance:**

- Render same PC scene with gemstone/metal/skin materials, chosen environment and prop at fixed camera/resolution; compare image appearance and transparent alpha.
- Work offline with complete bundled textures/environments/props; verify missing assets are explicit and output PNG saves locally.

**Dependencies:** V-Ray Rhino plugin/VRayInterface; RMA/Rhino render document; RenderMaterials .visopt/.vismat/textures; Props .3dm and camera/groundplane state

**Evidence:**

- [RenderHelper.cs:38](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/RenderHelper.cs#L38>) — RenderHelper.GenerateCustomRender
- [RenderHelper.cs:47](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/RenderHelper.cs#L47>) — RenderHelper.GenerateCustomRender
- [RenderHelper.cs:55](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/RenderHelper.cs#L55>) — RenderHelper.GenerateCustomRender
- [RenderHelper.cs:132](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/RenderHelper.cs#L132>) — RenderHelper.GenerateCustomRender

**Unresolved:** Equivalent iPad rendering and legal availability of renderer/material assets are not established by source recovery.

## CAD-050 Printer connection, support generation, job packing and controls

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ProjetHelper connects by IP through Geomagic printer APIs, requires idle state to create supports, applies preview rotations, prepares model/supports and sends print jobs. Multiple-job printing validates FTX gray/green cartridge, rotates/packs meshes into boundary rectangle, generates native build file and sends it; abort/pause/resume/status APIs are exposed.

**Offline disposition:** external-device-integration-and-native-replacement

**Acceptance:**

- Against a controlled supported printer or SDK simulator, test idle/busy/offline transitions, supports, model preview transforms, pause/resume/abort and error strings.
- Pack multiple jobs near build bounds with both supported cartridge IDs and unsupported material; compare build file and explicit oversized-job rejection.

**Dependencies:** Geomagic.Print.Headless native printer SDK; printer on local network; device material/status APIs; support generation/build format; Rhino viewport conduit

**Evidence:**

- [ProjetHelper.cs:462](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/ProjetHelper.cs#L462>) — ProjetHelper.ConnectToPrinter
- [ProjetHelper.cs:483](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/ProjetHelper.cs#L483>) — ProjetHelper.CSCreateSupportsForCSItem
- [ProjetHelper.cs:671](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/ProjetHelper.cs#L671>) — ProjetHelper.PrintCSModel
- [ProjetHelper.cs:681](<../../../recovery/canonical/cad/CounterSketchServer.Utilities.Helpers/ProjetHelper.cs#L681>) — ProjetHelper.PrintMultipleJobs

**Unresolved:** This feature inherently requires a physical/network device; an airplane-mode requirement cannot include sending printer jobs. No printer contacted in audit.

## CAD-051 CAD commands: parallel-processing toggle and VR service start/stop

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSToggleBlockParallelProcessing is a hidden Rhino command that flips application state and forces graph redraw. CSVRStart/CSVRStop are MRhinoTestCommand wrappers returning success/failure around CSVRHelper server start/stop. These are the only three command-derived declarations found in the baseline.

**Offline disposition:** adapt-command-ui-and-optional-service

**Acceptance:**

- Toggle processing twice and compare geometry ordering/cancellation/graph redraw against PC.
- Start/stop VR service with already-running/failure states and verify reported command result; determine whether iPad product retains VR scope.

**Dependencies:** Rhino command dispatch; CSVRHelper service/runtime; parallel scheduler

**Evidence:**

- [CSToggleBlockParallelProcessing.cs:21](<../../../recovery/canonical/cad/CounterSketchServer.Commands/CSToggleBlockParallelProcessing.cs#L21>) — CSToggleBlockParallelProcessing.RunCommand
- [CSVRStartCommand.cs:19](<../../../recovery/canonical/cad/CounterSketchServer.Commands/CSVRStartCommand.cs#L19>) — CSVRStartCommand.RunCommand
- [CSVRStopCommand.cs:19](<../../../recovery/canonical/cad/CounterSketchServer.Commands/CSVRStopCommand.cs#L19>) — CSVRStopCommand.RunCommand

**Unresolved:** Absence of more command classes does not mean absence of callable service/UI operations; RhinoServices has broader entry points.

## CAD-052 Legacy surface punching around gem rails

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSSurfacePunch1 combines channel spacings, computes left/right gem rails at positive and negative split distances, extends endpoints, closes open borders, lofts a splitter and invokes RhinoBrepSplit. Its Tests namespace does not remove its registered geometry behavior from old project dependencies.

**Offline disposition:** retain-versioned-local-algorithms

**Acceptance:**

- Load SurfacePunch V1 PC fixture with open/closed gem row; compare punched BRep pieces and splitter selection.
- Vary separate start/end spacing and split distance; compare endpoint rail extension and border closure.

**Dependencies:** RhinoCommon/Rhino native geometry kernel; RMA.OpenNURBS/RMA.Rhino bridge; CSData and graph channels

**Evidence:**

- [CSSurfacePunch1.cs:61](<../../../recovery/canonical/cad/CSBlocks.Tests.Obsolete/CSSurfacePunch1.cs#L61>) — CSSurfacePunch1.Update
- [CSSurfacePunch1.cs:144](<../../../recovery/canonical/cad/CSBlocks.Tests.Obsolete/CSSurfacePunch1.cs#L144>) — CSSurfacePunch1.Update

**Unresolved:** Other SurfacePunch generations remain per-GUID fixture requirements.
