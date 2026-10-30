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
    }
    double parameter=0;ok(jd_candidate_curve_length_parameter(context.id,circle,0,1e-9,&parameter,error,sizeof(error)));nearCurve(parameter,0);
    ok(jd_candidate_curve_length_parameter(context.id,circle,1,1e-9,&parameter,error,sizeof(error)));nearCurve(parameter,2*pi);
    checkCurve(jd_candidate_curve_length_parameter(context.id,circle,-.1,1e-9,&parameter,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Invalid length fraction accepted");
    uint64_t moved=0;ok(jd_candidate_translate(context.id,circle,1,2,3,&moved,error,sizeof(error)));
    double x=0,y=0,z=0;ok(jd_candidate_curve_point(context.id,moved,first,&x,&y,&z,error,sizeof(error)));nearCurve(x,1);nearCurve(y,2);nearCurve(z,10);
    uint64_t box=0;ok(jd_candidate_box(context.id,1,1,1,&box,error,sizeof(error)));
    checkCurve(jd_candidate_curve_inspect(context.id,box,&first,&last,&length,&degree,&poles,&rational,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION,"Solid produced fake curve info");
    checkCurve(jd_candidate_curve_point(context.id,circle,std::numeric_limits<double>::quiet_NaN(),&x,&y,&z,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Nonfinite parameter accepted");
    for(double radius:{0.0,-1.0,std::numeric_limits<double>::infinity()}){
        uint64_t rejected=99;checkCurve(jd_candidate_circle(context.id,radius,0,0,0,0,1,0,0,0,1,&rejected,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&rejected==0,"Invalid radius created geometry");
    }
    for(uint64_t shape:{circle,moved,box})ok(jd_candidate_shape_release(context.id,shape,error,sizeof(error)));
    uint64_t count=99;ok(jd_candidate_context_live_shapes(context.id,&count,error,sizeof(error)));checkCurve(count==0,"Native curve entries retained");
}
