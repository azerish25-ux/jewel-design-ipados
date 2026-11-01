// Observation-only containers around unchanged published viewport and matrix methods.
#include <cmath>
#include <cstring>
#include <iostream>
#include <sstream>
#include <iomanip>
#include <string>
#include <cstdlib>
#include <stdexcept>
namespace ON{enum coordinate_system{world_cs=0,camera_cs=1,clip_cs=2,screen_cs=3};enum view_projection{parallel_view=1,perspective_view=2};}
struct ON_3dPoint{double x,y,z;};struct ON_3dVector{double x,y,z;};
struct ON_Xform{
 double m_xform[4][4];double* operator[](int i){return m_xform[i];}const double* operator[](int i)const{return m_xform[i];}ON_Xform();explicit ON_Xform(const double m[4][4]){std::memcpy(m_xform,m,sizeof(m_xform));}static const ON_Xform IdentityTransformation;
 ON_Xform operator*(const ON_Xform&)const;bool IsIdentity(double)const;
 void WorldToCamera(const ON_3dPoint&,const ON_3dVector&,const ON_3dVector&,const ON_3dVector&);void CameraToWorld(const ON_3dPoint&,const ON_3dVector&,const ON_3dVector&,const ON_3dVector&);
 bool CameraToClip(bool,double,double,double,double,double,double);bool ClipToCamera(bool,double,double,double,double,double,double);
 bool ClipToScreen(double,double,double,double,double,double);bool ScreenToClip(double,double,double,double,double,double);
};
const ON_Xform ON_Xform::IdentityTransformation=[](){ON_Xform m;for(int i=0;i<4;i++)m.m_xform[i][i]=1;return m;}();
struct ON_Viewport{
 bool m_bValidCamera,m_bValidFrustum,m_bValidPort=false;unsigned m_projection;
 ON_3dPoint m_CamLoc;ON_3dVector m_CamX,m_CamY,m_CamZ;
 double m_frus_left,m_frus_right,m_frus_bottom,m_frus_top,m_frus_near,m_frus_far;
 int m_port_left=0,m_port_right=0,m_port_bottom=0,m_port_top=0,m_port_near=0,m_port_far=0;
 ON_Xform m_clip_mods,m_clip_mods_inverse;
 bool IsPerspectiveProjection()const;bool GetXform(ON::coordinate_system,ON::coordinate_system,ON_Xform&)const;
};
#include "viewport-cache-methods.inc"
static double number(std::istringstream& in){std::string s;if(!(in>>s))throw std::runtime_error("Missing cache token");char* end;double n=std::strtod(s.c_str(),&end);if(*end)throw std::runtime_error("Invalid cache token");return n;}
static std::string token(double n){if(std::isnan(n))return "nan";if(std::isinf(n))return n<0?"-inf":"inf";std::ostringstream s;s<<std::setprecision(17)<<n;return s.str();}
int main(){try{std::string line;while(std::getline(std::cin,line)){std::istringstream in(line);int flags;unsigned projection;if(!(in>>flags>>projection))throw std::runtime_error("Invalid cache header");ON_Viewport v;v.m_bValidCamera=(flags&1)!=0;v.m_bValidFrustum=(flags&2)!=0;v.m_projection=projection;v.m_CamLoc={number(in),number(in),number(in)};v.m_CamX={number(in),number(in),number(in)};v.m_CamY={number(in),number(in),number(in)};v.m_CamZ={number(in),number(in),number(in)};
 v.m_frus_left=number(in);v.m_frus_right=number(in);v.m_frus_bottom=number(in);v.m_frus_top=number(in);v.m_frus_near=number(in);v.m_frus_far=number(in);for(auto& row:v.m_clip_mods.m_xform)for(double& x:row)x=number(in);ON_Xform extra;for(auto& row:extra.m_xform)for(double& x:row)x=number(in);std::string rest;if(in>>rest)throw std::runtime_error("Extra cache input");ON_Xform m;bool ok=v.GetXform(ON::world_cs,ON::clip_cs,m),identity=extra.IsIdentity(0);if(ok&&!identity)m=m*extra;std::cout<<ok<<'\t'<<identity;for(auto& row:m.m_xform)for(double x:row)std::cout<<'\t'<<token(x);std::cout<<'\n';}}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
