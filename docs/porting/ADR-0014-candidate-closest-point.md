# ADR0014: candidate bounded closest-point query

Status: actual Windows native/CSharp tests passed; Apple verification pending.

The query uses the actual located3D edge curve and its domain, native orthogonal projection/extrema, and explicit comparison with both endpoints. It reports parameter/point/distance through fixed-width outputs, validates finite data and rejects unsupported solids or failed native convergence. Source geometry and ownership remain unchanged.

Equal candidate distances keep the first endpoint/last endpoint/native-event order under strict improvement; this is explicit candidate policy, not original Rhino tie-order equivalence. Open-curve endpoint, degenerate/nonunique projection, general NURBS/tolerance and original-PC comparison cases remain required. No managed synthetic points or fallback after nonconvergence are accepted.

Current actual-native tests cover a circle with off-plane3D query, seam point, translated geometry, point-at-returned-parameter consistency, native errors/null outputs and retained ownership guards. This is one OutsideRail_V1 prerequisite, not its intersection/seam/trim/join or full application implementation.
