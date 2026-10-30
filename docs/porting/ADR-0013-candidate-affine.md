# ADR0013: candidate affine BRep copies

Status: bounded Windows/iPad/iPhone native and CSharp assertions verified; unsigned device build passed. See [Apple reports](../evidence/apple/native-affine-results.json).

The candidate accepts exactly12 finite row-major3x4 affine coefficients with implicit last row[0,0,0,1]. It checks native singularity/determinant finiteness and applies gp_GTrsf through BRepBuilderAPI_GTransform with Copy=true. Output is actual native BRep geometry with independent ownership; source remains unchanged. CSharp uses an immutable DTO and a counted input array, avoiding ambiguous struct packing.

Native tests verify curve points, source preservation and invalid counts/nonfinite data; managed/native tests additionally verify shear/nonuniform/translation and determinant-scaled solid mass. Singular/overflowing matrices are explicitly unsupported. This is candidate policy, not proof of original Rhino singular/orientation/failure handling or OutsideRail_V1 ordered transforms. Caller plane/pivot composition, intersections/seams and PC/physical/cancellation/process-memory gates remain open.
