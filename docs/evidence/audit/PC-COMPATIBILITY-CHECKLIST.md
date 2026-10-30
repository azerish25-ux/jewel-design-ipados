# CounterSketch PC project compatibility checklist

58 source-backed compatibility requirements scoped to assembly 8.1.19323.1002 and its declared legacy paths. The required contract is editable PC -> iPad -> PC interchange, not a geometry-only conversion.

Every checkbox is an acceptance requirement, not a passed implementation test. Static evidence is distinct from runtime behavior.

## PC-001 Versioned compatibility claim

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Supported scope is the recovered 8.1.19323.1002 reader/writer and its explicit legacy branches; this does not establish future-version compatibility.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Label import/export compatibility by actual PC version and fixture; reject unsupported future structures without overwriting the input.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [AssemblyInfo.cs:153](<../../../recovery/canonical/desktop/properties/AssemblyInfo.cs#L153>) — AssemblyVersion(
- [CSDocument.cs:775](<../../../recovery/canonical/cad/CS/CSDocument.cs#L775>) — private bool Read(string filePath

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-002 Complete project bundle

- [ ] Implement and verify the acceptance criteria below.

**Observed:** A portfolio is a directory with a named .csp XML and sibling .csg designs/assets, not a single neutral CAD model.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Import/export an entire PC portfolio directory/archive; every referenced design and required sibling asset must survive.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:101](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L101>) — public string ProjectInfoFilePath
- [Project.cs:376](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L376>) — Directory.GetFiles(Path, "*.csg")

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-003 CSP XML metadata

- [ ] Implement and verify the acceptance criteria below.

**Observed:** SaveProjectData uses XmlSerializer(Project); deserialization restores customer/contact/address/comments/default ring size and design list.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- PC -> iPad -> PC preserves all persisted metadata including non-ASCII text, empty fields and default ring size; compare actual XML semantics.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:426](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L426>) — public bool SaveProjectData()
- [Project.cs:458](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L458>) — private bool DeserializeProjectInfo()

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-004 Base and variant filename identities

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Project population parses <BaseDesignInstance>_<DesignInstance>.csg; equal GUIDs mark a base design.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Import multiple base designs and variants with colliding friendly names; maintain both GUIDs and parent/history membership after round trip.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:383](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L383>) — fileNameWithoutExtension.Split

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-005 Design-specific metadata

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Design has serialized properties and computed/XmlIgnore preview/history/path properties; not every visible field is part of XML.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Compare each persisted Design property while allowing intentional recomputation of XmlIgnore values; retain component and iteration identifiers.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Design.cs:135](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs#L135>) — public Guid BaseDesignInstance
- [Design.cs:168](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs#L168>) — public string CSUserDesignName

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-006 Relative paths and Windows path reconciliation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Project population splits Windows paths and reconstructs relative design locations under CSProjectPath.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Move between Windows drive letters and iPad sandbox locations, then return to a different PC root; no dangling absolute path, separator or filename-case references.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:407](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L407>) — item.Path.Split('\\')

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-007 Portfolio copy semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CopyProject clones XML, copies sibling files, remaps design paths and clears customer fields intentionally.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Copy on each platform and compare exactly which identifiers/history/files persist and which customer fields intentionally reset.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [ProjectManagementHelper.cs:488](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/ProjectManagementHelper.cs#L488>) — public static Project CopyProject

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-008 Portfolio rename semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RenameProject moves the directory, removes the old named .csp and saves metadata at the new location.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Rename a populated portfolio, reopen it and return it to PC; design, preview and history links must remain valid.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [ProjectManagementHelper.cs:759](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/ProjectManagementHelper.cs#L759>) — public static bool RenameProject

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-009 Missing CSP recovery

- [ ] Implement and verify the acceptance criteria below.

**Observed:** PopulateProjectDesignsCollection discovers .csg files, reconciles XML and recreates metadata when deserialization is unavailable.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Import a directory with valid designs but missing/damaged .csp into a copy; report recovered vs unavailable metadata without overwriting the original.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:403](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L403>) — Recreating missing CSP

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-010 Missing design reconciliation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Project population removes design records whose files are absent and saves the collection.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Present absent sibling designs as explicit losses before committing an import; preserve original bundle and document the Windows reconciliation difference.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:421](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L421>) — list.RemoveAll

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-011 Preview and favorite image sidecars

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Design computes base SPI, favorite and favorite-orientation PNG filenames from GUID-derived names.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Transfer a project with all sidecar orientations/favorites; compare thumbnails, favorite flags and regeneration behavior after PC reopen.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Design.cs:46](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs#L46>) — public string BaseDesignImagePath
- [Design.cs:76](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs#L76>) — public string FavoriteImagePath

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-012 Iteration-selection reset behavior

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Project deserialization sets SelectedIterationId to null while restoring the rest of each design record.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Distinguish saved design options from transient selected iteration; reproduce expected default selection without deleting stored alternatives.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [Project.cs:486](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Project.cs#L486>) — d.SelectedIterationId = null

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-013 Dirty save/cancel workflows

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ContinueAfterSaveDesignPrompt and portfolio prompts distinguish accepted, cancelled and discarded edits.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- For every close/new/open/rename path test Save, Discard and Cancel; PC-compatible files must contain the intended committed design.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [ProjectManagementHelper.cs:395](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/ProjectManagementHelper.cs#L395>) — public static bool ContinueAfterSaveDesignPrompt
- [ProjectManagementHelper.cs:426](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/ProjectManagementHelper.cs#L426>) — public static bool ContinueAfterSaveNewPortfolioPrompt

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-014 CSB nested backup packaging

- [ ] Implement and verify the acceptance criteria below.

**Observed:** BackupHelper builds CSCommonAppDataContent.zip and an outer .csb with distinct project/data zip payloads.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Inspect and restore representative PC .csb backups into an isolated destination; retain project files, catalogs, settings and app-state distinctions.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [BackupHelper.cs:20](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/BackupHelper.cs#L20>) — AppDataBackupFileName
- [BackupHelper.cs:196](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/BackupHelper.cs#L196>) — CS_{1}_Backup_

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-015 Restore extension and database handling

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Restore paths distinguish .s3db/.csgl/.zbin and project data during restoration.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Verify schema migration and overwrite decisions independently for app DB, catalog DB, library packs and projects; no silent cross-version replacement.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [BackupHelper.cs:352](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/BackupHelper.cs#L352>) — case ".s3db":
- [BackupHelper.cs:357](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/BackupHelper.cs#L357>) — case ".zbin":

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-016 CSG custom OpenNURBS archive

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Binary CSG Write emits 3dm properties/settings, custom UUID and custom tables/graph. It is distinct from ordinary 3DM geometry export.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Open/edit/save a real CSG on iPad and reopen it in PC with its construction graph; a geometry-only 3DM/STL conversion is a failure.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:472](<../../../recovery/canonical/cad/CS/CSDocument.cs#L472>) — onBinaryFile.Write3dmStartSection
- [CSDocument.cs:508](<../../../recovery/canonical/cad/CS/CSDocument.cs#L508>) — WriteCSFileInformation(onBinaryFile)

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-017 Four custom container UUIDs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSDocument.Read accepts A31784F3, DD8ECCF8, 847D0609 and B237EE6C envelope UUIDs; two routes include an old display-group table.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Maintain one fixture per accepted container UUID and legacy table layout; test rejection of unrecognized markers without file mutation.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:816](<../../../recovery/canonical/cad/CS/CSDocument.cs#L816>) — A31784F3-23DF
- [CSDocument.cs:820](<../../../recovery/canonical/cad/CS/CSDocument.cs#L820>) — DD8ECCF8-9380
- [CSDocument.cs:851](<../../../recovery/canonical/cad/CS/CSDocument.cs#L851>) — new CSDisplayGroupTable().Read

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-018 Archive length widths and old headers

- [ ] Implement and verify the acceptance criteria below.

**Observed:** The available CSG sample is archive header 4, while a bundled ordinary 3DM prop is header 50; record/chunk width handling must be version-aware.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Verify 32/64-bit chunk-length representations, nested boundaries, end marks and CRC rules with independent archive fixtures; current byte probe is not a full decoder.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:110](<../../../recovery/canonical/cad/CS/CSDocument.cs#L110>) — Read3dmStartSection(ref version
- [CSDocument.cs:472](<../../../recovery/canonical/cad/CS/CSDocument.cs#L472>) — onBinaryFile.Write3dmStartSection

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-019 File build and provenance records

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ReadCSFileInformation reads build date/revision and designer GUID chain; the current designer may be appended and revision metadata may change.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Preserve provenance semantics and document intentional editor/revision changes; use semantic rather than unconditional byte-identical comparisons.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:258](<../../../recovery/canonical/cad/CS/CSDocument.cs#L258>) — public bool ReadCSFileInformation
- [CSDocument.cs:533](<../../../recovery/canonical/cad/CS/CSDocument.cs#L533>) — public bool WriteCSFileInformation

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-020 UTF-16, numeric and binary primitive layout

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Archive readers use typed strings, UUIDs, integers, bytes and doubles; class-specific readers specify ordering.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Golden-byte tests cover non-ASCII strings, empty values, UUID endian layout, numeric precision, array lengths, booleans and geometry blobs.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSUtilIO.cs:127](<../../../recovery/canonical/cad/CS/CSUtilIO.cs#L127>) — public static bool ReadArray(ref Guid[]
- [CSObject.cs:895](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L895>) — public virtual bool Read(ref OnBinaryArchive

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-021 JSON and GZip alternative document paths

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSDocument public Read/Write has .json/.gz branches. Their existence does not prove user-visible import UI or equivalence for every derived node.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Round trip each exposed/supported route, large UTF-8/GZip files and truncated streams; do not advertise it as a lossless CSG replacement before field coverage passes.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:229](<../../../recovery/canonical/cad/CS/CSDocument.cs#L229>) — if (text != ".json" && text != ".gz")
- [CSDocument.cs:246](<../../../recovery/canonical/cad/CS/CSDocument.cs#L246>) — using GZipStream gZipStream

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-022 JSON root sections

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Writer/reader handle FileInformation, DocumentIterations, MaterialGroups, ImportedObjects, DocumentUserData, GemTable, CategoryTable and Graph; preview read behavior differs.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Compare every root section after round trip; preserve or explicitly regenerate preview data and validate all graph-reader failures.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [JSonDocumentWriter.cs:33](<../../../recovery/canonical/cad/CS.JSonDoc/JSonDocumentWriter.cs#L33>) — outObj.Add("PreviewBitmap"
- [JSonDocumentReader.cs:12](<../../../recovery/canonical/cad/CS.JSonDoc/JSonDocumentReader.cs#L12>) — public static bool ReadCSFile

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-023 JSON read-result propagation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Public CSDocument.Read invokes JSonDocumentReader then returns true without propagating its graph result; unknown JSON types can fail lower down.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- A failed/unknown JSON graph must not be reported as a successful editable import or silently saved over its source; record this deliberate fail-closed deviation.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:251](<../../../recovery/canonical/cad/CS/CSDocument.cs#L251>) — JSonDocumentReader.ReadCSFile
- [CSParaGraph.cs:328](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L328>) — Unable to find type:

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-024 CSGL five-record container

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGLLibrary reads five compression/range descriptors and supports Deflate and LZMA; entry 4 is exported as a CSG design.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Decode bounded .csgl fixtures with both compression modes, verify all declared lengths and graph payloads, then load/edit the extracted CSG.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSGLLibrary.cs:195](<../../../recovery/canonical/desktop/DesignLibrary/CSGLLibrary.cs#L195>) — private static byte[] GetByteArray
- [CSGLLibrary.cs:149](<../../../recovery/canonical/desktop/DesignLibrary/CSGLLibrary.cs#L149>) — GetByteArray(text, 4)

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-025 ZBin and generated catalog database

- [ ] Implement and verify the acceptance criteria below.

**Observed:** LibraryPacker reconstructs SQLite metadata from ZBin; file packs and metadata catalog are distinct data requirements.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Import compatible metadata and all referenced design versions; verify IDs/filter metadata/thumbnail links rather than inferring catalog completeness from pack presence.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [LibraryPacker.cs:12](<../../../recovery/canonical/desktop/DesignLibrary/LibraryPacker.cs#L12>) — public static void CreateSqLiteDBFromZBinFile
- [LibraryPacker.cs:89](<../../../recovery/canonical/desktop/DesignLibrary/LibraryPacker.cs#L89>) — public static void GetDesignsFromZBinFile

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-026 Account-specific catalog selection

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSLibraryHelper requests GetMyLibraryZBin(GetKeyId()) when the local manifest is absent.

**Offline disposition:** Needs local data provisioning; the original first-time service dependency cannot execute offline.

**Acceptance:**

- Provision an actual supported offline catalog before first use; no fabricated key IDs or mandatory online fetch; verify the selected catalog matches its PC source.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSLibraryHelper.cs:115](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Utilities.Helpers/CSLibraryHelper.cs#L115>) — GetMyLibraryZBin

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-027 Declared type GUID catalog

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RegisterCSBlocks scans nonabstract CSObject descendants with CSBlockAttribute; the static matrix contains 654 unique declared GUIDs.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Map every declared GUID to an on-device factory and acceptance case; reconcile source catalog against actual runtime registration before claiming full type coverage.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSApplication.cs:491](<../../../recovery/canonical/cad/CS/CSApplication.cs#L491>) — BlockList.Add(new CSBlockDescription
- [CSBlockDescription.cs:25](<../../../recovery/canonical/cad/CS.Parametric/CSBlockDescription.cs#L25>) — public CSObject GetNew(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-028 Legacy node algorithm retention

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Legacy namespaces and Obsolete attributes identify old nodes that remain independently registered; one current builder is not automatically an equivalent substitute.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Cover all 272 declared legacy/obsolete node types in fixture coverage, including their historical algorithms, parameters and outputs; no GUID substitution without comparison.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSApplication.cs:494](<../../../recovery/canonical/cad/CS/CSApplication.cs#L494>) — Registering Blocks Complete
- [CSParaGraph.cs:443](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L443>) — BlockDescriptionSearch(list, guid)

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-029 Node record version dispatch

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSObject uses 2.4 record versions; CSValue switches between 3.0/5.0 based on CAM and reads older branches.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- For each of 654 types record effective inherited/overridden readers and test supported old/current versions, non-default fields and unsupported versions.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSObject.cs:1071](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L1071>) — protected virtual void MajorMinorVersion
- [CSValue.cs:275](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L275>) — protected override void MajorMinorVersion

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-030 Two-pass node allocation and links

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Graph reads a type GUID array, allocates node instances, then reads per-node payloads using the complete graph.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Reconstruct references only after all supported nodes exist; verify forward references, connection ordering and dependency evaluation against PC.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:484](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L484>) — public bool ReadGraphItems(OnBinaryArchive
- [CSBlockDescription.cs:28](<../../../recovery/canonical/cad/CS.Parametric/CSBlockDescription.cs#L28>) — BlockClass.GetConstructor

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-031 Instance GUIDs and old-ID regeneration

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSObject reads InstanceGuid for major >=2 and generates a new GUID when absent/invalid.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Preserve existing valid IDs and explicitly match old-format migration semantics; do not require byte-identical provenance after intentional GUID generation.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSObject.cs:939](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L939>) — if (major >= 2)
- [CSObject.cs:943](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L943>) — if (!Guid.TryParse(str

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-032 Channel ordering and expandable inputs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSBlock reads object/channel index arrays and grows expansion/old group inputs when serialized counts exceed constructor inputs.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Exercise multi-input, unconnected, expandable and legacy-group nodes; compare link endpoints, order and missing-input diagnostics.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSBlock.cs:437](<../../../recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs#L437>) — public override bool ReadJson
- [CSBlock.cs:392](<../../../recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs#L392>) — CreateExpansionInputChannel()

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-033 Static input/output links

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Static connection information is persisted in SuperView/StaticInputData and StaticOutputData with graph indices.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- After reorder/import/partial export, verify static endpoints, socket flags and output indices still reference the intended nodes.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSBlock.cs:579](<../../../recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs#L579>) — public void SetStaticUserData()
- [CSBlock.cs:407](<../../../recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs#L407>) — base.UserData.FindValue("SuperView", "StaticInputData")

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-034 Parent and import-offset remapping

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Parent data and imported graph links are remapped through Application.ImportOffset and category/import-group identifiers.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Import the same part twice into a populated document and retain distinct parent/group/connection identities without collisions.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSObject.cs:927](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L927>) — CSApplicationHelper.Application.ImportOffset > 0
- [CSBlock.cs:377](<../../../recovery/canonical/cad/CS.CSObjects.CSBlocks/CSBlock.cs#L377>) — num += CSApplicationHelper.Application.ImportOffset

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-035 Unknown binary node fallback

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Binary graph reader tries a zero-GUID fallback for a missing type; CSNullBlock reports Block No Longer Supported. The source branch also attempts external developer telemetry.

**Offline disposition:** Exact unsupported-node recovery is unresolved; native telemetry cannot be preserved in a fully offline app.

**Acceptance:**

- Detect unsupported types before destructive save; retain the original bytes or refuse editable round trip. Do not silently replace a foreign GUID or emit external telemetry offline.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:518](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L518>) — Expired Block loaded with Guid
- [CSNullBlock.cs:23](<../../../recovery/canonical/cad/CSBlocks/CSNullBlock.cs#L23>) — Block No Longer Supported

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-036 Unknown JSON types and partial graph state

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ReadGraphItemsJson returns false on missing type; public document path may ignore the result.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Test unknown GUID, bad version, missing payload and partial allocation; import must expose incomplete editability and must not overwrite the source.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:427](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L427>) — public bool ReadGraphItemsJson
- [CSDocument.cs:251](<../../../recovery/canonical/cad/CS/CSDocument.cs#L251>) — JSonDocumentReader.ReadCSFile

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-037 Arbitrary user-data triples

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSUserData stores ordered Type/Key/Value records and supports XML-serialized values inside them.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Retain unknown keys, Unicode values and embedded XML, including keys not currently interpreted by the iPad UI.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSUserData.cs:210](<../../../recovery/canonical/cad/CS.Parametric/CSUserData.cs#L210>) — public bool Read(ref OnBinaryArchive
- [CSUserData.cs:89](<../../../recovery/canonical/cad/CS.Parametric/CSUserData.cs#L89>) — public bool AddSerializedObject

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-038 Numeric, choice and CAM value semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSValue supports delta vs absolute CAM values, notes and older record branches; other value/handle types have independent contracts.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Round trip non-default typed values and CAM notes across supported versions; distinguish displayed/real/manufacturing values and test source rounding/units.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSValue.cs:295](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L295>) — public override bool Read(ref OnBinaryArchive
- [CSValue.cs:414](<../../../recovery/canonical/cad/CS.CSValues/CSValue.cs#L414>) — archive.WriteBool(_absoluteCAM)

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-039 Imported/freehand group serialization

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ImportedCSGParts is XML-serialized into ImportedObjects/Freehand user data for binary save and JSON for the alternate route.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Round trip identifiers, part groups, ownership, linked imported nodes and categories through repeated import/duplicate/delete operations.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:984](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L984>) — public void WriteImportedObjectData()
- [CSParaGraph.cs:979](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L979>) — public JArray WriteImportedObjectJson()

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-040 Category table and import identifiers

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSUICategoryTable embeds [IMPORTGUID] markers and constructs category indexes used by controls and imported parts.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Verify category labels/order, index remapping, imported IDs and parameter-panel membership after PC reopen.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSUICategoryTable.cs:126](<../../../recovery/canonical/cad/CS/CSUICategoryTable.cs#L126>) — public bool Read(ref OnBinaryArchive
- [CSUICategoryTable.cs:139](<../../../recovery/canonical/cad/CS/CSUICategoryTable.cs#L139>) — [IMPORTGUID]

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-041 Iterations and typed option graphs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** GCIterations persists XML in user data and uses JSON TypeNameHandling.Auto for option objects.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Preserve option subclasses, namespaces/type discriminators, GUID references, alternatives and selected outputs without requiring CLR type activation on iPad.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [GCIterations.cs:246](<../../../recovery/canonical/cad/CS.Iteration/GCIterations.cs#L246>) — public bool WriteToUserData
- [GCIterations.cs:260](<../../../recovery/canonical/cad/CS.Iteration/GCIterations.cs#L260>) — jsonSerializer.TypeNameHandling = TypeNameHandling.Auto

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-042 Embedded Rhino geometry and JSON asymmetry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSRhinoObjectsFromBoundingBox binary read/write stores native 3dm objects; it has no own JSON overrides, unlike its binary payload methods.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Load a fixture containing embedded objects, save through every proposed interchange route and compare editable/source geometry. Inherited base JSON alone is not accepted as lossless.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSRhinoObjectsFromBoundingBox.cs:88](<../../../recovery/canonical/cad/CSBlocks.Core/CSRhinoObjectsFromBoundingBox.cs#L88>) — public override bool Read(
- [CSRhinoObjectsFromBoundingBox.cs:174](<../../../recovery/canonical/cad/CSBlocks.Core/CSRhinoObjectsFromBoundingBox.cs#L174>) — public override bool Write(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-043 RMA/RhinoCommon interop conversion

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSConvert bridges legacy OnCurve/OnBrep/OnMesh/OnSurface and RhinoCommon types and transforms.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Compare orientation, knot vectors, trims, periodicity, ownership and precision through the proposed on-device kernel and back to PC.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSConvert.cs:230](<../../../recovery/canonical/cad/CSCompatibility/CSConvert.cs#L230>) — public static Brep Convert(this OnBrep
- [CSConvert.cs:142](<../../../recovery/canonical/cad/CSCompatibility/CSConvert.cs#L142>) — public static Transform Convert(this OnXform

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-044 Gem definition table and reload semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSGemInstanceDefinitionTable has version 1.0 records and references gem shapes/sizes; reloading warns that exclusions may be removed when table versions change.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Preserve embedded/source gem definitions and exclusions; distinguish opening unchanged from explicit library reload and compare geometry after both.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSGemInstanceDefinitionTable.cs:151](<../../../recovery/canonical/cad/CS/CSGemInstanceDefinitionTable.cs#L151>) — public bool Read(ref OnBinaryArchive
- [CSDocument.cs:328](<../../../recovery/canonical/cad/CS/CSDocument.cs#L328>) — public void ReloadGems()

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-045 Font reconciliation and text geometry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** PostProcessFile checks missing/unrecognized fonts and warns that a saved design may change when fonts cannot be restored.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Transfer text/engraving cases with available and missing fonts; preserve source font identifiers and report substitutions before producing changed manufacturing geometry.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:564](<../../../recovery/canonical/cad/CS/CSDocument.cs#L564>) — private static void CheckFontSourceErrors()
- [CSDocument.cs:663](<../../../recovery/canonical/cad/CS/CSDocument.cs#L663>) — private bool PostProcessFile(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-046 Layer-to-material migration

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSObject maps old LayerNumber values to material classifications and falls back to UseOriginal when values are unknown.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Cover old/new material classification values, mixed primary/secondary metals and unknown enums; compare displayed and exported assignments.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSObject.cs:900](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L900>) — if (major <= 2 && minor <= 3)
- [CSObject.cs:907](<../../../recovery/canonical/cad/CS.CSObjects/CSObject.cs#L907>) — GetMaterialClassifictionByLayer

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-047 Material groups and overrides

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Binary save serializes material groups into document user data; load restores them before imported-object processing.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Preserve group membership, gem/metal classification and per-object overrides; render and export the same assignments after round trip.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:518](<../../../recovery/canonical/cad/CS/CSDocument.cs#L518>) — MaterialGroupHelper.SaveMaterialGroups
- [CSDocument.cs:865](<../../../recovery/canonical/cad/CS/CSDocument.cs#L865>) — MaterialGroupHelper.LoadMaterialGroups

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-048 Saved display component visibility

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSDocument writes CurrentComponents and FreehandComponentsVisible to document user data.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Reopen with matching center/side/accent/freehand visibility and distinguish display-only settings from objects excluded in manufacturing export.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:514](<../../../recovery/canonical/cad/CS/CSDocument.cs#L514>) — "CurrentComponents"
- [CSDocument.cs:515](<../../../recovery/canonical/cad/CS/CSDocument.cs#L515>) — "FreehandComponentsVisible"

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-049 Active/selected subset serialization

- [ ] Implement and verify the acceptance criteria below.

**Observed:** WriteGraphItems filters Deleted nodes or selected nodes, while graph/category indices and user/static data are prepared for the output.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Save full design, active-only and selected-part subsets with external dependencies; prove all retained references are valid or explicitly warn about missing dependencies.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:910](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L910>) — public bool WriteGraphItems(
- [CSDocument.cs:509](<../../../recovery/canonical/cad/CS/CSDocument.cs#L509>) — Graph.SetIndexesOfGraphItems(activeOnly: true, selection)

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-050 Write-time graph cleanup and mutation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Save removes deleted graph entries, cleans iterations, sets/removes category indexes and resets graph indexes afterward.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Compare pre/post-save graph behavior, identifiers and undo state; failure/cancellation must not leave temporary indices or partial files.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:521](<../../../recovery/canonical/cad/CS/CSDocument.cs#L521>) — Graph.RemoveDeleted()
- [CSDocument.cs:156](<../../../recovery/canonical/cad/CS/CSDocument.cs#L156>) — Graph.CategoryTable.RemoveIndexes()

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-051 Read-file-values versus full replacement

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ReadFileValues reads a temporary document then applies SetFileValue by graph position and optionally records undo.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Use matched and mismatched graph structures to verify value-only loading does not masquerade as complete import; preserve documented behavior and warn on incompatibility.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:296](<../../../recovery/canonical/cad/CS/CSDocument.cs#L296>) — public void ReadFileValues(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-052 Transaction and edit history semantics

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSUndoTable maintains runtime undo/redo transactions and per-object snapshots; portfolio history files are a separate persistence mechanism.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Test edit/undo/redo within a session and variant-history persistence after reopen separately; do not claim that all undo history is serialized into CSG without evidence.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSUndoTable.cs:205](<../../../recovery/canonical/cad/CS/CSUndoTable.cs#L205>) — public bool UndoLast(
- [Design.cs:59](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/Design.cs#L59>) — public ObservableCollection<Design> DesignHistories

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-053 Stored CSharp source and typed outputs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CSScript serializes source/configuration, compiles it through CSharpCodeProvider and invokes ScriptClass methods/ref outputs.

**Offline disposition:** Arbitrary PC .NET/Windows scripts cannot be promised on iPad; preserve data and declare the supported execution subset.

**Acceptance:**

- Round trip source and configuration without execution, then test each supported script API/output on device. An iPad-compatible interpreter/compiler model and geometry API mapping are unresolved.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSScript.cs:90](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L90>) — public override bool Read(
- [CSScript.cs:749](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L749>) — new CSharpCodeProvider
- [CSScript.cs:858](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L858>) — CompileAssemblyFromSource

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-054 External CLR and Windows assumptions

- [ ] Implement and verify the acceptance criteria below.

**Observed:** The recovered script builder imports System.IO and uses CLR compilation/reflection; project scripts can rely on APIs unavailable on iPad.

**Offline disposition:** Platform contradiction for unrestricted Windows-specific scripts; no silent replacement by a formula-only node.

**Acceptance:**

- Use fixtures with filesystem/reflection/external references and verify safe, explicit unsupported results; a preserved script string is not execution parity.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSScript.cs:837](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L837>) — using System.IO;
- [CSScript.cs:870](<../../../recovery/canonical/cad/CS.CSValues/CSScript.cs#L870>) — CreateInstance

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-055 Editable project versus STL/3DM geometry

- [ ] Implement and verify the acceptance criteria below.

**Observed:** RhinoServices exposes separate STL and 3DM exports, distinct from custom CSG DocumentWrite.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Test editable PC project round trip separately from geometry-only manufacturing export; retained topology alone does not pass graph compatibility.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [RhinoServices.cs:1954](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L1954>) — public bool ExportDesignAsSTL
- [RhinoServices.cs:1987](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L1987>) — public bool ExportDesignAs3DM
- [RhinoServices.cs:650](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L650>) — public void DocumentWrite(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-056 Manufacturing selection and mesh repair

- [ ] Implement and verify the acceptance criteria below.

**Observed:** STL export includes selected center/side/accent options and optional repair through native helpers.

**Offline disposition:** Requires equivalent local geometry/repair implementation; Windows DLL reuse is not an iPad solution.

**Acceptance:**

- Validate component inclusion, unit scale, watertightness, normals, repair-induced changes and source mesh tolerances against PC output.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [RhinoServices.cs:1954](<../../../recovery/canonical/cad/CounterSketchServer/RhinoServices.cs#L1954>) — public bool ExportDesignAsSTL

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-057 Semantic PC -> iPad -> PC fixture suite

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Reader/writer migrate IDs and revision data; complete parity needs graph and behavioral comparisons, not only file hashes.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- For every supported PC version/format/GUID/version branch: open unchanged, edit one parameter, save, PC reopen/recompute/export; compare graph data, identities, dimensions, volume, materials, categories, options and output geometry.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSDocument.cs:113](<../../../recovery/canonical/cad/CS/CSDocument.cs#L113>) — _fileProperties.m_RevisionHistory.NewRevision()
- [CSParaGraph.cs:910](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L910>) — public bool WriteGraphItems(

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.

## PC-058 Unsupported and corrupt projects

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Graph and document readers expose failure paths and partial processing; error propagation is not uniform.

**Offline disposition:** Reimplement locally; no remote dependency needed for this data contract.

**Acceptance:**

- Test truncation, bad CRC/lengths/counts, unknown UUID/GUID, unavailable fonts/assets, malformed XML/JSON and interrupted save on copies; never imply complete editability after data loss.

**Dependencies:** See source and PC compatibility requirements.

**Evidence:**

- [CSParaGraph.cs:262](<../../../recovery/canonical/cad/CS/CSParaGraph.cs#L262>) — Read Graph Items Version Number
- [CSDocument.cs:128](<../../../recovery/canonical/cad/CS/CSDocument.cs#L128>) — Invalid File Format

**Unresolved:** No iPad implementation/runtime or PC round-trip verification.
