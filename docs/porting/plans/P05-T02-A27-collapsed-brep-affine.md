# P05-T02-A27 collapsed-apex BRep affine robustness

Known failure: old cap path transformed an already built collapsed-apex native face; generic affine rejected it after source plane normalization correction. A25 correctly changed cap order to transform surface before ToBrep, but existing BRep transforms still need support for downstream operations.

- Reproduce exact original local cap face plus inverse metadata transform from actual115/122/138/139. RED native invalid BRep required.
- Test existing world caps and joined140 under identity, rigid, uniform scale, nonuniform scale, shear and reflection; preserve actual requested coefficients/parameter points, volume determinant law, input geometry/tolerances and ownership.
- Inspect pinned GTransform/NurbsConvert/degenerate topology behavior. No matrix snapping, silent geometry substitution, inflated test tolerance, forced caps or removal of nonuniform support.
- Native valid result and complete appropriate regression/source/repository checks; document numerical/topology limits; macOS Actions; commit/push. Recovery/kernel read-only, all local writes D: with5-second transient retry.

Local result: exact failure reproduced, face27/wire28 observed; direct explicit-spline modifier fixes20 cases/350 points without matrix/tolerance changes. Analytic/wrapped conversion and broader degeneration/mesh families remain unverified. Apple tests pending.
