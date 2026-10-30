# P05-T02-A09 candidate affine BRep transformation

Goal: real nonuniform/shear/pivot-capable affine geometry copies needed by OutsideRail_V1 and recovered transform callers. Candidate protocol only; not Rhino singular-transform/orientation/failure-policy equivalence.

Pinned gp_GTrsf SetValue(row1..3,col1..4), SetForm/IsSingular and BRepBuilderAPI_GTransform(shape,transform,Copy=true) inspected. C API accepts a counted12-double row-major3x4 matrix, implicit last row[0,0,0,1]. Finite/non-singular supported matrices produce actual native geometry with independent ownership; invalid count/nonfinite/singular requests reject, never return an unchanged fake result.

- [x] RED managed tests call missing affine API/DTO before implementation.
- [x] Add C ABI and CSharp immutable matrix DTO/adapter through existing gated factory/SafeHandles. Explicit layout/count and fixed-width diagnostics. No managed synthetic geometry or mutation of source.
- [x] Actual native/CSharp tests transform ZX rational-circle sample points with nonuniform scale+shear+translation; source samples stay unchanged. Transform box and independently check determinant-scaled volume. Invalid/singular/nonfinite matrices and ownership guards remain explicit.
- [x] Run D:-local cached native host helper and Mac simulator/device builds. Preserve OutsideRail ordered plane/pivot/lock branches and original-PC/cancellation/process-memory gates.

Bounded [Apple runtime/device evidence](../../evidence/apple/native-affine-results.json) verified. Original Rhino failure/orientation/PC/physical gates remain open.
