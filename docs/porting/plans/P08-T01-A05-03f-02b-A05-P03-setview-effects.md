# A05-P03 concrete SetView collaborators

Consume [render-effect source](../../evidence/audit/rhino5-setview-render-effects-source.json) and validation/conduit evidence. Implement real parent/document lifetimes, title/application notification, image assets/decode/render/resize and display-mode selection before completing SetView. Retain original load-failure transitions: old runtime image is destroyed before a visible replacement load; failure may leave old stored metadata and a failed new runtime image while returningfalse before later notifications. Hidden images skip load. Do not replace these effects with status flags or success delegates.

Resolve UUID display-mode pipeline and binding collections, page/detail classification, viewport-ID preservation and actual image helper internals. Use real iOS Files/local bundled assets and Metal texture ownership with missing/corrupt/gray/hidden/resize/dispose cases. Preserve source metadata and unsupported paths without executing Windows paths. Full parent/page/navigation/Pop/screen/document integration remains required.

All local tools/cache/output D-only with five-second transient retries, GitHub via connected owner plugin. RED normal/boundary/failure before implementation; Windows/native/adjacent and actual macOS/iPad/iPhone evidence. Original-PC/physical acceptance remains open.

Bounded first child: [A01 wallpaper raster arithmetic](P08-T01-A05-03f-02b-A05-P03-A01-wallpaper-raster.md). Portable buffer processing does not complete the required actual decode/resample/render/lifecycle integration above.

Next source dependency is now [bitmap lookup/add/decode](../../evidence/audit/rhino5-bitmap-loading-source.json): encoded-byte table ownership precedes decode; original ReadFromMemory calls Ltfil15x/Ltkrn15x through ordinal imports. Keep imported bytes, cache success, decoded pixels and actual GPU presentation distinct. Implement a real bounded Apple decoder/texture path and characterize native conversion before claiming image parity.

Follow-up implementation and acceptance: [A02 offline decoder/texture ownership](P08-T01-A05-03f-02b-A05-P03-A02-image-decode.md).
