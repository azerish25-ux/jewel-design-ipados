# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-A03-A03-A01-N02-A02-N02-A03e: original ANSI conversion contract

UI-039; input7976d8e07901f1b2017f14182ef2c22aa099e0d6. Before runtime selection, resolve the actual original wide->byte helpers used by on_wcsicmp/MakeLower. Static export identity already proves calls to ON_ConvertUTF16ToUTF8, rather than presuming a device ANSI encoder. Query/write masks and malformed-input capacity behavior need typed whole-function review. Preserve ASCII bypass, exact locale/flags/counts, conversion/failure distinctions and C/stored-length rules.

Acquire complete hash-locked original PE regions using chained unwind ancestry, apply observed UTF16/UTF8 ABIs and inspect typed Ghidra outputs under D:. Original binaries never execute. This is source acquisition, so validate contents/hashes instead of inventing a RED unit test. Public Windows ANSI/reference measurements follow only after the source contract is known. No current-platform encoding or replacement fallback may be substituted speculatively.

Owned files: conversion region manifest, static contract verifier/tests, owned public Windows ANSI collector/fixture/verifier, macOS workflow and evidence; the observed missing-JDK launcher default is corrected to its existing D: JDK. Broader ANSI implementation, actual runtime selection, failure/errno and full document/view/PC/physical parity remain open. Use owner GitHub plugin only for remote publication and Apple CI for portable artifact verification.

Measured source:9 complete functions/46 chained regions/3751 original bytes locally checked, typed Ghidra views retained under ignored D:. Query is strict UTF16->UTF8; write and byte->wide decoder use all error bits masked with U+FFFD; ASCII bypass and C-prefix/sourceCount/capacity rules are explicit.

Measured public backend:CP1256/65536 repeated comparison pairs/65536 lower pairs/65536 decode pairs/1112064 valid scalar lower calls. No comparison/decode/lower composition disagreements; ACP1252 differs10460 pairs.27 byte mappings change50349 scalar encodings and create4096 invalid UTF8 outputs at U+10C000..U+10CFFF. Native full-plane digests reconstruct independently. These observations prohibit silently replacing the pipeline with standard Unicode lowercase/default UTF8 replacement.

Next: implement strict-prefix encoder, exact legacy masked decoder, CP1256 adapter and source ASCII/fallback/count routing; then combine with actual runtime state and source document/view cache ownership.

Actual source-bound macOS artifact verification reconstructs all65536 comparison/lower/decode controls and1112064 scalar lower digests, including4096 invalid UTF8 outputs.3 focused tests/13 adjacent Node checks/repository integrity pass. Local original PE bytes verified; remote source checking remains manifest-only and is labeled accordingly. The portable ANSI conversion/decoder/runtime implementation remains the next required work.
