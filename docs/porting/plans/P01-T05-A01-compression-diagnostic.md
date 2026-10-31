# P01-T05-A01 bounded recovered compression failure localization

Independent diagnostic while macOS runners are queued. Full viewport and whole-port acceptance remain open. This supports P01-T05/B-004 and file compatibility; it does not change recovered algorithms or assert original binary equivalence.

Read-only baselines: recovery/canonical/desktop/GV.CounterSketch.Studio.ZipIt and hash-locked src/Recovered/desktop. Existing Smoke.Checks records an empty-frame and100000-byte interoperability problem but catches exceptions without a full stage/stack report.

Owned files: tools/CompressionProbe project/Program.cs; diagnostic evidence under docs/porting/evidence/P01-T05-A01.json. Local output stays .local/evidence/compression. Use existing net10 desktop project reference with explicit alias; invoke only the already-exercised managed compression APIs. No original installer/application/native/runtime/account execution.

Measure bounded payload lengths0,1,32,1024,32768,65536,100000; repeated/text/seeded random content; recovered compression, BCL decoding of recovered bytes, recovered decoding of BCL bytes as separate stages. Preserve input/packed SHA256, sizes, exact exception/stack and source hashes. A stage failure remains visible in machine-readable output; do not label the matrix a compatibility pass. Do not mutate inputs or recovered code.

First reproduce known failure, then inspect the exact recovered method bodies at its stack. Original-versus-recovery IL/binary comparison is a separate evidence gate before changing behavior. Analysis acquisition validates actual artifact contents instead of inventing unit tests. Run repository source/security/link checks, review scope and commit/push the diagnostic with explicit unresolved limits.
