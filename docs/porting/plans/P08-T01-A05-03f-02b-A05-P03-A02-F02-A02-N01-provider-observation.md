# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-N01 native ImageIO provider observation

Requirement UI-039; child of exact straight-alpha decode/visible integration. This is bounded analysis, not a production decoder or full format/PC parity. Input8d2720a7da3f7ee4c5c88fed94f07663483e1f49.

Compile our own C++17 probe against Apple CoreGraphics/ImageIO/CoreFoundation on pinned macOS26/Xcode26.6. Inspect real CGImage data-provider bytes, stride, alpha/bitmap flags, component/pixel depths, color-space name/model and source status/type. Fixtures: original analytical PNG/BMP, unchanged recovered JPEG, and non-primary low-alpha/transparent RGB PNG authored to expose premultiplication loss. All original Windows binaries remain static-only.

Analysis validates actual artifacts rather than fake unit outputs: fixture input hashes, native metadata/dimensions/status, binary provider lengths, direct row-packed RGBA/RGB comparison when layout allows it. Unsupported/null comparisons remain explicit observations; a successful workflow does not establish a production adapter. Archive original/source hashes and toolchain/source commit.

Use result to choose lossless straight pixel extraction or identify a real unsupported layout. Do not unpremultiply guessed colors or assume macOS provider layout equals iOS. Actual iOS provider evidence, profile/bit-depth/palette/format support and visible/source renderer integration remain required. No parity rows close.

All Windows local writes/cache D-only through run-local; connected owner plugin for commits/Actions. Native macOS code is ours, not original vendor DLL execution.
