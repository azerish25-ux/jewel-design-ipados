# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-A03-A03-A01-N02-A02-N02-A03a: successful Unicode filename wrapper

UI-039; input b535e14f0a3b653614c7566a5321c29767024faa. Source: original core CompareNoCase RVA0x2e0d20, retained static view .local/evidence/source-cache/core-02/original-wstring-compare-no-case.c; public native locale1 corpus and tested direct/lower engines. Preserve right-null/first-NUL handling, left stored UTF16 length and terminated comparison; lowercase uses full stored length.

Add an explicit versioned successful Unicode profile to the actual filename/cache API, without switching existing renderer profile implicitly. Test null/empty/embedded-NUL matrix, full40663 native pairs with original wrapper, expansion/width cache hits, stored lowercase, invalidation/leases and unchanged CRT-C. RED: dotnet run --project tests/Port/Rendering.Tests fails CS0117 missing UnicodeNls1Reference60502.

Owned files: filename semantics, shared assertion, Windows/iOS runners, simulator gate and this evidence. Mac preflight and iPad/iPhone Actions required. Original NLS failure -> current CRT locale, ANSI conversion, locale mutation and source document/view cache adoption remain parent A03 prerequisites; this child does not claim those complete. Source binaries are never executed. All local writes D:, owner GitHub plugin for publication.
