# ADR 0010: additional legacy stored-double schemas

Status: implemented locally; Apple runtime proof pending for this extension.

The pinned library includes26 records whose registered types are CSValue, CSValueOnOff and CSValueChooser, all at record version1.1. Recovered CSObject/CSBlock/CSValueBase/CSValue readers define a shared header plus one double. OnOff adds no archive fields; Chooser derives an image-override list from existing user data. These records require their own exact profiles rather than fabricated range bounds or a Boolean byte.

LegacyCsg4Scalar now owns immutable header/link/category/user-data state and a bit-preserved stored double. LegacyCsg4Numeric retains its existing bounds/step as a derived schema. Existing Numeric source callers and the74 bounded-numeric count remain valid; Record.Scalar and ScalarRecordCount expose100 supported scalar payloads. WithScalarValue structurally rebuilds a decoded record; WithNumericValue retains its bounded-numeric guard. Unknown type/version records remain opaque and noneditable.

The file layer preserves serialized fields. It does not perform the original runtime reader load mutations, resolve live channels, normalize OnOff values, apply chooser selection policy, execute callbacks, regenerate CAD or reopen the archive in the PC application. Graph projection/edit-session support remains at the earlier numeric scope until a separate tested integration.

Validation: independent source-hashed oracle for26 bodies; every header/link/category/user-data field and value bits matched; no-op/reversal are byte-identical;26 individual edits preserve all240 other records. Exported examples for all3 added types pass independent Node CRC/full-body/header/bit checks. Existing four-output Node/CSharp crosscheck and the shared managed suite pass locally. Corrupt strings/links and future versions/nonfinite bits are covered. Apple shared-suite execution is required before the new codec checkpoint is promoted.

This is partial file compatibility. The remaining141 payloads, modern CAM/version profiles, runtime load policies, graph generation, geometry/preview updates, physical devices and PC/iPad/PC results remain incomplete.
