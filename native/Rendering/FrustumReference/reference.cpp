// Test-only state observations around exact pinned published numeric methods.
#include <cmath>
#include <cstdlib>
#include <cstdint>
#include <iostream>
#include <iomanip>
#include <sstream>
#include <string>
#include <stdexcept>
static int diagnostic_count=0;
#define ON_ERROR(message) (++diagnostic_count)
struct ON_SHA1_Hash { static constexpr int ZeroDigest=0; };
struct ON_Viewport {
 double m_frus_left,m_frus_right,m_frus_bottom,m_frus_top,m_frus_near,m_frus_far;
 bool m_bValidFrustum,perspective,up_locked,lr_symmetric,tb_symmetric;
 int m_projection_content_sha1=1;
 bool IsPerspectiveProjection()const{return perspective;}
 bool CameraUpIsLocked()const{return up_locked;}
 bool FrustumIsLeftRightSymmetric()const{return lr_symmetric;}
 bool FrustumIsTopBottomSymmetric()const{return tb_symmetric;}
 bool IsTwoPointPerspectiveProjection()const;
 bool SetFrustum(double,double,double,double,double,double);
 bool GetFrustum(double*,double*,double*,double*,double* = nullptr,double* = nullptr)const;
 bool GetFrustumAspect(double&)const;
 bool SetCameraAngle(double);
 bool GetCamera35mmLensLength(double*)const;
 bool SetCamera35mmLensLength(double);
};
#include "frustum-methods.inc"
static double number(std::istringstream& input){std::string s;if(!(input>>s))throw std::runtime_error("Missing numeric token");char* end=nullptr;double x=std::strtod(s.c_str(),&end);if(!end||*end)throw std::runtime_error("Invalid numeric token");return x;}
static std::string token(double x){if(std::isnan(x))return "nan";if(std::isinf(x))return x<0?"-inf":"inf";std::ostringstream out;out<<std::setprecision(17)<<x;return out.str();}
int main(){try{std::uint16_t endian=1;if(sizeof(double)!=8||sizeof(unsigned short)!=2||*reinterpret_cast<unsigned char*>(&endian)!=1)throw std::runtime_error("Unsupported reference representation");std::string line;while(std::getline(std::cin,line)){if(line.empty())continue;std::istringstream input(line);int id,op,flags;if(!(input>>id>>op>>flags))throw std::runtime_error("Invalid case header");ON_Viewport v{};v.m_bValidFrustum=(flags&1)!=0;v.perspective=(flags&2)!=0;v.up_locked=(flags&4)!=0;v.lr_symmetric=(flags&8)!=0;v.tb_symmetric=(flags&16)!=0;double* bounds[]={&v.m_frus_left,&v.m_frus_right,&v.m_frus_bottom,&v.m_frus_top,&v.m_frus_near,&v.m_frus_far};for(auto value:bounds)*value=number(input);diagnostic_count=0;bool ok=false;double result=0;
 if(op==0){double a[6];for(double& x:a)x=number(input);ok=v.SetFrustum(a[0],a[1],a[2],a[3],a[4],a[5]);}
 else if(op==1)ok=v.SetCamera35mmLensLength(number(input));
 else if(op==2)ok=v.SetCameraAngle(number(input));
 else if(op==3)ok=v.GetCamera35mmLensLength(&result);
 else if(op==4)ok=v.GetFrustumAspect(result);
 else throw std::runtime_error("Unknown operation");
 std::cout<<id<<'\t'<<ok<<'\t'<<token(result);for(auto value:bounds)std::cout<<'\t'<<token(*value);std::cout<<'\t'<<v.m_bValidFrustum<<'\t'<<(v.m_projection_content_sha1==0)<<'\t'<<diagnostic_count<<'\t'<<v.IsTwoPointPerspectiveProjection()<<'\n';
 }}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
