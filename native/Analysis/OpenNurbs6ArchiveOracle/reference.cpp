// Full pinned published library oracle; no original Rhino DLL execution or archive stubs.
#include "opennurbs.h"
#include <iostream>
#include <iomanip>
#include <sstream>
#include <string>
#include <vector>
#include <stdexcept>
static std::vector<unsigned char> unhex(const std::string& s){if(s=="-")return {};if(s.size()%2)throw std::runtime_error("Odd hex");std::vector<unsigned char> b;for(size_t i=0;i<s.size();i+=2){auto part=s.substr(i,2);size_t n;unsigned x=std::stoul(part,&n,16);if(n!=2||x>255)throw std::runtime_error("Bad hex");b.push_back(static_cast<unsigned char>(x));}return b;}
static void hex(const void* data,size_t n){if(n==0){std::cout<<'-';return;}auto p=static_cast<const unsigned char*>(data);for(size_t i=0;i<n;i++)std::cout<<std::hex<<std::setw(2)<<std::setfill('0')<<static_cast<unsigned>(p[i]);std::cout<<std::dec;}
static ON_wString text(const std::string& s){auto b=unhex(s);return ON_wString(reinterpret_cast<const char*>(b.data()),static_cast<int>(b.size()));}
static void utf16(const ON_wString& s){std::vector<unsigned char> bytes;auto put=[&](unsigned c){bytes.push_back(c&255);bytes.push_back(c>>8);};for(int i=0;i<s.Length();i++){unsigned c=static_cast<unsigned>(s.Array()[i]);if(sizeof(wchar_t)==2||c<=0xffff)put(c);else if(c<=0x10ffff){c-=0x10000;put(0xd800+(c>>10));put(0xdc00+(c&1023));}else put(0xfffd);}hex(bytes.data(),bytes.size());}
int main(){try{ON::Begin();std::string line;while(std::getline(std::cin,line)){if(line.empty())continue;std::istringstream in(line);char op;int version,count;in>>op>>version;ON_UserStringList strings;
 if(op=='W'){in>>count;if(count<0||count>10000)throw std::runtime_error("Count bound");for(int i=0;i<count;i++){std::string k,v;if(!(in>>k>>v))throw std::runtime_error("Missing pair");ON_UserString pair;pair.m_key=text(k);pair.m_string_value=text(v);strings.m_e.Append(pair);}ON_Write3dmBufferArchive archive(0,64*1024*1024,version,201508103);if(!strings.Write(archive))throw std::runtime_error("Native write failed");std::cout<<"W ";hex(archive.Buffer(),archive.SizeOfArchive());std::cout<<'\n';}
 else if(op=='R'){std::string payload;if(!(in>>payload>>count)||count<0||count>10000)throw std::runtime_error("Read header");for(int i=0;i<count;i++){std::string k,v;if(!(in>>k>>v))throw std::runtime_error("Initial pair");ON_UserString pair;pair.m_key=text(k);pair.m_string_value=text(v);strings.m_e.Append(pair);}auto bytes=unhex(payload);ON_Read3dmBufferArchive archive(bytes.size(),bytes.data(),false,version,201508103);bool ok=strings.Read(archive);std::cout<<"R "<<ok<<' '<<archive.BadCRCCount()<<' '<<archive.CurrentPosition()<<' '<<strings.m_e.Count();for(int i=0;i<strings.m_e.Count();i++){std::cout<<' ';utf16(strings.m_e[i].m_key);std::cout<<' ';utf16(strings.m_e[i].m_string_value);}std::cout<<'\n';}
 else throw std::runtime_error("Unknown operation");std::string extra;if(in>>extra)throw std::runtime_error("Extra input");}ON::End();return 0;}catch(const std::exception& e){std::cerr<<e.what()<<'\n';return 1;}}
