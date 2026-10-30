#include "candidate_paired_sweep.hpp"
#include <GeomAdaptor_Curve.hxx>
#include <GeomAPI_ProjectPointOnCurve.hxx>
#include <GeomFill_NSections.hxx>
#include <Geom_BSplineSurface.hxx>
#include <GeomFill_Sweep.hxx>
#include <BRepBuilderAPI_MakeFace.hxx>
#include <BRepCheck_Analyzer.hxx>
#include <BndLib_Add3dCurve.hxx>
#include <Bnd_Box.hxx>
#include <NCollection_Sequence.hxx>
#include <gp_Mat.hxx>
#include <gp_Vec.hxx>
#include <algorithm>
#include <array>
#include <cmath>
#include <limits>
#include <stdexcept>
#include <sstream>

namespace jewel::candidate {
namespace {
struct VectorJet {gp_Vec value,d1,d2;};
struct UnitJet {VectorJet unit;double length,d1,d2;};
UnitJet normalized(const VectorJet& input){
 double r=input.value.Magnitude();if(!std::isfinite(r)||r<=std::numeric_limits<double>::min())throw std::invalid_argument("Degenerate paired-rail frame");
 double dr=input.value.Dot(input.d1)/r;
 double d2r=(input.d1.SquareMagnitude()+input.value.Dot(input.d2)-dr*dr)/r;
 auto u=input.value/r;
 auto du=(input.d1-u*dr)/r;
 auto d2u=(input.d2-du*(2*dr)-u*d2r)/r;
 return {{u,du,d2u},r,dr,d2r};
}
VectorJet cross(const VectorJet& a,const VectorJet& b){return {a.value.Crossed(b.value),a.d1.Crossed(b.value)+a.value.Crossed(b.d1),a.d2.Crossed(b.value)+a.d1.Crossed(b.d1)*2+a.value.Crossed(b.d2)};}

double checkedDistance(const gp_Pnt& first,const gp_Pnt& second){double distance=first.Distance(second);if(!std::isfinite(distance))throw std::runtime_error("Nonfinite paired-surface distance");return distance;}
struct ScalarJet {double value,d1,d2;};
VectorJet scaled(const VectorJet& v,const ScalarJet& s){return {v.value*s.value,v.d1*s.value+v.value*s.d1,v.d2*s.value+v.d1*(2*s.d1)+v.value*s.d2};}
VectorJet added(const VectorJet& a,const VectorJet& b){return {a.value+b.value,a.d1+b.d1,a.d2+b.d2};}
struct ParameterJet {double parameter,d1,d2;};


// Pinned NSections misses profile rationality when weights are constant along the path.
class RationalSections final:public GeomFill_NSections {
public:
 RationalSections(const NCollection_Sequence<occ::handle<Geom_Curve>>& curves,const NCollection_Sequence<double>& parameters,double first,double last):GeomFill_NSections(curves,parameters,0,1,first,last){}
 bool IsRational()const override{auto surface=BSplineSurface();return !surface.IsNull()&&(surface->IsURational()||surface->IsVRational());}
 void GetMinimalWeight(NCollection_Array1<double>& weights)const override{
  auto surface=BSplineSurface();if(surface.IsNull())throw std::runtime_error("Missing rational section surface");
  if(weights.Length()!=static_cast<size_t>(surface->NbUPoles()))throw std::invalid_argument("Rational section weight buffer mismatch");
  for(int i=1;i<=surface->NbUPoles();i++){double minimum=surface->Weight(i,1);for(int j=2;j<=surface->NbVPoles();j++)minimum=std::min(minimum,surface->Weight(i,j));if(!std::isfinite(minimum)||minimum<=0)throw std::invalid_argument("Nonpositive rational section weight");weights(weights.Lower()+i-1)=minimum;}
 }
};

struct FrameJet {gp_Mat matrix,d1,d2;gp_Vec origin,v1,v2;};
class PairedLocation final:public GeomFill_LocationLaw {
 occ::handle<Adaptor3d_Curve> primary_,guide_;
 std::vector<double> a_,b_;
 bool height_;
 double intervalFirst_,intervalLast_,bound_=0;
 gp_Mat left_;

 PairedCorrespondence mode_;
 std::vector<std::array<double,4>> rotations_;
 void initializeRotations(){
  rotations_.clear();
  for(size_t i=0;i<a_.size();i++){
   gp_Pnt p;gp_Vec p1;primary_->D1(a_[i],p,p1);gp_Vec span(p,guide_->Value(b_[i]));
   auto tangent=normalized({p1,{},{}}).unit.value,unitSpan=normalized({span,{},{}}).unit.value;
   auto normal=normalized({p1.Crossed(span),{},{}}).unit.value;
   auto planeNormal=unitSpan.Crossed(normal);double w=std::sqrt(std::max(0.0,(1+tangent.Dot(planeNormal))*.5));
   if(!std::isfinite(w)||w<=1e-12)throw std::invalid_argument("Unsupported paired section rotation");
   auto axis=tangent.Crossed(planeNormal)/(2*w);rotations_.push_back({w,axis.X(),axis.Y(),axis.Z()});
  }
 }
 VectorJet sectionPlane(double t,size_t i)const{
  double width=a_[i+1]-a_[i],f=(t-a_[i])/width;
  std::array<double,4> q{},dq{},ddq{};double norm=0,dot=0,dot2=0;
  for(int j=0;j<4;j++){q[j]=rotations_[i][j]+f*(rotations_[i+1][j]-rotations_[i][j]);dq[j]=(rotations_[i+1][j]-rotations_[i][j])/width;norm+=q[j]*q[j];dot+=q[j]*dq[j];dot2+=dq[j]*dq[j];}
  norm=std::sqrt(norm);double dn=dot/norm,d2n=(dot2-dn*dn)/norm;
  for(int j=0;j<4;j++){double u=q[j]/norm,du=(dq[j]-u*dn)/norm;ddq[j]=(-2*du*dn-u*d2n)/norm;q[j]=u;dq[j]=du;}
  gp_Pnt p;gp_Vec p1,p2,p3;primary_->D3(t,p,p1,p2,p3);auto tangent=normalized({p1,p2,p3}).unit;
  VectorJet axis{gp_Vec(q[1],q[2],q[3]),gp_Vec(dq[1],dq[2],dq[3]),gp_Vec(ddq[1],ddq[2],ddq[3])};
  auto twice=scaled(cross(axis,tangent),{2,0,0});
  return added(added(tangent,scaled(twice,{q[0],dq[0],ddq[0]})),cross(axis,twice));
 }
 ParameterJet planeCorrespondence(double t,size_t i,double seed)const{
  auto plane=sectionPlane(t,i);gp_Pnt p;gp_Vec p1,p2;primary_->D2(t,p,p1,p2);
  auto value=[&](double u){return plane.value.Dot(gp_Vec(p,guide_->Value(u)));};
  double lower=std::min(b_[i],b_[i+1]),upper=std::max(b_[i],b_[i+1]);
  double u=t==a_[i]?b_[i]:t==a_[i+1]?b_[i+1]:seed;
  double fLower=value(lower),fUpper=value(upper);bool bracket=fLower==0||fUpper==0||(fLower<0)!=(fUpper<0);
  bool anchor=t==a_[i]||t==a_[i+1];
  if(!anchor&&!bracket)throw std::invalid_argument("Section plane has no bracketed guide intersection");
  bool converged=false;
  for(int iteration=0;iteration<80;iteration++){
   gp_Pnt q;gp_Vec q1;guide_->D1(u,q,q1);double f=plane.value.Dot(gp_Vec(p,q));
   double tolerance=1e-12*std::max(1.0,gp_Vec(p,q).Magnitude());
   if(std::abs(f)<=tolerance){converged=true;break;}
   if(anchor)throw std::invalid_argument("Paired section anchor fails its plane constraint");
   if((fLower<0)!=(f<0)){upper=u;fUpper=f;}else{lower=u;fLower=f;}
   double derivative=plane.value.Dot(q1),next=u-f/derivative;
   if(!std::isfinite(next)||next<=lower||next>=upper)next=lower+(upper-lower)*.5;
   if(next==u)break;u=next;
  }
  if(!converged)throw std::invalid_argument("Paired guide-plane root did not converge");
  gp_Pnt q;gp_Vec q1,q2;guide_->D2(u,q,q1,q2);gp_Vec span(p,q);
  double denominator=plane.value.Dot(q1);
  if(!std::isfinite(denominator)||std::abs(denominator)<=1e-14*q1.Magnitude())throw std::invalid_argument("Singular paired guide-plane derivative");
  double d1=(plane.value.Dot(p1)-plane.d1.Dot(span))/denominator;
  double d2=-(plane.d2.Dot(span)-2*plane.d1.Dot(p1)-plane.value.Dot(p2)+2*plane.d1.Dot(q1)*d1+plane.value.Dot(q2)*d1*d1)/denominator;
  return {u,d1,d2};
 }

 ParameterJet correspondence(double t)const{
  if(!std::isfinite(t)||t<a_.front()||t>a_.back())throw std::invalid_argument("Paired parameter outside supported interval");
  auto iterator=std::upper_bound(a_.begin(),a_.end(),t);
  size_t i=iterator==a_.begin()?0:static_cast<size_t>(iterator-a_.begin()-1);
  i=std::min(i,a_.size()-2);
  // At an interval's upper break use the left derivative, not the next piece.
  if(i>0&&t==a_[i]&&t==intervalLast_)--i;
  double slope=(b_[i+1]-b_[i])/(a_[i+1]-a_[i]);
  double u=t==a_[i]?b_[i]:t==a_[i+1]?b_[i+1]:b_[i]+(t-a_[i])*slope;
  if(mode_==PairedCorrespondence::SectionPlanes)return planeCorrespondence(t,i,u);
  return {u,slope,0};
 }
 FrameJet evaluate(double t)const{
  auto mapped=correspondence(t);gp_Pnt p,q;gp_Vec p1,p2,p3,q1,q2;
  primary_->D3(t,p,p1,p2,p3);guide_->D2(mapped.parameter,q,q1,q2);
  VectorJet span{gp_Vec(p,q),q1*mapped.d1-p1,q2*(mapped.d1*mapped.d1)+q1*mapped.d2-p2};
  auto unitSpan=normalized(span);
  auto normal=normalized(cross({p1,p2,p3},span)).unit;
  auto tangent=cross(unitSpan.unit,normal);
  double h=height_?1:unitSpan.length,h1=height_?0:unitSpan.d1,h2=height_?0:unitSpan.d2;
  VectorJet vertical{normal.value*h,normal.d1*h+normal.value*h1,normal.d2*h+normal.d1*(2*h1)+normal.value*h2};
  FrameJet result;
  result.matrix.SetCols(span.value.XYZ(),vertical.value.XYZ(),tangent.value.XYZ());
  result.d1.SetCols(span.d1.XYZ(),vertical.d1.XYZ(),tangent.d1.XYZ());
  result.d2.SetCols(span.d2.XYZ(),vertical.d2.XYZ(),tangent.d2.XYZ());
  result.matrix=left_*result.matrix;result.d1=left_*result.d1;result.d2=left_*result.d2;
  result.origin=gp_Vec(p.XYZ());result.v1=p1;result.v2=p2;
  for(int row=1;row<=3;row++){for(int column=1;column<=3;column++)if(!std::isfinite(result.matrix(row,column))||!std::isfinite(result.d1(row,column))||!std::isfinite(result.d2(row,column)))throw std::invalid_argument("Nonfinite paired-rail frame");if(!std::isfinite(result.origin.Coord(row)))throw std::invalid_argument("Nonfinite paired-rail origin");}
  return result;
 }

 double inversePlaneBreak(double parameter,size_t segment)const{
  gp_Pnt guidePoint=guide_->Value(parameter);double lower=a_[segment],upper=a_[segment+1];
  auto value=[&](double t){auto plane=sectionPlane(t,segment);return plane.value.Dot(gp_Vec(primary_->Value(t),guidePoint));};
  double fl=value(lower),fu=value(upper);double t=lower+(upper-lower)*(parameter-b_[segment])/(b_[segment+1]-b_[segment]);
  if((fl<0)==(fu<0)&&fl!=0&&fu!=0)throw std::invalid_argument("Guide knot has no bracketed section-plane preimage");
  for(int iteration=0;iteration<80;iteration++){
   auto plane=sectionPlane(t,segment);gp_Pnt p;gp_Vec p1;primary_->D1(t,p,p1);gp_Vec span(p,guidePoint);double f=plane.value.Dot(span);
   if(std::abs(f)<=1e-12*std::max(1.0,span.Magnitude()))return t;
   if((fl<0)!=(f<0)){upper=t;fu=f;}else{lower=t;fl=f;}
   double derivative=plane.d1.Dot(span)-plane.value.Dot(p1),next=t-f/derivative;
   if(!std::isfinite(next)||next<=lower||next>=upper)next=lower+(upper-lower)*.5;
   if(next==t)break;t=next;
  }
  throw std::invalid_argument("Guide knot section-plane preimage did not converge");
 }
 mutable std::vector<double> fullBreaks_;
 std::vector<double> breaks()const{
  if(fullBreaks_.empty()){
   std::vector<double> values{a_.front(),a_.back()};auto add=[&](double t){if(t>a_.front()&&t<a_.back())values.push_back(t);};
   for(double t:a_)add(t);
   NCollection_Array1<double> p(1,primary_->NbIntervals(GeomAbs_C3)+1);primary_->Intervals(p,GeomAbs_C3);for(int i=p.Lower();i<=p.Upper();i++)add(p(i));
   NCollection_Array1<double> q(1,guide_->NbIntervals(GeomAbs_C2)+1);guide_->Intervals(q,GeomAbs_C2);
   for(int j=q.Lower();j<=q.Upper();j++)for(size_t i=0;i+1<a_.size();i++)if(b_[i]!=b_[i+1]){
    double ratio=(q(j)-b_[i])/(b_[i+1]-b_[i]);if(ratio>0&&ratio<1)add(mode_==PairedCorrespondence::SectionPlanes?inversePlaneBreak(q(j),i):a_[i]+ratio*(a_[i+1]-a_[i]));
   }
   std::sort(values.begin(),values.end());values.erase(std::unique(values.begin(),values.end()),values.end());fullBreaks_=std::move(values);
  }
  std::vector<double> values{intervalFirst_};for(double t:fullBreaks_)if(t>intervalFirst_&&t<intervalLast_)values.push_back(t);values.push_back(intervalLast_);return values;
 }

public:
 PairedLocation(const occ::handle<Geom_Curve>& first,const occ::handle<Geom_Curve>& second,const std::vector<double>& a,const std::vector<double>& b,bool height,PairedCorrespondence mode):a_(a),b_(b),height_(height),mode_(mode){
  if(first.IsNull()||second.IsNull()||a.size()<2||a.size()>4096||a.size()!=b.size())throw std::invalid_argument("Paired sweep requires two curves and matching bounded parameter lists");
  for(size_t i=0;i<a.size();i++)if(!std::isfinite(a[i])||!std::isfinite(b[i])||(i>0&&a[i]<=a[i-1])||a[i]<first->FirstParameter()||a[i]>first->LastParameter()||b[i]<second->FirstParameter()||b[i]>second->LastParameter())throw std::invalid_argument("Invalid paired-rail parameter sequence");
  primary_=new GeomAdaptor_Curve(first,a.front(),a.back());guide_=new GeomAdaptor_Curve(second);
  intervalFirst_=a.front();intervalLast_=a.back();left_.SetIdentity();
  if(mode_!=PairedCorrespondence::ExplicitLinear&&mode_!=PairedCorrespondence::SectionPlanes)throw std::invalid_argument("Unknown paired correspondence mode");
  if(mode_==PairedCorrespondence::SectionPlanes)initializeRotations();
 }
 bool SetCurve(const occ::handle<Adaptor3d_Curve>& curve)override{if(curve.IsNull()||curve->FirstParameter()>a_.front()||curve->LastParameter()<a_.back())throw std::invalid_argument("Unsupported paired primary curve interval");primary_=curve;bound_=0;fullBreaks_.clear();if(mode_==PairedCorrespondence::SectionPlanes)initializeRotations();return true;}
 const occ::handle<Adaptor3d_Curve>& GetCurve()const override{return primary_;}
 void SetTrsf(const gp_Mat& value)override{left_=value;bound_=0;}
 occ::handle<GeomFill_LocationLaw> Copy()const override{return new PairedLocation(*this);}
 bool D0(double t,gp_Mat& m,gp_Vec& v)override{auto f=evaluate(t);m=f.matrix;v=f.origin;return true;}
 bool D0(double t,gp_Mat& m,gp_Vec& v,NCollection_Array1<gp_Pnt2d>&)override{return D0(t,m,v);}
 bool D1(double t,gp_Mat& m,gp_Vec& v,gp_Mat& dm,gp_Vec& dv,NCollection_Array1<gp_Pnt2d>&,NCollection_Array1<gp_Vec2d>&)override{auto f=evaluate(t);m=f.matrix;v=f.origin;dm=f.d1;dv=f.v1;return true;}
 bool D2(double t,gp_Mat& m,gp_Vec& v,gp_Mat& dm,gp_Vec& dv,gp_Mat& d2m,gp_Vec& d2v,NCollection_Array1<gp_Pnt2d>&,NCollection_Array1<gp_Vec2d>&,NCollection_Array1<gp_Vec2d>&)override{auto f=evaluate(t);m=f.matrix;v=f.origin;dm=f.d1;dv=f.v1;d2m=f.d2;d2v=f.v2;return true;}
 int NbIntervals(GeomAbs_Shape)const override{return static_cast<int>(breaks().size()-1);}
 void Intervals(NCollection_Array1<double>& values,GeomAbs_Shape)const override{auto b=breaks();if(values.Length()!=b.size())throw std::invalid_argument("Paired interval buffer mismatch");for(size_t i=0;i<b.size();i++)values(values.Lower()+static_cast<int>(i))=b[i];}
 void SetInterval(double first,double last)override{if(!std::isfinite(first)||!std::isfinite(last)||first<a_.front()||last>a_.back()||first>=last)throw std::invalid_argument("Invalid paired subinterval");intervalFirst_=first;intervalLast_=last;}
 void GetInterval(double& first,double& last)const override{first=intervalFirst_;last=intervalLast_;}
 void GetDomain(double& first,double& last)const override{first=a_.front();last=a_.back();}
 double GetMaximalNorm()override{
  if(bound_>0)return bound_;
  Bnd_Box p,q;BndLib_Add3dCurve::AddOptimal(*primary_,a_.front(),a_.back(),1e-9,p);
  double q0=*std::min_element(b_.begin(),b_.end()),q1=*std::max_element(b_.begin(),b_.end());
  if(q0==q1)q.Add(guide_->Value(q0));else BndLib_Add3dCurve::AddOptimal(*guide_,q0,q1,1e-9,q);
  if(p.IsVoid()||q.IsVoid()||p.IsOpen()||q.IsOpen())throw std::invalid_argument("Unbounded paired-rail frame");
  double x0,y0,z0,x1,y1,z1,u0,v0,w0,u1,v1,w1;p.Get(x0,y0,z0,x1,y1,z1);q.Get(u0,v0,w0,u1,v1,w1);
  double span=std::hypot(std::max(std::abs(x0-u1),std::abs(x1-u0)),std::max(std::abs(y0-v1),std::abs(y1-v0)),std::max(std::abs(z0-w1),std::abs(z1-w0)));
  double norm=0;for(int r=1;r<=3;r++)for(int c=1;c<=3;c++)norm+=left_(r,c)*left_(r,c);
  bound_=std::max(1.0,span)*std::sqrt(norm);if(!std::isfinite(bound_))throw std::invalid_argument("Nonfinite paired-rail norm bound");return bound_;
 }
 void GetAverageLaw(gp_Mat& m,gp_Vec& v)override{for(int r=1;r<=3;r++)for(int c=1;c<=3;c++)m.SetValue(r,c,0);v=gp_Vec(0,0,0);for(int i=0;i<16;i++){auto f=evaluate(intervalFirst_+(intervalLast_-intervalFirst_)*(i+.5)/16);m=m+f.matrix*(1.0/16);v=v+f.origin*(1.0/16);}}
};
}
occ::handle<GeomFill_LocationLaw> make_paired_location(const occ::handle<Geom_Curve>& first,const occ::handle<Geom_Curve>& second,const std::vector<double>& a,const std::vector<double>& b,bool height,PairedCorrespondence mode){return new PairedLocation(first,second,a,b,height,mode);}
namespace {
double profileContact(const occ::handle<Geom_BSplineCurve>& profile,const gp_Pnt& point,double tolerance){
 if(profile.IsNull())throw std::invalid_argument("Missing paired sweep profile");double first=profile->FirstParameter(),last=profile->LastParameter();
 double best=first,distance=profile->Value(first).Distance(point);double endDistance=profile->Value(last).Distance(point);if(endDistance<distance){best=last;distance=endDistance;}
 GeomAPI_ProjectPointOnCurve projected(point,profile,first,last);for(int i=1;i<=projected.NbPoints();i++)if(projected.Distance(i)<distance){best=projected.Parameter(i);distance=projected.Distance(i);}
 if(!std::isfinite(distance)||distance>tolerance)throw std::invalid_argument("Profile does not meet its paired rail point");
 double normalized=(best-first)/(last-first);if(!std::isfinite(normalized)||normalized<0||normalized>1)throw std::invalid_argument("Invalid normalized profile contact");if(profile->IsClosed()&&normalized>1-1e-8)normalized=0;return normalized;
}
}
PairedSweepResult make_paired_sweep(const occ::handle<Geom_Curve>& first,const occ::handle<Geom_Curve>& second,const std::vector<occ::handle<Geom_BSplineCurve>>& profiles,const std::vector<double>& a,const std::vector<double>& b,bool height,double tolerance,PairedCorrespondence mode){
 if(profiles.size()!=a.size()||!std::isfinite(tolerance)||tolerance<=0)throw std::invalid_argument("Invalid paired sweep profiles or tolerance");
 auto location=make_paired_location(first,second,a,b,height,mode);
 double railU1=profileContact(profiles[0],first->Value(a[0]),tolerance),railU2=profileContact(profiles[0],second->Value(b[0]),tolerance);
 for(size_t i=1;i<profiles.size();i++)if(std::abs(profileContact(profiles[i],first->Value(a[i]),tolerance)-railU1)>1e-8||std::abs(profileContact(profiles[i],second->Value(b[i]),tolerance)-railU2)>1e-8)throw std::invalid_argument("Profiles require compatible normalized rail contacts");
 NCollection_Sequence<occ::handle<Geom_Curve>> local;NCollection_Sequence<double> params;
 for(size_t i=0;i<profiles.size();i++){
  if(profiles[i].IsNull())throw std::invalid_argument("Missing paired sweep profile");
  gp_Mat m;gp_Vec v;location->D0(a[i],m,v);auto inverse=m.Inverted();
  auto curve=occ::down_cast<Geom_BSplineCurve>(profiles[i]->Copy());
  for(int j=1;j<=curve->NbPoles();j++)curve->SetPole(j,gp_Pnt(inverse*(curve->Pole(j).XYZ()-v.XYZ())));
  double start=curve->FirstParameter(),end=curve->LastParameter();if(!std::isfinite(start)||!std::isfinite(end)||end<=start)throw std::invalid_argument("Invalid paired profile domain");
  NCollection_Array1<double> knots(curve->Knots());for(int j=knots.Lower();j<=knots.Upper();j++)knots(j)=(knots(j)-start)/(end-start);curve->SetKnots(knots);
  local.Append(curve);params.Append(a[i]);
 }
 occ::handle<GeomFill_SectionLaw> sections=new RationalSections(local,params,a.front(),a.back());
 double sectionLoss=0;auto sectionSurface=sections->BSplineSurface();
 for(size_t row=0;row<profiles.size();row++)for(int sample=0;sample<=32;sample++){double u=sample/32.0;sectionLoss=std::max(sectionLoss,checkedDistance(sectionSurface->Value(u,a[row]),local.Value(static_cast<int>(row)+1)->Value(u)));}
 if(sectionLoss>tolerance){std::ostringstream message;message.precision(17);message<<"Native section interpolation loss="<<sectionLoss;throw std::runtime_error(message.str());}
 // Avoid topological helpers that coerce a nonuniform affine frame into gp_Trsf.
 GeomFill_Sweep sweep(location,false);sweep.SetDomain(a.front(),a.back(),a.front(),a.back());sweep.SetTolerance(tolerance,tolerance,tolerance*.1,1e-7);
 sweep.Build(sections,GeomFill_Section,GeomAbs_C0,11,128);
 if(!sweep.IsDone())throw std::runtime_error("Paired native surface approximation failed");
 auto surface=sweep.Surface();double error=sweep.ErrorOnSurface();
 if(surface.IsNull()||!std::isfinite(error)||error>tolerance){std::ostringstream diagnostic;diagnostic.precision(17);diagnostic<<"Paired sweep approximation error="<<error<<" requested="<<tolerance;throw std::runtime_error(diagnostic.str());}
 // Independently preserve each supplied full profile, not only the kernel error estimate.
 double profileLoss=0;for(size_t row=0;row<profiles.size();row++)for(int sample=0;sample<=32;sample++){
  double u=sample/32.0,source=profiles[row]->FirstParameter()+u*(profiles[row]->LastParameter()-profiles[row]->FirstParameter());
  profileLoss=std::max(profileLoss,checkedDistance(surface->Value(u,a[row]),profiles[row]->Value(source)));
 }
 if(profileLoss>tolerance){std::ostringstream message;message.precision(17);message<<"Paired sweep profile loss="<<profileLoss<<" requested="<<tolerance;throw std::runtime_error(message.str());}
 for(int sample=0;sample<=64;sample++){
  double t=a.front()+(a.back()-a.front())*sample/64;gp_Mat m;gp_Vec v;location->D0(t,m,v);
  if(checkedDistance(surface->Value(railU1,t),first->Value(t))>tolerance||checkedDistance(surface->Value(railU2,t),gp_Pnt(v.XYZ()+m.Column(1)))>tolerance)throw std::runtime_error("Paired surface leaves a rail constraint");
 }
 BRepBuilderAPI_MakeFace face(surface,tolerance);if(!face.IsDone()||!BRepCheck_Analyzer(face.Shape()).IsValid())throw std::runtime_error("Paired sweep native face invalid");
 return {surface,face.Shape(),error};
}
}
