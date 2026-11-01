// Observation-only containers around unchanged pinned published frame arithmetic.
#include <cmath>
#include <cfloat>
#include <cstdint>
#include <cstdlib>
#include <iostream>
#include <initializer_list>
#include <iomanip>
#include <sstream>
#include <string>
#include <stdexcept>
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
};
struct ON_Viewport{
 ON_3dPoint m_CamLoc;ON_3dVector m_CamDir,m_CamUp,m_CamX,m_CamY,m_CamZ;
 bool m_bLockCamLoc=false,m_bLockCamDir=false,m_bLockCamUp=false,m_bValidCamera=false,m_bValidCameraFrame=false;
 ObservedCache m_projection_content_sha1;
 static bool IsValidCameraLocation(ON_3dPoint);static bool IsValidCameraUpOrDirection(ON_3dVector);
 bool SetCameraLocation(const ON_3dPoint&);bool SetCameraDirection(const ON_3dVector&);bool SetCameraUp(const ON_3dVector&);bool SetCameraFrame();
};
#include "camera-frame-methods.inc"
static double number(std::istringstream& in){std::string s;if(!(in>>s))throw std::runtime_error("Missing frame token");char* end=nullptr;double x=std::strtod(s.c_str(),&end);if(!end||*end)throw std::runtime_error("Invalid frame token");return x;}
static std::string token(double x){if(std::isnan(x))return "nan";if(std::isinf(x))return x<0?"-inf":"inf";std::ostringstream out;out<<std::setprecision(17)<<x;return out.str();}
int main(){try{std::uint16_t endian=1;if(sizeof(double)!=8||sizeof(unsigned short)!=2||*reinterpret_cast<unsigned char*>(&endian)!=1)throw std::runtime_error("Unsupported source macro representation");std::string line;while(std::getline(std::cin,line)){if(line.empty())continue;std::istringstream in(line);int id,flags,count;if(!(in>>id>>flags))throw std::runtime_error("Invalid frame header");double a[18];for(double& x:a)x=number(in);ON_Viewport v;v.m_CamLoc={a[0],a[1],a[2]};v.m_CamDir={a[3],a[4],a[5]};v.m_CamUp={a[6],a[7],a[8]};v.m_CamX={a[9],a[10],a[11]};v.m_CamY={a[12],a[13],a[14]};v.m_CamZ={a[15],a[16],a[17]};v.m_bValidCamera=(flags&1)!=0;v.m_bValidCameraFrame=(flags&2)!=0;v.m_bLockCamLoc=(flags&4)!=0;v.m_bLockCamDir=(flags&8)!=0;v.m_bLockCamUp=(flags&16)!=0;if(!(in>>count)||count<1||count>16)throw std::runtime_error("Invalid frame step count");diagnostic_count=0;
 for(int step=0;step<count;++step){int op;if(!(in>>op))throw std::runtime_error("Missing frame operation");double x=number(in),y=number(in),z=number(in);bool ok;if(op==0)ok=v.SetCameraFrame();else if(op==1)ok=v.SetCameraLocation({x,y,z});else if(op==2)ok=v.SetCameraDirection({x,y,z});else if(op==3)ok=v.SetCameraUp({x,y,z});else throw std::runtime_error("Unknown frame operation");
 std::cout<<id<<'\t'<<step<<'\t'<<ok<<'\t'<<v.m_bValidCamera<<'\t'<<v.m_bValidCameraFrame<<'\t'<<v.m_projection_content_sha1.writes<<'\t'<<diagnostic_count;for(double value:{v.m_CamLoc.x,v.m_CamLoc.y,v.m_CamLoc.z,v.m_CamDir.x,v.m_CamDir.y,v.m_CamDir.z,v.m_CamUp.x,v.m_CamUp.y,v.m_CamUp.z,v.m_CamX.x,v.m_CamX.y,v.m_CamX.z,v.m_CamY.x,v.m_CamY.y,v.m_CamY.z,v.m_CamZ.x,v.m_CamZ.y,v.m_CamZ.z})std::cout<<'\t'<<token(value);std::cout<<'\n';}
 std::string extra;if(in>>extra)throw std::runtime_error("Extra frame input");}}
 catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
