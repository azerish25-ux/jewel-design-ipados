# P05-T02-A26 native BRep sewing candidate

Source prerequisite for legacy node140, not registration or complete equivalence. CSJoinBrep1 uses OnBrep[] legacy JoinAllBreps at.01, not the modern IEnumerable<Brep> filter/single-result overload. SDK5 static binding forwards all pointers to single-pointer native RhinoJoinBreps; nonnull invalid result invokes repair. Input-modification/disconnected/repair semantics need independent reference, not a guessed modern policy.

- RED missing CandidateContext.SewBreps.
- Expose actual pinned OCCT sewing of copied BRep inputs with explicit tolerance and native part/face/free/multiple/contiguous/deleted/closed-shell diagnostics. Preserve disconnected native results honestly; never claim a compound has been stitched into one connected object.
- No source validity/solid prefilter, forced closure, solidization or Rhino-equivalent repair in this raw candidate. Input pointers/IDs counted and bounded; native handles owned; failed creation zero handle and retained inputs unchanged.
- Test analytic adjacent/disjoint/tolerance-separated faces and six-face closed shell; actual134/138/139 native geometry before/after edits and save/reopen. Inspect actual free edges/components, validity, input preservation and ownership/error cleanup.
- Run native/shared/repository/source checks; macOS Actions; commit/push. Node140 legacy graph policy remains separate, including empty/null/retained output and input mutation/repair behavior.

All local writes D:; wait5 seconds on transient errors. Recovery/kernel read-only.

Local outcome:10 cases/8 ABI guards pass; actual sweep/caps form one3-face closed shell with0 free edges across edits/reopens. Input copies and topology diagnostics are explicit. Legacy graph/private kernel semantics and Apple sewing proof remain open.
