# A05-P03-A02-F02 image texture and source drawing integration

Next implementation after [F01](P08-T01-A05-03f-02b-A05-P03-A02-F01-apple-decoder.md). Requirement UI-039 remains open. Consume [eight original drawing bodies](../../evidence/audit/rhino5-image-drawing-source.json), [bitmap storage ownership](../../evidence/audit/rhino5-bitmap-loading-source.json) and [format module inventory](../../evidence/audit/rhino5-image-codec-module-inventory.json).

Before implementation, refine imported GL and GDI call ABIs and raw arguments. Current decompilation drops arguments and uses opaque class-scaled offsets; it is not a complete draw contract. Resolve filtering, texture coordinates, raster placement, viewport clipping, blending/depth policy, texture manager gray/cache keys and pipeline override behavior. Preserve GDI/OGL distinctions instead of treating either as automatically the complete reference.

Source already establishes distinct roles:
- Wallpaper is screen-space. Stored downscale factor selects GDI BitBlt versus StretchBlt; OGL can use a clipped temporary DIB before screen pixel drawing.
- Trace is a world-plane quad with intervals starting0. OGL derives one missing extent from texture aspect, skips both zero extents and has a real missing-texture diagnostic quad/text path. Preserve failure diagnostics as failure UI; never substitute it for successful CAD rendering.
- Trace GetImage calls a different grayscale converter: request IncreaseColorDepth24;24-bit rows skip padding and add0.5 before truncation; existing32-bit path writes computed gray into all four channels including alpha. Wallpaper A01 is flat BGR storage with truncation and no0.5. OGL gray texture-cache conversion is still unresolved. Do not apply either blindly to F01 premultiplied RGBA buffers.

Implement genuine GPU texture upload/sampling with explicit color/alpha representation and retain resources through command-buffer completion. Integrate image replacement and mesh replacement without disposing textures/buffers still leased by earlier frames. Preserve empty/hidden/decode-failure transitions and the original stored metadata; source current/previous image state must not be collapsed into a success flag.

Verify actual decoded fixture texture sampling/readback, row orientation and alpha; image replacement while frames are pending, resize, missing/corrupt input, hide/show, clear, dispose with work in flight and zero retired owned resources. Add actual visible image evidence separate from the existing six-stage CAD baseline; keep its screenshots/strict pixel gates unchanged. Source original image presets, GDI filtering equivalence, full Files workflow, page/detail/parent/document/SetView and original-PC/physical acceptance remain mandatory.

Use source hashes and RED boundary/failure tests first; all local writes/caches D-only and connected owner GitHub plugin for publication/macOS/actual iPad/iPhone evidence. No original Windows DLL execution and no parity closure from resource-only tests.
