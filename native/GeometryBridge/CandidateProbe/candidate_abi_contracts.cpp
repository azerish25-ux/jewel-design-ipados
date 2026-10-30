#include "candidate_abi.h"
#include <cmath>
#include <cstdint>
#include <limits>
#include <stdexcept>
#include <string>

namespace {
void check(bool value,const char* message) { if(!value)throw std::runtime_error(message); }
struct ContextOwner {
    uint64_t id=0;
    ~ContextOwner() { if(id)jd_candidate_context_release(id,nullptr,0); }
};
}

void run_candidate_abi_contracts()
{
    char error[256]={};
    auto ok=[&](int32_t status){if(status!=JD_CANDIDATE_OK)throw std::runtime_error(std::string("Candidate ABI: ")+error);};
    check(jd_candidate_abi_version()==1,"Candidate ABI version");
    uint64_t rejected=99;
    check(jd_candidate_context_create(99,&rejected,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_VERSION&&rejected==0,"Unsupported ABI must reject/zero handle");
    check(jd_candidate_context_create(1,nullptr,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Null context output");
    ContextOwner first,second;
    ok(jd_candidate_context_create(1,&first.id,error,sizeof(error)));
    ok(jd_candidate_context_create(1,&second.id,error,sizeof(error)));
    check(first.id&&second.id&&first.id!=second.id,"Context identity");
    ok(jd_candidate_context_retain(first.id,error,sizeof(error)));
    ok(jd_candidate_context_release(first.id,error,sizeof(error)));
    auto mass=[&](uint64_t context,uint64_t shape,double expected,uint32_t solids){
        double actual=0;uint32_t count=0,valid=0;
        ok(jd_candidate_shape_inspect(context,shape,&actual,&count,&valid,error,sizeof(error)));
        check(valid==1&&count==solids&&std::isfinite(actual)&&std::abs(actual-expected)<1e-7,"ABI actual BRep mass/topology");
    };
    uint64_t a=0,b=0,far=0,common=0,fused=0,cut=0,empty=0,copy=0,foreign=0;
    ok(jd_candidate_box(first.id,2,2,2,&a,error,sizeof(error)));
    ok(jd_candidate_box(second.id,1,1,1,&foreign,error,sizeof(error)));
    mass(first.id,a,8,1);
    double actual=999;uint32_t solids=999,valid=999;
    check(jd_candidate_shape_inspect(second.id,a,&actual,&solids,&valid,error,sizeof(error))==JD_CANDIDATE_INVALID_SHAPE,"Foreign shape must not retarget another context");
    ok(jd_candidate_translate(first.id,a,1,0,0,&b,error,sizeof(error)));
    ok(jd_candidate_translate(first.id,a,100,0,0,&far,error,sizeof(error)));
    ok(jd_candidate_boolean(first.id,JD_CANDIDATE_COMMON,a,b,.01,&common,error,sizeof(error)));
    ok(jd_candidate_boolean(first.id,JD_CANDIDATE_FUSE,a,b,.01,&fused,error,sizeof(error)));
    ok(jd_candidate_boolean(first.id,JD_CANDIDATE_CUT,a,b,.01,&cut,error,sizeof(error)));
    ok(jd_candidate_boolean(first.id,JD_CANDIDATE_COMMON,a,far,.01,&empty,error,sizeof(error)));
    mass(first.id,common,4,1);mass(first.id,fused,12,1);mass(first.id,cut,4,1);mass(first.id,empty,0,0);
    mass(first.id,a,8,1);mass(first.id,b,8,1);
    ok(jd_candidate_shape_clone(first.id,a,&copy,error,sizeof(error)));
    check(copy&&copy!=a,"Copy needs independent ownership ID");
    ok(jd_candidate_shape_retain(first.id,a,error,sizeof(error)));
    ok(jd_candidate_shape_release(first.id,a,error,sizeof(error)));
    mass(first.id,a,8,1);
    ok(jd_candidate_shape_release(first.id,a,error,sizeof(error)));
    check(jd_candidate_shape_release(first.id,a,error,sizeof(error))==JD_CANDIDATE_INVALID_SHAPE,"Double release");
    check(jd_candidate_shape_inspect(first.id,a,&actual,&solids,&valid,error,sizeof(error))==JD_CANDIDATE_INVALID_SHAPE,"Released shape remains stale");
    mass(first.id,copy,8,1);
    for(double bad:{-1.0,0.0,std::numeric_limits<double>::quiet_NaN(),std::numeric_limits<double>::infinity()}){
        uint64_t output=99;
        check(jd_candidate_box(first.id,bad,1,1,&output,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&output==0,"Invalid box must fail/zero output");
    }
    uint64_t output=99;
    check(jd_candidate_boolean(first.id,999,b,copy,.01,&output,error,sizeof(error))==JD_CANDIDATE_UNSUPPORTED_OPERATION&&output==0,"Unknown operation cannot fake success");
    output=99;
    check(jd_candidate_boolean(first.id,JD_CANDIDATE_COMMON,b,copy,-1,&output,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&output==0,"Negative fuzzy tolerance");
    output=99;
    check(jd_candidate_translate(first.id,b,std::numeric_limits<double>::infinity(),0,0,&output,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT&&output==0,"Invalid translation");
    char guarded[4]={'x','y','z','w'};
    check(jd_candidate_box(first.id,-1,1,1,&output,guarded+1,1)==JD_CANDIDATE_INVALID_ARGUMENT,"Tiny diagnostic status");
    check(guarded[0]=='x'&&guarded[1]==0&&guarded[2]=='z'&&guarded[3]=='w',"Diagnostic buffer overflow");
    check(jd_candidate_box(first.id,-1,1,1,&output,nullptr,0)==JD_CANDIDATE_INVALID_ARGUMENT,"Optional diagnostic buffer");
    check(jd_candidate_box(first.id,1,1,1,nullptr,error,sizeof(error))==JD_CANDIDATE_INVALID_ARGUMENT,"Null shape output");
    for(uint64_t shape:{b,far,common,fused,cut,empty,copy})ok(jd_candidate_shape_release(first.id,shape,error,sizeof(error)));
    uint64_t live=999;ok(jd_candidate_context_live_shapes(first.id,&live,error,sizeof(error)));check(live==0,"Owned entries after disposal");
    for(int i=0;i<64;i++){
        uint64_t shape=0;ok(jd_candidate_box(first.id,1,1,1,&shape,error,sizeof(error)));
        mass(first.id,shape,1,1);ok(jd_candidate_shape_release(first.id,shape,error,sizeof(error)));
    }
    ok(jd_candidate_context_live_shapes(first.id,&live,error,sizeof(error)));check(live==0,"Repeated native handle cycles retained entries");
    const auto closed=second.id;ok(jd_candidate_context_release(second.id,error,sizeof(error)));second.id=0;
    check(jd_candidate_shape_inspect(closed,foreign,&actual,&solids,&valid,error,sizeof(error))==JD_CANDIDATE_INVALID_CONTEXT,"Closed context must reject handles");
    check(jd_candidate_context_release(closed,error,sizeof(error))==JD_CANDIDATE_INVALID_CONTEXT,"Closed context double release");
}
