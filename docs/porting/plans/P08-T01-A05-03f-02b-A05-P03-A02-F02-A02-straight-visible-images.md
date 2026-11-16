# F02-A02 lossless straight-alpha decode and visible image integration

Prerequisites: F01 preserves encoded bytes and exposes premultiplied sRGB RGBA; F02-A01 implements real GPU quads with explicit straight RGBA storage. Full F02 remains open. Requirement UI-039.

Do not silently unpremultiply F01 pixels: at8-bit precision multiple straight colors can produce the same premultiplied sample, and original GL linear filtering interpolates straight texels before source-alpha blending. Even transparent RGB contributes to straight-alpha filtering. Preserve original bytes and represent unsupported conversions explicitly.

Inspect actual Apple decoded CGImage provider layout (component count/bit depth/row stride/alpha/byte order/profile) and extract unassociated samples losslessly where the real provider representation supports it. Define exact raw-layout conversion cases and retain source metadata. Do not assume every ImageIO format returns the same layout or transform color profiles without source evidence. Test non-primary low-alpha colors and transparent colored texels, row order, palette/gray/16bit/profile cases and structured unsupported paths. No lossy conversion should be called1:1.

Consume [refined original draw ABI](../../evidence/audit/rhino5-image-drawing-abi-source.json), original texture cache helper and source format inventory. Resolve OGL texture-manager decode/gray/orientation/cache lifetime before claiming original texture equivalence. Source trace32bit grayscale can alter alpha; distinguish this from wallpaper24bit processing.

Integrate GPU encoding into the real visible CAD renderer with retained combined image/mesh revisions and command completion ownership. Implement actual source trace world-plane and wallpaper screen placement, including saved filtering/visibility, extent/aspect rules and exact verified raster offsets. Keep geometry regeneration as scene truth. Add separate visible image evidence without weakening or replacing existing six-stage CAD baseline.

Retain original replacement failure behavior and explicit missing-texture diagnostics. Wire real parent/document notifications, SetView/Pop/screen effects; no success callbacks or mock caches. Full format catalog, GDI filtering, original-PC and physical acceptance remain open until measured.

Native prerequisite evidence: [macOS provider observation](../evidence/P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-N01.json) shows exact straight RGBA for the two controlled PNGs, including low-alpha/transparent RGB. Original BMP provider uses skip-last32bit; JPEG provider observed. Implement the typed iOS adapter and test actual device-family layouts rather than unpremultiplying F01 output.

Original cache/upload prerequisites: [seven-body source audit](../../evidence/audit/rhino5-image-texture-cache-source.json) now records per-context/lowercase-name/kind matching, file-first then embedded fallback, settings-controlled anisotropy/mipmaps, normal GL2D upload and GDI resize/32bit alpha repair. Visible integration must preserve these policies; direct raw PNG/GPU success does not close them.
