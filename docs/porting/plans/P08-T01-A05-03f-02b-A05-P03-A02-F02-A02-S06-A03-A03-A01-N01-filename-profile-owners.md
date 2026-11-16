# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-A03-A03-A01-N01 original filename profile initialization owners

UI-039; input0c35bdbf1f1f938d54c244b8257a7687d7212f60. Cache runtime works with explicit CRT-C policy; original application locale/Unicode/ANSI/CRT runtime initialization remains unresolved. Inspect original PE imports, all linear-disassembly references to profile globals/setter and owning startup functions without executing original binaries. Pin target/function bytes, retain instruction evidence and distinguish known stores from unresolved runtime callers. Validate acquisition coverage before changing portable policy. Then define native reference/differential tests for any newly proven behavior; no guessed platform comparer or silent non-ASCII fallback.

Own static acquisition/audit tools and bounded manifests/evidence. Recovery read-only; all local outputs D; owner GitHub plugin only. Full document, PC and physical acceptance unchanged.

Static acquisition result: six full function regions validated against both original PE files; four main/two core Ghidra views have explicit observed ABIs. Complete linear disassembly captured15 core-global and16 main-profile references, including2 direct manager callers and1 imported core-setter call. Nonzero requested language selects Unicode NLS1. Failed UI-language loading updates the UI fallback field without resetting the core string profile. A formatting self-test calls setlocale(LC_CTYPE,".65001"); that does not establish startup CRT state. Core globals begin in zero-fill virtual tails; indirect/plugin calls remain open.

Reproduce on D:

    node scripts/run-local.cjs node scripts/inspect-filename-profile.cjs .local/evidence/filename-profile/new-scan.json
    node scripts/run-local.cjs node scripts/verify-filename-profile.cjs "D:\MAIN SOFTWARE\jewerly software\analysis\runtime-prerequisites\rhino5\System"

[Source findings](../../evidence/audit/rhino5-filename-profile.json), [captured references](../../evidence/audit/rhino5-filename-profile-references.json). No product default changes until exact portable Unicode NLS1 behavior and original fallback/state are implemented. Next: exhaustive versioned public NLS1 controls and portable mapping/collation; the previous .NET default approximation remains rejected.
