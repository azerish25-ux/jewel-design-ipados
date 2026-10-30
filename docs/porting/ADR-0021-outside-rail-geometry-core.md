# ADR0021: adapted OutsideRail_V1 geometry core

Status: actual Windows native/CSharp analytical and stored-input tests pass; Apple runtime/device verification pending. Complete CSBlock/chooser/graph/PC parity is not established.

## Recovered algorithm

OutsideRailV1Candidate adapts CSOutsideRail1.Update and its helpers into CSharp over genuine native geometry. It preserves the legacy GUID, defaults3/2/1/false, explicit numeric-radius priority over curve-bounds Max.Z,50 normalized arc-length fractions i/50 excluding the endpoint, sampled minimum origin distance rather than exact closest distance, uniform normalization and finite100-unit axis rays.

Unlocked mode performs the two ordered Z stretches about the current bottom and then top, recomputing all four ray extents each time, before X scaling. It does not normalize seam/direction. The X extent is an axis intersection, not a box width: for a centered input ellipse the final X semiaxis is (R+side)/sqrt(1-centerZ^2/semiZ^2). Thus even a default circle produces a different full width from a generic offset.

Locked mode scales X, requires two finite horizontal-segment events, moves the seam to the first event, reverses a copy, trims both via closest point to the second event, chooses the top by parameter-domain midpoint Z, scales top/bottom separately, selects the first join output (retaining the scaled input for an empty result), then moves the final seam via finite positive-Z ray or closest-to-Z100 fallback and reverses when start tangent X is negative. OutsideScale is always0.

CurveScope owns every intermediate native shape and transfers only the result. Input and radius-curve ownership remain with callers. Explicit recovered missing/zero-input messages are retained; unchecked first-event indexing remains an exception path rather than an invented extent.

## Candidate policies and limits

Settings take separately resolved helper, current application, effective join and bounds tolerances. Global callback/cache integration is not implemented. The official RhinoCommon wrapper at commit3aebe3553b5e2c78ed4d5ae1c21f3e752a6b7f69 corroborates that no-tolerance JoinCurves passes native0 and preserveDirection=false; effective original native defaults remain unresolved. Its array length mapping uses an absolute segment tolerance and fractional1e-8; scalar native abscissa queries approximate that intent but are not an original batch-solver equivalence proof.

The pair-join adapter orients real copies, merges connected endpoints using the native operation or returns real unmerged inputs. Known native KernelFailure is retained in Diagnostics when mapped to the source empty-join fallback. Seam no-op/open/failure paths also retain diagnostics; ownership, unsupported resource and allocation errors are not silently converted into successful geometry. Normal analytical cases require an empty diagnostic list. General unmerged/empty/native-failure/ordering/parameterization outcomes still need wider original-runtime fixtures.

A new fixed-width native current-edge closure query supports the source Boolean seam behavior. It uses the kernel's finite-edge closure rule, not an assertion of Rhino tolerance equivalence.

## Uniform affine defect discovered by composition

The first source pipeline exposed InvalidPointOnCurve during consecutive transforms. Pinned gp_GTrsf.SetValue sets gp_Other with scale0. SetForm can reclassify uniform raw matrices without restoring that scale; vertex transformation then collapses points while curve transformation follows different matrix handling. Preserve the raw affine gp_Other form and remove SetForm. The original BRep modifier stays in use: no topology repair, reconstructed surrogate or curve approximation was added. Identity/near-identity/uniform/reflected curves, consecutive transforms and a uniformly scaled solid now execute through direct C ABI and managed regressions.

## Evidence and remaining work

The independent reader fully consumes fixture record13's known1.0 header, confirms six input links[2,6,8,9,10,11]/channels[0,3,0,0,0,0], preserves unknown versions and rejects malformed bounds/counts. It records stored radius8.667196228921199, thickness4.38/1.84/1.7 and lock1 (number-to-Boolean threshold >0.5). These are stored candidates, not evaluated active/override channels. Chooser6/channel3 points to CSGroupOfMatrixProfiles record7, GUID98415b72-dadb-4b9b-94a9-1e6c5865b5f2 v1.1; its actual selected profile is not generated yet.

Native tests cover both branches, asymmetric/default/negative thickness inputs, representative .001 and tighter tolerances, reversed/shifted seams, radius fallback/priority, final-ray closest fallback, finite helper failure cleanup and zero remaining native entries. An independent Simpson/bisection ellipse oracle gives50-sample minimum3.014498436003 versus exact closest3. Stored-input tests edit top4.38->4.48, save/reopen, rebuild from those values using an explicitly supplied native profile, and undo to identical archive bytes. They do not save regenerated native geometry into a PC project.

Next: decode/port actual matrix-profile/chooser input and integrate channels, activation/override, CSBlock lifecycle and output rail-data properties/export. Original-PC, arbitrary/degenerate failure cases, native batch/ordering/tolerance equivalence, physical-device/cancellation/process-memory and full UI/application parity remain open. No full feature row closes here.

[Work card](plans/P05-T02-A17-outside-rail-composition.md), [task evidence](evidence/P05-T02.json), [independent input oracle](../evidence/audit/outside-rail-input-reference.json), [official wrapper](https://github.com/mcneel/rhinocommon/blob/3aebe3553b5e2c78ed4d5ae1c21f3e752a6b7f69/dotnet/opennurbs/opennurbs_curve.cs).
