# P08-T01-A04 source-backed frustum/lens numerical port

Goal: preserve state-dependent published native frustum/lens behavior needed by the recovered50mm presets. Full UI-039/private Rhino5/PC/physical camera acceptance remains open. Existing C#/.NET iOS/UIKit/Metal architecture is approved; this is its numerical camera prerequisite.

Design: CandidateViewportFrustum owns immutable bounds/options snapshots and a raw validity flag. SetBounds, aspect, lens getter/setter and smallest-half-angle operations preserve state on failure, symmetry flags, diagnostic counts and source projection-cache invalidation. A separately compiled C++ reference uses exact pinned published method bodies with state-observation types. It is numerical source evidence, not original viewport/GPU execution.

Alternatives: generic PI/4 loses original behavior; generic vertical50mm assumptions miss limiting-axis and two-point rules. Port the actual state-dependent calculations before integrating projection/UI.

Source: mcneel/opennurbs pin f2dc7fba67017475b8d530835a40fcfb5372134a. Viewport SHA2403be6f12b9f3fa7c723da44793eda28c78e1e06d2ae7f2c529241c5b0498ce; point.cpp, math.h and defines.h individually hashed. ON_IS_VALID excludes both unset sentinels. World bounds at1e100 reject; perspective depth warnings still accept. Observe ON_ERROR and SHA1 reset without claiming a replacement native hash.

Children: P08-T01-A04-01 source/reference/cases; P08-T01-A04-02 C# adaptation and differentials; P08-T01-A04-03 source-preset/projection application and actual Metal frames after numeric proof.

Owned paths: native/Rendering/FrustumReference/reference.cpp; scripts/prepare-frustum-reference.cjs and generate-frustum-cases.cjs; Rendering.Candidate/CandidateViewportFrustum.cs; shared FrustumAssertions.cs/fixture and Rendering.Tests; audit/evidence/ADR. Recovery and CandidateProbe remain read-only, preserving existing native archive identity.

1. Hash dependencies; extract unique exact method bodies and relevant source macros. Compile reference to .local/native/frustum-reference using existing compiler/run-local.
2. Generate native cases: normal/asymmetric/all projection-symmetry-up flags; invalid/raw states; sentinel/NaN/inf; strict world bounds; accepted depth diagnostics; lens deadband boundary; angle/aspect failures. Compare return/out/bounds/validity/diagnostics/cache reset.
3. RED missing C# API; implement exact operations; GREEN independent source comparisons with tight finite tolerance and explicit NaN/inf/signed-zero handling.
4. Run adjacent tests/repository checks and macOS native/managed checks through Actions. Record source hashes/commands/warnings/review/limits and commit/push stable chunks. No full parity row closure.

All local work/cache/temp/auth/output D:;5-second retry on transient D: failures. Existing C: compiler executable is read-only tooling; no installation/config edits or original runtime execution. No subagents. No rendering change before numerical evidence. Self-review: source semantics preserved, independent real compiled arithmetic, explicit private/original/physical gaps.
