# P04-T03-A01 CSP XML metadata and paired-file identity

Goal: preserve saved portfolio XML and its unknown extensions, edit declared metadata without running desktop setters, and interpret paired design/preview filenames using recovered rules. This is source-contract compatibility, not proof from a real saved PC portfolio. No .csp fixture is currently supplied.

Sources: canonical Project.cs/Design.cs/ProjectType.cs and ProjectManagementHelper.cs. SaveProjectData uses XmlSerializer(Project) to Name.csp; PopulateProjectDesignsCollection reads baseGuid_designGuid.csg and compares raw stem parts case-sensitively for IsBaseDesign. Design image paths are baseGuid_spi.png, stem_fav.png and stem_fav_spi.png. Native load resets SelectedIterationId/ProjectType and rewrites paths; those mutations remain separate policies.

Architecture: bounded DTD-disabled XML reader, private immutable XDocument plus original bytes for exact no-op output, targeted text/bool/int edits preserving unknown elements/attributes/comments, UTF8/UTF16 encoding preservation. No file paths in XML are opened or rewritten. Filename helpers use Windows basename semantics on every host and never resolve stored paths against the iPad/host filesystem.

Independent oracle: SDK Roslyn generator derives declared public read/write non-XmlIgnore property shapes and enum from source into test-only POCOs. XmlSerializer operates on those contracts without native getters/setters, WPF or filesystem side effects. Generated source verification is required. Synthetic contract fixtures are labeled and never called original PC artifacts.

Owned: Documents portfolio XML/key modules; tools/PortfolioContractGenerator; shared Documents.Contracts fixture/oracle tests; CI and evidence.

- [x] Generate/verify source property contracts; record source hashes and scope.
- [x] RED actual codec API: generated schema metadata survives no-op/edit/serialize/read, nested variants and unknown XML retained; UTF8/UTF16/BOM, nil/absent and path cases; reject DTD, depth/count/size, ambiguous/complex scalar edits and foreign roots.
- [x] GREEN private immutable XML codec and paired filename/sidecar helper. No native getter/load/clone behavior or folder reconciliation claims.
- [x] Run local/independent XmlSerializer and existing CSG/graph/source checks; commit/push.
- [ ] Inspect iPad/iPhone assertions through macOS CI; record limitations and commit/push. Parent P04-T03 remains incomplete for real folder/corpus/history/sidecar/PC acceptance.
