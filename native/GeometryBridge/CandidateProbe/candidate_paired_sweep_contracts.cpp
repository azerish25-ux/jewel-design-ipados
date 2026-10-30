#include "candidate_paired_sweep.hpp"
#include <Geom_BSplineCurve.hxx>
#include <Geom_BezierCurve.hxx>
#include <BRepCheck_Analyzer.hxx>
#include <BRepGProp.hxx>
#include <GProp_GProps.hxx>
#include <NCollection_Array1.hxx>
#include <gp_Mat.hxx>
#include <gp_Vec.hxx>
#include <gp_Pnt.hxx>
#include <array>
#include <cmath>
#include <limits>
#include <sstream>
#include <stdexcept>

int run_candidate_paired_abi_contracts();
namespace {
void check_pair(bool ok,const char* message){if(!ok)throw std::runtime_error(message);}
void near_pair(double a,double b,double tol=1e-6){if(!std::isfinite(a)||std::abs(a-b)>tol){std::ostringstream message;message.precision(17);message<<"Paired sweep numeric mismatch actual="<<a<<" expected="<<b<<" tolerance="<<tol;throw std::runtime_error(message.str());}}
void point_pair(gp_Pnt a,gp_Pnt b,double tol=1e-6){check_pair(a.Distance(b)<=tol,"Paired sweep misses exact rail/profile contact");}
template<class F>void rejected_pair(F action){try{action();}catch(const std::invalid_argument&){return;}throw std::runtime_error("Expected explicit invalid paired-rail input");}
occ::handle<Geom_BSplineCurve> line_pair(gp_Pnt a,gp_Pnt b,double first,double last){
 NCollection_Array1<gp_Pnt> poles(1,2);poles(1)=a;poles(2)=b;
 NCollection_Array1<double> knots(1,2);knots(1)=first;knots(2)=last;
 NCollection_Array1<int> mult(1,2);mult(1)=2;mult(2)=2;
 return new Geom_BSplineCurve(poles,knots,mult,1,false);
}
occ::handle<Geom_BSplineCurve> section_pair(double x,double qx,double h){
 NCollection_Array1<gp_Pnt> p(1,5);p(1)=gp_Pnt(x,0,0);p(2)=gp_Pnt(qx,4,0);p(3)=gp_Pnt(qx,4,h);p(4)=gp_Pnt(x,0,h);p(5)=p(1);
 NCollection_Array1<double> k(1,5);NCollection_Array1<int> m(1,5);for(int i=1;i<=5;i++){k(i)=(i-1)*.25;m(i)=i==1||i==5?2:1;}return new Geom_BSplineCurve(p,k,m,1,false);
}

occ::handle<Geom_BSplineCurve> circle_pair(double x){
 NCollection_Array1<gp_Pnt> p(1,9);std::array<std::array<double,2>,9> yz={{{0,0},{0,-2},{2,-2},{4,-2},{4,0},{4,2},{2,2},{0,2},{0,0}}};
 NCollection_Array1<double> weights(1,9),knots(1,5);NCollection_Array1<int> mult(1,5);
 for(int i=1;i<=9;i++){p(i)=gp_Pnt(x,yz[i-1][0],yz[i-1][1]);weights(i)=i%2?1:std::sqrt(.5);}
 for(int i=1;i<=5;i++){knots(i)=(i-1)*.25;mult(i)=i==1||i==5?3:2;}
 return new Geom_BSplineCurve(p,weights,knots,mult,2,false,false);
}

gp_Pnt transformed_pair(const gp_Mat& m,const gp_Vec& v,gp_Pnt p){return gp_Pnt(m*p.XYZ()+v.XYZ());}
}
std::string run_candidate_paired_sweep_contracts(){
 using jewel::candidate::make_paired_location;using jewel::candidate::make_paired_sweep;
 auto first=line_pair(gp_Pnt(0,0,0),gp_Pnt(10,0,0),2,12),second=line_pair(gp_Pnt(0,4,0),gp_Pnt(10,4,0),40,60);
 std::vector<double> a={2,5,12},b={40,54,60};auto law=make_paired_location(first,second,a,b,true);
 int pointChecks=0,derivativeChecks=0;
 for(int i=0;i<3;i++){gp_Mat m;gp_Vec v;check_pair(law->D0(a[i],m,v),"Paired frame failed");point_pair(transformed_pair(m,v,gp_Pnt(0,0,0)),first->Value(a[i]));point_pair(transformed_pair(m,v,gp_Pnt(1,0,0)),second->Value(b[i]));near_pair(gp_Vec(m.Column(2)).Magnitude(),1);pointChecks+=2;}
 auto changed=make_paired_location(first,second,a,{40,46,60},true);gp_Mat oldM,newM;gp_Vec oldV,newV;law->D0(5,oldM,oldV);changed->D0(5,newM,newV);near_pair(transformed_pair(oldM,oldV,gp_Pnt(1,0,0)).X(),7);near_pair(transformed_pair(newM,newV,gp_Pnt(1,0,0)).X(),3);
 auto uniform=make_paired_location(first,second,a,b,false);uniform->D0(5,newM,newV);near_pair(gp_Vec(newM.Column(2)).Magnitude(),std::sqrt(32.0));
 rejected_pair([&]{make_paired_location(first,second,{2,5,5},b,true);});rejected_pair([&]{make_paired_location(first,second,a,{40,54},true);});rejected_pair([&]{make_paired_location(first,second,a,{40,54,std::numeric_limits<double>::quiet_NaN()},true);});rejected_pair([&]{make_paired_location(first,second,a,{40,54,61},true);});
 auto copy=law->Copy();copy->SetInterval(3,4);double lo,hi;law->GetInterval(lo,hi);near_pair(lo,2);near_pair(hi,12);
 gp_Mat left;left.SetIdentity();left.SetValue(1,2,.25);copy->SetTrsf(left);law->D0(3.5,oldM,oldV);copy->D0(3.5,newM,newV);auto expected=left*oldM;for(int r=1;r<=3;r++)for(int c=1;c<=3;c++)near_pair(newM(r,c),expected(r,c));point_pair(gp_Pnt(oldV.XYZ()),gp_Pnt(newV.XYZ()));
 // Independent finite differences on curved native input, away from correspondence breaks.
 NCollection_Array1<gp_Pnt> p(1,4),q(1,4);p(1)=gp_Pnt(0,0,0);p(2)=gp_Pnt(3,1,0);p(3)=gp_Pnt(7,-1,2);p(4)=gp_Pnt(10,0,1);for(int i=1;i<=4;i++)q(i)=p(i).Translated(gp_Vec(i*.2,4+i*.1,1));
 occ::handle<Geom_Curve> curved=new Geom_BezierCurve(p),guide=new Geom_BezierCurve(q);
 for(auto mode:{jewel::candidate::PairedCorrespondence::ExplicitLinear,jewel::candidate::PairedCorrespondence::SectionPlanes})for(bool height:{false,true}){auto f=make_paired_location(curved,guide,{0,.35,1},{0,.65,1},height,mode);for(double t:{.12,.24,.55,.72}){
  gp_Mat m,dm,d2m,minus,plus;gp_Vec v,dv,d2v,vm,vp;NCollection_Array1<gp_Pnt2d> points;NCollection_Array1<gp_Vec2d> dpoints,d2points;
  check_pair(f->D2(t,m,v,dm,dv,d2m,d2v,points,dpoints,d2points),"Paired frame derivatives failed");double h=1e-4;f->D0(t-h,minus,vm);f->D0(t+h,plus,vp);
  for(int r=1;r<=3;r++){near_pair(dv.Coord(r),(vp.Coord(r)-vm.Coord(r))/(2*h),1e-5);near_pair(d2v.Coord(r),(vp.Coord(r)-2*v.Coord(r)+vm.Coord(r))/(h*h),1e-4);for(int c=1;c<=3;c++){near_pair(dm(r,c),(plus(r,c)-minus(r,c))/(2*h),1e-5);near_pair(d2m(r,c),(plus(r,c)-2*m(r,c)+minus(r,c))/(h*h),2e-4);}derivativeChecks+=8;}
 }}
 std::vector<occ::handle<Geom_BSplineCurve>> profiles={section_pair(0,0,1),section_pair(3,7,2),section_pair(10,10,1)};
 auto result=make_paired_sweep(first,second,profiles,a,b,true,1e-7);check_pair(!result.shape.IsNull()&&BRepCheck_Analyzer(result.shape).IsValid(),"Paired native face invalid");
 double u0,u1,v0,v1;result.surface->Bounds(u0,u1,v0,v1);double worst=0;
 auto observe=[&](double u,double t,gp_Pnt expected){auto actual=result.surface->Value(u0+u*(u1-u0),v0+(t-a.front())/(a.back()-a.front())*(v1-v0));double d=actual.Distance(expected);worst=std::max(worst,d);check_pair(d<=1e-6,"Paired native surface violates input constraints");pointChecks++;};
 for(int i=0;i<3;i++)for(int j=0;j<=20;j++){double u=j/20.0;observe(u,a[i],profiles[i]->Value(u));}
 for(int j=0;j<=100;j++){double t=2+j*.1;double mapped=t<=5?40+(t-2)*14/3:54+(t-5)*6/7;observe(0,t,first->Value(t));observe(.25,t,second->Value(mapped));}
 auto cylinder=make_paired_sweep(first,second,{circle_pair(0),circle_pair(3),circle_pair(10)},a,{40,46,60},true,1e-7);
 GProp_GProps area;double integrationError=BRepGProp::SurfaceProperties(cylinder.shape,area,1e-10);check_pair(std::isfinite(integrationError)&&integrationError<=1e-8,"Adaptive rational surface area integration failed");near_pair(area.Mass(),40*std::acos(-1.0),1e-6);
 double cu0,cu1,cv0,cv1;cylinder.surface->Bounds(cu0,cu1,cv0,cv1);
 for(int j=0;j<=64;j++){auto point=cylinder.surface->Value(cu0+(cu1-cu0)*j/64.0,(cv0+cv1)*.5);near_pair((point.Y()-2)*(point.Y()-2)+point.Z()*point.Z(),4,1e-6);}
 int abiGuards=run_candidate_paired_abi_contracts();
 std::ostringstream report;report.precision(17);report<<"paired-rail-native-surface=pass; paired-contact-samples="<<pointChecks<<"; paired-derivative-components="<<derivativeChecks<<"; paired-max-residual="<<worst<<"; paired-surface-error="<<result.approximationError<<"; paired-abi-guards="<<abiGuards<<"; rational-profile-cylinder=pass; original134-full-block-parity=unverified; ";return report.str();
}
