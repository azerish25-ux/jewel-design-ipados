#include "candidate_abi.h"
#include <cmath>
#include <cstring>
#include <limits>
#include <stdexcept>

namespace {void require_pair_abi(bool ok,const char* message){if(!ok)throw std::runtime_error(message);}}
int run_candidate_paired_abi_contracts(){
 uint64_t context=0,foreignContext=0,first=0,second=0,p0=0,p1=0,foreign=0,result=0;char diagnostic[256]{};
 auto ok=[&](int status){require_pair_abi(status==JD_CANDIDATE_OK,diagnostic);};
 ok(jd_candidate_context_create(1,&context,diagnostic,256));ok(jd_candidate_context_create(1,&foreignContext,diagnostic,256));
 ok(jd_candidate_line(context,0,0,0,10,0,0,&first,diagnostic,256));ok(jd_candidate_line(context,0,4,0,10,4,0,&second,diagnostic,256));
 ok(jd_candidate_line(context,0,0,0,0,4,0,&p0,diagnostic,256));ok(jd_candidate_line(context,10,0,0,10,4,0,&p1,diagnostic,256));
 ok(jd_candidate_box(foreignContext,1,1,1,&foreign,diagnostic,256));
 uint64_t profiles[]={p0,p1};double a[]={0,10},b[]={0,10};int guards=0;
 auto rejected=[&](int status,int expected){require_pair_abi(status==expected&&result==0,"Paired ABI failure did not preserve status/zero output");guards++;};
 auto call=[&](const uint64_t* curves,uint32_t count,const double* x,uint32_t nx,const double* y,uint32_t ny,uint32_t height,uint32_t mode,double tolerance){result=999;return jd_candidate_paired_sweep(context,first,second,curves,count,x,nx,y,ny,height,mode,tolerance,&result,diagnostic,256);};
 rejected(call(nullptr,2,a,2,b,2,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,0,a,0,b,0,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,4097,a,4097,b,4097,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,nullptr,2,b,2,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,1,b,2,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,2,b,1,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,2,nullptr,2,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,2,b,2,2,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,2,b,2,1,2,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 rejected(call(profiles,2,a,2,b,2,1,1,0),JD_CANDIDATE_INVALID_ARGUMENT);
 double nan[]={0,std::numeric_limits<double>::quiet_NaN()};rejected(call(profiles,2,nan,2,b,2,1,1,1e-7),JD_CANDIDATE_INVALID_ARGUMENT);
 result=999;rejected(jd_candidate_paired_sweep(context,foreign,second,profiles,2,a,2,b,2,1,1,1e-7,&result,diagnostic,256),JD_CANDIDATE_INVALID_SHAPE);
 char shortBuffer[10];std::memset(shortBuffer,'Z',sizeof(shortBuffer));double duplicate[]={0,0};result=999;
 rejected(jd_candidate_paired_sweep(context,first,second,profiles,2,duplicate,2,b,2,1,1,1e-7,&result,shortBuffer,8),JD_CANDIDATE_INVALID_ARGUMENT);
 require_pair_abi(shortBuffer[7]==0&&shortBuffer[8]=='Z'&&shortBuffer[9]=='Z',"Paired ABI diagnostic overran capacity");
 require_pair_abi(jd_candidate_paired_sweep(context,first,second,profiles,2,a,2,b,2,1,1,1e-7,nullptr,nullptr,0)==JD_CANDIDATE_INVALID_ARGUMENT,"Paired ABI accepted missing output");guards++;
 ok(jd_candidate_paired_sweep(context,first,second,profiles,2,a,2,b,2,1,1,1e-7,&result,diagnostic,256));
 double u0,u1,v0,v1,x=1,y=1,z=1;ok(jd_candidate_surface_bounds(context,result,&u0,&u1,&v0,&v1,diagnostic,256));
 require_pair_abi(u0<u1&&v0<v1,"Invalid paired ABI face bounds");
 require_pair_abi(jd_candidate_surface_point(context,result,std::numeric_limits<double>::quiet_NaN(),v0,&x,&y,&z,diagnostic,256)==JD_CANDIDATE_INVALID_ARGUMENT&&x==0&&y==0&&z==0,"Surface error outputs were not zero");guards++;
 require_pair_abi(jd_candidate_surface_bounds(context,first,&u0,&u1,&v0,&v1,diagnostic,256)==JD_CANDIDATE_UNSUPPORTED_OPERATION&&u0==0&&u1==0&&v0==0&&v1==0,"Wrong-kind surface query accepted");guards++;
 ok(jd_candidate_surface_bounds(context,result,&u0,&u1,&v0,&v1,diagnostic,256));ok(jd_candidate_shape_retain(context,result,diagnostic,256));ok(jd_candidate_shape_release(context,result,diagnostic,256));
 for(uint64_t input:{first,second,p0,p1})ok(jd_candidate_shape_release(context,input,diagnostic,256));
 ok(jd_candidate_surface_point(context,result,u0,v0,&x,&y,&z,diagnostic,256));require_pair_abi(std::abs(x)+std::abs(y)+std::abs(z)<1e-7,"Paired output depends on released inputs");
 ok(jd_candidate_shape_release(context,result,diagnostic,256));uint64_t live=99;ok(jd_candidate_context_live_shapes(context,&live,diagnostic,256));require_pair_abi(live==0,"Paired ABI leaked native entries");
 ok(jd_candidate_context_release(context,diagnostic,256));ok(jd_candidate_shape_release(foreignContext,foreign,diagnostic,256));ok(jd_candidate_context_release(foreignContext,diagnostic,256));return guards;
}
