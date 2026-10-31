# P05-T02-A28 native BRep mesh snapshots

Goal: supply real regenerated geometry to the offline viewport, not cached installer previews. P05-T02/P05-T05 candidate prerequisite; original Rhino tessellation/material/render parity remains separate.

- RED missing CandidateContext.TessellateBrep.
- Mesh a deep copy of actual native BRep with explicit absolute chord/angle parameters, serial deterministic operation. Export counted world-space vertices/unit normals and zero-based triangle indices with mesh-local face ordinals. No new native persistent handles.
- Two-pass bounded query/fill: no partial writes on short buffers; verify dimensions/layout and detach immutable managed arrays. Face orientation controls winding/normals; all faces need real triangulations. No coarse fallback or preview/mesh substitute.
- Test analytic box volume/winding/normal/index bounds, rational cylinder geometry/normals, actual joined140 coarse/fine settings and edits/reopen, input/tolerance preservation, detached lifetime and error/resource guards.
- Native/shared/source/repository checks, macOS Actions, commit/push. Recovery/kernel read-only; all local output/cache D: and5-second transient retries.

Local result:9 cases/6 buffer guards pass; actual CAD coarse/fine triangle counts2196/108118, real edit/reopen regeneration and detached immutable payload. Apple tests pending; viewport/material/render/full parity not claimed.
