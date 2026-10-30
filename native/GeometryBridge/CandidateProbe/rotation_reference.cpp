// Test-only observation shim for pinned published Rotation bodies.
// This is not an original Rhino5 binary or a production CAD operation.
#include <cmath>
#include <cstdint>
#include <limits>
#include <stdexcept>
using std::sin;using std::cos;using std::fabs;
namespace rotation_comparison {
constexpr double ON_EPSILON=2.2204460492503131e-16;
constexpr double ON_SQRT_EPSILON=1.490116119385000000e-8;
#define ON_ERROR(message) ((void)0)
struct ON_3dPoint{double x,y,z;};
struct ON_3dVector{
 double x,y,z;
 double LengthSquared()const{return x*x+y*y+z*z;}
 bool Unitize(){const double length=std::sqrt(LengthSquared());if(!(length>0)||!std::isfinite(length))return false;x/=length;y/=length;z/=length;return true;}
};
struct ON_2dVector{
 double x,y;ON_2dVector(double x,double y):x(x),y(y){}
 bool Unitize(){const double length=std::hypot(x,y);if(!(length>0)||!std::isfinite(length))return false;x/=length;y/=length;return true;}
};
struct ON_Xform{
 double m_xform[4][4]{};
 ON_Xform(){for(int i=0;i<4;i++)m_xform[i][i]=1;}
 static const ON_Xform IdentityTransformation;
 void Rotation(double angle,ON_3dVector axis,ON_3dPoint center);
 void Rotation(double sin_angle,double cos_angle,ON_3dVector axis,ON_3dPoint center);
 ON_Xform operator*(const ON_Xform& other)const{
  ON_Xform result;
  for(int i=0;i<4;i++)for(int j=0;j<4;j++){result.m_xform[i][j]=0;for(int k=0;k<4;k++)result.m_xform[i][j]+=m_xform[i][k]*other.m_xform[k][j];}
  return result;
 }
};
const ON_Xform ON_Xform::IdentityTransformation{};
#include "rotation-methods.inc"
namespace {
bool valid(double* output,std::uint32_t count){return output&&count==12;}
void copy(const ON_Xform& x,double* output){for(int r=0;r<3;r++)for(int c=0;c<4;c++)output[4*r+c]=x.m_xform[r][c];}
ON_Xform axis(double angle,std::uint32_t index){ON_Xform x;ON_3dVector a{index==0?1.0:0.0,index==1?1.0:0.0,index==2?1.0:0.0};x.Rotation(angle,a,{0,0,0});return x;}
}
} // namespace rotation_comparison
using rotation_comparison::valid;using rotation_comparison::axis;using rotation_comparison::copy;
extern "C" std::int32_t jd_test_rotation_axis(double angle,std::uint32_t index,double* output,std::uint32_t count){
 if(!valid(output,count)||!std::isfinite(angle)||index>2)return 1;
 copy(axis(angle,index),output);return 0;
}
extern "C" std::int32_t jd_test_rotation_xyz(double x,double y,double z,double mirror,double* output,std::uint32_t count){
 if(!valid(output,count)||!std::isfinite(x)||!std::isfinite(y)||!std::isfinite(z))return 1;
 if(mirror>.5){x=0.0-x;z=0.0-z;}
 constexpr double radians=3.141592653589793238462643383279502884/180.0;
 copy(axis(radians*x,0)*axis(radians*y,1)*axis(radians*z,2),output);return 0;
}
void run_rotation_reference_contracts(){
 double fields[12];
 if(jd_test_rotation_axis(std::acos(-1.0)/2,0,fields,12)!=0||fields[0]!=1||fields[5]!=0||fields[6]!=-1||fields[9]!=1||fields[10]!=0)throw std::runtime_error("Published rotation cardinal reference failed");
 if(jd_test_rotation_xyz(0,0,0,0,fields,12)!=0||fields[0]!=1||fields[5]!=1||fields[10]!=1)throw std::runtime_error("Published rotation identity reference failed");
 fields[0]=123;
 if(jd_test_rotation_axis(0,3,fields,12)!=1||fields[0]!=123||jd_test_rotation_axis(std::numeric_limits<double>::infinity(),0,fields,12)!=1||fields[0]!=123||jd_test_rotation_axis(0,0,fields,11)!=1||fields[0]!=123||jd_test_rotation_axis(0,0,nullptr,12)!=1)throw std::runtime_error("Rotation reference argument guard failed");
}
