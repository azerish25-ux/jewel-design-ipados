# A05-P03-A01 wallpaper raster arithmetic

Requirement UI-039; parent A05-P03 remains open. Input commit dadbc516aa0d92358211d8e8b39fe589116d896d. Original Rhino.exe SHA256 951ff639f2c9b61bbf7c32a67169d0e23bdbd026f74ada80ace9b1f02b1b48eb. Source: CRhWallpaper::Resize RVA0xb333f0,970 bytes; exact function manifest native/Analysis/fixtures/rhino5-view-image-loading-regions.json.

Implement portable source-ordered sizing/divisor/aspect/truncation/width alignment and real in-place BGR24 storage grayscale. Preserve zero dimensions and distinguish recursive input rejection from attempted zero-size allocation. Nonpositive dimensions and signed32 product overflow reject explicitly in the managed API; no undefined native behavior emulation. Empty-filename success remains the caller's responsibility.

RED: missing Rhino5WallpaperRaster (Windows compiler exit1). Tests cover exact9M and strict divisor thresholds, portrait/landscape/square/both fit branches, odd width, zero truncation/reduced input, invalid/overflow domain, all16,777,216 RGB combinations against a JavaScript SHA256 generated from source SSE arithmetic, actual BGR mutation and untouched incomplete storage. No conventional0.587 green, rounding, gamma correction or FMA substitution.

Own src/Port/Rendering.Candidate/Rhino5WallpaperRaster.cs, common assertions and existing host/macOS/iPad/iPhone drivers. Keep actual CAD capture gates. Run Windows focused/native,57 adjacent Node checks, macOS numerics/bindings and actual iPad/iPhone execution through owner plugin. Original binaries remain static-only; local output D-only.

Still required: real local image resolution/decoding, GDI HALFTONE equivalence or a measured alternative, bitmap/cache lifetimes, Metal texture/display, hidden/failure transitions and full SetView/Pop/document/PC/physical acceptance. This child closes no parity row.

Result: [Windows/macOS/iPad/iPhone evidence](../evidence/P08-T01-A05-03f-02b-A05-P03-A01.json) verifies34 sizing/storage cases and all16,777,216 RGB triples per platform. Adjacent actual CAD captures reproduce with final-region difference0 and retired buffers0. This completes this arithmetic child only; parent image/decode/render work remains open.
