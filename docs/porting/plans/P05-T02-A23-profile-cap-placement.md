# P05-T02-A23 recovered cap construction with explicit plane

Prerequisite for BLD-0020/SER-0055. Source CSProfileCap.Update, CSUtilRailRevolve and CSUtilGeometry.CreateDomeRC. A22 provides actual native controls. Actual fixture138/139 automatic graph integration remains open pending source-equivalent AveragePlaneOfCurve/TryGetPlane orientation.

- RED missing ProfileCapWithPlaneCandidate.Build.
- Given an explicit orthonormal plane, preserve ChangeBasis(world,plane), Euclidean control-hull XY midpoint translation, inverse(translation*basis), recovered dome/tensor(zAdjust=true), inverse surface placement and inverse metadata with no second application.
- Do not use arbitrary fitted planes or accurate/extrema box for legacy fast-box centering. Published pinned openNURBS GetBBox control-list rule is comparison evidence; acquired Rhino5 wrapper call sites recorded separately.
- Test circle/asymmetric rational rail on3 translated/rotated frames with signed/flat domes, outer native boundary, apex/hullcenter, input/metadata preservation, invalid plane/ownership and cleanup. Run complete native/shared/repository checks and Apple CI; commit/push.

All local writes D:;5-second retry on transient D:. Recovery/kernel read-only. No PC/physical/full cap row closed.

Local result:18 native faces,594 boundary points,18 apex/metadata cases and failure/ownership guards pass. RED missing API recorded; source contract/tamper checks added. New Apple tests pending.
