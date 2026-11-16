# GDI RGB preparation boundary

[Complete RGB parent](plans/P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-rgb-preparation.md) retains HALFTONE reconstruction and composed source integration. [Integer implementation](evidence/P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S06-A01.json) is one required child.

Rhino5GdiIntegerStretch.ResizeStored accepts positive-height BI_RGB24/32 DIB bytes in stored bottom-row-first BGR(A) order. The source length must exactly match DWORD-aligned rows. The output preserves source channel bits and32bit alpha behavior for this GDI stage; destination padding retains the explicit destination seed. Input is never modified. This API does not accept the top-row-first RGBA provider buffers directly.

Modes1 BLACKONWHITE and3 COLORONCOLOR use measured current-host Windows rules. If both dimension differences are at most1, both axes select direct clamped coordinates. Otherwise selection is floor((2*i+1)*source/(2*destination)); mode1 compression ANDs bytes from the previous selected index+1 (first0) through the current selected index. That span can omit a trailing source tail; native last-pixel controls preserve it. Do not replace it with floor-partitioned boxes or ordinary nearest everywhere.

SelectTextureMode implements the original texture wrapper integer-ratio boundary: if either source/destination quotient reaches2, choose HALFTONE4; otherwise retain the supplied current mode (fresh measured public DCs use1). ResizeStored explicitly rejects HALFTONE4 and WHITEONBLACK2. There is no bilinear or mode3 fallback. Palette/profile/negative-height/conversion support remains open. Explicit checked allocation/storage limits guard managed code rather than reproducing native invalid-memory behavior.

Original32bit alpha OR repair is a separate already-implemented stage. It must be composed after RGB filtering only where the original caller invokes it, using the original stored-row mapping; do not substitute alpha replacement or unpremultiplication. Full trace/wallpaper/cache/document integration remains pending.

Evidence: [original public-GDI corpus](../../reference/fixtures/gdi-rgb-windows-26300.json), [expanded integer corpus](../../reference/fixtures/gdi-integer-windows-26300.json), [macOS results](../evidence/apple/gdi-integer-preflight-results.json). Independent actual Win32 output comprises1440 new cases/1824 controls and156 applicable previous cases. Portable comparisons are byte-exact, including padding and alpha. These are current-host public GDI observations, not original vendor/MFC/OS/driver or physical-device parity.
