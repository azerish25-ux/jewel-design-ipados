// Test-only observation types for unmodified pinned published plane methods.
// No production builder calls this entry point; not an original Rhino5 binary.
#include <algorithm>
#include <array>
#include <cmath>
#include <cstdint>
#include <limits>
#include <stdexcept>
#include <vector>
namespace plane_comparison {
using std::fabs;using std::sqrt;
constexpr double ON_DBL_MIN=std::numeric_limits<double>::min(),ON_SQRT_EPSILON=1.490116119385000000e-8,ON_ZERO_TOLERANCE=2.3283064365386962890625e-10;
#define ON_IS_FINITE(x) std::isfinite(x)
inline bool ON_IsValid(double x){return std::isfinite(x);}
inline bool ON_PointsAreCoincident(int,int,const double*,const double*){throw std::logic_error("Default tolerance is outside plane comparison inputs");}
double ON_Length3d(double,double,double);double ON_Length2d(double,double);
struct ON_3dPoint {double x=0,y=0,z=0;double DistanceTo(const ON_3dPoint& p)const{return ON_Length3d(x-p.x,y-p.y,z-p.z);}};
struct ON_3dVector {
 double x=0,y=0,z=0;
 static const ON_3dVector ZeroVector,XAxis,YAxis,ZAxis;
 double Length()const;bool Unitize();bool IsZero()const{return x==0&&y==0&&z==0;}
 bool PerpendicularTo(const ON_3dVector&);bool PerpendicularTo(const ON_3dPoint&,const ON_3dPoint&,const ON_3dPoint&);
};
const ON_3dVector ON_3dVector::ZeroVector{0,0,0},ON_3dVector::XAxis{1,0,0},ON_3dVector::YAxis{0,1,0},ON_3dVector::ZAxis{0,0,1};
ON_3dVector operator-(ON_3dPoint a,ON_3dPoint b){return {a.x-b.x,a.y-b.y,a.z-b.z};}
ON_3dVector operator-(ON_3dVector a,ON_3dVector b){return {a.x-b.x,a.y-b.y,a.z-b.z};}
ON_3dVector operator+(ON_3dVector a,ON_3dVector b){return {a.x+b.x,a.y+b.y,a.z+b.z};}
ON_3dVector operator*(double a,ON_3dVector b){return {a*b.x,a*b.y,a*b.z};}
double operator*(ON_3dVector a,ON_3dVector b){return a.x*b.x+a.y*b.y+a.z*b.z;}
double ON_DotProduct(ON_3dVector a,ON_3dVector b){return a*b;}
ON_3dVector ON_CrossProduct(ON_3dVector a,ON_3dVector b){return {a.y*b.z-a.z*b.y,a.z*b.x-a.x*b.z,a.x*b.y-a.y*b.x};}
struct ON_2dVector {double x=0,y=0;ON_2dVector()=default;ON_2dVector(double x,double y):x(x),y(y){}double Length()const;bool Unitize();};
struct Equation {bool Create(ON_3dPoint p,ON_3dVector n){return std::isfinite(p.x)&&std::isfinite(p.y)&&std::isfinite(p.z)&&n.Length()>0;}};
struct ON_Plane {
 ON_3dPoint origin;ON_3dVector xaxis{1,0,0},yaxis{0,1,0},zaxis{0,0,1};Equation plane_equation;
 bool CreateFromPoints(const ON_3dPoint&,const ON_3dPoint&,const ON_3dPoint&);bool CreateFromFrame(const ON_3dPoint&,const ON_3dVector&,const ON_3dVector&);
 bool UpdateEquation(){return plane_equation.Create(origin,zaxis);}
 bool IsValid()const{return std::isfinite(origin.x)&&std::isfinite(origin.y)&&std::isfinite(origin.z)&&std::fabs(xaxis.Length()-1)<1e-10&&std::fabs(yaxis.Length()-1)<1e-10&&std::fabs(zaxis.Length()-1)<1e-10;}
 double DistanceTo(ON_3dPoint p)const{return (p-origin)*zaxis;}
};
struct ON_Line {ON_3dPoint from,to;ON_Line()=default;ON_Line(ON_3dPoint a,ON_3dPoint b):from(a),to(b){}ON_3dPoint PointAt(double t)const{double s=1-t;return {s*from.x+t*to.x,s*from.y+t*to.y,s*from.z+t*to.z};}bool InPlane(ON_Plane&,double)const;};
struct ON_Curve {bool IsPlanar(ON_Plane*,double)const{throw std::logic_error("2D base plane outside comparison inputs");}};
struct ON_NurbsCurve:ON_Curve {
 int m_dim=3,m_cv_count=0;std::vector<ON_3dPoint> points;std::vector<double> knots;int degree=0;ON_3dPoint start,end;ON_3dVector tangent;
 bool IsLinear(double)const;bool IsPlanar(ON_Plane*,double)const;
 bool IsClamped()const{for(int i=1;i<=degree;i++)if(knots[i]!=knots[0]||knots[knots.size()-1-i]!=knots.back())return false;return true;}
 bool GetCV(int i,ON_3dPoint& p)const{if(i<0||i>=m_cv_count)return false;p=points[i];return true;}
 std::array<double,2> Domain()const{return {knots[degree],knots[m_cv_count]};}
 bool EvTangent(double,ON_3dPoint& p,ON_3dVector& v)const{p=start;v=tangent;return true;}
 ON_3dPoint PointAtEnd()const{return end;}
 bool IsInPlane(const ON_Plane& p,double tolerance)const{for(auto v:points)if(std::fabs(p.DistanceTo(v))>tolerance)return false;return true;}
};
#include "plane-methods.inc"
}
extern "C" int32_t jd_test_nurbs_plane(const double* coordinates,uint32_t coordinate_count,uint32_t degree,const double* knots,uint32_t knot_count,const double* ends,uint32_t end_count,double* output,uint32_t output_count){
 using namespace plane_comparison;
 if(!coordinates||!knots||!ends||!output||coordinate_count%3||coordinate_count/3>65536||degree==0||degree>64||coordinate_count/3<=degree||knot_count!=coordinate_count/3+degree+1||end_count!=9||output_count!=12)return 2;
 for(uint32_t i=0;i<coordinate_count;i++)if(!std::isfinite(coordinates[i]))return 2;
 for(uint32_t i=0;i<knot_count;i++)if(!std::isfinite(knots[i])||(i&&knots[i]<knots[i-1]))return 2;
 for(uint32_t i=0;i<9;i++)if(!std::isfinite(ends[i]))return 2;
 try{ON_NurbsCurve curve;curve.degree=static_cast<int>(degree);curve.m_cv_count=static_cast<int>(coordinate_count/3);curve.knots.assign(knots,knots+knot_count);for(uint32_t i=0;i<coordinate_count;i+=3)curve.points.push_back({coordinates[i],coordinates[i+1],coordinates[i+2]});curve.start={ends[0],ends[1],ends[2]};curve.end={ends[3],ends[4],ends[5]};curve.tangent={ends[6],ends[7],ends[8]};ON_Plane plane;if(!curve.IsPlanar(&plane,1e-12))return 1;const double fields[12]={plane.origin.x,plane.origin.y,plane.origin.z,plane.xaxis.x,plane.xaxis.y,plane.xaxis.z,plane.yaxis.x,plane.yaxis.y,plane.yaxis.z,plane.zaxis.x,plane.zaxis.y,plane.zaxis.z};std::copy(fields,fields+12,output);return 0;}catch(...){return 3;}
}

void run_plane_reference_contracts(){
 const double points[9]={0,0,0,1,0,0,1,1,0},knots[6]={0,0,0,1,1,1},ends[9]={0,0,0,1,1,0,1,0,0};double output[12];std::fill(output,output+12,987);
 if(jd_test_nurbs_plane(points,9,2,knots,6,ends,9,output,11)!=2||output[0]!=987)throw std::runtime_error("Plane comparison short-buffer guard failed");
 if(jd_test_nurbs_plane(nullptr,9,2,knots,6,ends,9,output,12)!=2||output[0]!=987)throw std::runtime_error("Plane comparison null-input guard failed");
 if(jd_test_nurbs_plane(points,9,2,knots,6,ends,8,output,12)!=2||output[0]!=987)throw std::runtime_error("Plane comparison endpoint-count guard failed");
 double invalid[9];std::copy(ends,ends+9,invalid);invalid[0]=std::numeric_limits<double>::quiet_NaN();if(jd_test_nurbs_plane(points,9,2,knots,6,invalid,9,output,12)!=2||output[0]!=987)throw std::runtime_error("Plane comparison nonfinite guard failed");
 if(jd_test_nurbs_plane(points,9,2,knots,6,ends,9,output,12)!=0||output[0]!=0||output[3]!=1||output[11]!=1)throw std::runtime_error("Published plane triangle smoke failed");
}
