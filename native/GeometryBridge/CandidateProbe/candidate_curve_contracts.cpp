#include "candidate_abi.h"
#include <cmath>
#include <limits>
#include <stdexcept>
#include <string>
namespace {
void checkCurve(bool value,const char* message){if(!value)throw std::runtime_error(message);}
void nearCurve(double value,double expected){checkCurve(std::isfinite(value)&&std::abs(value-expected)<1e-8,"Native curve analytic mismatch");}
struct CurveContext{uint64_t id=0;~CurveContext(){if(id)jd_candidate_context_release(id,nullptr,0);}};
}
void run_candidate_curve_contracts()
{
    char error[256]={};CurveContext context;
    auto ok=[&](int32_t status){if(status)throw std::runtime_error(std::string("Curve ABI: ")+error);};
    ok(jd_candidate_context_create(1,&context.id,error,sizeof(error)));
    uint64_t circle=0;ok(jd_candidate_circle(context.id,7,0,0,0,0,1,0,0,0,1,&circle,error,sizeof(error)));
    double first=0,last=0,length=0;uint32_t degree=0,poles=0,rational=0;
    ok(jd_candidate_curve_inspect(context.id,circle,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error)));
    const double pi=std::acos(-1.0);nearCurve(first,0);nearCurve(last,2*pi);nearCurve(length,14*pi);
    checkCurve(degree==2&&poles==9&&rational==1,"Four-span rational native curve");
    const int c[4]={1,0,-1,0},s[4]={0,1,0,-1};const double w=1/std::sqrt(2.0);
    for(int quarter=0;quarter<4;quarter++)for(double u:{0.0,.125,.25,.5,.75,.875,1.0}){
        const double b0=(1-u)*(1-u),b1=2*u*(1-u),b2=u*u,den=b0+w*b1+b2;
        const double a=7*(b0+w*b1)/den,b=7*(w*b1+b2)/den;
        double x=0,y=0,z=0;ok(jd_candidate_curve_point(context.id,circle,(quarter+u)*pi/2,&x,&y,&z,error,sizeof(error)));
        nearCurve(x,s[quarter]*a+c[quarter]*b);nearCurve(y,0);nearCurve(z,c[quarter]*a-s[quarter]*b);
    }
    double previous=-1;
    for(int i=0;i<50;i++){
        const double fraction=i/50.0;double parameter=0,x=0,y=0,z=0;
        ok(jd_candidate_curve_length_parameter(context.id,circle,fraction,1e-9,&parameter,error,sizeof(error)));
        checkCurve(parameter>=previous,"Native length parameters not monotone");previous=parameter;
        ok(jd_candidate_curve_point(context.id,circle,parameter,&x,&y,&z,error,sizeof(error)));
        nearCurve(x,7*std::sin(2*pi*fraction));nearCurve(y,0);nearCurve(z,7*std::cos(2*pi*fraction));
        ok(jd_candidate_curve_tangent(context.id,circle,parameter,&x,&y,&z,error,sizeof(error)));
        nearCurve(x,std::cos(2*pi*fraction));nearCurve(y,0);nearCurve(z,-std::sin(2*pi*fraction));
    }
    double parameter=0;ok(jd_candidate_curve_length_parameter(context.id,circle,0,1e-9,&parameter,error,sizeof(error)));nearCurve(parameter,0);
    ok(jd_candidate_curve_length_parameter(context.id,circle,1,1e-9,&parameter,error,sizeof(error)));nearCurve(parameter,2*pi);
    checkCurve(jd_candidate_curve_length_parameter(context.id,circle,-.1,1e-9,&parameter,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid length fraction accepted");
    uint64_t moved=0;ok(jd_candidate_translate(context.id,circle,1,2,3,&moved,error,sizeof(error)));
    double x=0,y=0,z=0;ok(jd_candidate_curve_point(context.id,moved,first,&x,&y,&z,error,sizeof(error)));nearCurve(x,1);nearCurve(y,2);nearCurve(z,10);
    ok(jd_candidate_curve_tangent(context.id,moved,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,1);nearCurve(y,0);nearCurve(z,0);
    ok(jd_candidate_curve_tangent(context.id,circle,2*pi,&x,&y,&z,error,sizeof(error)));nearCurve(x,1);nearCurve(y,0);nearCurve(z,0);
    for(double bad:{-.1,2*pi+.1,std::numeric_limits<double>::quiet_NaN(),std::numeric_limits<double>::infinity()})
        checkCurve(jd_candidate_curve_tangent(context.id,circle,bad,&x,&y,&z,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid tangent parameter accepted");
    checkCurve(jd_candidate_curve_tangent(context.id,circle,0,nullptr,&y,&z,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Missing tangent output accepted");
    uint64_t box=0;ok(jd_candidate_box(context.id,1,1,1,&box,error,sizeof(error)));
    checkCurve(jd_candidate_curve_inspect(context.id,box,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced fake curve info");
    checkCurve(jd_candidate_curve_point(context.id,circle,std::numeric_limits<double>::quiet_NaN(),&x,&y,&z,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Nonfinite parameter accepted");
    for(double radius:{0.0,-1.0,std::numeric_limits<double>::infinity()}){
        uint64_t rejected=99;checkCurve(jd_candidate_circle(context.id,radius,0,0,0,0,1,0,0,0,1,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Invalid radius created geometry");
    }
    checkCurve(jd_candidate_curve_tangent(context.id,box,0,&x,&y,&z,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced fake tangent");
    const double matrix[12]={2,0,.5,1,0,1,0,2,0,0,3,-4};uint64_t affine=0;
    ok(jd_candidate_affine(context.id,circle,matrix,12,&affine,error,sizeof(error)));
    ok(jd_candidate_curve_point(context.id,affine,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,4.5);nearCurve(y,2);nearCurve(z,17);
    ok(jd_candidate_curve_point(context.id,circle,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);nearCurve(y,0);nearCurve(z,7);
    uint64_t rejected=99;checkCurve(jd_candidate_affine(context.id,circle,matrix,11,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Invalid matrix count");
    double badMatrix[12]={1,0,0,0,0,1,0,0,0,0,1,0};badMatrix[0]=std::numeric_limits<double>::quiet_NaN();
    checkCurve(jd_candidate_affine(context.id,circle,badMatrix,12,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Nonfinite matrix");
    ok(jd_candidate_curve_tangent(context.id,affine,pi/2,&x,&y,&z,error,sizeof(error)));nearCurve(x,-.5/std::sqrt(9.25));nearCurve(y,0);nearCurve(z,-3/std::sqrt(9.25));
    ok(jd_candidate_shape_release(context.id,affine,error,sizeof(error)));
    double nearestParameter=0,nearestX=0,nearestY=0,nearestZ=0,nearestDistance=0;
    ok(jd_candidate_curve_closest(context.id,circle,10,3,10,&nearestParameter,&nearestX,&nearestY,&nearestZ,&nearestDistance,error,sizeof(error)));
    nearCurve(nearestX,7/std::sqrt(2.0));nearCurve(nearestY,0);nearCurve(nearestZ,7/std::sqrt(2.0));
    nearCurve(nearestDistance,std::sqrt(std::pow(std::sqrt(200.0)-7,2)+9));
    checkCurve(jd_candidate_curve_closest(context.id,box,10,3,10,&nearestParameter,&nearestX,&nearestY,&nearestZ,&nearestDistance,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced fake closest curve point");
    checkCurve(jd_candidate_curve_closest(context.id,circle,10,3,10,nullptr,&nearestX,&nearestY,&nearestZ,&nearestDistance,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Missing closest output accepted");
    uint64_t quarter=0,reversed=0;ok(jd_candidate_curve_trim(context.id,circle,0,pi/2,&quarter,error,sizeof(error)));
    ok(jd_candidate_curve_inspect(context.id,quarter,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error)));nearCurve(first,0);nearCurve(last,pi/2);nearCurve(length,7*pi/2);
    ok(jd_candidate_curve_reverse(context.id,quarter,&reversed,error,sizeof(error)));
    ok(jd_candidate_curve_tangent(context.id,quarter,pi/2,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);nearCurve(y,0);nearCurve(z,-1);
    ok(jd_candidate_curve_tangent(context.id,reversed,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);nearCurve(y,0);nearCurve(z,1);
    ok(jd_candidate_curve_point(context.id,reversed,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,7);nearCurve(y,0);nearCurve(z,0);
    ok(jd_candidate_curve_closest(context.id,quarter,10,0,-2,&nearestParameter,&nearestX,&nearestY,&nearestZ,&nearestDistance,error,sizeof(error)));
    nearCurve(nearestX,7);nearCurve(nearestY,0);nearCurve(nearestZ,0);nearCurve(nearestDistance,std::sqrt(13.0));
    checkCurve(jd_candidate_curve_trim(context.id,circle,1,0,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid trim interval accepted");
    uint64_t secondArc=0,joined=0;ok(jd_candidate_curve_trim(context.id,circle,pi/2,pi,&secondArc,error,sizeof(error)));
    ok(jd_candidate_curve_join(context.id,quarter,secondArc,1e-8,&joined,error,sizeof(error)));
    ok(jd_candidate_curve_inspect(context.id,joined,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error)));nearCurve(length,7*pi);
    ok(jd_candidate_curve_point(context.id,joined,first,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);nearCurve(y,0);nearCurve(z,7);
    ok(jd_candidate_curve_point(context.id,joined,last,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);nearCurve(y,0);nearCurve(z,-7);
    checkCurve(jd_candidate_curve_join(context.id,secondArc,quarter,1e-8,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Unordered native join accepted");
    ok(jd_candidate_shape_release(context.id,secondArc,error,sizeof(error)));ok(jd_candidate_shape_release(context.id,joined,error,sizeof(error)));
    ok(jd_candidate_shape_release(context.id,quarter,error,sizeof(error)));ok(jd_candidate_shape_release(context.id,reversed,error,sizeof(error)));

    uint32_t eventCount=0;double eventFuzzy=0;
    auto intersections=[&](uint64_t shape,double sx,double sy,double sz,double ex,double ey,double ez,jd_candidate_curve_line_event* events,uint32_t size){
        return jd_candidate_curve_line_intersections(context.id,shape,sx,sy,sz,ex,ey,ez,1e-6,events,size,&eventCount,&eventFuzzy,error,sizeof(error));
    };
    ok(intersections(circle,-200,0,0,200,0,0,nullptr,0));checkCurve(eventCount==2,"Native diameter event count");nearCurve(eventFuzzy,1e-6);
    jd_candidate_curve_line_event eventBuffer[2]{};eventBuffer[0].kind=99;
    checkCurve(intersections(circle,-200,0,0,200,0,0,eventBuffer,1)==JD_CANDIDATE_INVALID_ARGUMENT&&eventCount==2&&eventBuffer[0].kind==99,"Short event buffer wrote partial data");
    checkCurve(intersections(circle,-200,0,0,200,0,0,nullptr,1)==JD_CANDIDATE_INVALID_ARGUMENT,"Null counted event buffer accepted");
    ok(intersections(circle,-200,0,0,200,0,0,eventBuffer,2));
    checkCurve(eventCount==2&&eventBuffer[0].kind==JD_CANDIDATE_POINT&&eventBuffer[1].kind==JD_CANDIDATE_POINT,"Native diameter event type");
    nearCurve(eventBuffer[0].curve_start,pi/2);nearCurve(eventBuffer[1].curve_start,3*pi/2);
    nearCurve(eventBuffer[0].line_start,.5175);nearCurve(eventBuffer[0].curve_start_point.x,7);nearCurve(eventBuffer[0].line_start_point.x,7);
    ok(intersections(circle,0,0,0,6,0,0,nullptr,0));checkCurve(eventCount==0,"Native finite segment extended");
    ok(intersections(circle,-20,3,0,20,3,0,nullptr,0));checkCurve(eventCount==0,"Native skew segment projected to plane");
    checkCurve(jd_candidate_curve_line_intersections(context.id,circle,0,0,0,100,0,0,1e-6,nullptr,0,nullptr,&eventFuzzy,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Missing event count accepted");
    checkCurve(jd_candidate_curve_line_intersections(context.id,circle,0,0,0,100,0,0,0,nullptr,0,&eventCount,&eventFuzzy,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid event tolerance accepted");
    checkCurve(intersections(circle,0,0,0,0,0,0,nullptr,0)==JD_CANDIDATE_INVALID_ARGUMENT,"Degenerate event segment accepted");
    checkCurve(intersections(box,0,0,0,100,0,0,nullptr,0)==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced native curve events");
    uint64_t linear=0;ok(jd_candidate_line(context.id,0,0,0,10,0,0,&linear,error,sizeof(error)));
    ok(intersections(linear,12,0,0,3,0,0,eventBuffer,2));
    checkCurve(eventCount==1&&eventBuffer[0].kind==JD_CANDIDATE_OVERLAP,"Native reversed overlap lost");
    nearCurve(eventBuffer[0].curve_start,3);nearCurve(eventBuffer[0].curve_end,10);
    nearCurve(eventBuffer[0].curve_start_point.x,3);nearCurve(eventBuffer[0].curve_end_point.x,10);
    nearCurve(eventBuffer[0].line_start_point.x,10);nearCurve(eventBuffer[0].line_end_point.x,3);
    ok(jd_candidate_curve_point(context.id,linear,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,0);
    ok(jd_candidate_shape_release(context.id,linear,error,sizeof(error)));
    rejected=99;checkCurve(jd_candidate_line(context.id,1,2,3,1,2,3,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Degenerate native line created");

    uint64_t relocated=0;ok(jd_candidate_curve_seam(context.id,circle,.3,&relocated,error,sizeof(error)));
    ok(jd_candidate_curve_inspect(context.id,relocated,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error)));
    nearCurve(first,.3);nearCurve(last,.3+2*pi);nearCurve(length,14*pi);
    for(int i=0;i<=64;i++){
        const double parameter=.3+2*pi*i/64.0;double wrapped=parameter;
        if(wrapped>2*pi)wrapped-=2*pi;
        double sourceX=0,sourceY=0,sourceZ=0;
        ok(jd_candidate_curve_point(context.id,circle,wrapped,&sourceX,&sourceY,&sourceZ,error,sizeof(error)));
        ok(jd_candidate_curve_point(context.id,relocated,parameter,&x,&y,&z,error,sizeof(error)));
        nearCurve(x,sourceX);nearCurve(y,sourceY);nearCurve(z,sourceZ);
    }
    uint64_t openSeam=0;ok(jd_candidate_curve_trim(context.id,circle,0,pi/2,&openSeam,error,sizeof(error)));
    rejected=99;checkCurve(jd_candidate_curve_seam(context.id,openSeam,.3,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Open seam input accepted");
    for(double bad:{-.1,7.0,std::numeric_limits<double>::quiet_NaN(),std::numeric_limits<double>::infinity()}){
        rejected=99;checkCurve(jd_candidate_curve_seam(context.id,circle,bad,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Invalid seam parameter accepted");
    }
    rejected=99;checkCurve(jd_candidate_curve_seam(context.id,box,0,&rejected,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION&&rejected==0,"Solid seam produced fake curve");
    checkCurve(jd_candidate_curve_seam(context.id,circle,.3,nullptr,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Missing seam output accepted");
    uint64_t seamAffine=0,seamAffineResult=0;ok(jd_candidate_affine(context.id,circle,matrix,12,&seamAffine,error,sizeof(error)));
    ok(jd_candidate_curve_seam(context.id,seamAffine,.7,&seamAffineResult,error,sizeof(error)));
    double sourceX=0,sourceY=0,sourceZ=0;ok(jd_candidate_curve_point(context.id,seamAffine,.7,&sourceX,&sourceY,&sourceZ,error,sizeof(error)));
    ok(jd_candidate_curve_point(context.id,seamAffineResult,.7,&x,&y,&z,error,sizeof(error)));nearCurve(x,sourceX);nearCurve(y,sourceY);nearCurve(z,sourceZ);
    for(uint64_t owned:{relocated,openSeam,seamAffine,seamAffineResult})ok(jd_candidate_shape_release(context.id,owned,error,sizeof(error)));

    double boundsMinX=0,boundsMinY=0,boundsMinZ=0,boundsMaxX=0,boundsMaxY=0,boundsMaxZ=0,boundsPadding=0;
    auto bounds=[&](uint64_t shape,double tolerance){
        return jd_candidate_curve_bounds(context.id,shape,tolerance,&boundsMinX,&boundsMinY,&boundsMinZ,&boundsMaxX,&boundsMaxY,&boundsMaxZ,&boundsPadding,error,sizeof(error));
    };
    auto nearBounds=[&](double actual,double expected){checkCurve(std::isfinite(actual)&&std::abs(actual-expected)<1e-6,"Native optimal extrema mismatch");};
    ok(bounds(circle,1e-10));nearBounds(boundsMinX,-7);nearBounds(boundsMaxX,7);nearBounds(boundsMinY,0);nearBounds(boundsMaxY,0);nearBounds(boundsMinZ,-7);nearBounds(boundsMaxZ,7);
    checkCurve(boundsPadding>=1e-7,"Kernel bounds gap not exposed");
    uint64_t boundsArc=0;ok(jd_candidate_curve_trim(context.id,circle,pi/4,3*pi/4,&boundsArc,error,sizeof(error)));
    ok(bounds(boundsArc,1e-10));nearBounds(boundsMinX,7/std::sqrt(2.0));nearBounds(boundsMaxX,7);nearBounds(boundsMinZ,-7/std::sqrt(2.0));nearBounds(boundsMaxZ,7/std::sqrt(2.0));
    ok(jd_candidate_shape_release(context.id,boundsArc,error,sizeof(error)));
    uint64_t boundsEllipse=0;ok(jd_candidate_affine(context.id,circle,matrix,12,&boundsEllipse,error,sizeof(error)));
    ok(bounds(boundsEllipse,1e-10));nearBounds(boundsMinX,1-7*std::sqrt(4.25));nearBounds(boundsMaxX,1+7*std::sqrt(4.25));nearBounds(boundsMinY,2);nearBounds(boundsMaxY,2);nearBounds(boundsMinZ,-25);nearBounds(boundsMaxZ,17);
    checkCurve(boundsMaxX<17,"Native control-pole box used as extrema");
    ok(jd_candidate_shape_release(context.id,boundsEllipse,error,sizeof(error)));
    for(double bad:{0.0,-1.0,std::numeric_limits<double>::quiet_NaN(),std::numeric_limits<double>::infinity()})
        checkCurve(bounds(circle,bad)==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid bounds tolerance accepted");
    checkCurve(bounds(box,1e-10)==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced fake curve bounds");
    checkCurve(jd_candidate_curve_bounds(context.id,circle,1e-10,&boundsMinX,&boundsMinY,&boundsMinZ,&boundsMaxX,&boundsMaxY,&boundsMaxZ,nullptr,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Missing bounds output accepted");

    uint32_t curveClosed=99;ok(jd_candidate_curve_closed(context.id,circle,&curveClosed,error,sizeof(error)));checkCurve(curveClosed==1,"Closed circle query");
    uint64_t closureLine=0;ok(jd_candidate_line(context.id,0,0,0,1,0,0,&closureLine,error,sizeof(error)));
    ok(jd_candidate_curve_closed(context.id,closureLine,&curveClosed,error,sizeof(error)));checkCurve(curveClosed==0,"Open line query");
    ok(jd_candidate_shape_release(context.id,closureLine,error,sizeof(error)));
    checkCurve(jd_candidate_curve_closed(context.id,box,&curveClosed,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid closure query");
    checkCurve(jd_candidate_curve_closed(context.id,circle,nullptr,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Null closure output accepted");
    for(double scale:{1.0,1.0000000000000002,2.0,-1.0}){
        const double uniformMatrix[12]={scale,0,0,1,0,scale,0,2,0,0,scale,3};uint64_t uniformShape=0;
        ok(jd_candidate_affine(context.id,circle,uniformMatrix,12,&uniformShape,error,sizeof(error)));
        ok(jd_candidate_curve_point(context.id,uniformShape,0,&x,&y,&z,error,sizeof(error)));nearCurve(x,1);nearCurve(y,2);nearCurve(z,7*scale+3);
        ok(jd_candidate_shape_release(context.id,uniformShape,error,sizeof(error)));
    }
    const double boxUniform[12]={2,0,0,1,0,2,0,2,0,0,2,3};uint64_t uniformBox=0;
    ok(jd_candidate_affine(context.id,box,boxUniform,12,&uniformBox,error,sizeof(error)));
    double uniformVolume=0;uint32_t uniformSolids=0,uniformValid=0;
    ok(jd_candidate_shape_inspect(context.id,uniformBox,&uniformVolume,&uniformSolids,&uniformValid,error,sizeof(error)));nearCurve(uniformVolume,8);checkCurve(uniformSolids==1&&uniformValid==1,"Uniform solid affine validity");
    ok(jd_candidate_shape_release(context.id,uniformBox,error,sizeof(error)));
    for(uint64_t shape:{circle,moved,box})ok(jd_candidate_shape_release(context.id,shape,error,sizeof(error)));
    uint64_t count=99;ok(jd_candidate_context_live_shapes(context.id,&count,error,sizeof(error)));checkCurve(count==0,"Native curve entries retained");
}
