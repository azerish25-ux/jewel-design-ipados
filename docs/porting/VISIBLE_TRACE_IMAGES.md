# Visible prepared world trace images

CadMetalView.SetTracePixels(image, info, sourceDibDepth) accepts explicitly straight RGBA8/sRGB pixels prepared as a source24-bit RGB or32-bit BGRA DIB. The depth is required: ImageIO may promote opaque input to32-bit storage, which does not establish the original DIB depth/alpha semantics. Original encoded bytes remain unchanged.

Source trace grayscale uses0.299 red,0.589 green,0.111 blue plus0.5 before truncation.32-bit conversion writes the resulting gray into all four channels, including alpha;24-bit conversion has opaque alpha. This differs from wallpaper flat-storage truncation. No automatic modern alpha correction is applied.

Rhino5TracePlane preserves saved finite width/height, negative extents and plane axes. Hidden or both-zero extents produce no image draw. One missing extent is derived from prepared texture dimensions in original operation order. Four corners use(width,0),(width,height),(0,height),(0,0), with source UV(1,0),(1,1),(0,1),(0,0). Original PointAt scalar-double addition order is retained before float conversion.

The visible world vertex shader uses the same mesh center/radius normalization and view-projection as actual CAD geometry. Camera orbit moves the image with its world plane rather than leaving a screen-space rectangle. The current renderer still uses its fitted candidate camera; source CounterSketch camera integration is separate.

The optional Rhino5TextureAnisotropy argument on SetTracePixels captures source sampling policy with that uploaded image. ForApplicationSetting maps source settings1/2/3 to maximum/4,/2,/1; unknown settings select1. Source extension absence is recorded separately from an explicit parameter. Provide the actual source maximum/capability; the API does not guess a saved Windows GPU capability. Metal accepts only exact finite integer results1..16 and rejects other values before replacing the existing image. No rounding or capability substitution is silently applied.

Sampler variants are native nearest/linear clamp-edge descriptors, cached in at most32 states per pipeline and released after all pending frames retire. Pose/hide/filter metadata and mesh/wallpaper changes retain the uploaded trace sampling state. This does not establish original-driver anisotropic pixel equality or source settings invalidation/cache behavior.

SetTraceState changes pose/size/hidden/filter metadata without uploading image pixels again. Filename or gray-mode changes require source resolution/new prepared pixels and are rejected by this metadata-only method. ClearTrace removes the current trace while old frame snapshots retain native resources until completion. Mesh and wallpaper replacements preserve the current trace state. The scene gate remains bounded to3 pending frames.

Separate trace-* screenshots verify image visibility, orbit/pose motion and exact hidden/cleared CAD restoration. Existing CAD and wallpaper capture gates remain unchanged. These are real prepared pixels and regenerated CAD, not preview images or successful-load flags.

Still required before complete SetView/source image parity: original file/embedded/context cache policy, RGB resampling/size/mipmap/anisotropy, full source camera/channel sequencing, missing-texture diagnostic quad/text, source setter failure ordering and real document notifications. Original-PC and physical-device acceptance remain open.
