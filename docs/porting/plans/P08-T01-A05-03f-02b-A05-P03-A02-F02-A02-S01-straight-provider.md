# P08-T01-A05-03f-02b-A05-P03-A02-F02-A02-S01 typed lossless straight provider adapter

UI-039; child of F02-A02. Input199cc3dcece54b6b1df3237c306a1258442cfaf1. Previous goal turn produced actual GPU/probe evidence; continue the full offline1:1 port without closing overall scope. Native Mac prerequisite003a14986b81b290e5136c0c140df823a8c36406 observed unassociated RGB8 provider bytes exactly. Do not assume iOS equivalence; test it.

RED: Windows compiler missing ImageProviderLayout/StraightRgbaProvider/typed representation constructor. Implement checked row/channel extraction,8-bit RGB24/no-alpha and RGB32 straight-first/last or skip-first/last, default/big/little32 byte order. No alpha division, color/profile transforms, bit-depth loss or fallback on unknown representation. Preserve original encoded source and provider metadata. Reject premul/float/high-depth/unknown color/decode mapping explicitly. Other required layouts remain open; this does not remove them from full port scope.

Add explicit alpha representation to DecodedLocalImage with premul default for existing callers and locked copy/dispose. Native iOS DecodeStraight validates source/status/dimensions, profile/layout, stride/owned budgets and copies provider bytes directly. The only unsafe operation is checking CGImage.Decode pointer for null; it is never dereferenced. Typed Metal upload refuses premultiplied images.

Cases: normal/boundary layouts, endian/alpha placement, padding/missing last-row padding, budgets/truncation, unsupported metadata, source immutability, representation preservation and concurrent copy/dispose. Actual Apple cases require all256 alpha levels with non-primary/transparent RGB exactly retained, native metadata, unchanged PNG/BMP/JPEG source, GPU byte-accurate upload and source-alpha blending, wrong-representation/disposed rejection and native resource retirement. Existing six-stage CAD screenshot gates stay strict and unchanged.

Validation: Windows focused/full native/57 adjacent; macOS portable/bindings and actual iPad/iPhone. No original Windows binaries execute. All local tools/cache/output D-only; GitHub owner plugin for commits/Actions; five-second D retries.

Full format/profile/bit-depth/palette coverage, original LEADTOOLS/GDI/cache conversion, visible image/world/screen/source setter/document/PC/physical acceptance remain required. No parity row closes from this adapter.
