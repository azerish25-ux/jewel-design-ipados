// Observation containers only; arithmetic comes from hash-checked published source.
#include <cmath>
#include <cfloat>
#include <iostream>
#include <iomanip>
#include <string>
#include <sstream>
#include <cstdlib>
#include <stdexcept>
static int diagnostics=0;
#define ON_ERROR(message) (++diagnostics)
struct ON_2dVector{double x=0,y=0;ON_2dVector()=default;ON_2dVector(double a,double b):x(a),y(b){}double Length()const;bool Unitize();};
struct ON_3dVector{double x=0,y=0,z=0;ON_3dVector()=default;ON_3dVector(double a,double b,double c):x(a),y(b),z(c){}double Length()const;bool Unitize();double LengthSquared()const{return x*x+y*y+z*z;}};
struct ON_3dPoint{double x=0,y=0,z=0;};
struct ON_Xform{double m_xform[4][4]{};static const ON_Xform IdentityTransformation;void Rotation(double,ON_3dVector,ON_3dPoint);void Rotation(double,double,ON_3dVector,ON_3dPoint);};
const ON_Xform ON_Xform::IdentityTransformation=[](){ON_Xform r;for(int i=0;i<4;i++)r.m_xform[i][i]=1;return r;}();
#include "camera-rotation-methods.inc"
static double number(std::istringstream& s){std::string t;if(!(s>>t))throw std::runtime_error("Missing rotation token");char* end;double d=std::strtod(t.c_str(),&end);if(*end)throw std::runtime_error("Invalid rotation token");return d;}
static std::string token(double d){if(std::isnan(d))return "nan";if(std::isinf(d))return d<0?"-inf":"inf";std::ostringstream s;s<<std::setprecision(17)<<d;return s.str();}
int main(){try{std::string line;while(std::getline(std::cin,line)){std::istringstream in(line);int mode;if(!(in>>mode))throw std::runtime_error("Missing rotation mode");double s=number(in),c=number(in);ON_3dVector axis{number(in),number(in),number(in)},v{number(in),number(in),number(in)};std::string extra;if(in>>extra)throw std::runtime_error("Extra rotation token");diagnostics=0;ON_Xform r;if(mode==0)r.Rotation(s,c,axis,{});else if(mode==1)r.Rotation(s,axis,{});else throw std::runtime_error("Unknown rotation mode");std::cout<<diagnostics;for(int i=0;i<3;i++)std::cout<<'\t'<<token(v.y*r.m_xform[i][1]+v.x*r.m_xform[i][0]+v.z*r.m_xform[i][2]);std::cout<<'\n';}}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
