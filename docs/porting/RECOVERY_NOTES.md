# Recovery/build caveats

Canonical decompilation is reference source, not an automatically compilable iOS app. Earlier Windows rebuilds needed nine lambda-syntax recovery fixes and special handling for seven sealed classes declaring new virtual methods. The CAD rebuild temporarily unsealed those declarations and restored original sealed metadata in the output assembly. Do not blindly remove virtual methods or change inheritance to silence errors. tools/RecoveryMetadata retains the metadata inspection/restoration source.

The complete-ref Windows CAD rebuild had zero errors and 804 warnings, and reflection resolved 2113 types. Original embedded resource containers were hash-compared. These results did not execute the native CAD engine. Exported resource objects here still require reviewed packaging in a rebuilt app.

Observed legacy references were Rhino_DotNet 4.0.61206.141 and RhinoCommon 5.1.30000.16. Native Windows/RDK/V-Ray binaries, runtime activation, complete account catalog state and a contained executable reference remain external prerequisites. They are not iOS binaries and are not uploaded.

Prefer the installer baseline. The loose executable has different authentication behavior and is comparison evidence, not an entitlement implementation to copy.

SourceAudit targets .NET 10 and accepts repository-root and output-directory arguments. Write fresh indexes to .local before reviewing any curated audit update. ReuseAudit performs conservative whole-file compiler elimination; rejected files are not a mandatory-rewrite count. Roslyn audit hashes describe decoded text while recovery manifests describe exported bytes.
