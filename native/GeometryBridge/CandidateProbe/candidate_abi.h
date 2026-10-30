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
/* Unit tangent from actual native derivative; parameter must lie within edge domain. */
int32_t jd_candidate_curve_tangent(uint64_t context,uint64_t shape,double parameter,double* x,double* y,double* z,char* diagnostic,uint32_t capacity);

enum jd_candidate_curve_line_kind { JD_CANDIDATE_POINT=1, JD_CANDIDATE_OVERLAP=2 };
typedef struct jd_candidate_point3 { double x,y,z; } jd_candidate_point3;
typedef struct jd_candidate_curve_line_event {
    uint32_t kind,reserved;
    double curve_start,curve_end,line_start,line_end;
    jd_candidate_point3 curve_start_point,curve_end_point,line_start_point,line_end_point;
} jd_candidate_curve_line_event;
int32_t jd_candidate_line(uint64_t context,double sx,double sy,double sz,double ex,double ey,double ez,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Two-pass counted query. Null events/zero event_capacity queries count.
 * Short buffers receive required count, InvalidArgument, and no event writes.
 * Ranges are independently ascending; line parameters are fractions of segment.
 * Effective fuzzy tolerance includes the kernel minimum; edge tolerances also apply. */
int32_t jd_candidate_curve_line_intersections(uint64_t context,uint64_t shape,double sx,double sy,double sz,double ex,double ey,double ez,double tolerance,jd_candidate_curve_line_event* events,uint32_t event_capacity,uint32_t* count,double* effective_fuzzy,char* diagnostic,uint32_t capacity);
/* Copied closed seam; output domain starts at requested in-domain parameter. */
int32_t jd_candidate_curve_seam(uint64_t context,uint64_t shape,double seam,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Unpadded numerical extrema plus native box gap; not a pole/control-hull box. */
int32_t jd_candidate_curve_bounds(uint64_t context,uint64_t shape,double tolerance,double* min_x,double* min_y,double* min_z,double* max_x,double* max_y,double* max_z,double* padding,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_curve_closed(uint64_t context,uint64_t shape,uint32_t* closed,char* diagnostic,uint32_t capacity);
/* Standard full knot vector, Euclidean XYZ poles and optional declared rational weights. */
int32_t jd_candidate_nurbs(uint64_t context,uint32_t degree,uint32_t pole_count,uint32_t rational,const double* coordinates,uint32_t coordinate_count,const double* weights,uint32_t weight_count,const double* full_knots,uint32_t knot_count,uint64_t* output,char* diagnostic,uint32_t capacity);
/* Experimental open-path paired correspondence; no closed/no-param Rhino block policy. */
int32_t jd_candidate_paired_sweep(uint64_t context,uint64_t rail1,uint64_t rail2,const uint64_t* profiles,uint32_t count,const double* parameters1,uint32_t count1,const double* parameters2,uint32_t count2,uint32_t maintain_height,uint32_t correspondence,double tolerance,uint64_t* output,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_surface_bounds(uint64_t context,uint64_t shape,double* first_u,double* last_u,double* first_v,double* last_v,char* diagnostic,uint32_t capacity);
int32_t jd_candidate_surface_point(uint64_t context,uint64_t shape,double u,double v,double* x,double* y,double* z,char* diagnostic,uint32_t capacity);
/* Fast conservative or accurate shape box; no triangulation dependence. */
int32_t jd_candidate_shape_bounds(uint64_t context,uint64_t shape,uint32_t accurate,double* min_x,double* min_y,double* min_z,double* max_x,double* max_y,double* max_z,double* padding,char* diagnostic,uint32_t capacity);
#ifdef __cplusplus
}
#endif
#endif
