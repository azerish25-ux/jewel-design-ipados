#ifndef JEWEL_CANDIDATE_ABI_H
#define JEWEL_CANDIDATE_ABI_H
#include <stdint.h>
#ifdef __cplusplus
extern "C" {
#endif
/* Experimental candidate protocol, not the production geometry API.
 * IDs are process-local, never pointers or project identities. Final context
 * release invalidates all its shapes. Retaining a shape does not retain context.
 * Optional diagnostics are truncated/NUL-terminated within supplied capacity;
 * null/zero capacity is allowed. Failed creation writes zero to nonnull output.
 * Caller owns valid pointer ranges; no C++ exception may cross this boundary. */
enum jd_candidate_status {
    JD_CANDIDATE_OK=0, JD_CANDIDATE_INVALID_ARGUMENT=1,
    JD_CANDIDATE_UNSUPPORTED_VERSION=2, JD_CANDIDATE_INVALID_CONTEXT=3,
    JD_CANDIDATE_INVALID_SHAPE=4, JD_CANDIDATE_UNSUPPORTED_OPERATION=5,
    JD_CANDIDATE_KERNEL_FAILURE=6, JD_CANDIDATE_INTERNAL_ERROR=7,
    JD_CANDIDATE_OUT_OF_MEMORY=8, JD_CANDIDATE_LIFETIME_LIMIT=9
};
enum jd_candidate_boolean_operation {
    JD_CANDIDATE_COMMON=1, JD_CANDIDATE_FUSE=2, JD_CANDIDATE_CUT=3
};
uint32_t jd_candidate_abi_version(void);
int32_t jd_candidate_context_create(uint32_t version,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_context_retain(uint64_t context,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_context_release(uint64_t context,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_context_live_shapes(uint64_t context,uint64_t* count,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_box(uint64_t context,double x,double y,double z,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_translate(uint64_t context,uint64_t shape,double x,double y,double z,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_shape_clone(uint64_t context,uint64_t shape,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Fuzzy value is explicit in caller units, not established Rhino equivalence.
 * Pairwise result may be a compound/empty BRep; caller fallback is separate. */
int32_t jd_candidate_boolean(uint64_t context,int32_t operation,uint64_t lhs,uint64_t rhs,double fuzzy,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_shape_retain(uint64_t context,uint64_t shape,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_shape_release(uint64_t context,uint64_t shape,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_shape_inspect(uint64_t context,uint64_t shape,double* volume,uint32_t* solids,uint32_t* valid,char* diagnostic,uint32_t capacity);
/* Four-span clamped rational circle. Frame uses origin, normal and X direction;
 * parameters are native B-spline parameters, not angles or arc-length fractions. */
int32_t jd_candidate_circle(uint64_t context,double radius,double ox,double oy,double oz,double nx,double ny,double nz,double xx,double xy,double xz,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_curve_inspect(uint64_t context,uint64_t shape,double* first,double* last,double* length,uint32_t* degree,uint32_t* poles,uint32_t* rational,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_curve_point(uint64_t context,uint64_t shape,double parameter,double* x,double* y,double* z,char* diagnostic,uint32_t capacity);
/* Candidate normalized arc length with caller-supplied absolute model tolerance. */
int32_t jd_candidate_curve_length_parameter(uint64_t context,uint64_t shape,double fraction,double tolerance,double* parameter,char* diagnostic,uint32_t capacity);
/* Row-major3x4 affine matrix; implicit last row0,0,0,1. */
int32_t jd_candidate_affine(uint64_t context,uint64_t shape,const double* matrix,uint32_t count,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Bounded candidate closest point: endpoints plus native orthogonal candidates. */
int32_t jd_candidate_curve_closest(uint64_t context,uint64_t shape,double px,double py,double pz,double* parameter,double* x,double* y,double* z,double* distance,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_curve_trim(uint64_t context,uint64_t shape,double first,double last,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_curve_reverse(uint64_t context,uint64_t shape,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Ordered end-to-start pair join; no automatic source-compatible reordering. */
int32_t jd_candidate_curve_join(uint64_t context,uint64_t first,uint64_t second,double tolerance,uint64_t* output,char* diagnostic,uint32_t capacity);
#ifdef __cplusplus
}
#endif
#endif
