# ADR 0007: saved portfolio XML metadata and source-shaped serializer oracle

Status: accepted for independent P04-T03-A01; real PC portfolio/corpus acceptance remains open.

Recovered Project.SaveProjectData writes XmlSerializer(Project) to Name.csp. The declared serializable property shapes are 16 Project and 27 Design properties, plus ProjectType. PortfolioContractGenerator derives public read/write non-static non-XmlIgnore declarations with SDK Roslyn, rejects unmapped XML attributes/types, embeds source hashes and verifies generated test-only POCOs. This oracle does not execute desktop getters/setters/inheritance, WPF, filesystem mutations or original PC code. Fixtures are synthetic contract specimens, not recovered saved projects.

LegacyPortfolioXmlCodec keeps a private immutable DOM and original bytes. Untouched/same-text edits write the original bytes exactly. Targeted edits clone the DOM, preserve unknown namespaced nodes/attributes/comments/nested variants, and serialize UTF8/UTF16 with matching BOM profile. Unsupported encodings fail explicitly. Modified XML may change lexical quoting or namespace declarations; semantic unknown content is retained. Strings use omission for null, while nullable integers use xsi:nil; the source-shaped XmlSerializer tests caught and verify that distinction. Removing a string carrying opaque attributes/comments is rejected rather than dropping data. Complex/duplicate known scalar fields are not overwritten silently.

The parser prohibits DTD/external resolution, bounds bytes/characters/depth/elements and never follows paths or executes metadata. XML paths remain raw strings. Native load behaviors (path relocation, ProjectType forced to Portfolio, SelectedIterationId reset), clone GUID changes, favorite setter filesystem saves, filesystem timestamps/history ordering and folder reconciliation are not applied by the codec. The full application must adapt those behaviors separately.

LegacyDesignFileKey interprets baseGuid_designGuid.csg and the source preview names without host filesystem access. Extension matching follows Windows case-insensitive behavior; IsBaseDesign intentionally compares the two original stem strings ordinal/case-sensitively, as recovered PopulateProjectDesignsCollection does before GUID construction. StoredFileName handles both Windows and POSIX separators without resolving or rewriting a path. No sidecar image or history file is invented.

Shared tests deserialize edited output through XmlSerializer on the generated property contracts, retaining nullable iteration, 64-bit offsets, component arrays, nested variants and unknown extensions. They cover UTF8/BOM/UTF16 endian profiles, exact no-op, safe nil semantics, unsupported UTF32, DTD/resource limits, ambiguous/complex scalar edits and paired filenames. Reflection metadata for the three oracle types is explicitly rooted. Production parsing uses XmlReader/XDocument, not runtime model code generation.

The [.NET 10 XmlSerializer implementation](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.Xml/src/System/Xml/Serialization/XmlSerializer.cs) selects reflection-only mode when dynamic code is unavailable; the platform assertion reports RuntimeFeature.IsDynamicCodeSupported rather than assuming simulator/AOT equivalence. Device/physical execution still needs its own proof.

This child consumes source evidence and portable document foundations; it does not waive P02/P03 or P04 portfolio-folder/all-serializer/native geometry and PC/iOS/PC gates.

Modified output is revalidated with the same configured input limits before bytes are returned; a regression covers adding metadata exactly at the element budget. Earlier simulator runs must not be treated as evidence for a later guard change.
