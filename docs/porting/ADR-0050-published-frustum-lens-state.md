# ADR0050: source-backed numerical frustum and lens state

Status: local published-source differentials pass; Apple cross-compiler and projection integration remain separate gates.

CandidateViewportFrustum preserves a raw bounds/validity/options snapshot so invalid imported native state is not silently normalized. Its bounds setter, lens getter/setter, aspect and smallest-half-angle operations follow pinned published openNURBS6 methods. Symmetry, two-point conditions, strict1e100 world bounds, both unset sentinels and source warning-versus-failure distinctions are retained. Diagnostics and projection invalidation counts observe source side effects; they are not an implementation of native SHA1 projection hashing.

An independent C++ runner compiles exact extracted source bodies and macros. State accessors, ON_ERROR counting and the SHA1 reset marker are observation types only.1245 cases compare return/out values, stored state, raw validity, source diagnostics and reset behavior. NaN/inf and signed zero are compared explicitly. Native fixtures retain input flags and expected results; Windows and macOS compilers verify the same matrix. Source points/math/defines/viewport files and extracted method bodies are hash locked.

A successful source getter can return nonfinite output when its raw validity flag contradicts corrupt stored fields. This policy is preserved in this numerical layer; a GPU projection adapter must validate its resulting matrix explicitly. Lens changes inside the strict1e-6 scale deadband retain exact state and do not invalidate the projection cache. Accepted perspective-depth diagnostics do not become invented hard failures.

The recovered managed presets request50mm, but this module does not yet perform native camera movement, ChangeToPerspective/ParallelProjection, design-space zoom, viewport selection, matrices or Metal presentation. It also does not establish private Rhino5/original-PC/physical equivalence. Those remain full camera/UI acceptance requirements. [Work plan](plans/P08-T01-A04-frustum-lens.md).
