// Test-only observation containers for exact pinned published camera matrix methods.
#include <cmath>
#include <cstdlib>
#include <iostream>
#include <iomanip>
#include <sstream>
#include <string>
#include <stdexcept>
struct ON_3dPoint{double x,y,z;};struct ON_3dVector{double x,y,z;};
struct ON_Xform{
 double m_xform[4][4];
 void WorldToCamera(const ON_3dPoint&,const ON_3dVector&,const ON_3dVector&,const ON_3dVector&);
 void CameraToWorld(const ON_3dPoint&,const ON_3dVector&,const ON_3dVector&,const ON_3dVector&);
 bool CameraToClip(bool,double,double,double,double,double,double);
};
#include "camera-matrix-methods.inc"
static double number(std::istringstream& in){std::string s;if(!(in>>s))throw std::runtime_error("Missing matrix token");char* end=nullptr;double x=std::strtod(s.c_str(),&end);if(!end||*end)throw std::runtime_error("Invalid matrix token");return x;}
static std::string token(double x){if(std::isnan(x))return "nan";if(std::isinf(x))return x<0?"-inf":"inf";std::ostringstream s;s<<std::setprecision(17)<<x;return s.str();}
int main(){try{std::string line;while(std::getline(std::cin,line)){if(line.empty())continue;std::istringstream in(line);int id,op,projection;if(!(in>>id>>op>>projection))throw std::runtime_error("Invalid matrix header");ON_Xform m{};for(int i=0;i<4;i++)for(int j=0;j<4;j++)m.m_xform[i][j]=1+i*4+j;bool ok=true;
 if(op==0){double v[6];for(double& x:v)x=number(in);ok=m.CameraToClip(projection!=0,v[0],v[1],v[2],v[3],v[4],v[5]);}
 else{double v[12];for(double& x:v)x=number(in);ON_3dPoint p{v[0],v[1],v[2]};ON_3dVector x{v[3],v[4],v[5]},y{v[6],v[7],v[8]},z{v[9],v[10],v[11]};if(op==1)m.WorldToCamera(p,x,y,z);else if(op==2)m.CameraToWorld(p,x,y,z);else throw std::runtime_error("Unknown matrix operation");}
 std::cout<<id<<'\t'<<ok;for(const auto& row:m.m_xform)for(double x:row)std::cout<<'\t'<<token(x);std::cout<<'\n';}}
 catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
