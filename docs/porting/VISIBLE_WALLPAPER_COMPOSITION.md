# Visible wallpaper composition boundary

CadMetalView.SetWallpaperPixels(image, storedScale) accepts owned, explicitly straight RGBA8/sRGB pixels corresponding to an already-resized wallpaper DIB. It snapshots pixels into a real GPU texture. The original wallpaper resize target is24-bit RGB; upload therefore keeps straight RGB and makes display alpha opaque. Original encoded asset bytes are unchanged. This API does not perform original file resolution, gray conversion or HALFTONE resampling.

The visible pass draws wallpaper with depth writes disabled before regenerated CAD geometry. CAD encoding restores its pipeline/depth/culling state. Original GDI placement centers image dimensions using signed truncating division, scaling destination dimensions only when storedScale>=2. Managed guards reject native signed32 overflow. This is the GDI draw-rectangle rule, not the unresolved complete GL placement/resampling path.

SetMesh replaces geometry while retaining the current wallpaper upload. SetWallpaperPixels replaces the image while retaining the current geometry upload. ClearWallpaper removes the image while retaining geometry. Each successful change publishes a new immutable scene snapshot; older frame leases keep independent mesh/image references until actual GPU completion. The frame gate permits at most3 pending submissions. Normal visible drawing does not read pixels back or synchronously wait on the GPU.

All public view mutations require the main UI thread. Stats distinguish unique mesh uploads/buffers, image textures and scene snapshots. Temporary camera/image-quad buffers and drawable references belong to each submission and are released with it. These are owned-resource accounting checks, not a process-wide driver-memory measurement.

The acceptance harness keeps the original six CAD screenshot stages unchanged, then records six separate paper-* stages. Its independent pixel verifier checks image rectangles/background, preserved CAD/outside pixels, clear/final reproduction and actual disposal state. Source original-camera/material/trace/source metadata/document/PC/physical parity remains separate.

Next integration must consume the original texture cache/upload audit: per-context/kind/name matching, file-first then embedded fallback, gray rules, mipmaps/anisotropy, resize and32-bit alpha repair. Implement original source setter failure transitions and parent/document effects before connecting this low-level API as a completed SetView operation.
