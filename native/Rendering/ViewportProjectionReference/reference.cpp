// Observation-only state around exact pinned published viewport transition methods.
#include <cmath>
#include <cfloat>
#include <cstdint>
#include <cstdlib>
#include <iostream>
#include <iomanip>
#include <initializer_list>
#include <sstream>
#include <string>
#include <stdexcept>
namespace ON {
#include "projection-enum.inc"
}
static int diagnostic_count=0;
#define ON_ERROR(message) (++diagnostic_count)
struct ON_SHA1_Hash{static constexpr int ZeroDigest=0;};
struct ObservedCache{int writes=0;ObservedCache& operator=(int){++writes;return *this;}};
struct ON_3dVector{
 double x=0,y=0,z=0;ON_3dVector()=default;ON_3dVector(double a,double b,double c):x(a),y(b),z(c){}
 static const ON_3dVector UnsetVector;
 bool IsValid()const;bool Unitize();double Length()const;double MaximumCoordinate()const;
 ON_3dVector operator-()const;double operator*(const ON_3dVector&)const;
 ON_3dVector operator+(const ON_3dVector&)const;ON_3dVector operator-(const ON_3dVector&)const;bool operator==(const ON_3dVector&)const;
};
struct ON_3dPoint{
 double x=0,y=0,z=0;ON_3dPoint()=default;ON_3dPoint(double a,double b,double c):x(a),y(b),z(c){}
 static const ON_3dPoint UnsetPoint;bool IsValid()const;double MaximumCoordinate()const;bool operator==(const ON_3dPoint&)const;
 ON_3dPoint operator+(const ON_3dVector&)const;ON_3dVector operator-(const ON_3dPoint&)const;ON_3dPoint operator-(const ON_3dVector&)const;double DistanceTo(const ON_3dPoint&)const;
};
struct ON_Viewport{
 ON_3dPoint m_CamLoc,m_target_point;ON_3dVector m_CamDir,m_CamUp,m_CamX,m_CamY,m_CamZ;
 bool m_bLockCamLoc=false,m_bLockCamDir=false,m_bLockCamUp=false,m_bValidCamera=false,m_bValidCameraFrame=false,m_bValidFrustum=false;
 double m_frus_left,m_frus_right,m_frus_bottom,m_frus_top,m_frus_near,m_frus_far;
 unsigned char m_frustum_symmetry_flags=0;ON::view_projection m_projection=ON::unknown_view;
 ObservedCache m_projection_content_sha1;
 static bool IsValidCameraLocation(ON_3dPoint);static bool IsValidCameraUpOrDirection(ON_3dVector);
 bool SetCameraLocation(const ON_3dPoint&);bool SetCameraDirection(const ON_3dVector&);bool SetCameraUp(const ON_3dVector&);bool SetCameraFrame();
 bool IsValidCamera()const;bool IsValidFrustum()const;ON_3dPoint CameraLocation()const;ON_3dVector CameraZ()const;ON_3dPoint TargetPoint()const;
 double FrustumWidth()const;double FrustumHeight()const;bool CameraUpIsLocked()const;bool FrustumIsLeftRightSymmetric()const;bool FrustumIsTopBottomSymmetric()const;
 bool IsPerspectiveProjection()const;bool IsTwoPointPerspectiveProjection()const;bool GetFrustum(double*,double*,double*,double*,double* =nullptr,double* =nullptr)const;
 bool SetFrustum(double,double,double,double,double,double);bool SetCamera35mmLensLength(double);
 bool ChangeToParallelProjection(bool);bool ChangeToSymmetricFrustum(bool,bool,double);double TargetDistance(bool)const;bool SetProjection(ON::view_projection);bool SetTargetPoint(ON_3dPoint);
 void SetCameraLocationLock(bool);void SetCameraDirectionLock(bool);void SetCameraUpLock(bool);void SetFrustumLeftRightSymmetry(bool);void SetFrustumTopBottomSymmetry(bool);
};
#include "viewport-projection-methods.inc"
static double number(std::istringstream& in){std::string s;if(!(in>>s))throw std::runtime_error("Missing projection token");char* end=nullptr;double x=std::strtod(s.c_str(),&end);if(!end||*end)throw std::runtime_error("Invalid projection token");return x;}
static std::string token(double x){if(std::isnan(x))return "nan";if(std::isinf(x))return x<0?"-inf":"inf";std::ostringstream out;out<<std::setprecision(17)<<x;return out.str();}
int main(){try{
 std::uint16_t endian=1;if(sizeof(double)!=8||sizeof(unsigned short)!=2||*reinterpret_cast<unsigned char*>(&endian)!=1)throw std::runtime_error("Unsupported source macro representation");
 std::string line;while(std::getline(std::cin,line)){
  if(line.empty())continue;std::istringstream in(line);int id,flags,sym,count;std::uint32_t projection;
  if(!(in>>id>>flags>>projection>>sym)||sym<0||sym>255)throw std::runtime_error("Invalid viewport header");double a[27];for(double& x:a)x=number(in);
  ON_Viewport v;v.m_CamLoc={a[0],a[1],a[2]};v.m_CamDir={a[3],a[4],a[5]};v.m_CamUp={a[6],a[7],a[8]};v.m_CamX={a[9],a[10],a[11]};v.m_CamY={a[12],a[13],a[14]};v.m_CamZ={a[15],a[16],a[17]};
  double* bounds[]={&v.m_frus_left,&v.m_frus_right,&v.m_frus_bottom,&v.m_frus_top,&v.m_frus_near,&v.m_frus_far};for(int i=0;i<6;i++)*bounds[i]=a[18+i];v.m_target_point={a[24],a[25],a[26]};
  v.m_bValidCamera=(flags&1)!=0;v.m_bValidCameraFrame=(flags&2)!=0;v.m_bLockCamLoc=(flags&4)!=0;v.m_bLockCamDir=(flags&8)!=0;v.m_bLockCamUp=(flags&16)!=0;v.m_bValidFrustum=(flags&32)!=0;v.m_projection=static_cast<ON::view_projection>(projection);v.m_frustum_symmetry_flags=static_cast<unsigned char>(sym);
  if(!(in>>count)||count<1||count>32)throw std::runtime_error("Invalid projection step count");diagnostic_count=0;
  for(int step=0;step<count;++step){int op;if(!(in>>op))throw std::runtime_error("Missing projection operation");double r[6];for(double& x:r)x=number(in);bool ok=true;double result=0;
   switch(op){
    case 0:ok=v.ChangeToParallelProjection(r[0]!=0);break;
    case 1:ok=v.ChangeToSymmetricFrustum(r[0]!=0,r[1]!=0,r[2]);break;
    case 2:result=v.TargetDistance(r[0]!=0);break;
    case 3:ok=v.SetProjection(static_cast<ON::view_projection>(static_cast<std::uint32_t>(r[0])));break;
    case 4:ok=v.SetTargetPoint({r[0],r[1],r[2]});break;
    case 5:ok=v.SetCameraLocation({r[0],r[1],r[2]});break;
    case 6:ok=v.SetCameraDirection({r[0],r[1],r[2]});break;
    case 7:ok=v.SetCameraUp({r[0],r[1],r[2]});break;
    case 8:ok=v.SetCamera35mmLensLength(r[0]);break;
    case 9:ok=v.SetFrustum(r[0],r[1],r[2],r[3],r[4],r[5]);break;
    case 10:v.SetCameraLocationLock(r[0]!=0);v.SetCameraDirectionLock(r[1]!=0);v.SetCameraUpLock(r[2]!=0);break;
    case 11:v.SetFrustumLeftRightSymmetry(r[0]!=0);v.SetFrustumTopBottomSymmetry(r[1]!=0);break;
    case 12:ok=v.SetCameraFrame();break;
    default:throw std::runtime_error("Unknown viewport operation");
   }
   int observed=(v.m_bValidCamera?1:0)|(v.m_bValidCameraFrame?2:0)|(v.m_bLockCamLoc?4:0)|(v.m_bLockCamDir?8:0)|(v.m_bLockCamUp?16:0)|(v.m_bValidFrustum?32:0);
   std::cout<<id<<'\t'<<step<<'\t'<<ok<<'\t'<<token(result)<<'\t'<<observed<<'\t'<<static_cast<std::uint32_t>(v.m_projection)<<'\t'<<static_cast<int>(v.m_frustum_symmetry_flags)<<'\t'<<v.m_projection_content_sha1.writes<<'\t'<<diagnostic_count;
   for(double value:{v.m_CamLoc.x,v.m_CamLoc.y,v.m_CamLoc.z,v.m_CamDir.x,v.m_CamDir.y,v.m_CamDir.z,v.m_CamUp.x,v.m_CamUp.y,v.m_CamUp.z,v.m_CamX.x,v.m_CamX.y,v.m_CamX.z,v.m_CamY.x,v.m_CamY.y,v.m_CamY.z,v.m_CamZ.x,v.m_CamZ.y,v.m_CamZ.z,v.m_frus_left,v.m_frus_right,v.m_frus_bottom,v.m_frus_top,v.m_frus_near,v.m_frus_far,v.m_target_point.x,v.m_target_point.y,v.m_target_point.z})std::cout<<'\t'<<token(value);std::cout<<'\n';
  }
  std::string extra;if(in>>extra)throw std::runtime_error("Extra viewport input");
 }
}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
