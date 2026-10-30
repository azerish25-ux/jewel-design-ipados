# Loft and planar extrusion source semantics

[Source-hashed contracts](surface-caller-contracts.json) cover two registered nodes and the loft matching/join helpers. This is source analysis; native PC/iOS results and archive regeneration are unverified.

ExtrudePlanarCurve makes two curves displaced along the recovered plane normal by plus and minus distance. Its default 20 therefore spans 40 units. It creates an uncapped ruled surface, skips nonplanar/unconvertible inputs and can emit an array despite its declared individual output. A generic one-sided extrusion would change behavior.

Loft consumes only the first flattened curve on each input. It duplicates profiles, uses 50-point orientation/seam matching, requests a straight uncapped loft, removes short segments and can join/repair multiple native surfaces. The OnBrep join overload has different filtering and failure rules from the Rhino Brep overload. Detailed match scoring, native tolerances/topology and failure paths still need audit and an original-PC oracle.
