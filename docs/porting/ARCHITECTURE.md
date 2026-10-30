# Architecture and boundary contracts

## Direction and ownership

Keep managed logic in C# and use .NET iOS/UIKit for the application. Native C++/C ABI and Metal are introduced where a genuine CAD/render backend needs them. Swift is optional. This direction is not proof that Rhino or the original renderer has been ported; P05 selects the backend using actual operation fixtures.

| Planned module                            | Responsibility                                           | Excluded responsibility            |
| ----------------------------------------- | -------------------------------------------------------- | ---------------------------------- |
| src/Port/Domain                           | identities, units, values, validation, materials         | Windows/UI/files/services          |
| src/Port/Graph                            | snapshots, scheduling, invalidation, undo                | native pointer ownership/dialogs   |
| src/Port/Documents                        | bounded codecs, legacy versions, opaque data, portfolios | geometry fabrication               |
| src/Port/Geometry + native/GeometryBridge | real kernel operations, ownership, tolerances            | UI/activation                      |
| src/Port/Features                         | adapted builders, catalog, commerce                      | hidden platform calls              |
| src/Port/Rendering + native/Rendering     | viewport, picking, materials, images                     | truth stored only in cached meshes |
| src/Port/Scripting                        | supported typed APIs, bounded execution                  | arbitrary Windows execution        |
| src/Port/Platform.iOS                     | UIKit, Files, lifecycle, gestures                        | serialized version decisions       |
| tests/Interop                             | independent PC/port comparisons                          | mocked success claims              |

## Contracts to implement in P03–P05

These are specified boundaries, not existing production APIs. Create the DTOs and focused tests before downstream use. Revise through an ADR if recovered behavior disproves a signature.

```csharp
public interface IProjectCodec {
    ProjectDocument Read(ReadOnlyMemory<byte> bytes, CodecReadOptions options);
    byte[] Write(ProjectDocument document, CodecWriteOptions options);
}
public interface IGraphEvaluator {
    EvaluationResult Evaluate(GraphSnapshot graph, EvaluationContext context,
                              CancellationToken cancellationToken);
}
public interface IGeometryKernel {
    KernelResult Execute(GeometryOperation operation, GeometryContext context,
                         CancellationToken cancellationToken);
}
```

ProjectDocument stores original GUID/version, graph links, numeric fields and unknown payloads independently of cached geometry. CodecReadOptions/CodecWriteOptions explicitly select supported versions and unknown-data behavior. GraphSnapshot is immutable and parameter identities survive UI/model renames. EvaluationContext injects real kernel access and document units. EvaluationResult contains diagnostics and generated geometry references. GeometryOperation is a typed operation union, not a string-success stub. KernelResult contains actual geometry handles or structured errors. GeometryContext records tolerance/units and native lifetime. P03/P05 must define these DTOs before implementing callers.

The native ABI is versioned extern-C with fixed-width values, opaque handles, explicit retain/release, structured errors and cancellation. Do not share managed object pointers or undocumented Windows/Rhino binary layouts.

Original assembly-qualified names may stop resolving when assemblies are renamed. Preserve GUID/version registries, explicit factories and legacy behavior. Reflection/XML/resources need actual AOT/trim cases. CSScript needs a supported offline execution strategy rather than assuming CodeDom works.

P05 records 315 operation families, candidate SDK versions/distribution terms, real iOS builds, Windows comparisons and gaps. Missing required operations remain blocked. External kernel source is not counted as recovered or implemented code.

Implemented capture-local addressing: [ADR 0001](ADR-0001-snapshot-parameter-addresses.md). Persistent identity across edits/load/save remains a separate codec/document contract.

Implemented scalar parameter planning: [ADR 0002](ADR-0002-numeric-value-plans.md). Plans do not execute graph changes or stand in for undo records.

Implemented portable undo scheduling: [ADR 0003](ADR-0003-portable-undo-journal.md). Actual native/parameter snapshot adapters remain unfinished.

Session parameter identity: [ADR 0004](ADR-0004-session-parameter-identity.md) defines raw/effective registration GUIDs and explicit identity mappings between captured views; codec persistence remains open.

Production legacy library codec: [ADR 0005](ADR-0005-legacy-csg4-codec.md). The file layer rebuilds records and preserves opaque data; application save still requires graph regeneration and explicit migration/sidecar policies.

Legacy scalar file-edit bridge and unknown-header contracts: [ADR 0006](ADR-0006-legacy-scalar-edit-bridge.md). Exporting archive data is separate from a regenerated application save.
