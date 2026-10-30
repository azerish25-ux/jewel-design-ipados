# Transform caller semantics

[Source-hashed contracts](transform-caller-contracts.json) cover Move, Mirror, RotateObjectDeg, ScaleObject and TransformObjectXform plus data dispatch. Source analysis only: native PC/iOS geometry, archive behavior and full transform coverage remain unverified.

The shared helper ignores ApplyTransform failure after matrix/interface checks. Move uses failed point conversions as Unset. Rotation composes Rx*Ry*Rz and may use the tight bounding-box center. TransformObjectXform left-multiplies valid flattened inputs. Matrix conventions need independent point fixtures.

Mirror duplicate mode grows combinations in X/Y/Z order without directly appending originals: (2^k-1)*n results, including an empty array when no axes are enabled. In the Z branch, prior accumulated items omit Flip. This recovered asymmetry and RotateObjectDeg Boolean input declaration require original-runtime verification.

Zero/negative scale, unsupported data, failed duplication, invalid frames and output retention remain explicit cases. No native transform backend or caller implementation is claimed.

Expansion-base inspection resolves the misleading property name: NonExpandableInputChannels selects every slot except the last. The base reuses a trailing unlinked slot and appends when the last is linked; load calls that method, while AddLink itself sets the link without creating a new trailing slot afterward. Include last-slot/channel lifecycle tests before deciding what the production composition should consume.
